using System;
using System.Collections;
using UnityEngine;

public class IA : MonoBehaviour
{
    [SerializeField] private Transform[] checkpoints;   // Array di checkpoint che l'IA deve seguire
    [SerializeField] private float pauseAtCheckpoint = .5f; // Pausa ad ogni checkpoint
    [SerializeField] private float checkMinDist = .2f;  // Distanza minima prima di poter assicurare di aver raggiunto il checkpoint
    [SerializeField] private bool isMoveGrounded = true;

    private int index;  // Indice nell'array dei checkpoint da raggiungere
    private bool indexIncrease; // Indica se l'indice è crescente o decrescente
    protected bool isPatrolMoving;  // Se true passa il movimento

    private Vector2 movement;   // Vettore di movimento target

    protected CharacterMovement characterMovement;    // Reference allo script del movimento

    // Scipts references
    //private CharacterHealth characterHealth;

    //(GABRIELE)COMMENTATO PERCHE' ONDEATH ADESSO PRENDE RIFERIMENTO AD UN'ALTRA FUNZIONE NEL SUO START

    [Space]
    [Header("Attack settings")]
    [Tooltip("Distanza a cui il mostro vede il player")]
    [SerializeField] private float spotRange = 5f;
    [SerializeField] protected Collider2D attackCollider;
    protected Transform playerTransform;
    protected bool isPlayerInRange = false; // True quando il player entra in range d'attacco
    protected bool isComputing; // Se true permette di abilitare il ciclo Update ai figli ereditati

    private void Awake()
    {
        // Get references
        characterMovement = GetComponent<CharacterMovement>();


        //characterHealth = GetComponent<CharacterHealth>();

        // Setup listeners
        //characterHealth.onDeath += Death;

    }

    private void Start()
    {
        
        // Se ci sono checkpoint, inizia il movimento
        isPatrolMoving = checkpoints.Length > 0;

        // Segui i checkpoints dal primo indice
        index = 0;
        indexIncrease = true;

        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        if(playerTransform is null)
        {
            Debug.LogWarning("Monster " + name + " can't find the player!");
        }

        isPlayerInRange = false;
        isComputing = false;

        attackCollider.enabled = false;
    }

    private void Update()
    {
        if (PauseManager.IsGamePaused()) return;

        // Check player distance
        float dist = Vector2.Distance(playerTransform.position, transform.position);

        // Verifichiamo se il giocatore sia in range oppure no
        if (dist <= spotRange)
        {
            // Triggera l'evento solo se non era precedentemente in range
            if (!isPlayerInRange)
            {
                isPlayerInRange = true;
                PlayerEnteredRange();
            }
        }
        else if (isPlayerInRange)
        {
            isPlayerInRange = false;
            PlayerExitRange();
        }

        // Se la variabile isComputing è true, permettiamo alle classi ereditate di inserirsi su questo ciclo update
        if (isComputing)
            HierarchyUpdate();

        // Se è true, seguiamo il nostro percorso di checkpoints
        if (isPatrolMoving)
        {
            CheckpointsMovement();
            // E passalo al componente
            characterMovement.Move(movement);
        }
    }

    /// <summary>
    /// Update ereditato
    /// </summary>
    virtual protected void HierarchyUpdate() { }

    virtual protected void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, spotRange);
    }

    /// <summary>
    /// Descrive cosa succede quando il player entra in range
    /// </summary>
    virtual protected void PlayerEnteredRange()
    {
        isPatrolMoving = false;
    }

    /// <summary>
    /// Descrive cosa fare dopo che il player esce dal range
    /// </summary>
    virtual protected void PlayerExitRange()
    {
        isPatrolMoving = true;
    }

    /// <summary>
    /// Attendi prima di poter muovere nuovamente
    /// </summary>
    /// <returns></returns>
    private IEnumerator WaitToMove()
    {
        yield return new WaitForSeconds(pauseAtCheckpoint);
        isPatrolMoving = true;
    }

    /// <summary>
    /// Effettua i controlli sulla distanza ai checkpoint e aggiorna il vettore di movimento
    /// </summary>
    private void CheckpointsMovement()
    {
        // TODO: Check diff for different move types
        Vector2 diff = checkpoints[index].position - transform.position;

        bool condition = isMoveGrounded ? (Mathf.Abs(diff.x) <= checkMinDist) : (diff.magnitude <= checkMinDist);

        // Se sull'asse x sei abbastanza vicino al checkpoint
        if (condition)
        {
            // Aggiorna l'indice
            index = indexIncrease ? index + 1 : index - 1;
            
            if (index >= checkpoints.Length)
            {
                index -= 2;
                indexIncrease = false;
            } else if (index < 0)
            {
                index += 2;
                indexIncrease = true;
            }
            
            // Effettua la pausa
            isPatrolMoving = false;

            StartCoroutine(WaitToMove());
        }
        
        // Normalizza il movimento per non essere troppo elevato
        movement = diff.normalized;

        if (isMoveGrounded)
            movement.y = 0;
    }
}
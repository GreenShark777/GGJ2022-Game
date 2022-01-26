using System.Collections;
using UnityEngine;

public class Platform : MonoBehaviour
{
    [SerializeField] private Checkpoint[] checkpoints;   // Array di checkpoint che l'IA deve seguire

    [SerializeField] private float checkMinDist = .2f;  // Distanza minima prima di poter assicurare di aver raggiunto il checkpoint
    [SerializeField] private float moveSpeed = 2f;

    [SerializeField] private bool allowXMove = true;
    [SerializeField] private bool allowYMove = true;

    private int index;  // Indice nell'array dei checkpoint da raggiungere
    private int prevIndex; // Indice di appoggio per effettuare alcune operazioni
    private bool indexIncrease; // Indica se l'indice è crescente o decrescente
    private bool isMoving;  // Se true passa il movimento

    private Vector2 movement;   // Vettore di movimento target

    private void Start()
    {
        // Se ci sono checkpoint, inizia il movimento
        isMoving = checkpoints.Length > 0;

        // Parti dal primo indice
        index = 0;
        indexIncrease = true;
    }

    private void Update()
    {
        if (isMoving && !PauseManager.IsGamePaused())
        {
            // Aggiorna il vettore di movimento
            CheckMovement();
            // E passalo al componente
            Move();
        }
    }

    private void Move()
    {
        transform.Translate(movement * Time.deltaTime * moveSpeed);
    }

    /// <summary>
    /// Attendi prima di poter muovere nuovamente
    /// </summary>
    /// <returns></returns>
    private IEnumerator WaitToMove()
    {
        yield return new WaitForSeconds(checkpoints[prevIndex].pauseLength);
        isMoving = true;
    }

    /// <summary>
    /// Effettua i controlli sulla distanza ai checkpoint e aggiorna il vettore di movimento
    /// </summary>
    private void CheckMovement()
    {
        Vector2 diff = checkpoints[index].transform.position - transform.position;

        // Verifichiamo prima di aver raggiunto il checkpoint
        bool reachedX = allowXMove ? (Mathf.Abs(diff.x) <= checkMinDist) : true;
        bool reachedY = allowYMove ? (Mathf.Abs(diff.y) <= checkMinDist) : true;

        if (reachedX && reachedY)
        {
            // Aggiorna l'indice
            prevIndex = index;
            index = indexIncrease ? index + 1 : index - 1;

            if (index >= checkpoints.Length)
            {
                index -= 2;
                indexIncrease = false;
            }
            else if (index < 0)
            {
                index += 2;
                indexIncrease = true;
            }

            // Effettuare la pausa?
            if(checkpoints[prevIndex].action == CheckpointAction.Pause)
            {
                isMoving = false;

                StartCoroutine(WaitToMove());
            }

            movement = Vector2.zero;
        } 
        else
        {
            // Se non abbiamo ancora raggiunto il checkpoint calcoliamo il movimento

            if (!allowXMove) diff.x = 0;
            if (!allowYMove) diff.y = 0;

            movement = diff.normalized;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            collision.transform.parent = this.transform;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.parent = null;
        }
    }
}

[System.Serializable]
class Checkpoint
{
    public Transform transform;
    public CheckpointAction action;
    public float pauseLength;
}

enum CheckpointAction
{
    Pause,
    Continue
}
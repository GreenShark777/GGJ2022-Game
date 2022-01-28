//Si occupa del comportamento delle porte nelle stanze
using System.Collections;
using UnityEngine;

public class DoorsBehaviour : MonoBehaviour
{
    //riferimento al contenitore dei contatori di nemici
    private Transform countersContainer;
    //array di riferimenti agli script dei contatori di nemici
    private EnemyCountersBehaviour[] allCounters;
    //riferimento al collider di questa porta
    private Collider2D doorColl;
    //riferimento all'Animator del testo che indica che tutte le maschere sono state distrutte
    [SerializeField]
    private Animator adviseTextAnim = default;
    //riferimento alle metà della porta
    [SerializeField]
    private Transform leftDoor = default,
        rightDoor = default;

    //indica il numero di nemici presenti inizialmente nella stanza
    private int nEnemies;
    //indica se in questa stanza questa è la porta che controlla i nemici
    [SerializeField]
    private bool checkingDoor = false;
    //indica il numero di nemici a cui è stata tolta la maschera in questa stanza
    private int removedMasks = 0;
    //indica l'ID della stanza in cui si trova questa porta
    private int ownRoomID;
    //indica quanto velocemente si apre la porta
    [SerializeField]
    private float openingSpeed = 1;


    private void Awake()
    {
        //ottiene il riferimento al collider di questa porta
        doorColl = GetComponent<Collider2D>();
    }

    private void Start()
    {
        //se questa è la porta che deve controllare i nemici nella stanza...
        if (checkingDoor)
        {
            //...ottiene il riferimento al contenitore dei contatori di nemici...
            countersContainer = transform.GetChild(0);
            //...e riempe l'array di contatori di nemici prendendo dal contenitore
            allCounters = countersContainer.GetComponentsInChildren<EnemyCountersBehaviour>();

            //DEBUG-------------------------------------------------------------------------------------------------------------------------
            //se il numero di nemici non è uguale al numero di contatori, comunica l'errore
            if (nEnemies != allCounters.Length)
            { Debug.LogError("IL NUMERO DI NEMICI NON E' UGUALE AL NUMERO DI CONTATORI PER LA STANZA: " + transform.parent.name); }

        }

    }

    private void Update()
    {
        //DEBUG-----------------------------------------------------------------------------------------------------------------------------
        //if (Input.GetKeyDown(KeyCode.F1) && checkingDoor) { RemovedMaskOfAnEnemy(); }
        //if (Input.GetKeyDown(KeyCode.F3)) { StartCoroutine(OpenDoor()); }
        //DEBUG-----------------------------------------------------------------------------------------------------------------------------
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //cerca di prendere riferimento al manager delle collisioni dell'oggetto con cui si è colliso
        CollisionsManager cm = collision.GetComponent<CollisionsManager>();
        //se esiste il riferimento al manager delle collisioni, ed è il giocatore, lo porta alla stanza dopo quella in cui ci si trova
        if (cm && cm.IsPlayer()) { RoomsManager.instance.GoToNextRoom(ownRoomID); }
        
    }
    /// <summary>
    /// Permette di impostare il numero di nemici da controllare
    /// </summary>
    /// <param name="enemiesInRoom"></param>
    public void SetNumberOfEnemies(int enemiesInRoom) { nEnemies = enemiesInRoom; }
    /// <summary>
    /// Permette di impostare l'ID della stanza in cui si trova questa porta
    /// </summary>
    /// <param name="roomID"></param>
    public void SetOwnRoomID(int roomID) { ownRoomID = roomID; }
    /// <summary>
    /// Controlla se ci sono nemici presenti con maschere e chiude o apre la porta di conseguenza
    /// </summary>
    public void CheckForEnemiesWithMasks()
    {
        //se esistono dei nemici nella stanza con ancora la maschera...
        if ((nEnemies - removedMasks) > 0)
        {
            //...la porta si chiude(se non lo è già)
            if (doorColl.enabled) doorColl.enabled = false;

        } //altrimenti...
        else if (!doorColl.enabled)
        {
            //...la porta si apre...
            StartCoroutine(OpenDoor());
            //...ne attiva il collider...
            doorColl.enabled = true;
            //...e fa partire l'animazione del testo che indica al giocatore che può andare avanti
            adviseTextAnim.SetTrigger("Advise");
        
        }

    }
    /// <summary>
    /// Attiva un contatore di nemici e controlla se è stata completata la stanza
    /// </summary>
    public void RemovedMaskOfAnEnemy()
    {
        //se questa è la porta che deve controllare il completamento della stanza...
        if (checkingDoor)
        {
            //...incrementa il numero di maschere rimosse...
            removedMasks++;
            //...attiva il contatore corrispondente...
            allCounters[removedMasks - 1].ActivateThis();
            //...e controlla se ci sono ancora nemici con la maschera
            CheckForEnemiesWithMasks();

        }
        else { Debug.LogError("Chiamato controllo dalla porta sbagliata! Porta: " + name + " della stanza : " + transform.parent.name); }

    }
    /// <summary>
    /// Apre la porta visualmente
    /// </summary>
    /// <returns></returns>
    public IEnumerator OpenDoor()
    {
        //ottiene le grandezze delle metà della porta
        Vector3 leftDoorScale = leftDoor.localScale;
        Vector3 rightDoorScale = rightDoor.localScale;
        //diminuisce di poco la grandezza delle metà
        leftDoor.localScale = Vector2.Lerp(leftDoorScale, new Vector3(0, leftDoorScale.y), openingSpeed * Time.deltaTime);
        rightDoor.localScale = Vector2.Lerp(rightDoorScale, new Vector3(0, rightDoorScale.y), openingSpeed * Time.deltaTime);
        //aspetta il fixedUpdate
        yield return new WaitForFixedUpdate();
        //continua ad aprire la porta
        StartCoroutine(OpenDoor());

    }

}

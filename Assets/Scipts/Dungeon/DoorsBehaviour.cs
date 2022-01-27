//Si occupa del comportamento delle porte nelle stanze
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
    //indica il numero di nemici presenti inizialmente nella stanza
    private int nEnemies;
    //indica se in questa stanza questa è la porta che controlla i nemici
    [SerializeField]
    private bool checkingDoor = false;
    //indica il numero di nemici a cui è stata tolta la maschera in questa stanza
    private int removedMasks = 0;
    //indica l'ID della stanza in cui si trova questa porta
    private int ownRoomID;


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
        if (Input.GetKeyDown(KeyCode.F1) && checkingDoor) { RemovedMaskOfAnEnemy(); }
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

}

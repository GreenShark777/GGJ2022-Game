//Si occupa di mandare il giocatore nelle varie stanze
using System.Collections;
using UnityEngine;

public class RoomsManager : MonoBehaviour, IUpdateData
{
    //riferimento statico a questo manager delle stanze
    public static RoomsManager instance;
    //riferimento al manager delle musiche
    [SerializeField]
    private BgMusicManager bgMusicManager = default;
    //riferimento all'Animator dell'immagine di transizione
    [SerializeField]
    private Animator transitionAnim = default;
    //riferimento al giocatore
    [SerializeField]
    private Transform player = default;
    //riferimento al GameManag di scena
    [SerializeField]
    private GameManag g = default;
    //array di riferimenti di tutti gli script di comportamento delle stanze
    [SerializeField]
    private RoomsBehaviour[] allRooms = default;
    //indica la stanza in cui il giocatore è entrato
    private int enteredRoom = 0;


    private void Awake()
    {
        //se non esiste ancora un'istanza del manager delle stanza, questo manager diventa l'istanza
        if (!instance) { instance = this; }
        //imposta gli ID a tutte le stanze in lista
        for (int i = 0; i < allRooms.Length; i++) { allRooms[i].SetRoomID(i); }

    }

    private void Start()
    {
        //aggiorna la stanza in cui il giocatore è entrato al valore salvato
        enteredRoom = g.lastEnteredRoom;
        //comunica alla stanza in cui il giocatore è entrato per l'ultima volta che il giocatore è entrato
        allRooms[enteredRoom].PlayerEntered(player);
        //effettua il primo controllo sulla musica
        CheckMusic(true);

    }

    /// <summary>
    /// Porta il giocatore alla stanza immediatamente successiva a quella indicata dal parametro ricevuto
    /// </summary>
    /// <param name="currentRoom"></param>
    public void GoToNextRoom(int currentRoom)
    {
        //fa partire la coroutine di transizione
        StartCoroutine(TransitionToRoom(currentRoom + 1));

    }
    /// <summary>
    /// Si occupa della transizione da una stanza all'altra
    /// </summary>
    /// <param name="nextRoom"></param>
    /// <returns></returns>
    private IEnumerator TransitionToRoom(int nextRoom)
    {
        //aggiorna la stanza in cui il giocatore è entrato
        enteredRoom = nextRoom;
        //mette il gioco in stato di pausa
        PauseManager.SetPauseState(true);
        //fa partire l'animazione di transizione in fadeIn
        transitionAnim.SetBool("FadeIn", true);
        //aspetta del tempo(in secondi reali)
        yield return new WaitForSecondsRealtime(2f);
        //fa entrare il giocatore nella prossima stanza
        allRooms[nextRoom].PlayerEntered(player);
        //disattiva la stanza da cui è uscito
        allRooms[nextRoom - 1].gameObject.SetActive(false);
        //toglie il gioco dallo stato di pausa
        PauseManager.SetPauseState(false);
        //fa partire l'animazione di transizione in fadeOut
        transitionAnim.SetBool("FadeIn", false);
        //infine, salva i dati...
        g.SaveDataAfterUpdate();
        //...e controlla che la musica sia adatta per la stanza in cui ci si trova
        CheckMusic();

    }
    /// <summary>
    /// Controlla la musica in base alla stanza in cui ci si trova
    /// </summary>
    private void CheckMusic(bool firstCheck = false)
    {

        bool changeMusic = g.lastEnteredRoom == GameManag.SECOND_AREA_ROOM || (firstCheck && g.lastEnteredRoom >= GameManag.SECOND_AREA_ROOM);

        if (changeMusic) { bgMusicManager.ChangeMusic(false); /*Debug.Log("Cambiata musica");*/ }
        //Debug.Log("Effettuato controllo sulla musica");
    }

    //IUpdateData---------------------------------------------------------------------------------------------------------
    public void UpdateData()
    {
        //aggiorna l'ultima stanza in cui il giocatore è entrato
        g.lastEnteredRoom = enteredRoom;

    }

}

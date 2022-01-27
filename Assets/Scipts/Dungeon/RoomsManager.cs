//Si occupa di mandare il giocatore nelle varie stanze
using System.Collections;
using UnityEngine;

public class RoomsManager : MonoBehaviour
{
    //riferimento statico a questo manager delle stanze
    public static RoomsManager instance;
    //riferimento all'Animator dell'immagine di transizione
    [SerializeField]
    private Animator transitionAnim = default;
    //riferimento al giocatore
    [SerializeField]
    private Transform player = default;
    //array di riferimenti di tutti gli script di comportamento delle stanze
    [SerializeField]
    private RoomsBehaviour[] allRooms = default;


    private void Awake()
    {
        //se non esiste ancora un'istanza del manager delle stanza, questo manager diventa l'istanza
        if (!instance) { instance = this; }
        //imposta gli ID a tutte le stanze in lista
        for (int i = 0; i < allRooms.Length; i++) { allRooms[i].SetRoomID(i); }

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
        //mette il gioco in stato di pausa
        PauseManager.SetPauseState(true);
        //fa partire l'animazione di transizione in fadeIn
        transitionAnim.SetBool("FadeIn", true);
        //aspetta del tempo(in secondi reali)
        yield return new WaitForSecondsRealtime(2f);
        //fa entrare il giocatore nella prossima stanza
        allRooms[nextRoom].PlayerEntered(player);
        //toglie il gioco dallo stato di pausa
        PauseManager.SetPauseState(false);
        //fa partire l'animazione di transizione in fadeOut
        transitionAnim.SetBool("FadeIn", false);

    }

}

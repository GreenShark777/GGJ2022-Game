//Si occupa dello stato di pausa del gioco(questo script sar� attaccato al men� di pausa)
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    //indica se il gioco � in pausa o meno
    private static bool isPaused = false;
    //riferimento statico al men� di pausa
    private static GameObject pauseMenu;


    private void Start()
    {
        //ottiene il riferimento statico al men� di pausa
        if (!pauseMenu) { pauseMenu = gameObject; }
        //disattiva il men� di pausa e imposta il gioco allo stato di non-pausa
        SetPauseMenuState(false);

    }

    /// <summary>
    /// Permette di impostare lo stato di pausa del gioco
    /// </summary>
    /// <param name="state"></param>
    public static void SetPauseState(bool state)
    {
        //imposta il nuovo stato di pausa
        isPaused = state;
        //in base allo stato di pausa, imposta il timeScale
        Time.timeScale = isPaused ? 0 : 1;

    }
    /// <summary>
    /// Permette di impostare in una sola volta lo stato del men� di pausa e di pausa del gioco(usato per i bottoni del men� di pausa)
    /// </summary>
    /// <param name="state"></param>
    public static void SetPauseMenuState(bool state)
    {
        //imposta il nuovo stato di attivazione del men� di pausa in base al parametro ricevuto
        pauseMenu.SetActive(state);
        //imposta lo stato di pausa del gioco allo stesso del men�(se il men� di pausa � attivo, il gioco viene messo in pausa)
        SetPauseState(state);

    }
    /// <summary>
    /// Ritorna lo stato di pausa del gioco
    /// </summary>
    /// <returns></returns>
    public static bool IsGamePaused() { return isPaused; }

}

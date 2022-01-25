//Si occupa dello stato di pausa del gioco
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    //indica se il gioco è in pausa o meno
    private static bool isPaused = false;


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
    /// Ritorna lo stato di pausa del gioco
    /// </summary>
    /// <returns></returns>
    public static bool IsGamePaused() { return isPaused; }

}

//Si occupa dello stato del giocatore
using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    //comunica se il giocatore è morto o meno
    public static bool isDead = false;
    //riferimento al manager delle animazioni del giocatore
    [SerializeField]
    private SpriteAnimationManager playerSam = default;
    //riferimento statico al manager delle animazioni del giocatore
    private static SpriteAnimationManager staticPlayerSam;


    private void Awake()
    {
        //comunica all'inizio che il giocatore non è morto
        isDead = false;
        //rende statico il riferimento al manager delle animazioni del giocatore
        staticPlayerSam = playerSam;

    }

    /// <summary>
    /// Imposta lo stato di morte del giocatore
    /// </summary>
    /// <param name="deadState"></param>
    public static void SetPlayerDeathState(bool deadState)
    {
        //imposta il nuovo stato di morte del giocatore
        isDead = deadState;
        //se è morto...
        if (isDead)
        {
            //...verrà fatta partire l'animazione di morte del giocatore
            Debug.LogError("NON VIENE ANCORA RICHIAMATA L'ANIMAZIONE DI MORTE DEL GIOCATORE!");
            //staticPlayerSam.StartNewAnimation(999, da mettere, da mettere, true);

            Debug.LogError("Il giocatore è morto!");
        }
        else { Debug.LogError("Il giocatore deve essere respawnato!"); }

    }
    /// <summary>
    /// Permette di impostare lo stato di morte del giocatore a morto
    /// </summary>
    public static void PlayerDied() { SetPlayerDeathState(true); }

}

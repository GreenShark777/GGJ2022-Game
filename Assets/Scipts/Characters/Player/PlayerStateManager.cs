//Si occupa dello stato del giocatore
using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    //comunica se il giocatore è morto o meno
    public static bool isDead = false;
    //comunica se il giocatore è già trasformato
    public static bool hasTransformed = false;
    //riferimento al GameManag di scena
    private GameManag g;
    //riferimento al manager delle animazioni del giocatore
    [SerializeField]
    private SpriteAnimationManager playerSam = default;
    //riferimento statico al manager delle animazioni del giocatore
    private static SpriteAnimationManager staticPlayerSam;
    //riferimento alla schermata da attivare quando si è morti
    [SerializeField]
    private GameObject deathScreen = default;
    //riferimento statico alla schermata da attivare quando si è morti
    private static GameObject staticDeathScreen;


    private void Awake()
    {
        //comunica all'inizio che il giocatore non è morto
        isDead = false;
        //rende statici i riferimenti
        staticPlayerSam = playerSam;
        staticDeathScreen = deathScreen;

    }

    private void Start()
    {
        //ottiene dal GameManag lo stato di trasformazione del giocatore
        g = GetComponent<GameManag>();
        hasTransformed = g.transformed;

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
            //...verrà fatta partire l'animazione di morte del giocatore...
            staticPlayerSam.GoToDeath();
            //...viene fatta apparire la schermata di morte
            staticDeathScreen.SetActive(true);

            Cursor.lockState = CursorLockMode.None;

            Debug.LogError("Il giocatore è morto!");
        }
        else { Debug.LogError("Il giocatore deve essere respawnato!"); }

    }

}

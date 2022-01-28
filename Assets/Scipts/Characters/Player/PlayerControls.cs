//Si occupa di controllare gli input del giocatore
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    //riferimento al manager delle animazioni del giocatore
    [SerializeField]
    private SpriteAnimationManager playerSam = default;
    //riferimento allo script che si occupa del movimento del giocatore
    private CharacterMovement cm;
    //riferimento allo script che si occupa dell'attacco del giocatore
    private AttacksManager am;
    //riferimento allo script che si occupa dell'attacco speciale del giocatore
    private PietrificationAttack pa;

    // Scipts references
    //private CharacterHealth ch;

    //(GABRIELE)COMMENTATO PERCHE' ONDEATH ADESSO PRENDE RIFERIMENTO AD UN'ALTRA FUNZIONE NEL SUO START

    private void Awake()
    {
        // Get references
        //ottiene il riferimento allo script che si occupa del movimento del giocatore
        cm = GetComponent<CharacterMovement>();
        //ottiene il riferimento allo script che si occupa dell'attacco del giocatore
        am = GetComponent<AttacksManager>();
        //ottiene il riferimento allo script che si occupa dell'attacco speciale del giocatore
        pa = GetComponent<PietrificationAttack>();


        //ch = GetComponent<CharacterHealth>();

        // Setup listeners
        //ch.onDeath += Death;
    }

    private void Update()
    {
        //se il gioco non è in pausa e il giocatore non è morto, controlla gli input
        if (!PauseManager.IsGamePaused() && !PlayerStateManager.isDead) { CheckInputs(); }
    }

    /// <summary>
    /// Controlla gli input del giocatore
    /// </summary>
    private void CheckInputs()
    {
        //se il giocatore non sta attaccando...
        if (!am.IsAttacking())
        {
            //...controlla se il giocatore si sta muovendo...
            float movement = Input.GetAxisRaw("Horizontal");
            //...se vuole muoversi, si muove(se non sta attaccando)...
            if (movement != 0) { cm.Move(new Vector2(movement, 0), Input.GetButton("Run")); }
            //...altrimenti, lo fa rimanere in idle...
            else { playerSam.GoBackToIdle(); }
            //...e se il giocatore preme il tasto di salto, lo fa saltare se può
            if (Input.GetButtonDown("Jump")) { cm.Jump(); }

        }
        //se il giocatore preme il tasto d'attacco, prova a farlo attaccare
        if (Input.GetButtonDown("Attack")) { am.WantsToAttack(); }
        //se il giocatore preme il tasto d'attacco speciale, prova a farglielo usare
        if (Input.GetButtonDown("SpecialAttack")) { pa.UsePetrificationAttack(); }
        //se il giocatore preme il tasto per mettere il gioco in pausa, viene attivato il menù di pausa
        if (Input.GetButtonDown("Pause")) { PauseManager.SetPauseMenuState(true); }

    }

    /// <summary>
    /// Richiamato quando la vita scende a 0
    /// </summary>
    private void Death()
    {
        gameObject.SetActive(false);
        Debug.Log("Hai perso!");
    }

    private void OnDestroy()
    {
        // Unsubscribe from listeners
        //if(ch) ch.onDeath -= Death;
    }
}

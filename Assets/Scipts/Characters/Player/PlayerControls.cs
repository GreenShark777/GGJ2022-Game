//Si occupa di controllare gli input del giocatore
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    //riferimento allo script che si occupa del movimento del giocatore
    private CharacterMovement cm;
    //riferimento allo script che si occupa dell'attacco del giocatore
    private AttacksManager am;
    //riferimento allo script che si occupa dell'attacco speciale del giocatore
    private PietrificationAttack pa;

    // Scipts references
    private CharacterHealth ch;

    private void Awake()
    {
        // Get references
        //ottiene il riferimento allo script che si occupa del movimento del giocatore
        cm = GetComponent<CharacterMovement>();
        //ottiene il riferimento allo script che si occupa dell'attacco del giocatore
        am = GetComponent<AttacksManager>();
        //ottiene il riferimento allo script che si occupa dell'attacco speciale del giocatore
        pa = GetComponent<PietrificationAttack>();
        ch = GetComponent<CharacterHealth>();

        // Setup listeners
        ch.onDeath += Death;
    }

    private void Update()
    {
        //se il gioco non è in pausa, controlla gli input
        if (!PauseManager.IsGamePaused()) { CheckInputs(); }
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
            //...e se vuole muoversi, si muove(se non sta attaccando)...
            if (movement != 0) { cm.Move(new Vector2(movement, 0)); }
            //...e se il giocatore preme il tasto di salto, lo fa saltare se può
            if (Input.GetButtonDown("Jump")) { cm.Jump(); }

        }
        //se il giocatore preme il tasto d'attacco, prova a farlo attaccare
        if (Input.GetButtonDown("Attack")) { am.WantsToAttack(); }
        //se il giocatore preme il tasto d'attacco speciale, prova a farglielo usare
        if (Input.GetButtonDown("SpecialAttack")) { pa.UsePetrificationAttack(); }

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
        if(ch) ch.onDeath -= Death;
    }
}

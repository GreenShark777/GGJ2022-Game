//Si occupa di controllare gli input del giocatore
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    //riferimento allo script che si occupa del movimento del giocatore
    private CharacterMovement cm;
    //riferimento allo script che si occupa dell'attacco del giocatore
    private AttacksManager am;
    
    private void Start()
    {
        //ottiene il riferimento allo script che si occupa del movimento del giocatore
        cm = GetComponent<CharacterMovement>();
        //ottiene il riferimento allo script che si occupa dell'attacco del giocatore
        am = GetComponent<AttacksManager>();

    }

    private void Update()
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

    }

}

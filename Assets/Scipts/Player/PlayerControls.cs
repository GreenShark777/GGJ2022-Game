//Si occupa di controllare gli input del giocatore
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    //riferimento allo script che si occupa del movimento del giocatore
    private PlayerMovement pm;


    private void Start()
    {
        //ottiene il riferimento allo script che si occupa del movimento del giocatore
        pm = GetComponent<PlayerMovement>();

    }

    private void Update()
    {
        //controlla se il giocatore si sta muovendo
        float movement = Input.GetAxisRaw("Horizontal");
        //se il giocatore vuole muoversi, si muove
        if (movement != 0) { pm.MovePlayer(new Vector2(movement, 0)); }
        //se il giocatore preme il tasto di salto, lo fa saltare se può
        if (Input.GetButtonDown("Jump")) { pm.Jump(); }

    }

}

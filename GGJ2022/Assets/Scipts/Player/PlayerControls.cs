//Si occupa di controllare gli input del giocatore
using System.Collections;
using System.Collections.Generic;
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

        float movement = Input.GetAxisRaw("Horizontal");

        if (movement != 0) { pm.MovePlayer(new Vector2(movement, 0)); }

    }

}

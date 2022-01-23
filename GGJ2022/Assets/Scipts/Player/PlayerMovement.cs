//Si occupa del movimento del giocatore
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //riferimento al Rigidbody2D del giocatore
    private Rigidbody2D playerRb;

    [SerializeField]
    private Transform playerSprite = default;

    [SerializeField]
    private float speed = 1;


    private void Start()
    {
        //ottiene il riferimento al Rigidbody2D del giocatore
        playerRb = GetComponent<Rigidbody2D>();

    }

    public void MovePlayer(Vector2 newVelocity)
    {

        playerRb.AddForce(newVelocity * speed);

        //if (newVelocity.x > 0) { playerSprite }

    }

}

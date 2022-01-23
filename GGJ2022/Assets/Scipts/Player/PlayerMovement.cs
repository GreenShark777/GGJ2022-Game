//Si occupa del movimento del giocatore
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //riferimento al Rigidbody2D del giocatore
    private Rigidbody2D playerRb;
    //riferimento allo sprite del giocatore
    [SerializeField]
    private Transform playerSprite = default;
    
    [SerializeField]
    private float speed = 1, //indica la velocità di movimento del giocatore
        jumpForce = 1; //indica quanto potente è il salto del giocatore

    //indica la rotazione del giocatore
    private bool facingRight = true;


    private void Start()
    {
        //ottiene il riferimento al Rigidbody2D del giocatore
        playerRb = GetComponent<Rigidbody2D>();

    }

    /// <summary>
    /// Muove il giocatore in base alla direzione ricevuta come parametro
    /// </summary>
    /// <param name="newVelocity"></param>
    public void MovePlayer(Vector2 newVelocity)
    {
        //muove il giocatore, aggiungendo forza al Rigidbody del giocatore in base alla direzione ricevuta per la velocità
        playerRb.AddForce(newVelocity * speed);
        //crea una variabile locale che indica la nuova rotazione che deve avere il giocatore
        Vector3 newRotation = transform.eulerAngles;
        //crea una variabile locale che indica la rotazione del giocatore prima del controllo
        bool checkedRotation = facingRight;
        //se il giocatore va verso destra e il giocatore non è già ruotato verso destra...
        if (newVelocity.x > 0 && !facingRight)
        {
            //...il giocatore viene ruotato verso destra...
            newRotation = new Vector3(0, 0);
            //...e comunica che il giocatore è voltato a destra
            facingRight = true;
            //Debug.Log("Destra");
        }
        //altrimenti, se va verso sinistra e non è già voltato a sinistra...
        else if (newVelocity.x < 0 && facingRight)
        {
            //...il giocatore viene ruotato a sinistra...
            newRotation = new Vector3(0, 180);
            //...e comunica che il giocatore è voltato a sinistra
            facingRight = false;
            //Debug.Log("Sinistra");
        }
        //se la rotazione è cambiata, cambia la rotazione del giocatore con quella calcolata
        if (checkedRotation != facingRight) { playerSprite.eulerAngles = newRotation; }

    }

    public void Jump()
    {

        Vector2 jumpVelocity = new Vector2(0, jumpForce);

        playerRb.AddForce(jumpVelocity);

    }

}

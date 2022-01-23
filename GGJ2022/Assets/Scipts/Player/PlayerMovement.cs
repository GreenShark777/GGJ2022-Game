//Si occupa del movimento del giocatore
using UnityEngine;

public class PlayerMovement : MonoBehaviour, INeedGroundCheck
{
    //riferimento al Rigidbody2D del giocatore
    private Rigidbody2D playerRb;
    //riferimento allo sprite del giocatore
    [SerializeField]
    private Transform playerSprite = default;
    
    [SerializeField]
    private float speed = 1, //indica la velocit� di movimento del giocatore
        jumpForce = 1; //indica quanto potente � il salto del giocatore

    private bool facingRight = true, //indica la rotazione del giocatore
        canJump = true; //indica se il giocatore pu� saltare o meno


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
        //muove il giocatore, aggiungendo forza al Rigidbody del giocatore in base alla direzione ricevuta per la velocit�
        playerRb.AddForce(newVelocity * speed);
        //crea una variabile locale che indica la nuova rotazione che deve avere il giocatore
        Vector3 newRotation = transform.eulerAngles;
        //crea una variabile locale che indica la rotazione del giocatore prima del controllo
        bool checkedRotation = facingRight;
        //se il giocatore va verso destra e il giocatore non � gi� ruotato verso destra...
        if (newVelocity.x > 0 && !facingRight)
        {
            //...il giocatore viene ruotato verso destra...
            newRotation = new Vector3(0, 0);
            //...e comunica che il giocatore � voltato a destra
            facingRight = true;
            //Debug.Log("Destra");
        }
        //altrimenti, se va verso sinistra e non � gi� voltato a sinistra...
        else if (newVelocity.x < 0 && facingRight)
        {
            //...il giocatore viene ruotato a sinistra...
            newRotation = new Vector3(0, 180);
            //...e comunica che il giocatore � voltato a sinistra
            facingRight = false;
            //Debug.Log("Sinistra");
        }
        //se la rotazione � cambiata, cambia la rotazione del giocatore con quella calcolata
        if (checkedRotation != facingRight) { playerSprite.eulerAngles = newRotation; }

    }
    /// <summary>
    /// Fa saltare il giocatore
    /// </summary>
    public void Jump()
    {
        //se pu� saltare...
        if (canJump)
        {
            //...calcola la forza da aggiungere per far saltare il giocatore...
            Vector2 jumpVelocity = new Vector2(0, jumpForce);
            //...aggiunge la forza calcolata, facendo saltare il giocatore...
            playerRb.AddForce(jumpVelocity);
            //...e comunica che non pu� pi� saltare
            canJump = false;

        }

    }
    /// <summary>
    /// Permette al giocatore di saltare di nuovo
    /// </summary>
    public void HasLanded() { canJump = true; }

}

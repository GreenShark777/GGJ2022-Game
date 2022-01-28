//Si occupa del movimento del giocatore
using UnityEngine;

public class CharacterMovement : MonoBehaviour, INeedGroundCheck
{
    //riferimento al Rigidbody2D del giocatore
    private Rigidbody2D rb;
    //riferimento al manager delle animazioni
    [SerializeField]
    private SpriteAnimationManager sam = default;
    //riferimento allo sprite del giocatore
    [SerializeField]
    private Transform characterBody = default;

    [SerializeField]
    private float speed = 1, //indica la velocità di movimento del giocatore
        maxVelocity = 5, //indica la velocità massima a cui può andare il giocatore
        maxFallSpeed = 7, //indica quanto velocemente può cadere al massimo il giocatore
        jumpForce = 1; //indica quanto potente è il salto del giocatore

    [SerializeField]
    private float jumpFallMultiplier = 2.5f, // Aggiungi un moltiplicatore alla gravità durante la discesa
        lowJumpMultiplier = 2f; // Aggiungi un moltiplicatore al salto, durante la prima metà 

    //limiti delle animazioni di movimento del giocatore
    [SerializeField]
    private int[] walkAnimationLimits = new int[2], //CAMMINATA
        runAnimationLimits = new int[2], //CORSA
        jumpAnimationLimits = new int[2]; //SALTO

    private bool facingRight = true, //indica la rotazione del giocatore
        canJump = true; //indica se il giocatore può saltare o meno

    [SerializeField] private bool isPhysiqueDriven = true;


    private void Start()
    {
        //ottiene il riferimento al Rigidbody2D del giocatore
        if(isPhysiqueDriven)
            rb = GetComponent<Rigidbody2D>();

    }

    private void FixedUpdate()
    {
        //corregge problemi nella nuova velocità del Rigidbody del giocatore
        if(isPhysiqueDriven)
            CorrectVelocity();
    }

    /// <summary>
    /// Muove il giocatore in base alla direzione ricevuta come parametro
    /// </summary>
    /// <param name="newVelocity">the movement direction</param>
    public void Move(Vector2 newVelocity, bool running = false)
    {
        //fa partire l'animazione di movimento
        StartMovementAnimation(running);

        if(isPhysiqueDriven)
        {
            PhysicMovement(newVelocity);
            return;
        }

        transform.Translate(newVelocity * speed * Time.deltaTime);

    }

    private void StartMovementAnimation(bool running)
    {
        //ottiene i limiti dell'animazione da far partire(camminata o corsa)
        int[] limits = running ? walkAnimationLimits : runAnimationLimits;
        //fa partire l'animazione di movimento(camminata o corsa)
        sam.StartNewAnimation(1, limits[0], limits[1]);

    }

    /// <summary>
    /// Muove il giocatore in base alla direzione ricevuta come parametro, facendo l'override della velocità
    /// </summary>
    /// <param name="newVelocity">the movement direction</param>
    /// <param name="overSpeed">The speed to use</param>
    public void Move(Vector2 newVelocity, float overSpeed)
    {
        if (isPhysiqueDriven)
        {
            PhysicMovement(newVelocity);
            return;
        }

        transform.Translate(newVelocity * overSpeed * Time.deltaTime);
    }

    private void PhysicMovement(Vector2 newVelocity)
    {
        //muove il giocatore, aggiungendo forza al Rigidbody del giocatore in base alla direzione ricevuta per la velocità
        rb.AddForce(newVelocity * speed);
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
        if (checkedRotation != facingRight) { characterBody.eulerAngles = newRotation; }

        // rb.velocity = Vector2.SmoothDamp(rb.velocity, movement, ref m_Velocity, m_MovementSmoothing);
    }

    /// <summary>
    /// Corregge errori nella velocity del giocatore
    /// </summary>
    private void CorrectVelocity()
    {
        //ottiene la velocity attuale del giocatore in entrambi assi
        float XVelocity = rb.velocity.x;
        float YVelocity = rb.velocity.y;
        
        //impedisce al giocatore di superare la velocità massima sia a destra che a sinistra
        if (XVelocity > maxVelocity) { SetNewVelocity(maxVelocity, true); }
        if (XVelocity < -maxVelocity) { SetNewVelocity(-maxVelocity, true); }
        
        //impedisce al giocatore di cadere troppo velocemente
        if (YVelocity < -maxFallSpeed) { SetNewVelocity(-maxFallSpeed, false); }

        // Se siamo in caduta (seconda metà del salto)
        if (rb.velocity.y < 0)
        {
            // Applica l'effetto del moltiplicatore
            rb.velocity += Vector2.up * Physics2D.gravity * (jumpFallMultiplier - 1) * Time.fixedDeltaTime;
        }
        else if (rb.velocity.y > 0)
        {
            // Applica l'effetto del moltiplicatore durante la prima metà del salto
            rb.velocity += Vector2.up * Physics2D.gravity * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
        }
    }

    /// <summary>
    /// Imposta la velocità del Rigidbody del giocatore a quella indicata e nell'asse indicato
    /// </summary>
    /// <param name="minus"></param>
    private void SetNewVelocity(float newVelocity, bool horizontal)
    {
        //crea un vettore locale, inizializzato alla velocity del Rigidbody del giocatore
        Vector2 velocityToSet = rb.velocity;
        //in base all'asse indicato, viene cambiata la velocity con il parametro ricevuto
        if (horizontal) velocityToSet.x = newVelocity; //ASSE X
        else velocityToSet.y = newVelocity; //ASSE Y
        //infine, imposta la nuova velocity al Rigidbody del giocatore
        rb.velocity = velocityToSet;

    }
    /// <summary>
    /// Fa saltare il giocatore
    /// </summary>
    public void Jump()
    {
        //se può saltare...
        if (canJump)
        {
            //...fa partire l'animazione di salto...
            sam.StartNewAnimation(2, jumpAnimationLimits[0], jumpAnimationLimits[1]);
            //...rimuove ogni forza di movimento nell'asse Y...
            rb.velocity = new Vector2(rb.velocity.x, 0);
            //...calcola la forza da aggiungere per far saltare il giocatore...
            Vector2 jumpVelocity = new Vector2(0, jumpForce);
            //...aggiunge la forza calcolata, facendo saltare il giocatore...
            rb.AddForce(jumpVelocity);
            //...e comunica che non può più saltare
            canJump = false;
        }

    }
    /// <summary>
    /// Permette di impostare se il giocatore può saltare o meno
    /// </summary>
    public void HasLanded(bool landed) { canJump = landed; }

}

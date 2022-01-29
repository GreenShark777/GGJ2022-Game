//Si occupa dell'attacco pietrificazione del giocatore
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PietrificationAttack : MonoBehaviour
{
    //indica il numero massimo di collider da poter controllare con un singolo attacco
    private const int NCOLLIDERS = 50;

    [Header("References")]
    //riferimento al manager delle animazioni dello sprite
    [SerializeField]
    private SpriteAnimationManager sam = default;
    //indica quanto velocemente deve andare l'animazione d'attacco
    [SerializeField]
    private float attackAnimationSpeed = default;
    //indica i limiti per l'animazione dell'attacco pietrificazione
    [SerializeField]
    private int[] pietrificationAnimationLimits = new int[2];
    //indica i limiti per l'animazione della trasformazione
    [SerializeField]
    private int[] transformationAnimationLimits = new int[2];
    //riferimento allo slider della carica per l'attacco speciale
    [SerializeField]
    private Slider specialAttackSlider = default;
    //riferimento allo sprite da usare durante l'attacco per scurire la scena
    [SerializeField]
    private SpriteRenderer lightsOutSprite = default;
    //riferimento al punto in cui deve iniziare l'attacco
    [SerializeField]
    private Transform attackStartPoint = default;
    //riferimento allo script che si occupa del karma del giocatore
    private PlayerDuality pd;

    [Header("Animation")]
    //indica se la mossa speciale può essere usata o meno
    private bool canUse = false;
    //indica la carica che si deve avere per poter nuovamente usare l'attacco speciale
    [SerializeField]
    private int maxCharge = 100;
    //indica quanto velocemente si ricarica l'attacco speciale
    [SerializeField]
    private float chargeRatio = 0.5f;
    //indica quanto durerà l'attacco pietrificazione
    [SerializeField]
    private float attackDuration = 4;
    //indica il valore di alpha che deve avere l'immagine di lightsOut quando è in corso l'attacco speciale
    [SerializeField]
    private float lightsOutAlpha = 0.7f;
    //indica quanto velocemente avviene il fadeIn o fadeOut dell'immagine lightsOut
    [SerializeField]
    private float fadeInOutRatio = 0.08f;

    [Header("Attack")]
    //indica quanto lontano l'attacco di pietrificazione ha effetto
    [SerializeField]
    private float attackRange = 13.3f;
    //variabile per stabilire l'angolo massimo di effetto dell'attacco
    [SerializeField]
    private float maxAngle = 75;


    private void Awake()
    {
        //imposta il valore massimo dello slider
        specialAttackSlider.maxValue = maxCharge;
        //imposta al valore massimo il valore dello slider
        specialAttackSlider.value = maxCharge;
        //infine, rende invisibile l'immagine di lightsOut
        Color transparent = lightsOutSprite.material.color;
        transparent.a = 0;
        lightsOutSprite.material.color = transparent;
        //ottiene il riferimento allo script che si occupa del karma del giocatore
        pd = GetComponent<PlayerDuality>();

        //StartCoroutine(FadeInOutImage(0, false));
        //NON USARE QUESTO PERCHE' POTREBBE CREARE PROBLEMI NEL CASO IL GIOCATORE FACCIA LA MOSSA SPECIALE DURANTE IL FADE OUT
    }

    private void FixedUpdate()
    {
        //se non si può usare la mossa speciale, vuol dire che la carica non è al massimo, quindi...
        if (!canUse && !PauseManager.IsGamePaused())
        {
            //...continua ad aumentare il valore dello slider...
            specialAttackSlider.value += chargeRatio;
            //...e, se raggiunge il massimo, comunica che si può nuovamente usare
            if (specialAttackSlider.value >= maxCharge) { canUse = true; }

        }

    }
    /// <summary>
    /// Cerca di usare l'attacco di pietrificazione
    /// </summary>
    public void UsePetrificationAttack(bool isTransforming = false)
    {
        //se si può usare(o ci si sta trasformando)...
        if (canUse || isTransforming)
        {
            //...comunica che non si può più usare...
            canUse = false;
            //...scarica lo slider...
            specialAttackSlider.value = 0;
            //..se ci si sta trasformando, ferma tutte le coroutine...
            if (isTransforming) { StopAllCoroutines(); }
            //...e fa partire la coroutine d'attacco
            StartCoroutine(ManagePietrificationAttackTiming(isTransforming));

        }

    }
    /// <summary>
    /// Si occupa delle tempistiche dell'attacco pietrificazione
    /// </summary>
    /// <returns></returns>
    private IEnumerator ManagePietrificationAttackTiming(bool isTransforming)
    {
        //mette il gioco in pausa
        PauseManager.SetPauseState(true);
        //prende i limiti dell'animazione da far fare al giocatore in base al parametro ricevuto
        int[] animationLimits = !isTransforming ? pietrificationAnimationLimits : transformationAnimationLimits;
        //fa partire l'animazione di attacco speciale del giocatore
        sam.StartNewAnimation(100, animationLimits[0], animationLimits[1], true);
        sam.SetAnimationSpeed(attackAnimationSpeed);
        //fa partire una coroutine per il fadeIn dell'immagine di animazione
        StartCoroutine(FadeInOutImage(lightsOutAlpha, true));
        //aspetta metà del tempo d'attacco
        yield return new WaitForSecondsRealtime(attackDuration / 2);
        //pietrifica i nemici di fronte al giocatore
        PetrifyEnemies();
        //aspetta che finisca l'attacco
        yield return new WaitForSecondsRealtime(attackDuration / 2);
        //toglie il gioco dalla pausa
        PauseManager.SetPauseState(false);
        //fa partire una coroutine per il fadeOut dell'immagine di animazione
        StartCoroutine(FadeInOutImage(0, false));

    }
    /// <summary>
    /// Si occupa del fadeIn o fadeOut dell'immagine di animazione
    /// </summary>
    /// <param name="targetAlpha"></param>
    /// <param name="fadeIn"></param>
    /// <returns></returns>
    private IEnumerator FadeInOutImage(float targetAlpha, bool fadeIn)
    {
        //calcola di quanto deve aumentare o diminuire l'alpha dell'immagine
        float ratio = fadeIn ? fadeInOutRatio : (fadeInOutRatio * -1);
        //crea un nuovo colore locale che indicherà il colore che deve avere l'immagine di animazione
        Color newColor = lightsOutSprite.material.color;
        //calcola l'alpha che il nuovo colore deve avere
        float newAlpha = Mathf.Clamp((newColor.a + ratio), 0, lightsOutAlpha);
        //viene impostato al nuovo colore l'alpha calcolato
        newColor.a = newAlpha;
        //viene cambiato il colore dell'immagine di animazione
        lightsOutSprite.material.color = newColor;
        //aspetta che arrivi il prossimo FixedUpdate
        yield return new WaitForSecondsRealtime(Time.fixedDeltaTime);
        //se non si è ancora arrivati all'alpha obiettivo, comunica che bisogna continuare il ciclo
        bool continueCycle = (fadeIn && lightsOutSprite.material.color.a < targetAlpha) ||
            (!fadeIn && lightsOutSprite.material.color.a > targetAlpha);

        //se bisogna continuare il ciclo, fa ripartire la coroutine
        if (continueCycle) { StartCoroutine(FadeInOutImage(targetAlpha, fadeIn)); }

    }
    /// <summary>
    /// Pietrifica tutti i nemici nel raggio d'azione dell'attacco
    /// </summary>
    private void PetrifyEnemies()
    {
        Debug.Log("Inizio pietrificazione");
        //array locale che conterrà tutti i collider dentro il raggio d'azione dell'attacco
        Collider2D[] overlaps = new Collider2D[NCOLLIDERS];
        //prende tutti i collider degli oggetti vicini al punto d'inizio dell'attacco in un cerchio con come diametro il raggio d'azione dell'attacco
        int count = Physics2D.OverlapCircleNonAlloc(attackStartPoint.position, attackRange, overlaps);
        //cicla tra tutti i collider presenti nella vista del gameobject
        for (int i = 0; i < count; i++)
        {
            //se il collider nell'indice i dell'array non è nullo...
            Collider2D collFound = overlaps[i];
            if (collFound)
            {
                //...prende il riferimento al transform dell'oggetto con il collider trovato...
                Transform objFound = collFound.transform;
                //...se l'oggetto trovato è vulnerabile alla pietrificazione...
                PetrificationVulnerability pv = objFound.GetComponent<PetrificationVulnerability>();
                if (pv)
                {
                    //...calcola la direzione tra il punto d'inizio dell'attacco e l'oggetto trovato e lo normalizza...
                    Vector3 directionBetween = (objFound.position - attackStartPoint.position).normalized;
                    //...porta la z a 0 per avere maggiore accuratezza, in quanto potrebbe provocare errori nel calcolo dell'angolo...
                    directionBetween.z *= 0;
                    //...calcola l'angolo tra il centro del raggio d'attacco e l'oggetto trovato...
                    float Angle = Vector3.Angle(attackStartPoint.right, directionBetween);
                    //...se l'angolo è minore o uguale all' angolo massimo...
                    if (Angle <= maxAngle)
                    {
                        Debug.LogError("Provato a pietrificare oggetto: " + objFound.name);
                        //...si prova a pietrificare il nemico...
                        bool petrified = pv.TryToPetrify();
                        //...se il nemico è stato pietrificato, comunica che è stato ucciso un nemico
                        if (petrified) { pd.KilledAnEnemy(); }

                    }

                }

            }

        }

    }
    /// <summary>
    /// Ritorna il tempo che impiega la mossa speciale a finire
    /// </summary>
    /// <returns></returns>
    public float GetAttackDuration() { return attackDuration; }


    private void OnDrawGizmos()
    {
        //se esiste il punto d'inizio dell'attacco, crea i gizmo
        if (attackStartPoint)
        {
            //crea un gizmo che indica il raggio d'azione dell'attacco
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(attackStartPoint.position, attackRange);
            //calcola come disegnare le linee che indicheranno l'angolo d'azione dell'attacco
            Vector3 fovPoint1 = Quaternion.AngleAxis(maxAngle, attackStartPoint.forward) * attackStartPoint.right * attackRange;
            Vector3 fovPoint2 = Quaternion.AngleAxis(-maxAngle, attackStartPoint.forward) * attackStartPoint.right * attackRange;
            //crea le linee calcolate
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(attackStartPoint.position, fovPoint1);
            Gizmos.DrawRay(attackStartPoint.position, fovPoint2);

        }

    }

}

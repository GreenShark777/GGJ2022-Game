//Si occupa del karma del giocatore
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDuality : MonoBehaviour, IUpdateData
{
    //riferimento allo slider della dualità del giocatore
    [SerializeField]
    private Slider dualitySlider = default;
    //riferimento al CanvasGroup che si occupa di nascondere o mostrare lo slider
    private CanvasGroup sliderCG;
    //riferimento al GameManag di scena
    [SerializeField]
    private GameManag g = default;
    //riferimento allo script che si occupa dell'attacco speciale del giocatore
    private PietrificationAttack pa;
    //riferimento al collider di danno del giocatore
    [SerializeField]
    private DamagingCollider playerDmgColl = default;
    //riferimento allo SpriteRenderer per la luce attorno al giocatore
    [SerializeField]
    private SpriteRenderer playerLightSr = default;
    //riferimento allo sprite della luce oscura
    [SerializeField]
    private Sprite blackLightSprite = default;
    //indica quanti nemici il giocatore ha ucciso
    private int enemiesKilled = default;
    //indica quanti nemici deve uccidere il giocatore per arrivare al punto di non ritorno
    [SerializeField]
    private int killsToBad = default;
    //indica quante volte il giocatore deve essere avvisato del suo livello di cattiveria(contando anche quando si sta trasformando)
    [SerializeField]
    private int numberOfAlerts = 3;
    //indica per quanto tempo viene mostrato lo slider
    [SerializeField]
    private float showForHowMuch = 4;
    //indica quanto velocemente deve essere mostrato lo slider
    [SerializeField]
    private float showSpeed = 0.01f;


    private void Awake()
    {
        //ottiene il numero di nemici uccisi salvato
        enemiesKilled = g.enemiesPetrified;
        //imposta il valore massimo allo slider al numero di nemici da uccidere per diventare cattivo
        dualitySlider.maxValue = killsToBad;
        //imposta il valore dello slider al numero di nemici uccisi
        dualitySlider.value = enemiesKilled;
        //ottiene il riferimento al CanvasGroup dello slider di dualità
        sliderCG = dualitySlider.GetComponent<CanvasGroup>();
        //nasconde lo slider all'inizio
        sliderCG.alpha = 0;
        //ottiene il riferimento allo script che si occupa dell'attacco speciale del giocatore
        pa = GetComponent<PietrificationAttack>();

    }

    private void Start()
    {
        //se il giocatore si era trasformato, viene impostato lo stato di trasformazione
        if (g.transformed) { SetTransformationState(); }

    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.F2)) { KilledAnEnemy(); }
        //Debug.LogError("RICORDA: NON VIENE FATTA ANCORA BENE L'ANIMAZIONE DI TRASFORMAZIONE");
    }

    /// <summary>
    /// Aumenta il numero di nemici uccisi e controlla se si è superata una soglia
    /// </summary>
    /// <param name="checkForTresholds"></param>
    public void KilledAnEnemy()
    {
        //incrementa il numero di nemici uccisi, se bisogna controllare le soglie
        enemiesKilled++;
        //controlla se il nemico è troppo cattivo
        bool isTooBad = (enemiesKilled == killsToBad);
        //imposta il valore al numero di nemici uccisi
        dualitySlider.value = enemiesKilled;
        //crea una variabile che indica a quale soglia di controllo siamo arrivati
        int alertsChecked;
        //cicla il controllo per il numero di volte che bisogna avvisare il giocatore in caso di superamento di soglia
        for (alertsChecked = 0; alertsChecked < numberOfAlerts; alertsChecked++)
        {
            //ottiene il numero che indica la soglia da controllare
            int treshold = (killsToBad / numberOfAlerts) * alertsChecked;
            //se il numero di nemici uccisi è uguale alla soglia controllata, mostra lo slider di dualità
            if (isTooBad || enemiesKilled == treshold) { StartCoroutine(ShowSlider(true)); break; }

        }
        //se il numero di nemici uccisi raggiunge l'ultima soglia, il giocatore si trasforma
        if (isTooBad) { StartCoroutine(TransformPlayer()); }

    }
    /// <summary>
    /// Mostra lo slider di dualità
    /// </summary>
    private IEnumerator ShowSlider(bool show)
    {
        //calcola l'alpha che dovrà avere il CanvasGroup dello slider
        float newAlpha = sliderCG.alpha + (show ? showSpeed : -showSpeed);
        //imposta il nuovo alpha calcolato
        sliderCG.alpha = newAlpha;
        //in base a se si vuole mostrare o nascondere lo slider, imposta l'alpha obiettivo
        float targetAlpha = show ? 1 : 0;
        //controlla se è stato raggiunto l'alpha obiettivo
        bool targetReached = sliderCG.alpha == targetAlpha;
        //se l'alpha obiettivo è stato raggiunto, aspetta del tempo per far vedere più a lungo lo slider
        float timeToWait = targetReached ? showForHowMuch : showSpeed;
        yield return new WaitForSecondsRealtime(timeToWait);
        //se si vuole mostrare lo slider o lo si vuole nascondere ma non è stato raggiunto l'alpha obiettivo, ricomincia la coroutine
        if (show || (!show && !targetReached)) { StartCoroutine(ShowSlider(show && !targetReached)); }

        //Debug.LogError("Alpha = " + sliderCG.alpha + " | Target = " + targetAlpha + " -> " + targetReached);
    }
    /// <summary>
    /// Il giocatore si trasforma e cambiano varie statistiche e visuals
    /// </summary>
    private IEnumerator TransformPlayer()
    {
        //Debug.LogError("Giocatore si trasforma");
        //fa usare l'attacco pietrificazione al giocatore, indicando che è per la trasformazione
        pa.UsePetrificationAttack(true);
        //toglie la luce attorno al giocatore
        playerLightSr.sprite = null;
        //aspetta che l'animazione di trasformazione arrivi a metà
        yield return new WaitForSecondsRealtime(pa.GetAttackDuration() / 2);
        //imposta tutti gli stati di quando il giocatore viene trasformato
        SetTransformationState();
        //indica al GameManag che il giocatore si è trasformato
        g.transformed = true;

    }
    /// <summary>
    /// Imposta tutti gli stati di quando il giocatore viene trasformato
    /// </summary>
    private void SetTransformationState()
    {
        //imposta il danno del giocatore al valore massimo
        playerDmgColl.SetNewDamage(999);
        //cambia la luce attorno al giocatore
        playerLightSr.sprite = blackLightSprite;
        //comunica che il giocatore si è trasformato
        PlayerStateManager.hasTransformed = true;

        //Debug.LogError("Cambiate statistiche e altro");
    }

    public void UpdateData()
    {
        //aggiorna il numero di nemici pietrificati
        g.enemiesPetrified = enemiesKilled;

    }

}

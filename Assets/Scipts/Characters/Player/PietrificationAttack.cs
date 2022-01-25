//Si occupa dell'attacco pietrificazione del giocatore
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PietrificationAttack : MonoBehaviour
{
    //riferimento al manager delle animazioni dello sprite
    [SerializeField]
    private SpriteAnimationManager sam = default;
    //riferimento allo slider della carica per l'attacco speciale
    [SerializeField]
    private Slider specialAttackSlider = default;
    //riferimento allo sprite da usare durante l'attacco per scurire la scena
    [SerializeField]
    private SpriteRenderer lightsOutSprite = default;
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
    public void UsePetrificationAttack()
    {
        //se si può usare...
        if (canUse)
        {
            //...comunica che non si può più usare...
            canUse = false;
            //...scarica lo slider...
            specialAttackSlider.value = 0;
            //...e fa partire la coroutine d'attacco
            StartCoroutine(ManagePietrificationAttack());
        
        }

    }
    /// <summary>
    /// Si occupa delle tempistiche dell'attacco pietrificazione
    /// </summary>
    /// <returns></returns>
    private IEnumerator ManagePietrificationAttack()
    {
        //mette il gioco in pausa
        PauseManager.SetPauseState(true);
        //fa partire una coroutine per il fadeIn dell'immagine di animazione
        StartCoroutine(FadeInOutImage(lightsOutAlpha, true));
        //aspetta che finisca l'attacco
        yield return new WaitForSecondsRealtime(attackDuration);
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

}

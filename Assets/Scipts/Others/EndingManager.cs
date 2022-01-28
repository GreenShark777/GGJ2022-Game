//Si occupa di mostrare il finale adatto ai dati salvati
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EndingManager : MonoBehaviour
{
    //riferimento al GameManag di scena
    [SerializeField]
    private GameManag g = default;
    //riferimento all'Animator dell'immagine transizione
    [SerializeField]
    private Animator transitionImage = default;
    //riferimento all'immagine che mostra il finale ottenuto dal giocatore
    private Image endingImage;
    //riferimento allo sprite per il finale cattivo
    [SerializeField]
    private Sprite badEndingSprite = default;


    private void Start()
    {
        //ottiene il riferimento all'immagine che mostra il finale ottenuto dal giocatore
        endingImage = GetComponent<Image>();
        //se il giocatore si è trasformato, cambia lo sprite con quello per il finale cattivo
        if (g.transformed) { ChangeToBadEnding(); }

    }
    /// <summary>
    /// Cambia lo sprite con quello per il finale cattivo
    /// </summary>
    private void ChangeToBadEnding() { endingImage.sprite = badEndingSprite; }
    /// <summary>
    /// Fa andare al menù principale tramite transizione(richiamata da bottone nella scena dei finali)
    /// </summary>
    public void GoBackToMainMenu() { StartCoroutine(TransitionToMainMenu()); }
    /// <summary>
    /// Dopo la dovuta transizione, porta alla scena del menù principale
    /// </summary>
    /// <returns></returns>
    private IEnumerator TransitionToMainMenu()
    {
        //fa partire il fadeIn di transizione
        transitionImage.SetBool("FadeIn", true);
        //aspetta un po'
        yield return new WaitForSeconds(2);
        //carica la scena di gameplay
        SceneChange.StaticGoToScene("MainMenu");

    }

}

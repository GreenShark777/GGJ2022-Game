//Si occupa di come far iniziare a giocare il giocatore(se cancellando i dati o partendo da un checkpoint)
using System.Collections;
using UnityEngine;

public class StartPlaying : MonoBehaviour
{
    //riferimento al GameManag di scena
    private GameManag g;
    //riferimento all'Animator dell'immagine transizione
    [SerializeField]
    private Animator transitionImage = default;


    private void Awake()
    {
        //ottiene il riferimento al GameManag di scena
        g = GetComponent<GameManag>();

    }

    /// <summary>
    /// Comincia una nuova partita
    /// </summary>
    public void StartNewGame()
    {
        //cancella i dati di gioco(solo riguardanti gameplay)
        SaveSystem.ClearData(g, false);
        //carica la scena di gameplay
        StartCoroutine(TransitionToGameplay());

    }
    /// <summary>
    /// Continua la partita salvata
    /// </summary>
    public void LoadGame() { StartCoroutine(TransitionToGameplay()); }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private IEnumerator TransitionToGameplay()
    {
        //fa partire il fadeIn di transizione
        transitionImage.SetBool("FadeIn", true);
        //aspetta un po'
        yield return new WaitForSeconds(2);
        //carica la scena di gameplay
        SceneChange.StaticGoToScene("Gameplay");

    }

}

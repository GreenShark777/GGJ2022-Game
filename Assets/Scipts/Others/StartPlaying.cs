//Si occupa di come far iniziare a giocare il giocatore(se cancellando i dati o partendo da un checkpoint)
using UnityEngine;

public class StartPlaying : MonoBehaviour
{
    //riferimento al GameManag di scena
    private GameManag g;


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
        SceneChange.StaticGoToScene("Gameplay");

    }
    /// <summary>
    /// Continua la partita salvata
    /// </summary>
    public void LoadGame() { SceneChange.StaticGoToScene("Gameplay"); }

}

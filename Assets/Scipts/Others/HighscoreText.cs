//Si occupa di cambiare il testo di highscore
using UnityEngine;
using TMPro;

public class HighscoreText : MonoBehaviour
{
    //riferimento al testo di highscore
    private TextMeshProUGUI highscoreText;
    //riferimento al GameManag di scena
    [SerializeField]
    private GameManag g = default;


    private void Start()
    {
        //ottiene il riferimento al testo di highscore
        highscoreText = GetComponent<TextMeshProUGUI>();
        //se si ha un highscore, cambia il testo
        if (g.highscore != 0) highscoreText.text = "Highscore: " + g.highscore;
        //altrimenti disattiva il testo
        else { gameObject.SetActive(false); }

    }

}

//Si occupa della canzone di background
using System.Collections;
using UnityEngine;

public class BgMusicManager : MonoBehaviour
{
    //riferimento statico di questo singleton
    public static BgMusicManager instance;
    //riferimento al GameManag di scena
    [SerializeField]
    private GameManag g = default;
    //riferimento all'audioSource che suona la musica di background
    private AudioSource bgMusic;
    //riferimenti a tutte le musiche di gioco
    [SerializeField]
    private AudioClip mainMenuMusic = default,
        firstAreaMusic = default,
        secondAreaMusic = default,
        fightingMusic = default,
        goodEndingMusic = default,
        badEndingMusic = default;

    //indica l'indice della scena in cui ci si trova
    private int sceneIndex = 0;
    //indica quanto velocemente deve essere fatto il fade della musica
    [SerializeField]
    private float fadeRatio = 0.45f;


    private void Start()
    {
        //crea il riferimento all'istanza
        instance = this;
        //ottiene il riferimento all'audioSource che suona la musica di background
        bgMusic = GetComponent<AudioSource>();
        //prende l'indice della scena in cui ci si trova
        sceneIndex = gameObject.scene.buildIndex;
        //fa partire la canzone in base alla scena in cui ci si trova
        StartCoroutine(FadeMusic(false));

    }
    /// <summary>
    /// Ottiene la musica da suonare in base alla scena in cui ci si trova
    /// </summary>
    /// <param name="fighting"></param>
    /// <returns></returns>
    private AudioClip GetMusic(bool fighting = false)
    {
        //crea una variabile locale alla clip audio da suonare
        AudioClip clip = null;
        //se bisogna mettere la canzone di combattimento, viene messa quella di combattimento
        if (fighting) { clip = fightingMusic; }
        //altrimenti...
        else
        {
            //...in base all'indice della scena in cui ci si trova, viene messa la musica adatta
            switch (sceneIndex)
            {
                //MAIN MENU
                case 0: { clip = mainMenuMusic; break; }
                //GAMEPLAY(IN BASE ALLA STANZA IN CUI IL GIOCATORE E', VIENE SCELTA LA MUSICA DELL'AREA IN CUI SI TROVA)
                case 1: { clip = (g.lastEnteredRoom < GameManag.SECOND_AREA_ROOM) ? firstAreaMusic : secondAreaMusic; break; }
                //ENDING(IN BASE A CIO' CHE IL GIOCATORE HA FATTO, CAMBIA MUSICA)
                case 2: { clip = !g.transformed ? goodEndingMusic : badEndingMusic; break; }
                //DEBUG
                default: { Debug.LogError("Ci si trova in una scena di cui non si ha l'audio"); break; }

            }

        }
        //ritorna la clip ottenuta
        return clip;

    }
    /// <summary>
    /// Permette di cambiare musica in base a dove ci si trova
    /// </summary>
    public void ChangeMusic() { StartCoroutine(FadeMusic(true, true)); }
    /// <summary>
    /// Gestisce fade-in e fade-out della musica per poi cambiarla
    /// </summary>
    /// <param name="fadeOut"></param>
    /// <param name="fighting"></param>
    /// <returns></returns>
    private IEnumerator FadeMusic(bool fadeOut, bool fighting = false)
    {
        //Debug.LogError("FADE: " + fadeOut);

        //continua a ciclare fino a quando il fade della musica non è finito
        while ((fadeOut && bgMusic.volume > 0.05f) || (!fadeOut && bgMusic.volume < 0.95f))
        {
            //incrementa o decrementa il volume della musica
            bgMusic.volume += (fadeOut ? -fadeRatio : fadeRatio) * Time.deltaTime;

            //Debug.LogError("STILL IN CYCLE: " + bgMusic.volume);

            //attende il prossimo fixed update
            yield return new WaitForFixedUpdate();

        }
        //se è stato eseguito il fade-out o non c'è ancora clip...
        if (fadeOut || !bgMusic.clip)
        {
            //...imposta la clip da suonare...
            bgMusic.clip = GetMusic(fighting);
            //...fa partire l'AudioSource...
            bgMusic.Play();
            //...fa ripartire la coroutine, stavolta facendo invece il fade-in
            StartCoroutine(FadeMusic(false, fighting));

        }

    }

}

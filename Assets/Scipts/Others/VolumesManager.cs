//Si occupa dei volumi di gioco
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumesManager : MonoBehaviour, IUpdateData
{
    //riferimento al GameManag di scena
    [SerializeField]
    private GameManag g = default;

    private float musicValue, //indica il volume della musica di gioco
        sfxValue, //indica il volume degli effetti sonori di gioco
        globalValue; //indica il volume di tutti i suoni di gioco

    //riferimento all'audio mixer master
    [SerializeField]
    private AudioMixer master = default;
    //riferimenti agli slider dei volumi
    [SerializeField]
    private Slider musicSlider = default,
        sfxSlider = default,
        globalSlider = default;


    private void Start()
    {
        //ottiene il volume salvato per tutti gli slider
        musicSlider.value = g.savedMusicVolume;
        sfxSlider.value = g.savedSfxVolume;
        globalSlider.value = g.savedMasterVolume;
        //cambia il volume in base ai valori salvati
        ChangeMasterVolume();
        ChangeMusicVolume();
        ChangeSfxVolume();

    }

    /// <summary>
    /// Cambia il volume della musica in base al valore dello slider(questa funzione viene richiamata dall'appositi slider)
    /// </summary>
    public void ChangeMusicVolume() { musicValue = musicSlider.value; master.SetFloat("musicVolume", musicValue); }
    /// <summary>
    /// Cambia il volume degli effetti sonori in base al valore dello slider(questa funzione viene richiamata dall'appositi slider)
    /// </summary>
    public void ChangeSfxVolume() { sfxValue = sfxSlider.value; master.SetFloat("sfxVolume", sfxValue); }
    /// <summary>
    /// Cambia il volume globale di gioco in base al valore dello slider(questa funzione viene richiamata dall'appositi slider)
    /// </summary>
    public void ChangeMasterVolume() { globalValue = globalSlider.value; master.SetFloat("globalVolume", globalValue); }

    public void UpdateData()
    {
        //aggiorna i dati riguardanti le musiche di gioco
        g.savedMusicVolume = musicValue;
        g.savedSfxVolume = sfxValue;
        g.savedMasterVolume = globalValue;
        Debug.Log("Dati dei volumi aggiornati");
    }

}

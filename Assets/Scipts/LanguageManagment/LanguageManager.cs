//si occupa del cambio della lingua nel gioco
using TMPro;
using System.Collections.Generic;
using UnityEngine;

public class LanguageManager : MonoBehaviour
{
	//VARIABILI PUBBLICHE
	//variabile che tiene conto dell'ultima lingua selezionata
	//
	//0 = ITALIANO
	//1 = INGLESE
	//public static int mostRecentValue;
	//lista di tutti gli script dei testi da far diventare della lingua selezionata(vengono aggiunti alll'Awake di TextLanguageChange)
	[HideInInspector]
	public List<TextLanguageChange> textsToChangeLanguage;


	//VARIABILI PRIVATE
	//riferimento alla lista dropdown che si occupa del cambio di lingua
	[SerializeField]
	private TMP_Dropdown languageDropdownList = default;
	//riferimento al GameManag della scena
	[SerializeField]
	private GameManag g = default;


    //all'inizio della scena la lingua viene cambiata in base all'ultima lingua impostata dal giocatore nella scena precedente
    void Start()
	{
		//prende l'indice della lingua corrente(salvata nel GameManag)
		int currentLanguage = g.savedLanguage;
		//se esiste il riferimento alla dropdown list per il cambio della lingua...
		if (languageDropdownList != null)
		{
			//...viene cambiato il suo valore in base al valore salvato
			languageDropdownList.value = currentLanguage;

		}
		//Debug.Log("Lingua caricata: " + g.savedLanguage);
		//viene cambiata la lingua in base al valore nella lista dropdown
		LanguageChange(currentLanguage);
	
	}

    //DEBUG---------------------------------------------------------------------------------------------------------------------------------------------------------------------------
	/*
    private void Update()
    {
        //premendo il tasto L...
        if (Input.GetKeyDown(KeyCode.L))
        {
            //...cambia alla lingua successiva...
            g.savedLanguage++;
            //...se non esiste una lingua successiva, ritorna alla prima...
            if (g.savedLanguage > 1) { g.savedLanguage = 0; }
            //se esiste il riferimento alla dropdown list per il cambio della lingua, gli da il valore appena modificato al GameManag
            if (languageDropdownList != null) { languageDropdownList.value = g.savedLanguage; }
            //...infine, cambia la lingua di tutti i testi
            LanguageChange();

        }

    }
	*/
    //DEBUG---------------------------------------------------------------------------------------------------------------------------------------------------------------------------


    /// <summary>
    /// Cambia la lingua di tutti i testi che fanno parte della lista. Viene richiamata quando viene cambiato il valore della lista dropdown
    /// </summary>
    public void LanguageChange(int language)
	{
		//aggiorna il valore del GameManag per la lingua selezionata
		g.savedLanguage = language;
		//cicla ogni oggetto nella lista dei testi da cambiare per cambiargli la lingua
		for (int i = 0; i < textsToChangeLanguage.Count; i++)
        {
			//cambia la lingua in base al valore della lista dropdown
			textsToChangeLanguage[i].ChangeLanguage(g.savedLanguage);

        }
		//Debug.Log("Lingua cambiata in: " + g.savedLanguage);
	}
	/// <summary>
	/// Permette ad altri script di sapere la lingua corrente nel gioco
	/// </summary>
	/// <returns></returns>
	public int GetCurrentLanguage() { /*Debug.Log("Ottiene la lingua attuale");*/ return g.savedLanguage; }

}

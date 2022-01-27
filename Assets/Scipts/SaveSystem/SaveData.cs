//Si occupa di salvare i dati di gioco

[System.Serializable]
public class SaveData
{
    //VALORI DI IMPOSTAZIONI----------------------------------------------------------------------------------------------------------------
    public float savedMasterVolume, //indica il valore del volume generale scelto dal giocatore l'ultima volta che è stato salvato
        savedMusicVolume, //indica il valore del volume della musica scelto dal giocatore l'ultima volta che è stato salvato
        savedSfxVolume; //indica il valore del volume degli effetti sonori scelto dal giocatore l'ultima volta che è stato salvato

    //indica la lingua che è stata messa l'ultima volta dal giocatore
    public int savedLanguage = 0;

    //VALORI DI GAMEPLAY--------------------------------------------------------------------------------------------------------------------
    //indica la stanza a cui il giocatore è arrivato
    public int lastEnteredRoom = 0;

    public SaveData(GameManag g)
    {
        //aggiorna i dati da salvare in base ai valori dentro GameManag
        savedMasterVolume = g.savedMasterVolume;
        savedMusicVolume = g.savedMusicVolume;
        savedSfxVolume = g.savedSfxVolume;
        savedLanguage = g.savedLanguage;
        lastEnteredRoom = g.lastEnteredRoom;

    }

}

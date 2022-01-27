//si occupa di tenere conto delle variabili salvate quando viene caricato il gioco
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManag : MonoBehaviour
{
    //indica se vogliamo caricare i dati ad inizio scena o meno(PER DEBUG, COMMENTARE A GIOCO FINITO)
    [SerializeField]
    private bool loadData = true;

    //VALORI DI IMPOSTAZIONI------------------------------------------------------------------------------------------------------------------
    public float savedMasterVolume = 0, //indica il valore del volume generale scelto dal giocatore l'ultima volta che è stato salvato
        savedMusicVolume = 0, //indica il valore del volume della musica scelto dal giocatore l'ultima volta che è stato salvato
        savedSfxVolume = -0; //indica il valore del volume degli effetti sonori scelto dal giocatore l'ultima volta che è stato salvato

    //indica la lingua che è stata messa l'ultima volta dal giocatore
    public int savedLanguage = 0;

    //VALORI DI GAMEPLAY----------------------------------------------------------------------------------------------------------------------
    //indica la stanza a cui il giocatore è arrivato
    public int lastEnteredRoom = 0;

    //riferimento a tutti gli script che usano l'interfaccia per l'aggiornamento dei dati nel GameManag
    public static List<IUpdateData> dataToSave = new List<IUpdateData>();


    private void Awake()
    {
        //se i dati stavano venendo cancellati, indica che il cancellamento è finito in quanto si stanno per caricare i dati
        //if (SaveSystem.isDeleting) { SaveSystem.isDeleting = false; }

        //carica i dati salvati
        if (loadData) OnGameLoad(SaveSystem.LoadGame());
        else { Debug.LogError("NON SONO STATI AGGIORNATI I DATI PERCHE' LOADDATA E' MESSO A FALSE"); }
        //dopo il caricamento dei dati controlla se gli array sono vuoti, nel qual caso li inizializza
        InizializeEmptyArrays();

    }

    private void Start()
    {
        //viene svuotata la lista di script che devono salvare i dati
        dataToSave.Clear();
        //viene creato un'array recipiente con tutti gli script che devono salvare dati(anche quelli inattivi)
        var recipient = FindObjectsOfType<MonoBehaviour>(true).OfType<IUpdateData>();
        //inizializza la lista di script che devono salvare i dati, aggiungendo tutti gli elementi nella lista recipiente
        foreach (IUpdateData elem in recipient) { dataToSave.Add(elem); }

    }

    /// <summary>
    /// Carica i dati salvati in SaveData
    /// </summary>
    /// <param name="sd"></param>
    public void OnGameLoad(SaveData sd)
    {
        //se esistono dati di salvataggio
        if (sd != null)
        {
            //aggiorna i dati in base ai dati salvati su SaveData
            savedMasterVolume = sd.savedMasterVolume;
            savedMusicVolume = sd.savedMusicVolume;
            savedSfxVolume = sd.savedSfxVolume;
            savedLanguage = sd.savedLanguage;
            lastEnteredRoom = sd.lastEnteredRoom;

            //Debug.Log("Caricati dati salvati");
        } //altrimenti, tutti i dati vengono messi al loro valore originale, in quanto non si è trovato un file di salvataggio
        else { DataErased(true); }

    }
    /// <summary>
    /// Riporta tutti i dati salvati al loro valore originale
    /// </summary>
    public void DataErased(bool totalErase)
    {
        //se bisogna cancellare tutti i dati...
        if (totalErase)
        {
            //...vengono riportate al loro valore originale anche i dati intoccabili
            savedMasterVolume = 0;
            savedMusicVolume = 0;
            savedSfxVolume = 0;
            savedLanguage = 0;
            //Debug.LogError("Cancellati anche dati intoccabili");
        }
        //else { Debug.LogError("Cancellati solo dati di gameplay"); }

        //cancella i dati di gameplay
        lastEnteredRoom = 0;

        //tutti gli array vengono svuotati
        EmptyArrays();

        //Debug.Log("Caricati dati iniziali");
    }
    /// <summary>
    /// Riporta tutti gli array al loro valore originale
    /// </summary>
    private void EmptyArrays()
    {
        //variabile di controllo che indicherà quanti cicli hanno fatto i cicli for sottostanti
        //int nControl = 0;

        //Debug.Log("Cicli fatti per i frammenti ottenuti: " + nControl); nControl = 0;
    }
    /// <summary>
    /// Controlla, per ogni array, se è nullo, nel qual caso inizializza l'array nel modo necessario
    /// </summary>
    private void InizializeEmptyArrays()
    {


    }
    /// <summary>
    /// Aggiorna i dati da salvare nel GameManag prima di salvare i dati
    /// </summary>
    private void UpdateDataBeforeSave()
    {
        //variabile di controllo, per vedere quante volte viene svolto il ciclo foreach
        int n = 0;
        //viene richiamata la funzione dell'interfaccia per aggiornare i dati di ogni elemento nella lista
        foreach (IUpdateData elem in dataToSave) { elem.UpdateData(); n++; }
        //Debug.LogWarning("Aggiornati dati nel GameManag. Il numero di elementi aggiornati sono: " + n);
    }
    /// <summary>
    /// Salva i dati dopo averli aggiornati
    /// </summary>
    public void SaveDataAfterUpdate()
    {
        //salva i dati ogni volta che si va da una scena all'altra, se i dati non stanno venendo cancellati...
        if (!SaveSystem.isDeleting)
        {
            //...aggiorna i dati se la scena non è un livello o, se lo è, se il livello è stato completato...
            UpdateDataBeforeSave();
            //...e salva i dati
            SaveSystem.DataSave(this);

            //Debug.Log("Dati aggiornati e salvati");
        }
        //else Debug.LogError("Dati non aggiornati, perchè stanno venendo cancellati");

    }
    /// <summary>
    /// Cancella tutti i dati(viene richiamato da un bottone nel menù principale)
    /// </summary>
    public void EraseAllData()
    {
        //cancella tutti i dati
        SaveSystem.ClearData(this, true);
        //carica la scena del menù principale
        SceneChange.StaticGoToScene("MainMenu");

        Debug.LogError("CANCELLATI TUTTI I DATI!");
    }

    private void OnDestroy()
    {
        //salva i dati di gioco dopo aver ottenuto gli aggiornamenti dalla lista di script che devono aggiornare i dati(solo se carica i dati)
        if (loadData) SaveDataAfterUpdate();
        //else { Debug.LogError("Dati non aggiornati, perchè non sono stati caricati i dati"); }

    }

}

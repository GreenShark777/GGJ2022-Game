//Si occupa di ciò che deve fare una stanza
using UnityEngine;

public class RoomsBehaviour : MonoBehaviour
{
    
    [SerializeField]
    private Transform entrance = default, //riferimento alla porta d'entrata della stanza
        enemiesContainer = default; //riferimento al contenitore dei nemici

    //riferimento alla porta che controlla se ci sono nemici nella stanza
    [SerializeField]
    private DoorsBehaviour checkingDoor = default;
    //indica l'ID di questa stanza(nonchè la sua posizione nell'array di stanze nel RoomsManager)
    private int roomID = -1;
    //indica il numero di nemici presenti in questa stanza
    private int enemiesInRoom = 0;


    private void Awake()
    {
        //informa i nemici della stanza in cui si trovano
        InformEnemiesOfTheirRoom();
        //la stanza viene disattivata, in modo che non sia visibile finchè il giocatore non ci entri
        gameObject.SetActive(false);

    }

    /// <summary>
    /// Imposta il nuovo ID di questa stanza
    /// </summary>
    /// <param name="newID"></param>
    public void SetRoomID(int newID) { roomID = newID; }
    /// <summary>
    /// Informa i nemici della stanza in cui sono e comunica il numero di nemici alla porta di controllo
    /// </summary>
    public void InformEnemiesOfTheirRoom()
    {
        //ottiene il riferimento a tutti gli script di vulnerabilità alla pietrificazione dei nemici nel contenitore
        var allEnemies = enemiesContainer.GetComponentsInChildren<PetrificationVulnerability>();
        //per ogni nemico nell'array appena ottenuto...
        foreach (PetrificationVulnerability enemy in allEnemies)
        {
            //...incrementa il numero di nemici presenti...
            enemiesInRoom++;
            //...e informa il nemico della stanza in cui si trova
            enemy.SetRoom(this);

        }
        //infine, indica alla porta il numero di nemici presenti nella stanza
        checkingDoor.SetNumberOfEnemies(enemiesInRoom);

    }
    /// <summary>
    /// Si occupa di ciò che deve accadere quando il giocatore entra in questa stanza
    /// </summary>
    /// <param name="player"></param>
    public void PlayerEntered(Transform player)
    {
        //riattiva questa stanza
        gameObject.SetActive(true);
        //porta il giocatore alla porta d'entrata di questa stanza
        player.position = entrance.position;
        //fa controllare alla porta di controllo se ci sono nemici nella stanza
        checkingDoor.CheckForEnemiesWithMasks();

    }
    /// <summary>
    /// Informa la porta di controllo che un nemico ha perso la maschera
    /// </summary>
    public void InformDoorOfMaskBreaking() { checkingDoor.RemovedMaskOfAnEnemy(); }
    /// <summary>
    /// Ritorna l'ID di questa stanza
    /// </summary>
    /// <returns></returns>
    public int GetRoomID() { return roomID; }

}

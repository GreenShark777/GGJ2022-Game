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


    private void Awake()
    {
        //informa i nemici della stanza in cui si trovano
        InformEnemiesOfTheirRoom();



        //DEBUG PER RICORDARE---------------------------------------------------------------------------------------------------------------
        InformDoorOfMaskBreaking();

    }

    /// <summary>
    /// Imposta il nuovo ID di questa stanza
    /// </summary>
    /// <param name="newID"></param>
    public void SetRoomID(int newID) { roomID = newID; }

    public void InformEnemiesOfTheirRoom()
    {

        //BISOGNA DARE AI NEMICI QUESTO SCRIPT COME RIFERIMENTO, IN MODO CHE QUANDO VENGONO SCONFITTI POSSANO DICHIARARLO ALLA STESSA STANZA
        //CHE A SUA VOLTA INFORMERA' LA PORTA DI CONTROLLO

        //var allEnemies = enemiesContainer.GetComponentsInChildren<PetrificationVulnerability>();

        Debug.LogWarning("DA RICORDARE: I NEMICI NON VENGONO ANCORA INFORMATI DELLA STANZA IN CUI SONO");

    }
    /// <summary>
    /// Si occupa di ciò che deve accadere quando il giocatore entra in questa stanza
    /// </summary>
    /// <param name="player"></param>
    public void PlayerEntered(Transform player)
    {
        //porta il giocatore alla porta d'entrata di questa stanza
        player.position = entrance.position;
        //fa controllare alla porta di controllo se ci sono nemici nella stanza
        checkingDoor.CheckForEnemiesWithMasks();

    }
    /// <summary>
    /// Informa la porta di controllo che un nemico ha perso la maschera
    /// </summary>
    public void InformDoorOfMaskBreaking()
    {
        
        checkingDoor.RemovedMaskOfAnEnemy();
        Debug.LogWarning("DA RICORDARE: LA PORTA DI COTROLLO NON VIENE ANCORA INFORMATA QUANDO UNA MASCHERA VIENE ROTTA");
    }
    /// <summary>
    /// Ritorna l'ID di questa stanza
    /// </summary>
    /// <returns></returns>
    public int GetRoomID() { return roomID; }

}

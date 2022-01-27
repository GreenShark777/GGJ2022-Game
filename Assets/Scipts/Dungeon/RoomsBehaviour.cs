//Si occupa di ciò che deve fare una stanza
using UnityEngine;

public class RoomsBehaviour : MonoBehaviour
{
    //riferimento alla porta d'entrata della stanza
    [SerializeField]
    private Transform entrance = default;
    //riferimento alla porta che controlla se ci sono nemici nella stanza
    [SerializeField]
    private DoorsBehaviour checkingDoor = default;
    //indica l'ID di questa stanza(nonchè la sua posizione nell'array di stanze nel RoomsManager)
    private int roomID = -1;


    /// <summary>
    /// Imposta il nuovo ID di questa stanza
    /// </summary>
    /// <param name="newID"></param>
    public void SetRoomID(int newID) { roomID = newID; }
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
    /// Ritorna l'ID di questa stanza
    /// </summary>
    /// <returns></returns>
    public int GetRoomID() { return roomID; }

}

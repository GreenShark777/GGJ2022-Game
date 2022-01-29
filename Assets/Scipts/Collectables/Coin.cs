//Si occupa del collezionabile a moneta
using UnityEngine;

public class Coin : MonoBehaviour
{
    //indica quanto questo cura il giocatore
    [SerializeField]
    private int points = 5;
    //riferimento al GameManag di scena
    [SerializeField]
    private GameManag g = default;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        //se si è colliso con il giocatore, lo si cura e questo collezionabile viene distrutto
        CollisionsManager cm = collision.GetComponent<CollisionsManager>();
        if (cm && cm.IsPlayer()) { g.points += points; Destroy(gameObject); }

    }

}

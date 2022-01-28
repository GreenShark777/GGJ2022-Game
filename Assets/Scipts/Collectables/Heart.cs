//Si occupa del collezionabile a cuore
using UnityEngine;

public class Heart : MonoBehaviour
{
    //indica quanto questo cura il giocatore
    [SerializeField]
    private int cureAmount = 20;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        //se si è colliso con il giocatore, lo si cura e questo collezionabile viene distrutto
        CollisionsManager cm = collision.GetComponent<CollisionsManager>();
        if (cm && cm.IsPlayer()) { cm.GetCured(cureAmount); Destroy(gameObject); }

    }

}

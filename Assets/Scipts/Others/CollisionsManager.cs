//Controlla le collisioni del gameObject di cui � componente
using UnityEngine;

public class CollisionsManager : MonoBehaviour
{
    //definisce un tipo di variabile per differenziare il tipo di entit� di cui si controllano le collisioni
    public enum CollisionType { enemy, player }
    //indica l'entit� di cui si controllano le collisioni
    [SerializeField]
    private CollisionType thisCollType = default;
    //riferimento al gameObject che ha lo script con l'interfaccia a cui si vuole far prendere danno
    [SerializeField]
    private GameObject objToGetInterfaceFrom = default;
    //riferimento allo script che si occupa della vita dell'entit�
    private IDamageable healthManager;


    private void Awake()
    {
        //ottiene il riferimento allo script che si occupa della vita dell'entit�
        healthManager = objToGetInterfaceFrom.GetComponent<IDamageable>();

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        //controlla la collisione appena subita
        CheckCollision(collision);

    }
    /// <summary>
    /// Controlla la collisione appena avvenuta
    /// </summary>
    /// <param name="coll"></param>
    private void CheckCollision(Collider2D coll)
    {
        //crea una variabile locale che indica se la collisione subita pu� far subire danni
        //bool isDmg =  coll.CompareTag("Damaging");

        //cerca di ottenere il riferimento all'interfaccia di danno dell'oggetto con cui si � colliso
        IGiveDamage dmgGiver = coll.GetComponent<IGiveDamage>();
        //crea una variabile locale che indica se quest'entit� � il giocatore
        bool isPlayer = (thisCollType == CollisionType.player);

        //se il riferimento all'interfaccia di danno esiste...
        if (/*isDmg*/dmgGiver != null)
        {
            //...se il danno proviene da un nemico e l'entit� � il giocatore o il danno non proviene da un nemico e l'entit� non � il giocatore...
            if (dmgGiver.IsEnemyOrTrapAttack() == isPlayer)
            {
                //...l'entit� subisce danni
                healthManager.TakeDmg(dmgGiver.GiveDamage());

            }
            /*
            //...prende il riferimento all'interfaccia di danno...
            IGiveDamage dmgGiver = coll.GetComponent<IGiveDamage>();
            //...se esiste il riferimento all'interfaccia di danno...
            if (dmgGiver != null)
            {
                //...se il danno proviene da un nemico e l'entit� � il giocatore o il danno non proviene da un nemico e l'entit� non � il giocatore...
                if (dmgGiver.IsEnemyAttack() == isPlayer)
                {
                    //...l'entit� subisce danni


                }

            }
            else { Debug.LogError(""); }
            */
        }

        //se quest'entit� � il giocatore...
        if (isPlayer)
        {
            //...esegue controlli specifici solo al giocatore

            //...infine, esce dalla funzione
            return;

        }
        //se quest'entit� � un nemico...
        if (thisCollType == CollisionType.enemy)
        {

            //...esegue controlli specifici solo ai nemici

        }

    }

    public bool IsPlayer() { return thisCollType == CollisionType.player; }

    public void GetCured(int amount) { healthManager.TakeDmg(-amount); }

    private void OnDestroy()
    {

        if (gameObject) { Destroy(gameObject); }

    }

}

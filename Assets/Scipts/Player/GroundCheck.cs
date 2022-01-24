//Controlla se viene toccata terra o meno
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    //riferimento al gameObject che ha lo script con l'interfaccia che che si vuole informare
    [SerializeField]
    private GameObject objToGetInterfaceFrom = default;
    //riferimento allo script da informare quando si tocca terra
    private INeedGroundCheck toInform;


    private void Awake()
    {
        //ottiene il riferimento allo script da informare quando si tocca terra
        toInform = objToGetInterfaceFrom.GetComponent<INeedGroundCheck>();

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //se si tocca il terreno, comunica allo script da informare che si è toccata terra
        if (collision.CompareTag("Ground")) { toInform.HasLanded(); }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //se non si tocca più il terreno, comunica allo script da informare che non si è più a terra
        if (collision.CompareTag("Ground")) { toInform.HasLanded(false); }

    }

}

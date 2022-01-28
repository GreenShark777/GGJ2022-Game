//Si occupa della camera e di dove deve essere
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    //riferimento all'oggetto da seguire
    [SerializeField]
    private Transform toFollow = default;

    [SerializeField]
    private float followSpeed = 1, //indica quanto velocemente la telecamera segue l'oggetto da seguire
        camYPosOffset = 0; //indica di quanto su o giù rispetto all'oggetto da seguire deve essere la camera

    //indica la posizione Z della camera
    private float camZPos;


    private void Start()
    {
        //ottiene la posizione Z della camera
        camZPos = transform.position.z;

    }

    private void FixedUpdate()
    {
        //calcola la posizione in cui deve essere la camera per seguire il giocatore
        Vector3 newCamPosition = Vector2.Lerp(transform.position, toFollow.position, followSpeed * Time.deltaTime);
        //aggiunge la posizione di offset nella Y alla posizione calcolata
        newCamPosition.y += camYPosOffset;
        //viene messa la posizione Z della camera alla posizione calcolata
        newCamPosition.z = camZPos;
        //la posizione della camera viene impostata a quella calcolata
        transform.position = newCamPosition;
    }

    // Set toFollow target
    public void SetTarget(Transform newTarget)
    {
        if (newTarget)
            toFollow = newTarget; 
    }
}

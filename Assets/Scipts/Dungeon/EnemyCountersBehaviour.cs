//Si occupa di cosa devono fare i contatori di nemici
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCountersBehaviour : MonoBehaviour
{
    //riferimento allo SpriteRenderer di questo contatore
    private SpriteRenderer sr;
    //riferimento al colore da avere da attivato
    [SerializeField]
    private Sprite activatedSprite = default;


    private void Awake()
    {
        //ottiene il riferimento allo SpriteRenderer di questo contatore
        sr = GetComponent<SpriteRenderer>();

    }

    public void ActivateThis()
    {
        //cambia lo sprite con quello da attivo
        sr.sprite = activatedSprite;

    }

}

//Si occupa del comportamento delle porte nelle stanze
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorsBehaviour : MonoBehaviour
{
    //riferimento al contenitore dei contatori di nemici
    private Transform countersContainer;
    //array di riferimenti agli script dei contatori di nemici
    private EnemyCountersBehaviour[] allCounters;
    //riferimento al collider di questa porta
    private Collider2D doorColl;
    //riferimento al contenitore di tutti i nemici nella stanza con maschere
    [SerializeField]
    private Transform enemiesContainer = default;
    //riferimento all'Animator del testo che indica che tutte le maschere sono state distrutte
    [SerializeField]
    private Animator adviseTextAnim = default;
    //indica il numero di nemici presenti inizialmente nella stanza
    private int nEnemies;
    //indica se in questa stanza questa è la porta che controlla i nemici
    [SerializeField]
    private bool checkingDoor = false;
    //indica il numero di nemici a cui è stata tolta la maschera in questa stanza
    private int removedMasks = 0;


    void Start()
    {
        //se questa è la porta che deve controllare i nemici nella stanza...
        if (checkingDoor)
        {
            //...ottiene il numero di nemici presenti inizialmente nella stanza...
            nEnemies = enemiesContainer.childCount;
            //...ottiene il riferimento al contenitore dei contatori di nemici...
            countersContainer = transform.GetChild(0);
            //...riempe l'array di contatori di nemici prendendo dal contenitore
            allCounters = countersContainer.GetComponentsInChildren<EnemyCountersBehaviour>();
            //...infine, ottiene il riferimento al collider di questa porta
            doorColl = GetComponent<Collider2D>();



            //DEBUG-------------------------------------------------------------------------------------------------------------------------
            //se il numero di nemici non è uguale al numero di contatori, comunica l'errore
            if (nEnemies != countersContainer.childCount)
            { Debug.LogError("IL NUMERO DI NEMICI NON E' UGUALE AL NUMERO DI CONTATORI PER LA STANZA: " + transform.parent.name); }

        }

    }

    private void Update()
    {
        //DEBUG-----------------------------------------------------------------------------------------------------------------------------
        if (Input.GetKeyDown(KeyCode.F1)) { RemovedMaskOfAnEnemy(); }
        //DEBUG-----------------------------------------------------------------------------------------------------------------------------
    }

    /// <summary>
    /// Controlla se ci sono nemici presenti con maschere e chiude o apre la porta di conseguenza
    /// </summary>
    public void CheckForEnemiesWithMasks()
    {
        //se esistono dei nemici nella stanza con ancora la maschera...
        if ((nEnemies - removedMasks) > 0)
        {
            //...la porta si chiude(se non lo è già)
            /*if (doorColl.enabled)*/ doorColl.enabled = false;

        } //altrimenti...
        else /*if (!doorColl.enabled)*/
        {
            //...la porta si apre...
            doorColl.enabled = true;
            //...e fa partire l'animazione del testo che indica al giocatore che può andare avanti
            adviseTextAnim.SetTrigger("Advise");
        
        }

    }
    /// <summary>
    /// Attiva un contatore di nemici e controlla se è stata completata la stanza
    /// </summary>
    public void RemovedMaskOfAnEnemy()
    {
        //se questa è la porta che deve controllare il completamento della stanza...
        if (checkingDoor)
        {
            //...incrementa il numero di maschere rimosse...
            removedMasks++;
            //...attiva il contatore corrispondente...
            allCounters[removedMasks - 1].ActivateThis();
            //...e controlla se ci sono ancora nemici con la maschera
            CheckForEnemiesWithMasks();

        }
        else { Debug.LogError("Chiamato controllo dalla porta sbagliata! Porta: " + name + " della stanza : " + transform.parent.name); }

    }

}

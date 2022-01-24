using System;
using System.Collections;
using UnityEngine;

public class IA : MonoBehaviour
{
    [SerializeField] private Transform[] checkpoints;   // Array di checkpoint che l'IA deve seguire
    [SerializeField] private float pauseAtCheckpoint = .5f; // Pausa ad ogni checkpoint
    [SerializeField] private float checkMinDist = .2f;  // Distanza minima prima di poter assicurare di aver raggiunto il checkpoint
    
    private int index;  // Indice nell'array dei checkpoint da raggiungere
    private bool indexIncrease; // Indica se l'indice è crescente o decrescente
    private bool isMoving;  // Se true passa il movimento

    private Vector2 movement;   // Vettore di movimento target

    private CharacterMovement characterMovement;    // Reference allo script del movimento
    
    private void Start()
    {
        characterMovement = GetComponent<CharacterMovement>();
        
        // Se ci sono checkpoint, inizia il movimento
        isMoving = checkpoints.Length > 0;

        // Parti dal primo indice
        index = 0;
        indexIncrease = true;
    }

    private void Update()
    {
        if (isMoving)
        {
            // Aggiorna il vettore di movimento
            CheckMovement();
            // E passalo al componente
            characterMovement.Move(movement);
        }
    }

    /// <summary>
    /// Attendi prima di poter muovere nuovamente
    /// </summary>
    /// <returns></returns>
    private IEnumerator WaitToMove()
    {
        yield return new WaitForSeconds(pauseAtCheckpoint);
        isMoving = true;
    }

    /// <summary>
    /// Effettua i controlli sulla distanza ai checkpoint e aggiorna il vettore di movimento
    /// </summary>
    private void CheckMovement()
    {
        // TODO: Check diff for different move types
        Vector2 diff = checkpoints[index].position - transform.position;

        // Se sull'asse x sei abbastanza vicino al checkpoint
        if (Mathf.Abs(diff.x) <= checkMinDist)
        {
            // Aggiorna l'indice
            index = indexIncrease ? index + 1 : index - 1;
            
            if (index >= checkpoints.Length)
            {
                index -= 2;
                indexIncrease = false;
            } else if (index < 0)
            {
                index += 2;
                indexIncrease = true;
            }
            
            // Effettua la pausa
            isMoving = false;

            StartCoroutine(WaitToMove());
        }
        
        // Normalizza il movimento per non essere troppo elevato
        movement = diff.normalized;
        movement.y = 0;
    }
}
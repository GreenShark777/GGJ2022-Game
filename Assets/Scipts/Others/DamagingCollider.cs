//Script generico per tutti i collider che devono fare danno a giocatore o altri elementi di gioco
using UnityEngine;

public class DamagingCollider : MonoBehaviour, IGiveDamage
{
    //indica se questo collider danneggiante appartiene ad una trappola ad un nemico(se false, appartiene al giocatore)
    [SerializeField]
    private bool enemyOrTrapAttack = true;
    //indica quanto danno questo collider infligge agli elementi che prendono danni
    [SerializeField]
    private int dmg = default;


    private void Awake()
    {

        if (dmg <= 0) { Debug.LogWarning("Questo collider di danno fa 0 o meno danni: " + name + ".\nE' voluto?"); }
        
    }

    /// <summary>
    /// Permette di impostare il danno che deve fare questo collider
    /// </summary>
    /// <param name="newDmg"></param>
    public void SetNewDamage(int newDmg) { dmg = newDmg; }

    //INTERFACCIA-------------------------------------------------------------------------------------------------------------
    public int GiveDamage() { return dmg; }
    public bool IsEnemyOrTrapAttack() { return enemyOrTrapAttack; }

}

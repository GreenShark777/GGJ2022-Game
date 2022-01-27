using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CharacterHealth : MonoBehaviour, IDamageable
{
    [Tooltip("Reference to the helth slider used by this character")]
    [SerializeField] private Slider healthSlider;
    
    [Range(0, 100)] [SerializeField] private int maxHealth;
    private int currentHealth;

    public UnityAction onDeath;

    //riferimento allo script di vulnerabilità alla pietrificazione, se questa vita riguarda un nemico
    [SerializeField]
    public PetrificationVulnerability pv = default;
    //indica se quest'entità ha perso tutta la vita
    private bool lostAllHealth = false;


    void Start()
    {
        currentHealth = maxHealth;
        UpdateSlider();

        /*
         * ottiene il riferimento alla funzione da cambiare quando finisce la vita
         * se non è un nemico sarà il giocatore, quindi prende come riferimento la funzione per indicare che il giocatore è morto
         * altrimenti, essendo nemico, gli toglie la maschera e lo rende vulnerabile alla pietrificazione
        */
        onDeath += (!pv) ? PlayerStateManager.PlayerDied : pv.BreakMask;

    }

    /// <summary>
    /// Update the slider value
    /// </summary>
    private void UpdateSlider() { healthSlider.value = (float)currentHealth / (float)maxHealth; }

    // IDamageable   - - -

    /// <summary>
    /// Riceve il valore di danno da sottrarre alla currentHealth
    /// </summary>
    /// <param name="dmgValue">value to be removed from currentHealth</param>
    public void TakeDmg(int dmgValue) {
        // TODO: modifiers on dmg?

        // Evitiamo che il valore della vita esca dai limiti
        currentHealth = Mathf.Clamp(currentHealth - dmgValue, 0, maxHealth);

        // Dopo aver calcolato il nuovo valore, aggiorniamolo a schermo 
        UpdateSlider();

        // Se la vita scende a 0 per la prima volta...
        if (currentHealth < 1 && !lostAllHealth)
        {
            //...richiama la funzione che gestisce la fine della vita di quest'entità...
            onDeath?.Invoke();
            //...e comunica che si è persa tutta la vita
            lostAllHealth = true;

        }
    }

    private void OnDestroy()
    {

        onDeath -= onDeath;

    }

}

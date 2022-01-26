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

    void Start()
    {
        currentHealth = maxHealth;
        UpdateSlider();
    }

    /// <summary>
    /// Update the slider value
    /// </summary>
    private void UpdateSlider()
    {
        healthSlider.value = (float)currentHealth / (float)maxHealth;
    }

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

        // Se la vita scende a 0...
        if(currentHealth < 1)
        {
            onDeath?.Invoke();
        }
    }
}

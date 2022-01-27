using System.Collections;
using UnityEngine;

public class FlyingEnemy : IA
{

    [Space]
    [Header("Flying enemy settings")]
    [SerializeField] private float beforeAttackTime = 2f; // Quanto tempo aspettare prima di iniziare la picchiata verso il player
    [SerializeField] private float attackSpeed = 5f; // Velocità utilizzata durante l'attacco
    [SerializeField] private float stunTime = 3f; // Tempo prima di riprendersi dopo aver sbattuto
    [SerializeField] private float playerAttackOffset = 1f; // Di quanto spostare la posizione d'attacco "dietro" il player

    private Vector3 positionBeforeAttack;
    private Vector3 attackPosition;
    private bool isAttacking = false;
    private bool isResetting = false;

    Vector2 diff;

    protected override void PlayerEnteredRange()
    {
        base.PlayerEnteredRange();

        if (isAttacking) return;

        positionBeforeAttack = transform.position;

        isAttacking = false;
        attackCollider.enabled = false;
        isComputing = true;

        StartCoroutine(WaitBeforeAttack());
    }

    /// <summary>
    /// Se il player esce dal range mentre attacco, non succede nulla, arrivato in posizione il mostro ritornerà in patrol
    /// </summary>
    protected override void PlayerExitRange() { }

    protected override void HierarchyUpdate()
    {
        if(isAttacking)
        {
            Vector2 diff = attackPosition - transform.position;
            characterMovement.Move(diff.normalized, attackSpeed);

            // Ho raggiunto il punto in cui volevo arrivare
            if(diff.magnitude <= .4f)
            {
                isAttacking = false;
                attackCollider.enabled = false;
                ResetPosition();
            }
        }
        else if (isResetting)
        {
            diff = positionBeforeAttack - transform.position;
            characterMovement.Move(diff.normalized);

            // Ho raggiunto il punto in cui volevo arrivare
            if (diff.magnitude <= .4f)
            {
                isResetting = false;

                if (isPlayerInRange)
                    StartCoroutine(WaitBeforeAttack());
                else
                {
                    isPatrolMoving = true;
                    isAttacking = false;
                    isComputing = false;
                }
            }
        }
    }

    /// <summary>
    /// Dopo l'attacco ritorno alla posizione del patrol
    /// </summary>
    private void ResetPosition()
    {
        isResetting = true;
    }

    IEnumerator WaitBeforeAttack()
    {
        attackPosition = playerTransform.position + (playerTransform.position - transform.position).normalized * playerAttackOffset;

        // Aspettiamo tot secondi prima di attaccare
        yield return new WaitForSeconds(beforeAttackTime);

        isAttacking = true;
        attackCollider.enabled = true;
    }

    IEnumerator WaitStunTime()
    {
        // Aspettiamo tot secondi prima di attacre
        yield return new WaitForSeconds(stunTime);

        ResetPosition();
    }

    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPosition, .5f);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(positionBeforeAttack, .5f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            isAttacking = false;
            attackCollider.enabled = false;
            StartCoroutine(WaitStunTime());
        }
    }
}
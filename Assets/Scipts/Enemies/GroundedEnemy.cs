using System.Collections;
using UnityEngine;

public class GroundedEnemy : IA
{
    
    [SerializeField]
    private SpriteAnimationManager enemySam = default;

    [Space]
    [Header("Grounded enemy settings")]
    [SerializeField] private float attackRange = 2f; // Distanza minima affinchè il nemico possa attaccare il player
    [SerializeField] private float attackCooldown = 1f; // Tempo necessario affinchè il nemico possa attaccare nuovamente
    [SerializeField] private float followSpeed = 7f; // Velocità utilizzata per avvicinarsi al player

    private Vector3 positionBeforeAttack;
    private bool isFollowing = false;
    private bool isResetting = false;

    protected override void PlayerEnteredRange()
    {
        base.PlayerEnteredRange();

        StartFollowing();
    }

    /// <summary>
    /// Se il player esce dal range mentre attacco, non succede nulla, arrivato in posizione il mostro ritornerà in patrol
    /// </summary>
    protected override void PlayerExitRange() 
    {
        if (isResetting) return;

        if(isFollowing)
        {
            isFollowing = false;
            ResetPosition();
        }
    }

    protected override void HierarchyUpdate()
    {
        if (isFollowing)
        {
            Vector2 diff = playerTransform.position - transform.position;
            diff.y = 0; // I nemici grounded non utilizzano y
            characterMovement.Move(diff.normalized, followSpeed);

            // Ho raggiunto il punto in cui volevo arrivare
            if (diff.magnitude <= attackRange)
            {
                StopAllCoroutines();
                StartCoroutine(Attack());
            }
        }
        else if (isResetting)
        {
            Vector2 diff = positionBeforeAttack - transform.position;
            diff.y = 0;
            characterMovement.Move(diff.normalized);

            // Ho raggiunto il punto in cui volevo arrivare
            if (diff.magnitude <= .4f)
            {
                isResetting = false;

                if (isPlayerInRange)
                    StartFollowing();
                else
                {
                    isPatrolMoving = true;
                    isComputing = false;
                }
            }
        }
    }

    private void StartFollowing()
    {
        positionBeforeAttack = transform.position;
        isFollowing = true;
        isComputing = true;
    }

    IEnumerator Attack()
    {

        enemySam.StartNewAnimation(1, 24, 47);

        isFollowing = false;
        attackCollider.enabled = true;

        yield return new WaitForSeconds(attackCooldown);

        attackCollider.enabled = false;

        if (isPlayerInRange)
        {
            isFollowing = true;
        }
        else
        {
            ResetPosition();
        }
    }

    /// <summary>
    /// Dopo l'attacco ritorno alla posizione del patrol
    /// </summary>
    private void ResetPosition()
    {
        isResetting = true;
    }

    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(positionBeforeAttack, .5f);
    }
}
using UnityEngine;
using UnityEngine.AI;

public class Walker : MonoBehaviour
{
    [SerializeField] private float detectionRadius = 10f;
    [SerializeField] private float fieldOfViewAngle = 45f;
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float stoppingDistance = 1.5f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float closeRange = 3f;
    [SerializeField] private float attackRange = 1f; // Distance at which Walker attacks
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private NavMeshAgent _navMeshAgent;

    private Transform targetPlayer;
    private Animator _animator;
    private bool isChasing = false;
    private bool isRunning = false;
    private bool isAttacking = false;

    private void Awake()
    {
        _navMeshAgent.speed = moveSpeed;
        _navMeshAgent.stoppingDistance = stoppingDistance;
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        DetectPlayer();

        if (isChasing && targetPlayer != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, targetPlayer.position);

            if (distanceToPlayer <= attackRange)
            {
                AttackPlayer();
            }
            else
            {
                isAttacking = false;
                _navMeshAgent.isStopped = false;

                if (distanceToPlayer <= closeRange)
                {
                    FacePlayerInstantly(); // Always face the player if very close
                }
                else
                {
                    RotateTowardsPlayer(); // Smoothly rotate if farther away
                }

                _navMeshAgent.SetDestination(targetPlayer.position);

                if (!isRunning)
                {
                    _animator.SetTrigger("Running");
                    isRunning = true;
                }
            }
        }
        else
        {
            if (isRunning)
            {
                _animator.SetTrigger("Idle");
                isRunning = false;
            }
        }
    }

    private void DetectPlayer()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius, playerLayer);

        foreach (var hit in hits)
        {
            Vector3 directionToPlayer = (hit.transform.position - transform.position).normalized;
            float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);
            float distanceToPlayer = Vector3.Distance(transform.position, hit.transform.position);

            if (angleToPlayer < fieldOfViewAngle && distanceToPlayer <= detectionRadius)
            {
                targetPlayer = hit.transform;
                isChasing = true;
                return;
            }
        }

        isChasing = false;
        targetPlayer = null;
    }

    private void RotateTowardsPlayer()
    {
        if (targetPlayer == null) return;

        Vector3 direction = (targetPlayer.position - transform.position).normalized;
        direction.y = 0;

        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }

    private void FacePlayerInstantly()
    {
        if (targetPlayer == null) return;

        Vector3 direction = (targetPlayer.position - transform.position).normalized;
        direction.y = 0;

        transform.rotation = Quaternion.LookRotation(direction);
    }

    private void AttackPlayer()
    {
        if (!isAttacking)
        {
            isAttacking = true;
            _navMeshAgent.isStopped = true; // Stop movement while attacking
            _animator.SetTrigger("Attack");
        }
    }
}

using UnityEngine;
using UnityEngine.AI;

public class Walker : MonoBehaviour
{
    [SerializeField] private float detectionRadius = 10f;
    [SerializeField] private float fieldOfViewAngle = 45f;
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float stoppingDistance = 1.5f;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private NavMeshAgent _navMeshAgent;

    private Transform targetPlayer;
    private Animator _animator;
    private bool isChasing = false;
    private bool isRunning = false; 

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
            _navMeshAgent.SetDestination(targetPlayer.position);

            if (!isRunning) // Only set animation if not already running
            {
                _animator.SetTrigger("Running");
                isRunning = true;
            }
        }
        else
        {
            if (isRunning) // Only reset if it was running before
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

            if (angleToPlayer < fieldOfViewAngle)
            {
                targetPlayer = hit.transform;
                isChasing = true;
                return;
            }
        }

        isChasing = false;
        targetPlayer = null;
    }
}
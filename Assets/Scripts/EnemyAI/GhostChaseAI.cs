using UnityEngine;
using UnityEngine.AI;

// Gắn script này vào GameObject "creepy+ghost+girl+3d+model"
// Yêu cầu: GameObject phải có component NavMeshAgent
// Scene phải được Bake NavMesh (Window > AI > Navigation > Bake)
[RequireComponent(typeof(NavMeshAgent))]
public class GhostChaseAI : MonoBehaviour
{
    [Header("Tham chiếu")]
    public Transform player;              // Kéo thả nhân vật chính vào đây
    public JumpscareController jumpscare; // Kéo thả object quản lý jumpscare vào đây

    [Header("Thông số đuổi theo")]
    public float detectRange = 15f;       // Khoảng cách ma phát hiện người chơi
    public float catchRange = 1.2f;       // Khoảng cách ma bắt được người chơi
    public float chaseSpeed = 3.5f;
    public float walkSpeed = 1.5f;        // Tốc độ khi chưa phát hiện (đi lang thang)

    [Header("Đi lang thang (Idle Patrol)")]
    public float wanderRadius = 10f;
    public float wanderInterval = 5f;

    private NavMeshAgent agent;
    private Animator animator;            // Nếu model có Animator (Walk/Run/Idle)
    private float wanderTimer;
    private bool isChasing = false;
    private bool hasCaught = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }

        agent.speed = walkSpeed;
    }

    void Update()
    {
        if (hasCaught || player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        // Bắt được người chơi
        if (distance <= catchRange)
        {
            CatchPlayer();
            return;
        }

        // Phát hiện người chơi trong tầm -> đuổi theo
        if (distance <= detectRange)
        {
            isChasing = true;
            agent.speed = chaseSpeed;
            agent.SetDestination(player.position);
        }
        else
        {
            isChasing = false;
            agent.speed = walkSpeed;
            Wander();
        }

        UpdateAnimator();
    }

    void Wander()
    {
        wanderTimer += Time.deltaTime;
        if (wanderTimer >= wanderInterval || (!agent.pathPending && agent.remainingDistance < 0.5f))
        {
            wanderTimer = 0f;
            Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
            randomDirection += transform.position;

            if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, wanderRadius, NavMesh.AllAreas))
            {
                agent.SetDestination(hit.position);
            }
        }
    }

    void UpdateAnimator()
    {
        if (animator == null) return;
        // Đổi tên tham số cho khớp với Animator Controller của bạn
        animator.SetBool("IsChasing", isChasing);
        animator.SetFloat("Speed", agent.velocity.magnitude);
    }

    void CatchPlayer()
    {
        hasCaught = true;
        agent.isStopped = true;

        if (jumpscare != null)
        {
            jumpscare.TriggerJumpscare();
        }

        // Tuỳ chọn: khoá điều khiển người chơi khi bị bắt
        var playerMovement = player.GetComponent<MonoBehaviour>();
        // Nếu bạn có script di chuyển riêng, có thể disable nó ở đây, ví dụ:
        // player.GetComponent<PlayerMovement>().enabled = false;
    }

    // Vẽ vùng phát hiện trong Scene view để dễ chỉnh
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, catchRange);
    }
}

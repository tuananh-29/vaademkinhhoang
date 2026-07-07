using UnityEngine;

/// <summary>
/// Đọc trạng thái từ PlayerController và cập nhật Animator tương ứng.
/// Gắn script này vào cùng object với Animator (thường là BroModel).
/// </summary>
[RequireComponent(typeof(Animator))]
public class PlayerAnimatorController : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private Animator animator;

    // Phải khớp CHÍNH XÁC tên parameter trong Animator (đang là "isRuning")
    private const string IS_RUNNING_PARAM = "isRuning";

    private void Awake()
    {
        if (animator == null) animator = GetComponent<Animator>();
        if (playerController == null) playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (playerController == null || animator == null) return;

        // Kích hoạt animation "chạy" khi nhân vật di chuyển (đi bộ HOẶC chạy),
        // vì hiện chưa có animation Walk riêng.
        animator.SetBool(IS_RUNNING_PARAM, playerController.IsMoving);

        // Đi bộ: phát animation chậm hơn. Chạy: phát animation tốc độ bình thường.
        // Giúp phân biệt trực quan giữa đi bộ và chạy dù dùng chung 1 clip.
        if (playerController.IsMoving)
        {
            animator.speed = playerController.IsRunning
                ? 1f
                : Mathf.Clamp(playerController.WalkSpeed / playerController.RunSpeed, 0.5f, 1f);
        }
        else
        {
            animator.speed = 1f; // Idle luôn phát bình thường
        }
    }
}
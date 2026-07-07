using UnityEngine;

/// <summary>
/// Điều khiển di chuyển nhân vật góc nhìn thứ nhất (FPS).
/// Gắn script này vào GameObject Player có component CharacterController.
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Tốc độ di chuyển")]
    [SerializeField] private float walkSpeed = 3.5f;
    [SerializeField] private float runSpeed = 6f;
    [SerializeField] private float crouchSpeed = 1.8f;

    [Header("Nhảy & Trọng lực")]
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private bool allowJump = false; // Game kinh dị thường không cần nhảy
    [SerializeField] private float jumpHeight = 1.2f;

    [Header("Ngồi (Crouch)")]
    [SerializeField] private float standingHeight = 1.8f;
    [SerializeField] private float crouchingHeight = 1.0f;
    [SerializeField] private float crouchTransitionSpeed = 8f;

    [Header("Stamina (chạy)")]
    [SerializeField] private float maxStamina = 100f;
    [SerializeField] private float staminaDrainRate = 20f;   // mỗi giây khi chạy
    [SerializeField] private float staminaRegenRate = 15f;   // mỗi giây khi không chạy
    [SerializeField] private float minStaminaToRun = 5f;

    [Header("Tham chiếu")]
    [SerializeField] private Transform cameraHolder; // Camera con của Player

    private CharacterController controller;
    private Vector3 velocity;
    private bool isCrouching;
    private bool isRunning;
    private float currentStamina;
    private float targetHeight;

    // Cho phép các script khác (VD: hệ thống phát hiện của AI) đọc trạng thái
    public bool IsCrouching => isCrouching;
    public bool IsRunning => isRunning;
    public bool IsMoving { get; private set; }
    public float CurrentStamina => currentStamina;
    public float MaxStamina => maxStamina;
    public float CurrentSpeed { get; private set; }
    public float WalkSpeed => walkSpeed;
    public float RunSpeed => runSpeed;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        currentStamina = maxStamina;
        targetHeight = standingHeight;
        controller.height = standingHeight;
    }

    private void Update()
    {
        HandleCrouchInput();
        HandleCrouchHeight();
        HandleMovement();
        HandleGravity();
    }

    private void HandleCrouchInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.C))
        {
            isCrouching = !isCrouching;
            targetHeight = isCrouching ? crouchingHeight : standingHeight;
        }
    }

    private void HandleCrouchHeight()
    {
        // Chuyển chiều cao mượt mà thay vì đổi ngay lập tức
        controller.height = Mathf.Lerp(controller.height, targetHeight, Time.deltaTime * crouchTransitionSpeed);

        if (cameraHolder != null)
        {
            Vector3 camPos = cameraHolder.localPosition;
            float targetCamY = targetHeight - 0.2f;
            camPos.y = Mathf.Lerp(camPos.y, targetCamY, Time.deltaTime * crouchTransitionSpeed);
            cameraHolder.localPosition = camPos;
        }
    }

    private void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        IsMoving = horizontal != 0 || vertical != 0;

        bool wantsToRun = Input.GetKey(KeyCode.LeftShift) && !isCrouching;
        isRunning = wantsToRun && currentStamina > minStaminaToRun && IsMoving;

        // Cập nhật stamina
        if (isRunning)
        {
            currentStamina -= staminaDrainRate * Time.deltaTime;
            currentStamina = Mathf.Max(currentStamina, 0f);
        }
        else
        {
            currentStamina += staminaRegenRate * Time.deltaTime;
            currentStamina = Mathf.Min(currentStamina, maxStamina);
        }

        float speed = isCrouching ? crouchSpeed : (isRunning ? runSpeed : walkSpeed);
        CurrentSpeed = speed;

        Vector3 move = transform.right * horizontal + transform.forward * vertical;
        move = Vector3.ClampMagnitude(move, 1f); // tránh di chuyển chéo nhanh hơn

        controller.Move(move * speed * Time.deltaTime);
    }

    private void HandleGravity()
    {
        bool isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // giữ nhân vật bám đất
        }

        if (allowJump && isGrounded && Input.GetButtonDown("Jump") && !isCrouching)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
using UnityEngine;

/// <summary>
/// Điều khiển góc nhìn camera theo chuột (First Person).
/// Gắn script này vào Camera (là object con của Player).
/// Player (object cha) sẽ xoay trái/phải, còn Camera chỉ xoay lên/xuống.
/// </summary>
public class MouseLook : MonoBehaviour
{
    [Header("Độ nhạy chuột")]
    [SerializeField] private float mouseSensitivity = 150f;

    [Header("Giới hạn góc nhìn lên/xuống")]
    [SerializeField] private float minPitch = -80f;
    [SerializeField] private float maxPitch = 80f;

    [Header("Tham chiếu")]
    [SerializeField] private Transform playerBody; // object cha (Player)

    private float pitch = 0f; // góc xoay lên/xuống hiện tại

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Xoay camera lên/xuống (pitch), có giới hạn để tránh lộn ngược
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
        transform.localRotation = Quaternion.Euler(pitch, 0f, 0f);

        // Xoay cả thân người trái/phải (yaw)
        if (playerBody != null)
        {
            playerBody.Rotate(Vector3.up * mouseX);
        }
    }

    /// <summary>Gọi hàm này khi mở menu Pause để trả lại con trỏ chuột.</summary>
    public void SetCursorLocked(bool locked)
    {
        Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !locked;
    }
}

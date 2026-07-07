using UnityEngine;

/// <summary>
/// Gắn script này vào Main Camera (bên trong BroModel).
/// Bắn raycast từ giữa màn hình ra phía trước để phát hiện vật thể có thể tương tác.
/// </summary>
public class PlayerInteractor : MonoBehaviour
{
    [Header("Cài đặt tương tác")]
    [SerializeField] private float interactRange = 3f;
    [SerializeField] private LayerMask interactableLayers = ~0; // Mặc định: mọi layer
    [SerializeField] private KeyCode interactKey = KeyCode.E;

    [Header("Tham chiếu")]
    [SerializeField] private GameObject player; // Object chứa InventoryManager (thường là BroModel)

    // UI có thể lắng nghe biến này để hiển thị dòng gợi ý ("Nhấn E để nhặt...")
    public string CurrentPrompt { get; private set; } = "";

    private IInteractable currentInteractable;

    private void Update()
    {
        DetectInteractable();

        if (currentInteractable != null && Input.GetKeyDown(interactKey))
        {
            currentInteractable.Interact(player != null ? player : gameObject);
        }
    }

    private void DetectInteractable()
    {
        Ray ray = new Ray(transform.position, transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, interactRange, interactableLayers))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();
            currentInteractable = interactable;
            CurrentPrompt = interactable != null ? interactable.GetInteractPrompt() : "";
        }
        else
        {
            currentInteractable = null;
            CurrentPrompt = "";
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * interactRange);
    }
}

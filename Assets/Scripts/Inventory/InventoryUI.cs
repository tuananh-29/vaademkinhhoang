using System.Text;
using UnityEngine;
using TMPro;

/// <summary>
/// Hiển thị túi đồ dạng danh sách text đơn giản (dùng TextMeshPro).
/// Gắn script này vào 1 GameObject UI, kéo các Text (TMP) tương ứng vào field bên dưới.
/// </summary>
public class InventoryUI : MonoBehaviour
{
    [SerializeField] private InventoryManager inventoryManager;
    [SerializeField] private TMP_Text inventoryText;   // Text hiển thị danh sách vật phẩm
    [SerializeField] private TMP_Text promptText;      // Text hiển thị "Nhấn E để nhặt..."
    [SerializeField] private PlayerInteractor playerInteractor; // Để đọc CurrentPrompt

    [SerializeField] private GameObject inventoryPanel; // Panel chứa toàn bộ UI túi đồ (ẩn/hiện theo Tab)

    private void OnEnable()
    {
        if (inventoryManager != null)
        {
            inventoryManager.OnInventoryChanged += RefreshUI;
        }
    }

    private void OnDisable()
    {
        if (inventoryManager != null)
        {
            inventoryManager.OnInventoryChanged -= RefreshUI;
        }
    }

    private void Update()
    {
        // Cập nhật dòng gợi ý tương tác mỗi frame (VD: "Nhấn E để nhặt Đèn pin")
        if (promptText != null && playerInteractor != null)
        {
            promptText.text = playerInteractor.CurrentPrompt;
            promptText.gameObject.SetActive(!string.IsNullOrEmpty(playerInteractor.CurrentPrompt));
        }

        if (inventoryPanel != null && inventoryManager != null)
        {
            inventoryPanel.SetActive(inventoryManager.IsInventoryOpen);
        }
    }

    private void RefreshUI()
    {
        if (inventoryText == null || inventoryManager == null) return;

        StringBuilder sb = new StringBuilder();
        sb.AppendLine("TÚI ĐỒ (Tab để đóng/mở, 1-9 để chọn, F để dùng)");
        sb.AppendLine();

        var slots = inventoryManager.Slots;
        for (int i = 0; i < slots.Count; i++)
        {
            string prefix = (i == inventoryManager.SelectedIndex) ? "> " : "   ";
            string qtyText = slots[i].itemData.isStackable ? $" x{slots[i].quantity}" : "";
            sb.AppendLine($"{prefix}{i + 1}. {slots[i].itemData.itemName}{qtyText}");
        }

        if (slots.Count == 0)
        {
            sb.AppendLine("(Chưa có vật phẩm nào)");
        }

        inventoryText.text = sb.ToString();
    }
}
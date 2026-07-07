using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 1 ô trong túi đồ: vật phẩm + số lượng.
/// </summary>
[Serializable]
public class InventorySlot
{
    public ItemData itemData;
    public int quantity;

    public InventorySlot(ItemData data, int qty)
    {
        itemData = data;
        quantity = qty;
    }
}

/// <summary>
/// Gắn script này vào Player (BroModel). Quản lý danh sách vật phẩm đã nhặt,
/// vật phẩm đang được chọn, và xử lý khi người chơi "dùng" vật phẩm đó.
/// </summary>
public class InventoryManager : MonoBehaviour
{
    [Header("Tham chiếu")]
    [SerializeField] private FlashlightController flashlightController;

    [Header("Phím tắt")]
    [SerializeField] private KeyCode useItemKey = KeyCode.F;
    [SerializeField] private KeyCode toggleInventoryKey = KeyCode.Tab;

    private readonly List<InventorySlot> slots = new List<InventorySlot>();
    private int selectedIndex = -1;

    // UI sẽ lắng nghe sự kiện này để tự vẽ lại danh sách
    public event Action OnInventoryChanged;

    public IReadOnlyList<InventorySlot> Slots => slots;
    public int SelectedIndex => selectedIndex;
    public bool IsInventoryOpen { get; private set; }

    private void Update()
    {
        if (Input.GetKeyDown(toggleInventoryKey))
        {
            IsInventoryOpen = !IsInventoryOpen;
            OnInventoryChanged?.Invoke();
        }

        if (!IsInventoryOpen) return;

        // Chọn vật phẩm bằng phím số (1-9)
        for (int i = 0; i < slots.Count && i < 9; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                selectedIndex = i;
                OnInventoryChanged?.Invoke();
            }
        }

        if (Input.GetKeyDown(useItemKey) && selectedIndex >= 0 && selectedIndex < slots.Count)
        {
            UseItem(slots[selectedIndex]);
        }
    }

    /// <summary>Thêm vật phẩm vào túi đồ. Gọi hàm này từ WorldItem khi nhặt được đồ.</summary>
    public void AddItem(ItemData itemData, int quantity = 1)
    {
        if (itemData == null) return;

        if (itemData.isStackable)
        {
            InventorySlot existing = slots.Find(s => s.itemData == itemData);
            if (existing != null)
            {
                existing.quantity += quantity;
                OnInventoryChanged?.Invoke();
                return;
            }
        }

        slots.Add(new InventorySlot(itemData, quantity));

        // Tự động chọn vật phẩm đầu tiên nếu chưa chọn gì
        if (selectedIndex < 0) selectedIndex = slots.Count - 1;

        OnInventoryChanged?.Invoke();
    }

    private void UseItem(InventorySlot slot)
    {
        switch (slot.itemData.itemType)
        {
            case ItemType.Flashlight:
                if (flashlightController != null)
                {
                    flashlightController.Toggle();
                }
                else
                {
                    Debug.LogWarning("Chưa gán Flashlight Controller trong InventoryManager.");
                }
                break;

            case ItemType.Note:
                Debug.Log($"Đọc ghi chú: {slot.itemData.description}");
                break;

            default:
                Debug.Log($"Dùng vật phẩm: {slot.itemData.itemName}");
                break;
        }
    }
}

using UnityEngine;

/// <summary>
/// Loại vật phẩm, dùng để InventoryManager biết cách xử lý khi "dùng" vật phẩm.
/// </summary>
public enum ItemType
{
    Flashlight,   // Đèn pin
    KeyItem,      // Chìa khóa / vật phẩm dùng để mở khóa gì đó
    Note,         // Ghi chú / mảnh ký ức (chỉ đọc, không "dùng" được)
    Generic       // Vật phẩm chung chung khác
}

/// <summary>
/// Dữ liệu tĩnh của 1 loại vật phẩm. Tạo asset bằng chuột phải trong Project
/// → Create → Inventory → Item Data.
/// </summary>
[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item Data")]
public class ItemData : ScriptableObject
{
    [Header("Thông tin cơ bản")]
    public string itemName = "Vật phẩm mới";
    [TextArea(2, 4)]
    public string description = "";
    public ItemType itemType = ItemType.Generic;

    [Header("Có thể xếp chồng số lượng không (VD: nhiều mảnh ký ức)")]
    public bool isStackable = false;
}

using UnityEngine;

/// <summary>
/// Gắn script này vào GameObject vật phẩm nằm trong scene (VD: cây đèn pin trên bàn).
/// Khi người chơi nhấn E nhìn vào nó, vật phẩm sẽ được thêm vào túi đồ và biến mất khỏi scene.
/// </summary>
public class WorldItem : MonoBehaviour, IInteractable
{
    [SerializeField] private ItemData itemData;
    [SerializeField] private int quantity = 1;

    public void Interact(GameObject interactor)
    {
        InventoryManager inventory = interactor.GetComponentInParent<InventoryManager>();
        if (inventory == null)
        {
            inventory = interactor.GetComponent<InventoryManager>();
        }

        if (inventory == null)
        {
            Debug.LogWarning("Không tìm thấy InventoryManager trên Player.");
            return;
        }

        inventory.AddItem(itemData, quantity);

        // Vật phẩm đã nhặt thì biến mất khỏi scene
        gameObject.SetActive(false);
    }

    public string GetInteractPrompt()
    {
        if (itemData == null) return "Nhấn E để nhặt";
        return $"Nhấn E để nhặt {itemData.itemName}";
    }
}

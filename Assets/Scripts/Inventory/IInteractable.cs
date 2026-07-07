/// <summary>
/// Interface dùng cho mọi vật thể người chơi có thể tương tác bằng phím E:
/// nhặt đồ, mở cửa, đọc ghi chú, bật công tắc...
/// </summary>
public interface IInteractable
{
    /// <summary>Được gọi khi người chơi nhấn phím tương tác trong khi đang nhìn vào vật thể này.</summary>
    void Interact(UnityEngine.GameObject interactor);

    /// <summary>Dòng chữ gợi ý hiển thị trên màn hình, VD: "Nhấn E để nhặt Đèn pin".</summary>
    string GetInteractPrompt();
}

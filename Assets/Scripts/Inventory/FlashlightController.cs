using UnityEngine;

/// <summary>
/// Gắn script này vào GameObject "FlashLight" đã có sẵn trong scene của bạn
/// (object chứa component Light). Mặc định đèn sẽ TẮT cho đến khi người chơi
/// nhặt được đèn pin và bấm dùng.
/// </summary>
public class FlashlightController : MonoBehaviour
{
    [SerializeField] private Light flashlightLight;
    [SerializeField] private bool startOff = true;

    private bool isOn;

    private void Awake()
    {
        if (flashlightLight == null)
        {
            flashlightLight = GetComponent<Light>();
        }

        isOn = !startOff;
        ApplyState();
    }

    public void Toggle()
    {
        isOn = !isOn;
        ApplyState();
    }

    public void TurnOn()
    {
        isOn = true;
        ApplyState();
    }

    public void TurnOff()
    {
        isOn = false;
        ApplyState();
    }

    private void ApplyState()
    {
        if (flashlightLight != null)
        {
            flashlightLight.enabled = isOn;
        }
    }
}

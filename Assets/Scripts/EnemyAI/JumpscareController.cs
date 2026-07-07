using UnityEngine;
using UnityEngine.UI;

// Gắn script này vào một GameObject quản lý (ví dụ: GameManager hoặc UICanvas)
public class JumpscareController : MonoBehaviour
{
    [Header("UI Jumpscare")]
    public Image jumpscareImage;      // Ảnh hù dọa (đặt sẵn trong Canvas, để trống/tắt lúc đầu)
    public AudioSource jumpscareAudio; // Âm thanh hét/động lớn khi hù

    [Header("Thông số")]
    public float showDuration = 2f;    // Ảnh hiển thị bao lâu
    public bool pauseGameOnJumpscare = true;
    public bool reloadSceneAfter = false; // Có load lại scene sau khi hù không

    private bool triggered = false;

    void Start()
    {
        if (jumpscareImage != null)
        {
            jumpscareImage.gameObject.SetActive(false);
        }
    }

    public void TriggerJumpscare()
    {
        if (triggered) return;
        triggered = true;

        if (jumpscareImage != null)
        {
            jumpscareImage.gameObject.SetActive(true);
        }

        if (jumpscareAudio != null)
        {
            jumpscareAudio.Play();
        }

        if (pauseGameOnJumpscare)
        {
            Time.timeScale = 0f; // Dừng game lại
        }

        Invoke(nameof(EndJumpscare), showDuration);
    }

    void EndJumpscare()
    {
        if (pauseGameOnJumpscare)
        {
            Time.timeScale = 1f;
        }

        if (reloadSceneAfter)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(
                UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }
    }
}

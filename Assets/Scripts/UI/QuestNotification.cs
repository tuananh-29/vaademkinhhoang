using UnityEngine;
using TMPro;
using System.Collections;

public class QuestNotification : MonoBehaviour
{
    public GameObject panel;
    public TMP_Text titleText;
    public TMP_Text descText;
    public float displayDuration = 4f;

    private Coroutine currentRoutine;

    // ========================================================
    // CHÈN THÊM ĐOẠN CODE TEST NÀY VÀO TRONG FILE CỦA BẠN:
    [ContextMenu("Test Show Notification")]
    public void TestNotification()
    {
        ShowNotification("NHIỆM VỤ MỚI", "Tìm kiếm 3 mảnh ký ức trong phòng kho.");
    }
    // ========================================================

    public void ShowNotification(string title, string desc)
    {
        titleText.text = title;
        descText.text = desc;
        panel.SetActive(true);

        if (currentRoutine != null)
            StopCoroutine(currentRoutine);
        
        currentRoutine = StartCoroutine(HideAfterDelay());
    }

    IEnumerator HideAfterDelay()
    {
        yield return new WaitForSeconds(displayDuration);
        panel.SetActive(false);
    }
}
using UnityEngine;

public class TalkUIManager : MonoBehaviour
{
    [Header("UI面板引用")]
    [Tooltip("需要控制的对话UI面板（需在Inspector关联）")]
    public GameObject talkUI;  // 对话面板对象引用

    void Awake()
    {
        // 初始化时关闭面板（确保游戏开始时不显示）
        CloseTalkUI();
    }

    /// <summary>
    /// 打开对话UI面板
    /// </summary>
    public void OpenTalkUI()
    {
        if(talkUI != null)
        {
            talkUI.SetActive(true);
        }
    }

    /// <summary>
    /// 关闭对话UI面板
    /// </summary>
    public void CloseTalkUI()
    {
        if(talkUI != null)
        {
            talkUI.SetActive(false);
        }
    }
}
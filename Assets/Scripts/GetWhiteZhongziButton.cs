using UnityEngine;
using UnityEngine.UI;

public class GetWhiteZhongziButton : MonoBehaviour
{
    // 按钮组件（需在Inspector关联）
    public Button zhongziButton;

    private ParabolaMovement parabolaMovement;
    private ItemManager itemManager;

    void Start()
    {
        // 获取必要组件实例
        parabolaMovement = FindObjectOfType<ParabolaMovement>();
        itemManager = ItemManager.Instance;

        // 绑定按钮点击事件
        zhongziButton.onClick.AddListener(OnButtonClicked);

        // 检查依赖是否存在
        if (parabolaMovement == null)
        {
            Debug.LogError("未找到ParabolaMovement组件，请确保场景中存在抛物线运动对象！");
        }
        if (itemManager == null)
        {
            Debug.LogError("未找到ItemManager单例，请检查ItemManager脚本是否正确挂载！");
        }
    }

    void OnButtonClicked()
    {
        if (parabolaMovement == null || itemManager == null) return;

        // 根据当前状态添加对应粽子
        switch (parabolaMovement.currentState)
        {
            case "good":
                itemManager.AddProWhiteZhongzi();
                Debug.Log("按钮触发：当前状态为good，已添加pro白粽子");
                break;
            case "ordinary":
                itemManager.AddWhiteZhongzi();
                Debug.Log("按钮触发：当前状态为ordinary，已添加白粽子");
                break;
            case "bad":
                Debug.Log("按钮触发：当前状态为bad，无奖励");
                break;
        }
    }
}
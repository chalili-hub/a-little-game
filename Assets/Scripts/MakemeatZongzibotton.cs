using UnityEngine;
using UnityEngine.UI;

public class MakeMeatZongziButton : MonoBehaviour
{
    public Button makeButton;  // 需在Inspector面板关联按钮组件

    private void Start()
    {
        makeButton.onClick.AddListener(OnMakeButtonClicked);
    }

    private void OnMakeButtonClicked()
    {
        // 检查肉粽子所需物资（粽叶、糯米、肉）是否足够
        if (ItemManager.Instance.DecreaseZongyeNuomiAndMeat())
        {
            // 扣除物资并显示肉粽子界面（需确保UIManager已实现ShowMeatZhongziUI）
            UIManager.Instance.ShowMeatZhongziUI();
        }
        else
        {
            // 显示无法制作界面（与白粽子共用）
            UIManager.Instance.ShowCannotMakeUI();
        }
    }
}
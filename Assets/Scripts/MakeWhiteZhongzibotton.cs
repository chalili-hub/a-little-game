using UnityEngine;
using UnityEngine.UI;

public class MakeWhiteZhongziButton : MonoBehaviour
{
    public Button makeButton;  // 需在Inspector面板关联按钮组件

    private void Start()
    {
        makeButton.onClick.AddListener(OnMakeButtonClicked);
    }

    private void OnMakeButtonClicked()
    {
        // 检查物资是否足够
        if (ItemManager.Instance.DecreaseZongyeAndNuomi())
        {
            // 扣除物资并显示粽子界面
            UIManager.Instance.ShowWhiteZhongziUI();
        }
        else
        {
            // 显示无法制作界面
            UIManager.Instance.ShowCannotMakeUI();
        }
    }
}

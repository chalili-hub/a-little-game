using UnityEngine;
using UnityEngine.UI;

public class MakeRedBeanZongziButton : MonoBehaviour
{
    public Button makeButton;  // 需在Inspector面板关联按钮组件

    private void Start()
    {
        makeButton.onClick.AddListener(OnMakeButtonClicked);
    }

    private void OnMakeButtonClicked()
    {
        // 检查红豆粽子所需物资（粽叶、糯米、红豆）是否足够
        if (ItemManager.Instance.DecreaseZongyeNuomiAndHongdou())
        {
            // 扣除物资并显示红豆粽子界面
            UIManager.Instance.ShowRedBeanZhongziUI();
        }
        else
        {
            // 显示无法制作界面（与白粽子共用）
            UIManager.Instance.ShowCannotMakeUI();
        }
    }
}
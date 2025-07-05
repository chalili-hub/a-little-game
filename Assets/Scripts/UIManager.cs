using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public GameObject mainUI;       // 主界面面板（需在Inspector关联）
    public GameObject inventoryUI;  // 背包界面面板（需在Inspector关联）
    public GameObject riceNPCUI;    // NPC对话界面面板（需在Inspector关联）
    public GameObject meatNPCUI;    // meatNPC对话界面面板（需在Inspector关联，新增）
    public GameObject waitUI;       // 等待提示界面（需在Inspector关联新增）
    public GameObject cookingUI;    // 新增：烹饪界面面板（需在Inspector关联）
    public GameObject whiteZhongziUI; // 新增：粽子界面面板（需在Inspector关联）
    public GameObject redbeanZhongziUI; // 新增：红豆粽子界面面板（需在Inspector关联）
    public GameObject meatZhongziUI;    // 新增：肉粽子界面面板（需在Inspector关联）
    public GameObject eggZhongziUI;     // 新增：蛋粽子界面面板（需在Inspector关联）
    public GameObject CannotMakeUI; // 新增：无法制作界面面板（需在Inspector关联）
    // 新增：龙舟升级界面面板（需在Inspector关联）
    public GameObject dragonBoatUpgradeUI;
    // 新增：选择龙舟界面面板（需在Inspector关联）
    public GameObject chooseDragonBoatUI;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 标记UIManager对象不随场景销毁
        }
        else
        {
            Destroy(gameObject); // 避免重复实例化
            return;
        }

        // 原有初始化逻辑（保持UI初始状态）
        mainUI.SetActive(true);   // 默认显示主界面
        riceNPCUI.SetActive(false);   // 默认隐藏NPC对话界面
        meatNPCUI.SetActive(false);   // 默认隐藏meatNPC对话界面（新增）
        waitUI.SetActive(false);       // 新增：初始隐藏等待界面
        cookingUI.SetActive(false);    // 新增：初始隐藏烹饪界面
        whiteZhongziUI.SetActive(false); // 新增：初始隐藏粽子界面
        redbeanZhongziUI.SetActive(false); // 新增：初始隐藏红豆粽子界面
        meatZhongziUI.SetActive(false);    // 新增：初始隐藏肉粽子界面
        eggZhongziUI.SetActive(false);     // 新增：初始隐藏蛋粽子界面
        CannotMakeUI.SetActive(false);// 新增：初始隐藏无法制作界面
        inventoryUI.SetActive(true); // 隐藏背包界面
        dragonBoatUpgradeUI.SetActive(true); // 新增：初始隐藏龙舟升级界面
        chooseDragonBoatUI.SetActive(false);  // 新增：初始隐藏选择龙舟界面
    }
    void Start()
    {
        inventoryUI.SetActive(false); // 隐藏背包界面
        dragonBoatUpgradeUI.SetActive(false); // 新增：初始隐藏龙舟升级界面
    }

    // 新增：显示等待界面
    public void ShowWaitUI()
    {
        mainUI.SetActive(false);   // 隐藏主界面
        waitUI.SetActive(true);    // 显示等待界面
    }

    // 新增：隐藏等待界面（返回主界面）
    public void HideWaitUI()
    {
        waitUI.SetActive(false);   // 隐藏等待界面
        mainUI.SetActive(true);    // 恢复显示主界面
    }

    // 显示背包界面
    public void ShowInventory()
    {
        mainUI.SetActive(false);
        inventoryUI.SetActive(true);
    }

    // 隐藏背包界面（返回主界面）
    public void HideInventory()
    {
        mainUI.SetActive(true);
        inventoryUI.SetActive(false);
    }

    // 显示NPC对话界面（与NPC对话时调用）
    public void ShowRiceNPCUI()
    {
        mainUI.SetActive(false);      // 隐藏主界面
        riceNPCUI.SetActive(true);    // 显示NPC对话界面
    }

    // 隐藏NPC对话界面（结束对话时调用）
    public void HideRiceNPCUI()
    {
        riceNPCUI.SetActive(false);   // 隐藏NPC对话界面
        mainUI.SetActive(true);       // 恢复显示主界面
    }

    // 显示meatNPC对话界面（新增）
    public void ShowMeatNPCUI()
    {
        mainUI.SetActive(false);      // 隐藏主界面
        meatNPCUI.SetActive(true);    // 显示meatNPC对话界面
    }

    // 隐藏meatNPC对话界面（新增）
    public void HideMeatNPCUI()
    {
        meatNPCUI.SetActive(false);   // 隐藏meatNPC对话界面
        mainUI.SetActive(true);       // 恢复显示主界面
    }

    // 新增：显示烹饪界面
    public void ShowCookingUI()
    {
        mainUI.SetActive(false);    // 隐藏主界面
        cookingUI.SetActive(true);  // 显示烹饪界面
    }

    // 新增：隐藏烹饪界面（返回主界面）
    public void HideCookingUI()
    {
        cookingUI.SetActive(false); // 隐藏烹饪界面
        mainUI.SetActive(true);     // 恢复显示主界面
    }

    // 新增：显示WhiteZhongziUI界面（从烹饪界面跳转）
    public void ShowWhiteZhongziUI()
    {
        cookingUI.SetActive(false);     // 隐藏烹饪界面
        whiteZhongziUI.SetActive(true); // 显示粽子界面
    }

    // 新增：隐藏WhiteZhongziUI界面（返回烹饪界面）
    public void HideWhiteZhongziUI()
    {
        whiteZhongziUI.SetActive(false); // 隐藏粽子界面
        cookingUI.SetActive(true);       // 恢复显示烹饪界面
    }

    // 新增：显示RedbeanZhongziUI界面（从烹饪界面跳转）
    public void ShowRedBeanZhongziUI()
    {
        cookingUI.SetActive(false);       // 隐藏烹饪界面
        redbeanZhongziUI.SetActive(true); // 显示红豆粽子界面（需在Inspector关联redbeanZhongziUI对象）
    }

    // 新增：隐藏RedbeanZhongziUI界面（返回烹饪界面）
    public void HideRedBeanZhongziUI()
    {
        redbeanZhongziUI.SetActive(false); // 隐藏红豆粽子界面
        cookingUI.SetActive(true);         // 恢复显示烹饪界面
    }

    //无法制作UI
    public void ShowCannotMakeUI()
    {
        cookingUI.SetActive(false);     // 隐藏烹饪界面
        CannotMakeUI.SetActive(true); // 显示无法制作界面
    }
    // 新增：隐藏无法制作UI界面（返回烹饪界面）
    public void HideCannotMakeUI()
    {
        CannotMakeUI.SetActive(false); // 隐藏无法制作界面
        cookingUI.SetActive(true);       // 恢复显示烹饪界面
    }

    // 新增：显示MeatZhongziUI界面（从烹饪界面跳转）
    public void ShowMeatZhongziUI()
    {
        cookingUI.SetActive(false);     // 隐藏烹饪界面
        meatZhongziUI.SetActive(true);  // 显示肉粽子界面
    }

    // 新增：隐藏MeatZhongziUI界面（返回烹饪界面）
    public void HideMeatZhongziUI()
    {
        meatZhongziUI.SetActive(false); // 隐藏肉粽子界面
        cookingUI.SetActive(true);       // 恢复显示烹饪界面
    }

    // 新增：显示EggZhongziUI界面（从烹饪界面跳转）
    public void ShowEggZhongziUI()
    {
        cookingUI.SetActive(false);     // 隐藏烹饪界面
        eggZhongziUI.SetActive(true);   // 显示蛋粽子界面
    }

    // 新增：隐藏EggZhongziUI界面（返回烹饪界面）
    public void HideEggZhongziUI()
    {
        eggZhongziUI.SetActive(false);  // 隐藏蛋粽子界面
        cookingUI.SetActive(true);       // 恢复显示烹饪界面
    }

    // 新增：显示龙舟升级界面
    public void ShowDragonBoatUpgradeUI()
    {
        mainUI.SetActive(false);          // 隐藏主界面
        dragonBoatUpgradeUI.SetActive(true); // 显示龙舟升级界面
    }

    // 新增：隐藏龙舟升级界面（返回主界面）
    public void HideDragonBoatUpgradeUI()
    {
        dragonBoatUpgradeUI.SetActive(false); // 隐藏龙舟升级界面
        mainUI.SetActive(true);               // 恢复显示主界面
    }

    // 新增：显示选择龙舟界面
    public void ShowChooseDragonBoatUI()
    {
        mainUI.SetActive(false);          // 隐藏主界面
        chooseDragonBoatUI.SetActive(true); // 显示选择龙舟界面
    }

    // 新增：隐藏选择龙舟界面（返回主界面）
    public void HideChooseDragonBoatUI()
    {
        chooseDragonBoatUI.SetActive(false); // 隐藏选择龙舟界面
        mainUI.SetActive(true);               // 恢复显示主界面
    }
}
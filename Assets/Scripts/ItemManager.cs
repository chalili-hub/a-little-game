
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    // 添加单例实例定义
    public static ItemManager Instance;

    // 物品槽位数组（大小为13）
    public ItemSlot[] itemSlots = new ItemSlot[13]; 

    private void Awake()
    {
        // 单例初始化逻辑
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // 原有初始化逻辑：确保数组长度正确（可在Inspector面板手动绑定）
        if (itemSlots.Length != 13)
        {
            Debug.LogError("物品槽位数量必须为13！");
            return;
        }
    }

    /// <summary>
    /// 增加物品到背包（自动处理堆叠）
    /// </summary>
    /// <param name="targetID">要增加的物品ID</param>
    /// <param name="targetName">要增加的物品名称</param>
    /// <param name="addNumber">要增加的数量</param>
    public void AddItem(int targetID, string targetName, int addNumber)
    {
        // 1. 优先查找同ID且未堆叠满的槽位
        foreach (var slot in itemSlots)
        {
            if (slot.itemID == targetID && slot.number < slot.maxStack)
            {
                int available = slot.maxStack - slot.number; // 剩余可堆叠数量
                int actualAdd = Mathf.Min(addNumber, available); // 实际能增加的数量
                
                slot.SetItem(slot.number + actualAdd); // 调用ItemSlot的SetItem方法更新数量并保存
                addNumber -= actualAdd;
                
                if (addNumber <= 0) return; // 数量已全部添加完毕
            }
        }

        // 2. 若还有剩余数量，查找空槽位（number=0的槽位）
        foreach (var slot in itemSlots)
        {
            if (slot.number == 0)
            {
                slot.itemID = targetID;
                slot.itemName = targetName;
                slot.SetItem(addNumber); // 直接设置数量（已通过SetItem的Clamp限制最大值）
                return;
            }
        }

        // 3. 没有可用槽位时提示
        Debug.LogWarning("背包已满，无法添加物品！");
    }

    /// <summary>
    /// 减少指定物品的数量（返回是否成功扣除）
    /// </summary>
    /// <param name="targetID">要减少的物品ID</param>
    /// <param name="removeNumber">要减少的数量</param>
    public bool RemoveItem(int targetID, int removeNumber)
    {
        foreach (var slot in itemSlots)
        {
            if (slot.itemID == targetID && slot.number > 0)
            {
                int actualRemove = Mathf.Min(removeNumber, slot.number);
                slot.SetItem(slot.number - actualRemove); 
                removeNumber -= actualRemove;
                if (removeNumber <= 0) return true; // 成功扣除
            }
        }
        Debug.LogWarning("未找到足够数量的该物品！");
        return false; // 扣除失败
    }

    /// <summary>
    /// 减少1个粽叶（id=3）和1个糯米（id=2）（带数量检查）
    /// </summary>
    public bool DecreaseZongyeAndNuomi()
    {
        int zongyeCount = 0;
        int nuomiCount = 0;

        // 遍历物品槽位获取当前数量
        foreach (var slot in itemSlots)
        {
            if (slot.itemID == 3) zongyeCount = slot.number;
            if (slot.itemID == 2) nuomiCount = slot.number;
        }

        // 检查数量是否足够
        if (zongyeCount >= 1 && nuomiCount >= 1)
        {
            RemoveItem(3, 1); // 扣除粽叶
            RemoveItem(2, 1); // 扣除糯米
            Debug.Log("成功扣除1个粽叶和1个糯米");
            return true;
        }
        else
        {
            Debug.LogWarning("粽叶或糯米数量不足（需要至少各1个），无法扣除！");
            return false;
        }
    }

    /// <summary>
    /// 减少1个粽叶（id=3）、1个糯米（id=2）和1个肉（id=4）（带数量检查）
    /// </summary>
    public bool DecreaseZongyeNuomiAndMeat()
    {
        int zongyeCount = 0;
        int nuomiCount = 0;
        int meatCount = 0;

        // 遍历物品槽位获取当前数量
        foreach (var slot in itemSlots)
        {
            if (slot.itemID == 3) zongyeCount = slot.number;
            if (slot.itemID == 2) nuomiCount = slot.number;
            if (slot.itemID == 4) meatCount = slot.number;
        }

        // 检查数量是否足够
        if (zongyeCount >= 1 && nuomiCount >= 1 && meatCount >= 1)
        {
            RemoveItem(3, 1); // 扣除粽叶
            RemoveItem(2, 1); // 扣除糯米
            RemoveItem(4, 1); // 扣除肉
            Debug.Log("成功扣除1个粽叶、1个糯米和1个肉");
            return true;
        }
        else
        {
            Debug.LogWarning("粽叶、糯米或肉数量不足（需要至少各1个），无法扣除！");
            return false;
        }
    }

    /// <summary>
    /// 减少1个粽叶（id=3）、1个糯米（id=2）和1个红豆（id=1）（带数量检查）
    /// </summary>
    public bool DecreaseZongyeNuomiAndHongdou()
    {
        int zongyeCount = 0;
        int nuomiCount = 0;
        int hongdouCount = 0;

        // 遍历物品槽位获取当前数量
        foreach (var slot in itemSlots)
        {
            if (slot.itemID == 3) zongyeCount = slot.number;
            if (slot.itemID == 2) nuomiCount = slot.number;
            if (slot.itemID == 1) hongdouCount = slot.number;
        }

        // 检查数量是否足够
        if (zongyeCount >= 1 && nuomiCount >= 1 && hongdouCount >= 1)
        {
            RemoveItem(3, 1); // 扣除粽叶
            RemoveItem(2, 1); // 扣除糯米
            RemoveItem(1, 1); // 扣除红豆
            Debug.Log("成功扣除1个粽叶、1个糯米和1个红豆");
            return true;
        }
        else
        {
            Debug.LogWarning("粽叶、糯米或红豆数量不足（需要至少各1个），无法扣除！");
            return false;
        }
    }
    /// <summary>
    /// 减少1个粽叶（id=3）、1个糯米（id=2）和1个蛋（id=5）（带数量检查）
    /// </summary>
    public bool DecreaseZongyeNuomiAndEgg()
    {
        int zongyeCount = 0;
        int nuomiCount = 0;
        int eggCount = 0;

        // 遍历物品槽位获取当前数量
        foreach (var slot in itemSlots)
        {
            if (slot.itemID == 3) zongyeCount = slot.number;
            if (slot.itemID == 2) nuomiCount = slot.number;
            if (slot.itemID == 5) eggCount = slot.number;
        }

        // 检查数量是否足够
        if (zongyeCount >= 1 && nuomiCount >= 1 && eggCount >= 1)
        {
            RemoveItem(3, 1); // 扣除粽叶
            RemoveItem(2, 1); // 扣除糯米
            RemoveItem(5, 1); // 扣除蛋
            Debug.Log("成功扣除1个粽叶、1个糯米和1个蛋");
            return true;
        }
        else
        {
            Debug.LogWarning("粽叶、糯米或蛋数量不足（需要至少各1个），无法扣除！");
            return false;
        }
    }
    /// <summary>
    /// 增加1个白粽子（id=7）
    /// </summary>
    public void AddWhiteZhongzi()
    {
        AddItem(7, "白粽子", 1);
    }

    public void AddProWhiteZhongzi()
    {
        AddItem(11, "pro白粽子", 1);
    }
    public void AddRedBeanZhongzi()
    {
        AddItem(6, "红豆粽子", 1);
    }

    public void AddProRedBeanZhongzi()
    {
        AddItem(10, "pro红豆粽子", 1);
    }
    public void AddmeatZhongzi()
    {
        AddItem(8, "肉粽子", 1);
    }

    public void AddPromeatZhongzi()
    {
        AddItem(12, "pro肉粽子", 1);
    }
    public void AddeggZhongzi()
    {
        AddItem(9, "蛋粽子", 1);
    }

    public void AddProeggZhongzi()
    {
        AddItem(13, "pro蛋粽子", 1);
    }

    /// <summary>
    /// 检测是否拥有任意一个指定ID的物品（数量>0）
    /// </summary>
    public bool HasAnyItem(int[] targetIds)
    {
        foreach (var slot in itemSlots)
        {
            foreach (var id in targetIds)
            {
                if (slot.itemID == id && slot.number > 0)
                    return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 检测是否拥有指定ID的物品（数量>0）
    /// </summary>
    public bool HasItem(int targetId)
    {
        foreach (var slot in itemSlots)
        {
            if (slot.itemID == targetId && slot.number > 0)
                return true;
        }
        return false;
    }
}
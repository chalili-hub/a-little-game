using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemSlot : MonoBehaviour
{    
    public TMP_Text countText; 
    public string itemName;       // 物品名称（如"糯米"）
    public int itemID;            // 唯一ID（用于标识物品，关键存储标识）

    public int maxStack = 99;     // 最大堆叠数量（可堆叠物品默认99）
    public int number = 0;         // 当前数量

    private void Start()
    {
        // 游戏启动时自动加载上次保存的数据
        LoadItemData();
    }

    // 设置物品数据并更新UI（处理数量并保存）
    public void SetItem(int newNumber)
    {
        // 限制数量不超过最大堆叠且不小于0
        number = Mathf.Clamp(newNumber, 0, maxStack);
        
        // 保存到本地存储（使用唯一ID作为键）
        PlayerPrefs.SetInt($"Item_{itemID}_Count", number);
        PlayerPrefs.Save(); // 立即保存确保数据持久化
        
        UpdateUI();
    }

    // 加载已保存的物品数量（自动在Start中调用）
    private void LoadItemData()
    {
        // 读取存储数据（若不存在则默认0）
        number = PlayerPrefs.GetInt($"Item_{itemID}_Count", 0);
        UpdateUI();
    }

    // 更新UI显示
    private void UpdateUI()
    {
        if (countText != null)
        {
            countText.text = number.ToString(); // 直接显示number的数值（包括0）
        }
    }
}
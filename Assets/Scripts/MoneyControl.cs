using UnityEngine;
using TMPro;

public class MoneyControl : MonoBehaviour
{
    public static MoneyControl Instance;
    public TMP_Text currencyText;
    private int currentCurrency;
    private const string currencyKey = "PlayerCurrency";

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        // 修改初始化逻辑：若未存储过货币则初始化为100
        currentCurrency = PlayerPrefs.HasKey(currencyKey) ? PlayerPrefs.GetInt(currencyKey) : 100;
        UpdateUI();
    }

    public int GetCurrentCurrency()
    {
        return currentCurrency;
    }

    // 更新货币数量并保存
    public void UpdateCurrency(int amount)
    {
        currentCurrency += amount;
        currentCurrency = Mathf.Max(currentCurrency, 0); // 防止负数
        PlayerPrefs.SetInt(currencyKey, currentCurrency); // 保存到本地
        UpdateUI(); // 更新UI显示
    }

    // 更新UI文本
    private void UpdateUI()
    {
        if (currencyText != null)
        {
            currencyText.text = currentCurrency.ToString(); // 显示货币数量
        }
    }

    // 购买糯米（扣除3货币，增加1个rice，id=2）
    public void BuyRice()
    {
        if (currentCurrency >= 3)
        {
            UpdateCurrency(-3); // 扣除3货币
            ItemManager.Instance.AddItem(2, "rice", 1); // 添加1个rice
        }
        else
        {
            Debug.LogWarning("货币不足，无法购买糯米！");
        }
    }
    // 购买红豆（扣除2货币，增加1个red baen，id=1）
    public void BuyRedBean()
    {
        if (currentCurrency >= 2)
        {
            UpdateCurrency(-2); // 扣除2货币
            ItemManager.Instance.AddItem(1, "red baen", 1); // 添加1个red baen
        }
        else
        {
            Debug.LogWarning("货币不足，无法购买红豆！");
        }
    }
    // 购买粽叶（扣除1货币，增加1个leaf，id=3）
    public void BuyLeaf()
    {
        if (currentCurrency >= 1)
        {
            UpdateCurrency(-1); // 扣除3货币
            ItemManager.Instance.AddItem(3, "leaf", 1); // 添加1个leaf
        }
        else
        {
            Debug.LogWarning("货币不足，无法购买粽叶！");
        }
    }
    // 购买肉（扣除8货币，增加1个meat，id=4）
    public void BuyMeat()
    {
        if (currentCurrency >= 8)
        {
            UpdateCurrency(-8); // 扣除8货币
            ItemManager.Instance.AddItem(4, "meat", 1); // 添加1个meat
        }
        else
        {
            Debug.LogWarning("货币不足，无法购买肉！");
        }
    }
    //购买蛋（扣除5货币，增加1个egg，id=5）
    public void BuyEgg()
    {
        if (currentCurrency >= 5)
        {
            UpdateCurrency(-5); // 扣除5货币
            ItemManager.Instance.AddItem(5, "egg", 1); // 添加1个egg
        }
        else
        {
            Debug.LogWarning("货币不足，无法购买蛋！");
        }
    }

    // 给予100货币
    public void Give100Currency()
    {
        UpdateCurrency(100); // 增加100货币
    }
}
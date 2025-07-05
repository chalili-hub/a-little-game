using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

// 新增：定义龙舟属性结构体（可序列化以便在Inspector显示）
[System.Serializable]
public struct DragonBoatStats
{
    public float dexterity;    // 灵巧
    public float sturdiness;   // 稳固
    public float moveSpeed;    // 移速
}

public class DragonBoatManager : MonoBehaviour
{
    public static DragonBoatManager Instance;

    // 三个龙舟的等级（初始0）
    private int dragonBoat1Level = 0;
    private int dragonBoat2Level = 0;
    private int dragonBoat3Level = 0;
    // 新增：当前选择的龙舟索引（1-3）
    private int currentSelectedIndex = 1;

    // 新增：各龙舟的初始属性值
    [Header("龙舟初始属性")]
    [Tooltip("龙舟1初始属性：灵巧/稳固/移速")]//均衡之舟（修改后）
    public DragonBoatStats dragonBoat1Base = new DragonBoatStats { dexterity = 2, sturdiness = 2, moveSpeed = 2 }; // 初始属性调整为2
    [Tooltip("龙舟2初始属性：灵巧/稳固/移速")]//浮舟飞渡（保持不变）
    public DragonBoatStats dragonBoat2Base = new DragonBoatStats { dexterity = 10, sturdiness = 10, moveSpeed = 3 };
    [Tooltip("龙舟3初始属性：灵巧/稳固/移速")]//磐岩（保持不变）
    public DragonBoatStats dragonBoat3Base = new DragonBoatStats { dexterity = 1, sturdiness = 4, moveSpeed = 1 };

    // 关联等级显示的文本组件（需在Inspector绑定）
    public TextMeshProUGUI dragonBoat1LevelText;
    public TextMeshProUGUI dragonBoat3LevelText;
    public TextMeshProUGUI dragonBoat2LevelText;
    public TextMeshProUGUI ChoosedragonBoat1LevelText;
    public TextMeshProUGUI ChoosedragonBoat3LevelText;
    public TextMeshProUGUI ChoosedragonBoat2LevelText;
    public List<GameObject> dragonBoatImages = new List<GameObject>();

    void Awake()
    {
        // 单例模式初始化
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 跨场景保留
            LoadLevels(); // 加载已保存的等级
            UpdateAllLevelTexts(); // 初始化显示

            // 新增：初始状态隐藏所有龙舟（需确保dragonBoatImages已在Inspector中赋值）
            foreach (var image in dragonBoatImages)
            {
                image.SetActive(false);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 增加指定龙舟的等级
    public void IncreaseLevel(int dragonBoatIndex)
    {
        switch (dragonBoatIndex)
        {
            case 1:
                dragonBoat1Level++;
                break;
            case 2:
                dragonBoat2Level++;
                break;
            case 3:
                dragonBoat3Level++;
                break;
            default:
                Debug.LogError("无效的龙舟索引（1-3）");
                return;
        }
        UpdateLevelText(dragonBoatIndex); // 更新对应文本
        SaveLevels(); // 实时保存
    }

    // 获取指定龙舟的等级
    public int GetLevel(int dragonBoatIndex)
    {
        switch (dragonBoatIndex)
        {
            case 1: return dragonBoat1Level;
            case 2: return dragonBoat2Level;
            case 3: return dragonBoat3Level;
            default:
                Debug.LogError("无效的龙舟索引（1-3）");
                return -1;
        }
    }

    // 保存所有龙舟等级到本地（使用PlayerPrefs持久化）
    private void SaveLevels()
    {
        PlayerPrefs.SetInt("DragonBoat1Level", dragonBoat1Level);
        PlayerPrefs.SetInt("DragonBoat2Level", dragonBoat2Level);
        PlayerPrefs.SetInt("DragonBoat3Level", dragonBoat3Level);
        PlayerPrefs.Save(); // 强制保存
    }

    // 从本地加载所有龙舟等级
    private void LoadLevels()
    {
        dragonBoat1Level = PlayerPrefs.GetInt("DragonBoat1Level", 0);
        dragonBoat2Level = PlayerPrefs.GetInt("DragonBoat2Level", 0);
        dragonBoat3Level = PlayerPrefs.GetInt("DragonBoat3Level", 0);
    }

    // 更新指定龙舟的等级显示
    private void UpdateLevelText(int dragonBoatIndex)
    {
        switch (dragonBoatIndex)
        {
            case 1:
                dragonBoat1LevelText.text = $"{dragonBoat1Level}";
                ChoosedragonBoat1LevelText.text = $"{dragonBoat1Level}";
                break;
            case 2:
                dragonBoat2LevelText.text = $"{dragonBoat2Level}";
                ChoosedragonBoat2LevelText.text = $"{dragonBoat2Level}";
                break;
            case 3:
                dragonBoat3LevelText.text = $"{dragonBoat3Level}";
                ChoosedragonBoat3LevelText.text = $"{dragonBoat3Level}";
                break;
        }
    }

    // 更新所有龙舟的等级显示
    private void UpdateAllLevelTexts()
    {
        dragonBoat1LevelText.text = $"{dragonBoat1Level}";
        dragonBoat2LevelText.text = $"{dragonBoat2Level}";
        dragonBoat3LevelText.text = $"{dragonBoat3Level}";
        ChoosedragonBoat1LevelText.text = $"{dragonBoat1Level}";
        ChoosedragonBoat2LevelText.text = $"{dragonBoat2Level}";
        ChoosedragonBoat3LevelText.text = $"{dragonBoat3Level}";
    }

    // 游戏退出时自动保存（确保数据持久化）
    void OnApplicationQuit()
    {
        SaveLevels();
    }

    // 升级龙舟1（消耗100货币，需等级1-3级）
    public void UpgradeDragonBoat1()
    {
        if (MoneyControl.Instance.GetCurrentCurrency() >= 100) // 检查货币是否足够
        {
            int currentLevel = GetLevel(1);
            if (currentLevel == 0)
            {
                Debug.LogWarning("龙舟1等级为0，无法升级！");
            }
            else if (currentLevel >= 4) // 新增：等级≥4级时提示上限
            {
                Debug.LogWarning("龙舟1已达到最高等级（4级），无法升级！");
            }
            else // 等级1-3级时允许升级
            {
                MoneyControl.Instance.UpdateCurrency(-100); // 扣除100货币
                IncreaseLevel(1); // 升级龙舟1
            }
        }
        else
        {
            Debug.LogWarning("货币不足，无法升级龙舟1！");
        }
    }

    // 新增：尝试选择当前龙舟并跳转场景
    public void TrySelectDragonBoat()
    {
        int selectedIndex = GetSelectedIndex();
        int currentLevel = GetLevel(selectedIndex);

        if (currentLevel > 0)
        {
            SceneManager.LoadScene("SelectLevel"); // 跳转到目标场景
        }
        else
        {
            Debug.Log("无法选择"); // 等级为0时输出提示
        }
    }

    // 升级龙舟2（消耗100货币，需等级1-3级）
    public void UpgradeDragonBoat2()
    {
        if (MoneyControl.Instance.GetCurrentCurrency() >= 100)
        {
            int currentLevel = GetLevel(2);
            if (currentLevel == 0)
            {
                Debug.LogWarning("龙舟2等级为0，无法升级！");
            }
            else if (currentLevel >= 4) // 新增：等级≥4级时提示上限
            {
                Debug.LogWarning("龙舟2已达到最高等级（4级），无法升级！");
            }
            else // 等级1-3级时允许升级
            {
                MoneyControl.Instance.UpdateCurrency(-100);
                IncreaseLevel(2);
            }
        }
        else
        {
            Debug.LogWarning("货币不足，无法升级龙舟2！");
        }
    }

    // 升级龙舟3（消耗100货币，需等级1-3级）
    public void UpgradeDragonBoat3()
    {
        if (MoneyControl.Instance.GetCurrentCurrency() >= 100)
        {
            int currentLevel = GetLevel(3);
            if (currentLevel == 0)
            {
                Debug.LogWarning("龙舟3等级为0，无法升级！");
            }
            else if (currentLevel >= 4) // 新增：等级≥4级时提示上限
            {
                Debug.LogWarning("龙舟3已达到最高等级（4级），无法升级！");
            }
            else // 等级1-3级时允许升级
            {
                MoneyControl.Instance.UpdateCurrency(-100);
                IncreaseLevel(3);
            }
        }
        else
        {
            Debug.LogWarning("货币不足，无法升级龙舟3！");
        }
    }

    // 新增：重置所有龙舟等级为0（包含显示更新和持久化保存）
    public void ResetAllDragonBoatLevels()
    {
        // 重置等级为0
        dragonBoat1Level = 0;
        dragonBoat2Level = 0;
        dragonBoat3Level = 0;

        // 更新所有等级显示
        UpdateAllLevelTexts();

        // 持久化保存归零后的等级
        SaveLevels();

        Debug.Log("所有龙舟等级已重置为0");
    }

    // 新增：获取指定龙舟的当前属性（根据等级计算）
    // 修改属性计算逻辑（原乘数改为每级+1）
    public DragonBoatStats GetCurrentStats(int dragonBoatIndex)
    {
        int level = GetLevel(dragonBoatIndex);
        DragonBoatStats baseStats;

        switch (dragonBoatIndex)
        {
            case 1: baseStats = dragonBoat1Base; break;
            case 2: baseStats = dragonBoat2Base; break;
            case 3: baseStats = dragonBoat3Base; break;
            default: return new DragonBoatStats();
        }

        // 每升一级各属性+1（等级0时为初始值，等级1时+1，等级2时+2...）
        return new DragonBoatStats
        {
            dexterity = baseStats.dexterity + level,
            sturdiness = baseStats.sturdiness + level,
            moveSpeed = baseStats.moveSpeed + level
        };
    }
    // 新增：设置当前选择的龙舟索引
    public void SetSelectedIndex(int index)
    { 
        foreach (var image in dragonBoatImages)
        {
            image.SetActive(false);
        }       
        if(index >= 1 && index <= 3)
        {            
            currentSelectedIndex = index;            
            Debug.Log($"已选择龙舟{index}");  
            dragonBoatImages[index - 1].SetActive(true);      
        }        
        else Debug.LogError("无效的龙舟索引（1-3）"); 
    }   

    // 新增：获取当前选择的龙舟索引
    public int GetSelectedIndex()
    {        
        return currentSelectedIndex;    
    }
}

 


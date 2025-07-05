using UnityEngine;
using TMPro; 
using UnityEngine.UI;
using System.Collections.Generic;

public class DialogueSystem : MonoBehaviour
{
    [Header("UI引用")]
    public TextMeshProUGUI dialogueText; 
    public GameObject dialogueBox; 

    [Header("多组对话配置")]
    public List<DialogueGroup> dialogueGroups; // 所有对话组（需在Inspector按顺序添加）
    private int currentGroupIndex = 0; // 当前使用的对话组索引（默认第一组）
    private int currentPage = 0;      
    private bool isDialogueActive = false; 
    private const string FIRST_DIALOGUE_GROUP_ID = "Group1"; // 第一段对话组的ID（需在Inspector设置对应Group的groupId）

    void Update()
    {
        if (isDialogueActive && Input.GetKeyDown(KeyCode.F))
        {
            ShowNextPage();
        }
    }

    public void StartDialogue() 
    {
        // 获取当前对话组（初始组）
        DialogueGroup currentGroup = dialogueGroups[currentGroupIndex];
        // 加载当前组的进度（键格式："Dialogue_{groupId}_Page"）
        currentPage = PlayerPrefs.GetInt($"Dialogue_{currentGroup.groupId}_Page", 0);
    
        // 处理边界：如果进度超过当前组长度，切换到下一组并重置（提前到条件检测前）
        if (currentPage >= currentGroup.content.Length)
        {
            SwitchToNextGroup(); // 切换到下一组
            currentGroup = dialogueGroups[currentGroupIndex]; // 重新获取新组
            currentPage = 0; // 新组从第0页开始
        }
    
        // 新增：对话前条件检测（现在针对切换后的新组）
        bool conditionCheckPassed = true;
        switch (currentGroup.groupId)
        {
            case "Group2":  // 第二组检测：是否有白米粽（id=7或11）
                conditionCheckPassed = ItemManager.Instance.HasAnyItem(new int[] {7, 11});
                if (!conditionCheckPassed) Debug.LogWarning("触发第二组对话失败：需要至少1个白米粽（id=7或pro白粽子id=11）");
                break;
            case "Group3":  // 第三组检测：是否有粽叶（id=3）
                conditionCheckPassed = ItemManager.Instance.HasItem(3);
                if (!conditionCheckPassed) Debug.LogWarning("触发第三组对话失败：需要至少1个粽叶（id=3）");
                break;
            case "Group5":  
                if (DragonBoatManager.Instance == null)
                {
                    Debug.LogError("DragonBoatManager 实例未初始化！");
                    conditionCheckPassed = false;
                    break;
                }
                int dragonBoat1Level = DragonBoatManager.Instance.GetLevel(1);
                Debug.Log("当前龙舟1等级：" + dragonBoat1Level); 
                conditionCheckPassed = dragonBoat1Level >= 2;
                if (!conditionCheckPassed) 
                    Debug.LogWarning("触发第五组对话失败：龙舟1等级需≥2");
                break;
            case "Group6":  
                if (DragonBoatManager.Instance == null)
                {
                    Debug.LogError("DragonBoatManager 实例未初始化！");
                    conditionCheckPassed = false;
                    break;
                }
                int dragonBoat1Lv = DragonBoatManager.Instance.GetLevel(1);
                int dragonBoat2Lv = DragonBoatManager.Instance.GetLevel(2);
                conditionCheckPassed = (dragonBoat1Lv == 4 && dragonBoat2Lv == 4);
                if (!conditionCheckPassed) 
                    Debug.LogWarning("触发第六组对话失败：龙舟1和龙舟2等级需都为4");
                break;
        }
    
        // 条件不满足时终止对话（不显示对话框）
        if (!conditionCheckPassed)
        {
            return;
        }
    
        // 条件满足后再激活对话
        isDialogueActive = true;
        dialogueBox.SetActive(true);
        
        dialogueText.text = currentGroup.content[currentPage]; 
    }

    private void ShowNextPage()
    {
        DialogueGroup currentGroup = dialogueGroups[currentGroupIndex];
        currentPage++;
        // 保存当前组的进度
        PlayerPrefs.SetInt($"Dialogue_{currentGroup.groupId}_Page", currentPage);
        
        if (currentPage < currentGroup.content.Length)
        {
            dialogueText.text = currentGroup.content[currentPage]; 
        }
        else
        {
            EndDialogue(); 
        }
    }

    public void EndDialogue()
    {
        isDialogueActive = false;
        dialogueBox.SetActive(false);
        dialogueText.text = "";
        // 结束时保存当前组进度（确保提前退出保留进度）
        DialogueGroup currentGroup = dialogueGroups[currentGroupIndex];
        PlayerPrefs.SetInt($"Dialogue_{currentGroup.groupId}_Page", currentPage);

        // 首次完成第一段对话时奖励物品（原有逻辑）
        if (currentGroup.groupId == FIRST_DIALOGUE_GROUP_ID 
            && !PlayerPrefs.HasKey("FirstDialogueRewarded") 
            && currentPage >= currentGroup.content.Length)
        {
            // 添加糯米（id=2）和粽叶（id=3）各1个
            ItemManager.Instance.AddItem(2, "糯米", 1);
            ItemManager.Instance.AddItem(3, "粽叶", 1);
            PlayerPrefs.SetInt("FirstDialogueRewarded", 1); // 标记已奖励
            Debug.Log("首次完成第一段对话，已奖励1个糯米和1个粽叶");
        }

        // 新增：完成第二段对话时奖励100货币（需要在Inspector设置第二组的groupId为"Group2"）
        const string SECOND_DIALOGUE_GROUP_ID = "Group2"; // 第二组对话ID（需与Inspector配置一致）
        if (currentGroup.groupId == SECOND_DIALOGUE_GROUP_ID 
            && !PlayerPrefs.HasKey("SecondDialogueRewarded") 
            && currentPage >= currentGroup.content.Length)
        {
            MoneyControl.Instance.UpdateCurrency(100); // 增加100货币
            PlayerPrefs.SetInt("SecondDialogueRewarded", 1); // 标记已奖励
            Debug.Log("完成第二段对话，已奖励100货币");
        }

        // 新增：完成第四段对话时提升龙舟1等级（需在Inspector设置第四组的groupId为"Group4"）
        const string FOURTH_DIALOGUE_GROUP_ID = "Group4"; 
        if (currentGroup.groupId == FOURTH_DIALOGUE_GROUP_ID 
            && !PlayerPrefs.HasKey("FourthDialogueRewarded") 
            && currentPage >= currentGroup.content.Length)
        {
            DragonBoatManager.Instance.IncreaseLevel(1); // 提升龙舟1等级
            PlayerPrefs.SetInt("FourthDialogueRewarded", 1); // 标记已奖励
            Debug.Log("完成第四段对话，已提升龙舟1等级");
        }

        // 新增：完成第五段对话时奖励龙舟2等级
        const string FIFTH_DIALOGUE_GROUP_ID = "Group5"; 
        if (currentGroup.groupId == FIFTH_DIALOGUE_GROUP_ID 
            && !PlayerPrefs.HasKey("FifthDialogueRewarded") 
            && currentPage >= currentGroup.content.Length)
        {
            DragonBoatManager.Instance.IncreaseLevel(2); // 提升龙舟2等级
            PlayerPrefs.SetInt("FifthDialogueRewarded", 1); // 标记已奖励
            Debug.Log("完成第五段对话，已提升龙舟2等级");
        }

        // 新增：完成第六段对话时奖励龙舟3等级
        const string SIXTH_DIALOGUE_GROUP_ID = "Group6"; 
        if (currentGroup.groupId == SIXTH_DIALOGUE_GROUP_ID 
            && !PlayerPrefs.HasKey("SixthDialogueRewarded") 
            && currentPage >= currentGroup.content.Length)
        {
            DragonBoatManager.Instance.IncreaseLevel(3); // 提升龙舟3等级
            PlayerPrefs.SetInt("SixthDialogueRewarded", 1); // 标记已奖励
            Debug.Log("完成第六段对话，已提升龙舟3等级");
        }
    }

    /// <summary>
    /// 切换到下一组对话（循环或停止）
    /// </summary>
    private void SwitchToNextGroup()
    {
        currentGroupIndex++;
        // 可选：如果希望循环，改为 currentGroupIndex %= dialogueGroups.Count;
        if (currentGroupIndex >= dialogueGroups.Count)
        {
            currentGroupIndex = dialogueGroups.Count - 1; // 停留在最后一组（可根据需求调整）
        }
    }

    /// <summary>
    /// 重置所有对话组进度
    /// </summary>
    public void ResetAllDialogueProgress()
    {
        foreach (var group in dialogueGroups)
        {
            PlayerPrefs.DeleteKey($"Dialogue_{group.groupId}_Page");
        }
        currentGroupIndex = 0; // 重置组索引为第一组
        currentPage = 0;
        // 重置奖励状态（用于测试）
        PlayerPrefs.DeleteKey("FirstDialogueRewarded"); // 第一组奖励标记
        PlayerPrefs.DeleteKey("SecondDialogueRewarded"); // 第二组奖励标记（新增）
        PlayerPrefs.DeleteKey("FourthDialogueRewarded"); // 第四组奖励标记（新增）
        PlayerPrefs.DeleteKey("FifthDialogueRewarded"); // 第五组奖励标记（新增）
        PlayerPrefs.DeleteKey("SixthDialogueRewarded"); // 第六组奖励标记（新增）
        
        Debug.Log("已重置所有奖励");
    }
}

[System.Serializable] // 允许在Inspector显示
public class DialogueGroup
{
    public string groupId; // 对话组唯一标识（如"Group1"）
    public string[] content; // 该组的对话内容数组
}
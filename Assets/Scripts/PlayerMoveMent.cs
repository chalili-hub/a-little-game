using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine; // 添加Cinemachine命名空间

public class PlayerMoveMent : MonoBehaviour
{
    public float moveSpeed;
    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 moveDirection;
    private const float inputThreshold = 0.1f; 
    private float lastHorizontal; 
    private float lastVertical;   
    
    // 新增变量
    public GameObject doorPrompt; // 关联UI提示对象（需在Inspector面板赋值）
    private bool isNearDoor; // 是否在门附近
    // 新增riceNPC相关变量
    public GameObject riceNPCText; // 关联"按E购买"提示文本（需在Inspector赋值）
    private bool isNearRiceNPC;    // 是否在riceNPC附近
    // 新增meatNPC相关变量
    public GameObject meatNPCText; // 关联"按E购买"提示文本（需在Inspector赋值）
    private bool isNearMeatNPC;    // 是否在meatNPC附近
    // 新增出售相关变量（从CustomerMovement迁移）
    public GameObject sellPrompt;  // 关联"按E出售"提示UI（需在Inspector关联）
    private bool isNearCustomer;   // 是否在顾客附近（新增状态标记）
    // 新增boatNPC相关变量（新增代码）
    public GameObject boatNPCText; // 关联"按E开始游戏"提示文本（需在Inspector赋值）
    private bool isNearBoatNPC;    // 是否在boatNPC附近
    // 新增pot相关变量
    public GameObject potNPCText; // 关联"按E烹饪"提示文本（需在Inspector赋值）
    private bool isNearPot;    // 是否在pot附近
    // 新增RoomDoor相关变量
    public GameObject roomDoorPrompt; // 关联"按E离开"提示文本（需在Inspector赋值）
    private bool isNearRoomDoor;       // 是否在RoomDoor附近
    // 新增grandfather相关变量
    public GameObject grandfatherText; // 关联"按E交谈"提示文本（需在Inspector赋值）
    private bool isNearGrandfather;    // 是否在爷爷附近
        // 新增：BoatDoor相关变量
    public GameObject boatDoorPrompt; // 关联"按E进入"提示UI（需在Inspector赋值）
    private bool isNearBoatDoor;       // 是否在BoatDoor附近

    private static bool isPlayerCreated = false; // 新增静态变量，标记Player是否已创建

    // 新增：爷爷NPC相关变量（需在Inspector关联）
    // 移除：爷爷头像变量（不再需要）
    // public Sprite grandfatherPortrait; 
    private DialogueSystem dialogueSystem; // 对话系统引用
    
    [Header("Cinemachine设置")]
    public CinemachineVirtualCamera virtualCamera; // 在Inspector绑定虚拟相机（可选）

    void Start()
    {

        // 检查场景中是否已有Player实例（通过标签）
        PlayerMoveMent[] existingPlayers = FindObjectsOfType<PlayerMoveMent>();
        if (existingPlayers.Length > 1)
        {
            Destroy(gameObject); // 已有其他实例时销毁当前对象
            return;
        }

        // 唯一实例时标记不销毁
        DontDestroyOnLoad(gameObject);
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        if(doorPrompt != null) doorPrompt.SetActive(false); // 初始隐藏提示
        if(riceNPCText != null) riceNPCText.SetActive(false); // 初始隐藏提示
        if(meatNPCText != null) meatNPCText.SetActive(false); // 初始隐藏meat提示
        if(boatNPCText != null) boatNPCText.SetActive(false); // 初始隐藏boat提示（新增代码）
        if(potNPCText != null) potNPCText.SetActive(false); // 新增：初始隐藏pot提示
        if(roomDoorPrompt != null) roomDoorPrompt.SetActive(false); // 初始隐藏RoomDoor提示（新增）
        if(sellPrompt != null) sellPrompt.SetActive(false); // 新增：初始隐藏出售提示（从CustomerMovement迁移）
        if(grandfatherText != null) grandfatherText.SetActive(false); // 新增：初始隐藏爷爷提示
        dialogueSystem = FindObjectOfType<DialogueSystem>(); // 初始化对话系统引用
        if(grandfatherText != null) grandfatherText.SetActive(false); 
        // 新增：初始化BoatDoor提示
        if(boatDoorPrompt != null) boatDoorPrompt.SetActive(false); 
        
        // 注册场景加载完成事件
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // 新增：场景加载完成时触发
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 仅当加载的是Main场景时绑定相机（新增条件）
        if (scene.name == "Main")
        {
            // 重新绑定Cinemachine相机（原有逻辑）
            if (virtualCamera == null)
            {
                virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
            }
            if (virtualCamera != null)
            {
                virtualCamera.Follow = transform;
                Debug.Log("Cinemachine Virtual Camera Follow已绑定到Player");
            }
        }

        // 新增：重新绑定所有UI提示对象（关键修改）
        BindUIPrompts();
    }

    // 新增：统一绑定UI提示的方法
    private void BindUIPrompts()
    {
        // 示例：查找嵌套在UI/MainUI下的doorPrompt（路径：UI/MainUI/DoorPrompt）
        doorPrompt = GameObject.Find("UI/MainUI/EnterDoor"); 
        if (doorPrompt != null) doorPrompt.SetActive(false);
        else Debug.LogWarning("场景中未找到doorPrompt对象，请检查路径是否为'UI/MainUI/DoorPrompt'");
    
        // 示例：查找嵌套在UI/MainUI下的riceNPCText（路径：UI/MainUI/RiceNPCText）
        riceNPCText = GameObject.Find("UI/MainUI/TalkwithRiceNPC"); 
        if (riceNPCText != null) riceNPCText.SetActive(false);
        else Debug.LogWarning("场景中未找到riceNPCText对象，请检查路径是否为'UI/MainUI/RiceNPCText'");
    
        // 其他UI对象同理（根据实际嵌套路径调整）
        meatNPCText = GameObject.Find("UI/MainUI/TalkwithRiceNPC"); 
        if (meatNPCText != null) meatNPCText.SetActive(false);
    
        boatNPCText = GameObject.Find("UI/MainUI/start boat game"); 
        if (boatNPCText != null) boatNPCText.SetActive(false);
    
        potNPCText = GameObject.Find("UI/MainUI/make food"); 
        if (potNPCText != null) potNPCText.SetActive(false);
    
        roomDoorPrompt = GameObject.Find("UI/MainUI/EnterDoor"); 
        if (roomDoorPrompt != null) roomDoorPrompt.SetActive(false);
    
        grandfatherText = GameObject.Find("UI/MainUI/TalkwithGrandfather"); 
        if (grandfatherText != null) grandfatherText.SetActive(false);
    
        boatDoorPrompt = GameObject.Find("UI/MainUI/EnterDoor"); 
        if (boatDoorPrompt != null) boatDoorPrompt.SetActive(false);
    
        sellPrompt = GameObject.Find("UI/MainUI/sell Zhongzi"); 
        if (sellPrompt != null) sellPrompt.SetActive(false);
    }

    void OnDestroy()
    {
        // 取消事件注册（避免内存泄漏）
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Update()
    {
        // 仅在Main场景中允许处理输入
        if (SceneManager.GetActiveScene().name != "Main") return;
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        moveX = Mathf.Abs(moveX) < inputThreshold ? 0 : moveX;
        moveY = Mathf.Abs(moveY) < inputThreshold ? 0 : moveY;

        if (moveX != 0 || moveY != 0)
        {
            lastHorizontal = moveX;
            lastVertical = moveY;
        }

        moveDirection = new Vector2(moveX, moveY).normalized;

        float animHorizontal = (moveX == 0 && moveY == 0) ? lastHorizontal : moveX;
        float animVertical = (moveX == 0 && moveY == 0) ? lastVertical : moveY;

        animator.SetFloat("Horizontal", animHorizontal);
        animator.SetFloat("Vertical", animVertical);
        animator.SetFloat("Speed", new Vector2(moveX, moveY).sqrMagnitude);

        // 检测E键输入（门交互）
        if(isNearDoor && Input.GetKeyDown(KeyCode.E))
        {
            // 显示等待界面
            UIManager.Instance.ShowWaitUI();
            
            // 设置玩家位置
            transform.position = new Vector2(-170f, -14f); 
            // 设置摄像机位置（假设为2D场景，保持Z轴不变）
            if (Camera.main != null)
            {
                Camera.main.transform.position = new Vector3(-170f, -14f, Camera.main.transform.position.z);
            }

            // 启动协程：2秒后隐藏等待界面
            StartCoroutine(WaitAndHideWaitUI(2f));
        }
        
        // 新增：检测E键输入（RoomDoor交互）
        if(isNearRoomDoor && Input.GetKeyDown(KeyCode.E))
        {
            // 显示等待界面
            UIManager.Instance.ShowWaitUI();
            
            // 设置玩家位置为(3,7)
            transform.position = new Vector2(3f, 7f); 
            // 设置摄像机位置（保持Z轴不变）
            if (Camera.main != null)
            {
                Camera.main.transform.position = new Vector3(3f, 7f, Camera.main.transform.position.z);
            }

            // 启动协程：2秒后隐藏等待界面
            StartCoroutine(WaitAndHideWaitUI(2f));
        }        
        // 新增：检测E键输入（BoatDoor交互）
        if(isNearBoatDoor && Input.GetKeyDown(KeyCode.E))
        {
            // 调用UIManager显示龙舟升级界面
            UIManager.Instance.ShowDragonBoatUpgradeUI();
        }
        // 新增：检测E键输入（riceNPC交互）
        if(isNearRiceNPC && Input.GetKeyDown(KeyCode.E))
        {
            // 通过UIManager的单例方法显示NPC对话界面
            UIManager.Instance.ShowRiceNPCUI();
        }

        // 新增：检测E键输入（meatNPC交互）
        if(isNearMeatNPC && Input.GetKeyDown(KeyCode.E))
        {
            // 通过UIManager的单例方法显示meat/egg出售界面（需确保UIManager有此方法）
            UIManager.Instance.ShowMeatNPCUI();
        }

        // 新增：检测E键输入（boatNPC交互，新增代码）
        if(isNearBoatNPC && Input.GetKeyDown(KeyCode.E))
        {
            UIManager.Instance.ShowChooseDragonBoatUI(); // 显示选择龙舟界面
        }
        
        // 新增：检测E键输入（pot交互）
        if(isNearPot && Input.GetKeyDown(KeyCode.E))
        {
            // 通过UIManager的单例方法显示烹饪界面（需确保UIManager有此方法）
            UIManager.Instance.ShowCookingUI();
        }
        // 新增：检测E键输入（grandfather交互）
        if(isNearGrandfather && Input.GetKeyDown(KeyCode.E))
        {
            if(dialogueSystem != null) 
            {
                dialogueSystem.StartDialogue(); // 启动对话
                // 新增：隐藏"按E交谈"提示文本
                if(grandfatherText != null)
                {
                    grandfatherText.SetActive(false);
                }
            }
        }
    } // Update 方法结束

    void FixedUpdate() 
    {
        rb.velocity = moveDirection * moveSpeed;
    }

    // 新增：触发器检测方法
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Door")) // 确保房门对象标签为"Door"
        {
            isNearDoor = true;
            if(doorPrompt != null) doorPrompt.SetActive(true);
        }
        if(other.CompareTag("BoatDoor")) 
        {
            isNearBoatDoor = true;
            if(boatDoorPrompt != null) boatDoorPrompt.SetActive(true);
        }
        // 新增：检测riceNPC
        if(other.CompareTag("riceNPC")) 
        {
            isNearRiceNPC = true;
            if(riceNPCText != null) riceNPCText.SetActive(true); // 显示提示文本
        }
        // 新增：检测meatNPC（需确保meatNPC对象标签为"meatNPC"）
        if(other.CompareTag("meatNPC")) 
        {
            isNearMeatNPC = true;
            if(meatNPCText != null) meatNPCText.SetActive(true); // 显示提示文本
        }
        // 新增：检测boatNPC（需确保boatNPC对象标签为"boatNPC"，新增代码）
        if(other.CompareTag("boatNPC")) 
        {
            isNearBoatNPC = true;
            if(boatNPCText != null) boatNPCText.SetActive(true); // 显示"按E开始游戏"提示文本
        }
        // 新增：检测pot（需确保pot对象标签为"pot"）
        if(other.CompareTag("pot")) 
        {
            isNearPot = true;
            if(potNPCText != null) potNPCText.SetActive(true); // 显示"按E烹饪"提示文本
        }
        // 新增：检测RoomDoor（需确保对象标签为"RoomDoor"）
        if(other.CompareTag("RoomDoor")) 
        {
            isNearRoomDoor = true;
            if(roomDoorPrompt != null) roomDoorPrompt.SetActive(true); // 显示"按E离开"提示文本
        }
        // 新增：检测顾客（需确保顾客对象标签为"Customer"）
        if(other.CompareTag("customer")) 
        {
            isNearCustomer = true;
            if(sellPrompt != null) sellPrompt.SetActive(true); // 显示"按E出售"提示文本
        }
        // 新增：检测grandfather（需确保爷爷对象标签为"grandfather"）
        if(other.CompareTag("grandfather")) 
        {
            isNearGrandfather = true;
            if(grandfatherText != null) grandfatherText.SetActive(true); // 显示"按E交谈"提示文本
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
       if(other.CompareTag("Door"))
        {
            isNearDoor = false;
            if(doorPrompt != null) doorPrompt.SetActive(false);
        }
        // 新增：检测BoatDoor标签
        if(other.CompareTag("BoatDoor")) 
        {
            isNearBoatDoor = false;
            if(boatDoorPrompt != null) boatDoorPrompt.SetActive(false);
        }
        // 新增：离开riceNPC
        if(other.CompareTag("riceNPC")) 
        {
            isNearRiceNPC = false;
            if(riceNPCText != null) riceNPCText.SetActive(false); // 隐藏提示文本
        }
        // 新增：离开meatNPC（需确保meatNPC对象标签为"meatNPC"）
        if(other.CompareTag("meatNPC")) 
        {
            isNearMeatNPC = false;
            if(meatNPCText != null) meatNPCText.SetActive(false); // 隐藏提示文本
        }
        // 新增：离开boatNPC（新增代码）
        if(other.CompareTag("boatNPC")) 
        {
            isNearBoatNPC = false;
            if(boatNPCText != null) boatNPCText.SetActive(false); // 隐藏提示文本
        }
        // 新增：离开pot（需确保pot对象标签为"pot"）
        if(other.CompareTag("pot")) 
        {
            isNearPot = false;
            if(potNPCText != null) potNPCText.SetActive(false); // 隐藏提示文本
        }
        // 新增：离开RoomDoor
        if(other.CompareTag("RoomDoor")) 
        {
            isNearRoomDoor = false;
            if(roomDoorPrompt != null) roomDoorPrompt.SetActive(false); // 隐藏提示文本
        }
        // 新增：离开grandfather
        if(other.CompareTag("grandfather")) 
        {
            isNearGrandfather = false;
            if(grandfatherText != null) grandfatherText.SetActive(false); 
            if(dialogueSystem != null)
            {
                dialogueSystem.EndDialogue(); // 离开时结束对话
            }
        }
        if(other.CompareTag("customer")) 
        {
            isNearCustomer = false;
            if(sellPrompt != null) sellPrompt.SetActive(false); // 隐藏出售提示文本
        }
    }

    // 新增协程：延迟隐藏等待界面
    private IEnumerator WaitAndHideWaitUI(float delay)
    {
        yield return new WaitForSeconds(delay);
        UIManager.Instance.HideWaitUI();
    }
}

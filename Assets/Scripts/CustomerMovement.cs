using UnityEngine;

public class CustomerMovement : MonoBehaviour
{
    public float moveSpeed = 2f; 
    public Vector2 startPos = new Vector2(5.5f, -25f); 
    public Vector2 endPos = new Vector2(0f, -15f);     
    private GameObject Zongzi; 
    public GameObject[] zongziPrefabs; 
    //public GameObject sellPrompt; // 新增：出售提示UI（需在Inspector关联）
    private bool isPlayerNear = false; // 新增：玩家是否在触发区域
    private Vector2 currentTarget;                      
    private Animator animator;
    private Rigidbody2D rb;
    private Vector2 lastDirection;                      
    private bool isStationary = false; 
    private bool isSold = false; // 新增：标记是否已完成出售

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        currentTarget = endPos; 
        transform.position = startPos; 
        StartCoroutine(MoveRoutine()); 
        //if (sellPrompt != null) sellPrompt.SetActive(false); // 初始隐藏提示

        if (zongziPrefabs.Length > 0)
        {
            int randomIndex = Random.Range(0, zongziPrefabs.Length); 
            for (int i = 0; i < zongziPrefabs.Length; i++)
            {
                zongziPrefabs[i].SetActive(false);
            }
            Zongzi = zongziPrefabs[randomIndex]; 
            Zongzi.SetActive(false); // 初始隐藏粽子图标
        }        
    }

    void Update()
    {
        if (isStationary && isPlayerNear)
        {
            int zongziId = GetCurrentZongziId();
            if (zongziId == -1) return;

            bool isPro = IsProZongzi(zongziId);

            // 检测E键出售普通粽子（非pro类型）
            if (Input.GetKeyDown(KeyCode.E) )
            {
                SellZongzi();
            }

        }
    }

    private void SellZongzi()
    {
        int zongziId = GetCurrentZongziId();
        if (zongziId == -1)
        {
            Debug.LogWarning("未找到当前显示的粽子类型！");
            return;
        }

        bool success = ItemManager.Instance.RemoveItem(zongziId, 1);
        if (success)
        {
            int price = GetZongziPrice(zongziId);
            MoneyControl.Instance.UpdateCurrency(price);
            Debug.Log($"出售成功，获得{price}货币");
            isSold = true; // 触发提前离开
            //sellPrompt.SetActive(false); // 隐藏出售提示
            Zongzi.SetActive(false); // 隐藏粽子图标
        }
        else
        {
            Debug.LogWarning("玩家没有该粽子，无法出售！");
        }
    }

    private int GetCurrentZongziId()
    {
        for (int i = 0; i < zongziPrefabs.Length; i++)
        {
            if (zongziPrefabs[i].activeSelf)
            {
                switch (i)
                {
                    case 0: return 7;  // 白粽子
                    case 1: return 6;  // 红豆粽子
                    case 2: return 8;  // 肉粽子
                    case 3: return 9;  // 蛋粽子
                    case 4: return 11; // pro白粽子
                    case 5: return 10; // pro红豆粽子
                    case 6: return 12; // pro肉粽子
                    case 7: return 13; // pro蛋粽子
                }
            }
        }
        return -1;
    }

    private int GetZongziPrice(int zongziId)
    {
        switch (zongziId)
        {
            case 7: return 8;   // 白粽子价格
            case 6: return 12;  // 红豆粽子价格
            case 8: return 20;  // 肉粽子价格
            case 9: return 18;  // 蛋粽子价格
            // pro粽子价格 = 普通价格 + 3
            case 11: return GetZongziPrice(7) + 3; // pro白粽子
            case 10: return GetZongziPrice(6) + 3; // pro红豆粽子
            case 12: return GetZongziPrice(8) + 3; // pro肉粽子
            case 13: return GetZongziPrice(9) + 3; // pro蛋粽子
            default: return 0;
        }
    }

    // 新增：判断是否为pro粽子
    private bool IsProZongzi(int id)
    {
        Debug.Log($"当前粽子ID：{id}，是否是Pro？{id == 10 || id == 11 || id == 12 || id == 13}"); // 新增日志
        return id == 10 || id == 11 || id == 12 || id == 13;
    }

    void UpdateAnimation(Vector2 direction)
    {
        if (direction != Vector2.zero)
        {
            lastDirection = direction; 
        }
        
        animator.SetFloat("Horizontal", lastDirection.x);
        animator.SetFloat("Vertical", lastDirection.y);
        animator.SetFloat("Speed", direction.sqrMagnitude);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && Zongzi != null && isStationary)
        {
            Zongzi.SetActive(true);
            isPlayerNear = true;
            //sellPrompt.SetActive(true); // 显示出售提示
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && Zongzi != null)
        {
            Zongzi.SetActive(false);
            isPlayerNear = false;
            //sellPrompt.SetActive(false); // 隐藏出售提示
        }
    }

    // 新增：声明离开事件
    public event System.Action OnCustomerLeaving;

    private System.Collections.IEnumerator MoveRoutine()
    {
        // 移动到终点（此时isStationary为false）
        yield return MoveToPosition(endPos);
        
        // 原30秒等待改为可中断的等待循环
        float maxWaitTime = 30f;
        float currentWaitTime = 0f;
        while (currentWaitTime < maxWaitTime && !isSold)
        {
            currentWaitTime += Time.deltaTime;
            yield return null;
        }
        
        // 移动回起点（此时isStationary为false）
        yield return MoveToPosition(startPos);
        
        // 新增：触发离开事件（在销毁前）
        OnCustomerLeaving?.Invoke();

        Destroy(gameObject);
    }

    private System.Collections.IEnumerator MoveToPosition(Vector2 target)
    {
        isStationary = false; 
        
        while (Vector2.Distance(transform.position, target) > 0.1f)
        {
            Vector2 direction = (target - (Vector2)transform.position).normalized;
            rb.velocity = direction * moveSpeed;
            UpdateAnimation(direction);
            yield return null;
        }
        
        rb.velocity = Vector2.zero; 
        UpdateAnimation(Vector2.zero); 
        isStationary = true; 
        lastDirection = new Vector2(0, -1); // 静止时设置方向向下
        UpdateAnimation(Vector2.zero); // 新增：强制更新动画参数，确保方向生效
    }
}
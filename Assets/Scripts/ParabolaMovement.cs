using UnityEngine;
using UnityEngine.UI;

public class ParabolaMovement : MonoBehaviour
{
    [Header("路径参数（基于Canvas的坐标）")]
    public Vector2 startPoint = new Vector2(-30, -180);  // 起点（Canvas空间坐标）
    public Vector2 controlPoint = new Vector2(200, -100); // 中间控制点
    public Vector2 endPoint = new Vector2(430, -160);    // 终点
    
    [Header("运动参数")]
    public float totalDuration = 3f;  // 总运动时间（秒）
    public bool loop = true;          // 是否循环运动

    [Header("状态判断阈值（可外部修改）")]
    [Tooltip("普通状态区间1起点X坐标")]
    public float ordinaryStart1 = -80f;
    [Tooltip("普通状态区间1终点X坐标")]
    public float ordinaryEnd1 = 7f;
    [Tooltip("普通状态区间2起点X坐标")]
    public float ordinaryStart2 = 60f;
    [Tooltip("普通状态区间2终点X坐标")]
    public float ordinaryEnd2 = 143f;

    [Header("状态输出")]
    [Tooltip("当前状态：ordinary/good/bad")]
    public string currentState = "bad"; // 初始状态

    private float currentTime = 0f;   // 当前已运动时间
    private RectTransform rectTrans;  // UI元素的RectTransform组件
    private bool isMoving = true;     // 运动状态控制变量（true=运动中，false=停止）
    private string previousState;     // 记录上一次状态
    public event System.Action<string> StateChanged; // 状态变化事件

    void Start()
    {
        rectTrans = GetComponent<RectTransform>();
        if (rectTrans == null)
        {
            Debug.LogError("当前对象不是UI元素，缺少RectTransform组件！");
        }
        previousState = currentState; // 初始化上一次状态
    }

    void Update()
    {
        if (rectTrans == null) return;

        if (isMoving)
        {
            currentTime += Time.deltaTime;
        }
        
        float t = Mathf.Min(currentTime / totalDuration, 1f); 

        Vector2 currentPos = 
            (1 - t) * (1 - t) * startPoint + 
            2 * t * (1 - t) * controlPoint + 
            t * t * endPoint;

        rectTrans.anchoredPosition = currentPos;

        // 状态判断逻辑（使用公共变量）
        float currentX = currentPos.x;
        if ((currentX >= ordinaryStart1 && currentX <= ordinaryEnd1) || (currentX >= ordinaryStart2 && currentX <= ordinaryEnd2))
        {
            currentState = "ordinary";
        }
        else if (currentX > ordinaryEnd1 && currentX < ordinaryStart2)
        {
            currentState = "good";
        }
        else
        {
            currentState = "bad";
        }

        // 检测状态变化并触发事件
        if (currentState != previousState)
        {
            previousState = currentState;
            StateChanged?.Invoke(currentState); // 触发状态变化事件
        }

        // 循环控制（仅当isMoving为true时处理循环）
        if (t >= 1f && loop && isMoving)
        {
            currentTime = 0f; 
        }

        //Debug.Log($"当前x={currentX}，状态={currentState}");
    }

    /// <summary>
    /// 停止木棍运动（用户调用此方法即可停止）
    /// </summary>
    public void StopMovement()
    {
        isMoving = false;
    }

    // 可选：添加恢复运动的方法（如需恢复可取消注释）
    public void ResumeMovement()
    {
         isMoving = true;
    }

    /// <summary>
    /// 切换运动状态（第一次点击暂停，第二次点击继续）
    /// </summary>
    public void ToggleMovement()
    {
        isMoving = !isMoving; // 取反当前状态
        // 可选：添加状态变化时的额外逻辑（如音效）
        // if(isMoving) Debug.Log("继续运动");
        // else Debug.Log("暂停运动");
    }

    /// <summary>
    /// 让木棍回到初始点（起点）
    /// </summary>
    public void HomingToStartPoint()
    {
        // 检查并尝试初始化rectTrans
        if (rectTrans == null)
        {
            rectTrans = GetComponent<RectTransform>();
            if (rectTrans == null)
            {
                Debug.LogError("RectTransform组件未初始化，无法归位！");
                return;
            }
        }

        // 重置位置为起点
        rectTrans.anchoredPosition = startPoint;
        // 重置运动时间（下次运动时从起点重新开始）
        currentTime = 0f;
        // 可选：停止当前运动（根据需求决定是否添加）
        isMoving = true;
    }
}
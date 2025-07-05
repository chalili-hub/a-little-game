using UnityEngine;
using UnityEngine.SceneManagement;

public class DragonBoat : MonoBehaviour
{
    // 新增：动画控制器数组，用于存储不同龙舟的动画
    public RuntimeAnimatorController[] animatorControllers;
    
    public float stopXPosition = 120f;
    public float rightSpeed = 2f;
    public float verticalSpeed = 3f;
    public float slowdownDuration = 1f;

    public float originalRightSpeed;
    private float originalVerticalSpeed;
    private bool isSlowingDown = false;
    private float slowdownTimer = 0f;
    private bool isStopped = false;
    // 新增：动画组件引用
    private Animator animator;

    private void Start()
    {
        // 获取动画组件
        animator = GetComponent<Animator>();
        
        int selectedIndex = DragonBoatManager.Instance.GetSelectedIndex();    
        DragonBoatStats currentStats = DragonBoatManager.Instance.GetCurrentStats(selectedIndex); 

        // 新增：将1-3的龙舟索引转换为0-2的动画索引
        int animationIndex = selectedIndex - 1;
        
        // 修改：使用转换后的索引访问动画控制器数组
        if (animator != null && animatorControllers != null && animationIndex >= 0 && animationIndex < animatorControllers.Length)
        {
            animator.runtimeAnimatorController = animatorControllers[animationIndex];
            animator.Play("Idle");
        }
        else
        {
            Debug.LogWarning("无效的龙舟索引或动画控制器未分配");
        }

        rightSpeed = currentStats.moveSpeed;    
        verticalSpeed = currentStats.dexterity;    
        slowdownDuration = 1.5f - currentStats.sturdiness * 0.1f; 

        originalRightSpeed = rightSpeed;    
        originalVerticalSpeed = verticalSpeed;
    }

    private void Update()
    {
        // 新增：检查是否到达停止条件
        if (!isStopped && transform.position.x >= stopXPosition)
        {
            isStopped = true; // 标记为已停止
            rightSpeed = 0f; // 停止横向移动
            verticalSpeed = 0f; // 停止纵向移动
            SceneManager.LoadScene("winScene"); // 跳转场景
            return; // 提前退出，避免执行后续移动逻辑
        }

        // 处理减速计时（无论是否减速都执行）
        if (isSlowingDown)
        {
            slowdownTimer += Time.deltaTime;
            if (slowdownTimer >= slowdownDuration)
            {
                rightSpeed = originalRightSpeed;
                verticalSpeed = originalVerticalSpeed;
                isSlowingDown = false;
                slowdownTimer = 0f;
                Debug.Log("结束减速");
            }
        }

        // 移动逻辑（仅当未停止时执行）
        if (!isStopped)
        {
            float verticalInput = 0f;
            if (Input.GetKey(KeyCode.W)) verticalInput = 1f;
            else if (Input.GetKey(KeyCode.S)) verticalInput = -1f;

            transform.Translate(Vector3.right * rightSpeed * Time.deltaTime);
            transform.Translate(Vector3.up * verticalInput * verticalSpeed * Time.deltaTime);
        }

        // 移动时播放划桨动画
        if (!isStopped && animator != null)
        {
            animator.SetBool("IsMoving", true);
        }
        else if (animator != null)
        {
            animator.SetBool("IsMoving", false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Duck") || other.CompareTag("Lotus")) {
            if (!isSlowingDown && !isStopped) { // 减速状态时!isSlowingDown为false，不会触发
                rightSpeed = originalRightSpeed * 0.5f;
                verticalSpeed = originalVerticalSpeed * 0.5f;
                isSlowingDown = true;
                Debug.Log("开始减速");
            }
        }
    }
}
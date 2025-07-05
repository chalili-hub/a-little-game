using UnityEngine;

public class Duck : MonoBehaviour
{
    // 上边界Y坐标（公有变量，直接输入数值）
    public float topYBoundary = 3f;
    // 下边界Y坐标（公有变量，直接输入数值）
    public float bottomYBoundary = -3f;
    // 移动速度（公有变量，可在Inspector面板调整）
    public float speed = 2f;

    private float currentDirection; // 当前移动方向（1为上，-1为下）

    private void Start()
    {
        // 初始化随机方向（游戏开始时随机选择向上或向下）
        currentDirection = Random.value > 0.5f ? 1f : -1f;
    }

    private void Update()
    {
        // 上下移动
        transform.Translate(Vector3.up * currentDirection * speed * Time.deltaTime);

        // 检查是否超出上边界
        if (transform.position.y > topYBoundary)
        {
            currentDirection = -1f; // 反向为向下
            transform.position = new Vector3(transform.position.x, topYBoundary, transform.position.z); // 限制在边界位置
        }

        // 检查是否超出下边界
        if (transform.position.y < bottomYBoundary)
        {
            currentDirection = 1f; // 反向为向上
            transform.position = new Vector3(transform.position.x, bottomYBoundary, transform.position.z); // 限制在边界位置
        }
    }
}
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CustomerManager : MonoBehaviour
{
    public GameObject customerPrefab; // 顾客预制体（需在Inspector关联）
    public Vector2 startPosition = new Vector2(5.5f, -25f); // 顾客起始位置（可根据需求调整）
    [Tooltip("顾客生成间隔时间（秒）")]  // 新增：提示信息
    public float spawnInterval = 20f;  // 新增：可配置的生成间隔
    private readonly Vector2[] targetPositions = { 
        new Vector2(-170, -5), 
        new Vector2(-175, -5), 
        new Vector2(-170, -10), 
        new Vector2(-175, -10)  // 注意：此处与第二个坐标重复，实际使用时可根据需求调整
    };
    private readonly HashSet<Vector2> occupiedPositions = new HashSet<Vector2>();

    void Start()
    {
        StartCoroutine(SpawnCustomerRoutine());
    }

    private IEnumerator SpawnCustomerRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);  // 使用可配置变量代替固定值
            TrySpawnCustomer();
        }
    }

    private void TrySpawnCustomer()
    {
        // 收集所有未被占用的坐标
        List<Vector2> availablePositions = new List<Vector2>();
        foreach (var pos in targetPositions)
        {
            if (!occupiedPositions.Contains(pos))
            {
                availablePositions.Add(pos);
            }
        }

        if (availablePositions.Count == 0)
        {
            Debug.LogWarning("所有目标坐标已被占用，不生成新顾客");
            return;
        }

        // 随机选择一个可用坐标
        int randomIndex = Random.Range(0, availablePositions.Count);
        Vector2 selectedPos = availablePositions[randomIndex];

        // 实例化顾客并设置参数
        GameObject newCustomer = Instantiate(customerPrefab, startPosition, Quaternion.identity);
        CustomerMovement customerMovement = newCustomer.GetComponent<CustomerMovement>();
        
        if (customerMovement != null)
        {
            customerMovement.endPos = selectedPos;  // 设置目标坐标
            customerMovement.OnCustomerLeaving += () => occupiedPositions.Remove(selectedPos);  // 注册离开事件
            occupiedPositions.Add(selectedPos);  // 标记坐标为已占用
        }
        else
        {
            Debug.LogError("顾客预制体未绑定CustomerMovement组件");
            Destroy(newCustomer);
        }
    }
}
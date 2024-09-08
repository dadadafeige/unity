using UnityEngine;

public class RandomMovement : MonoBehaviour
{
    public float moveSpeed = 2f; // 移动速度
    private float moveRange = 1f; // 移动范围
    private Vector3 initialPosition; // 初始位置
    private Vector3 targetPosition; // 目标位置

    void Start()
    {
        // 初始化初始位置和目标位置为对象的当前位置
        initialPosition = transform.position;
        targetPosition = initialPosition;
    }

    void Update()
    {
        // 如果到达目标位置，则选择一个新的随机目标位置
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            ChooseNewTargetPosition();
        }
        

        // 移动到目标位置
        MoveTowardsTarget();
    }

    void ChooseNewTargetPosition()
    {
        // 在初始位置的范围内选择一个新的随机目标位置
        float randomX = Random.Range(-moveRange, moveRange);
        float randomY = Random.Range(-moveRange, moveRange);
        targetPosition = new Vector3(initialPosition.x + randomX, initialPosition.y + randomY, initialPosition.z);
    }
    // 鼠标按下事件处理方法
   
    void MoveTowardsTarget()
    {
        // 移动到目标位置
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }
}

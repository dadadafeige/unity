using UnityEngine;

public class CollectMaterialPlayerNode : MonoBehaviour
{
    // 设置角色的移动速度
    public float moveSpeed = 5f;

    // 向上移动
    public void MoveUp()
    {
        transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);
    }

    // 向下移动
    public void MoveDown()
    {
        transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);
    }

    // 向左移动
    public void MoveLeft()
    {
        transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
    }

    // 向右移动
    public void MoveRight()
    {
        transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
    }
}

using UnityEngine;

public class CollectMaterialPlayerNode : MonoBehaviour
{
    // ���ý�ɫ���ƶ��ٶ�
    public float moveSpeed = 5f;

    // �����ƶ�
    public void MoveUp()
    {
        transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);
    }

    // �����ƶ�
    public void MoveDown()
    {
        transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);
    }

    // �����ƶ�
    public void MoveLeft()
    {
        transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
    }

    // �����ƶ�
    public void MoveRight()
    {
        transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
    }
}

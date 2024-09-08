using UnityEngine;

public class RandomMovement : MonoBehaviour
{
    public float moveSpeed = 2f; // �ƶ��ٶ�
    private float moveRange = 1f; // �ƶ���Χ
    private Vector3 initialPosition; // ��ʼλ��
    private Vector3 targetPosition; // Ŀ��λ��

    void Start()
    {
        // ��ʼ����ʼλ�ú�Ŀ��λ��Ϊ����ĵ�ǰλ��
        initialPosition = transform.position;
        targetPosition = initialPosition;
    }

    void Update()
    {
        // �������Ŀ��λ�ã���ѡ��һ���µ����Ŀ��λ��
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            ChooseNewTargetPosition();
        }
        

        // �ƶ���Ŀ��λ��
        MoveTowardsTarget();
    }

    void ChooseNewTargetPosition()
    {
        // �ڳ�ʼλ�õķ�Χ��ѡ��һ���µ����Ŀ��λ��
        float randomX = Random.Range(-moveRange, moveRange);
        float randomY = Random.Range(-moveRange, moveRange);
        targetPosition = new Vector3(initialPosition.x + randomX, initialPosition.y + randomY, initialPosition.z);
    }
    // ��갴���¼�������
   
    void MoveTowardsTarget()
    {
        // �ƶ���Ŀ��λ��
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }
}

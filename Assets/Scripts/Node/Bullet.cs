using DG.Tweening;
using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifetime = 2f; // �ӵ�����������
    private float timeSinceSpawned;
    public BulletPool pool;
    public float speed = 20f; // �ӵ��ٶ�
    private Vector3 direction;
    public Action<Bullet,Collider2D> action;
    public Tween tween;
    void OnEnable()
    {
        timeSinceSpawned = 0f;
    }

    void Update()
    {
        timeSinceSpawned += Time.deltaTime;
        if (timeSinceSpawned >= lifetime)
        {
            // ���ӵ��������ڽ���ʱ�����ض����
            pool.ReturnBullet(gameObject);
        }
    }
    public void Initialize(Vector3 fireDirection)
    {
        direction = fireDirection.normalized;
    }

    public void MoveBullet()
    {
        tween = transform.DOMove(transform.position + direction * speed * lifetime, lifetime).SetEase(Ease.Linear).OnComplete(() =>
        {
            // ���ӵ��������ڽ���ʱ�����ض����
            pool.ReturnBullet(gameObject);
        });
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // �����ӵ���ײ
        // ...
        action.Invoke(this, other);
        // ���ض����
        pool.ReturnBullet(gameObject);
    }
}

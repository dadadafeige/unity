using DG.Tweening;
using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifetime = 2f; // 子弹的生命周期
    private float timeSinceSpawned;
    public BulletPool pool;
    public float speed = 20f; // 子弹速度
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
            // 当子弹生命周期结束时，返回对象池
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
            // 当子弹生命周期结束时，返回对象池
            pool.ReturnBullet(gameObject);
        });
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // 处理子弹碰撞
        // ...
        action.Invoke(this, other);
        // 返回对象池
        pool.ReturnBullet(gameObject);
    }
}

using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    
    public GameObject bulletPrefab;
    public int poolSize = 10;
    private List<GameObject> bulletPool = new List<GameObject>();

    public GameObject GetBullet(Action<Bullet, Collider2D> action)
    {
        foreach (GameObject bullet in bulletPool)
        {
            if (!bullet.activeInHierarchy)
            {
                bullet.SetActive(true);
                return bullet;
            }
        }

        // �������û�п��õ��ӵ����򴴽�һ���µ��ӵ�
        GameObject newBullet = Instantiate(bulletPrefab);
        newBullet.SetActive(true);
        bulletPool.Add(newBullet);
        Bullet bulletScript = newBullet.GetComponent<Bullet>();
        bulletScript.action = action;
        return newBullet;
    }

    public void ReturnBullet(GameObject bullet)
    {
        bullet.SetActive(false);
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        bulletScript.tween.Kill();
    }
    
}

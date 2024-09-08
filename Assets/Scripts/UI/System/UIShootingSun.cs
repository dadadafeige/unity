using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIShootingSun : UIBase
{
    public Image image; // 需要跟随旋转的Image
    public Transform point; // 发射射线的起点
    public DragonBonesController dragon;
    public Transform gongjian;
    public GameObject jian_item;
    private float bulletSpeed = 20f;
    public BulletPool pool;
    private int right_num = 0;
    public Image pro;
    public Button close_btn;
    public TextMeshProUGUI sun_num;
    public List<DragonBonesController> sun_list;
    private bool isStart = false;
    Tween tween;
    public Button rule_btn;
    // Start is called before the first frame update
    public override void OnStart()
    {
        pool.bulletPrefab = jian_item;
 
        MissionManage.ShowDescription(() => {
            Common.Instance.ShowBones("youxikaishi_bones", () =>
            {
                StartGame();
            });
        });
        rule_btn.onClick.AddListener(() =>
        {
            tween.Pause();
            isStart = false;
            MissionManage.ShowDescription(() =>
            {
                if (tween != null)
                {
                    tween.Play();
                    isStart = true;
                }
  
            });

        });


    }
    // Update is called once per frame
    private void StartGame()
    {
        right_num = 0;
        sun_num.text = "<sprite=" + right_num + ">";
        pro.fillAmount = 1;
        tween = pro.DOFillAmount(0, 30);
        for (int i = 0; i < sun_list.Count; i++)
        {
            sun_list[i].gameObject.SetActive(true);
        }
        tween.onComplete = () =>
        {
            tween = null;
            Common.Instance.ShowSettleUI(2, MissionManage.GetCurrdDrop(1), () =>
            {
                StartGame();
            }, () => { CloseSelf(); }, () =>
            {
                

            });
            isStart = false;
            

        };
        isStart = true;
    }
    

    void Update()
    {
        if (!isStart)
        {
            return;
        }
        // 获取鼠标在屏幕上的位置
        Vector3 mousePosition = Input.mousePosition;

        // 将鼠标位置转换为世界坐标
        Vector3 worldMousePosition = UiManager.uiCamera.ScreenToWorldPoint(mousePosition);
        worldMousePosition.z = 0; // 确保Z坐标为0，因为这是2D场景

        // 计算从点到鼠标位置的方向向量
        Vector3 direction = worldMousePosition - point.position;

        // 计算从方向向量到旋转角度
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // 将角度应用到Image上
        image.rectTransform.rotation = Quaternion.Euler(0, 0, angle - 180);
        gongjian.transform.rotation = Quaternion.Euler(0, 0, angle);
        // 检测鼠标左键按下事件
        if (Input.GetMouseButtonDown(0))
        {
            OnMouseDown();
        }

        // 检测鼠标左键抬起事件
        if (Input.GetMouseButtonUp(0))
        {
            OnMouseUp();
            Fire(angle);
        }
    }
    void Fire(float angle)
    {
        // 从对象池获取子弹
        GameObject bullet = pool.GetBullet( (Bullet bullet1, Collider2D cv) =>
        {
            right_num++;
            sun_num.text = "<sprite=" + right_num + ">";
            cv.gameObject.SetActive(false);
            if (right_num == 9)
            {
                tween.Kill();
                isStart = false;
                Common.Instance.ShowSettleUI(3, MissionManage.GetCurrdDrop(1), () =>
                {
                    StartGame();
                }, () => { CloseSelf(); }, () =>
                {


                });
            }

        });
        bullet.transform.SetParent(transform);
        bullet.transform.position = point.position;
        bullet.transform.rotation = Quaternion.Euler(0, 0, angle - 180);
        bullet.transform.localScale = Vector3.one;

      

        // 初始化子弹方向
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        Vector3 fireDirection = Quaternion.Euler(0, 0, angle - 90) * Vector3.up; // 假设子弹沿 up 方向发射
        bulletScript.Initialize(fireDirection);
        bulletScript.MoveBullet();
    }
    void OnMouseDown()
    {
        Debug.Log("鼠标按下");
        dragon.PlayAnimation("02_Pull",false);
        // 在这里添加鼠标按下时的处理逻辑

    }
    // 鼠标抬起事件处理方法
    void OnMouseUp()
    {
        dragon.PlayAnimation("01_Attack", false);
        Debug.Log("鼠标抬起");
        // 在这里添加鼠标抬起时的处理逻辑
    }
    public override void OnDestroyImp()
    {
  
    }
}


using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIShootingSun : UIBase
{
    public Image image; // ��Ҫ������ת��Image
    public Transform point; // �������ߵ����
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
        // ��ȡ�������Ļ�ϵ�λ��
        Vector3 mousePosition = Input.mousePosition;

        // �����λ��ת��Ϊ��������
        Vector3 worldMousePosition = UiManager.uiCamera.ScreenToWorldPoint(mousePosition);
        worldMousePosition.z = 0; // ȷ��Z����Ϊ0����Ϊ����2D����

        // ����ӵ㵽���λ�õķ�������
        Vector3 direction = worldMousePosition - point.position;

        // ����ӷ�����������ת�Ƕ�
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // ���Ƕ�Ӧ�õ�Image��
        image.rectTransform.rotation = Quaternion.Euler(0, 0, angle - 180);
        gongjian.transform.rotation = Quaternion.Euler(0, 0, angle);
        // ��������������¼�
        if (Input.GetMouseButtonDown(0))
        {
            OnMouseDown();
        }

        // ���������̧���¼�
        if (Input.GetMouseButtonUp(0))
        {
            OnMouseUp();
            Fire(angle);
        }
    }
    void Fire(float angle)
    {
        // �Ӷ���ػ�ȡ�ӵ�
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

      

        // ��ʼ���ӵ�����
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        Vector3 fireDirection = Quaternion.Euler(0, 0, angle - 90) * Vector3.up; // �����ӵ��� up ������
        bulletScript.Initialize(fireDirection);
        bulletScript.MoveBullet();
    }
    void OnMouseDown()
    {
        Debug.Log("��갴��");
        dragon.PlayAnimation("02_Pull",false);
        // �����������갴��ʱ�Ĵ����߼�

    }
    // ���̧���¼�������
    void OnMouseUp()
    {
        dragon.PlayAnimation("01_Attack", false);
        Debug.Log("���̧��");
        // ������������̧��ʱ�Ĵ����߼�
    }
    public override void OnDestroyImp()
    {
  
    }
}


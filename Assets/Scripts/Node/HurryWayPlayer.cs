using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurryWayPlayer : MonoBehaviour
{
    public DragonBonesController dragon;
    public UIHurryWay hurryWayUI;
    enum ColorType
    {
        hong,
        jin,
        an
    }
    private ColorType colorType = ColorType.an;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("collision.name:"+ collision.name);
        if (collision.name == "color1")
        {
            colorType = ColorType.jin;
            dragon.PlayAnimation("03_Jin_Idle", true);
            collision.gameObject.SetActive(false);
        }
        else if (collision.name == "color2")
        {
            colorType = ColorType.hong;
            dragon.PlayAnimation("02_Huo_Idle", true);
            collision.gameObject.SetActive(false);
        }
        else if (collision.name == "color3")
        {
            colorType = ColorType.an;
            dragon.PlayAnimation("01_An_Idle", true);

            collision.gameObject.SetActive(false);
        }
        else if (collision.name == "hong_hurry_way(Clone)")
        {
            if (colorType == ColorType.hong)
            {
                DragonBonesController dragon = collision.GetComponent<DragonBonesController>();
                dragon.PlayAnimation("02_ChengGong", false, () =>
                {
                    dragon.gameObject.SetActive(false);

                });
            }
            else
            {

                Common.Instance.ShowSettleUI(2, MissionManage.GetCurrdDrop(1), () =>
                {
                    hurryWayUI.Reset();
                }, () => { hurryWayUI.CloseSelf(); }, () =>
                {
                  

                });
                hurryWayUI.isFinish = true;
  
            }
        }
        else if (collision.name == "jin_hurry_way(Clone)")
        {
            if (colorType == ColorType.jin)
            {
                DragonBonesController dragon = collision.GetComponent<DragonBonesController>();
                dragon.PlayAnimation("02_ChengGong", false, () =>
                {
                    dragon.gameObject.SetActive(false);

                });
            }
            else
            {
                Common.Instance.ShowSettleUI(2, MissionManage.GetCurrdDrop(1), () =>
                {
                    hurryWayUI.Reset();
                }, () => { hurryWayUI.CloseSelf(); }, () =>
                {


                });
                hurryWayUI.isFinish = true;
            }
        }
        else if (collision.name == "an_hurry_way(Clone)")
        {
            if (colorType == ColorType.an)
            {
                DragonBonesController dragon = collision.GetComponent<DragonBonesController>();
                dragon.PlayAnimation("02_ChengGong", false, () =>
                {
                    dragon.gameObject.SetActive(false);

                });
            }
            else
            {
                Common.Instance.ShowSettleUI(2, MissionManage.GetCurrdDrop(1), () =>
                {
                    hurryWayUI.Reset();
                }, () => { hurryWayUI.CloseSelf(); }, () =>
                {


                });
                hurryWayUI.isFinish = true;

            }
        }
       
    }
}

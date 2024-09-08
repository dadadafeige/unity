using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class FindSomethingBtn
{
    public Button btn;
    public GameObject item;
    public bool isPrompt = false;
    public GameObject something_red;


}
public class UIFindSomething : UIBase
{
    // Start is called before the first frame update
    public List<Button> btnList1;
    public List<Button> btnList2;
    public List<Button> btnList3;
    public List<GameObject> roots;
    public RawImage bg;
    public RectTransform item_root;
    public GameObject item;
    private List<Button> btnList;
    private List<FindSomethingBtn> currBtnList =new List<FindSomethingBtn>();
    private List<GameObject> curItem;
    private int curRightNum = 0;
    private int diff = 0;
    public RectTransform item_root2;
    public GameObject item_bg2;
    private int oneNum=999;
    public Image pro;
    public Image diffImage;
    private float time=60;
    public Button prompt_btn;
    public Button close_btn;
    private Tween tween;
    public Button rule_btn;
    public override void OnStart()
    {
       
   
        close_btn.onClick.AddListener(CloseSelf);
        prompt_btn.onClick.AddListener(() =>
        {
            if (GameManage.userData.gold > 1000)
            {
                GameManage.userData.SetAddGoldValue(-1000);
            }
            else
            {
                Common.Instance.ShowTips("金币不足，请前往行囊出售药材");
                //    callBack();
                return;
            }
            int indxe = -1;
            for (int i = 0; i < currBtnList.Count; i++)
            {
                if (!currBtnList[i].isPrompt && currBtnList[i].something_red == null)
                {
                    currBtnList[i].isPrompt = true;
                    indxe = i;
                    break;
                } 
            }
            if (indxe >= 0)
            {
                DragonBonesController go = UiManager.LoadBonesByNmae("xunbao_bones");
                go.transform.SetParent(currBtnList[indxe].btn.transform);
                go.transform.localPosition = Vector3.zero;
                go.transform.localScale = Vector3.one;
            }
            else
            {
                Common.Instance.ShowTips("已全部提示完毕");
            }
            //currBtnList[curRightNum].btn
      

        });
        MissionManage.ShowDescription(() => {
            UpdataDiff();
        });
       
        rule_btn.onClick.AddListener(() =>
        {
            tween.Pause();
            MissionManage.ShowDescription(() =>
            {
                if (tween != null)
                {
                    tween.Play();
                }
            });

        });

    }
    private void InitData()
    {
        if (tween != null)
        {
            tween.Kill();
            tween = null;
        }
        pro.fillAmount = 1;
        tween = pro.DOFillAmount(0, time).SetEase(Ease.Linear);
        tween.onComplete = ()=> {
            Common.Instance.ShowSettleUI(2, MissionManage.GetCurrdDrop(diff), () =>
            {
                UpdataDiff(false);
            }, () => { CloseSelf(); }, () =>
            {
                UpdataDiff();

            });
           


        };
       
        for (int i = 0; i < currBtnList.Count; i++)
        {
            GameObject.Destroy(currBtnList[i].item);
            if (currBtnList[i].something_red !=null)
            {
                GameObject.Destroy(currBtnList[i].something_red);
            }
        }
        currBtnList.Clear();
        for (int i = 0; i < btnList.Count; i++)
        {
            FindSomethingBtn find = new FindSomethingBtn();
            Button btn = btnList[i];
            find.btn = btnList[i];
            GameObject new_item = GameObject.Instantiate(item);
            if (i +1 >oneNum)
            {
                new_item.transform.SetParent(item_root2);
            }
            else
            {
                new_item.transform.SetParent(item_root);
            }
            new_item.transform.localPosition = Vector3.zero;
            new_item.transform.localScale = Vector3.one;
            new_item.SetActive(true);
            find.item = new_item;
            Image image = new_item.transform.Find("icon").GetComponent<Image>();
            image.sprite = UiManager.LoadSprite("find_something", "find_something"+diff+"_" + (i + 1));
            image.SetNativeSize();
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(delegate { ClickItemBtn(find); });
            currBtnList.Add(find);

        }


    }
    private void UpdataDiff(bool isAdd = true)
    {
        if (isAdd)
        {
            diff++;
        }
        curRightNum = 0;
        for (int i = 0; i < roots.Count; i++)
        {
            roots[i].SetActive(i + 1 == diff);
        }
        bg.texture = UiManager.getTextureByNmae("find_something_trexture", "find_something_bg" + diff);
        item_bg2.SetActive(diff != 1);
        item_root2.gameObject.SetActive(diff != 1);
        
        if (diff == 1)
        {
            diffImage.sprite = UiManager.LoadSprite("numbe_rmemory", "NumbeRmemory_13");
            btnList = btnList1;
        }
        else if (diff == 2)
        {
            diffImage.sprite = UiManager.LoadSprite("numbe_rmemory", "NumbeRmemory_14");
            oneNum = 5;
            btnList = btnList2;
        }
        else if (diff == 3)
        {
            diffImage.sprite = UiManager.LoadSprite("numbe_rmemory", "NumbeRmemory_15");
            oneNum = 7;
            btnList = btnList3;
        }
        InitData();

    }
    private void ClickItemBtn(FindSomethingBtn find)
    {
        if (find.something_red != null)
        {
            return;
        }
        curRightNum++;
        GameObject right = find.item.transform.Find("right").gameObject;
        right.SetActive(true);
        GameObject newObject = new GameObject("NewImageObject");
        find.something_red = newObject;
        newObject.transform.SetParent(find.btn.transform);
        newObject.transform.localPosition = Vector3.zero;
        newObject.transform.localScale = Vector3.one;
        Image newImage = newObject.AddComponent<Image>();
        newImage.sprite = UiManager.LoadSprite("find_something", "find_something_red");
        if (curRightNum == btnList.Count)
        {
            if (diff == 3)
            {
                Common.Instance.ShowSettleUI(2, MissionManage.GetCurrdDrop(diff), () =>
                {
                    UpdataDiff(false);
                }, () => { CloseSelf(); }, () =>
                {
                    UpdataDiff();

                });
            }
            else
            {
                Common.Instance.ShowSettleUI(1, MissionManage.GetCurrdDrop(diff), () =>
                {
                    UpdataDiff(false);
                }, () => { CloseSelf(); }, () =>
                {
                    UpdataDiff();

                });
            }
            

    
        }
    }


}

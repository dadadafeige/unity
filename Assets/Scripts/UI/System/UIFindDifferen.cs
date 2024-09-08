using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;



public class UIFindDifferen : UIBase
{
    public RectTransform left;
    public RectTransform right;
    Button[] left_btns;
    Button[] right_btns;
    private int curr_right_index = 0;
    public Button prompt_btn;
    private int max_right;
    public Image pro;
    private List<int> capable_prompt_index = new List<int>();
    private List<GameObject> red_list = new List<GameObject>();
    private Tween tween;
    public Button close_btn;
    public Button rule_btn;
    public TextMeshProUGUI num1;
    public TextMeshProUGUI num2;
    private int maxNum;
    // Start is called before the first frame update
    public override void OnStart()
    {
        left_btns = left.GetComponentsInChildren<Button>();
        right_btns = right.GetComponentsInChildren<Button>();
        max_right = left_btns.Length;
        PrintAllImageChildren(left);
        prompt_btn.onClick.AddListener(ClickPrompt);
        close_btn.onClick.AddListener(CloseSelf);
        MissionManage.ShowDescription(() =>{
            UpdataDiff();
        });
        //UpdataDiff();
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
    private void UpdataDiff()
    {

        for (int i = 0; i < red_list.Count; i++)
        {
            GameObject.Destroy(red_list[i]);
        }
        red_list.Clear();
        tween = pro.DOFillAmount(0, 20);
        tween.onComplete = () => {
            Common.Instance.ShowSettleUI(2, MissionManage.GetCurrdDrop(1), () =>
            {
                UpdataDiff();
            }, () => { CloseSelf(); }, () =>
            {


            });

        };
    }
    private void ClickPrompt()
    {

        if (capable_prompt_index.Count == 0)
        {

            Common.Instance.ShowTips("已全部提示完毕");
            return;
        }
        DragonBonesController go = UiManager.LoadBonesByNmae("xunbao_bones");
        go.transform.SetParent(left_btns[capable_prompt_index[0]].transform);
        go.transform.localPosition = Vector3.zero;
        go.transform.localScale = Vector3.one;
        DragonBonesController go1 = UiManager.LoadBonesByNmae("xunbao_bones");
        go1.transform.SetParent(right_btns[capable_prompt_index[0]].transform);
        go1.transform.localPosition = Vector3.zero;
        go1.transform.localScale = Vector3.one;
        //currBtnList[curRightNum].btn
        capable_prompt_index.RemoveAt(0);


    }
    void PrintAllImageChildren(Transform parent)
    {
       
        for (int i = 0; i < left_btns.Length; i++)
        {
            Button btn = left_btns[i];
          
            if (btn != null)
            {
                capable_prompt_index.Add(i);
                InitBtn(btn, i);
            }
        }
        maxNum = capable_prompt_index.Count;
        for (int i = 0; i < right_btns.Length; i++)
        {
            Button btn = right_btns[i];
            if (btn != null)
            {
                InitBtn(btn, i);
            }
        }

    }
    private void InitBtn(Button btn,int index)
    {
        num1.text = curr_right_index + "/" + max_right;
        num2.text = curr_right_index + "/" + max_right;
        btn.onClick.AddListener(() =>
        {
            capable_prompt_index.Remove(index);
            NewRedImage(left_btns[index].transform);
            NewRedImage(right_btns[index].transform);
            curr_right_index++;
            num1.text = curr_right_index + "/" + max_right;
            num2.text = curr_right_index + "/" + max_right;

        });


    }
    private void NewRedImage(Transform parent)
    {
  
        GameObject newObject = new GameObject("NewImageObject");
        red_list.Add(newObject);
        newObject.transform.SetParent(parent);
        newObject.transform.localPosition = Vector3.zero;
        newObject.transform.localScale = Vector3.one;
        Image newImage = newObject.AddComponent<Image>();
        newImage.sprite = UiManager.LoadSprite("find_something", "find_something_red");
   
        if (curr_right_index == max_right)
        {
            tween.Kill();
            Common.Instance.ShowSettleUI(3, MissionManage.GetCurrdDrop(1), () =>
            {
                UpdataDiff();
            }, () => { CloseSelf(); }, () =>
            {


            });
        }
    }
 
}

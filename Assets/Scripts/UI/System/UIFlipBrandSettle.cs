using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIFlipBrandSettle : UIBase
{
    public Image result;
    public TextMeshProUGUI qiuyin_num;
    public TextMeshProUGUI she_num;
    public TextMeshProUGUI caoyao_num;
    public TextMeshProUGUI baoxiang_num;
    public Button close_btn;
    public Button come_back_btn;
    public Button next_btn;
    public TextMeshProUGUI haoshi;
    public GameObject qiuyin;
    public GameObject she;
    public GameObject caoyao;
    public GameObject baoxiang;
    public GameObject haoshi_image;
    public override void OnAwake()
    {
      
    }
    // Start is called before the first frame update
    public override void OnStart()
    {



    }
    public void SetNum(int qiuyinNum,int sheNum,int caoyaoNum,int baoxiangNum, int mhaoshi)
    {
        qiuyin.SetActive(qiuyinNum > 0);
        she.SetActive(qiuyinNum > 0);
        caoyao.SetActive(qiuyinNum > 0);
        baoxiang.SetActive(qiuyinNum > 0);
        qiuyin_num.text = "x"+qiuyinNum;
        she_num.text = "x" + sheNum;
        caoyao_num.text = "x" + caoyaoNum;
        baoxiang_num.text = "x" + baoxiangNum;
        haoshi.text = mhaoshi.ToString();

    }  
    public void SetData(bool isSuccess,Action action, Action completeAction = null)
    {
        come_back_btn.gameObject.SetActive(!isSuccess);
        next_btn.gameObject.SetActive(isSuccess);
        close_btn.onClick.AddListener(() =>
        {
            if (completeAction != null)
            {
                completeAction();
            }
            CloseSelf();
        });
        come_back_btn.onClick.AddListener(() =>
        {
            if (completeAction != null)
            {
                action();
            }
            CloseSelf();
        });
        next_btn.onClick.AddListener(() =>
        {
            if (completeAction != null)
            {
                action();
            }
            CloseSelf();
        });
        if (isSuccess)
        {
            result.sprite = UiManager.LoadSprite("common", "success_word"); 
        }
        else
        {
            result.sprite = UiManager.LoadSprite("common", "fail_word");
        }
        result.SetNativeSize();
       
    }

}

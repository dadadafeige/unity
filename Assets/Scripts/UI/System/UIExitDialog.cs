using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIExitDialog : UIBase
{
    public Button cancelBtn;
    public Button okBtn;
    public TextMeshProUGUI label;

    private void Awake()
    {
      
    }
    // Start is called before the first frame update
    void Start()
    {
        cancelBtn.onClick.AddListener(() =>
        {
            CloseSelf();

        });
       

    }
    public void ShowUi(string str,Action callBack)
    {
        label.text = str;
        okBtn.onClick.AddListener(() =>
        {
            callBack();
            CloseSelf();

        });

    }
    public void ShowUi(string str, Action callBack, Action callCancelBack)
    {
        label.text = str;
        okBtn.onClick.AddListener(() =>
        {
            callBack();
            CloseSelf();

        });
        cancelBtn.onClick.RemoveAllListeners();
        cancelBtn.onClick.AddListener(() => {

            callCancelBack();
            CloseSelf();
        });
    }
    public void ShowUi(Action callBack)
    {
        okBtn.onClick.AddListener(() =>
        {
            callBack();
            CloseSelf();

        });

    }



}

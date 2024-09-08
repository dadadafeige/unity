using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UITipsBoard : UIBase
{
    public TextMeshProUGUI word;
    public Button okBtn;
    public Button closeBtn;
    public TextMeshProUGUI price;
    // Start is called before the first frame update
    public override void OnStart()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetData(string str,int priceNum, Action action)
    {
        word.text = str;
        price.text = priceNum.ToString();
        okBtn.onClick.AddListener(() =>
        {

            action();
            CloseSelf();

        });
        closeBtn.onClick.AddListener(CloseSelf);

    }
}

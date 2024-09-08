using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UISetUpNotice : UIBase
{
    public TextMeshProUGUI wenzi;
    public Button close_btn;
    public override void OnStart()
    {
        close_btn.onClick.AddListener(CloseSelf);
    }
    public void SetLabel(string str)
    {
        wenzi.text = str;

    }
}

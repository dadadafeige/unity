using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UISelectRole : UIBase 
{

    public Button boyBtn;
    public Button girlBtn;
    public RectTransform girl_name_root;
    public RectTransform boy_name_root;
    public RectTransform name_root;
    public Button ok_btn;
    public TextMeshProUGUI inputLabel;
    public Image make;
    private Gender selectGender;
    public TMP_InputField inputField;
    public TextMeshProUGUI inputLabel2;
    public override void OnAwake()
    {
      
    }
    // Start is called before the first frame update
    public override void OnStart()
    {

        boyBtn.onClick.AddListener(OnSelectBoy);
        girlBtn.onClick.AddListener(OnSelectGirl);
        ok_btn.onClick.AddListener(OnOkBtnClick);
        inputField.onValueChanged.AddListener(OnInputValueChanged);
        make.sprite = UiManager.LoadSprite("common", "make");
    }
    void OnSelectBoy()
    {
        boyBtn.transform.SetAsLastSibling();
        make.transform.SetSiblingIndex(1);
        name_root.SetParent(boy_name_root);
        name_root.anchoredPosition = Vector2.zero;
        selectGender = Gender.Boy;
    }
    void OnInputValueChanged(string value)
    {
        // 检查每个字符是否是数字，如果不是则移除该字符
        string newValue = "";
        foreach (char c in value)
        {
            if (char.IsDigit(c))
            {
                newValue += c;
            }
        }

        // 更新InputField的值
        inputField.text = newValue;
    }
    void OnSelectGirl()
    {
        girlBtn.transform.SetAsLastSibling();
        make.transform.SetSiblingIndex(1);
        name_root.SetParent(girl_name_root);
        name_root.anchoredPosition = Vector2.zero;
        selectGender = Gender.Girl;

    }
    void OnOkBtnClick()
    {

     
        int byteCount = inputLabel.text.Length;
        if (byteCount > 1)
        {
            if (byteCount > 5)
            {
                UITips gui1 = UiManager.OpenUI<UITips>("UITips");
                gui1.SetLabel("名字不能超过4个字");
                return;
            }
            
                        
      
        }
        else
        {
            UITips gui1 = UiManager.OpenUI<UITips>("UITips");
            gui1.SetLabel("名字不能为空");
            return;
        }
        GameManage.userData.userGender = selectGender;
        GameManage.userData.userName = inputLabel.text;
        GameManage.curStoryId = 1;
        int num;
        bool success = int.TryParse(inputField.text, out num);
        if (success)
        {
            GameManage.curMissionId = num;
            GameManage.curGameMissionId = GameManage.curMissionId;
        }
 

        UiManager.OpenUI<UIOpening_Remarks>("UIOpening_Remarks");
        CloseSelf();
    }
    public override void OnDestroyImp()
    {
     
    }


}

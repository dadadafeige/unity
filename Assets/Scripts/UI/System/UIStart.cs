using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class UIStart : UIBase
{
    public Button btn;
    public TextMeshProUGUI inputLabel;
    public TMP_InputField inputField;
    private const string phoneNumberPattern = @"^1[3-9]\d{9}$";
    private string serverURL = "http://gm.beijingliuli.top/gameManger/login";
    public GameObject logon;
    public GameObject loadingSpinner;
    private string masterKey = "18888888888";
    // public Button logON;
    public override void OnStart()
    {
        //btn.onClick.AddListener(() =>
        //{
        //    if (GameManage.userData.is_first)
        //    {
        //        UiManager.OpenUI<UISelectRole>("UISelectRole");
        //        CloseSelf();
        //    }
        //    else
        //    {
        //        UIPlayerStory gui = UiManager.OpenUI<UIPlayerStory>("UIPlayerStory");
        //        UiManager.uIPlayer = gui;
        //    }


        //});
        btn.onClick.AddListener(ValidatePhoneNumber);
        loadingSpinner.SetActive(true);
        logon.SetActive(false);
    }
    public void InitOk()
    {

        loadingSpinner.SetActive(false);
        logon.SetActive(true);


    }
    public void ValidatePhoneNumber()
    {
        string phoneNumber = inputField.text;
        if (phoneNumber == masterKey)
        {
            if (GameManage.userData.is_first)
            {
                UiManager.OpenUI<UISelectRole>("UISelectRole");
                CloseSelf();
            }
            else
            {
                UIPlayerStory gui = UiManager.OpenUI<UIPlayerStory>("UIPlayerStory");
                UiManager.uIPlayer = gui;
                CloseSelf();
            }
            return;
        }
        if (System.Text.RegularExpressions.Regex.IsMatch(phoneNumber, phoneNumberPattern))
        {
            StartCoroutine(SendPostRequest());
            //  CloseSelf();
        }
        else
        {

            UITips gui = UiManager.OpenUI<UITips>("UITips");
            gui.SetLabel("请输入正确的手机号");


        }
    }
    IEnumerator SendPostRequest()
    {
        using (UnityWebRequest webRequest = new UnityWebRequest(serverURL, "POST"))//第二种写法此行注释
        {
            //UnityWebRequest webRequest = new UnityWebRequest(url, "POST");//第二种写法此行取消注释
            string lebel = inputLabel.text;
            UpDataObject upDataObject = new UpDataObject(lebel, "111111");
            string postData = JsonUtility.ToJson(upDataObject);
       
            // 设置请求头
            webRequest.SetRequestHeader("Content-Type", "application/json");

            // 将JSON数据放入请求体
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(postData);
            webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
            webRequest.downloadHandler = new DownloadHandlerBuffer();

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                DownDataObject downDataObject = JsonUtility.FromJson<DownDataObject>(webRequest.downloadHandler.text);
     
                if (downDataObject.code == 0)
                {
                    // "{\"success\":true,\"message\":\"\",\"code\":0,\"result\":{\"userId\":1,\"mobile\":\"13702144685\"},\"timestamp\":1708417896531}"
                    if (GameManage.userData.is_first)
                    {
                        UiManager.OpenUI<UISelectRole>("UISelectRole");
                        CloseSelf();
                    }
                    else
                    {
                        UIPlayerStory gui = UiManager.OpenUI<UIPlayerStory>("UIPlayerStory");
                        UiManager.uIPlayer = gui;
                    }
                    Common.Instance.ShowTips("登录成功");
                    GameManage.phone = downDataObject.result.mobile;
                    GameManage.caseInfos = downDataObject.result.caseInfos;
                    CloseSelf();

                }
                else
                {
                    Common.Instance.ShowTips(downDataObject.message);
                  

                }
                // string result = webRequest.downloadHandler.text;

            }
            else
            {
                Debug.LogError(webRequest.error);

            }
        }
    }
}
[System.Serializable]
public class UpDataObject
{
    public string mobileNumber;
    public string smsVerificationCode;
    public UpDataObject(string mobileNumber, string smsVerificationCode)
    {
        this.mobileNumber = mobileNumber;
        this.smsVerificationCode = smsVerificationCode;

    }
    // 添加其他属性以匹配服务器返回的数据
}
[System.Serializable]
public class DownDataObject
{
    public int code;
    public string message;
    public bool success;
    public int timestamp;
    public Result result;

}
[System.Serializable]
public class Result
{
    public string mobile;
    public int userId;
    public List<CaseInfo> caseInfos;


}
[System.Serializable]
public class CaseInfo
{
    public int caseId;
    public string caseName;
    public int statusId;


}

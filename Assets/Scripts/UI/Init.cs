using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    //void Start()
    //{
    //    //string path1 = Application.streamingAssetsPath + "/Windows/AssetBundles/assetbundle/spine/spine_1.assetbundle";   //资源包路径
    //    //                                                                                                                  //   string path2 = "ChinarAssetBundles/chinar/sprite.unity3d";     //子选项精灵图片文件路径
    //    //AssetBundle ab1 = AssetBundle.LoadFromFile(path1);                //资源1：直接读出资源
    //    //object sphereHead = ab1.LoadAsset("spine_1");
    //    //Instantiate((GameObject)sphereHead);
    //    // AssetBundle ab2 = AssetBundle.LoadFromFile(path2);

    //    // 初始化 AssetBundleManager，传入 AssetBundleManifest 路径和 AssetBundle 文件夹路径


    //}

    // Use this for initialization
    UIStart uIStart;
    void Start()
    {
        //string manifestPath = Application.streamingAssetsPath + "/Windows/AssetBundles/AssetBundles";
        //string assetBundleFolderPath = Application.streamingAssetsPath + "/Windows/AssetBundles/assetbundle/";
        //AssetBundleManager.Instance.Initialize(manifestPath, assetBundleFolderPath);
        //// 加载 AssetBundle 中的资源
        //string assetBundleName = "ui/uilogon";
        //string assetName = "uilogon";
        //GameObject myPrefab = AssetBundleManager.Instance.LoadAsset<GameObject>(assetBundleName, assetName);
        //if (myPrefab != null)
        //{
        //    // 使用资源
        //    Instantiate(myPrefab);
        //}
        //else
        //{
        //    Debug.LogError("Failed to load asset from AssetBundle: " + assetBundleFolderPath + assetBundleName);
        //}
        //AssetBundleManager.Instance.UnloadAssetBundle(assetBundleName);
        Debug.Log(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>");
        GameManage.InIt();
        UiManager.InitUI();
        BagManage.Instance.LoadItems();
        MissionManage.LoadItems();
        GameManage.LoadUserData();

        uIStart = UiManager.OpenUI<UIStart>("UIStart");
   
        StartCoroutine(SendPostRequest());
        //  BattleManager.Instance.StartBattle();
        //  UiManager.OpenUI<UIOpening_Remarks>("UIOpening_Remarks");
        //uIMain.OpenNewBie();
        //GameObject gameObject = UiManager.LoadBonesByNmae("luohou_bones").gameObject;
        //gameObject.SetActive(true);
        //gameObject.transform.SetParent(gui.transform);
        //gameObject.transform.localPosition = Vector3.zero;
        //gameObject.transform.localScale = Vector3.one;

        //DelayedActionProvider.Instance.DelayedAction(() =>
        //{
        //    gameObject.SetActive(false);


        //},3);
        //  UiManager.OpenUI<UIFlipBrand>("UIFlipBrand");
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
    // Update is called once per frame
    IEnumerator SendPostRequest()
    {
        using (UnityWebRequest webRequest = new UnityWebRequest("http://gm.beijingliuli.top/gameManger/gameVersion", "GET"))//第二种写法此行注释
        {
            //UnityWebRequest webRequest = new UnityWebRequest(url, "POST");//第二种写法此行取消注释
            byte[] postBytes = System.Text.Encoding.UTF8.GetBytes("");
            webRequest.uploadHandler = (UploadHandler)new UploadHandlerRaw(postBytes);
            webRequest.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");

            yield return webRequest.SendWebRequest();
            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                string result = webRequest.downloadHandler.text;
                print(result);
                DownVersionDataObject downVersionDataObject = JsonUtility.FromJson<DownVersionDataObject>(result);

                if (downVersionDataObject.result.gameVersion == GameManage.version)
                {
                    uIStart.InitOk();
                }
                else
                {
                    Common.Instance.OpenExitDialog("版本过低是否前往下载", () =>
                    {
                        Application.Quit();
                        if (Application.platform == RuntimePlatform.WindowsPlayer)
                        {
                            Application.OpenURL(downVersionDataObject.result.windowsUrl);
                        }
                        else if (Application.platform == RuntimePlatform.Android)
                        {
                            Application.OpenURL(downVersionDataObject.result.androidUrl);
                        }
                        else if (Application.platform == RuntimePlatform.IPhonePlayer)
                        {
                            Application.OpenURL(downVersionDataObject.result.iosUrl);
                        }



                    }, () =>
                    {
                        Application.Quit();
                    });

                }
            }
            else
            {
                Debug.LogError(webRequest.error);

            }
        }
    }
}
[System.Serializable]
public class DownVersionDataObject
{
    public bool success;
    public string message;
    public int code;
    public VersionResult result;
    public long timestamp;

}
[System.Serializable]
public class VersionResult
{
    public string gameName;
    public string gameVersion;
    public string windowsUrl;
    public string androidUrl;
    public string iosUrl;

}

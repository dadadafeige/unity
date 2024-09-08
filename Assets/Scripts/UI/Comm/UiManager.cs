using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.U2D;
public enum UIOrderType
{
    UIMain,
    UIDefault,
    UITop,
    UINoFull,



};
[System.Serializable]
public class UIImageInfo
{
    public string UIName;
    public string ComponentName;
    public string SpritePath;
}

public static class UiManager
{

    private static int curStorey = 0;
    public static Camera uiCamera;
    public static Camera lineCamera;
    private static Dictionary<int, UIBase>  uiMap = new Dictionary<int, UIBase>();
    private static Dictionary<string, SpriteAtlas> atlasDictionary = new Dictionary<string, SpriteAtlas>();
    public static CustomTimer customTimer;
    static List<UIImageInfo>  imageInfoList;
    public static UIPlayerStory uIPlayer;
    public static void InitUI()
    {

        uiCamera = GameObject.Find("UICamera").GetComponent<Camera>();
        lineCamera = GameObject.Find("LineCamera").GetComponent<Camera>();
        customTimer = GameObject.Find("SplashTexture").GetComponent<CustomTimer>();



    }
    public static T OpenUI<T>(string uiName) {

        AssetBundleManager.Instance.Initialize();
        // 加载 AssetBundle 中的资源
        string assetBundleName = "ui/" + uiName;
        string assetName = uiName;
        GameObject myPrefab = AssetBundleManager.Instance.LoadAsset<GameObject>(assetBundleName, assetName);

        if (myPrefab != null)
        {
            UIBase uiBase = myPrefab.GetComponent<UIBase>();
            // 使用资源
            GameObject obj = uiBase.LoadSelf(myPrefab);
            obj.name = myPrefab.name;
            RectTransform selfRectTransform = obj.GetComponent<RectTransform>();
            Canvas mCanvas = obj.GetComponent<Canvas>();
            GameObject uiRoot = GameObject.Find("UIRoot");
            RectTransform uiRootRectTransform = uiRoot.GetComponent<RectTransform>();
            curStorey++;
            mCanvas.sortingOrder = curStorey;
            selfRectTransform.SetParent(uiRoot.transform);
            selfRectTransform.anchoredPosition = Vector2.zero;
            if (Screen.height < 1080 && uiBase.UIOrder == UIOrderType.UINoFull)
            {
                float ratio = (float)Screen.height / 1080;
                if (uiName != "UIGuessingPuzzle" && uiName != "UIBalanceBall")
                {
                    if (ratio < 0.7)
                    {
                        ratio = 0.7f;
                     
                    }
                }
                selfRectTransform.sizeDelta = new Vector2(1920, 1080);
                obj.transform.localScale = new Vector3(ratio, ratio, ratio);
            }
            else
            {
                selfRectTransform.localScale = Vector3.one;
            }
            selfRectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            selfRectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            float width = selfRectTransform.sizeDelta.x;
            float height = selfRectTransform.sizeDelta.y;
            if (uiRootRectTransform.rect.height > height)
            {
                float scale = uiRootRectTransform.rect.height / height;
                selfRectTransform.sizeDelta = new Vector2(selfRectTransform.rect.width * scale,
                    uiRootRectTransform.rect.height);
            }
            else if (uiRootRectTransform.rect.width > width)
            {
                float scale = uiRootRectTransform.rect.width / width;
                selfRectTransform.sizeDelta = new Vector2(uiRootRectTransform.rect.width,
                    selfRectTransform.rect.height * scale);
            }
            T uiNode = obj.GetComponent<T>();
            UIBase nodeBase = uiNode as UIBase;
            nodeBase.SearchForImages(nodeBase.transform);
            if (nodeBase.UIOrder != UIOrderType.UITop)
            {
                UILayerManager.Instance.AddUIObject(nodeBase);
            }
         
            //if (uiMap.ContainsKey(curStorey - 1))
            //{
            //    uiMap[curStorey - 1].OutUI();
            //}
            //uiMap.Add(curStorey, nodeBase);

            //nodeBase.GoInUI();
            return uiNode;

        }
        else
        {
            AssetBundleManager.Instance.UnloadAssetBundle(assetBundleName);
            Debug.LogError("Failed to load asset from AssetBundle: " + assetBundleName);
            return default(T);

        }



    }
    public static void CloseUI(UIBase uiBase)
    {
        GameObject.Destroy(uiBase.gameObject);
        if (uiBase.UIOrder != UIOrderType.UITop)
        {
            UILayerManager.Instance.RemoveUIObject(uiBase);
        }
  
        //if (uiMap.ContainsKey(curStorey - 1))
        //{
        //    uiMap[curStorey - 1].GoInUI();
        //}
        //uiMap.Remove(curStorey);
        //   curStorey--;
    }
    public static Texture getTextureByNmae(string textureName)
    {
        AssetBundleManager.Instance.Initialize();
        // 加载 AssetBundle 中的资源
        string assetBundleName = "images/texture/" + textureName;
        //"ui/uilogon";
        string assetName = textureName;
        //"uilogon";
        Texture myPrefab = AssetBundleManager.Instance.LoadAsset<Texture>(assetBundleName, assetName);
        return myPrefab;
    }
    public static Sprite getTextureSpriteByNmae(string textureName)
    {
        AssetBundleManager.Instance.Initialize();
        // 加载 AssetBundle 中的资源
        string assetBundleName = "images/texture/" + textureName;
        //"ui/uilogon";
        string assetName = textureName;
        //"uilogon";
        Sprite myPrefab = AssetBundleManager.Instance.LoadAsset<Sprite>(assetBundleName, assetName);
        return myPrefab;
    }
    public static Sprite getTextureSpriteByNmae(string path, string textureName)
    {
        AssetBundleManager.Instance.Initialize();
        // 加载 AssetBundle 中的资源
        string assetBundleName = "images/texture/"+ path+"/" + textureName;
        //"ui/uilogon";
        string assetName = textureName;
        //"uilogon";
        Sprite myPrefab = AssetBundleManager.Instance.LoadAsset<Sprite>(assetBundleName, assetName);
        return myPrefab;
    }
    public static Texture getTextureByNmae(string path, string textureName)
    {
        AssetBundleManager.Instance.Initialize();
        // 加载 AssetBundle 中的资源
        string assetBundleName = "images/texture/" + path+"/"+ textureName;
        //"ui/uilogon";
        string assetName = textureName;
        //"uilogon";
        Texture myPrefab = AssetBundleManager.Instance.LoadAsset<Texture>(assetBundleName, assetName);
        return myPrefab;
    }
    //public static Sprite getTextureByNmae(string spriteName)
    //{
    //    AssetBundleManager.Instance.Initialize();
    //    // 加载 AssetBundle 中的资源
    //    string assetBundleName = "images/texture/" + textureName;
    //    //"ui/uilogon";
    //    string assetName = textureName;
    //    //"uilogon";
    //    Texture myPrefab = AssetBundleManager.Instance.LoadAsset<Texture>(assetBundleName, assetName);
    //    return myPrefab;
    //}
    public static GameObject LoadSpineByNmae( string spineName)
    {
        string assetBundlePath = spineName + "/" + spineName;
        AssetBundleManager.Instance.Initialize();
        // 加载 AssetBundle 中的资源
        string assetBundleName =  "spine/" + assetBundlePath;
        //"ui/uilogon";
        string assetName = spineName;
        //"uilogon";
        Debug.Log("spine/" + assetBundlePath);
        GameObject myPrefab = AssetBundleManager.Instance.LoadAsset<GameObject>(assetBundleName, assetName);
       // GameObject go = GameObject.Instantiate(myPrefab);
        return myPrefab;
    }
    public static GameObject LoaChapterMapByNmae(string chapterMapName)
    {
        string assetBundlePath = chapterMapName;
        AssetBundleManager.Instance.Initialize();
        // 加载 AssetBundle 中的资源
        string assetBundleName = "chaptermap/" + assetBundlePath;
        //"ui/uilogon";
        string assetName = chapterMapName;
        //"uilogon";
        Debug.Log("spine/" + assetBundlePath);
        GameObject myPrefab = AssetBundleManager.Instance.LoadAsset<GameObject>(assetBundleName, assetName);
        GameObject go = GameObject.Instantiate(myPrefab);
        return go;
    }
    public static DragonBonesController LoadBonesByNmae(string bonesName)
    {
        string assetBundlePath = bonesName;
        AssetBundleManager.Instance.Initialize();
        // 加载 AssetBundle 中的资源
        string assetBundleName = "bones/" + assetBundlePath;
        //"ui/uilogon";
        string assetName = bonesName;
        //"uilogon";
        Debug.Log("bones/" + assetBundlePath);
        GameObject myPrefab = AssetBundleManager.Instance.LoadAsset<GameObject>(assetBundleName, assetName);
        if (myPrefab == null)
        {
            return null;
        }
        GameObject gameObject = GameObject.Instantiate(myPrefab);
        DragonBonesController dragonBonesController = gameObject.GetComponent<DragonBonesController>();
        // GameObject go = GameObject.Instantiate(myPrefab);
        return dragonBonesController;
    }
    // 加载图集中的图片
    public static Sprite LoadSprite(string atlasPath, string spriteName)
    {
        // 检查是否已加载过图集
        if (!atlasDictionary.ContainsKey(atlasPath))
        {
            AssetBundleManager.Instance.Initialize();
            string assetBundleName = "Images/Atlas/" + atlasPath+"/" + atlasPath;
            // 加载图集
            SpriteAtlas atlas  = AssetBundleManager.Instance.LoadAsset<SpriteAtlas>(assetBundleName, atlasPath);

            if (atlas != null)
            {
                // 将图集存入字典
                atlasDictionary[atlasPath] = atlas;
            }
            else
            {
                Debug.LogError($"Failed to load sprite atlas at path: {atlasPath}");
                return null;
            }
        }

        // 从图集中获取精灵
        SpriteAtlas spriteAtlas = atlasDictionary[atlasPath];
        return spriteAtlas.GetSprite(spriteName);
    }
    public static Sprite GetImageReference(string uiName, string componentName)
    {
        string jsonPath = "Assets/Resources/ImageReferences.json"; // 替换为你保存的 JSON 文件路径
        TextAsset jsonContent = Resources.Load<TextAsset>("ImageReferences");

        if (imageInfoList == null)
        {
            imageInfoList = JsonConvert.DeserializeObject<List<UIImageInfo>>(jsonContent.text);
                //JsonUtility.FromJson<List<UIImageInfo>>(jsonContent.text);
        }
        
        UIImageInfo targetInfo = imageInfoList.Find(info => info.UIName == uiName && info.ComponentName == componentName);

        if (targetInfo != null)
        {
            string spritePath = targetInfo.SpritePath;
            string[] strings = spritePath.Split("/");
            Sprite sprite = LoadSprite(strings[strings.Length-2], Path.GetFileNameWithoutExtension(strings[strings.Length - 1]));
            return sprite;
        }
        else
        {
     //       Debug.Log("Image reference not found for UI: " + uiName + ", Component: " + componentName);
            return null;
        }
    }
}

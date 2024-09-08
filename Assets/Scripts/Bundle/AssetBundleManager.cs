using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Unity.VisualScripting;
using System.Collections;
using UnityEditor;

public class AssetBundleManager : MonoBehaviour
{
#if UNITY_ANDROID
    static string m_CurPlatformName = "Android";
#elif UNITY_IOS
    static string m_CurPlatformName = "IOS";
#else
    static string m_CurPlatformName = "Windows";
#endif
    private static AssetBundleManager instance;
    private static AssetBundleManifest manifest;
    private static string assetBundleDirectory;

    // �Ѽ��ص� AssetBundle �ֵ�
    private static Dictionary<string, AssetBundle> loadedAssetBundles = new Dictionary<string, AssetBundle>();
    // AssetBundle ���ü����ֵ�
    private static Dictionary<string, int> assetBundleRefCount = new Dictionary<string, int>();
    void Start()
    {
       


    }
    public static AssetBundleManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = new GameObject("AssetBundleManager");
                instance = go.AddComponent<AssetBundleManager>();
            }
            return instance;
        }
    }

    // ��ʼ�� AssetBundleManager
    public void Initialize()
    {
        string manifestPath = Application.streamingAssetsPath + "/"+ m_CurPlatformName + "/AssetBundles/AssetBundles";
        string assetBundleFolderPath = Application.streamingAssetsPath + "/"+ m_CurPlatformName + "/AssetBundles/assetbundle/";
        manifest = LoadAssetBundleManifest(manifestPath);
        assetBundleDirectory = assetBundleFolderPath;

    }

    // ����ָ�� AssetBundle �е���Դ
    public T LoadAsset<T>(string assetBundleName, string assetName) where T : UnityEngine.Object
    {
        //if (loadedAssetBundles.ContainsKey(assetBundleName))
        //{
        //    AssetBundle assetBundl1e1 = loadedAssetBundles[assetBundleName];

        //    if (assetBundl1e1 != null)
        //    {
        //        T request = assetBundl1e1.LoadAsset<T>(assetName);
        //        //wait request
        //        return request;
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}
        //#if UNITY_EDITOR
        //        //pathΪ����ļ��е����·��(�����·��)(Editor����)
        //        string path = "Assets/AssetBundle/UI/UILogon.prefab";
        //            //"Assets/AssetBundle/" + dependencePath;
        //        Debug.Log(path);
        //        T obj = (T)AssetDatabase.LoadAssetAtPath(path, typeof(T));
        //        return obj;


        //#else
        assetBundleName = assetBundleName.ToLower();
        assetName = assetName.ToLower();
        List<AssetBundle> depenceAssetBundles = new List<AssetBundle>(); //������ż��س�����������Դ��AssetBundle
        string[] dependences = manifest.GetAllDependencies("assetbundle/"+ assetBundleName + ".assetbundle");
        Debug.Log("�����ļ�������" + dependences.Length);
        int length = dependences.Length;
        int finishedCount = 0;
        if (length == 0)
        {

        }
        else
        {
            //����������������������Դ
            for (int i = 0; i < length; i++)
            {
                string dependenceAssetName = dependences[i];
                dependenceAssetName = Application.streamingAssetsPath + "/"+ m_CurPlatformName + "/AssetBundles/" + dependenceAssetName; //eg:Windows/altas/heroiconatlas.unity3d
                LoadAssetBundle(dependenceAssetName);                                                                     //���أ��ӵ�assetpool

            }
        }
        string assetBundleFolderPath = Application.streamingAssetsPath + "/"+ m_CurPlatformName + "/AssetBundles/assetbundle/" ;
        assetBundleName = assetBundleFolderPath + assetBundleName;
        AssetBundle assetBundle = LoadAssetBundle(assetBundleName + ".assetbundle");

        if (assetBundle != null)
        {
            T request = assetBundle.LoadAsset<T>(assetName);
            //wait request
            return request;
        }
        else
        {
            return null;
        }
//#endif
    }

    // ж�� AssetBundle
    public void UnloadAssetBundle(string assetBundleName)
    {
        if (loadedAssetBundles.ContainsKey(assetBundleName))
        {
            AssetBundle assetBundle = loadedAssetBundles[assetBundleName];
            loadedAssetBundles.Remove(assetBundleName);

            // ���ü�����һ
            if (assetBundleRefCount.ContainsKey(assetBundleName))
            {
                assetBundleRefCount[assetBundleName]--;
                if (assetBundleRefCount[assetBundleName] <= 0)
                {
                    assetBundleRefCount.Remove(assetBundleName);

                    // �첽ж�� AssetBundle
                    assetBundle.Unload(false);
                }
            }
        }

    }

    // ͬ������ AssetBundle
    private AssetBundle LoadAssetBundle(string assetBundleName)
    {
  
        Debug.Log(assetBundleName);
        if (loadedAssetBundles.ContainsKey(assetBundleName))
        {
            // ���ü�����һ
            if (!assetBundleRefCount.ContainsKey(assetBundleName))
            {
                assetBundleRefCount.Add(assetBundleName, 1);
            }
            else
            {
                assetBundleRefCount[assetBundleName]++;
            }
            return loadedAssetBundles[assetBundleName];
        }
        string path;
        //if (assetBundleDirectory != null)
        //{
        //    path = Path.Combine(assetBundleDirectory, assetBundleName);


        //}
        //else
        //{
          
        //}
        path = assetBundleName;
        AssetBundle request = null;
        try
        {
            request = AssetBundle.LoadFromFile(path);
        }
        catch
        {

        }
      

        //await request;
        AssetBundle assetBundle = request;
        if (assetBundle != null)
        {
            loadedAssetBundles.Add(assetBundleName, assetBundle);
            assetBundleRefCount.Add(assetBundleName, 1);
        }
        else
        {
           // Debug.LogError("Failed to load AssetBundle!");
        }

        return assetBundle;
    }


   // ���� AssetBundleManifest
    private AssetBundleManifest LoadAssetBundleManifest(string manifestPath)
    {
        AssetBundle assetBundle = LoadAssetBundle(manifestPath);
        manifest = assetBundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        return manifest;
    }
    public static Texture2D getTextureByNmae(string textureName)
    {

        string manifestPath = Application.streamingAssetsPath + "/Windows/AssetBundles/AssetBundles";
        string assetBundleFolderPath = Application.streamingAssetsPath + "/Windows/AssetBundles/assetbundle/";


        //�����ļ���ȡ��
        FileStream fileStream = new FileStream(Application.streamingAssetsPath + "/" + manifestPath + textureName + ".jpg", FileMode.Open, FileAccess.Read);
        fileStream.Seek(0, SeekOrigin.Begin);
        //�����ļ����Ȼ�����
        byte[] bytes = new byte[fileStream.Length];
        //��ȡ�ļ�
        fileStream.Read(bytes, 0, (int)fileStream.Length);
        //�ͷ��ļ���ȡ��
        fileStream.Close();
        fileStream.Dispose();
        fileStream = null;
        //����Texture
        int width = 1920;
        int height = 1080;
        Texture2D tex = new Texture2D(width, height);
        tex.LoadImage(bytes);
        return tex;
    }

}
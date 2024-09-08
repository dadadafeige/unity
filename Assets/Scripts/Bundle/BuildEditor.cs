#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class BuildEditor : EditorWindow
{
#if UNITY_ANDROID
    static string m_CurPlatformName = "Android";
#elif UNITY_IOS
    static string m_CurPlatformName = "IOS";
#else
    static string m_CurPlatformName = "Windows";
#endif
    /// <summary>
    /// 资源打包路径
    /// </summary>
    private static string m_OutPath = Application.dataPath + "/AssetBundle";
    /// <summary>
    /// 当前选择的平台
    /// </summary>
    private static BuildTarget m_BuildTarget = EditorUserBuildSettings.activeBuildTarget;
    /// <summary>
    /// 资源输出路径
    /// </summary>
    private static string m_BundlePutPath = Application.streamingAssetsPath + "/" + m_CurPlatformName + "/AssetBundles";
    /// <summary>
    /// 打包列表
    /// </summary>
    private static List<string> finalFiles = new List<string>();
    /// <summary>
    /// 打包完成之后生成的Manifest文件
    /// </summary>
    private static AssetBundleManifest m_Manifest;
    /// <summary>
    /// 打包
    /// </summary>
    private static string[] buildBatchPaths = {
    "Assets\\AssetBundle\\Bones\\"

    };
    [MenuItem("Build/Build ab")]
    [System.Obsolete]
    private static void Build()
    {
        GameUtility.SafeClearDir(Application.streamingAssetsPath);
    //    GameUtility.SafeClearDir(m_BundlePutPath);
        //1.清除AssetBundleName
        ClearAssetBundleName();
        //2.设置AssetBundleName
        Pack(m_OutPath);
        //3.打包
       // if (Directory.Exists(m_BundlePutPath)) Directory.Delete(m_BundlePutPath, true);
        string tempFilePath = m_BundlePutPath;
        Directory.CreateDirectory(m_BundlePutPath);
        m_Manifest = BuildPipeline.BuildAssetBundles(m_BundlePutPath, BuildAssetBundleOptions.ChunkBasedCompression, m_BuildTarget);
        if (m_Manifest != null)
        {
            DeleteManifestFile(m_BundlePutPath);
            CreateFileList();
            Util.BuildSuccessOrFail("Build AB", "Build Succeed", "OK");
        }
        else
        {
            Util.BuildSuccessOrFail("Build AB", "Build Fail", "OK");
        }
        EditorUtility.ClearProgressBar();
        AssetDatabase.Refresh();
        Debug.Log("Build Succeed !!!");
    }
    /// <summary>
    /// 清除AssetBundle
    /// </summary>
    public static void ClearAssetBundleName()
    {
        string[] strs = AssetDatabase.GetAllAssetBundleNames();
        foreach (var bundleName in strs)
        {
            AssetDatabase.RemoveAssetBundleName(bundleName, true);
        }
        AssetDatabase.Refresh();
    }
    /// <summary>
    /// 检查文件
    /// </summary>
    /// <param name="path"></param>
    private static void Pack(string path)
    {
        DirectoryInfo infos = new DirectoryInfo(path);
        FileSystemInfo[] files = infos.GetFileSystemInfos();
        for (int i = 0; i < files.Length; i++)
        {
            if (files[i] is DirectoryInfo)
            {
                Pack(files[i].FullName);
            }
            else
            {
                if (!files[i].FullName.EndsWith(".meta"))
                {
                    SetAssetBundleName(files[i].FullName);
                }
            }
        }
    }

    /// <summary>
    /// 删除掉当前路径下所有.Manifest文件
    /// </summary>
    /// <param name="path"></param>
    private static void DeleteManifestFile(string path)
    {
        DirectoryInfo infos = new DirectoryInfo(path);
        FileSystemInfo[] files = infos.GetFileSystemInfos();
        for (int i = 0; i < files.Length; i++)
        {
            if (files[i] is DirectoryInfo)
            {
                DeleteManifestFile(files[i].FullName);
            }
            else
            {
                if (files[i].FullName.EndsWith(".manifest"))
                {
                    File.Delete(files[i].FullName);
                }
            }
        }
    }
    private static bool IsNeedBundleOne(string _assetPath1,out string specificFolderPath)
    {
        string folderPath = Path.GetDirectoryName(_assetPath1);
        // 获取文件夹的名字
        Debug.Log(_assetPath1);
        string fileNmae = Path.GetFileName(folderPath);
        string temp = "";
        bool isInSpecificFolder = false;
        for (int i = 0; i < buildBatchPaths.Length; i++)
        {
            temp = buildBatchPaths[i];
            isInSpecificFolder = _assetPath1.StartsWith(temp);
            if (isInSpecificFolder)
            {
                break;
            };
        }
        specificFolderPath = temp + fileNmae;
       // specificFolderPath = "Assets\\AssetBundle\\Bones\\" + fileNmae;
        return isInSpecificFolder;



    }
    /// <summary>
    /// 设置AssetBundleName
    /// </summary>
    /// <param name="source"></param>
    private static void SetAssetBundleName(string source)
    {
        string _source = source.Replace(@"\\", "/");
        //截取完整路径到Assets/位置获得Assets路径
        string _assetPath1 = "Assets" + _source.Substring(Application.dataPath.Length);
        //获取Assets/之后的路径 以方便做AssetBunndleName使用
        string _assetPath2 = _source.Substring(Application.dataPath.Length + 1);
        //获取Assets路径下的文件
        AssetImporter assetImporter = AssetImporter.GetAtPath(_assetPath1);
        //获取AssetBundleName
        string bundleName = _assetPath2;

        // 判断是否在特定文件夹下
        // 获取文件所在的文件夹路径
        string folderPath = Path.GetDirectoryName(_assetPath1);
        // 获取文件夹的名字
        string fileNmae = Path.GetFileName(folderPath);
        string specificFolderPath;
        bool isInSpecificFolder = IsNeedBundleOne(_assetPath1, out specificFolderPath);
        if (isInSpecificFolder)
        {
            //获取文件的扩展名并将其替换成.assetbundle
            // 获取文件夹的名字
            bundleName = "AssetBundle\\Bones\\" + fileNmae + ".assetbundle";
            bundleName = bundleName.ToLower();
            //设置Bundle名
            assetImporter.assetBundleName = bundleName;
            //获取每个文件的指定路径并添加到列表里以方便写file文件使用
           // string newFilePath = m_BundlePutPath + PathUtils.GetRelativePath("Assets/AssetBundle/Bones/" + fileNmae, Application.dataPath, "").ToLower();
            string absolutePath = Path.Combine(Application.dataPath, m_BundlePutPath + specificFolderPath.Substring("Assets".Length).ToLower());
            finalFiles.Add(absolutePath + ".assetbundle");
        }
        else
        {
            bundleName = bundleName.Replace(Path.GetExtension(bundleName), ".assetbundle");
            //设置Bundle名
            assetImporter.assetBundleName = bundleName;
            //获取每个文件的指定路径并添加到列表里以方便写file文件使用
            string newFilePath = m_BundlePutPath + PathUtils.GetRelativePath(source, Application.dataPath, "").ToLower();
            string[] strs = newFilePath.Split('.');
            finalFiles.Add(strs[0] + ".assetbundle");
        }
     

    }

    /// <summary>
    /// 生成 file 文件
    /// </summary>
    static void CreateFileList()
    {
        //files文件 目标路径
        string targetFilePath = m_BundlePutPath + "/files.txt";
        //检查是否存在该文件 存在则删除
        if (File.Exists(targetFilePath))
            File.Delete(targetFilePath);
        //统计大小 单位B
        long totalFileSize = 0;
        FileStream fs = new FileStream(targetFilePath, FileMode.CreateNew);
        StreamWriter sw = new StreamWriter(fs);
        int count = 0;
        string file;
        finalFiles.Add(m_BundlePutPath + "/AssetBundles");
        File.Delete(m_BundlePutPath + "/AssetBundles.manifest");
        m_BundlePutPath = m_BundlePutPath.Replace('\\', '/');
        foreach (var files in finalFiles)
        {
            file = PathUtils.NormalizePath(files);
            count++;
            Util.UpdateProgress(count, finalFiles.Count + 1, "Createing files.txt");
            //文件Hash
            string hash = "";
            //取到文件路径
            string _path = file.Replace(m_BundlePutPath + "/", string.Empty);
            FileInfo fi = new FileInfo(file);
            if (Path.GetExtension(file) == ".assetbundle")
            {
                //取到文件在Manifest中引用名字
                string abname = file.Replace(m_BundlePutPath + "/", "");
                //通过引用名字去Manifest文件中获取到该文件的Hash值
                hash = m_Manifest.GetAssetBundleHash(abname).ToString();
            }
            else
            {
                hash = Util.md5file(file);
            }
            totalFileSize += fi.Length;
            //将文件信息按行写入到files文件中 路径|Hash|文件大小(单位B)
            sw.WriteLine(_path + "|" + hash + "|" + fi.Length);
        }
        //最后写入总大小(单位B)
        sw.WriteLine("" + totalFileSize);
        sw.Close();
        fs.Close();
    }
}
#endif
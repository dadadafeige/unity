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
    /// ��Դ���·��
    /// </summary>
    private static string m_OutPath = Application.dataPath + "/AssetBundle";
    /// <summary>
    /// ��ǰѡ���ƽ̨
    /// </summary>
    private static BuildTarget m_BuildTarget = EditorUserBuildSettings.activeBuildTarget;
    /// <summary>
    /// ��Դ���·��
    /// </summary>
    private static string m_BundlePutPath = Application.streamingAssetsPath + "/" + m_CurPlatformName + "/AssetBundles";
    /// <summary>
    /// ����б�
    /// </summary>
    private static List<string> finalFiles = new List<string>();
    /// <summary>
    /// ������֮�����ɵ�Manifest�ļ�
    /// </summary>
    private static AssetBundleManifest m_Manifest;
    /// <summary>
    /// ���
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
        //1.���AssetBundleName
        ClearAssetBundleName();
        //2.����AssetBundleName
        Pack(m_OutPath);
        //3.���
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
    /// ���AssetBundle
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
    /// ����ļ�
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
    /// ɾ������ǰ·��������.Manifest�ļ�
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
        // ��ȡ�ļ��е�����
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
    /// ����AssetBundleName
    /// </summary>
    /// <param name="source"></param>
    private static void SetAssetBundleName(string source)
    {
        string _source = source.Replace(@"\\", "/");
        //��ȡ����·����Assets/λ�û��Assets·��
        string _assetPath1 = "Assets" + _source.Substring(Application.dataPath.Length);
        //��ȡAssets/֮���·�� �Է�����AssetBunndleNameʹ��
        string _assetPath2 = _source.Substring(Application.dataPath.Length + 1);
        //��ȡAssets·���µ��ļ�
        AssetImporter assetImporter = AssetImporter.GetAtPath(_assetPath1);
        //��ȡAssetBundleName
        string bundleName = _assetPath2;

        // �ж��Ƿ����ض��ļ�����
        // ��ȡ�ļ����ڵ��ļ���·��
        string folderPath = Path.GetDirectoryName(_assetPath1);
        // ��ȡ�ļ��е�����
        string fileNmae = Path.GetFileName(folderPath);
        string specificFolderPath;
        bool isInSpecificFolder = IsNeedBundleOne(_assetPath1, out specificFolderPath);
        if (isInSpecificFolder)
        {
            //��ȡ�ļ�����չ���������滻��.assetbundle
            // ��ȡ�ļ��е�����
            bundleName = "AssetBundle\\Bones\\" + fileNmae + ".assetbundle";
            bundleName = bundleName.ToLower();
            //����Bundle��
            assetImporter.assetBundleName = bundleName;
            //��ȡÿ���ļ���ָ��·������ӵ��б����Է���дfile�ļ�ʹ��
           // string newFilePath = m_BundlePutPath + PathUtils.GetRelativePath("Assets/AssetBundle/Bones/" + fileNmae, Application.dataPath, "").ToLower();
            string absolutePath = Path.Combine(Application.dataPath, m_BundlePutPath + specificFolderPath.Substring("Assets".Length).ToLower());
            finalFiles.Add(absolutePath + ".assetbundle");
        }
        else
        {
            bundleName = bundleName.Replace(Path.GetExtension(bundleName), ".assetbundle");
            //����Bundle��
            assetImporter.assetBundleName = bundleName;
            //��ȡÿ���ļ���ָ��·������ӵ��б����Է���дfile�ļ�ʹ��
            string newFilePath = m_BundlePutPath + PathUtils.GetRelativePath(source, Application.dataPath, "").ToLower();
            string[] strs = newFilePath.Split('.');
            finalFiles.Add(strs[0] + ".assetbundle");
        }
     

    }

    /// <summary>
    /// ���� file �ļ�
    /// </summary>
    static void CreateFileList()
    {
        //files�ļ� Ŀ��·��
        string targetFilePath = m_BundlePutPath + "/files.txt";
        //����Ƿ���ڸ��ļ� ������ɾ��
        if (File.Exists(targetFilePath))
            File.Delete(targetFilePath);
        //ͳ�ƴ�С ��λB
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
            //�ļ�Hash
            string hash = "";
            //ȡ���ļ�·��
            string _path = file.Replace(m_BundlePutPath + "/", string.Empty);
            FileInfo fi = new FileInfo(file);
            if (Path.GetExtension(file) == ".assetbundle")
            {
                //ȡ���ļ���Manifest����������
                string abname = file.Replace(m_BundlePutPath + "/", "");
                //ͨ����������ȥManifest�ļ��л�ȡ�����ļ���Hashֵ
                hash = m_Manifest.GetAssetBundleHash(abname).ToString();
            }
            else
            {
                hash = Util.md5file(file);
            }
            totalFileSize += fi.Length;
            //���ļ���Ϣ����д�뵽files�ļ��� ·��|Hash|�ļ���С(��λB)
            sw.WriteLine(_path + "|" + hash + "|" + fi.Length);
        }
        //���д���ܴ�С(��λB)
        sw.WriteLine("" + totalFileSize);
        sw.Close();
        fs.Close();
    }
}
#endif
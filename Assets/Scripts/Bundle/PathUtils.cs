using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// Path utils.·��������
/// </summary>
public class PathUtils
{

    /// <summary>
    /// ����һ������·�� ��������Դ��assetbundle name
    /// </summary>
    /// <param name="path"></param>
    /// <param name="root">��Դ�ļ��еĸ�Ŀ¼</param>
    /// <returns></returns>
    public static string GetAssetBundleNameWithPath(string path, string root)
    {
        string str = NormalizePath(path);
        str = ReplaceFirst(str, root + "/", "");
        return str;
    }

    /// <summary>
    /// ��ȡ�ļ��е������ļ����������ļ��� ������.meta�ļ�
    /// </summary>
    /// <returns>The files.</returns>
    /// <param name="path">Path.</param>
    public static FileInfo[] GetFiles(string path)
    {
        DirectoryInfo folder = new DirectoryInfo(path);

        DirectoryInfo[] subFolders = folder.GetDirectories();
        List<FileInfo> filesList = new List<FileInfo>();

        foreach (DirectoryInfo subFolder in subFolders)
        {
            filesList.AddRange(GetFiles(subFolder.FullName));
        }

        FileInfo[] files = folder.GetFiles();
        foreach (FileInfo file in files)
        {
            if (file.Extension != ".meta")
            {
                filesList.Add(file);
            }

        }
        return filesList.ToArray();
    }
    /// <summary>
    /// ��ȡ�ļ��е������ļ�·�����������ļ��� ������.meta�ļ�
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static string[] GetFilesPath(string path)
    {
        DirectoryInfo folder = new DirectoryInfo(path);
        DirectoryInfo[] subFolders = folder.GetDirectories();
        List<string> filesList = new List<string>();

        foreach (DirectoryInfo subFolder in subFolders)
        {
            filesList.AddRange(GetFilesPath(subFolder.FullName));
        }

        FileInfo[] files = folder.GetFiles();
        foreach (FileInfo file in files)
        {
            if (file.Extension != ".meta")
            {
                filesList.Add(NormalizePath(file.FullName));
            }

        }
        return filesList.ToArray();
    }

    /// <summary>
    /// �����ļ�Ŀ¼ǰ���ļ��У���֤�����ļ���ʱ�򲻻�����ļ��в����ڵ����
    /// </summary>
    /// <param name="path"></param>
    public static void CreateFolderByFilePath(string path)
    {
        FileInfo fi = new FileInfo(path);
        DirectoryInfo dir = fi.Directory;
        if (!dir.Exists)
        {
            dir.Create();
        }
    }

    /// <summary>
    /// �����ļ���
    /// </summary>
    /// <param name="path"></param>
    public static void CreateFolder(string path)
    {
        DirectoryInfo dir = new DirectoryInfo(path);
        if (!dir.Exists)
        {
            dir.Create();
        }
    }

    /// <summary>
    /// Deletes the file.ɾ���ļ�
    /// </summary>
    /// <param name="path">Path.</param>
    public static void DeleteFile(string path)
    {
        if (File.Exists(path)) File.Delete(path);
    }

    /// <summary>
    /// �淶��·������ ����·���е�����б��
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static string NormalizePath(string path)
    {
        return path.Replace(@"\", "/");
    }

    /// <summary>
    /// //������·��ת�ɹ����ռ��ڵ����·��
    /// </summary>
    /// <param name="fullPath"></param>
    /// <returns></returns>
    public static string GetRelativePath(string fullPath, string root, string insert = "Assets")
    {
        string path = NormalizePath(fullPath);
        path = ReplaceFirst(path, root, insert);
        return path;
    }
    /// <summary>
    /// �����·��ת�ɾ���·��
    /// </summary>
    /// <param name="relativePath"></param>
    /// <returns></returns>
    public static string GetAbsolutelyPath(string relativePath, string root)
    {
        string path = NormalizePath(relativePath);
        path = ReplaceFirst(root, "Assets", "") + path;
        return path;
    }

    /// <summary>
    /// �滻����һ��������ָ���ַ���
    /// </summary>
    /// <param name="str"></param>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    /// <returns></returns>
    public static string ReplaceFirst(string str, string oldValue, string newValue)
    {
        int i = str.IndexOf(oldValue);
        str = str.Remove(i, oldValue.Length);
        str = str.Insert(i, newValue);
        return str;
    }

    /// <summary>
    /// Copies the folder to.��һ��Ŀ¼�������ݸ��Ƶ���һĿ¼
    /// </summary>
    /// <param name="directorySource">Directory source.</param>
    /// <param name="directoryTarget">Directory target.</param>
    public static void CopyFolderTo(string directorySource, string directoryTarget)
    {
        //����Ƿ����Ŀ��Ŀ¼
        if (!Directory.Exists(directoryTarget))
        {
            Directory.CreateDirectory(directoryTarget);
        }
        //���������ļ�
        DirectoryInfo directoryInfo = new DirectoryInfo(directorySource);
        FileInfo[] files = directoryInfo.GetFiles();
        //���������ļ�
        foreach (FileInfo file in files)
        {
            file.CopyTo(Path.Combine(directoryTarget, file.Name));
        }
        //�����Ŀ¼
        DirectoryInfo[] directoryInfoArray = directoryInfo.GetDirectories();
        foreach (DirectoryInfo dir in directoryInfoArray)
        {
            CopyFolderTo(Path.Combine(directorySource, dir.Name), Path.Combine(directoryTarget, dir.Name));
        }
    }

    /// <summary>
    /// Creates the stream dir.����·��
    /// </summary>
    /// <returns>The stream dir.</returns>
    /// <param name="dir">Dir.</param>
    public static string CreateStreamDir(string dir)
    {
        string path = Application.streamingAssetsPath + "/" + dir;

        if (!File.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        return path;
    }


    /// <summary>
    /// Gets all dirs.��ȡ�ļ�·��
    /// </summary>
    /// <param name="dir">Dir.</param>
    /// <param name="list">List.</param>
    public static void GetAllDirs(string dir, List<string> list)
    {
        string[] dirs = Directory.GetDirectories(dir);
        list.AddRange(dirs);

        for (int i = 0; i < dirs.Length; i++)
        {
            GetAllDirs(dirs[i], list);
        }
    }
}
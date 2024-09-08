using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// Path utils.路径工具类
/// </summary>
public class PathUtils
{

    /// <summary>
    /// 根据一个绝对路径 获得这个资源的assetbundle name
    /// </summary>
    /// <param name="path"></param>
    /// <param name="root">资源文件夹的根目录</param>
    /// <returns></returns>
    public static string GetAssetBundleNameWithPath(string path, string root)
    {
        string str = NormalizePath(path);
        str = ReplaceFirst(str, root + "/", "");
        return str;
    }

    /// <summary>
    /// 获取文件夹的所有文件，包括子文件夹 不包含.meta文件
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
    /// 获取文件夹的所有文件路径，包括子文件夹 不包含.meta文件
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
    /// 创建文件目录前的文件夹，保证创建文件的时候不会出现文件夹不存在的情况
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
    /// 创建文件夹
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
    /// Deletes the file.删除文件
    /// </summary>
    /// <param name="path">Path.</param>
    public static void DeleteFile(string path)
    {
        if (File.Exists(path)) File.Delete(path);
    }

    /// <summary>
    /// 规范化路径名称 修正路径中的正反斜杠
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static string NormalizePath(string path)
    {
        return path.Replace(@"\", "/");
    }

    /// <summary>
    /// //将绝对路径转成工作空间内的相对路径
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
    /// 将相对路径转成绝对路径
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
    /// 替换掉第一个遇到的指定字符串
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
    /// Copies the folder to.从一个目录将其内容复制到另一目录
    /// </summary>
    /// <param name="directorySource">Directory source.</param>
    /// <param name="directoryTarget">Directory target.</param>
    public static void CopyFolderTo(string directorySource, string directoryTarget)
    {
        //检查是否存在目的目录
        if (!Directory.Exists(directoryTarget))
        {
            Directory.CreateDirectory(directoryTarget);
        }
        //先来复制文件
        DirectoryInfo directoryInfo = new DirectoryInfo(directorySource);
        FileInfo[] files = directoryInfo.GetFiles();
        //复制所有文件
        foreach (FileInfo file in files)
        {
            file.CopyTo(Path.Combine(directoryTarget, file.Name));
        }
        //最后复制目录
        DirectoryInfo[] directoryInfoArray = directoryInfo.GetDirectories();
        foreach (DirectoryInfo dir in directoryInfoArray)
        {
            CopyFolderTo(Path.Combine(directorySource, dir.Name), Path.Combine(directoryTarget, dir.Name));
        }
    }

    /// <summary>
    /// Creates the stream dir.创建路径
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
    /// Gets all dirs.获取文件路径
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
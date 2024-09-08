using UnityEditor;

public class CustomBuildSettings
{
    [InitializeOnLoadMethod]
    static void ExcludeFoldersFromBuild()
    {
        // 在这里添加需要排除的文件夹路径
        //string[] excludedFolders = new string[]
        //{
        //    "Assets/Images",
        //};

        // 排除文件夹
        //foreach (var folder in excludedFolders)
        //{
        //    AssetDatabase.ExportPackage(folder, "TempPackage.unitypackage", ExportPackageOptions.Recurse);
        //}
    }
}

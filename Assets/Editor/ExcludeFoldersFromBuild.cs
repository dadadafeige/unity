using UnityEditor;

public class CustomBuildSettings
{
    [InitializeOnLoadMethod]
    static void ExcludeFoldersFromBuild()
    {
        // �����������Ҫ�ų����ļ���·��
        //string[] excludedFolders = new string[]
        //{
        //    "Assets/Images",
        //};

        // �ų��ļ���
        //foreach (var folder in excludedFolders)
        //{
        //    AssetDatabase.ExportPackage(folder, "TempPackage.unitypackage", ExportPackageOptions.Recurse);
        //}
    }
}

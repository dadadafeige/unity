using System;
using System.IO;
using System.Text;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Util
{
    /// <summary>
    /// 计算文件的MD5值
    /// </summary>
    public static string md5file(string file)
    {
        try
        {
            FileStream fs = new FileStream(file, FileMode.Open);
            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] retVal = md5.ComputeHash(fs);
            fs.Close();

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < retVal.Length; i++)
            {
                sb.Append(retVal[i].ToString("x2"));
            }
            return sb.ToString();
        }
        catch (Exception ex)
        {
            throw new Exception("md5file() fail, error:" + ex.Message);
        }
    }

#if UNITY_EDITOR
    public static void BuildSuccessOrFail(string tittle, string message, string okmsg)
    {
        EditorUtility.DisplayDialog(tittle, message, okmsg);
    }

    /// <summary>
    /// Updates the progress.更新进度跳
    /// </summary>
    public static void UpdateProgress(int progress, int progressMax, string desc)
    {
        string title = "Processing...[" + progress + " - " + progressMax + "]";
        float value = (float)progress / (float)progressMax;
        EditorUtility.DisplayProgressBar(title, desc, value);
    }
#endif

}
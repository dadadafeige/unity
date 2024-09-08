using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using System.Text;
using System.Collections;
using System.Collections.Generic;

public static class ExcelTools 
{
    /// <summary>
    /// 文件目录
    /// </summary>
    public static string Excel_Path   = "E:\\RPG/Configs";
    /// <summary>
    /// 文件夹输出目录
    /// </summary>
    public static string Excel_Out_Path = "Assets/AssetBundle/Config";
    /// <summary>
    /// 文件后缀
    /// </summary>
    public static string[] Excel_Suffix = new string[]{".xlsx"};
    /// <summary>
    /// 文件列表
    /// </summary>
    private static List<string> excelList;

    // Encoding encoding=Encoding.GetEncoding("utf-8");

    /// <summary>
	/// 加载Excel
	/// </summary>
    [MenuItem("Tools/ExcelTools", false, 4)]   
	private static void LoadExcel()
	{
		if(excelList==null) excelList=new List<string>();
		excelList.Clear();
        // 清理旧的文件
        GameUtility.SafeClearDir(Excel_Out_Path);
        string[] allfils = GameUtility.GetSpecifyFilesInFolder(Excel_Path,Excel_Suffix);
        for (int i = 0; i < allfils.Length; i++)
        {
           excelList.Add(allfils[i]);
        }
        Convert();
	}

    /// <summary>
	/// 转换Excel文件
	/// </summary>
	private static void Convert()
	{
		foreach(string assetsPath in excelList)
		{
			//获取Excel文件的绝对路径
			string excelPath = assetsPath;
			//构造Excel工具类
			ExcelUtility excel=new ExcelUtility(excelPath);
            excel.ConvertToJson(Excel_Out_Path,Encoding.GetEncoding("utf-8"));
			//刷新本地资源
			AssetDatabase.Refresh();
		}
	}


}

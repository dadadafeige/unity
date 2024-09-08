using UnityEngine;
using System.Collections.Generic;
using Excel;
using System.Data;
using System.IO;
using Newtonsoft.Json;
using System.Text;
using System;


public class ExcelUtility
{

	/// <summary>
	/// 表格数据集合
	/// </summary>
	private DataSet mResultSet;

	/// <summary>
	/// 构造函数
	/// </summary>
	/// <param name="excelFile">Excel file.</param>
	public ExcelUtility (string excelFile)
	{
		FileStream mStream = File.Open (excelFile, FileMode.Open, FileAccess.Read);
		IExcelDataReader mExcelReader = ExcelReaderFactory.CreateOpenXmlReader (mStream);
		mResultSet = mExcelReader.AsDataSet ();
	}
			
	/// <summary>
	/// 转换为Json
	/// string  int float  double  bool
	/// </summary>
	/// <param name="JsonPath">Json文件路径</param>
	/// <param name="Header">表头行数</param>
	public void ConvertToJson (string JsonPath, Encoding encoding)
	{
		//判断Excel文件中是否存在数据表
		if ( mResultSet==null || mResultSet.Tables.Count < 1)
			return;


		for (int x = 0; x < mResultSet.Tables.Count; x++)
		{
			//默认读取第一个数据表
			DataTable mSheet = mResultSet.Tables[x];

			string outname = mSheet.TableName;
			if(outname.IndexOf('#') >= 0 && outname.LastIndexOf('#') != outname.IndexOf('#'))
			{
				outname = outname.Substring(outname.IndexOf('#')+1,outname.LastIndexOf('#')-outname.IndexOf('#')-1);
			}
			else
			{
				Debug.Log("无法世界导出名 "+outname+"  请确定#书写正确!");
				continue;
			}
			Debug.Log(outname);
			//判断数据表内是否存在数据
			if (mSheet.Rows.Count < 1)
				continue;

			//读取数据表行数和列数
			int rowCount = mSheet.Rows.Count;
			int colCount = mSheet.Columns.Count;

			//准备一个列表存储整个表的数据
			List<Dictionary<string, object>> table = new List<Dictionary<string, object>> ();

			//读取数据
			for (int i = 3; i < rowCount; i++) {
				//准备一个字典存储每一行的数据
				Dictionary<string, object> row = new Dictionary<string, object> ();
				for (int j = 0; j < colCount; j++) {
					
                    //读取第1行数据作为表头字段
                    string field = mSheet.Rows [1] [j].ToString();
					field = field.Trim();

					string typestring = mSheet.Rows [2] [j].ToString();
					typestring = typestring.ToLower().Trim();
                

                    string valuestr = mSheet.Rows[i][j].ToString();
					valuestr = valuestr.Trim();
					//Key-Value对应 按类型存放
					switch(typestring)
					{
						case "int":
							if(valuestr != "")
							{
								row [field] = Convert.ToInt32(valuestr);
							}
							else
							{
								row [field] = -1;
							}
							break;
						case "float":
							if(valuestr != "")
							{
								row [field] = float.Parse(valuestr);
							}
							else
							{
								row [field] = -1;
							}
							break;
						case "double":
							if(valuestr != "")
							{
								row [field] = Convert.ToDouble(valuestr);
							}
							else
							{
								row [field] = -1;
							}
							
							break;
						case "bool":
							if(valuestr == "0" || valuestr == "fasle" || valuestr == "")
							{
								row [field] = false;
							}
							else
							{
								row [field] = true;
							}
							break;
						default:
							row [field] = valuestr;
							break;
					}
				}
				if (row.ContainsKey("id"))
				{
					if (Convert.ToInt32(row["id"]) > 0)
					{
                        //添加到表数据中
                        table.Add(row);
                    }

				}else
				{
                    table.Add(row);
                }
            }

			//生成Json字符串
			string json = JsonConvert.SerializeObject (table, Newtonsoft.Json.Formatting.Indented);
		
			//写入文件
			using (FileStream fileStream=new FileStream(JsonPath+"/"+outname+ ".json", FileMode.Create,FileAccess.Write)) {
				using (TextWriter textWriter = new StreamWriter(fileStream, encoding)) {
					textWriter.Write (json);
				}
			}
		}
	}


}


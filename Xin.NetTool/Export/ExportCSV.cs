using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Easy.Tool.Export
{
    public class ExportCSV
    {
        public static void ExportToCSV(DataTable dt,string fullpathName)
        {
            if (dt == null || dt.Rows.Count == 0)
            {
                throw new Exception("DataTable为空");
            }
            if (string.Equals(Path.GetExtension(fullpathName), ".csv"))
            {
                throw new Exception("文件不是csv格式");
            }
            if (!File.Exists(fullpathName))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(fullpathName));
            }
            try
            {
                using (StreamWriter sw = new StreamWriter(fullpathName, false, Encoding.UTF8))
                {
                    StringBuilder colbuilder = new StringBuilder();
                    StringBuilder valbuilder = new StringBuilder();
                    // 写入列名
                    string[] columnNames = dt.Columns.Cast<DataColumn>().Select(column => column.ColumnName).ToArray();
                    foreach (string colName in columnNames)
                    {
                        colbuilder.Append(colName);
                        colbuilder.Append(",");
                    }
                    colbuilder.Remove(colbuilder.Length - 1, 1);
                    sw.WriteLine(colbuilder.ToString());
                    // 写入数据
                    foreach (DataRow row in dt.Rows)
                    {
                        valbuilder.Clear();
                        for (int i = 0; i < dt.Columns.Count; i++)
                        {
                            valbuilder.Append(row[i].ToString());
                            valbuilder.Append(",");
                        }
                        valbuilder.Remove(valbuilder.Length - 1, 1);
                        sw.WriteLine(valbuilder.ToString());
                    }
                }

            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static DataTable ImportCSVToDataTable(string fullpathName)
        {
            if(string.Equals(Path.GetExtension(fullpathName), ".csv"))
            {
                throw new Exception("文件不是csv格式");
            }
            if (!File.Exists(fullpathName))
            {
                throw new Exception("文件不存在");
            }

            DataTable dt = new DataTable();
            try
            {
                using (StreamReader sr = new StreamReader(fullpathName, Encoding.UTF8))
                {
                    string line = sr.ReadLine();
                    string[] colNames = line.Split(',');
                    foreach (string colName in colNames)
                    {
                        dt.Columns.Add(colName);
                    }
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] vals = line.Split(',');
                        DataRow dr = dt.NewRow();
                        for (int i = 0; i < vals.Length; i++)
                        {
                            dr[i] = vals[i];
                        }
                        dt.Rows.Add(dr);
                    }
                }
                return dt;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}

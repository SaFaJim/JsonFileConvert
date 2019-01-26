using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Data;
using System.Collections;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Converters;
using System.Linq;

namespace Test
{
    /// <summary>
    /// 转换Json格式帮助类
    /// </summary>
    public static class JsonHelper
    {
        public static object ToJson(this string Json)
        {
            return JsonConvert.DeserializeObject(Json);
        }
        public static string ToJson(this object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
        public static DataTable JsonToDataTable(this string strJson)
        {
            #region
            DataTable tb = null;
            //获取数据  
            Regex rg = new Regex(@"(?<={)[^}]+(?=})");
            MatchCollection mc = rg.Matches(strJson);
            for (int i = 0; i < mc.Count; i++)
            {
                string strRow = mc[i].Value;
                string[] strRows = strRow.Split(',');
                //创建表  
                if (tb == null)
                {
                    tb = new DataTable();
                    tb.TableName = "Table";
                    foreach (string str in strRows)
                    {
                        DataColumn dc = new DataColumn();
                        string[] strCell = str.Split(':');
                        dc.DataType = typeof(String);
                        dc.ColumnName = strCell[0].ToString().Replace("\"", "").Trim();
                        tb.Columns.Add(dc);
                    }
                    tb.AcceptChanges();
                }
                //增加内容
                DataRow dr = tb.NewRow();
                for (int r = 0; r < strRows.Length; r++)
                {
                    string[] rowArray = strRows[r].Split(':');
                    if (rowArray.Length <= 1) continue;
                    object strText = rowArray[1].Trim().Replace("，", ",").Replace("：", ":").Replace("/", "").Replace("\"", "").Trim();
                    if (strText.ToString().Length >= 5)
                    {
                        if (strText.ToString().Substring(0, 5) == "Date(")//判断是否JSON日期格式
                        {
                            strText = JsonToDateTime(strText.ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                        }
                    }
                    dr[r] = strText;
                }
                tb.Rows.Add(dr);
                tb.AcceptChanges();
            }
            return tb;
            #endregion
        }
        public static List<T> JonsToList<T>(this string Json)
        {
            return JsonConvert.DeserializeObject<List<T>>(Json);
        }
        public static T JsonToEntity<T>(this string Json)
        {
            return JsonConvert.DeserializeObject<T>(Json);
        }
        /// <summary>
        /// Json 的日期格式与.Net DateTime类型的转换
        /// </summary>
        /// <param name="jsonDate">Date(1242357713797+0800)</param>
        /// <returns></returns>
        public static DateTime JsonToDateTime(string jsonDate)
        {
            string value = jsonDate.Substring(5, jsonDate.Length - 6) + "+0800";
            DateTimeKind kind = DateTimeKind.Utc;
            int index = value.IndexOf('+', 1);
            if (index == -1)
                index = value.IndexOf('-', 1);
            if (index != -1)
            {
                kind = DateTimeKind.Local;
                value = value.Substring(0, index);
            }
            long javaScriptTicks = long.Parse(value, System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture);
            long InitialJavaScriptDateTicks = (new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).Ticks;
            DateTime utcDateTime = new DateTime((javaScriptTicks * 10000) + InitialJavaScriptDateTicks, DateTimeKind.Utc);
            DateTime dateTime;
            switch (kind)
            {
                case DateTimeKind.Unspecified:
                    dateTime = DateTime.SpecifyKind(utcDateTime.ToLocalTime(), DateTimeKind.Unspecified);
                    break;
                case DateTimeKind.Local:
                    dateTime = utcDateTime.ToLocalTime();
                    break;
                default:
                    dateTime = utcDateTime;
                    break;
            }
            return dateTime;
        }

        /// <summary>
        /// 将 Object 序列化为Json String
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToJSONString(this object obj)
        {
            if (obj != null)
            {
                IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
                timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
                var str = Newtonsoft.Json.JsonConvert.SerializeObject(obj, timeFormat);
                return str;
            }
            return string.Empty;
        }
        /// <summary>
        /// 将 Json String [] 序列化为 Object List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsArr"></param>
        /// <returns></returns>
        public static IList<T> ToObjectListFromJson<T>(this string[] jsArr) where T : new()
        {
            if (jsArr != null)
            {
                if (jsArr.Length > 0)
                {
                    if (jsArr.Length == 1 && jsArr[0] == "[]") { return new List<T>(); }
                    var s = Array.ConvertAll(jsArr, u => u.JsonToObject<T>());
                    return s.ToList();
                }
                return new List<T>();
            }
            return null;
        }

        /// <summary>
        /// 将 Json String 反序列化为 Object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T JsonToObject<T>(this string json) where T : new()
        {
            if (!string.IsNullOrEmpty(json))
            {
                var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
                return obj;
            }
            return new T();
        }
    }
}

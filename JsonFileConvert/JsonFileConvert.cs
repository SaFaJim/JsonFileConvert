using Swifter.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace JsonFileConvert
{
    public class JsonFileConvert
    {

        /// <summary>
        /// 保存json
        /// </summary>
        /// 
        public static bool SaveJsonFile<T>(List<T> list, string savePath, int batchNumber = 1000)
        {
            if (list == null || list.Count == 0 || string.IsNullOrEmpty(savePath))
            {
                return false;
            }
            if (File.Exists(savePath))
            {
                File.Delete(savePath);
            }
            var count = (int)Math.Ceiling((double)list.Count / batchNumber);
            bool isFirstBanth = false;
            bool isLastBanth = false;
            for (var i = 0; i < count; i++)
            {
                isFirstBanth = false;
                isLastBanth = false;
                if (i == 0)
                {
                    isFirstBanth = true;
                }
                if (i == count - 1)
                {
                    isLastBanth = true;
                }
                //以下10行代码是为了分页
                int start = i * batchNumber;
                int end = start + batchNumber;
                if (end >= list.Count)
                {
                    end = list.Count - 1;
                }
                List<T> batchList = new List<T>();
                for (var j = start; j < end; j++)
                {
                    batchList.Add(list[j]);
                }
                //var batchList = list.Skip(i * batchNumber).Take(batchNumber).ToList(); .net 4
                SaveJsonBatch<T>(batchList, savePath, isFirstBanth, isLastBanth);
            }
            return true;
        }
        /// <summary>
        /// 批次保存
        /// </summary>
        private static void SaveJsonBatch<T>(List<T> list, string path, bool isFirstBanth, bool isLastBanth)
        {
            //实例化一个文件流—>与写入文件相关联 
            string jsonString = string.Empty;
            StringBuilder json = new StringBuilder();
            using (StreamWriter sw = new StreamWriter(path, true))
            {
                if (isFirstBanth)
                {
                    jsonString = "[";
                }
                jsonString += ToJson<T>(list, isLastBanth);
                if (isLastBanth)
                {
                    jsonString += "]";
                }
                //开始写入 
                sw.Write(jsonString);
                //清空缓冲区 
                sw.Flush();
                //关闭流 
                sw.Close();
            }
        }

        /// <summary>
        /// 转换为json String
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        private static string ToJson<T>(List<T> list, bool isLastBanth)
        {
            string jsonString = string.Empty;
            StringBuilder str = new StringBuilder();
            for (int i = 0; i < list.Count; i++)
            {
                var item = list[i];
                if (i == list.Count - 1 && isLastBanth)
                {
                    str.Append(JsonFormatter.SerializeObject(item));
                }
                else
                {
                    str.Append(JsonFormatter.SerializeObject(item) + "," + Environment.NewLine);
                }
            }
            jsonString += str.ToString();
            return jsonString;
        }

        /// <summary>
        /// 二进制读取转换
        /// </summary>
        public static List<T> ConvertJsonFile<T>(string path)
        {
            List<T> returnList = new List<T>();
            if (!File.Exists(path))
            {
                return returnList;
            }
            try
            {
                // 在当前目录创建一个文件myfile.txt，对该文件具有读写权限         
                FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                BinaryReader br = new BinaryReader(fs);
                // 把文件指针重新定位到文件的开始         
                br.BaseStream.Seek(0, SeekOrigin.Begin);
                // 打印文件文本内容
                long start = 0;
                long end = 0;
                byte[] data = null;
                long length = 0;
                string jsonString = "";
                long MbSize = 8 * 1024 * 1024;
                long fileSize = fs.Length;
                while (end < fileSize)
                {
                    jsonString = "";
                    br.BaseStream.Position = start;
                    //按照块读取
                    if (MbSize > fileSize)
                    {
                        length = fileSize;
                    }
                    else
                    {
                        length = MbSize;
                    }
                    data = new byte[length];
                    br.Read(data, 0, (int)length);
                    int index = 0;
                    //查找最后一次回车的位置
                    for (var i = data.Length - 1; i > 0; i--)
                    {
                        if (data[i] == 13)
                        {
                            index = i;
                            break;
                        }
                    }
                    if (index == 0)
                    {
                        index = data.Length - 1;
                    }
                    jsonString = System.Text.Encoding.UTF8.GetString(data, 0, index);
                    if (!jsonString.StartsWith("["))
                    {
                        jsonString = "[" + jsonString;
                    }
                    if (!jsonString.EndsWith("]"))
                    {
                        jsonString = jsonString.TrimEnd('\n').TrimEnd('\r').TrimEnd(',');
                        jsonString += "]";
                    }
                    //根据指定json工具转换
                    var tempList = Convert<T>(jsonString);
                    returnList.AddRange(tempList);
                    end = start + index;
                    start = end;
                }
                br.Close();
                fs.Close();
                return returnList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 反序列化方式
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private static List<T> Convert<T>(string json)
        {
            List<T> list = null;
            list = JsonFormatter.DeserializeObject<List<T>>(json);
            return list;
        }
    }
}

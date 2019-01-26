using Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Swifter.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Test
{
    public class Program
    {
        static void Main(string[] args)
        {
            //各种json工具测试
            SaveTest(JsonToolType.Newtonsoft); 
            ConvertTest(JsonToolType.FastJson);

            //类库测试
            //TestJsonFileConvertSaveJsonFile();
            //TestJsonFileConvertConvertJson();
        }

        /// <summary>
        /// 序列化保存测试
        /// </summary>
        /// <param name="type"></param>
        public static void SaveTest(JsonToolType type)
        {
            var path = Environment.CurrentDirectory + "\\json.txt";
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            int length = 100000;
            List<Student> list = new List<Model.Student>();
            for (var i = 0; i < length; i++)
            {
                Student student = new Model.Student();
                student.ID = i;
                student.Name = "Student"+i;
                student.Mobile = i.ToString();
                student.Address = "city"+i.ToString();
                list.Add(student);
            }
            var date1 = DateTime.Now;
            SaveJsonFile<Student>(list, path, type,1000);
            var date2 = DateTime.Now;
            Console.WriteLine("序列化用时:{0}秒", (date2 - date1).TotalSeconds);
        }
        /// <summary>
        /// 反序列化测试
        /// </summary>
        /// <param name="type"></param>
        public static  void ConvertTest(JsonToolType type)
        {
            var date1 = DateTime.Now;
            var path = Environment.CurrentDirectory + "\\json.txt";
            var list = ConvertJsonFile<Student>(path,type);
            if (list == null)
            {
                return;
            }
            var date2 = DateTime.Now;
            Console.WriteLine("反序列化用时:{0}秒", (date2 - date1).TotalSeconds);
        }
        /// <summary>
        /// 保存json
        /// </summary>
        /// 
        public static bool SaveJsonFile<T>(List<T> list, string savePath, JsonToolType type, int batchNumber = 1000)
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
            bool isFirst = false;
            bool isLast = false;
            for (var i = 0; i < count; i++)
            {
                isFirst = false;
                isLast = false;
                if (i == 0)
                {
                    isFirst = true;
                }
                if (i == count - 1)
                {
                    isLast = true;
                }
                var batchList = list.Skip(i * batchNumber).Take(batchNumber).ToList();
                SaveJsonBatch<T>(batchList, savePath, isFirst, isLast,type);
            }
            return true;
        }
        /// <summary>
        /// 批次保存
        /// </summary>
        private static void SaveJsonBatch<T>(List<T> list, string path, bool isFirst, bool isLast, JsonToolType type)
        {
            //实例化一个文件流—>与写入文件相关联 
            string jsonString = string.Empty;
            StringBuilder json = new StringBuilder();
            using (StreamWriter sw = new StreamWriter(path, true))
            {
                if (isFirst)
                {
                    jsonString = "[";
                }
                jsonString += ToJson<T>(list, type, isLast);
                if (isLast)
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
        private static string ToJson<T>(List<T> list, JsonToolType type, bool isLast)
        {
            string jsonString = string.Empty;
            StringBuilder str = new StringBuilder();
            switch (type)
            {
                case JsonToolType.Newtonsoft:
                    for (int i = 0; i < list.Count; i++)
                    {
                        var item = list[i];
                        if (i == list.Count - 1 && isLast)
                        {
                            str.Append(item.ToJson());
                        }
                        else
                        {
                            str.Append(item.ToJson() + "," + Environment.NewLine);
                        }
                    }
                    jsonString += str.ToString();
                    break;
                case JsonToolType.Swifter:
                    for (int i = 0; i < list.Count; i++)
                    {
                        var item = list[i];
                        if (i == list.Count - 1 && isLast)
                        {
                            str.Append(JsonFormatter.SerializeObject(item));
                        }
                        else
                        {
                            str.Append(JsonFormatter.SerializeObject(item) + "," + Environment.NewLine);
                        }
                    }
                    jsonString += str.ToString();
                    break;
                case JsonToolType.FastJson:
                    for (int i = 0; i < list.Count; i++)
                    {
                        var item = list[i];
                        if (i == list.Count - 1 && isLast)
                        {
                            str.Append(fastJSON.JSON.ToJSON(item));
                        }
                        else
                        {
                            str.Append(fastJSON.JSON.ToJSON(item) + "," + Environment.NewLine);
                        }
                    }
                    jsonString += str.ToString();
                    break;
            }
            return jsonString;
        }
        /// <summary>
        /// 二进制读取转换
        /// </summary>
        public static List<T> ConvertJsonFile<T>(string path, JsonToolType type)
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
                    var tempList = Convert<T>(jsonString,type);
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
        private static List<T> Convert<T>(string json, JsonToolType type)
        {
            List<T> list = null;
            switch (type)
            {
                case JsonToolType.Newtonsoft:
                    JsonReader reader = new JsonTextReader(new StringReader(json));
                    Newtonsoft.Json.Linq.JArray array = (JArray)JToken.ReadFrom(reader);
                    list = array.ToObject<List<T>>();
                    break;
                case JsonToolType.Swifter:
                    list = JsonFormatter.DeserializeObject<List<T>>(json);
                    break;
                case JsonToolType.FastJson:
                    list = fastJSON.JSON.ToObject<List<T>>(json);
                    break;
                default:
                    break;
            }
            return list;
        }
        /// <summary>
        /// 类库测试方法
        /// </summary>
        public static void TestJsonFileConvertSaveJsonFile()
        {
            var path = Environment.CurrentDirectory + "\\json.txt";
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            int length = 100000;
            List<Student> list = new List<Student>();
            for (var i = 0; i < length; i++)
            {
                Student student = new Student();
                student.ID = i;
                student.Name = "Student" + i;
                student.Mobile = i.ToString();
                student.Address = "city" + i.ToString();
                list.Add(student);
            }
            JsonFileConvert.JsonFileConvert.SaveJsonFile<Student>(list, path);
        }
        /// <summary>
        /// 类库测试方法
        /// </summary>
        public static void TestJsonFileConvertConvertJson()
        {
            var path = Environment.CurrentDirectory + "\\json.txt";
            var list = JsonFileConvert.JsonFileConvert.ConvertJsonFile<Student>(path);
            if (list == null)
            {
                return;
            }
        }
    }

    public enum JsonToolType
    {
        Newtonsoft = 1,
        Swifter = 2,
        FastJson = 3,
    }
}


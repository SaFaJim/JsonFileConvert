# JsonFileConvert

 将大数据量list 转换为json文本文件和将大数据量json文本文件(GB大小也不在话下)转换为list的类库
 =====================================================================================
 0 实用场景描述
 -------------
    可用于将单个表结构数据导出为json格式文本文件，内部实用了批次处理所以支持大数据list序列化,
    将json格式文本文件反序列化为list，json文本文件大小不受限制，读取文本文件采用了数据块读取技术，否则的将文本文件全部加载到内存里面容易耗尽 内存。
    反序列化后的数据list稍作加工就可以方便导入到表中。
    100万对象的序列化后生成的文本文件大概有500~8000M，实际看类的复杂程度，压缩后也就10M左右。
    
 1 依赖要求
 -------
   C# .netFramework2.0 以上，目前依赖于Swifter.json类库，因为目前序列化和反序列化这个类库效率最高,测试里面有对比Newtonsoft.json和FastJson
   
 2 类库代码结构
 -------------
   JsonFileConvert项目为发布使用类库，Test项目用于各个json组件跑测试。
   
 3  Newtonsoft.json,Swifter.json,fastJson 序列化为json文件时间对比
 ---------------------------------------------------------------
   10万个object的List序列化为json文本文件耗时对比列表，单位(秒)
   
|次数| Newtonsoft.json | Swifter.json | fastJson |
| ------ | ------ | ------ |-----|
|1  | 2.65   | 1.78    | 4.22   |
 |2| 2.87  | 1.53  | 4.42 |
 |3| 2.89  | 3.9   |  6.96|
 |4| 3.66 | 1.62  | 4.70 |
 |5| 3.32 | 1.97  |  6.83|
 |6| 3.46  | 1.59  |  3.45|
 |7| 2.99  | 1.66  |  6.39|
 |8| 3.11  | 1.50  |  3.18|
 
    100万个object的List序列化为json文本文件耗时对比列表，单位(秒),100w比较稳定只测试3次足够
    
|次数| Newtonsoft.json | Swifter.json | fastJson |
| ------ | ------ | ------ |-----|
|1  | 46.14  | 40.81    | 50.51   |
|2 | 43.75  | 34.59  | 50.01 |
|3 | 41.91  | 38.23   |  51.99|
 
 4  Newtonsoft.json,Swifter.json,fastJson 将文本json文件反序列化为list时间对比
 --------------------------------------------------------------------------
    10万个jsonString的文本文件反序列化时间对比单位(秒)，测试8次
    
|次数| Newtonsoft.json | Swifter.json | fastJson |
| ------ | ------ | ------ |-----|
|1  | 16.53   | 6.83    | 9.57   |
 |2| 15.97  | 6.76  |  8.91 |
 |3| 16.48  | 6.35   |  9.14|
 |4| 17.55 |  6.69  |  9.27 |
 |5| 17.84 | 5.85  |  9.14|
 |6| 17.20  | 6.26  |  9.19|
 |7| 14.82  | 6.56  |  9.64|
 |8| 17.05  | 6.10  |  9.47|
 
    100万个jsonString的文本文件反序列化时间对比单位(秒)，测试3次
    
|次数| Newtonsoft.json | Swifter.json | fastJson |
| ------ | ------ | ------ |-----|
|1  | 178.81   | 53.32    | 88.61   |
 |2|  158.89  | 52.01  |  89.13 |
 |3|140.72  |  56.83   |  83.11|
 
 经过测试比对发现Swifter.json做序列化和反序列化效率突出，所以这类库选择了它作为依赖组件。
 
 5 测试代码说明
 -------------
    Test项目Program->  SaveTest(JsonToolType.Newtonsoft); 方法为测试将list序列化后保存为json.txt文本文件，可以修改里面的生成list数量来测试
    Test项目Program->   ConvertTest(JsonToolType.FastJson);方法为测试将json.txt文本文件反序列化为list
    JsonToolType枚举类型定义如下
    ```
     public enum JsonToolType
    {
        Newtonsoft = 1,
        Swifter = 2,
        FastJson = 3,
    }
    ```
 
 
最后感谢Swifter.json 作者提供帮助，Swifter.json github的地址为 https://github.com/Dogwei/Swifter
JsonFileConvert类库有问题请提交说明或者发邮件到1812813640@qq.com

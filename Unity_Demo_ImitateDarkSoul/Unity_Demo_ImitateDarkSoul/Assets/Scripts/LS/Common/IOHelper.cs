using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace LS.Common
{
    /// IO辅助类
    /// JsonNet. 转为Json字符串
    /// JsonNet. Version  : 9.0.1
    /// Create Time:  2019.7.28
    /// Latest Time:2019.12.18
    /// 
    /// Update Log:
    ///         1.对io操作进行一个封装，Stream流的读写操作，同时将Set/GetData方法从file操作更改为Stream操作
    ///         2.Json操作添加泛型

    /// <summary>
    /// IO辅助类
    /// </summary>
    public static class IOHelper
    {
        #region Save/Load Data Methds

        /// <summary>
        /// 存储数据
        /// </summary>
        /// <param name="filePath">文件路径名</param>
        /// <param name="obj">存储对象</param>
        public static bool SetData(string filePath, object obj)
        {

            string toSave = SerializeObject(obj);
            return Stream_FileWriter(filePath, toSave, false, Encoding.UTF8);
            //StreamWriter writer = File.CreateText(filePath);
            //writer.Write(toSave);
            //writer.Close(); 
        }

        public static bool SetData<T>(string filePath,T obj)
        {
            string toSave = SerializeObject<T>(obj);
            return Stream_FileWriter(filePath, toSave, false, Encoding.UTF8);
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="filePath">文件路径名</param>
        /// <param name="type">数据类型</param>
        public static object GetData(string filePath, Type type)
        {
            Stream_FileRead(filePath, out string data, Encoding.UTF8);
            return DeserializeObject(data, type);
        }

        public static T GetData<T>(string filePath)
        {
            Stream_FileRead(filePath, out string data, Encoding.UTF8);
            return DeserializeObject<T>(data);
        }

        /// <summary>
        /// 将对象转化为字符串
        /// </summary>
        /// <param name="obj">目标对象</param>
        public static string SerializeObject(object obj, Formatting format = Formatting.None)
        {
            string serializedString = string.Empty;
            serializedString = JsonConvert.SerializeObject(obj,format);
            if (serializedString == string.Empty || serializedString == null)
            {
                Debug.LogWarning("IOHelper/SerializeObject Warning : the serialized string is null or empty ," +
                    "the object is " + obj);
            }
            return serializedString;
        }

        public static string SerializeObject<T>(T obj, Formatting format=Formatting.None)
        {
            string serializedString = string.Empty;
            serializedString = JsonConvert.SerializeObject(obj, format);
            if (serializedString == string.Empty || serializedString == null)
            {
                Debug.LogWarning("IOHelper/SerializeObject Warning : the serialized string is null or empty ," +
                    "the object is " + obj);
            }
            return serializedString;
        }

        /// <summary>
        /// 将字符串转换为目标对象
        /// </summary>
        /// <param name="serializedString">序列化字符串</param>
        /// <param name="type">对象类型</param>
        public static object DeserializeObject(string serializedString, Type type)
        {
            object obj = null;
            obj = JsonConvert.DeserializeObject(serializedString, type);

            if (obj == null)
            {
                Debug.LogWarning("IOHelper/DeserializeObject Warning : the deserialized string to object is null !");
            }
            return obj;
        }

        public static T DeserializeObject<T>(string serializedString)
        {
            T obj;
            obj = JsonConvert.DeserializeObject<T>(serializedString);
            if (obj == null)
            {
                Debug.LogWarning("IOHelper/DeserializeObject Warning : the deserialized string to object is null !");
            }
            return obj;
        }
        #endregion

        #region File Operation

        /// <summary>
        /// Stream流读取文件内容，行读取
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="data">文本数据</param>
        /// <param name="encodingType">编码</param>
        public static bool Stream_FileReadByLine(string path, out List<string> data, Encoding encodingType)
        {
            data = new List<string>();
            try
            {
                StreamReader reader = new StreamReader(path, encodingType);
                while (reader.Peek() >= 0)
                {
                    data.Add(reader.ReadLine());
                }
                reader.Close();
                return true;
            }
            catch (OutOfMemoryException)
            {
                Debug.LogError("IOHelper/ReadFileByLine Error : Out of memory !");
                return false;
            }catch(FileNotFoundException)
            {
                Debug.LogError("IOHelper/ReadFileByLine Error : Can't find the file ,the path of file is " + path);
                return false;
            }
        }

        /// <summary>
        /// Stream流读取文本数据，全部读取
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="data">文件数据</param>
        /// <param name="encodingType">编码</param>
        public static bool Stream_FileRead(string filePath,out string data,Encoding encodingType)
        {
            data = string.Empty;
            try
            {
                StreamReader reader = new StreamReader(filePath, encodingType);
                data = reader.ReadToEnd();
                reader.Close();
                return true;
            }catch(OutOfMemoryException)
            {
                Debug.LogError("IOHelper/ReadFile Error : Out of memory !");
                return false;
            }catch(FileNotFoundException)
            {
                Debug.LogError("IOHelper/ReadFile Error : Can't find the file ,the path of file is " + filePath) ;
                return false;
            }
        }

        /// <summary>
        /// 文本写入
        /// </summary>
        /// <param name="filePath">文本路径</param>
        /// <param name="data">数据</param>
        /// <param name="isCover">是否覆盖，true追加，false 覆盖 如果该文件不存在则创建</param>
        /// <param name="encodingType">编码</param>
        public static bool Stream_FileWriter(string filePath,string data,bool isCover,Encoding encodingType)
        {
            try
            {
                StreamWriter writer = new StreamWriter(filePath, isCover, encodingType);
                writer.Write(data);
                writer.Close();
                return true;
            }
            catch(ObjectDisposedException e)
            {
                Debug.Log(e.ToString());
                return false;
            }
            catch(NotSupportedException e)
            {
                Debug.Log(e.ToString());
                return false;
            }
        }

        /// <summary>
        /// 判断文件是否存在
        /// </summary>
        public static bool IsFileExists(string fileName)
        {
            return File.Exists(fileName);
        }

        /// <summary>
        /// 判断文件夹是否存在
        /// </summary>
        public static bool IsDirectoryExists(string fileName)
        {
            return Directory.Exists(fileName);
        }

        /// <summary>
        /// 创建一个文本文件    
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="content">文件内容</param>
        public static bool CreateFile(string filePath, string content)
        {
            return Stream_FileWriter(filePath,content, false, Encoding.UTF8);
            //StreamWriter streamWriter = File.CreateText(filePath);
            //streamWriter.Write(content);
            //streamWriter.Close();
        }

        /// <summary>
        /// 创建一个文件夹
        /// </summary>
        public static void CreateDirectory(string fileName)
        {
            //文件夹存在则返回
            if (IsDirectoryExists(fileName))
                return;
            Directory.CreateDirectory(fileName);
        }


        //http://www.voidcn.com/article/p-ombibfik-ry.html
        /// <summary>
        /// 根据指定的 Assets下的文件路径 返回这个路径下的所有文件名//
        /// </summary>
        /// <returns>文件名数组</returns>
        /// <param name="nameArray">存放name的数组</param>
        /// <param name="path">Assets下“一"级路径,例如Resources文件夹  "/Resources"  </param>
        /// <param name="passFiles">筛选文件后缀名的条件.</param>
        public static void GetFileNameToArray(ref List<string> nameArray, string path, params string[] passFiles)
        {
            string objPath = Application.dataPath + path;

            string[] directoryEntries;
            bool restart;

            try
            {
                //返回指定的目录中文件和子目录的名称的数组或空数组
                directoryEntries = Directory.GetFileSystemEntries(objPath);
                for (int i = 0; i < directoryEntries.Length; i++)
                {
                    restart = false;
                    string p = directoryEntries[i];
                    //得到要求目录下的文件或者文件夹（一级的）//
                    string[] tempPaths = SplitWithString(p, "/Assets" + path + "\\");

                    //tempPaths 分割后的不可能为空,只要directoryEntries不为空//
                    //先判断尾串是否有要筛选的后缀名，如果为空或者无要筛选的后缀名进入下一步
                    if (tempPaths[1].EndsWith(".meta"))
                        continue;

                    foreach (string str in passFiles)
                    {
                        if (tempPaths[1].EndsWith(str))
                        {
                            restart = true;
                            break;
                        }
                    }
                    if (restart)
                        continue;

                    //将后半串再进行处理，分割符号为  ‘.’此时，分割出来的前半串为文件名或者文件的路径，后半串为后缀名
                    //若空文件或者没有到最后一层，则pathsplit返回为空
                    string[] pathSplit = SplitWithString(tempPaths[1], ".");
                    //文件
                    if (pathSplit.Length > 1)
                    {
                        nameArray.Add(pathSplit[0]);
                    }
                    //遍历子目录下 递归吧！
                    else
                    {
                        GetFileNameToArray(ref nameArray, path + "/" + pathSplit[0], passFiles);
                        continue;
                    }
                }
            }
            catch (DirectoryNotFoundException)
            {
                Debug.LogError("IOHelper/GetObjectNameToArray Error ：" +
                    " The path encapsulated in the " + objPath + "Directory object does not exist.");
            }
        }

        /// <summary>
        /// 获取Assets文件夹下的指定路径的path-name字典，path仅包含Assets文件夹下的路径
        /// </summary>
        /// <param name="dic">获得到的<path-name>字典</param>
        /// <param name="path">路径 例如Resources文件夹  "/Resources" </param>
        /// <param name="passFiles">要忽略的文件后缀名</param>
        public static void GetFilePathNameToDic(ref Dictionary<string,string> dic,string path,params string[] passFiles)
        {
            string objPath = Application.streamingAssetsPath+ path;

            string[] directoryEntries;
            bool restart;
            try
            {
                directoryEntries = Directory.GetFileSystemEntries(objPath);

                for (int i=0;i<directoryEntries.Length;i++)
                {
                    restart = false;
                    string url = directoryEntries[i];
                    string[] tempPaths = SplitWithString(url, "/Assets/StreamingAssets" + path + "\\");
                    if (tempPaths[1].EndsWith(".meta"))
                        continue;
                    foreach(string cell in passFiles)
                    {
                        if(tempPaths[1].EndsWith(cell))
                        {
                            restart = true;
                            break;
                        }
                    }
                    if (restart)
                        continue;

                    string[] pathSplit = SplitWithString(tempPaths[1], ".");
                    if(pathSplit.Length>1)
                    {
                        string pathName = SplitWithString(url, "/Assets/StreamingAssets")[1];
                        dic.Add(pathName, pathSplit[0]);
                    }
                    else
                    {
                        GetFilePathNameToDic(ref dic, path + "/" + pathSplit[0], passFiles);
                        continue;
                    }
                }
            }
            catch (DirectoryNotFoundException)
            {
                Debug.LogError("IOHelper/GetObjectNameToArray Error ：" +
                    " The path encapsulated in the " + objPath + "Directory object does not exist.");
            }
        }

        #endregion

        #region Private Methods
        private static string[] SplitWithString(string sourceString, string splitString)
        {
            List<string> arrayList = new List<string>();
            string s = string.Empty;

            //例：sourceString：D:/OneDrive/Projects/Unity/LS_Scripts/Assets/AddressableAssetsData\AddressableAssetSettings.asset.meta
            //splitString:/Assets/AddressableAssetsData\
            while (sourceString.IndexOf(splitString) > -1)  //分割
            {
                //获取sourceString前半串，除开splitString
                //s=D:/OneDrive/Projects/Unity/LS_Scripts
                s = sourceString.Substring(0, sourceString.IndexOf(splitString));
                //获取sourceString后半串，除开splitString
                //sourceString=AddressableAssetSettings.asset.meta
                sourceString = sourceString.Substring(sourceString.IndexOf(splitString) + splitString.Length);
                arrayList.Add(s);
            }
            arrayList.Add(sourceString);
            return arrayList.ToArray();
        }
        #endregion
    }

}
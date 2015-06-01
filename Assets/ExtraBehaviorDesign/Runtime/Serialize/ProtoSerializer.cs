using ExtraBehaviorDesign.Runtime.Utility;
using ProtoBuf;
using System;
using System.IO;
using UnityEngine;
namespace ExtraBehaviorDesign.Runtime.Serialize
{
    public class ProtoSerializer
    {
        public static readonly string SERIALIZATION_CACHE_DATA_PATH = "Assets/SerializationCacheData/";
        public static readonly string DATA_POSTFIX = ".bytes";

        public static readonly string CONFIG_FILE_NAME = "SerializationConfigData/BehaviorTreeNamewithRootTaskNameMap";

        /// <summary>
        /// 序列化对象到指定文件内，成功将返回序列化文件路径,失败返回空路径
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string SerializeToFile(object obj, string fileName)
        {
            string serializationFilePath = "";

            if (null == obj)
            {
#if UNITY_EDITOR
                Debug.LogWarning("ProtoSerializer.Serialize : The object to serialize is NULL");
#endif
                return serializationFilePath;
            }

            // 目录路径
            string directoryPath = BehaviorTreeUtility.GetDirectorPathFromCompleteFilePath(fileName);
            if ("" == directoryPath)
            {
                directoryPath = SERIALIZATION_CACHE_DATA_PATH;
            }
            else
            {
                // 配置文件多一层目录
                directoryPath = SERIALIZATION_CACHE_DATA_PATH + directoryPath;
            }

            serializationFilePath = SERIALIZATION_CACHE_DATA_PATH + fileName + DATA_POSTFIX;

            byte[] messageArr;

            // 获取待序列化对象的类名
            string className = BehaviorTreeUtility.GetClassNameFromInstance(obj);
            // 获取待序列化对象的类型
            Type type = BehaviorTreeUtility.GetAssemblyQualifiedType(className);

            if (false == Directory.Exists(directoryPath))
            {
                try
                {
                    Directory.CreateDirectory(directoryPath);
                }
                catch (System.Exception)
                {
                    serializationFilePath = "";
#if UNITY_EDITOR
                    Debug.LogWarning("ProtoSerializer.Serialize : Can NOT SaveData because exception is thrown when creating folder!");
#endif
                }
            }

            // try to read the serilization data
            try
            {
                using (FileStream fileStream = File.Create(serializationFilePath))
                {
                    using (MemoryStream messageStream = new MemoryStream())
                    {
                        // 由于泛型未知，只能根据Type实例来调用ProtoBuf的Serlizer类的泛型函数Serialize
                        BehaviorTreeUtility.CallGenericFunctionByType(typeof(Serializer), "Serialize", new object[] { messageStream, obj }, type);
                        messageArr = messageStream.ToArray();

                        fileStream.Write(messageArr, 0, messageArr.Length);
                        fileStream.Close();
                    }
                }
	        }
            catch (System.Exception)
            {
#if UNITY_EDITOR
#if UNITY_EDITOR
                Debug.LogWarning("ProtoSerializer.Serialize : Can NOT SaveData " + serializationFilePath +
#endif
                    ", because exception is thrown when creating the file.");
#endif

                serializationFilePath = "";
            }
            return serializationFilePath;
        }

        /// <summary>
        /// 从指定文件（完整路径名）反序列化出指定类型的对象
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool DeserializeFromFile<T>(string filePath, Type type, ref T obj)
        {
            bool result = true;

            if ("" == filePath)
            {
#if UNITY_EDITOR
                Debug.LogWarning("ProtoSerializer.Deserialize: The filePath is not allowed empty when serializing from filestream");
#endif
                return false;
            }

            try
            {
                using (FileStream fileStream = File.OpenRead(filePath))
                {
                    // 由于泛型未知，只能根据Type实例来调用ProtoBuf的Serlizer类的泛型函数Deserialize
                    obj = (T)(BehaviorTreeUtility.CallGenericFunctionByType(typeof(Serializer), "Deserialize", new object[] { fileStream }, type));             
                }
            }
            catch (System.Exception)
            {
#if UNITY_EDITOR
#if UNITY_EDITOR
                Debug.LogWarning("ProtoSerializer.Serialize: Can NOT ReadData " + filePath +
#endif
                    ", because exception is thrown when finding or reading the file.");
#endif
            }

            return result;
        }

        /// <summary>
        /// <para>由于序列化到目标文件的文件名格式为Asset/SerializationCacheData/ + 行为树名 + .data</para>
        /// <para>将行为树-根结点映射表得到根结点类型</para>
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string GetValidateRootTaskNameFromSerFileName(string filePath)
        {
            string className = "";


            return className;
        }
    }
}

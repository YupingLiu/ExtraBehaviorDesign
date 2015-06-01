using System;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;
namespace ExtraBehaviorDesign.Runtime.Utility
{
    public class BehaviorTreeUtility
    {
        private static string[] nsSeparator = { "namespace" };
        private static char[] rightBraceSeparator = new char[] { '{' };
        private static string[] resultArray;

        /// <summary>
        /// Type.GetType的传入参数的有效性保护
        /// <para>注意：传入的类名是包含了命名空间的</para>
        /// </summary>
        /// <param name="className"></param>
        /// <returns></returns>
        public static Type GetAssemblyQualifiedType(string className)
        {
            Type type = Type.GetType(className);
            if (null == type)
            {
                type = Type.GetType(className + ", Assembly-CSharp");
            }
            if (null == type)
            {
                type = Type.GetType(className + ", Assembly-CSharp-firstpass");
            }
            if (null == type)
            {
#if UNITY_EDITOR
                Debug.LogError("不是当前程序集下有效的类名，请注意传入的类名的命名空间和程序集");
#endif
            }
            return type;
        }

        /// <summary>
        /// 根据对象得到类型
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string GetClassNameFromInstance(object obj)
        {
            string typeString = "";
            if (null != obj)
            {
                typeString = obj.ToString();
                char[] separators = { '\"', '(', ')' };
                string[] resultArray = typeString.Split(separators, System.StringSplitOptions.RemoveEmptyEntries);
                if (" " != resultArray[0])
                {
                    typeString = resultArray[0];
                }
                else
                {
                    typeString = resultArray[1];
                }
            }
            else
            {
                Debug.LogError("传入对象为空");
            }
            return typeString;
        }

        /// <summary>
        /// 传入.CS文件来获取包含命名空间的合法类名
        /// </summary>
        /// <param name="asset"></param>
        /// <returns></returns>
        public static string GetClassNameFromTextAsset(TextAsset asset)
        {
            string className = "";
            resultArray = asset.text.Split(nsSeparator, System.StringSplitOptions.RemoveEmptyEntries);

            // If the class has a namespace
            if (1 < resultArray.Length)
            {
                resultArray = resultArray[1].Split(rightBraceSeparator, System.StringSplitOptions.RemoveEmptyEntries);
                className = resultArray[0].Trim() + "." + Path.GetFileNameWithoutExtension(AssetDatabase.GetAssetPath(asset));
            }
            else
            {
                // Has no NS, use the filename as calssname
                className = Path.GetFileNameWithoutExtension(AssetDatabase.GetAssetPath(asset));
            }
            return className;
        }

        /// <summary>
        /// <para>已知泛型类型实例type，调用T类的带泛型参数的functionName方法</para>
        /// <para>例如：Proto的序列化函数Serilizer.Serilize&lt;T&gt; (stream, obj)，T未知，但可以从obj得到T的类型实例</para>
        /// <para>对应这个例子，传入的callerClass为Serilizer，functionName为"Serilize"，parameterArray为{stream, obj}，而type就是T的类型实例</para>
        /// </summary>
        /// <param name="callerClass"></param>
        /// <param name="functionName"></param>
        /// <param name="parameterArray"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object CallGenericFunctionByType(Type callerClass, string functionName, object[] parameterArray, Type type)
        {
            object returnValue;
            MethodInfo method = callerClass.GetMethod(functionName);
            MethodInfo genericMethod = method.MakeGenericMethod(type);
            returnValue = genericMethod.Invoke(null, parameterArray);
            return returnValue;
        }

        /// <summary>
        /// 传入一个路径名，返回剔除文件名的纯路径
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string GetDirectorPathFromCompleteFilePath(string filePath)
        {
            string directoryPath = "";
            char[] separators = { '/'};
            string[] resultArray = filePath.Split(separators, System.StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < resultArray.Length - 1; i++)
            {
                directoryPath += (resultArray[i] + '/');
            }
            return directoryPath;
        }

        /// <summary>
        /// 诶，这个方法多好呀，上面那个真是挫爆了~
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string GetDirectorPath(string filePath)
        {
            if ("" != Path.GetExtension(filePath))
            {
                // 如果有扩展名，说明是个文件，所以去掉文件名和扩展名，只留该路径
                filePath = filePath.Replace(Path.GetFileName(filePath), "");
            }
            return filePath;
        }

        public static string GetAssetPath(UnityEngine.Object asset)
        {
            string path = "";
            if (null == asset)
            {
#if UNITY_EDITOR
                Debug.LogWarning("GetAssetPath ERROR : asset is NULL");
#endif
                return path;
            }
            path = AssetDatabase.GetAssetPath(asset);
            return path;
        }
    }
}

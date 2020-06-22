using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace LS.Common
{
    public static class PathTools
    {
        public const string AB_ResPath = "AB_Res";

        /// <summary>获取AB包的路径</summary>
        public static string AB_OutPath => GetPlatfromPath() + '/' + GetPlatfromName();

        /// <summary> 本地AB包加载</summary>
        public static string LocalAB_OutPath => Application.streamingAssetsPath + "/AssetBundle";

        /// <summary>获取AB标记路径</summary>
        public static string AB_RessourcePath => Application.dataPath + "/" + AB_ResPath;

        /// <summary>获取平台路径 </summary>
        public static string GetPlatfromPath()
        {
            string strReturnPlatformPath = string.Empty;
            switch (Application.platform)
            {
                case RuntimePlatform.WindowsPlayer:
                case RuntimePlatform.WindowsEditor:
                    strReturnPlatformPath = Application.streamingAssetsPath;
                    break;
                case RuntimePlatform.Android:
                case RuntimePlatform.IPhonePlayer:
                    strReturnPlatformPath = Application.persistentDataPath;
                    break;
            }
            return strReturnPlatformPath;
        }

        /// <summary>
        /// 获取平台名称
        /// </summary>
        /// <returns></returns>
        public static string GetPlatfromName()
        {
            string strReturnPlatformName = string.Empty;
            switch (Application.platform)
            {
                case RuntimePlatform.WindowsPlayer:
                case RuntimePlatform.WindowsEditor:
                    strReturnPlatformName = "Windows";
                    break;
                case RuntimePlatform.IPhonePlayer:
                    strReturnPlatformName = "IPhone";
                    break;
                case RuntimePlatform.Android:
                    strReturnPlatformName = "Android";
                    break;
                case RuntimePlatform.LinuxPlayer:
                case RuntimePlatform.LinuxEditor:
                    strReturnPlatformName = "Linux";
                    break;
            }
            return strReturnPlatformName;
        }
        public static string GetWWWPath()
        {
            string WWWpath = string.Empty;
            switch (Application.platform)
            {
                case RuntimePlatform.WindowsPlayer:
                case RuntimePlatform.WindowsEditor:
                    WWWpath = AB_OutPath;
                    break;
                case RuntimePlatform.IPhonePlayer:
                    WWWpath = AB_OutPath + "/Raw/";
                    break;
                case RuntimePlatform.Android:
                    WWWpath = AB_OutPath;
                    break;
            }
            return WWWpath;
        }
    }
}

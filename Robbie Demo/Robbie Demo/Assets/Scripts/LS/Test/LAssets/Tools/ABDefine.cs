using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/// <summary>
///  LAssets需要的常量或者定义
/// </summary>
namespace LS.AssetsFrameWork
{
    /// <summary>
    /// 完成加载的委托
    /// </summary>
    /// <param name="abName">包名</param>
    public delegate void DelLoadCompleteHandle(string abName);

    /// <summary>
    /// LAssets常量
    /// </summary>
    public class ABDefine
    {
        public static string AB_MANIFEST = "AssetBundleManifest";
        //public static string INIT_AB_MANIFEST="In"
    }

    /// <summary>
    /// AB包的加载源
    /// <para>Local 默认都在StreamDataPath</para>
    /// <para>OnLine 会根据平台的不同加载不同的路径</para>
    /// </summary>
    public enum LoadAssetFromWhere
    {
        Local,
        OnLine
    }

}

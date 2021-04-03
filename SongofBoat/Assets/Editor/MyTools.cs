using UnityEditor;
using UnityEngine;
public class MyTools : Editor
{
    // Start is called before the first frame update
    [MenuItem("Tools/CreateAsset")]
    static void CreateAssetBundle(){
        string path = "AssetBundle";
        BuildPipeline.BuildAssetBundles(path,BuildAssetBundleOptions.None,)
    }
}

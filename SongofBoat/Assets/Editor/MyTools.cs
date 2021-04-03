using UnityEditor;
using System.IO;
public class MyTools : Editor
{
    // Start is called before the first frame update
    [MenuItem("Tools/CreateAsset")]
    static void CreateAssetBundle(){
        string path = "Assets/AssetBundle";
        if(!Directory.Exists(path)){
            Directory.CreateDirectory(path);
        }
        BuildPipeline.BuildAssetBundles(path,BuildAssetBundleOptions.None,BuildTarget.StandaloneWindows64);
        
    }
}

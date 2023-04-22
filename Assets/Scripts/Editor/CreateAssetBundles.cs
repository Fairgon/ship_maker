using UnityEditor;
using System.IO;

public class CreateAssetBundles
{
    [MenuItem("Assets/Build AssetBundles")]
    static void BuildAllAssetBundles()
    {
        string assetBundleDirectory = "Assets/AssetBundles";
        if (!Directory.Exists(assetBundleDirectory))
        {
            Directory.CreateDirectory(assetBundleDirectory);
        }
        BuildPipeline.BuildAssetBundles(assetBundleDirectory,
                                        BuildAssetBundleOptions.None,
                                        BuildTarget.StandaloneWindows);

        //AssetBundleBuild[] buildMap = new AssetBundleBuild[1];

        //buildMap[0].assetBundleName = "TestEditorBundle";

        //string[] enemyAssets = new string[1];
        //enemyAssets[0] = "Assets/ShipData.json";

        //buildMap[0].assetNames = enemyAssets;

        //BuildPipeline.BuildAssetBundles(assetBundleDirectory,
        //                                buildMap,
        //                                BuildAssetBundleOptions.None,
        //                                BuildTarget.StandaloneWindows);
    }
}

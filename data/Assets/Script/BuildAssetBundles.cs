#if UNITY_EDITOR

using System.IO;
using UnityEditor;

public class BuildAssetBundles
{
	public static readonly string AssetBundlesOutputFolder = "GeneratedAssetBundles";

	[MenuItem("Nuterra/Build AssetBundles")]
	private static void BuildAllAssetBundles()
	{
		Directory.CreateDirectory(AssetBundlesOutputFolder);
		BuildPipeline.BuildAssetBundles(AssetBundlesOutputFolder, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);
	}
}

#endif
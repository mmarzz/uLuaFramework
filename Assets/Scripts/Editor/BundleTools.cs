using UnityEditor;
using UnityEngine;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using SimpleFramework;
using SimpleFramework.Utils;

public class BundleTools {
    public static string resPath = Application.dataPath + "/Examples/Builds/";
	public static string bundlePath = Application.dataPath + "/Examples/StreamingAssets/";

    [MenuItem("Build/Test Build Resources", false, 1)]
    public static void TestBuild() {
        BuildResources(BuildTarget.StandaloneWindows);
    }


    private static void BuildResources(BuildTarget target) {
        if (EditorUserBuildSettings.activeBuildTarget != target) {
			Debugger.LogWarning(string.Format("BuildTarget error! target: {0}  active: {1}", target.ToString(), EditorUserBuildSettings.activeBuildTarget.ToString()));
			return;
		}

        BuildAssetBundleOptions options = BuildAssetBundleOptions.ChunkBasedCompression; // LZ4压缩
        FileUtils.ExistOrCreateDirectory(bundlePath);
        BuildPipeline.BuildAssetBundles(bundlePath, options, target);
    }
}
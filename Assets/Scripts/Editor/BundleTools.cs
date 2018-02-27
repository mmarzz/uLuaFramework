using UnityEditor;
using UnityEngine;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using SimpleFramework;
using SimpleFramework.Utils;

public class BundleTools {
    public static string dataPath = Application.dataPath;
    public static string buildPath = Application.dataPath + "/Examples/Builds/";
	public static string bundlePath = Application.dataPath + "/Examples/StreamingAssets/";

    public static bool isIncrementalBuild = true; // 是否增量打包 AssetBundle

    [MenuItem("Build/Test Build Resources", false, 1)]
    public static void TestBuild() {
        BuildResources(BuildTarget.StandaloneWindows);
    }

    private static void BuildResources(BuildTarget target) {
        if (EditorUserBuildSettings.activeBuildTarget != target) {
			Debugger.LogWarning(string.Format("BuildTarget error! target: {0}  active: {1}", target.ToString(), EditorUserBuildSettings.activeBuildTarget.ToString()));
			return;
		}

        CopyLua();
        SetAssetBundleName();
        BuildAssetBundles(target);
        // DeleteManifestFiles();

        AssetDatabase.Refresh();
    }

    private static void CopyLua() {
        string luaPath = dataPath + "/Lua";
        string luaBuildPath = buildPath + "/lua";

        FileUtils.ExistOrClearDirectory(luaBuildPath);
        FileUtils.CopyDirectory(luaPath, luaBuildPath, EditorUtils.FilterFile);

        // 为 lua 文件加 .bytes 后缀
        string[] luaFiles = Directory.GetFiles(luaBuildPath, "*.*", SearchOption.AllDirectories);
        foreach (string file in luaFiles) {
			if (EditorUtils.FilterFile(file)) {
				string newPath = file.Replace("\\", "/") + ".bytes";
			    File.Move(file, newPath);
            }
		}
    }

    public static void SetAssetBundleName() {
		List<string> bundleList = new List<string>();
		string[] files = Directory.GetFiles(buildPath, "*.*", SearchOption.AllDirectories);
		foreach (string file in files) {
			string newPath = file.Replace("\\", "/");
			if (EditorUtils.FilterFile(newPath)) {
				bundleList.Add(newPath);
			}
		}

        foreach (string file in bundleList) {
            string name = file.Replace(buildPath, "");
            string assetName = file.Replace(dataPath, "Assets");
            AssetImporter ai = AssetImporter.GetAtPath(assetName);

            if (ai != null) {
                Debugger.LogWarning(file + "\n" + name);
                ai.assetBundleName = name;
            }
        }
    }

    private static void BuildAssetBundles(BuildTarget target) {
        BuildAssetBundleOptions options = BuildAssetBundleOptions.ChunkBasedCompression; // LZ4 压缩
        if (!isIncrementalBuild) {
			options |= BuildAssetBundleOptions.ForceRebuildAssetBundle; // 强制重打 AssetBundles
		}

        FileUtils.ExistOrCreateDirectory(bundlePath);
        BuildPipeline.BuildAssetBundles(bundlePath, options, target);
    }

    private static void DeleteManifestFiles() {
		if (isIncrementalBuild)
			return;
		string[] fileList = Directory.GetFiles(bundlePath, "*.manifest", SearchOption.AllDirectories);
		foreach (string file in fileList) {
			if (!file.EndsWith("StreamingAssets.manifest"))
				File.Delete(file);
		}
	}

}
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
        DeleteManifestFiles();
        SaveFilesText(target);

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

    private static void SaveFilesText(BuildTarget target) {
        string streamingPath = bundlePath + "StreamingAssets";
		AssetBundle asset = AssetBundle.LoadFromFile(streamingPath);
		AssetBundleManifest bundleManifest = asset.LoadAsset("AssetBundleManifest") as AssetBundleManifest;
		asset.Unload(false);

		Hashtable bundleDatas = new Hashtable();
		string[] bundleList = bundleManifest.GetAllAssetBundles();
		foreach (string bundleName in bundleList) {
            ArrayList fileInfo = new ArrayList();

			string abPath = bundlePath + bundleName;
            
            // TODO 查找依赖
            // SortList<string> deps = GetBundleDependencies(obj);
			// byte[] buffer = GetFilesBuffer(deps);

            byte[] fileBytes = FileUtils.ReadBytes(abPath);
            long fileSize = fileBytes.Length;
            string fileMD5 = MD5Utils.GetMD5(fileBytes);
			
            fileInfo.Add(fileSize);
            fileInfo.Add(fileMD5);
			bundleDatas.Add(bundleName, fileInfo);
        }

        // 保存到 files.txt
        string tmpPath = "Assets/tmp/"; // 一定要写绝对的路径
        string filePath = tmpPath + "files.txt";
        string jsonStr = MiniJSON.jsonEncode(bundleDatas);
        byte[] bytes = Encoding.UTF8.GetBytes(jsonStr);
        FileUtils.WriteBytes(filePath, bytes);

        AssetDatabase.Refresh();

        // 生成 assetbundle
        AssetBundleBuild[] buildMap = new AssetBundleBuild[1];
		string[] assetNames = new string[1];
		BuildAssetBundleOptions options = BuildAssetBundleOptions.ChunkBasedCompression; //LZ4压缩

        assetNames[0] = filePath;
		buildMap[0].assetNames = assetNames;
		buildMap[0].assetBundleName = "files";
		BuildPipeline.BuildAssetBundles(tmpPath, buildMap, options, target);

        // copy
		FileUtils.CopyFile(tmpPath + "files", bundlePath + "files");
		FileUtils.CopyFile(filePath, bundlePath + "files.txt");
		Directory.Delete(tmpPath, true);
		AssetDatabase.Refresh();

    }

}
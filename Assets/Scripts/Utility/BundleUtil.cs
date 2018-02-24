using UnityEngine;
using System;

// using SimpleFramework.Manager;

namespace SimpleFramework.Utils {
    public class BundleUtil {

    	public static string FileName = "files.txt";

        public static string PersistentDataPath { get { return Application.persistentDataPath + "/"; } }
        // 下载数据的目录
        public static string UpdateDataPath { get { return Application.persistentDataPath + "/update/data/"; } }
        // 下载缓存的目录
        public static string UpdateCachePath { get { return Application.persistentDataPath + "/update/cache/"; } }

        // 包内数据目录
		public static string StreamingDataPath {
			get
			{
				// if (Application.platform == RuntimePlatform.IPhonePlayer) {
				// 	return Application.dataPath + "/Raw/";
				// }

				// if (Application.platform == RuntimePlatform.Android) {
				// 	return Application.dataPath + "!assets/"; 
				// }
				return Application.streamingAssetsPath + "/";
				// return Application.dataPath + "/StreamingAssets/";
			}
		}
    }
}
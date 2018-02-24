using UnityEngine;
using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Reflection;
using LuaInterface;
using SimpleFramework.Manager;


namespace SimpleFramework.Utils {
    public class BundleUtil {


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
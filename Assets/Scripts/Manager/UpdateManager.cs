using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.IO;
using System;

using SimpleFramework.Utils;

namespace SimpleFramework.Manager {
    public class UpdateManager : MonoBehaviour {


    	Hashtable localFileList;
		Hashtable serverFileList;
		Hashtable updateFileList;
        public void Init() {

        	// Debugger.LogWarning("Application.persistentDataPath:" + Application.persistentDataPath);
        	// Debugger.LogWarning("Application.streamingAssetsPath:" + Application.streamingAssetsPath);
        	// Debugger.LogWarning("Application.dataPath:" + Application.dataPath);

            bool isExists = Directory.Exists(BundleUtils.UpdateDataPath) && File.Exists(BundleUtils.UpdateDataPath + BundleUtils.FileName);
            if (!AppConst.DebugMode && !isExists) 
           		StartCoroutine(ExtractResource());
        }

        IEnumerator ExtractResource() {
            string destPath = BundleUtils.UpdateDataPath;  // destination 下载数据目录
            string srcPath = BundleUtils.StreamingDataPath; // source 游戏包资源目录

            FileUtils.ExistOrClearDirectory(destPath);

            Debugger.Log("Extract --->");

            FileUtils.CopyDirectory(srcPath, destPath);

            Debugger.Log("<--- Extract Completed!");

            yield return null;
        }

        public void CheckUpdate() {
        	CheckUpdateFile((count) => {
           		Debugger.Log("<--- Update File Count" + count);
           		if (count > 0) {
           			HotUpdate();
           		}
           	});
        }


        /// <summary>
        ///	检查需要更新的文件数量 & 大小 TODO
        /// 会改变变量 localFileList serverFileList updateFileList
        /// </summary>
        public void CheckUpdateFile(UIEventListener.CallbackInt cb) {
        	if (!AppConst.UpdateMode) {
        		if (cb != null)
					cb(-1);
        	}

        	string fileUrl = AppConst.WebUrl + BundleUtils.FileName;

        	Debugger.Log("Load Update --->" + fileUrl);

        	int TIME_OUT = 100;
        	WWWLoadBytesAsync(fileUrl, TIME_OUT, (bytes) => {

        		string serverFlie = Encoding.UTF8.GetString(bytes);
        		Hashtable serverFileList = LoadFileStringToTable(serverFlie);

				string localFilePath = BundleUtils.UpdateDataPath + BundleUtils.FileName;
				string localFile = null;
            
	            if (File.Exists(localFilePath))
	            {
	                byte[] localBytes = FileUtils.ReadBytes(localFilePath);
	                localFile = Encoding.UTF8.GetString(localBytes);
	            }
	            localFileList = LoadFileStringToTable(localFile);
				
				int count = 0;
				updateFileList = new Hashtable();
				foreach (string key in serverFileList.Keys) {
					string serverValue = serverFileList[key].ToString();
					if (localFileList.Contains(key)) {
						string localValue = localFileList[key].ToString();
						if (!localValue.Equals(serverValue)) { // 修改的文件
							updateFileList.Add(key, serverValue);
							count ++;
						}
					} else { // 新增的文件
						updateFileList.Add(key, serverValue);
						count ++;
					}
				}

				if (cb != null)
					cb(count);
        	});

        }

        public void HotUpdate() {
        	ioo.GameManager.StartCoroutine(_HotUpdate());
        }

        private IEnumerator _HotUpdate() {
        	string cachePath = BundleUtils.UpdateCachePath;
        	FileUtils.ExistOrClearDirectory(BundleUtils.UpdateCachePath); // 清空 update/cache

        	foreach (string fileName in updateFileList.Keys) {
        		string serverFileUrl = AppConst.WebUrl + fileName;
        		Debugger.Log("Begin Download ---> " + fileName);

        		UnityWebRequest www = UnityWebRequest.Get(serverFileUrl);
        		yield return www.Send();

        		while (!www.isDone && string.IsNullOrEmpty(www.error))
					yield return null;

				byte[] bytes = null;
				if (www.isError) {  
	                Debugger.LogError("<--- Error Download" + fileName + "error: " + www.error);
	                OnUpdateFailed(); // 更新失败
	                yield break; 
	            } else {
	            	bytes = www.downloadHandler.data;
	            }

	            // 写入 update/cache
	            string cacheFilePath = cachePath + fileName;
	            FileUtils.WriteBytes(cacheFilePath, bytes);
	            // 修改 localFileList 作为 cacheFileList
	            string value = updateFileList[fileName].ToString();
	            if (localFileList.Contains(fileName)) {
	            	localFileList[fileName] = value;
	            } else {
	            	localFileList.Add(fileName, value);
	            }
	         Debugger.Log("<--- Success Download" + fileName);

        	}

			// 写入 update/files.txt
        	string cacheFile = LoadFileTableToString(localFileList);
	        string cacheFileTxtPath = cachePath + BundleUtils.FileName;
	        FileUtils.WriteBytes(cacheFileTxtPath, Encoding.ASCII.GetBytes(cacheFile));
        
        	// 移动文件
        	if (!FileUtils.CopyDirectory(BundleUtils.UpdateCachePath, BundleUtils.UpdateDataPath))
        		OnUpdateFailed();

        	FileUtils.ExistOrClearDirectory(BundleUtils.UpdateCachePath); // 清空 update/cache

        	// 更新成功
        	Debugger.Log("<--- Success Update");
        	CheckUpdate(); // 再 check 一遍，防止在更新时又加上新的热更新
        	OnUpdateSuccessed();
        }

        private void WWWLoadBytesAsync(string path, int timeout, UIEventListener.CallbackBytes cb) {
        	ioo.GameManager.StartCoroutine(_WWWLoadBytesAsync(path, timeout, cb));
        }

        private IEnumerator _WWWLoadBytesAsync(string path, int timeout, UIEventListener.CallbackBytes cb) {
        	byte[] bytes = null;

        	using (UnityWebRequest www = UnityWebRequest.Get(path)) {
				www.timeout = timeout; 
	            yield return www.Send();

	            while (!www.isDone && string.IsNullOrEmpty(www.error))
					yield return null;

				if (www.isError) {  
	                Debugger.LogError(www.error);  
	            } else {
	            	bytes = www.downloadHandler.data;
	            }
	        }

	        if (cb != null)
	        	cb(bytes);
        }

        private Hashtable LoadFileStringToTable(string content) {
        	string[] files = content.Split('\n');
        	Hashtable table = new Hashtable();
        	foreach (string file in files) {
        		if (string.IsNullOrEmpty(file)) 
        			continue;
        		string[] keyValue = file.Split('|');
        		table.Add(keyValue[0], keyValue[1]);
        	}
        	return table;
        }


		private string LoadFileTableToString(Hashtable table) {
        	StringBuilder sb = new StringBuilder();
        	foreach (DictionaryEntry kv in table) {
        		sb.Append(kv.Key + "|" + kv.Value + "\n");
        	}
        	return sb.ToString();
        }

        private void OnUpdateFailed() {

        }

        private void OnUpdateSuccessed() {
        	
        }

    }
}
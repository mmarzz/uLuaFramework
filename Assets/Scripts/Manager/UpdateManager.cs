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

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Init() {

        	// Debugger.LogWarning("Application.persistentDataPath:" + Application.persistentDataPath);
        	// Debugger.LogWarning("Application.streamingAssetsPath:" + Application.streamingAssetsPath);
        	// Debugger.LogWarning("Application.dataPath:" + Application.dataPath);

            bool isExists = Directory.Exists(BundleUtil.UpdateDataPath) && File.Exists(BundleUtil.UpdateDataPath + BundleUtil.FileName);
            if (!AppConst.DebugMode && !isExists) 
           		StartCoroutine(ExtractResource());
            
            // TODO
            // StartCoroutine(OnUpdateResource());

           	CheckUpdate();
            // OnResourceInited();
        }

        IEnumerator ExtractResource() {
            string destPath = BundleUtil.UpdateDataPath;  // destination 下载数据目录
            string srcPath = BundleUtil.StreamingDataPath; // source 游戏包资源目录

            FileUtil.ExistOrClearDirectory(destPath);

            Debugger.Log("Extract --->");

            FileUtil.CopyDirectory(srcPath, destPath);

            Debugger.Log("<--- Extract Completed!");

            yield return null;
        }

        public void CheckUpdate() {
        	CheckUpdateFile((count) => {
           		Debugger.Log("<--- Update File Count" + count);
           		// foreach (string key in updateFileList.Keys) {
           			// Debugger.LogWarning(key);
           		// }
           		if (count > 0) {
           			HotUpdate();
           		}
           	});
        }


        /// <summary>
        ///	检查需要更新的文件数量&大小 TODO
        /// 会改变变量 localFileList serverFileList updateFileList
        /// </summary>
        public void CheckUpdateFile(UIEventListener.CallbackInt cb) {
        	if (!AppConst.UpdateMode) {
        		if (cb != null)
					cb(-1);
        	}

        	string fileUrl = AppConst.WebUrl + BundleUtil.FileName;

        	Debugger.Log("Load Update --->" + fileUrl);

        	int TIME_OUT = 100;
        	WWWLoadBytesAsync(fileUrl, TIME_OUT, (bytes) => {

        		string serverFlie = Encoding.UTF8.GetString(bytes);
        		Hashtable serverFileList = LoadFileStringToTable(serverFlie);

				string localFilePath = BundleUtil.UpdateDataPath + BundleUtil.FileName;
				string localFile = null;
            
	            if (File.Exists(localFilePath))
	            {
	                byte[] localBytes = File.ReadAllBytes(localFilePath);
	                localFile = Encoding.UTF8.GetString(localBytes);
	            }
	            localFileList = LoadFileStringToTable(localFile);
				
				int count = 0;
				updateFileList = new Hashtable();
				foreach (string key in serverFileList.Keys) {
					string serverValue = (string) serverFileList[key];
					if (localFileList.Contains(key)) {
						string localValue = (string) localFileList[key];
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
        	string cachePath = BundleUtil.UpdateCachePath;
        	FileUtil.ExistOrClearDirectory(BundleUtil.UpdateCachePath); // 清空 update/cache

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
	            FileUtil.Write(cacheFilePath, bytes);
	            // 修改 localFileList 作为 cacheFileList
	            string value = (string) updateFileList[fileName];
	            if (localFileList.Contains(fileName)) {
	            	localFileList[fileName] = value;
	            } else {
	            	localFileList.Add(fileName, value);
	            }
	         Debugger.Log("<--- Success Download" + fileName);

        	}

			// 写入 update/files.txt
        	string cacheFile = LoadFileTableToString(localFileList);
	        string cacheFileTxtPath = cachePath + BundleUtil.FileName;
	        FileUtil.Write(cacheFileTxtPath, Encoding.ASCII.GetBytes(cacheFile));
        
        	// TODO 剪切文件
        	if (!FileUtil.CopyDirectory(BundleUtil.UpdateCachePath, BundleUtil.UpdateDataPath))
        		OnUpdateFailed();

        	FileUtil.ExistOrClearDirectory(BundleUtil.UpdateCachePath); // 清空 update/cache

        	// 更新成功
        	Debugger.Log("<--- Success Update");
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
        	CheckUpdate(); // 再 check 一遍，防止在更新时又加上新的热更新
        }

        /// <summary>
        /// 资源初始化结束
        /// </summary>
        public void OnResourceInited() {
        	// TODO
            ioo.LuaManager.Start();
            // ioo.LuaManager.DoFile("Logic/Network");       //加载网络
            ioo.LuaManager.DoFile("Logic/GameManager");    //加载游戏
            // initialize = true;                     //初始化完 

            // ioo.NetworkManager.OnInit();    //初始化网络

            object[] panels = ioo.LuaManager.CallLuaFunction("GameManager.LuaScriptPanel");  
            //---------------------Lua面板---------------------------
            foreach (object o in panels) {
                string name = o.ToString().Trim();
                if (string.IsNullOrEmpty(name)) continue;
                name += "Panel";    //添加

                ioo.LuaManager.DoFile("View/" + name);
                Debugger.LogWarning("LoadLua---->>>>" + name + ".lua");
            }
            //------------------------------------------------------------
            ioo.LuaManager.CallLuaFunction("GameManager.OnInitOK");   //初始化完成
        }

        void OnUpdateFailed(string file) {
            string message = "更新失败!>" + file;
            Debugger.LogError(message);
            // facade.SendMessageCommand(NotiConst.UPDATE_MESSAGE, message);
        }

    }
}
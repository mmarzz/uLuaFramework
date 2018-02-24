using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

using SimpleFramework.Utils;

namespace SimpleFramework.Manager {
    public class UpdateManager : MonoBehaviour {
		
		private List<string> downloadFiles = new List<string>();

        /// <summary>
        /// 释放资源
        /// </summary>
        public void CheckExtractResource() {

        	Debugger.LogWarning("Application.persistentDataPath:" + Application.persistentDataPath);
        	Debugger.LogWarning("Application.streamingAssetsPath:" + Application.streamingAssetsPath);
        	Debugger.LogWarning("Application.dataPath:" + Application.dataPath);

            bool isExists = Directory.Exists(Util.DataPath) &&
              Directory.Exists(Util.DataPath + "lua/") && File.Exists(Util.DataPath + "files.txt");
            if (isExists || AppConst.DebugMode) {
                StartCoroutine(OnUpdateResource());
                return;   //文件已经解压过了，自己可添加检查文件列表逻辑
            }
            StartCoroutine(OnExtractResource());    //启动释放协成 
        }

        IEnumerator OnExtractResource() {
            string dataPath = Util.DataPath;  //数据目录
            string resPath = Util.AppContentPath(); //游戏包资源目录

            if (Directory.Exists(dataPath)) Directory.Delete(dataPath, true);
            Directory.CreateDirectory(dataPath);

            string infile = resPath + "files.txt";
            string outfile = dataPath + "files.txt";
            if (File.Exists(outfile)) File.Delete(outfile);

            string message = "正在解包文件:>files.txt";
            Debugger.Log(message);
            // facade.SendMessageCommand(NotiConst.UPDATE_MESSAGE, message);

            if (Application.platform == RuntimePlatform.Android) {
                WWW www = new WWW(infile);
                yield return www;

                if (www.isDone) {
                    File.WriteAllBytes(outfile, www.bytes);
                }
                yield return 0;
            } else File.Copy(infile, outfile, true);
            yield return new WaitForEndOfFrame();

            //释放所有文件到数据目录
            string[] files = File.ReadAllLines(outfile);
            foreach (var file in files) {
                string[] fs = file.Split('|');
                infile = resPath + fs[0];  //
                outfile = dataPath + fs[0];

                message = "正在解包文件:>" + fs[0];
                Debugger.Log("正在解包文件:>" + infile);
                // facade.SendMessageCommand(NotiConst.UPDATE_MESSAGE, message);

                string dir = Path.GetDirectoryName(outfile);
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

                if (Application.platform == RuntimePlatform.Android) {
                    WWW www = new WWW(infile);
                    yield return www;

                    if (www.isDone) {
                        File.WriteAllBytes(outfile, www.bytes);
                    }
                    yield return 0;
                } else {
                    if (File.Exists(outfile)) {
                        File.Delete(outfile);
                    }
                    File.Copy(infile, outfile, true);
                }
                yield return new WaitForEndOfFrame();
            }
            message = "解包完成!!!";
            Debugger.LogWarning(message);
            // facade.SendMessageCommand(NotiConst.UPDATE_MESSAGE, message);

            yield return new WaitForSeconds(0.1f);
            message = string.Empty;

            //释放完成，开始启动更新资源
            StartCoroutine(OnUpdateResource());
        }

        /// <summary>
        /// 启动更新下载，这里只是个思路演示，此处可启动线程下载更新
        /// </summary>
        IEnumerator OnUpdateResource() {
            downloadFiles.Clear();

            if (!AppConst.UpdateMode) {
                ioo.ResourceManager.initialize(OnResourceInited);
                yield break;
            }
            string dataPath = Util.DataPath;  //数据目录
            string url = AppConst.WebUrl;
            string random = DateTime.Now.ToString("yyyymmddhhmmss");
            string listUrl = url + "files.txt?v=" + random;
            Debugger.LogWarning("LoadUpdate---->>>" + listUrl);

            WWW www = new WWW(listUrl); yield return www;
            if (www.error != null) {
                OnUpdateFailed(string.Empty);
                yield break;
            }
            if (!Directory.Exists(dataPath)) {
                Directory.CreateDirectory(dataPath);
            }
            File.WriteAllBytes(dataPath + "files.txt", www.bytes);

            string filesText = www.text;
            string[] files = filesText.Split('\n');

            string message = string.Empty;
            for (int i = 0; i < files.Length; i++) {
                if (string.IsNullOrEmpty(files[i])) continue;
                string[] keyValue = files[i].Split('|');
                string f = keyValue[0];
                string localfile = (dataPath + f).Trim();
                string path = Path.GetDirectoryName(localfile);
                if (!Directory.Exists(path)) {
                    Directory.CreateDirectory(path);
                }
                string fileUrl = url + keyValue[0] + "?v=" + random;
                bool canUpdate = !File.Exists(localfile);
                if (!canUpdate) {
                    string remoteMd5 = keyValue[1].Trim();
                    string localMd5 = Util.md5file(localfile);
                    canUpdate = !remoteMd5.Equals(localMd5);
                    if (canUpdate) File.Delete(localfile);
                }
                if (canUpdate) {   //本地缺少文件
                    Debugger.Log(fileUrl);
                    message = "downloading>>" + fileUrl;
                    Debugger.Log(message);
                    // facade.SendMessageCommand(NotiConst.UPDATE_MESSAGE, message);
                   
                    //这里都是资源文件，用线程下载
                    BeginDownload(fileUrl, localfile);
                    while (!(IsDownOK(localfile))) { yield return new WaitForEndOfFrame(); }
                }
            }
            yield return new WaitForEndOfFrame();
            message = "更新完成!!";
            Debugger.Log(message);
            // facade.SendMessageCommand(NotiConst.UPDATE_MESSAGE, message);

            ioo.ResourceManager.initialize(OnResourceInited);
        }

        /// <summary>
        /// 是否下载完成
        /// </summary>
        bool IsDownOK(string file) {
            return downloadFiles.Contains(file);
        }

        /// <summary>
        /// 线程下载
        /// </summary>
        void BeginDownload(string url, string file) {     //线程下载
            object[] param = new object[2] {url, file};

            ThreadEvent ev = new ThreadEvent();
            ev.Key = NotiConst.UPDATE_DOWNLOAD;
            ev.evParams.AddRange(param);
            ioo.ThreadManager.AddEvent(ev, OnThreadCompleted);   //线程下载
        }

        /// <summary>
        /// 线程完成
        /// </summary>
        /// <param name="data"></param>
        void OnThreadCompleted(NotiData data) {
            switch (data.evName) {
                case NotiConst.UPDATE_EXTRACT:  //解压一个完成
                    //
                break;
                case NotiConst.UPDATE_DOWNLOAD: //下载一个完成
                    downloadFiles.Add(data.evParam.ToString());
                break;
            }
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

            ioo.NetworkManager.OnInit();    //初始化网络

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
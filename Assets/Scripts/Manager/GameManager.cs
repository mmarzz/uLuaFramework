using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using LuaInterface;
using System.Reflection;
using System.IO;

using SimpleFramework.Utils;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SimpleFramework.Manager {
    public class GameManager : MonoBehaviour {
        

        LuaScriptMgr luaManager = null;
        public LuaScriptMgr LuaManager {
            get {
                if (luaManager == null) {
                    luaManager = new LuaScriptMgr();
                    luaManager.Start();
                    // luaMgr.name = (++luaIndex).ToString();
                }
                return luaManager;
            }
        }

        void Awake() {
            Init();
        }

        void Init() {
            // if (AppConst.ExampleMode) {
                // InitGui();
            // }
            DontDestroyOnLoad(gameObject);  //防止销毁自己

            InitManagers();
            

            // ioo.UpdateManager.Init(); //释放资源
            
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            Application.targetFrameRate = AppConst.GameFrameRate;

            //
            Test();
        }


        private void InitManagers() {
            Util.Add<UpdateManager>(gameObject).Init();

            Util.Add<PanelManager>(gameObject);
            Util.Add<MusicManager>(gameObject);
            Util.Add<ResourceManager>(gameObject);
            // Util.Add<UIManager>(gameObject);
            Util.Add<TimerManager> (gameObject);
            Util.Add<NetworkManager> (gameObject);
            Util.Add<ThreadManager>(gameObject);
        }
        // public void InitGui() {
        //     string name = "GUI";
        //     GameObject gui = GameObject.Find(name);
        //     if (gui != null) return;

        //     GameObject prefab = Util.LoadPrefab(name);
        //     gui = Instantiate(prefab) as GameObject;
        //     gui.name = name;
        // }

        private void Test() {
            ioo.UpdateManager.CheckUpdate();

            OnResourceInited();
        }

          /// <summary>
        /// 资源初始化结束
        /// </summary>
        public void OnResourceInited() {
            // TODO
            // ioo.LuaManager.Start();
            // ioo.LuaManager.DoFile("Logic/Network");       //加载网络
            LuaManager.DoFile("Logic/GameManager");    //加载游戏
            // initialize = true;                     //初始化完 

            // ioo.NetworkManager.OnInit();    //初始化网络

            LuaManager.CallLuaFunction("GameManager.OnInitOK");   //初始化完成

            object[] panels = LuaManager.CallLuaFunction("GameManager.LuaScriptPanel");  
            //---------------------Lua面板---------------------------
            foreach (object o in panels) {
                string name = o.ToString().Trim();
                if (string.IsNullOrEmpty(name)) continue;
                name += "Panel";    //添加

                LuaManager.DoFile("View/" + name);
                Debugger.LogWarning("LoadLua---->>>>" + name + ".lua");
            }
            //------------------------------------------------------------
            
        }
        
        void Update() {
            if (LuaManager != null) {
                LuaManager.Update();
            }
        }

        void LateUpdate() {
            if (LuaManager != null) {
                LuaManager.LateUpate();
            }
        }

        void FixedUpdate() {
            if (LuaManager != null) {
                LuaManager.FixedUpdate();
            }
        }

        /// <summary>
        /// 析构函数
        /// </summary>
        void OnDestroy() {
            // if (NetManager != null) {
            //     NetManager.Unload();
            // }
            LuaManager.Destroy();
            Debugger.Log("~GameManager was destroyed");
        }
    }
}
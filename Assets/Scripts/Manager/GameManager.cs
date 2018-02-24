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
            

            ioo.UpdateManager.Init(); //释放资源
            
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            Application.targetFrameRate = AppConst.GameFrameRate;
        }


        private void InitManagers() {
            Util.Add<PanelManager>(gameObject);
            Util.Add<MusicManager>(gameObject);
            Util.Add<ResourceManager>(gameObject);
            Util.Add<UpdateManager>(gameObject);
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
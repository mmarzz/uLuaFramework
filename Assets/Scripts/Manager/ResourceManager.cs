using UnityEngine;
using System.Collections;
using System.Text;
using System.IO;
using System;

using SimpleFramework.Utils;

namespace SimpleFramework.Manager {
    public class ResourceManager : MonoBehaviour {
        private AssetBundle shared;

        public void initialize(Action func) {
            if (AppConst.ExampleMode) {
                byte[] stream;
                string uri = string.Empty;
                //------------------------------------Shared--------------------------------------
                uri = BundleUtils.UpdateDataPath + "shared.assetbundle";
                Debug.LogWarning("LoadFile::>> " + uri);

                stream = File.ReadAllBytes(uri);
                shared = AssetBundle.LoadFromMemory(stream);
#if UNITY_5
        shared.LoadAsset("Dialog", typeof(GameObject));
#else
                shared.Load("Dialog", typeof(GameObject));
#endif
            }
            if (func != null) func();   
        }


        public byte[] LoadLuaBytes(string luaName) {
            string filePath;
            if (string.IsNullOrEmpty(Path.GetExtension(luaName))) {
                filePath = string.Format("lua/{0}.lua", luaName);
            } else {
                filePath = string.Format("lua/{0}", luaName);
            }

            filePath = filePath.ToLower();

            if (AppConst.DebugMode) { // debug 模式下读项目内 lua
                filePath = Application.dataPath + "/" + filePath;
            } else {
                filePath = BundleUtils.UpdateDataPath + "/" + filePath;
            }

            byte[] bytes = FileUtils.ReadBytes(filePath);

            return bytes;
        } 


        public AssetBundle LoadBundle(string name) {
            string uri = BundleUtils.UpdateDataPath + name.ToLower() + ".assetbundle";

            return LoadAssetBundle(uri);
        }

        public AssetBundle LoadAssetBundle(string path) {
            AssetBundle bundle = null;
            bundle = AssetBundle.LoadFromFile(path);

            if (bundle == null) {
                Debugger.LogError(string.Format("Loading Asset Bundle Error: {0}", path));
            }
            return bundle;
        }

        void OnDestroy() {
            if (shared != null) shared.Unload(true);
            Debug.Log("~ResourceManager was destroy!");
        }
    }
}
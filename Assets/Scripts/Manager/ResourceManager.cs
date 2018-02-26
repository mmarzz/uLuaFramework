using UnityEngine;
using System.Collections;
using System.Text;
using System.IO;
using System;

using SimpleFramework.Utils;

namespace SimpleFramework.Manager {
    public class ResourceManager : MonoBehaviour {
        private AssetBundle shared;

        /// <summary>
        /// ��ʼ��
        /// </summary>
        public void initialize(Action func) {
            if (AppConst.ExampleMode) {
                byte[] stream;
                string uri = string.Empty;
                //------------------------------------Shared--------------------------------------
                uri = BundleUtil.UpdateDataPath + "shared.assetbundle";
                Debug.LogWarning("LoadFile::>> " + uri);

                stream = File.ReadAllBytes(uri);
                shared = AssetBundle.LoadFromMemory(stream);
#if UNITY_5
        shared.LoadAsset("Dialog", typeof(GameObject));
#else
                shared.Load("Dialog", typeof(GameObject));
#endif
            }
            if (func != null) func();    //��Դ��ʼ����ɣ��ص���Ϸ��������ִ�к������� 
        }


        public byte[] LoadLuaBytes(string luaName) {
            string filePath;
            if (string.IsNullOrEmpty(Path.GetExtension(luaName))) {
                filePath = string.Format("lua/{0}.lua", luaName);
            } else {
                filePath = string.Format("lua/{0}", luaName);
            }

            filePath = filePath.ToLower();

            if (AppConst.DebugMode) { // debug模式下读项目内 lua
                filePath = Application.dataPath + "/" + filePath;
            } else {
                filePath = BundleUtil.UpdateDataPath + "/" + filePath;
            }


            byte[] bytes = null;
            if (File.Exists(filePath)) {
                bytes = File.ReadAllBytes(filePath);
            }

            return bytes;
        } 


        /// <summary>
        /// �����ز�
        /// </summary>
        public AssetBundle LoadBundle(string name) {
            byte[] stream = null;
            AssetBundle bundle = null;
            string uri = BundleUtil.UpdateDataPath + name.ToLower() + ".assetbundle";
            stream = File.ReadAllBytes(uri);
            bundle = AssetBundle.LoadFromMemory(stream); //�������ݵ��زİ�
            return bundle;
        }

        /// <summary>
        /// ������Դ
        /// </summary>
        void OnDestroy() {
            if (shared != null) shared.Unload(true);
            Debug.Log("~ResourceManager was destroy!");
        }
    }
}
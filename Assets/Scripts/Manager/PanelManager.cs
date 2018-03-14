using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LuaInterface;

using SimpleFramework.Utils;

namespace SimpleFramework.Manager {
    public class PanelManager : MonoBehaviour {
        private Transform parent;

        Transform Parent {
            get {
                if (parent == null) {
                    GameObject go = GameObject.FindWithTag("GuiCamera");
                    if (go != null) parent = go.transform;
                }
                return parent;
            }
        }


        public void CreatePanel(string name, LuaFunction func = null) {
            AssetBundle bundle = ioo.ResourceManager.LoadBundle(name);
            StartCoroutine(StartCreatePanel(name, bundle, func));
            Debug.LogWarning("CreatePanel::>> " + name + " " + bundle);
        }


        IEnumerator StartCreatePanel(string name, AssetBundle bundle, LuaFunction func = null) {
            name += "Panel";
            GameObject prefab = Util.LoadAsset(bundle, name);
            yield return new WaitForEndOfFrame();
            if (Parent.Find(name) != null || prefab == null) {
                yield break;
            }
            GameObject go = Instantiate(prefab) as GameObject;
            go.name = name;
            go.layer = LayerMask.NameToLayer("Default");
            go.transform.parent = Parent;
            go.transform.localScale = Vector3.one;
            go.transform.localPosition = Vector3.zero;

            yield return new WaitForEndOfFrame();
            go.AddComponent<LuaBehaviour>().OnInit(bundle);

            if (func != null) func.Call(go);
            Debug.Log("StartCreatePanel------>>>>" + name);
        }
    }
}
// using UnityEngine;
// using System;
// using System.Collections;
// using System.Collections.Generic;
// using SimpleFramework;
// using SimpleFramework.Manager;

// public class View : MonoBehaviour {
//     // private AppFacade m_Facade;
    
//     private ResourceManager m_ResMgr;
//     private NetworkManager m_NetMgr;
//     private MusicManager m_MusicMgr;
//     private TimerManager m_TimerMgr;
//     private ThreadManager m_ThreadMgr;

//     // public virtual void OnMessage(IMessage message) {
//     // }

//     // /// <summary>
//     // /// 注册消息
//     // /// </summary>
//     // /// <param name="view"></param>
//     // /// <param name="messages"></param>
//     // protected void RegisterMessage(IView view, List<string> messages) {
//     //     if (messages == null || messages.Count == 0) return;
//     //     Controller.Instance.RegisterViewCommand(view, messages.ToArray());
//     // }

//     // /// <summary>
//     // /// 移除消息
//     // /// </summary>
//     // /// <param name="view"></param>
//     // /// <param name="messages"></param>
//     // protected void RemoveMessage(IView view, List<string> messages) {
//     //     if (messages == null || messages.Count == 0) return;
//     //     Controller.Instance.RemoveViewCommand(view, messages.ToArray());
//     // }

//     // protected AppFacade facade {
//     //     get {
//     //         if (m_Facade == null) {
//     //             m_Facade = AppFacade.Instance;
//     //         }
//     //         return m_Facade;
//     //     }
//     // }
//     private static GameObject m_manager = null;
//     public static GameObject manager {
//         get { 
//             if (m_manager == null)
//                 m_manager = GameObject.Find("GameManager");
//             return m_manager;
//         }
//     }

//     private static GameManager m_gameManager = null;
//     public static GameManager gameManager {
//         get {
//             if (m_gameManager == null && manager != null)
//                 m_gameManager = manager.GetComponent<GameManager> ();
//             return m_gameManager;
//         }
//     }

//     public static LuaScriptMgr LuaManager {
//         get {
//             if (gameManager == null )
//                 return null;
//             return gameManager.LuaManager;
//         }
//     }

//     protected ResourceManager ResManager {
//         get {
//             if (m_ResMgr == null) {
//                 m_ResMgr = manager.GetComponent<ResourceManager>();
//             }
//             return m_ResMgr;
//         }
//     }

//     protected NetworkManager NetManager {
//         get {
//             if (m_NetMgr == null) {
//                 m_NetMgr = manager.GetComponent<NetworkManager>();
//             }
//             return m_NetMgr;
//         }
//     }

//     protected MusicManager MusicManager {
//         get {
//             if (m_MusicMgr == null) {
//                 m_MusicMgr = manager.GetComponent<MusicManager>();
//             }
//             return m_MusicMgr;
//         }
//     }

//     protected TimerManager TimerManger {
//         get {
//             if (m_TimerMgr == null) {
//                 m_TimerMgr = manager.GetComponent<TimerManager>();
//             }
//             return m_TimerMgr;
//         }
//     }

//     protected ThreadManager ThreadManager {
//         get {
//             if (m_ThreadMgr == null) {
//                 m_ThreadMgr = manager.GetComponent<ThreadManager>();
//             }
//             return m_ThreadMgr;
//         }
//     }
// }

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using SimpleFramework;
using SimpleFramework.Manager;

namespace SimpleFramework.Utils {
    public class ioo {
        
        private static GameObject m_manager = null;
        public static GameObject manager {
            get { 
                if (m_manager == null)
                    m_manager = GameObject.Find("GameManager");
                return m_manager;
            }
        }

        private static GameManager m_gameManager = null;
        public static GameManager GameManager {
            get {
                if (m_gameManager == null && manager != null)
                    m_gameManager = manager.GetComponent<GameManager> ();
                return m_gameManager;
            }
        }

        public static LuaScriptMgr LuaManager {
            get {
                if (GameManager == null )
                    return null;
                return GameManager.LuaManager;
            }
        }

        private static ResourceManager m_ResMgr;
        public static ResourceManager ResourceManager {
            get {
                if (m_ResMgr == null) {
                    m_ResMgr = manager.GetComponent<ResourceManager>();
                }
                return m_ResMgr;
            }
        }

        private static NetworkManager m_NetMgr;
        public static NetworkManager NetworkManager {
            get {
                if (m_NetMgr == null) {
                    m_NetMgr = manager.GetComponent<NetworkManager>();
                }
                return m_NetMgr;
            }
        }

        private static MusicManager m_MusicMgr;
        public static MusicManager MusicManager {
            get {
                if (m_MusicMgr == null) {
                    m_MusicMgr = manager.GetComponent<MusicManager>();
                }
                return m_MusicMgr;
            }
        }

        private static TimerManager m_TimerMgr;
        public static TimerManager TimerManger {
            get {
                if (m_TimerMgr == null) {
                    m_TimerMgr = manager.GetComponent<TimerManager>();
                }
                return m_TimerMgr;
            }
        }

        private static ThreadManager m_ThreadMgr;
        public static ThreadManager ThreadManager {
            get {
                if (m_ThreadMgr == null) {
                    m_ThreadMgr = manager.GetComponent<ThreadManager>();
                }
                return m_ThreadMgr;
            }
        }
    }
}
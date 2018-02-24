using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using SimpleFramework;
using SimpleFramework.Manager;

namespace SimpleFramework.Utils {
    public class ioo {
        
        private static GameObject m_Manager = null;
        public static GameObject Manager {
            get { 
                if (m_Manager == null)
                    m_Manager = GameObject.Find("GameManager");
                return m_Manager;
            }
        }

        private static GameManager m_GameManager = null;
        public static GameManager GameManager {
            get {
                if (m_GameManager == null && Manager != null)
                    m_GameManager = Manager.GetComponent<GameManager> ();
                return m_GameManager;
            }
        }

        public static LuaScriptMgr LuaManager {
            get {
                if (GameManager == null )
                    return null;
                return GameManager.LuaManager;
            }
        }

        private static UpdateManager m_UpdateManager;
        public static UpdateManager UpdateManager {
            get {
                if (m_UpdateManager == null) {
                    m_UpdateManager = Manager.GetComponent<UpdateManager>();
                }
                return m_UpdateManager;
            }
        }

        private static ResourceManager m_ResourceManager;
        public static ResourceManager ResourceManager {
            get {
                if (m_ResourceManager == null) {
                    m_ResourceManager = Manager.GetComponent<ResourceManager>();
                }
                return m_ResourceManager;
            }
        }

        private static NetworkManager m_NetworkManager;
        public static NetworkManager NetworkManager {
            get {
                if (m_NetworkManager == null) {
                    m_NetworkManager = Manager.GetComponent<NetworkManager>();
                }
                return m_NetworkManager;
            }
        }

        private static MusicManager m_MusicManager;
        public static MusicManager MusicManager {
            get {
                if (m_MusicManager == null) {
                    m_MusicManager = Manager.GetComponent<MusicManager>();
                }
                return m_MusicManager;
            }
        }

        private static TimerManager m_TimerManager;
        public static TimerManager TimerManager {
            get {
                if (m_TimerManager == null) {
                    m_TimerManager = Manager.GetComponent<TimerManager>();
                }
                return m_TimerManager;
            }
        }

        private static ThreadManager m_ThreadManager;
        public static ThreadManager ThreadManager {
            get {
                if (m_ThreadManager == null) {
                    m_ThreadManager = Manager.GetComponent<ThreadManager>();
                }
                return m_ThreadManager;
            }
        }

        private static PanelManager m_PanelManager;
        public static PanelManager PanelManager {
            get {
                if (m_PanelManager == null) {
                    m_PanelManager = Manager.GetComponent<PanelManager>();
                }
                return m_PanelManager;
            }
        }
    }
}
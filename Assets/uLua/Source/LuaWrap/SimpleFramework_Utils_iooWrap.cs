using System;
using LuaInterface;

public class SimpleFramework_Utils_iooWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("New", _CreateSimpleFramework_Utils_ioo),
			new LuaMethod("GetClassType", GetClassType),
		};

		LuaField[] fields = new LuaField[]
		{
			new LuaField("Manager", get_Manager, null),
			new LuaField("GameManager", get_GameManager, null),
			new LuaField("LuaManager", get_LuaManager, null),
			new LuaField("ResourceManager", get_ResourceManager, null),
			new LuaField("NetworkManager", get_NetworkManager, null),
			new LuaField("MusicManager", get_MusicManager, null),
			new LuaField("TimerManager", get_TimerManager, null),
			new LuaField("ThreadManager", get_ThreadManager, null),
			new LuaField("PanelManager", get_PanelManager, null),
		};

		LuaScriptMgr.RegisterLib(L, "SimpleFramework.Utils.ioo", typeof(SimpleFramework.Utils.ioo), regs, fields, typeof(object));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateSimpleFramework_Utils_ioo(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 0)
		{
			SimpleFramework.Utils.ioo obj = new SimpleFramework.Utils.ioo();
			LuaScriptMgr.PushObject(L, obj);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: SimpleFramework.Utils.ioo.New");
		}

		return 0;
	}

	static Type classType = typeof(SimpleFramework.Utils.ioo);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Manager(IntPtr L)
	{
		LuaScriptMgr.Push(L, SimpleFramework.Utils.ioo.Manager);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_GameManager(IntPtr L)
	{
		LuaScriptMgr.Push(L, SimpleFramework.Utils.ioo.GameManager);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_LuaManager(IntPtr L)
	{
		LuaScriptMgr.PushObject(L, SimpleFramework.Utils.ioo.LuaManager);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_ResourceManager(IntPtr L)
	{
		LuaScriptMgr.Push(L, SimpleFramework.Utils.ioo.ResourceManager);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_NetworkManager(IntPtr L)
	{
		LuaScriptMgr.Push(L, SimpleFramework.Utils.ioo.NetworkManager);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_MusicManager(IntPtr L)
	{
		LuaScriptMgr.Push(L, SimpleFramework.Utils.ioo.MusicManager);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_TimerManager(IntPtr L)
	{
		LuaScriptMgr.Push(L, SimpleFramework.Utils.ioo.TimerManager);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_ThreadManager(IntPtr L)
	{
		LuaScriptMgr.Push(L, SimpleFramework.Utils.ioo.ThreadManager);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_PanelManager(IntPtr L)
	{
		LuaScriptMgr.Push(L, SimpleFramework.Utils.ioo.PanelManager);
		return 1;
	}
}


using System;
using LuaInterface;

public class SimpleFramework_Utils_BundleUtilWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("New", _CreateSimpleFramework_Utils_BundleUtil),
			new LuaMethod("GetClassType", GetClassType),
		};

		LuaField[] fields = new LuaField[]
		{
			new LuaField("FileName", get_FileName, set_FileName),
			new LuaField("PersistentDataPath", get_PersistentDataPath, null),
			new LuaField("UpdateDataPath", get_UpdateDataPath, null),
			new LuaField("UpdateCachePath", get_UpdateCachePath, null),
			new LuaField("StreamingDataPath", get_StreamingDataPath, null),
		};

		LuaScriptMgr.RegisterLib(L, "SimpleFramework.Utils.BundleUtil", typeof(SimpleFramework.Utils.BundleUtil), regs, fields, typeof(object));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateSimpleFramework_Utils_BundleUtil(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 0)
		{
			SimpleFramework.Utils.BundleUtil obj = new SimpleFramework.Utils.BundleUtil();
			LuaScriptMgr.PushObject(L, obj);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: SimpleFramework.Utils.BundleUtil.New");
		}

		return 0;
	}

	static Type classType = typeof(SimpleFramework.Utils.BundleUtil);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_FileName(IntPtr L)
	{
		LuaScriptMgr.Push(L, SimpleFramework.Utils.BundleUtil.FileName);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_PersistentDataPath(IntPtr L)
	{
		LuaScriptMgr.Push(L, SimpleFramework.Utils.BundleUtil.PersistentDataPath);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_UpdateDataPath(IntPtr L)
	{
		LuaScriptMgr.Push(L, SimpleFramework.Utils.BundleUtil.UpdateDataPath);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_UpdateCachePath(IntPtr L)
	{
		LuaScriptMgr.Push(L, SimpleFramework.Utils.BundleUtil.UpdateCachePath);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_StreamingDataPath(IntPtr L)
	{
		LuaScriptMgr.Push(L, SimpleFramework.Utils.BundleUtil.StreamingDataPath);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_FileName(IntPtr L)
	{
		SimpleFramework.Utils.BundleUtil.FileName = LuaScriptMgr.GetString(L, 3);
		return 0;
	}
}



CtrlName = {
	Prompt = "PromptCtrl",
	Message = "MessageCtrl"
}

--协议类型--
ProtocalType = {
	BINARY = 0,
	PB_LUA = 1,
	PBC = 2,
	SPROTO = 3,
}
--当前使用的协议类型--
TestProtoType = ProtocalType.BINARY

Util = SimpleFramework.Utils.Util
AppConst = SimpleFramework.AppConst
LuaHelper = SimpleFramework.LuaHelper
ByteBuffer = SimpleFramework.ByteBuffer

ioo = SimpleFramework.Utils.ioo
ResManager = ioo.ResourceManager
NetManager = ioo.NetworkManager
PanelManager = ioo.PanelManager
MusicManager = ioo.MusicManager
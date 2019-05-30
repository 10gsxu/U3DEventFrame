
CtrlNames = {
	Prompt = "PromptCtrl",
	Message = "MessageCtrl"
}

PanelNames = {
	"PromptPanel",	
	"MessagePanel",
}

--协议类型--
ProtocalType = {
	BINARY = 0,
	PB_LUA = 1,
	PBC = 2,
	SPROTO = 3,
}
--当前使用的协议类型--
TestProtoType = ProtocalType.BINARY;

Util = LuaFramework.Util;
AppConst = LuaFramework.AppConst;
LuaHelper = LuaFramework.LuaHelper;
ByteBuffer = LuaFramework.ByteBuffer;

resMgr = LuaHelper.GetResManager();
panelMgr = LuaHelper.GetPanelManager();
soundMgr = LuaHelper.GetSoundManager();
networkMgr = LuaHelper.GetNetManager();

WWW = UnityEngine.WWW;
GameObject = UnityEngine.GameObject;

MsgSpan = 3000;
LManagerID = {
	LuaManager = 0,
	LNetManager = MsgSpan * 1,
	LUIManager = MsgSpan * 2,
	LNPCManager = MsgSpan * 3,
	LCharactorManager = MsgSpan * 4,
	LAssetManager = MsgSpan * 5,
	LDataManager = MsgSpan * 6,
	LAudioManager = MsgSpan * 7
}
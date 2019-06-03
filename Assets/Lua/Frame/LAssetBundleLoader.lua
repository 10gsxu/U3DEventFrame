LAssetBundleLoader = {

}
LAssetBundleLoader.__index = LAssetBundleLoader;
local this = LAssetBundleLoader;

function LAssetBundleLoader:New()
    local self = {};
    setmetatable(self, LAssetBundleLoader);

    return self;
end

function LAssetBundleLoader.Awake()
    this.msgId[1] = LAssetEvent.HunkRes;
    this.msgId[2] = LAssetEvent.ReleaseSingleObj;
    this.msgId[3] = LAssetEvent.MaxValue;

    this.RegistSelf(this, self.msgId);
end

function LAssetBundleLoader:SendMsg()

end

function LAssetBundleLoader:ProcessEvent(msg)
    if msg.msgId == LAssetEvent.HunkRes then
        LuaLoadRes.Instance:GetResources(msg.sceneName, msg.bundleName, msg.resName, msg.isSingle, msg.callBackFunc);
    elseif msg.msgId == LAssetEvent.ReleaseSingleObj then
        LuaLoadRes.Instance:UnLoadResObj(msg.sceneName, msg.bundleName, msg.resName);
    end
end

function LAssetBundleLoader.Destroy()
    this.UnRegistSelf(this, self.msgId);
end
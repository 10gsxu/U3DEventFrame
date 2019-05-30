LAssetMsg = LMsgBase:New();

LAssetMsg.__index = LAssetMsg;

function LAssetMsg:New(msgid, sceneName, bundleName, resName, single, backFunc)
    local self = {};
    setmetatable(self, LAssetMsg);

    self.msgid = msgid;
    self.sceneName = sceneName;
    self.bundleName = bundleName;
    self.resName = resName;
    self.isSingle = single;
    self.callBackFunc = backFunc;

    return self;
end
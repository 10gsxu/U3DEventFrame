print("LMsgCenter")
LMsgCenter = {}
LMsgCenter.__index = LMsgCenter;
local this = LMsgCenter;

function LMsgCenter:New(msgid)
    local self = {};
    setmetatable(self, LMsgCenter);
    return self;
end

function LMsgCenter:Awake()
    LuaAndCMsgCenter.instance:SettingLuaCallBack(this.RecvMsg);
end

function LMsgCenter:GetInstance()
    return this;
end

function LMsgCenter:SendToMsg(msg)
    managerId = msg:GetManager();
end

function LMsgCenter:RecvMsg(arg1, arg2, arg3, arg4)

end
LMsgCenter = {
    managerDict = {}
}
LMsgCenter.__index = LMsgCenter;
local this = LMsgCenter;

function LMsgCenter:New(msgid)
    local self = {};
    setmetatable(self, LMsgCenter);
    return self;
end

function LMsgCenter:Awake()
    LuaEventProcess.Instance:SettingLuaCallBack(this.RecvMsg);
    this.managerDict[LManagerID.LUIManager] = LUIManager;
    --this.managerDict[LManagerID.LNetManager] = LNetManager;
    --this.managerDict[LManagerID.LAudioManager] = LAudioManager;
end

function LMsgCenter:GetInstance()
    return this;
end

function LMsgCenter:SendToMsg(msg)
    managerId = msg:GetManager();
end

function LMsgCenter:RecvMsg(fromNet, arg0, arg1, arg2)
    if fromNet == true then
        local tmpMsg = LMsgBase:New(arg0);
        tmpMsg.state = arg1;
        tmpMsg.data = arg2;
        this.ProcessEvent(tmpMsg);
    else
        this.ProcessEvent(arg0);
    end
end

function LMsgCenter:ProcessEvent(msg)
    managerId = msg:GetManager();
    this.managerDict[managerId].GetInstance().ProcessEvent(msg);
end
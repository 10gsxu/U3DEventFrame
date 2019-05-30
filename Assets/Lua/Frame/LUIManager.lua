﻿print("LUIManager")

LUIManager = LManagerBase:New()
LUIManager.__index = LUIManager;
local this = LUIManager;

function LUIManager:New()
    local self = {};
    setmetatable(self, LUIManager);
    return self;
end

function LUIManager:Awake()
end

function LUIManager:GetInstance()
    return this;
end

function LUIManager:SendMsg(msg)
    if msg:GetManager() == LManagerID.LUIManager then
        self:ProcessEvent(msg);
    else
        LMsgCenter.SendToMsg(msg);
    end
end
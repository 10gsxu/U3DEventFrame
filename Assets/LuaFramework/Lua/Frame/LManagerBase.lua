
LManagerBase = {
    eventTree = {}
};
local this = LManagerBase;

function LManagerBase:RegistMsgs(script, msgs)
    for i, v in pairs(msgs) do
        eventNode = LEventNode:New(script);
        self:RegistMsg(v, eventNode);
    end
end

function LManagerBase:RegistMsg(id, eventNode)
end

function LManagerBase:UnRegistMsgs(script, ...)
    if arg == nil then
        return
    end
    for i in arg do
        self:UnRegistMsg(script, i);
    end
end

function LManagerBase:UnRegistMsg(script, id)
    if this:FindKey(self.eventTree, id) then
        tmpNode = self.eventTree[id];
        if tmpNode.data == script then
            if tmpNode.next ~= nil then
                self.eventTree[id] = tmpNode.next;
                tmpNode.next = nil;
            end
        else
            while tmpNode.next ~= nil and tmpNode.next.data ~= script do
                tmpNode = tmpNode.next;
            end
            if tmpNode.next.next ~= nil then
                curNode = tmpNode.next;
                tmpNode.next = curNode.next;
                curNode.next = nil;
            else
                tmpNode.next = nil;
            end
        end
    end
end

function LManagerBase:ProcessEvent (msg)
    if this:FindKey(self.eventTree, msg.msgId) then
        local tmpNode = self.eventTree[msg.msgId];
        while tmpNode ~= nil do
            tmpNode.value:ProcessEvent(msg);
            if tmpNode.next ~= nil then
                tmpNode = tmpNode.next;
            end
        end
    else
        print("Msg not contain msg ==="..msg.msgId);
    end
end

function LManagerBase:FindKey (eventTree, id)

end

function LManagerBase:Destroy()
    keys= {}
    keyCount = 0;
    for k, v in pairs(self.eventTree) do
        keys[keyCount] = k;
        keyCount = keyCount + 1;
    end
    for i=1, keyCount do
        self.eventTree[keys[i]] = nil;
    end
end
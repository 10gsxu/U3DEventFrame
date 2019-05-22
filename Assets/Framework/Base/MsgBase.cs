using System.Collections;
using System.Collections.Generic;

namespace U3DEventFrame {
	public class MsgBase {
		//表示65535个消息，占两个字节，int占4个字节
		public ushort msgId;

		public ManagerID GetManager() {
			int tmpId = msgId / FrameTools.MsgSpan;
			return (ManagerID)(tmpId * FrameTools.MsgSpan);
		}

		public MsgBase(ushort tmpMsg) {
			msgId = tmpMsg;
		}
	}
}
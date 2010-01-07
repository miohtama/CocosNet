using System;
using CocosNet.Base;

namespace CocosNet.Actions {
	public delegate void CallFuncNoArgs();
	public delegate void CallFuncSender(CocosNode sender);
	public delegate void CallFuncSenderData(CocosNode sender, object data);

	public class CallFunc : InstantAction {
		private CallFuncNoArgs _method;

		public CallFunc(CallFuncNoArgs method) {
			if (method == null) {
				throw new ArgumentNullException("method");
			}
			
			_method = method;
		}

		public override Action Clone() {
			return new CallFunc(_method);
		}

		public override void Start() {
			_method();
		}
	}

	public class CallFuncN : InstantAction {
		private CallFuncSender _method;

		public CallFuncN(CallFuncSender method) {
			if (method == null) {
				throw new ArgumentNullException("method");
			}
			
			_method = method;
		}

		public override Action Clone() {
			return new CallFuncN(_method);
		}

		public override void Start() {
			_method(Target);
		}
	}

	public class CallFuncND : InstantAction {
		private CallFuncSenderData _method;
		private object _data;

		public CallFuncND(CallFuncSenderData method, object data) {
			if (method == null) {
				throw new ArgumentNullException("method");
			}
			
			_method = method;
			_data = data;
		}

		public override Action Clone() {
			return new CallFuncND(_method, _data);
		}

		public override void Start() {
			;
			_method(Target, _data);
		}
	}
}

using System;

namespace Bang.Process {
	public class NetResult {
		public Int32 ExitCode { get; private set; }
		public String Message { get; private set; }
		public String Error { get; private set; }
		public static NetResult Empty = new NetResult(0, String.Empty, String.Empty);

		public NetResult(Int32 exitCode, String message, String error) {
			ExitCode = exitCode;
			Message = message;
			Error = error;
		}
	}
}
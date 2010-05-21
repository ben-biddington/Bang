using System;
using Bang.IO.Pipes;
using Bang.Process;

namespace Bang.Integration.Tests.Util {
	public class RmtShare {
		private readonly string _executable;

		public RmtShare(String executable) {
			_executable = executable;
		}

		public NetResult New(String server, String share, String path) {
			var remark = String.Format("Shared folder: {0}", path);

			var arguments = String.Format(
				@"\\{0}\{1}={2} /GRANT everyone:f" + String.Format("/REMARK:\"{0}\"", remark), 
				server, 
				share, 
				path,
				remark
			);

			var pipe = new SimplePipe();

			using (var process = ProcessFactory.New(_executable, arguments)) {
				process.Start();

				pipe.ConnectTo(process);

				Lang.Wait.On(process);

				return new NetResult(process.ExitCode, pipe.StdOut(), pipe.StdErr());
			}
		}
	}
}
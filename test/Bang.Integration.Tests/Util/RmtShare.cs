using System;
using Bang.IO.Pipes;
using Bang.Process;

namespace Bang.Integration.Tests.Util {
	// @see: http://support.microsoft.com/kb/155449
	public class RmtShare {
		private readonly string _executable;

		public RmtShare(String executable) {
			_executable = executable;
		}

		public NetResult New(String server, String share, String path) {
			var remark = String.Format("Shared folder: {0}", path);

			// Assigning permissions does not seem to work in sever 2003 -- may be supported by net.exe
			// // @see:http://www.windowsitpro.com/article/tips/the-net-share-command-in-windows-server-2003-adds-the-grant-of-share-permissions-.aspx

			var arguments = String.Format(
				@"\\{0}\{1}={2} " + String.Format("/REMARK:\"{0}\"", remark), 
				server, 
				share, 
				path,
				remark
			);

			var pipe = new SimplePipe();

			using (var process = ProcessFactory.New("net", "share " + arguments)) {
				process.Start();

				pipe.ConnectTo(process);

				Lang.Wait.On(process);

				return new NetResult(process.ExitCode, pipe.StdOut(), pipe.StdErr());
			}
		}
	}
}
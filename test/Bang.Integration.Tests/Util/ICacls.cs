using System;
using System.IO;
using System.Net;
using Bang.IO.Pipes;
using Bang.Lang;
using Bang.Process;

namespace Bang.Integration.Tests.Util {
	public class ICacls {
		public static NetResult Grant(DirectoryInfo dir, NetworkCredential user, String permission) {
			return Go("grant", dir, user, permission);
		}

		public static NetResult Remove(DirectoryInfo dir, NetworkCredential user, String permission) {
			return Go("remove", dir, user, permission);
		}

		public static NetResult Reset(DirectoryInfo dir) {
			return Run(
				"/reset {0}",
				dir.FullName
			);
		}

		private static NetResult Go(
			String operation, 
			FileSystemInfo dir, 
			NetworkCredential user, 
			String permission
		) {
			return Run(
				"{0} /{1} {2}\\{3}:{4}", 
				operation, 
				dir.FullName, 
				user.Domain, 
				user.UserName, 
				permission
			);
		}

		private static NetResult Run(String arguments, params Object[] args) {
			var theArgs = String.Format(arguments, args);

			var pipe = new SimplePipe();

			using (var process = ProcessFactory.New("icacls.exe", theArgs)) {
				process.Start();

				pipe.ConnectTo(process);

				Wait.On(process, Timeout);

				return new NetResult(
					process.ExitCode,
					pipe.StdOut(),
					pipe.StdErr()
				);
			}
		}

		private static TimeSpan Timeout {
			get {
				return TimeSpan.FromSeconds(30);
			}
		}
	}
}

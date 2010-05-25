using System;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using Bang.IO.Pipes;
using Bang.Lang;
using Bang.Process;

namespace Bang.Core {
	public class Use {
		private static TimeSpan Timeout {
			get {
				return TimeSpan.FromSeconds(30);
			}
		}

		public static NetResult Ensure(String resource, NetworkCredential credential) {
			if (Contains(resource))
				return NetResult.Empty;

			return Go(resource, credential);
		}

		public static NetResult Go(String resource) {
			return Run(
				@"use {0}",
				resource
			);
		}

		public static NetResult Go(String resource, NetworkCredential credential) {
			return Run(
				@"use {0} /USER:{1}\{2} {3}",
				resource,
				credential.Domain,
				credential.UserName,
				credential.Password
			);
		}

		public static NetResult Delete(String resource) {
			// [!] May be interactive if connection is open, i.e., waiting for yes or no input.
			return Run("use {0} /DELETE", resource);
		}

		public static Boolean Contains(String resource) {
			var pattern = String.Format(@"\s+{0}\s+", Regex.Escape(resource.Trim()));

			return Regex.IsMatch(List().Message.Trim(), pattern);
		}

		public static NetResult List() {
			return Run("use");
		}

		public static NetResult Share(String name, DirectoryInfo dir) {
			return Run("share {0}", String.Format("\"{0}\"=\"{1}\"", name, dir.FullName));
		}

		private static NetResult Run(String arguments, params Object[] args) {
			var theArgs = String.Format(arguments, args);
			
			var pipe = new SimplePipe();

			using (var process = ProcessFactory.New("net.exe", theArgs)) {
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
	}
}
using System;
using Bang.IO.Pipes;
using Bang.Lang;
using Bang.Process;

namespace Bang.Core {
	public static class Stat {
		private static readonly Func<String, Boolean> _bypassFilter = line => true;

		public static NetResult Go() {
			return Run(null, Lines.All);
		}

		public static NetResult Go(String arguments) {
			return Go(arguments, Lines.All);
		}

		public static NetResult Go(String arguments, Func<String, Boolean> filter) {
			return Run(arguments, filter);
		}

		private static NetResult Run(String arguments, Func<String, Boolean> filter) {
			var pipe = new FilteredPipe(filter);

			using (var process = ProcessFactory.New("netstat.exe", arguments)) {
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

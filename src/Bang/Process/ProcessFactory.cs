using System;

namespace Bang.Process {
	public static class ProcessFactory {
		public static System.Diagnostics.Process New(String executable, String args) {
			return new System.Diagnostics.Process {
				StartInfo = {
					UseShellExecute = false,
					RedirectStandardInput = false,
					RedirectStandardOutput = true,
					RedirectStandardError = true,
					CreateNoWindow = true,
					FileName = executable,
					Arguments = args
				}
			};
		}
	}
}

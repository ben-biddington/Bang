using System;
using System.Diagnostics;
using System.Text;

namespace Bang.IO.Pipes {
	public abstract class Pipe : IPipeProcessOutput {
		protected StringBuilder _stdOut = new StringBuilder();
		protected StringBuilder _stdErr = new StringBuilder();

		protected abstract void OnOut(Object sender, DataReceivedEventArgs e);
		protected abstract void OnErr(Object sender, DataReceivedEventArgs e);
		
		public String StdOut() {
			return _stdOut.ToString();
		}

		public String StdErr() {
			return _stdErr.ToString();
		}

		public void ConnectTo(System.Diagnostics.Process process) {
			process.BeginOutputReadLine();
			process.BeginErrorReadLine();
			process.OutputDataReceived += OnOut;
			process.ErrorDataReceived += OnErr;
		}
	}
}
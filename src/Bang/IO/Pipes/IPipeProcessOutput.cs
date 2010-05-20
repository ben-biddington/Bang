using System;

namespace Bang.IO.Pipes {
	public interface IPipeProcessOutput {
		String StdOut();
		String StdErr();
		void ConnectTo(System.Diagnostics.Process process);
	}
}
using System.Diagnostics;

namespace Bang.IO.Pipes {
	public class SimplePipe : Pipe {
		protected override void OnOut(object sender, DataReceivedEventArgs e) {
			_stdOut.AppendLine(e.Data);
		}

		protected override void OnErr(object sender, DataReceivedEventArgs e) {
			_stdErr.AppendLine(e.Data);
		}
	}
}
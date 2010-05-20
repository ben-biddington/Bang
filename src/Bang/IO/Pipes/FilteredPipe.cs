using System;
using System.Diagnostics;

namespace Bang.IO.Pipes {
	public class FilteredPipe : SimplePipe {
		private readonly Func<String, Boolean> _filter;

		public FilteredPipe(Func<String, Boolean> filter) {
			_filter = filter;
		}

		protected override void OnOut(object sender, DataReceivedEventArgs e) {
			if (_filter(e.Data)) {
				base.OnOut(sender, e);
			}
		}

		protected override void OnErr(object sender, DataReceivedEventArgs e) {
			if (_filter(e.Data)) {
				base.OnErr(sender, e);
			}
		}
	}
}

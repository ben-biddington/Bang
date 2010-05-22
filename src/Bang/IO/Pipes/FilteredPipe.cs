using System;
using System.Diagnostics;

namespace Bang.IO.Pipes {
        // TODO: refactor filter abstraction. Filter should have a return value -- Some[T] or None[T] perhaps.
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

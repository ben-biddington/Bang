using System;

namespace Bang.Lang {
	public static class Wait {
		public static void On(System.Diagnostics.Process process) {
			On(process, TimeSpan.FromSeconds(17));
		}

		public static void On(System.Diagnostics.Process process, TimeSpan forHowLong) {
			var timeoutInMilliseconds = Convert.ToInt32(forHowLong.TotalMilliseconds);

			process.WaitForExit(timeoutInMilliseconds);

			if (false == process.HasExited) {
				process.Kill();
				process.WaitForExit();

				throw new TimeoutException(
					String.Format(
						"The process did not exit within the allowed period <{0}> " + 
						"and it has been forcibly closed.",
						forHowLong
					)
				);
			}
		}
	}
}
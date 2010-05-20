using System;
using System.Text.RegularExpressions;

namespace Bang.Core {
	public static class Lines {
		public static Func<String, Boolean> All = text => true;

		public static Func<String, Boolean> ThatStart(String with) {
			return Matching(Regex.Escape(with));
		}

		public static Func<String, Boolean> Matching(String pattern) {
			return text => 
				false == String.IsNullOrEmpty(text) && 
				Regex.IsMatch(text, pattern);
		}
	}
}

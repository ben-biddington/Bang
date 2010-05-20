using System;
using System.Net;
using Bang.Process;

namespace Bang {
	public static partial class Net {
		public static NetResult Use(String resource) {
			return Core.Use.Go(resource);
		}

		public static NetResult Use(String resource, NetworkCredential credential) {
			return Core.Use.Go(resource, credential);
		}

		public static NetResult Ensure(String resource, NetworkCredential credential) {
			return Core.Use.Ensure(resource, credential);
		}

		public static NetResult Delete(String resource) {
			return Core.Use.Delete(resource);
		}

		public static Boolean Contains(String resource) {
			return Core.Use.Contains(resource);
		}

		public static NetResult List() {
			return Core.Use.List();
		}
	}

	public static partial class Net {
		public static NetResult Stat() {
			return Core.Stat.Go();
		}

		public static NetResult Stat(String arguments) {
			return Core.Stat.Go(arguments);
		}

		public static NetResult Stat(String arguments, Func<String, Boolean> filter) {
			return Core.Stat.Go(arguments, filter);
		}
	}
}

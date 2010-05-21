using System;
using System.DirectoryServices;
using System.Net;

namespace Bang.Integration.Tests.Util {
	public class WindowsAccount {
		public static DirectoryEntry Ensure(NetworkCredential who, String group) {
			var localComputer = LocalComputer();
			
			if (Exists(who))
				return Get(who, localComputer);

			var newUser = Add(who);
			AddToGroup(newUser, "Guests");

			return newUser;
		}

		private static DirectoryEntry Add(NetworkCredential who) {
			var localComputer = LocalComputer();
			var user = localComputer.Children.Add(who.UserName, "user");

			user.Invoke("SetPassword", who.Password);
			user.Invoke("Put", "Description", "Test User from Bang.Net");
			user.CommitChanges();

			return user;
		}

		private static void AddToGroup(DirectoryEntry user, String group) {
			var localMachine = LocalComputer();

			var guestGroup = localMachine.Children.Find(group, "group");

			if (null == guestGroup)
				throw new InvalidOperationException("The group <" + group + "> could not be found");

			guestGroup.Invoke("Add", user.Path);
		}

		private static DirectoryEntry Get(NetworkCredential who, DirectoryEntry localComputer) {
			return localComputer.Children.Find(who.UserName);
		}

		private static DirectoryEntry LocalComputer() {
			return new DirectoryEntry("WinNT://" + Environment.MachineName + ",computer");
		}

		private static Boolean Exists(NetworkCredential who) {
			return DirectoryEntry.Exists(LocalMachine + "/" + who.UserName);
		}

		private static Boolean IsMemberOf(DirectoryEntry who, String group) {
			string path = LocalMachine + "/" + group+ "/" + "chubbs.peterson";

			return DirectoryEntry.Exists(path);
		}

		private static String LocalMachine {
			get { return "WinNT://" + Environment.MachineName; }
		}
	}
}

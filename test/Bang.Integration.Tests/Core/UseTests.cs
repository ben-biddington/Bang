﻿using System;
using System.Configuration;
using System.IO;
using System.Net;
using Bang.Core;
using Bang.Integration.Tests.Util;
using Bang.Process;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using Patience.Fluent;

namespace Bang.Integration.Tests.Core {
	[TestFixture]
	public class UseTests : IntegrationTest {
		private const Int32 OK = 0;
		private NetResult _result;

		private const String EXAMPLE_SHARE_NAME = "example.bang.share";
		private const String EXAMPLE_USER_NAME = "sir.chubbsalot";

		private TimeSpan GracePeriod {
			get { return TimeSpan.FromSeconds(10);  }
		}

		private NetworkCredential Who {
			get {
				return new NetworkCredential(
					ConfigurationManager.AppSettings["Credential.Username"],
					ConfigurationManager.AppSettings["Credential.Password"], 
					Environment.MachineName
				);
			}
		}
		
		private DirectoryInfo TestSharedDir {
			get {
				return new DirectoryInfo(String.Format(@"C:\temp\testshare\{0}", EXAMPLE_USER_NAME));
			}
		}

		private string Resource {
			get { return String.Format(@"\\{0}\{1}", Environment.MachineName, EXAMPLE_SHARE_NAME); }
		}

		private Boolean Clean { get { return true; } }

		[SetUp]
		public void SetUp() {
			_result = null;
			Given_there_is_test_data_available();
		}

		[TearDown]
		public void TearDown() {
			if (Clean) {
				if (TestSharedDir.Exists) {
					Net.Delete(Resource);

					Wait.Patiently.
						ForUpTo(TimeSpan.FromSeconds(30)).
						PollingEvery(TimeSpan.FromSeconds(1)).
						Until(() 
							=> false == Net.Contains(Resource)
						);

					TestSharedDir.Delete(true);
				}
			}
		}

		[TestFixtureTearDown]
		public void FinalTearDown() {
			if (Clean) {
				Delete(Who);

				Disconnect(Resource);

				Delete(TestSharedDir);

				Unshare(EXAMPLE_SHARE_NAME);
			}
		}

		[Test]
		public void Can_list_all_connections() {
			When_conections_listed();

			Then_some_text_is_returned();
		}

		[Test]
		public void Can_add_new_connection() {
			Given_computer_is_not_connected_to_resource();

			When_connection_added();

			Then_connection_is_added_successfully();
		}

		[Test]
		public void Can_add_a_connection_that_is_already_present_if_you_want_to() {
			Given_computer_is_connected_to_resource();

			When_connection_added();

			Then_process_exits_with_status(OK);

			Then_connection_exists();

			Then_process_is_not_running();
		}

		
		[Test, Ignore("PENDING")]
		public void Can_add_a_connection_that_user_does_not_have_read_permissions_on() {
			Assert.Fail("PENDING: Looking for that access denied message.");
		}

		[Test]
		public void Connecting_with_nonexistent_user_returns_error() {
			Given_computer_is_not_connected_to_resource();

			When_connection_added_as(new NetworkCredential("xxx", "xxx", Environment.MachineName));

			Then_process_exits_with_status(2);

			var expectedError = String.Format(
				"System error 1326 has occurred.{0}{0}"+
				"Logon failure: unknown user name or bad password.", 
				Environment.NewLine
			);

			Then_message(Is.Empty);

			Then_error(Is.EqualTo(expectedError));

			Then_connection_does_not_exist();
		}

		[Test]
		public void Adding_a_connection_that_is_not_a_network_resource_returns_error() {
			_result = Use.Go("C:\\Temp", Who);

			var theExpectedError = String.Format(
				"System error 67 has occurred.{0}{0}" +
				"The network name cannot be found.", 
				Environment.NewLine
			);

			Then_error(Is.EqualTo(theExpectedError));
			Then_process_exits_with_status(2);
			Then_process_is_not_running();
		}

		[Test]
		public void Ensure_does_not_add_an_entry_if_it_exists() {
			Given_computer_is_connected_to_resource();

			When_connection_is_ensured();

			Then_process_exits_with_status(OK);

			Then_connection_exists();
		}

		[Test]
		public void Ensure_adds_an_entry_if_it_does_not_exist() {
			Given_computer_is_not_connected_to_resource();

			When_connection_is_ensured();

			Then_process_exits_with_status(OK);

			Then_connection_exists();
		}

		[Test]
		public void Can_delete_connection() {
			Given_computer_is_connected_to_resource();

			When_connection_deleted();

			Then_process_exits_with_status(OK);

			Then_message(Is.EqualTo(String.Format("{0} was deleted successfully.", Resource)));

			Then_connection_does_not_exist();
		}

		[Test]
		public void Deleting_connection_that_does_not_exist_returns_error() {
			Given_computer_is_not_connected_to_resource();

			When_connection_deleted();

			var theExpectedError = String.Format(
				"The network connection could not be found.{0}{0}" + 
				"More help is available by typing NET HELPMSG 2250.", 
				Environment.NewLine
			);

			Then_process_exits_with_status(2);
			Then_process_is_not_running();
			Then_error(Is.EqualTo(theExpectedError));
		}

		private void Given_there_is_test_data_available() {
			Given_the_example_user_exists();
			Given_the_example_share_is_available();
		}

		private void Given_the_example_user_exists() {
			WindowsAccount.Ensure(Who);
		}

		private void Given_the_example_share_is_available() {
			if (false == Net.Contains(Resource)) {
				if (false == TestSharedDir.Exists) {
					TestSharedDir.Create();
				}

				// TODO: Add Net.Share.List so we only do this once.
				Net.Share(EXAMPLE_SHARE_NAME, TestSharedDir);

				ICacls.Grant(TestSharedDir, Who, "F");
			}
		}

		private void Given_computer_is_connected_to_resource() {
			Use.Ensure(Resource, Who);
		}

		private void Given_computer_is_not_connected_to_resource() {
			if (Use.Contains(Resource)) {
				Use.Delete(Resource);
			}
		}

		private void When_conections_listed() {
			_result = Net.List();
		}

		private void When_connection_added() {
			When_connection_added_as(Who);
		}

		private void When_connection_added_as(NetworkCredential who) {
			_result = Net.Use(Resource, who);
		}

		private void When_connection_deleted() {
			_result = Net.Delete(Resource);
		}

		private void When_connection_is_ensured() {
			_result = Net.Ensure(Resource, Who);
		}

		private void Then_connection_is_added_successfully() {
			Then_process_exits_with_status(OK);
			
			Then_message(Is.EqualTo("The command completed successfully."));

			Then_connection_exists();
		}

		private void Then_process_exits_with_status(Int32 exitCode) {
			Then_process_is_not_running();
			Assert.That(
				_result.ExitCode, Is.EqualTo(exitCode),
				"Expected the process to have exited with code <{0}>, but " + 
				"it was <{1}>.\r\n" + 
				"Message: {2}\r\n" + 
				"Error: {3}",
				exitCode, 
				_result.ExitCode, 
				_result.Message,
				_result.Error
			);
		}

		private void Then_connection_exists() {
			Assert.That(Use.Contains(Resource), Is.True);
			Assert.That(Directory.Exists(Resource), Is.True,
				"The directory <{0}> does not exist.", Resource
			);
		}

		private void Then_connection_does_not_exist() {
			Assert.That(Use.Contains(Resource), Is.False);
		}

		private void Then_some_text_is_returned() {
			Assert.That(_result.Message, Is.Not.EqualTo(String.Empty));
		}

		private void Then_process_is_not_running() {
			var theProcessName = "net";

			Wait.Patiently.ForUpTo(GracePeriod).
				IfTimesOutThen(timedOut => 
					Assert.Fail(
						"Timed out while waiting for the process <{0}> to exit", 
						theProcessName
					)
				).
				Until(There_are_no_processes_running);
		}

		private void Then_message(IResolveConstraint matchesThis) {
			Assert.That(_result.Message.Trim(), matchesThis);
		}

		private void Then_error(IResolveConstraint matchesThis) {
			Assert.That(_result.Error.Trim(), matchesThis);
		}

		private Boolean There_are_no_processes_running() {
			return System.Diagnostics.Process.GetProcessesByName("net").Length == 0;
		}

		private void Unshare(String shareName) {
			Net.UnShare(shareName);
		}

		private void Delete(DirectoryInfo what) {
			if (what.Exists) {
				Console.WriteLine("Deleting temp directory: {0}", what.FullName);
				what.Delete(true);
			}
		}

		private void Disconnect(String resource) {
			if (Net.Contains(resource)) {
				Console.WriteLine("Disconnecting: {0}", resource);
				Net.Delete(resource);
			}
		}

		private void Delete(NetworkCredential who) {
			Console.WriteLine("Deleting user: {0}\\{1}", who.Domain, who.UserName);
			WindowsAccount.Delete(who);
		}
	}
}

using System;
using Bang.Core;
using NUnit.Framework;

namespace Bang.Integration.Tests.Core {
	[TestFixture]
	public class StatTests : IntegrationTest {
		[Test] 
		public void Can_list_current_connections_with_numeric_addresses() {
			var result = Net.Stat("-n");

			Assert.That(result.Message.Trim(), Is.Not.Empty);
		}

		[Test]
		public void Can_filter_message_lines() {
			var result = Net.Stat("-n", Lines.Matching("State"));

			Assert.That(ToLines(result.Message).Length, Is.EqualTo(1));
		}

		[Test]
		public void Can_filter_message_lines_with_regex_pattern_if_you_like() {
			var result = Net.Stat("-n", Lines.Matching("^Active Connections"));

			Assert.That(ToLines(result.Message).Length, Is.EqualTo(1));
		}

		[Test]
		public void Can_filter_message_lines_starting_with_a_prefix() {
			var result = Net.Stat("-n", Lines.ThatStart("Active Connections"));

			Assert.That(ToLines(result.Message).Length, Is.EqualTo(1));
		}

		private String[] ToLines(String what) {
			return what.Split(
				Environment.NewLine.ToCharArray(), 
				StringSplitOptions.RemoveEmptyEntries
			);
		}
	}
}

Examples - Net.Stat
========

How to net stat
---------------

	Net.Stat();
	Net.Stat("-n");
	
How to net stat and filter lines
--------------------------------

    Net.Stat("-n", Lines.Matching("^Active Connections"));

[1]: https://github.com/ben-biddington/Bang/blob/master/src/Bang.Integration.Tests/Core/StatTests.cs "netstat examples"
[2]: https://github.com/ben-biddington/Bang/blob/master/src/Bang.Integration.Tests/Core/UseTests.cs "net use examples"
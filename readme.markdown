Examples
--------

### How to net stat

	Net.Stat();
	Net.Stat("-n");
	
### Net stat and filter lines

    Net.Stat("-n", Lines.Matching("^Active Connections"));

See: [the executable examples netstat][1]	

### List all shares

    Net.List();
	
### Add connection to share, connecting as particular user

    var who	= new NetworkCredential("ghay", "password");
	var shareName = "\\graeme\shared\pictures";
	
    Net.Use(shareName, who);
	
### Add connection to share only if it does not already exist

    var who	= new NetworkCredential("ghay", "password");
	var shareName = "\\graeme\shared\pictures";
	
    Net.Ensure(shareName, who);
	
[1]: https://github.com/ben-biddington/Bang/blob/master/src/Bang.Integration.Tests/Core/StatTests.cs "netstat examples"
[2]: https://github.com/ben-biddington/Bang/blob/master/src/Bang.Integration.Tests/Core/UseTests.cs "net use examples"
int maxShotguns = 0;
String factionName = null;
List<String> shotgunTeam = new List<string>();

Match shotgunCommand = Regex.Match(player.LastChat, @"!shotgun", RegexOptions.IgnoreCase);
Match shinfoCommand = Regex.Match(player.LastChat, @"!shinfo", RegexOptions.IgnoreCase);

if (plugin.RoundData.issetInt("shotgunSlots")) maxShotguns = plugin.RoundData.getInt("shotgunSlots");

if (!plugin.RoundData.issetObject("shlEntries1")) plugin.RoundData.setObject("shlEntries1", new List<String>());
if (!plugin.RoundData.issetObject("shlEntries2")) plugin.RoundData.setObject("shlEntries2", new List<String>());
List<String> shotgunTeam1 = (List<String>)plugin.RoundData.getObject("shlEntries1");
List<String> shotgunTeam2 = (List<String>)plugin.RoundData.getObject("shlEntries2");

if (player.TeamId == 1) shotgunTeam = shotgunTeam1;
else if (player.TeamId == 2) shotgunTeam = shotgunTeam2;

if (server.GetFaction(player.TeamId) == 0) factionName = "US";
else if (server.GetFaction(player.TeamId) == 1) factionName = "RU";
else if (server.GetFaction(player.TeamId) == 2) factionName = "CN";

List<String> showShotgun = new List<String>();

if (shotgunCommand.Success) {
	if (shotgunTeam.Count == 0) {
		showShotgun.Add("Currently no one is in shotgun limit (Team " + factionName + ")");
	} else {
		showShotgun.Add("Current players in shotgun limit (Team " + factionName + "):");
		foreach (string shotgun in shotgunTeam) {
			showShotgun.Add("Slot (" + (shotgunTeam.IndexOf(shotgun)+1) + "/" + maxShotguns + ") is occupied by " + shotgun);
		}
	}
} else if (shinfoCommand.Success) {
	showShotgun.Add("First kill using shotgun gives you a slot");
	showShotgun.Add("You will lose your slot with non-shotgun kill");
	showShotgun.Add("Allowed categories: Shotgun, Handguns, Knifes, Explosives and Vehicles");
	showShotgun.Add("Type !shotgun to see occupied slots");
}

foreach (string show in showShotgun) {
	plugin.SendPlayerMessage(player.Name, show);
}

return false;
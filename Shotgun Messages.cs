// Insane Limits - Shotgunlimit by TheTomik

// Set max shotguns each team
int maxShotguns = 6;

// Set kills for timeban
// Info: at 3 the user get 2 warnings before being timebanned
int warnKick = 3;

// ReservedSlots Player can use shotgun without shotgunslot
// 0 = not allowed  |  1 = allowed
int vipAllowed = 1;

// Admins can use shotgun without shotgunslot
// 0 = not allowed  |  1 = allowed
int adminAllowed = 1;

// Set (main) allowed weapons and weaponcategory
bool shotgunLimit = kill.Category == "Shotgun";

// Set (second) allowed weapons and weaponcategory
bool shotgunAllowed = (kill.Category == "Handgun" || kill.Category == "Explosive" || kill.Category == "ProjectileExplosive" || kill.Weapon == "Melee" || Regex.Match(kill.Weapon, @"(_:Shorty)").Success);

// Allow all vehicles
bool vehiclesAllowed = (Regex.Match(kill.Weapon, @"(_:Death|Roadkill|AMTRAC|Vehicle|Jeep|Growler|MRAP|Buggy|Venom|APC|Anti-Air|PGZ-95|HIMARS|MBT|Tank|Jet|Heli|AC-130|Lancer|Xian|Global|CB90|DV-15|RHIB|Bird)", RegexOptions.IgnoreCase).Success);


// Dont edit
if (vipAllowed == 1) {
	List<String> ReservervedSlots = plugin.GetReservedSlotsList();
	if (ReservervedSlots.Contains(killer.Name))	return false;
}

if (adminAllowed == 1) {
	bool isAdmin = false;
	bool bKill = false;
	bool bKick = false;
	bool bBan = false;
	bool bMove = false;
	bool bLevel = false;
	if (plugin.CheckAccount(player.Name, out bKill, out bKick, out bBan, out bMove, out bLevel)) {
		if (bKill && bKick && bBan) return false;
	}
}

int warnings = 0;
int countShotguns = 0;
String privateMessage = null;
String globalMessage = null;
String playerKey = "shl_" + killer.Name;
String factionName = null;

if (plugin.RoundData.issetInt(playerKey)) warnings = plugin.RoundData.getInt(playerKey);
if (!plugin.RoundData.issetInt("shotgunSlots")) plugin.RoundData.setInt("shotgunSlots", maxShotguns);
if (!plugin.RoundData.issetObject("shlEntries1")) plugin.RoundData.setObject("shlEntries1", new List<String>());
if (!plugin.RoundData.issetObject("shlEntries2")) plugin.RoundData.setObject("shlEntries2", new List<String>());

List<String> shotgunTeam1 = (List<String>)plugin.RoundData.getObject("shlEntries1");
List<String> shotgunTeam2 = (List<String>)plugin.RoundData.getObject("shlEntries2");

if (killer.TeamId == 1) countShotguns = shotgunTeam1.Count;
else if (killer.TeamId == 2) countShotguns = shotgunTeam2.Count;

if (server.GetFaction(player.TeamId) == 0) factionName = "US";
else if (server.GetFaction(player.TeamId) == 1) factionName = "RU";
else if (server.GetFaction(player.TeamId) == 2) factionName = "CN";

if (shotgunTeam1.Contains(killer.Name) || shotgunTeam2.Contains(killer.Name)) {
	if (!shotgunLimit && !shotgunAllowed && !vehiclesAllowed) {
		privateMessage = "You have lost your shotgun slot!";

		if (killer.TeamId == 1 && shotgunTeam1.Contains(killer.Name)) {
            shotgunTeam1.Remove(killer.Name);
            plugin.SendPlayerMessage(killer.Name, privateMessage);
            plugin.SendPlayerYell(killer.Name, privateMessage, 10);
		} else if (killer.TeamId == 1 && shotgunTeam2.Contains(killer.Name)) {
            shotgunTeam2.Remove(killer.Name);
            plugin.SendPlayerMessage(killer.Name, privateMessage);
            plugin.SendPlayerYell(killer.Name, privateMessage, 10);
        } else if (killer.TeamId == 2 && shotgunTeam2.Contains(killer.Name)) {
            shotgunTeam2.Remove(killer.Name);
            plugin.SendPlayerMessage(killer.Name, privateMessage);
            plugin.SendPlayerYell(killer.Name, privateMessage, 10);
        } else if (killer.TeamId == 2 && shotgunTeam1.Contains(killer.Name)) {
            shotgunTeam1.Remove(killer.Name);
            plugin.SendPlayerMessage(killer.Name, privateMessage);
            plugin.SendPlayerYell(killer.Name, privateMessage, 10);
        }
		return false;
	}
} else if (shotgunLimit) {
	if (countShotguns < maxShotguns) {
		if (killer.TeamId == 1) shotgunTeam1.Add(killer.Name);
		else if (killer.TeamId == 2) shotgunTeam2.Add(killer.Name);
        countShotguns += 1;
		warnings = 0;
		privateMessage = "You now have a shotgun slot! (" + countShotguns + "/" + maxShotguns + ")";
        plugin.SendPlayerMessage(killer.Name, privateMessage);
        plugin.SendPlayerYell(killer.Name, privateMessage, 10);
		return false;
	} else
    {
		if (limit.Activations(killer.Name, TimeSpan.FromSeconds(2)) > 1) return false;
		warnings += 1;
		plugin.RoundData.setInt(playerKey, warnings);
		if (warnings <= (warnKick-1)) {
			privateMessage = "You were killed by shotgun limit! Warning " + warnings + " out of " + warnKick +". Type !sinfo for more information.";
			plugin.SendPlayerMessage(killer.Name, privateMessage);
			plugin.SendPlayerYell(killer.Name, privateMessage, 10);
			plugin.KillPlayer(killer.Name, 1);
			return false;
		} else if (warnings == warnKick) {
            globalMessage = "Kicked for ignoring warnings of shotgun limit.";
            plugin.KickPlayerWithMessage(killer.Name, globalMessage);
            return false;
		}
	}
}

return false;
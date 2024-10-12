String factionName = null;

if (!plugin.RoundData.issetObject("shlEntries1")) plugin.RoundData.setObject("shlEntries1", new List<String>());
if (!plugin.RoundData.issetObject("shlEntries2")) plugin.RoundData.setObject("shlEntries2", new List<String>());
List<String> shotgunTeam1 = (List<String>)plugin.RoundData.getObject("shlEntries1");
List<String> shotgunTeam2 = (List<String>)plugin.RoundData.getObject("shlEntries2");

if (server.GetFaction(player.TeamId) == 0) factionName = "US";
else if (server.GetFaction(player.TeamId) == 1) factionName = "RU";
else if (server.GetFaction(player.TeamId) == 2) factionName = "CN";

if (shotgunTeam1.Contains(player.Name)) {
    shotgunTeam1.Remove(player.Name);
} else if (shotgunTeam2.Contains(player.Name)) {
    shotgunTeam2.Remove(player.Name);
}

return false;
using System.ComponentModel;
using Exiled.API.Interfaces;
using UnityEngine;

namespace RandomChapterSB
{
    public class Config : IConfig
    {
        [Description("Is the plugin enabled?")]
        public bool IsEnabled { get; set; } = true;

        [Description("Is the debug enabled?")]
        public bool Debug { get; set; } = false;

        [Description("Minimum number of players for SCP-343 to spawn")]
        public int MinPlayersForAutoSpawn { get; set; } = 5;

        [Description("Custom name for SCP-343")]
        public string CustomName { get; set; } = "SCP-343";

        [Description("SCP-343 Spawn Position")]
        public Vector3 SpawnPosition { get; set; } = new Vector3(50.727f, 112.429f, 10.465f);
        
        [Description("Hint when 343 is spawned")]
        public string SpawnMessge { get; set; } = "You are SCP-343";
        
        [Description("Hint when 343 have been teleported")]
        public string TelepeortMessge { get; set; } = $"<color=#00FF00>You have been teleported</color>";
        
        [Description("Hint when players for teleportation not found")]
        public string NoPlayersForTp { get; set; } = "<color=red>There are no suitable players for teleportation!</color>";
        
        [Description("Hint when 343 have Cooldown")]
        public string CDMessage { get; set; } = "<color=yellow>CD</color>";
        
        [Description("Hint when 343 interacting with SCP-330")]
        public string InteractingWith330 { get; set; } = "<color=red>Undo action! Made to remove bugs</color>";
        
        [Description("Hint when 343 links players")]
        public string SCP343LinksPlayers { get; set; } = "<color=red>You can't link players!</color>";
        
        [Description("Hint when players bind 343")]
        public string PlayersBind343 { get; set; } = "<color=red>You cannot bind SCP-343</color>";


    }
}
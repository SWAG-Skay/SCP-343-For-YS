using System.ComponentModel;
using Exiled.API.Interfaces;

namespace RandomChapterSB
{
    public class Translations : ITranslation
    {
        [Description("Hint when 343 is spawned")]
        public string SpawnMessage { get; set; } = "You are SCP-343";

        [Description("Hint when 343 have been teleported")]
        public string TeleportMessage { get; set; } = "<color=#00FF00>You have been teleported</color>";

        [Description("Hint when players for teleportation not found")]
        public string NoPlayersForTp { get; set; } = "<color=red>There are no suitable players for teleportation!</color>";

        [Description("Hint when 343 have Cooldown")]
        public string CDMessage { get; set; } = "<color=yellow>Cooldown</color>";

        [Description("Hint when 343 interacting with SCP-330")]
        public string InteractingWith330 { get; set; } = "<color=red>Undo action! Made to remove bugs</color>";

        [Description("Hint when 343 links players")]
        public string SCP343LinksPlayers { get; set; } = "<color=red>You can't link players!</color>";

        [Description("Hint when players bind 343")]
        public string PlayersBind343 { get; set; } = "<color=red>You cannot bind SCP-343</color>";

        [Description("Flight mode activation message")]
        public string FlightActivated { get; set; } = "Flight mode activated!";

        [Description("Flight mode ended message")]
        public string FlightEnded { get; set; } = "Flight mode has ended!";

        [Description("Flight mode cooldown message")]
        public string FlightCooldown { get; set; } = "Wait some more {0:0} seconds!";

        [Description("SCP-343 spawn success message")]
        public string SpawnSuccess { get; set; } = "{0} is now SCP-343!";

        [Description("SCP-343 already exists message")]
        public string AlreadySCP343 { get; set; } = "{0} is already SCP-343!";
    }
}

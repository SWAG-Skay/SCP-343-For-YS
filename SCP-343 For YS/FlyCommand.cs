using System;
using System.Collections.Generic;
using CommandSystem;
using Exiled.API.Features;
using MEC;
using RemoteAdmin;

namespace RandomChapterSB
{
    public class FlyCommand : ICommand
    {
        private readonly Plugin plugin;
        public Dictionary<Player, DateTime> lastFlyUsage = new Dictionary<Player, DateTime>();

        public FlyCommand(Plugin plugin)
        {
            this.plugin = plugin;
        }

        public string Command { get; } = "fly";
        public string[] Aliases { get; } = { };
        public string Description { get; } = "Activates flight mode for 25 seconds";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!(sender is PlayerCommandSender playerSender))
            {
                response = "This command is only available to players!";
                return false;
            }

            Player player = Player.Get(playerSender.ReferenceHub);

            if (player == null)
            {
                response = "Could not find player!";
                return false;
            }

            if (!plugin.ChapterSBPlayers.Contains(player))
            {
                response = "Only SCP-343 can use this command!";
                return false;
            }

            if (lastFlyUsage.TryGetValue(player, out DateTime lastUsage))
            {
                TimeSpan cooldownLeft = TimeSpan.FromMinutes(5) - (DateTime.Now - lastUsage);
                if (cooldownLeft > TimeSpan.Zero)
                {
                    response = $"Wait some more {cooldownLeft.TotalSeconds:0} seconds!";
                    player.ShowHint($"Wait some more {cooldownLeft.TotalSeconds:0} seconds!", 3);
                    return false;
                }
            }

            player.IsNoclipPermitted = true;
            player.IsGodModeEnabled = true;
            lastFlyUsage[player] = DateTime.Now;

            response = "flight mode activated!";

            Timing.CallDelayed(25f, () =>
            {
                if (player != null && player.IsConnected)
                {
                    player.IsNoclipPermitted = false;
                    player.ShowHint("flight mode has ended!", 5);
                }
            });

            return true;
        }
    }
}
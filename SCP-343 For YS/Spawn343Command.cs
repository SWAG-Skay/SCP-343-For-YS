using System;
using CommandSystem;
using Exiled.API.Features;
using RemoteAdmin;

namespace RandomChapterSB
{
    public class Spawn343Command : ICommand
    {
        private readonly Plugin plugin;

        public Spawn343Command(Plugin plugin)
        {
            this.plugin = plugin;
        }

        public string Command { get; } = "s343";
        public string[] Aliases { get; } = { };
        public string Description { get; } = "Spawns you as SCP-343. To spawn others: s343 ID";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!(sender is PlayerCommandSender playerSender))
            {
                response = "This command is only available through the admin console!";
                return false;
            }

            Player targetPlayer;
            Player player = Player.Get(playerSender.ReferenceHub);

            if (!playerSender.CheckPermission(PlayerPermissions.FacilityManagement))
            {
                response = "Insufficient permissions to use this command!";
                return false;
            }

            if (arguments.Count == 0)
            {
                targetPlayer = player;

                if (targetPlayer == null)
                {
                    response = "Couldn't find you among the players!";
                    return false;
                }
            }
            else
            {
                if (!int.TryParse(arguments.At(0), out int playerId))
                {
                    response = "Invalid ID!";
                    return false;
                }

                targetPlayer = Player.Get(playerId);

                if (targetPlayer == null)
                {
                    response = $"Player with ID {playerId} not found!";
                    return false;
                }
            }

            if (!targetPlayer.IsAlive)
            {
                response = "The player must be alive!";
                return false;
            }

            if (plugin.ChapterSBPlayers.Contains(targetPlayer))
            {
                response = $"{targetPlayer.Nickname} is already SCP-343!";
                return false;
            }

            plugin.MakePlayerChapterSB(targetPlayer);
            plugin.ChapterSBPlayers.Add(targetPlayer);

            response = $"{targetPlayer.Nickname} now SCP-343!";
            return true;
        }
    }
}
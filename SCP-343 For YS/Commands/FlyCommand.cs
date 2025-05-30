using System;
using System.Collections.Concurrent;
using CommandSystem;
using Exiled.API.Features;
using MEC;
using RemoteAdmin;

namespace SCP_343_For_YS.Commands
{
    /// <summary>
    /// Command to activate flight mode for SCP-343 players for a limited duration.
    /// </summary>
    public class FlyCommand : ICommand
    {
        private readonly Plugin _plugin;
        private readonly ConcurrentDictionary<Player, DateTime> _lastFlyUsage = new ConcurrentDictionary<Player, DateTime>();
        private const float FlightDuration = 25f;
        private const float CooldownMinutes = 5f;

        /// <summary>
        /// Initializes a new instance of the <see cref="FlyCommand"/> class.
        /// </summary>
        /// <param name="plugin">The plugin instance.</param>
        public FlyCommand(Plugin plugin)
        {
            _plugin = plugin ?? throw new ArgumentNullException(nameof(plugin));
        }

        /// <summary>
        /// Gets the command name.
        /// </summary>
        public string Command => "fly";

        /// <summary>
        /// Gets the command aliases.
        /// </summary>
        public string[] Aliases => Array.Empty<string>();

        /// <summary>
        /// Gets the command description.
        /// </summary>
        public string Description => Plugin.Instance.Translation.GetMessage("FlyCommandDescription", _plugin.Config.Language);

        /// <summary>
        /// Executes the command to activate flight mode for an SCP-343 player.
        /// </summary>
        /// <param name="arguments">The command arguments.</param>
        /// <param name="sender">The command sender.</param>
        /// <param name="response">The response message.</param>
        /// <returns>True if the command executes successfully, false otherwise.</returns>
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!(sender is PlayerCommandSender playerSender))
            {
                response = Plugin.Instance.Translation.GetMessage("CommandOnlyForPlayers", _plugin.Config.Language);
                return false;
            }

            Player player = Player.Get(playerSender.ReferenceHub);
            if (player == null)
            {
                response = Plugin.Instance.Translation.GetMessage("PlayerNotFound", _plugin.Config.Language);
                return false;
            }

            if (!_plugin.ChapterSBPlayers.Contains(player))
            {
                response = Plugin.Instance.Translation.GetMessage("OnlySCP343", _plugin.Config.Language);
                return false;
            }

            if (_lastFlyUsage.TryGetValue(player, out DateTime lastUsage))
            {
                TimeSpan cooldownLeft = TimeSpan.FromMinutes(CooldownMinutes) - (DateTime.Now - lastUsage);
                if (cooldownLeft > TimeSpan.Zero)
                {
                    response = string.Format(Plugin.Instance.Translation.GetMessage("FlyCooldown", _plugin.Config.Language), cooldownLeft.TotalSeconds);
                    player.ShowHint(string.Format(Plugin.Instance.Translation.GetMessage("FlyCooldownHint", _plugin.Config.Language), cooldownLeft.TotalSeconds), 3f);
                    return false;
                }
            }

            player.IsNoclipPermitted = true;
            player.IsGodModeEnabled = true;
            _lastFlyUsage[player] = DateTime.Now;

            response = Plugin.Instance.Translation.GetMessage("FlyActivated", _plugin.Config.Language);

            Timing.CallDelayed(FlightDuration, () =>
            {
                if (player?.IsConnected == true)
                {
                    player.IsNoclipPermitted = false;
                    player.ShowHint(Plugin.Instance.Translation.GetMessage("FlyEnded", _plugin.Config.Language), 5f);
                }
            });

            return true;
        }
    }
}
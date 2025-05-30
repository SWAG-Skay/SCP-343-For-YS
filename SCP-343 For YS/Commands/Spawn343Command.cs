using System;
using System.Text;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using RemoteAdmin;

namespace SCP_343_For_YS.Commands
{
    /// <summary>
    /// Command to spawn a player as SCP-343.
    /// </summary>
    public class Spawn343Command : ICommand, IUsageProvider
    {
        private readonly Plugin _plugin;

        /// <summary>
        /// Initializes a new instance of the <see cref="Spawn343Command"/> class.
        /// </summary>
        /// <param name="plugin">The plugin instance.</param>
        public Spawn343Command(Plugin plugin)
        {
            _plugin = plugin ?? throw new ArgumentNullException(nameof(plugin));
        }

        /// <summary>
        /// Gets the command name.
        /// </summary>
        public string Command => "s343";

        /// <summary>
        /// Gets the command aliases.
        /// </summary>
        public string[] Aliases => Array.Empty<string>();

        /// <summary>
        /// Gets the command description.
        /// </summary>
        public string Description => Plugin.Instance.Translation.GetMessage("Spawn343CommandDescription", _plugin.Config.Language);

        /// <summary>
        /// Gets the command usage instructions.
        /// </summary>
        public string[] Usage => new[] { "[ID]" };

        /// <summary>
        /// Executes the command to spawn a player as SCP-343.
        /// </summary>
        /// <param name="arguments">The command arguments.</param>
        /// <param name="sender">The command sender.</param>
        /// <param name="response">The response message.</param>
        /// <returns>True if the command executes successfully, false otherwise.</returns>
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            StringBuilder responseBuilder = new StringBuilder();

            if (!(sender is PlayerCommandSender playerSender))
            {
                response = Plugin.Instance.Translation.GetMessage("CommandOnlyForConsole", _plugin.Config.Language);
                return false;
            }

            Player executor = Player.Get(playerSender.ReferenceHub);
            if (!executor.CheckPermission(PlayerPermissions.FacilityManagement))
            {
                response = Plugin.Instance.Translation.GetMessage("InsufficientPermissions", _plugin.Config.Language);
                return false;
            }

            Player targetPlayer;
            if (arguments.Count == 0)
            {
                targetPlayer = executor;
            }
            else if (!int.TryParse(arguments.At(0), out int playerId) || (targetPlayer = Player.Get(playerId)) == null)
            {
                response = Plugin.Instance.Translation.GetMessage("InvalidPlayerId", _plugin.Config.Language);
                return false;
            }

            if (!targetPlayer.IsAlive)
            {
                response = Plugin.Instance.Translation.GetMessage("PlayerMustBeAlive", _plugin.Config.Language);
                return false;
            }

            if (_plugin.ChapterSBPlayers.Contains(targetPlayer))
            {
                response = string.Format(Plugin.Instance.Translation.GetMessage("AlreadySCP343", _plugin.Config.Language), targetPlayer.Nickname);
                return false;
            }

            targetPlayer.MakePlayerChapterSB();
            _plugin.ChapterSBPlayers.Add(targetPlayer);

            response = string.Format(Plugin.Instance.Translation.GetMessage("SpawnSuccess", _plugin.Config.Language), targetPlayer.Nickname);
            return true;
        }
    }
}
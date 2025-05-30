using System;
using System.Collections.Generic;
using System.Linq;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Toys;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Scp096;
using Exiled.Events.EventArgs.Scp330;
using MEC;
using PlayerRoles;
using RemoteAdmin;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SCP_343_For_YS
{
    public class Plugin : Plugin<Config, Translations>
    {
        /// <summary>
        /// Gets the plugin's metadata.
        /// </summary>
        public override string Author => "Skay";
        public override string Name => "SCP-343";
        public override string Prefix => "s343";
        public override Version Version => new Version(1, 3, 1);
        public override Version RequiredExiledVersion => new Version(9, 6, 0);

        /// <summary>
        /// Gets the singleton instance of the plugin.
        /// </summary>
        public static Plugin Instance { get; private set; }


        /// <summary>
        /// Gets the list of players assigned as SCP-343.
        /// </summary>
        public List<Player> ChapterSBPlayers { get; } = new List<Player>();

        /// <summary>
        /// Gets the dictionary tracking the last radio usage time for SCP-343 players.
        /// </summary>
        public Dictionary<Player, DateTime> LastRadioUsage { get; } = new Dictionary<Player, DateTime>();

        /// <summary>
        /// Gets the dictionary tracking the number of radio uses by SCP-343 players.
        /// </summary>
        public Dictionary<Player, int> RadioUsageCount { get; } = new Dictionary<Player, int>();

        /// <summary>
        /// Gets the dictionary tracking the last item drop time for SCP-343 players.
        /// </summary>
        public Dictionary<Player, DateTime> LastItemDropTime { get; } = new Dictionary<Player, DateTime>();

        /// <summary>
        /// Gets the dictionary tracking the current item set for SCP-343 players.
        /// </summary>
        public Dictionary<Player, int> CurrentItemSet { get; } = new Dictionary<Player, int>();

        /// <summary>
        /// Gets the dictionary tracking the last standard item set activation time.
        /// </summary>
        public Dictionary<Player, DateTime> LastStandardSetActivation { get; } = new Dictionary<Player, DateTime>();

        /// <summary>
        /// Gets the dictionary tracking light rings associated with SCP-343 players.
        /// </summary>
        public Dictionary<Player, Exiled.API.Features.Toys.Light> PlayerRings { get; } = new Dictionary<Player, Exiled.API.Features.Toys.Light>();

        private EventHandlers _eventHandlers;

        
        /// <summary>
        /// Called when the plugin is enabled, initializing commands and event handlers.
        /// </summary>
        public override void OnEnabled()
        {
            Instance = this;
            Config.PostInitialize();

            // Initialize event handlers
            _eventHandlers = new EventHandlers(this);
            _eventHandlers.RegisterEvents();

            base.OnEnabled();
        }

        /// <summary>
        /// Called when the plugin is disabled, cleaning up commands and event handlers.
        /// </summary>
        public override void OnDisabled()
        {
            // Initialize event handlers
            _eventHandlers = null;
            _eventHandlers.UnregisterEvents();

            Instance = null;
            base.OnDisabled();
        }
    }
}
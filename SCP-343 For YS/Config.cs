using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Exiled.API.Interfaces;
using UnityEngine;

namespace SCP_343_For_YS
{
    /// <summary>
    /// Configuration class for the SCP-343 plugin.
    /// </summary>
    public class Config : IConfig
    {
        /// <summary>
        /// Gets or sets whether the plugin is enabled.
        /// </summary>
        [Description("Is the plugin enabled?")]
        public bool IsEnabled { get; set; } = true;

        /// <summary>
        /// Gets or sets whether debug logging is enabled.
        /// </summary>
        [Description("Is debug logging enabled?")]
        public bool Debug { get; set; } = false;

        /// <summary>
        /// Gets or sets the minimum number of players required for SCP-343 to auto-spawn.
        /// </summary>
        [Description("Minimum number of players for SCP-343 to spawn")]
        public int MinPlayersForAutoSpawn { get; set; } = 5;

        /// <summary>
        /// Gets or sets the custom name for SCP-343.
        /// </summary>
        [Description("Custom name for SCP-343")]
        public string CustomName { get; set; } = "SCP-343";

        /// <summary>
        /// Gets or sets the spawn position for SCP-343.
        /// </summary>
        [Description("SCP-343 Spawn Position")]
        public Vector3 SpawnPosition { get; set; } = new Vector3(50.727f, 112.429f, 10.465f);

        /// <summary>
        /// Gets or sets the language for messages (en, ru, ja, zh, or custom).
        /// </summary>
        [Description("Language for messages (en, ru, ja, zh, or custom). Use 'custom' to load from Translations.")]
        public string Language { get; set; } = "en";

        /// <summary>
        /// Gets or sets the hint displayed when SCP-343 spawns (used if Language is not 'custom').
        /// </summary>
        [Description("Hint when SCP-343 is spawned")]
        public string SpawnMessage { get; set; } = "You are SCP-343";

        /// <summary>
        /// Gets or sets the hint displayed when SCP-343 teleports (used if Language is not 'custom').
        /// </summary>
        [Description("Hint when SCP-343 has been teleported")]
        public string TeleportMessage { get; set; } = "<color=#00FF00>You have been teleported</color>";

        /// <summary>
        /// Gets or sets the hint displayed when no players are available for teleportation (used if Language is not 'custom').
        /// </summary>
        [Description("Hint when no players are found for teleportation")]
        public string NoPlayersForTp { get; set; } = "<color=red>There are no suitable players for teleportation!</color>";

        /// <summary>
        /// Gets or sets the hint displayed when the item set change is on cooldown (used if Language is not 'custom').
        /// </summary>
        [Description("Hint when SCP-343 has a cooldown")]
        public string CDMessage { get; set; } = "<color=yellow>Cooldown {0:0} sec.</color>";

        /// <summary>
        /// Gets or sets the hint displayed when SCP-343 interacts with SCP-330 (used if Language is not 'custom').
        /// </summary>
        [Description("Hint when SCP-343 interacts with SCP-330")]
        public string InteractingWith330 { get; set; } = "<color=red>Undo action! Made to remove bugs</color>";

        /// <summary>
        /// Gets or sets the cooldown time between item set changes (in seconds).
        /// </summary>
        [Description("Cooldown between item set changes (seconds)")]
        public float ItemSetCooldown { get; set; } = 65f;

        /// <summary>
        /// Gets or sets the hint displayed when SCP-343 attempts to handcuff players (used if Language is not 'custom').
        /// </summary>
        [Description("Hint when SCP-343 attempts to handcuff players")]
        public string SCP343LinksPlayers { get; set; } = "<color=red>You can't link players!</color>";

        /// <summary>
        /// Gets or sets the hint displayed when players attempt to handcuff SCP-343 (used if Language is not 'custom').
        /// </summary>
        [Description("Hint when players attempt to handcuff SCP-343")]
        public string PlayersBind343 { get; set; } = "<color=red>You cannot bind SCP-343</color>";

        /// <summary>
        /// Gets or sets the list of keycard items for SCP-343.
        /// </summary>
        [Description("List of keycard items")]
        public List<ItemType> KeycardItems { get; set; } = new List<ItemType>
        {
            ItemType.KeycardScientist,
            ItemType.KeycardZoneManager,
            ItemType.KeycardFacilityManager,
            ItemType.KeycardChaosInsurgency,
            ItemType.KeycardO5,
            ItemType.KeycardMTFCaptain
        };

        /// <summary>
        /// Gets or sets the list of SCP items for SCP-343.
        /// </summary>
        [Description("List of SCP items")]
        public List<ItemType> SCPItems { get; set; } = new List<ItemType>
        {
            ItemType.SCP207,
            ItemType.SCP268,
            ItemType.SCP1576,
            ItemType.SCP1853
        };

        /// <summary>
        /// Gets or sets the list of pistol items for SCP-343.
        /// </summary>
        [Description("List of pistol items")]
        public List<ItemType> PistolItems { get; set; } = new List<ItemType>
        {
            ItemType.GunCOM15,
            ItemType.GunCOM18,
            ItemType.GunRevolver
        };

        /// <summary>
        /// Gets or sets the list of miscellaneous items for SCP-343.
        /// </summary>
        [Description("List of miscellaneous items")]
        public List<ItemType> MiscItems { get; set; } = new List<ItemType>
        {
            ItemType.Adrenaline,
            ItemType.Medkit,
            ItemType.Painkillers,
            ItemType.Flashlight,
            ItemType.Lantern
        };

        /// <summary>
        /// Gets the list of all case items (populated in PostInitialize).
        /// </summary>
        [Description("All items that count as case items")]
        public List<ItemType> AllCaseItems { get; private set; }

        /// <summary>
        /// Initializes the AllCaseItems list by combining all item lists and adding default items.
        /// </summary>
        public void PostInitialize()
        {
            AllCaseItems = KeycardItems
                .Concat(SCPItems)
                .Concat(PistolItems)
                .Concat(MiscItems)
                .Distinct()
                .ToList();
            AllCaseItems.Add(ItemType.Radio);
            AllCaseItems.Add(ItemType.Coin);
        }
    }
}
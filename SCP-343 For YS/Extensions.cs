using Exiled.API.Enums;
using Exiled.API.Features;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCP_343_For_YS
{
    public static class Extensions
    {
        /// <summary>
        /// Constant for SCP-343 custom info display.
        /// </summary>
        private const string CustomInfo = "SCP-343";
        
        /// <summary>
        /// Gives keycard items to the SCP-343 player.
        /// </summary>
        public static void GiveKeycards(this Player player)
        {
            foreach (var itemType in Plugin.Instance.Config.KeycardItems)
            {
                player.AddItem(itemType);
            }
        }

        /// <summary>
        /// Gives SCP items to the SCP-343 player.
        /// </summary>
        public static void GiveSCPItems(this Player player)
        {
            foreach (var itemType in Plugin.Instance.Config.SCPItems)
            {
                player.AddItem(itemType);
            }
        }

        /// <summary>
        /// Gives pistol items to the SCP-343 player.
        /// </summary>
        public static void GivePistols(this Player player)
        {
            foreach (var itemType in Plugin.Instance.Config.PistolItems)
            {
                player.AddItem(itemType);
            }
        }

        /// <summary>
        /// Gives miscellaneous items to the SCP-343 player.
        /// </summary>
        public static void GiveItems(this Player player)
        {
            foreach (var itemType in Plugin.Instance.Config.MiscItems)
            {
                player.AddItem(itemType);
            }
            player.AddItem(ItemType.Radio);
        }

        /// <summary>
        /// Checks if an item is part of the case items list.
        /// </summary>
        public static bool IsCaseItem(this ItemType item)
        {
            return Plugin.Instance.Config.AllCaseItems.Contains(item);
        }

        /// <summary>
        /// Gives the standard item set (Coin and Radio) to the player.
        /// </summary>
        private static void GiveStandardSet(this Player player)
        {
            player.ClearInventory();
            player.AddItem(ItemType.Coin);
            player.AddItem(ItemType.Radio);
        }

        /// <summary>
        /// Teleports SCP-343 to a random alive player.
        /// </summary>
        private static void TeleportPlayer(this Player player)
        {
            var targets = Player.List
                .Where(p => p != player && p.IsAlive && p.Role != RoleTypeId.Spectator && p.Role != RoleTypeId.Scp079)
                .ToList();

            if (targets.Count == 0)
            {
                player.ShowHint(Plugin.Instance.Translation.GetMessage("NoPlayersForTp", Plugin.Instance.Config.Language), 3f);
                return;
            }

            Player target = targets[UnityEngine.Random.Range(0, targets.Count)];
            player.Position = target.Position;
            player.ClearBroadcasts();
            player.ShowHint(Plugin.Instance.Translation.GetMessage("TelepeortMessge", Plugin.Instance.Config.Language), 5f);
        }

        /// <summary>
        /// Assigns the SCP-343 role to a player with specific attributes and items.
        /// </summary>
        public static void MakePlayerChapterSB(this Player player)
        {
            player.Role.Set(RoleTypeId.Tutorial);
            player.ClearInventory();
            player.AddItem(ItemType.Coin);
            player.AddItem(ItemType.Radio);
            player.CustomInfo = CustomInfo;
            player.InfoArea |= PlayerInfoArea.CustomInfo;
            player.IsGodModeEnabled = true;
            player.IsBypassModeEnabled = true;
            player.Position = Plugin.Instance.Config.SpawnPosition;
            player.EnableEffect(EffectType.Scp207, 15);
            player.CustomName = Plugin.Instance.Config.CustomName;
            player.ShowHint(Plugin.Instance.Translation.GetMessage("SpawnMessge", Plugin.Instance.Config.Language), 3f);
        }

        /// <summary>
        /// Clears SCP-343 status and associated data from a player.
        /// </summary>
        private static void ClearChapterSBStatus(this Player player, string reason)
        {
            player.IsGodModeEnabled = false;
            player.IsBypassModeEnabled = false;
            player.CustomInfo = string.Empty;
            player.DisableEffect(EffectType.Scp207);
            Plugin.Instance.ChapterSBPlayers.Remove(player);

            Plugin.Instance.LastRadioUsage.Remove(player);
            Plugin.Instance.RadioUsageCount.Remove(player);
            Plugin.Instance.LastItemDropTime.Remove(player);
            Plugin.Instance.CurrentItemSet.Remove(player);
            Plugin.Instance.LastStandardSetActivation.Remove(player);
            if (Plugin.Instance.PlayerRings.TryGetValue(player, out var ring) && Plugin.Instance.PlayerRings.Remove(player))
            {
                ring.Destroy();
            }

            Log.Debug($"SCP-343 status cleared for {player.Nickname} due to {reason}");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Scp096;
using Exiled.Events.EventArgs.Scp330;
using MEC;
using PlayerRoles;
using UnityEngine;

namespace SCP_343_For_YS
{
    /// <summary>
    /// Handles all event-related logic for the SCP-343 plugin.
    /// </summary>
    public class EventHandlers
    {
        private readonly Plugin _plugin;

        
        /// <summary>
        /// Initializes a new instance of the <see cref="EventHandlers"/> class.
        /// </summary>
        /// <param name="plugin">The plugin instance.</param>
        public EventHandlers(Plugin plugin)
        {
            _plugin = plugin ?? throw new ArgumentNullException(nameof(plugin));
        }

        /// <summary>
        /// Retrieves a localized message with null check and fallback to config or default.
        /// </summary>
        /// <param name="key">The message key.</param>
        /// <returns>The localized message or fallback value.</returns>
        public string GetLocalizedMessage(string key)
        {
            if (Plugin.Instance.Translation != null)
            {
                return Plugin.Instance.Translation.GetMessage(key, _plugin.Config.Language);
            }

            // Fallback to config values or hardcoded defaults
            return key switch
            {
                "SpawnMessge" => _plugin.Config.SpawnMessage ?? "You are SCP-343",
                "TelepeortMessge" => _plugin.Config.TeleportMessage ?? "<color=#00FF00>You have been teleported</color>",
                "NoPlayersForTp" => _plugin.Config.NoPlayersForTp ?? "<color=red>There are no suitable players for teleportation!</color>",
                "CDMessage" => _plugin.Config.CDMessage ?? "<color=yellow>Cooldown {0:0} sec.</color>",
                "InteractingWith330" => _plugin.Config.InteractingWith330 ?? "<color=red>Undo action! Made to remove bugs</color>",
                "SCP343LinksPlayers" => _plugin.Config.SCP343LinksPlayers ?? "<color=red>You can't link players!</color>",
                "PlayersBind343" => _plugin.Config.PlayersBind343 ?? "<color=red>You cannot bind SCP-343</color>",
                _ => $"Missing translation for {key}"
            };
        }

        /// <summary>
        /// Registers all event handlers for the plugin.
        /// </summary>
        public void RegisterEvents()
        {
            Exiled.Events.Handlers.Server.RoundStarted += OnRoundStart;
            Exiled.Events.Handlers.Player.Died += OnPlayerDeath;
            Exiled.Events.Handlers.Player.ChangingRole += OnRoleChange;
            Exiled.Events.Handlers.Player.DroppingItem += OnDroppingItem;
            Exiled.Events.Handlers.Player.Hurting += OnPlayerHurting;
            Exiled.Events.Handlers.Player.PickingUpItem += OnPickingUpItem;
            Exiled.Events.Handlers.Scp330.InteractingScp330 += OnInteractingScp330;
            Exiled.Events.Handlers.Player.Handcuffing += OnHandcuffing;
            Exiled.Events.Handlers.Scp096.AddingTarget += OnAddingTarget;
            Exiled.Events.Handlers.Player.InteractingDoor += OnInteractingDoor;
            Exiled.Events.Handlers.Player.FlippingCoin += OnCoinIsThrown;
        }

        /// <summary>
        /// Unregisters all event handlers for the plugin.
        /// </summary>
        public void UnregisterEvents()
        {
            Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStart;
            Exiled.Events.Handlers.Player.Died -= OnPlayerDeath;
            Exiled.Events.Handlers.Player.ChangingRole -= OnRoleChange;
            Exiled.Events.Handlers.Player.DroppingItem -= OnDroppingItem;
            Exiled.Events.Handlers.Player.Hurting -= OnPlayerHurting;
            Exiled.Events.Handlers.Player.PickingUpItem -= OnPickingUpItem;
            Exiled.Events.Handlers.Scp330.InteractingScp330 -= OnInteractingScp330;
            Exiled.Events.Handlers.Player.Handcuffing -= OnHandcuffing;
            Exiled.Events.Handlers.Scp096.AddingTarget -= OnAddingTarget;
            Exiled.Events.Handlers.Player.InteractingDoor -= OnInteractingDoor;
            Exiled.Events.Handlers.Player.FlippingCoin -= OnCoinIsThrown;
        }

        /// <summary>
        /// Handles the round start event, assigning SCP-343 to a random player if conditions are met.
        /// </summary>
        private void OnRoundStart()
        {
            Timing.CallDelayed(1f, () =>
            {
                try
                {
                    if (Player.List.Count() < _plugin.Config.MinPlayersForAutoSpawn)
                        return;

                    var alivePlayers = Player.List.Where(p => p.IsAlive).ToList();
                    if (alivePlayers.Count == 0)
                        return;

                    Player randomPlayer = alivePlayers[UnityEngine.Random.Range(0, alivePlayers.Count)];
                    randomPlayer.MakePlayerChapterSB();
                    _plugin.ChapterSBPlayers.Add(randomPlayer);
                }
                catch (Exception ex)
                {
                    Log.Error($"Error assigning SCP-343: {ex.Message}");
                }
            });
        }

        /// <summary>
        /// Prevents SCP-343 from interacting with SCP-330.
        /// </summary>
        private void OnInteractingScp330(InteractingScp330EventArgs ev)
        {
            if (_plugin.ChapterSBPlayers.Contains(ev.Player))
            {
                ev.IsAllowed = false;
                ev.Player.ShowHint(GetLocalizedMessage("InteractingWith330"), 3f);
            }
        }

        /// <summary>
        /// Restricts SCP-343 from interacting with SCP-079 doors.
        /// </summary>
        private void OnInteractingDoor(InteractingDoorEventArgs ev)
        {
            if (!_plugin.ChapterSBPlayers.Contains(ev.Player))
                return;

            if (ev.Door.GameObject.name.Contains("079_First") || ev.Door.Type == DoorType.Scp079First)
            {
                ev.IsAllowed = false;
            }
        }

        /// <summary>
        /// Handles coin flip event to teleport SCP-343 to a random player.
        /// </summary>
        private void OnCoinIsThrown(FlippingCoinEventArgs ev)
        {
            if (!_plugin.ChapterSBPlayers.Contains(ev.Player) || ev.Item.Type != ItemType.Coin)
                return;

            TeleportPlayer(ev.Player);
            ev.IsAllowed = true;
        }

        /// <summary>
        /// Prevents SCP-343 from being handcuffed or handcuffing others.
        /// </summary>
        private void OnHandcuffing(HandcuffingEventArgs ev)
        {
            if (_plugin.ChapterSBPlayers.Contains(ev.Target))
            {
                ev.IsAllowed = false;
                ev.Player.ShowHint(GetLocalizedMessage("PlayersBind343"), 3f);
            }
            else if (_plugin.ChapterSBPlayers.Contains(ev.Player))
            {
                ev.IsAllowed = false;
                ev.Player.ShowHint(GetLocalizedMessage("SCP343LinksPlayers"), 3f);
            }
        }

        /// <summary>
        /// Prevents SCP-343 from being targeted by SCP-096.
        /// </summary>
        private void OnAddingTarget(AddingTargetEventArgs ev)
        {
            if (_plugin.ChapterSBPlayers.Contains(ev.Target))
            {
                ev.IsAllowed = false;
            }
        }

        /// <summary>
        /// Prevents SCP-343 from picking up SCP-330.
        /// </summary>
        private void OnPickingUpItem(PickingUpItemEventArgs ev)
        {
            if (_plugin.ChapterSBPlayers.Contains(ev.Player) && ev.Pickup.Type == ItemType.SCP330)
            {
                ev.IsAllowed = false;
                ev.Player.ShowHint(GetLocalizedMessage("InteractingWith330"), 3f);
            }
        }

        /// <summary>
        /// Prevents SCP-343 from dealing damage to other players.
        /// </summary>
        private void OnPlayerHurting(HurtingEventArgs ev)
        {
            if (ev?.Attacker != null && _plugin.ChapterSBPlayers.Contains(ev.Attacker))
            {
                ev.IsAllowed = false;
            }
        }

        /// <summary>
        /// Handles item dropping logic for SCP-343, including item set changes and cooldowns.
        /// </summary>
        private void OnDroppingItem(DroppingItemEventArgs ev)
        {
            if (ev.Item.Type == ItemType.Radio && ev.IsThrown)
            {
                if (_plugin.LastStandardSetActivation.TryGetValue(ev.Player, out DateTime lastActivation) &&
                    (DateTime.Now - lastActivation).TotalSeconds < _plugin.Config.ItemSetCooldown)
                {
                    ev.Player.ShowHint(string.Format(GetLocalizedMessage("CDMessage"),
                        _plugin.Config.ItemSetCooldown - (DateTime.Now - lastActivation).TotalSeconds), 3f);
                    ev.IsAllowed = false;
                    return;
                }

                ChangeItemSet(ev.Player);
                ev.IsAllowed = false;
                return;
            }

            if (ev.IsThrown && ev.Item.Type.IsCaseItem())
            {
                ev.IsAllowed = true;
                Timing.CallDelayed(0.1f, () =>
                {
                    if (ev.Player?.IsConnected == true)
                    {
                        GiveStandardSet(ev.Player);
                        _plugin.LastStandardSetActivation[ev.Player] = DateTime.Now;
                    }
                });
                return;
            }

            ev.IsAllowed = false;
        }

        /// <summary>
        /// Changes the item set for an SCP-343 player based on a cycle.
        /// </summary>
        private void ChangeItemSet(Player player)
        {
            int set = _plugin.CurrentItemSet.TryGetValue(player, out int currentSet) ? (currentSet % 4) + 1 : 1;
            _plugin.CurrentItemSet[player] = set;
            player.ClearInventory();

            switch (set)
            {
                case 1:
                    player.GiveKeycards();
                    break;
                case 2:
                    player.GiveSCPItems();
                    break;
                case 3:
                    player.GivePistols();
                    break;
                case 4:
                    player.GiveItems();
                    break;
            }

            player.AddItem(ItemType.Coin);
            player.AddItem(ItemType.Radio);
        }

        /// <summary>
        /// Gives the standard item set (Coin and Radio) to the player.
        /// </summary>
        private void GiveStandardSet(Player player)
        {
            player.ClearInventory();
            player.AddItem(ItemType.Coin);
            player.AddItem(ItemType.Radio);
        }

        /// <summary>
        /// Teleports SCP-343 to a random alive player.
        /// </summary>
        private void TeleportPlayer(Player player)
        {
            var targets = Player.List
                .Where(p => p != player && p.IsAlive && p.Role != RoleTypeId.Spectator && p.Role != RoleTypeId.Scp079)
                .ToList();

            if (targets.Count == 0)
            {
                player.ShowHint(GetLocalizedMessage("NoPlayersForTp"), 3f);
                return;
            }

            Player target = targets[UnityEngine.Random.Range(0, targets.Count)];
            player.Position = target.Position;
            player.ClearBroadcasts();
            player.ShowHint(GetLocalizedMessage("TelepeortMessge"), 5f);
        }

        /// <summary>
        /// Handles player death, removing SCP-343 status if applicable.
        /// </summary>
        private void OnPlayerDeath(DiedEventArgs ev)
        {
            if (_plugin.ChapterSBPlayers.Contains(ev.Player))
            {
                ev.Player.IsGodModeEnabled = false;
                ClearChapterSBStatus(ev.Player, "Death");
            }
        }

        /// <summary>
        /// Handles role changes, removing SCP-343 status if the role changes from Tutorial.
        /// </summary>
        private void OnRoleChange(ChangingRoleEventArgs ev)
        {
            if (_plugin.ChapterSBPlayers.Contains(ev.Player) && ev.NewRole != RoleTypeId.Tutorial)
            {
                ClearChapterSBStatus(ev.Player, "Role Change");
            }
        }

        /// <summary>
        /// Clears SCP-343 status and associated data from a player.
        /// </summary>
        private void ClearChapterSBStatus(Player player, string reason)
        {
            player.IsGodModeEnabled = false;
            player.IsBypassModeEnabled = false;
            player.CustomInfo = string.Empty;
            player.DisableEffect(EffectType.Scp207);
            _plugin.ChapterSBPlayers.Remove(player);

            _plugin.LastRadioUsage.Remove(player);
            _plugin.RadioUsageCount.Remove(player);
            _plugin.LastItemDropTime.Remove(player);
            _plugin.CurrentItemSet.Remove(player);
            _plugin.LastStandardSetActivation.Remove(player);
            if (_plugin.PlayerRings.TryGetValue(player, out var ring) && _plugin.PlayerRings.Remove(player))
            {
                ring.Destroy();
            }

            Log.Debug($"SCP-343 status cleared for {player.Nickname} due to {reason}");
        }
    }
}
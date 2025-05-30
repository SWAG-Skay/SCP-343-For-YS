using System;
using System.Collections.Generic;
using System.Linq;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Interfaces;
using PlayerRoles;
using MEC;
using RemoteAdmin;
using UnityEngine;

namespace RandomChapterSB
{
    public class Plugin : Plugin<Config>
    {
        public override string Author => "Skay";
        public override string Name => "SCP-343";
        public override string Prefix => "s343";
        public override Version Version => new Version(1, 2, 0);
        public override Version RequiredExiledVersion => new Version(9, 6, 0);

        public List<Player> ChapterSBPlayers { get; } = new List<Player>();
        public static Plugin Instance { get; private set; }
        public Translations Translations { get; } = new Translations();

        private const string CustomInfo = "SCP-343";
        private Spawn343Command _spawn343Command;
        private FlyCommand _flyCommand;
        private EventHandlers _eventHandlers;

        public Dictionary<Player, DateTime> lastRadioUsage = new Dictionary<Player, DateTime>();
        public Dictionary<Player, int> radioUsageCount = new Dictionary<Player, int>();
        public Dictionary<Player, DateTime> lastItemDropTime = new Dictionary<Player, DateTime>();
        public Dictionary<Player, int> currentItemSet = new Dictionary<Player, int>();
        public Dictionary<Player, DateTime> lastStandardSetActivation = new Dictionary<Player, DateTime>();
        public Dictionary<Player, Exiled.API.Features.Toys.Light> PlayerRings = new Dictionary<Player, Exiled.API.Features.Toys.Light>();

        public override void OnEnabled()
        {
            Instance = this;
            Config.PostInitialize();

            _eventHandlers = new EventHandlers(this);
            _spawn343Command = new Spawn343Command(this);
            _flyCommand = new FlyCommand(this);

            Exiled.Events.Handlers.Server.RoundStarted += _eventHandlers.OnRoundStart;
            Exiled.Events.Handlers.Player.Died += _eventHandlers.OnPlayerDeath;
            Exiled.Events.Handlers.Player.ChangingRole += _eventHandlers.OnRoleChange;
            Exiled.Events.Handlers.Player.DroppingItem += _eventHandlers.OnDroppingItem;
            Exiled.Events.Handlers.Player.Hurting += _eventHandlers.OnPlayerHurting;
            Exiled.Events.Handlers.Player.PickingUpItem += _eventHandlers.OnPickingUpItem;
            Exiled.Events.Handlers.Scp330.InteractingScp330 += _eventHandlers.OnInteractingScp330;
            Exiled.Events.Handlers.Player.Handcuffing += _eventHandlers.OnHandcuffing;
            Exiled.Events.Handlers.Scp096.AddingTarget += _eventHandlers.OnAddingTarget;
            Exiled.Events.Handlers.Player.InteractingDoor += _eventHandlers.OnInteractingDoor;
            Exiled.Events.Handlers.Player.FlippingCoin += _eventHandlers.OnCoinIsThrowen;

            CommandProcessor.RemoteAdminCommandHandler.RegisterCommand(_spawn343Command);
            QueryProcessor.DotCommandHandler.RegisterCommand(_flyCommand);

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Exiled.Events.Handlers.Server.RoundStarted -= _eventHandlers.OnRoundStart;
            Exiled.Events.Handlers.Player.Died -= _eventHandlers.OnPlayerDeath;
            Exiled.Events.Handlers.Player.ChangingRole -= _eventHandlers.OnRoleChange;
            Exiled.Events.Handlers.Player.DroppingItem -= _eventHandlers.OnDroppingItem;
            Exiled.Events.Handlers.Player.Hurting -= _eventHandlers.OnPlayerHurting;
            Exiled.Events.Handlers.Player.PickingUpItem -= _eventHandlers.OnPickingUpItem;
            Exiled.Events.Handlers.Scp330.InteractingScp330 -= _eventHandlers.OnInteractingScp330;
            Exiled.Events.Handlers.Player.Handcuffing -= _eventHandlers.OnHandcuffing;
            Exiled.Events.Handlers.Scp096.AddingTarget -= _eventHandlers.OnAddingTarget;
            Exiled.Events.Handlers.Player.InteractingDoor -= _eventHandlers.OnInteractingDoor;
            Exiled.Events.Handlers.Player.FlippingCoin -= _eventHandlers.OnCoinIsThrowen;

            CommandProcessor.RemoteAdminCommandHandler.UnregisterCommand(_spawn343Command);
            QueryProcessor.DotCommandHandler.UnregisterCommand(_flyCommand);

            _eventHandlers = null;
            Instance = null;

            base.OnDisabled();
        }

        public void TeleportPlayer(Player player)
        {
            List<Player> targets = Player.List
                .Where(p => p != player && p.IsAlive && p.Role != RoleTypeId.Spectator && p.Role != RoleTypeId.Scp079)
                .ToList();

            if (targets.Count == 0)
            {
                player.ShowHint(Translations.NoPlayersForTp, 3);
                return;
            }

            Player target = targets[UnityEngine.Random.Range(0, targets.Count)];
            player.Position = target.Position;
            player.ClearBroadcasts();
            player.ShowHint(Translations.TeleportMessage, 5);
        }

        public void MakePlayerChapterSB(Player player)
        {
            player.Role.Set(RoleTypeId.Tutorial);
            player.ClearInventory();
            player.AddItem(ItemType.Coin);
            player.AddItem(ItemType.Radio);
            player.CustomInfo = CustomInfo;
            player.InfoArea |= PlayerInfoArea.CustomInfo;
            player.IsGodModeEnabled = true;
            player.IsBypassModeEnabled = true;
            player.Position = Config.SpawnPosition;
            player.EnableEffect(EffectType.Scp207, 15);
            player.CustomName = Config.CustomName;
            player.ShowHint(Translations.SpawnMessage, 3);
        }

        public void ClearChapterSBStatus(Player player, string reason)
        {
            player.IsGodModeEnabled = false;
            player.IsBypassModeEnabled = false;
            player.CustomInfo = string.Empty;
            player.DisableEffect(EffectType.Scp207);
            ChapterSBPlayers.Remove(player);

            lastRadioUsage.Remove(player);
            radioUsageCount.Remove(player);
            lastItemDropTime.Remove(player);
            currentItemSet.Remove(player);
            lastStandardSetActivation.Remove(player);
        }
    }
}

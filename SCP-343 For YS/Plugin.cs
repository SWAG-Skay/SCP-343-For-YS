using System;
using System.Linq;
using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using PlayerRoles;
using MEC;
using RemoteAdmin;
using Exiled.Events.EventArgs.Scp330;
using Exiled.Events.EventArgs.Scp096;

namespace RandomChapterSB
{
    public class Plugin : Plugin<Config>
    {
        public override string Author => "Skay";
        public override string Name => "SCP-343";
        public override string Prefix => "s343";
        public override Version Version => new Version(1, 2, 1);
        public override Version RequiredExiledVersion => new Version(9, 6, 0);

        public List<Player> ChapterSBPlayers { get; private set; } = new List<Player>();
        public static object Instance { get; private set; }

        private const string CustomInfo = "SCP-343";
        private Spawn343Command spawn343Command;
        private FlyCommand flyCommand;
        public Dictionary<Player, DateTime> lastRadioUsage = new Dictionary<Player, DateTime>();
        public Dictionary<Player, int> radioUsageCount = new Dictionary<Player, int>();
        public Dictionary<Player, DateTime> lastItemDropTime = new Dictionary<Player, DateTime>();
        public Dictionary<Player, int> currentItemSet = new Dictionary<Player, int>();
        public Dictionary<Player, DateTime> lastStandardSetActivation = new Dictionary<Player, DateTime>();
        public Dictionary<Player, Exiled.API.Features.Toys.Light> PlayerRings = new Dictionary<Player, Exiled.API.Features.Toys.Light>();

        public override void OnEnabled()
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
            Exiled.Events.Handlers.Player.FlippingCoin += OnCoinIsThrowen;
            Instance = this;
            Config.PostInitialize();

            spawn343Command = new Spawn343Command(this);
            flyCommand = new FlyCommand(this);

            CommandProcessor.RemoteAdminCommandHandler.RegisterCommand(spawn343Command);
            QueryProcessor.DotCommandHandler.RegisterCommand(flyCommand);

            base.OnEnabled();
        }

        public override void OnDisabled()
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
            Exiled.Events.Handlers.Player.FlippingCoin -= OnCoinIsThrowen;
            Instance = null;

            CommandProcessor.RemoteAdminCommandHandler.UnregisterCommand(spawn343Command);
            QueryProcessor.DotCommandHandler.UnregisterCommand(flyCommand);

            base.OnDisabled();
        }

        private void OnRoundStart()
        {
            Timing.CallDelayed(1f, () =>
            {
                try
                {
                    if (Player.List.Count() < Config.MinPlayersForAutoSpawn)
                    {
                        return;
                    }

                    var alivePlayers = Player.List.Where(p => p.IsAlive).ToList();

                    if (alivePlayers.Count == 0) return;

                    Player randomPlayer = alivePlayers[UnityEngine.Random.Range(0, alivePlayers.Count)];
                    MakePlayerChapterSB(randomPlayer);
                    ChapterSBPlayers.Add(randomPlayer);
                }
                catch (Exception e)
                {
                    Log.Error($"Error in SCP-343 assignment: {e}");
                }
            });
        }

        private void OnInteractingScp330(InteractingScp330EventArgs ev)
        {
            if (ChapterSBPlayers.Contains(ev.Player))
            {
                ev.IsAllowed = false;
                ev.Player.ShowHint(Config.InteractingWith330, 3f);
            }
        }

        private void OnInteractingDoor(InteractingDoorEventArgs ev)
        {
            if (!ChapterSBPlayers.Contains(ev.Player))
                return;

            if (ev.Door.GameObject.name.Contains("079_First") ||
                ev.Door.Type == DoorType.Scp079First)
            {
                ev.IsAllowed = false;
            }
        }

        private void OnCoinIsThrowen(FlippingCoinEventArgs ev)
        {
            if (!ChapterSBPlayers.Contains(ev.Player)) return;

            if (ev.Item.Type == ItemType.Coin && ev.IsAllowed)
            {
                TeleportPlayer(ev.Player);
                ev.IsAllowed = true;
                return;
            }
        }

        private void OnHandcuffing(HandcuffingEventArgs ev)
        {
            if (ChapterSBPlayers.Contains(ev.Target))
            {
                ev.IsAllowed = false;
                ev.Player.ShowHint(Config.PlayersBind343, 3f);
            }

            if (ChapterSBPlayers.Contains(ev.Player))
            {
                ev.IsAllowed = false;
                ev.Player.ShowHint(Config.SCP343LinksPlayers, 3);
            }
        }

        private void OnAddingTarget(AddingTargetEventArgs ev)
        {
            if (ChapterSBPlayers.Contains(ev.Target))
            {
                ev.IsAllowed = false;
            }
        }

        private void OnPickingUpItem(PickingUpItemEventArgs ev)
        {
            if (ChapterSBPlayers.Contains(ev.Player) && ev.Pickup.Type == ItemType.SCP330)
            {
                ev.IsAllowed = false;
                ev.Player.ShowHint(Config.InteractingWith330, 3);
            }
        }

        private void OnPlayerHurting(HurtingEventArgs ev)
        {
            if (ev != null && ChapterSBPlayers.Contains(ev.Attacker))
            {
                ev.IsAllowed = false;
            }
        }

        private void OnDroppingItem(DroppingItemEventArgs ev)
        {
            if (ev.Item.Type == ItemType.Radio && ev.IsThrown)
            {
                if (lastStandardSetActivation.TryGetValue(ev.Player, out DateTime lastActivation) &&
                   (DateTime.Now - lastActivation).TotalSeconds < Config.ItemSetCooldown)
                {
                    ev.Player.ShowHint(Config.CDMessage, 3);
                    ev.IsAllowed = false;
                    return;
                }

                ChangeItemSet(ev.Player);
                ev.IsAllowed = false;
                return;
            }

            if (ev.IsThrown && IsCaseItem(ev.Item.Type))
            {
                ev.IsAllowed = true;

                Timing.CallDelayed(0.1f, () => {
                    if (ev.Player != null && ev.Player.IsConnected)
                    {
                        GiveStandardSet(ev.Player);
                        lastStandardSetActivation[ev.Player] = DateTime.Now;
                    }
                });
                return;
            }

            ev.IsAllowed = false;
        }

        private void ChangeItemSet(Player player)
        {
            if (!currentItemSet.TryGetValue(player, out int set))
                set = 0;
            set = (set % 4) + 1;
            currentItemSet[player] = set;
            player.ClearInventory();

            switch (set)
            {
                case 1:
                    GiveKeycards(player);
                    break;
                case 2:
                    GiveSCPItems(player);
                    break;
                case 3:
                    GivePistols(player);
                    break;
                case 4:
                    GiveItems(player);
                    break;
            }

            player.AddItem(ItemType.Coin);
            player.AddItem(ItemType.Radio);
        }

        private void GiveStandardSet(Player player)
        {
            player.ClearInventory();
            player.AddItem(ItemType.Coin);
            player.AddItem(ItemType.Radio);
        }
        private void TeleportPlayer(Player player)
        {
            List<Player> targets = Player.List
                .Where(p => p != player && p.IsAlive && p.Role != RoleTypeId.Spectator && p.Role != RoleTypeId.Scp079)
                .ToList();

            if (targets.Count == 0)
            {
                player.ShowHint(Config.NoPlayersForTp, 3);
                return;
            }

            Player target = targets[UnityEngine.Random.Range(0, targets.Count)];
            player.Position = target.Position;
            player.ClearBroadcasts();
            player.ShowHint(Config.TelepeortMessge, 5);
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
            player.ShowHint(Config.SpawnMessge, 3);
        }

        private void OnPlayerDeath(DiedEventArgs ev)
        {
            if (ChapterSBPlayers.Contains(ev.Player))
            {
                ev.Player.IsGodModeEnabled = false;
                ClearChapterSBStatus(ev.Player, "Death");
            }
        }

        private void OnRoleChange(ChangingRoleEventArgs ev)
        {
            if (ChapterSBPlayers.Contains(ev.Player) && ev.NewRole != RoleTypeId.Tutorial)
            {
                ClearChapterSBStatus(ev.Player, "Change Role");
            }
        }

        private void ClearChapterSBStatus(Player player, string reason)
        {
            player.IsGodModeEnabled = false;
            player.IsBypassModeEnabled = false;
            player.CustomInfo = string.Empty;
            player.DisableEffect(EffectType.Scp207);
            ChapterSBPlayers.Remove(player);

            if (lastRadioUsage.ContainsKey(player))
                lastRadioUsage.Remove(player);

            if (radioUsageCount.ContainsKey(player))
                radioUsageCount.Remove(player);
        }

        private void GiveKeycards(Player player)
        {
            foreach (var itemType in Config.KeycardItems)
            {
                player.AddItem(itemType);
            }
        }

        private void GiveSCPItems(Player player)
        {
            foreach (var itemType in Config.SCPItems)
            {
                player.AddItem(itemType);
            }
        }

        private void GivePistols(Player player)
        {
            foreach (var itemType in Config.PistolItems)
            {
                player.AddItem(itemType);
            }
        }

        private void GiveItems(Player player)
        {
            foreach (var itemType in Config.MiscItems)
            {
                player.AddItem(itemType);
            }
            player.AddItem(ItemType.Radio);
        }

        private bool IsCaseItem(ItemType item)
        {
            return Config.AllCaseItems.Contains(item);
        }
    }
}

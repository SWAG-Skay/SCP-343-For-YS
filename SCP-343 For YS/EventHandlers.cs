using System;
using System.Collections.Generic;
using System.Linq;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Scp096;
using Exiled.Events.EventArgs.Scp330;
using PlayerRoles;
using MEC;

namespace RandomChapterSB
{
    public class EventHandlers
    {
        private readonly Plugin _plugin;

        public EventHandlers(Plugin plugin)
        {
            _plugin = plugin;
        }

        public void OnRoundStart()
        {
            Timing.CallDelayed(1f, () =>
            {
                try
                {
                    if (Player.List.Count() < _plugin.Config.MinPlayersForAutoSpawn)
                        return;

                    var alivePlayers = Player.List.Where(p => p.IsAlive).ToList();
                    if (alivePlayers.Count == 0) return;

                    Player randomPlayer = alivePlayers[UnityEngine.Random.Range(0, alivePlayers.Count)];
                    _plugin.MakePlayerChapterSB(randomPlayer);
                    _plugin.ChapterSBPlayers.Add(randomPlayer);
                }
                catch (Exception e)
                {
                    Log.Error($"Error in SCP-343 assignment: {e}");
                }
            });
        }

        public void OnInteractingScp330(InteractingScp330EventArgs ev)
        {
            if (ev.Player.IsSCP343())
            {
                ev.IsAllowed = false;
                ev.Player.ShowHint(_plugin.Translations.InteractingWith330, 3f);
            }
        }

        public void OnInteractingDoor(InteractingDoorEventArgs ev)
        {
            if (!ev.Player.IsSCP343()) return;

            if (ev.Door.GameObject.name.Contains("079_First") || ev.Door.Type == DoorType.Scp079First)
                ev.IsAllowed = false;
        }

        public void OnCoinIsThrowen(FlippingCoinEventArgs ev)
        {
            if (!ev.Player.IsSCP343()) return;

            if (ev.Item.Type == ItemType.Coin && ev.IsAllowed)
            {
                _plugin.TeleportPlayer(ev.Player);
                ev.IsAllowed = true;
            }
        }

        public void OnHandcuffing(HandcuffingEventArgs ev)
        {
            if (ev.Target.IsSCP343())
            {
                ev.IsAllowed = false;
                ev.Player.ShowHint(_plugin.Translations.PlayersBind343, 3f);
            }

            if (ev.Player.IsSCP343())
            {
                ev.IsAllowed = false;
                ev.Player.ShowHint(_plugin.Translations.SCP343LinksPlayers, 3);
            }
        }

        public void OnAddingTarget(AddingTargetEventArgs ev)
        {
            if (ev.Target.IsSCP343())
                ev.IsAllowed = false;
        }

        public void OnPickingUpItem(PickingUpItemEventArgs ev)
        {
            if (ev.Player.IsSCP343() && ev.Pickup.Type == ItemType.SCP330)
            {
                ev.IsAllowed = false;
                ev.Player.ShowHint(_plugin.Translations.InteractingWith330, 3);
            }
        }

        public void OnPlayerHurting(HurtingEventArgs ev)
        {
            if (ev.Attacker != null && ev.Attacker.IsSCP343())
                ev.IsAllowed = false;
        }

        public void OnDroppingItem(DroppingItemEventArgs ev)
        {
            if (ev.Item.Type == ItemType.Radio && ev.IsThrown)
            {
                if (_plugin.lastStandardSetActivation.TryGetValue(ev.Player, out DateTime lastActivation) &&
                   (DateTime.Now - lastActivation).TotalSeconds < _plugin.Config.ItemSetCooldown)
                {
                    ev.Player.ShowHint(_plugin.Translations.CDMessage, 3);
                    ev.IsAllowed = false;
                    return;
                }

                if (!_plugin.currentItemSet.TryGetValue(ev.Player, out int set))
                    set = 0;
                set = (set % 4) + 1;
                _plugin.currentItemSet[ev.Player] = set;
                ev.Player.ChangeItemSet(set);
                ev.IsAllowed = false;
                return;
            }

            if (ev.Item.Type == ItemType.Coin)
            {
                ev.IsAllowed = false;
                return;
            }

            if (ev.IsThrown && ev.Item.IsCaseItem())
            {
                ev.IsAllowed = true;
                Timing.CallDelayed(0.1f, () => {
                    if (ev.Player != null && ev.Player.IsConnected)
                    {
                        ev.Player.GiveStandardSet();
                        _plugin.lastStandardSetActivation[ev.Player] = DateTime.Now;
                    }
                });
            }
            else
            {
                ev.IsAllowed = false;
            }
        }

        public void OnPlayerDeath(DiedEventArgs ev)
        {
            if (ev.Player.IsSCP343())
            {
                ev.Player.IsGodModeEnabled = false;
                _plugin.ClearChapterSBStatus(ev.Player, "Death");
            }
        }

        public void OnRoleChange(ChangingRoleEventArgs ev)
        {
            if (ev.Player.IsSCP343() && ev.NewRole != RoleTypeId.Tutorial)
                _plugin.ClearChapterSBStatus(ev.Player, "Change Role");
        }
    }
}

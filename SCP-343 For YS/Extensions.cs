using System;
using System.Collections.Generic;
using System.Linq;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using PlayerRoles;

namespace RandomChapterSB
{
    public static class Extensions
    {
        public static bool IsSCP343(this Player player) => Plugin.Instance.ChapterSBPlayers.Contains(player);

        public static void GiveStandardSet(this Player player)
        {
            player.ClearInventory();
            player.AddItem(ItemType.Coin);
            player.AddItem(ItemType.Radio);
        }

        public static void ChangeItemSet(this Player player, int set)
        {
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
                    player.GiveMiscItems();
                    break;
            }

            player.AddItem(ItemType.Coin);
            player.AddItem(ItemType.Radio);
        }

        public static void GiveKeycards(this Player player)
        {
            foreach (var item in Plugin.Instance.Config.KeycardItems)
            {
                player.AddItem(item);
            }
        }

        public static void GiveSCPItems(this Player player)
        {
            foreach (var item in Plugin.Instance.Config.SCPItems)
            {
                player.AddItem(item);
            }
        }

        public static void GivePistols(this Player player)
        {
            foreach (var item in Plugin.Instance.Config.PistolItems)
            {
                player.AddItem(item);
            }
        }

        public static void GiveMiscItems(this Player player)
        {
            foreach (var item in Plugin.Instance.Config.MiscItems)
            {
                player.AddItem(item);
            }
            player.AddItem(ItemType.Radio);
        }

        public static bool IsCaseItem(this Item item) => Plugin.Instance.Config.AllCaseItems.Contains(item.Type);
    }
}

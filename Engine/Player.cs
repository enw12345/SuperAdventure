﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class Player : LivingCreature
    {
        public int Gold { get; set; }
        public int ExperiencePoints { get; set; }
        public int Level { get { return ((ExperiencePoints / 100) + 1); } }

        public List<InventoryItem> Inventory { get; set; }
        public List<PlayerQuest> Quests { get; set; }

        public Location CurrentLocation { get; set; }

        public Player(int currentHitPoints, int maximumHitPoints, int gold, int experiencePoints) : base(currentHitPoints, maximumHitPoints)
        {
            Gold = gold;
            ExperiencePoints = experiencePoints;

            Inventory = new List<InventoryItem>();
            Quests = new List<PlayerQuest>();
        }

        public bool HasRequiredItemToEnterThisLocation(Location location)
        {
            if (location.ItemRequiredToEnter == null)
            {
                // There is no required item for this location, so return "true"
                return true;
            }

            // See if the player has the required item in thier inventory

            //LINQ way
            return Inventory.Exists(ii => ii.Details.ID ==
                location.ItemRequiredToEnter.ID);

            //Old Way
            //foreach (InventoryItem ii in Inventory)
            //{
            //    if (ii.Details.ID == location.ItemRequiredToEnter.ID)
            //    {
            //        // We found the required item, so return "true"
            //        return true;
            //    }
            //}

            //// We didn'tfind the required item in their inventory, so return "false:
            //return false;
        }

        public bool HasThisQuest(Quest quest)
        {
            return Quests.Exists(pq => pq.Details.ID == quest.ID);
        }

        public bool CompletedThisQuest(Quest quest)
        {
            foreach (PlayerQuest playerQuest in Quests)
            {
                if (playerQuest.Details.ID == quest.ID)
                {
                    return playerQuest.IsCompleted;
                }
            }

            return false;
        }

        public bool HasAllQuestCompletionItems(Quest quest)
        {
            // See if the player has all the items needed to complete the quest here

            //Old Way
            //foreach (QuestCompletionItem qci in quest.QuestCompletionItems)
            //{
            //    bool foundItemInPlayersInventory = false;

            //    // Check each item in the player's inventory, to see if they have it, and enough of it
            //    foreach (InventoryItem ii in Inventory)
            //    {
            //        // The player has the item in their inventory
            //        if (ii.Details.ID == qci.Details.ID)
            //        {
            //            foundItemInPlayersInventory = true;
            //            // The player does not have enough of this item to complete the quest
            //            if (ii.Quantity < qci.Quantity)
            //            {
            //                return false;
            //            }
            //        }
            //    }

            //    // The player does not have any of this quest completion item in their inventory
            //    if (!foundItemInPlayersInventory)
            //    {
            //        return false;
            //    }
            //}

            //// If we got here then the player must have all the required items, and enough of them, to complete the quest
            //return true;

            //See if the player has all the items needed to complete the quest
            foreach (QuestCompletionItem qci in quest.QuestCompletionItems)
            {
                // Check each item in the player's inventory, to see if they have it, and enough of it
                if (!Inventory.Exists(ii => ii.Details.ID == qci.Details.ID && ii.Quantity >= qci.Quantity))
                {
                    return false;
                }
            }

            return true;
        }

        public void RemoveQuestCompletionItems(Quest quest)
        {
            foreach (QuestCompletionItem qci in quest.QuestCompletionItems)
            {
                InventoryItem item = Inventory.SingleOrDefault(ii => ii.Details.ID == qci.Details.ID);

                if (item != null)
                {
                    // subtract the quantity from the player's inventory that was needed to complete the quest
                    item.Quantity -= qci.Quantity;
                }
            }
        }

        public void AddItemToInventory(Item itemToAdd)
        {
            InventoryItem item = Inventory.SingleOrDefault(ii => ii.Details.ID == itemToAdd.ID);

            if (item == null)
            {
                // They didn't have the item in their inventory, so increse the quantity by one
                Inventory.Add(new InventoryItem(itemToAdd, 1));
            }
            else
            {
                // They did have the item in their inventory, so increase the quantity by one
                item.Quantity++;
            }
        }

        public void MarkQuestCompleted(Quest quest)
        {
            // Find the quest in the player's quest list
            PlayerQuest playerQuest = Quests.SingleOrDefault(pq => pq.Details.ID == quest.ID);

            if (playerQuest != null)
            {
                playerQuest.IsCompleted = true;
            }
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;

namespace Arrowgene.Ddon.Shared.Model
{
    public class ClientItemInfo
    {
        public uint ItemId;
        public byte Category;
        public ushort Price;
        public byte StackLimit;
        public byte Rank;
        public string Name;
        public ItemSubCategory? SubCategory;

        public byte? Level;
        public EquipJobList? JobGroup;
        public byte? CrestSlots;
        public byte? Quality;
        public Gender? Gender;

        public StorageType StorageType
        { 
            get
            {
                // For consumables, materials and equipment, itemlist.ipa Category
                // matches with StorageType values. But key and job items differ.
                if (Category == 4)
                    return StorageType.KeyItems;
                else if (Category == 5)
                    return StorageType.ItemBagJob;
                else
                    return (StorageType) Category;
            }
        }

        public HashSet<JobId>? JobIds
        {
            get
            {
                if (JobGroup == null) return null;
                switch (JobGroup)
                {
                    case EquipJobList.All:
                        return Enum.GetValues(typeof(JobId)).Cast<JobId>().ToHashSet();
                    case EquipJobList.GroupHeavy:
                        return new HashSet<JobId> { JobId.Fighter, JobId.Warrior };
                    case EquipJobList.GroupLight:
                        return new HashSet<JobId> { JobId.Seeker, JobId.Hunter, JobId.SpiritLancer };
                    case EquipJobList.GroupPhysical:
                        return new HashSet<JobId> {
                        JobId.Fighter, JobId.Warrior,
                        JobId.Seeker, JobId.Hunter, JobId.SpiritLancer
                    };
                    case EquipJobList.GroupMagickRanged:
                        return new HashSet<JobId> { JobId.Priest, JobId.Sorcerer, JobId.ElementArcher };
                    case EquipJobList.GroupMagickMelee:
                        return new HashSet<JobId> { JobId.ShieldSage, JobId.Alchemist, JobId.HighScepter };
                    case EquipJobList.GroupMagickal:
                        return new HashSet<JobId> {
                        JobId.Priest, JobId.Sorcerer, JobId.ElementArcher,
                        JobId.ShieldSage, JobId.Alchemist, JobId.HighScepter
                    };
                    default:
                        return new HashSet<JobId> { (JobId)((int)JobGroup) };
                }
            }
        }

        public override string ToString()
        {
            return String.Format("{0} <{1}>", Name, ItemId);
        }

        public static ClientItemInfo GetInfoForItemId(Dictionary<uint, ClientItemInfo> clientItemInfos, uint itemId)
        {
            if (clientItemInfos.ContainsKey(itemId)) return clientItemInfos[itemId];
            throw new Exception("No item found with ID "+itemId);
        }
    }
}

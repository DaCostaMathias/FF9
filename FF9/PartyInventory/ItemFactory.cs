using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace FF9.PartyInventory
{
    public static class ItemFactory 
    {
        public static List<Item> CreateFullInventory()
        {
            var finalList = new List<Item>();
            return AppInfo.Info.Items
                .Where(item => !string.IsNullOrWhiteSpace(item.Value))
                .Select(item => new Item { id = item.Value, count = "50" }).ToList();

        }
    }
}

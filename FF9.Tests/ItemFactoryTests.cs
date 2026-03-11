using FF9.PartyInventory;
using Xunit;

namespace FF9.Tests;

public sealed class ItemFactoryTests
{
    [Fact]
    public void CreateFullInventory_ReturnsOneItemPerNamedItem()
    {
        int expectedCount = AppInfo.Info.Items.Count(i => !string.IsNullOrWhiteSpace(i.Value));

        List<Item> items = ItemFactory.CreateFullInventory();

        Assert.Equal(expectedCount, items.Count);
    }

    [Fact]
    public void CreateFullInventory_AllItemsHaveCountFifty()
    {
        List<Item> items = ItemFactory.CreateFullInventory();

        Assert.True(items.All(i => i.count == "50"));
    }

    [Fact]
    public void CreateFullInventory_ExcludesItemsWithBlankNames()
    {
        List<Item> items = ItemFactory.CreateFullInventory();

        Assert.False(items.Any(i => string.IsNullOrWhiteSpace(i.id)));
    }
}

using Xunit;

namespace FF9.Tests;

public sealed class JsonObjectTests
{
    [Fact]
    public void ItemId_SetterRaisesPropertyChanged()
    {
        Item item = new();
        string? propertyName = null;
        item.PropertyChanged += (_, e) => propertyName = e.PropertyName;

        item.id = "12";

        Assert.Equal("id", propertyName);
    }

    [Fact]
    public void MiniGameCardId_SetterRaisesPropertyChanged()
    {
        MiniGameCard card = new();
        string? propertyName = null;
        card.PropertyChanged += (_, e) => propertyName = e.PropertyName;

        card.id = "34";

        Assert.Equal("id", propertyName);
    }
}

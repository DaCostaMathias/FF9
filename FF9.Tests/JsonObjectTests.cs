namespace FF9.Tests;

[TestClass]
public sealed class JsonObjectTests
{
    [TestMethod]
    public void ItemId_SetterRaisesPropertyChanged()
    {
        Item item = new();
        string? propertyName = null;
        item.PropertyChanged += (_, e) => propertyName = e.PropertyName;

        item.id = "12";

        Assert.AreEqual("id", propertyName);
    }

    [TestMethod]
    public void MiniGameCardId_SetterRaisesPropertyChanged()
    {
        MiniGameCard card = new();
        string? propertyName = null;
        card.PropertyChanged += (_, e) => propertyName = e.PropertyName;

        card.id = "34";

        Assert.AreEqual("id", propertyName);
    }
}

using System.Linq;

namespace FF9.Tests;

[TestClass]
public sealed class CardCollectionFactoryTests
{
    [TestMethod]
    public void CreatePerfectCollection_CreatesOnePerfectCopyPerNamedCard()
    {
        var cards = CardCollectionFactory.CreatePerfectCollection();
        var expectedIds = AppInfo.Info.Cards
            .Where(card => !string.IsNullOrWhiteSpace(card.Value))
            .OrderBy(card => card.Key)
            .Select(card => card.Key.ToString())
            .ToList();

        CollectionAssert.AreEqual(expectedIds, cards.Select(card => card.id).ToList());
        Assert.IsTrue(cards.All(card =>
            card.type == "3" &&
            card.side == "50" &&
            card.atk == "10" &&
            card.pdef == "10" &&
            card.mdef == "10" &&
            card.cpoint == "255" &&
            card.arrow == "255"));
    }
}

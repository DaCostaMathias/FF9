using FF9.Card;
using System.Linq;
using Xunit;

namespace FF9.Tests;

public sealed class CardCollectionFactoryTests
{
    [Fact]
    public void CreatePerfectCollection_CreatesOnePerfectCopyPerNamedCard()
    {
        var cards = CardCollectionFactory.CreatePerfectCollection();
        var expectedIds = AppInfo.Info.Cards
            .Where(card => !string.IsNullOrWhiteSpace(card.Value))
            .OrderBy(card => card.Key)
            .Select(card => card.Key.ToString())
            .ToList();

        Assert.Equal(expectedIds, cards.Select(card => card.id).ToList());
        Assert.True(cards.All(card =>
            card.type == "3" &&
            card.side == "0" &&
            card.atk == "255" &&
            card.pdef == "255" &&
            card.mdef == "255" &&
            card.cpoint == "0" &&
            card.arrow == "255"));
    }
}

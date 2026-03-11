using FF9.Ability;
using FF9.Card;
using FF9.PartyInventory;
using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using Xunit;

namespace FF9.Tests;

public sealed class AppInfoAndConverterTests
{
    [Fact]
    public void AppInfoSingleton_LoadsDataFromInfoFiles()
    {
        Assert.Equal("Dagger", AppInfo.Info.Items[0x01]);
        Assert.Equal("Goblin", AppInfo.Info.Cards[0x00]);
        Assert.Equal("Cure", AppInfo.Info.Abilitys[0x01]);
    }

    [Fact]
    public void AppInfoConstructor_ParsesSupportedLinesAndSkipsInvalidOnes()
    {
        string originalDirectory = Environment.CurrentDirectory;
        string tempDirectory = Path.Combine(Path.GetTempPath(), $"FF9.Tests.{Guid.NewGuid():N}");

        try
        {
            Directory.CreateDirectory(Path.Combine(tempDirectory, "info"));
            File.WriteAllText(
                Path.Combine(tempDirectory, "info", "item.txt"),
                "#comment\r\n\r\ninvalid\r\n7\tPotion\r\n0x08\tHi-Potion\r\n");

            Environment.CurrentDirectory = tempDirectory;

            ConstructorInfo constructor = typeof(AppInfo).GetConstructor(
                BindingFlags.Instance | BindingFlags.NonPublic,
                binder: null,
                Type.EmptyTypes,
                modifiers: null)!;

            AppInfo info = (AppInfo)constructor.Invoke(null);

            Assert.Equal(2, info.Items.Count);
            Assert.Equal("Potion", info.Items[7]);
            Assert.Equal("Hi-Potion", info.Items[8]);
            Assert.Equal(0, info.Cards.Count);
            Assert.Equal(0, info.Abilitys.Count);
        }
        finally
        {
            Environment.CurrentDirectory = originalDirectory;
            if (Directory.Exists(tempDirectory))
            {
                Directory.Delete(tempDirectory, recursive: true);
            }
        }
    }

    [Fact]
    public void NameValueLine_ReturnsFalseForInvalidInput()
    {
        NameValue nameValue = new();

        Assert.False(nameValue.Line("invalid"));
    }

    [Fact]
    public void NameValueLine_ParsesDecimalAndHexValues()
    {
        NameValue nameValue = new();

        Assert.True(nameValue.Line("7\tPotion"));
        Assert.Equal((uint)7, nameValue.ID);
        Assert.Equal("Potion", nameValue.Name);

        Assert.True(nameValue.Line("0x08\tHi-Potion"));
        Assert.Equal((uint)8, nameValue.ID);
        Assert.Equal("Hi-Potion", nameValue.Name);
    }

    [Fact]
    public void ItemValueConverter_UsesItemDictionary()
    {
        ItemValueConverter converter = new();

        object result = converter.Convert("1", typeof(string), null, CultureInfo.InvariantCulture);

        Assert.Equal("Dagger", result);
    }

    [Fact]
    public void CardValueConverter_UsesCardDictionary()
    {
        CardValueConverter converter = new();

        object result = converter.Convert(0, typeof(string), null, CultureInfo.InvariantCulture);

        Assert.Equal("Goblin", result);
    }

    [Fact]
    public void AbilityValueConverter_UsesAbilityDictionary()
    {
        AbilityValueConverter converter = new();

        object result = converter.Convert("1", typeof(string), null, CultureInfo.InvariantCulture);

        Assert.Equal("Cure", result);
    }

    [Fact]
    public void ConvertersConvertBack_ThrowNotImplementedException()
    {
        ItemValueConverter itemConverter = new();
        CardValueConverter cardConverter = new();
        AbilityValueConverter abilityConverter = new();

        Assert.Throws<NotImplementedException>(() => itemConverter.ConvertBack("value", typeof(string), null, CultureInfo.InvariantCulture));
        Assert.Throws<NotImplementedException>(() => cardConverter.ConvertBack("value", typeof(string), null, CultureInfo.InvariantCulture));
        Assert.Throws<NotImplementedException>(() => abilityConverter.ConvertBack("value", typeof(string), null, CultureInfo.InvariantCulture));
    }

    [Fact]
    public void NameValueLine_ReturnsFalseForEmptyString()
    {
        NameValue nameValue = new();

        Assert.False(nameValue.Line(string.Empty));
    }

    [Fact]
    public void ItemValueConverter_ThrowsForUnknownKey()
    {
        ItemValueConverter converter = new();

        Assert.Throws<KeyNotFoundException>(() => converter.Convert(99999, typeof(string), null, CultureInfo.InvariantCulture));
    }

    [Fact]
    public void CardValueConverter_ThrowsForUnknownKey()
    {
        CardValueConverter converter = new();

        Assert.Throws<KeyNotFoundException>(() => converter.Convert(99999, typeof(string), null, CultureInfo.InvariantCulture));
    }

    [Fact]
    public void AbilityValueConverter_ThrowsForUnknownKey()
    {
        AbilityValueConverter converter = new();

        Assert.Throws<KeyNotFoundException>(() => converter.Convert(99999, typeof(string), null, CultureInfo.InvariantCulture));
    }
}

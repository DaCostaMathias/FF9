using System;
using System.Globalization;
using System.IO;
using System.Reflection;

namespace FF9.Tests;

[TestClass]
public sealed class AppInfoAndConverterTests
{
    [TestMethod]
    public void AppInfoSingleton_LoadsDataFromInfoFiles()
    {
        Assert.AreEqual("Dagger", AppInfo.Info.Items[0x01]);
        Assert.AreEqual("Goblin", AppInfo.Info.Cards[0x00]);
        Assert.AreEqual("Cure", AppInfo.Info.Abilitys[0x01]);
    }

    [TestMethod]
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

            Assert.AreEqual(2, info.Items.Count);
            Assert.AreEqual("Potion", info.Items[7]);
            Assert.AreEqual("Hi-Potion", info.Items[8]);
            Assert.AreEqual(0, info.Cards.Count);
            Assert.AreEqual(0, info.Abilitys.Count);
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

    [TestMethod]
    public void NameValueLine_ReturnsFalseForInvalidInput()
    {
        NameValue nameValue = new();

        Assert.IsFalse(nameValue.Line("invalid"));
    }

    [TestMethod]
    public void NameValueLine_ParsesDecimalAndHexValues()
    {
        NameValue nameValue = new();

        Assert.IsTrue(nameValue.Line("7\tPotion"));
        Assert.AreEqual((uint)7, nameValue.ID);
        Assert.AreEqual("Potion", nameValue.Name);

        Assert.IsTrue(nameValue.Line("0x08\tHi-Potion"));
        Assert.AreEqual((uint)8, nameValue.ID);
        Assert.AreEqual("Hi-Potion", nameValue.Name);
    }

    [TestMethod]
    public void ItemValueConverter_UsesItemDictionary()
    {
        ItemValueConverter converter = new();

        object result = converter.Convert("1", typeof(string), null, CultureInfo.InvariantCulture);

        Assert.AreEqual("Dagger", result);
    }

    [TestMethod]
    public void CardValueConverter_UsesCardDictionary()
    {
        CardValueConverter converter = new();

        object result = converter.Convert(0, typeof(string), null, CultureInfo.InvariantCulture);

        Assert.AreEqual("Goblin", result);
    }

    [TestMethod]
    public void AbilityValueConverter_UsesAbilityDictionary()
    {
        AbilityValueConverter converter = new();

        object result = converter.Convert("1", typeof(string), null, CultureInfo.InvariantCulture);

        Assert.AreEqual("Cure", result);
    }

    [TestMethod]
    public void ConvertersConvertBack_ThrowNotImplementedException()
    {
        ItemValueConverter itemConverter = new();
        CardValueConverter cardConverter = new();
        AbilityValueConverter abilityConverter = new();

        Assert.Throws<NotImplementedException>(() => itemConverter.ConvertBack("value", typeof(string), null, CultureInfo.InvariantCulture));
        Assert.Throws<NotImplementedException>(() => cardConverter.ConvertBack("value", typeof(string), null, CultureInfo.InvariantCulture));
        Assert.Throws<NotImplementedException>(() => abilityConverter.ConvertBack("value", typeof(string), null, CultureInfo.InvariantCulture));
    }
}

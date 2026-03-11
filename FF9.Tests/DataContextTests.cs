using System;
using System.IO;
using Xunit;

namespace FF9.Tests;

public sealed class DataContextTests
{
    [Fact]
    public void DefaultConstructor_UsesSingletonAppInfo()
    {
        DataContext context = new();

        Assert.Same(AppInfo.Info, context.Info);
        Assert.Null(context.Json);
    }

    [Fact]
    public void FileConstructor_LeavesJsonNullWhenFileDoesNotExist()
    {
        DataContext context = new(Path.Combine(Path.GetTempPath(), $"missing-{Guid.NewGuid():N}.json"));

        Assert.Null(context.Json);
    }

    [Fact]
    public void FileConstructor_LoadsJsonFromDisk()
    {
        string fileName = Path.Combine(Path.GetTempPath(), $"FF9.Tests.{Guid.NewGuid():N}.json");

        try
        {
            File.WriteAllText(fileName, "{\"Data\":{\"40000_Common\":{\"gil\":\"99\"},\"91000_State\":[\"state\"]}}");

            DataContext context = new(fileName);

            Assert.NotNull(context.Json);
            Assert.Equal("99", context.Json.Data.__invalid_name__40000_Common.gil);
            Assert.Equal(new[] { "state" }, context.Json.Data.__invalid_name__91000_State);
        }
        finally
        {
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
        }
    }

    [Fact]
    public void Save_DoesNothingWhenJsonIsNull()
    {
        string fileName = Path.Combine(Path.GetTempPath(), $"FF9.Tests.{Guid.NewGuid():N}.json");

        try
        {
            DataContext context = new();

            context.Save(fileName);

            Assert.False(File.Exists(fileName));
        }
        finally
        {
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
        }
    }

    [Fact]
    public void Save_WritesFormattedJson()
    {
        string fileName = Path.Combine(Path.GetTempPath(), $"FF9.Tests.{Guid.NewGuid():N}.json");

        try
        {
            DataContext context = new()
            {
                Json = new RootObject
                {
                    Data = new Data
                    {
                        __invalid_name__40000_Common = new __invalid_type__40000Common
                        {
                            gil = "321",
                            items =
                            [
                                new Item { id = "1", count = "2" }
                            ]
                        },
                        __invalid_name__91000_State =
                        [
                            "state"
                        ]
                    }
                }
            };

            context.Save(fileName);

            string text = File.ReadAllText(fileName);
            Assert.True(text.Contains(", "));
            Assert.True(text.Contains("[ {"));
            Assert.True(text.Contains("} ]"));
            Assert.True(text.Contains("[ \"state\" ]"));
        }
        finally
        {
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
        }
    }

    [Fact]
    public void GetCardsSection_WhenJsonIsNull_ReturnsEmptyList()
    {
        DataContext context = new("nonexistent_file.json");

        List<MiniGameCard> cards = context.GetCardsSection();

        Assert.Equal(0, cards.Count);
    }

    [Fact]
    public void SetCardsSection_WhenJsonIsNull_ReturnsFalse()
    {
        DataContext context = new("nonexistent_file.json");

        bool result = context.SetCardsSection(new List<MiniGameCard>());

        Assert.False(result);
    }

    [Fact]
    public void GetItemsSection_WhenJsonIsNull_ReturnsEmptyList()
    {
        DataContext context = new("nonexistent_file.json");

        List<Item> items = context.GetItemsSection();

        Assert.Equal(0, items.Count);
    }

    [Fact]
    public void SetItemsSection_WhenJsonIsNull_ReturnsFalse()
    {
        DataContext context = new("nonexistent_file.json");

        bool result = context.SetItemsSection(new List<Item>());

        Assert.False(result);
    }
}

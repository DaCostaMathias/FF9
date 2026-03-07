using System;
using System.IO;

namespace FF9.Tests;

[TestClass]
public sealed class DataContextTests
{
    [TestMethod]
    public void DefaultConstructor_UsesSingletonAppInfo()
    {
        DataContext context = new();

        Assert.AreSame(AppInfo.Info, context.Info);
        Assert.IsNull(context.Json);
    }

    [TestMethod]
    public void FileConstructor_LeavesJsonNullWhenFileDoesNotExist()
    {
        DataContext context = new(Path.Combine(Path.GetTempPath(), $"missing-{Guid.NewGuid():N}.json"));

        Assert.IsNull(context.Json);
    }

    [TestMethod]
    public void FileConstructor_LoadsJsonFromDisk()
    {
        string fileName = Path.Combine(Path.GetTempPath(), $"FF9.Tests.{Guid.NewGuid():N}.json");

        try
        {
            File.WriteAllText(fileName, "{\"Data\":{\"40000_Common\":{\"gil\":\"99\"},\"91000_State\":[\"state\"]}}");

            DataContext context = new(fileName);

            Assert.IsNotNull(context.Json);
            Assert.AreEqual("99", context.Json.Data.__invalid_name__40000_Common.gil);
            CollectionAssert.AreEqual(new[] { "state" }, context.Json.Data.__invalid_name__91000_State);
        }
        finally
        {
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
        }
    }

    [TestMethod]
    public void Save_DoesNothingWhenJsonIsNull()
    {
        string fileName = Path.Combine(Path.GetTempPath(), $"FF9.Tests.{Guid.NewGuid():N}.json");

        try
        {
            DataContext context = new();

            context.Save(fileName);

            Assert.IsFalse(File.Exists(fileName));
        }
        finally
        {
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
        }
    }

    [TestMethod]
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
            Assert.IsTrue(text.Contains(", "));
            Assert.IsTrue(text.Contains("[ {"));
            Assert.IsTrue(text.Contains("} ]"));
            Assert.IsTrue(text.Contains("[ \"state\" ]"));
        }
        finally
        {
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
        }
    }
}

using System.IO;

namespace FF9.Tests;

[TestClass]
public sealed class TestAssemblyHooks
{
    [AssemblyInitialize]
    public static void Initialize(TestContext _)
    {
        Directory.SetCurrentDirectory(AppContext.BaseDirectory);
    }
}

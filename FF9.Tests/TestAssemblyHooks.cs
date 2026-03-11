using System.IO;
using System.Runtime.CompilerServices;

namespace FF9.Tests;

public static class TestAssemblyHooks
{
    [ModuleInitializer]
    public static void Initialize()
    {
        Directory.SetCurrentDirectory(AppContext.BaseDirectory);
    }
}

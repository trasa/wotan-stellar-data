using Xunit.Abstractions;

namespace WotanStellar.Test.Extensions;

public static class TestOutputHelperExtensions
{
    public static XunitLogger<T> BuildLoggerFor<T>(this ITestOutputHelper outputHelper)
    {
        return new XunitLogger<T>(outputHelper);
    }
}

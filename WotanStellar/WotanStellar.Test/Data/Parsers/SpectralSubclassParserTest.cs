using WotanStellar.Data.Parsers;
using Xunit.Abstractions;

namespace WotanStellar.Test.Data.Parsers;

public class SpectralSubclassParserTest
{
    private readonly SpectralSubclassParser _subclassParser;

    public SpectralSubclassParserTest(ITestOutputHelper outputHelper)
    {
        _subclassParser  = new SpectralSubclassParser(/*outputHelper.BuildLoggerFor<SpectralSubclassParser>()*/);
    }

    [Theory]
    [InlineData("5 V", 5.0, "V")]
    [InlineData("", null, "")]
    [InlineData("5.5+dM+dM", 5.5, "+dM+dM")]
    [InlineData("7.4", 7.4, "")]
    [InlineData("7+DA7", 7.0, "+DA7")]
    public void ParseValidSubclasses(string input, double? expectedValue, string expectedRemainder)
    {
        var (value, remainder) = _subclassParser.Parse(input);
        Assert.Equal(expectedValue, value);
        Assert.Equal(expectedRemainder, remainder);
    }

}

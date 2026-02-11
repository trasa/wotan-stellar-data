using WotanStellar.Data.Parsers;

namespace WotanStellar.Test.Data.Parsers;

public class LuminosityParserTest
{
    private readonly LuminosityParser _parser;

    public LuminosityParserTest()
    {
        _parser = new LuminosityParser();
    }

    [Theory]
    [InlineData("0", "0", "")]
    [InlineData("Ia+", "Ia+", "")]
    [InlineData("Ia", "Ia", "")]
    [InlineData("Iab", "Iab", "")]
    [InlineData("Ib", "Ib", "")]
    [InlineData("II", "II", "")]
    [InlineData("III", "III", "")]
    [InlineData("IV", "IV", "")]
    [InlineData("V", "V", "")]
    [InlineData("IV-V", "IV-V", "")]
    [InlineData("VI", "VI", "")]
    [InlineData("VII", "VII", "")]
    [InlineData("V+DA7", "V", "+DA7")]  // TODO this case?
    [InlineData(" V", "V", "")]
    [InlineData("", null, "")]
    public void ParseValidLuminosityClasses(string input, string? expectedLuminosity, string expectedRemainder)
    {
        // two extra cases:
        // sd prefix (subdwarf) or VI which is not a luminosity class but can appear before it, e.g. "sdB5" or "sdB5VI"
        // D prefix (dwarf) or VII - "DZ8"
        var (luminosity, remainder) = _parser.Parse(input);
        Assert.Equal(expectedLuminosity, luminosity);
        Assert.Equal(expectedRemainder, remainder);
    }
}

using WotanStellar.Data.Entities;
/*

   DA3+DC10

   DA5.5+dM+dM

       DA7.3+dM

   DA7+DA5
       DA7+DA7

   DAH+DC

   DC9+DC
       DC9+DC+M VI

       DZ9+DB
 */
using WotanStellar.Data.Parsers;
using Xunit.Abstractions;


namespace WotanStellar.Test.Data.Parsers;

public class SpectralTypeParserTest
{
    private readonly SpectralTypeParser _parser;

    public SpectralTypeParserTest(ITestOutputHelper outputHelper)
    {
        var subclassParser = new SpectralSubclassParser();
        var luminosityParser = new LuminosityParser();
        var degenerateParser = new DegenerateParser();
        _parser = new SpectralTypeParser(outputHelper.BuildLoggerFor<SpectralTypeParser>(),
            subclassParser, luminosityParser, degenerateParser);
    }

    [Theory]
    [InlineData("G2 V", 'G', 2.0, "V")]
    [InlineData("M5 III", 'M', 5.0, "III")]
    [InlineData("M5.5 V", 'M', 5.5, "V")]
    [InlineData("A0Ia", 'A', 0.0, "Ia")]
    [InlineData("K7IV", 'K', 7.0, "IV")]
    [InlineData("A1 V", 'A', 1.0, "V")]
    [InlineData("A7 IV-V", 'A', 7.0, "IV-V")]
    [InlineData("F5 IV-V", 'F', 5.0, "IV-V")]
    [InlineData("G8 IV", 'G', 8.0, "IV")]
    [InlineData("G8.5 V", 'G', 8.5, "V")]
    [InlineData("K V", 'K', null, "V")]
    [InlineData("K V?", 'K', null, "V")]
    [InlineData("K VI?", 'K', null, "VI")]
    [InlineData("K0 V", 'K', 0.0, "V")]
    [InlineData("L7.5 V", 'L', 7.5, "V")]
    [InlineData("M4.5Ve", 'M', 4.5, "V")]
    [InlineData("T1 V", 'T', 1.0, "V")]
    public void ParseMainSequence(string spectralType, char expectedClass, double? expectedSubclass, string expectedLuminosityClass)
    {
        var starClass = _parser.Parse(spectralType);

        var mainSequence = Assert.IsType<MainSequenceSpectralType>(starClass);
        Assert.Equal(expectedClass, mainSequence.SpectralClass);
        Assert.Equal(expectedSubclass, mainSequence.SpectralSubClass);
        Assert.Equal(expectedLuminosityClass, mainSequence.LuminosityClass);
    }

    [Theory]
    [InlineData("DA", null)]
    [InlineData("DA10", 10.0)]
    [InlineData("DA8", 8.0)]
    [InlineData("DA10.5", 10.5)]
    [InlineData("DAH9.5", 9.5)]
    [InlineData("DAV4", 4.0)]
    [InlineData("DC10.2", 10.2)]

    public void ParseDegenerate(string spectralType, double? expectedClass)
    {
        var result = _parser.Parse(spectralType);
        Assert.NotNull(result);
        var d = Assert.IsType<DegenerateSpectralType>(result);
        Assert.Equal(expectedClass, d.TemperatureClass);
    }

    [Fact]
    public void ParseMainSequenceCompound()
    {
        var result = _parser.Parse("K7 VI/M1 VI");
        var compound = Assert.IsType<CompoundSpectralType>(result);
        Assert.Equal(2, compound.Components.Count);

        var main1 = Assert.IsType<MainSequenceSpectralType>(compound.Components[0]);
        Assert.Equal('K', main1.SpectralClass);
        Assert.Equal(7.0, main1.SpectralSubClass);
        Assert.Equal("VI", main1.LuminosityClass);

        var main2 = Assert.IsType<MainSequenceSpectralType>(compound.Components[1]);
        Assert.Equal('M', main2.SpectralClass);
        Assert.Equal(1.0, main2.SpectralSubClass);
        Assert.Equal("VI", main2.LuminosityClass);
    }

    [Theory]
    [InlineData("DA3+DC10", 3.0)]
    [InlineData("DA5.5+dM+dM", 5.5)]
    [InlineData(     "DC9+DC+M VI", 9.0)]
    public void ParseDegenerateCompound(string spectralType, double? expectedClass)
    {
        var result = _parser.Parse(spectralType);
        Assert.NotNull(result);
        var d = Assert.IsType<DegenerateSpectralType>(result);
        Assert.Equal(expectedClass, d.TemperatureClass);
    }
}

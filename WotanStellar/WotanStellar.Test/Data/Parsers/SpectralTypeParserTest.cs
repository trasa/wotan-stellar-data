using WotanStellar.Data.Entities;
/*
         K7 VI/M1 VI

       DA
   DA10
       DA10.5
   DA11
       DA2
   DA3.3
   DA3.5
   DA3+DC10
       DA4
   DA4.2
   DA4.5
   DA4.7
   DA5
       DA5.5
   DA5.5+dM+dM
       DA5.8
   DA5.9
   DA58.6
   DA6
       DA6.1
   DA6.5
   DA6.6
   DA6.7
   DA7
       DA7.3+dM
       DA7.4
   DA7.5
   DA7.7
   DA7.8
   DA7+DA5
       DA7+DA7
       DA8
   DA8.1
   DA8.5
   DA8.6
   DA8.7
   DA9
       DA9.2
   DA9.5
   DA9.8
   DAH+DC
       DAH9.5
   DAP3.2
   DAP3.4
   DAP8
       DAP9
   DAV4
       DAV4.1
   DAV4.6
   DAZ7
       DAZ8
   DBH7
       DC
   DC10.2
   DC10.5
   DC10.9
   DC5
       DC7
   DC7.9
   DC8
       DC8.8
   DC9
       DC9.9
   DC9+DC
       DC9+DC+M VI
   DCP7
       DCP9
   DQ10.2
   DQ4
       DQ5
   DQ6
       DQ7
   DQ7.9
   DQ8
       DQ9
   DQZ
       DXP9
   DZ6
       DZ7
   DZ7.8
   DZ9
       DZ9+DB
       DZA7.7
   DZQ
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
        _parser = new SpectralTypeParser(outputHelper.BuildLoggerFor<SpectralTypeParser>(),
            subclassParser, luminosityParser);
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
}

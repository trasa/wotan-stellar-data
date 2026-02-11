using Microsoft.Extensions.Logging;
using WotanStellar.Data.Calculators;
using WotanStellar.Data.Parsers;
using Xunit.Abstractions;

namespace WotanStellar.Test.Calculators;

public class LuminosityCalculatorTest
{
    private readonly SpectralTypeParser _parser;
    private readonly LuminosityCalculator _calculator;
    private readonly ILogger<LuminosityCalculatorTest> _logger;
    public LuminosityCalculatorTest(ITestOutputHelper testOutputHelper)
    {
        _parser =  new SpectralTypeParser(testOutputHelper.BuildLoggerFor<SpectralTypeParser>(),
            new SpectralSubclassParser(), new LuminosityParser(), new DegenerateParser());
        _logger = testOutputHelper.BuildLoggerFor<LuminosityCalculatorTest>();
        _calculator = new LuminosityCalculator();
    }

    [Theory]
    [InlineData("G2 V")]
    [InlineData("M5 III")]
    [InlineData("M5.5 V")]
    [InlineData("A0Ia")]
    [InlineData("K7IV")]
    [InlineData("A1 V")]
    [InlineData("A7 IV-V")]
    [InlineData("F5 IV-V")]
    [InlineData("G8 IV")]
    [InlineData("G8.5 V")]
    [InlineData("K V")]
    [InlineData("K V?")]
    [InlineData("K VI?")]
    [InlineData("K0 V")]
    [InlineData("L7.5 V")]
    [InlineData("M4.5Ve")]
    [InlineData("T1 V")]
    [InlineData("DA")]
    [InlineData("DA10")]
    [InlineData("DA8")]
    [InlineData("DA10.5")]
    [InlineData("DAH9.5")]
    [InlineData("DAV4")]
    [InlineData("DC10.2")]
    [InlineData("DA3+DC10")]
    [InlineData("DA5.5+dM+dM")]
    [InlineData(     "DC9+DC+M VI")]
    [InlineData("K7 VI/M1 VI")]
    public void CalculateLuminosity(string spectralType)
    {
        var starClass = _parser.Parse(spectralType);
        var luminosity = _calculator.CalculateLuminosity(starClass);
        Assert.NotNull(luminosity);
        _logger.LogInformation("Luminosity of {SpectralType} is approximately {Luminosity} times that of the Sun.", spectralType, luminosity);
    }
}

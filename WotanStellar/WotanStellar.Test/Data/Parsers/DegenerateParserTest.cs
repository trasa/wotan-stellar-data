/*
 DA3+DC10

 DA5.5+dM+dM
     DA7.3+dM

 DA7+DA5
     DA7+DA7

 DAH+DC

 DC9+DC

     DZ9+DB
*/
using WotanStellar.Data.Parsers;

namespace WotanStellar.Test.Data.Parsers;

public class DegenerateParserTest
{
    private readonly DegenerateParser _parser;

    public DegenerateParserTest()
    {
        _parser = new DegenerateParser();
    }

    [Theory]
    [InlineData("DA", null)]
    [InlineData("DA10", 10.0)]
    [InlineData("DA8", 8.0)]
    [InlineData("DA10.5", 10.5)]
    [InlineData("DAH9.5", 9.5)]
    [InlineData("DAV4", 4.0)]
    [InlineData("DC10.2", 10.2)]
    public void Parse(string input, double? expectedClass)
    {
        var d = _parser.Parse(input);
        Assert.Equal(d.TemperatureClass, expectedClass);
    }

    [Theory]
    [InlineData("DA3+DC10", 3.0)]
    [InlineData("DA5.5+dM+dM", 5.5)]
    [InlineData(     "DC9+DC+M VI", 9.0)]
    public void ParseCompound(string input, double? expectedClass)
    {
        var d = _parser.Parse(input);
        Assert.Equal(d.TemperatureClass, expectedClass);
    }
}

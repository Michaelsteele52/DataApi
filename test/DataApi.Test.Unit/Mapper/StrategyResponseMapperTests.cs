using AutoFixture;
using DataApi.Integrations.WebClients.ThirdPartyA;
using DataApi.Integrations.WebClients.ThirdPartyB;
using DataApi.Mapper;
using FluentAssertions;

namespace DataApi.Test.Unit.Mapper;

public class StrategyResponseMapperTests
{
    private readonly StrategyResponseMapper _mapper;

    public StrategyResponseMapperTests()
    {
        _mapper = new StrategyResponseMapper();
    }
    
    [Fact]
    public void ShouldMapThirdPartyAData()
    {
        //Arrange
        var thirdPartyAData = new Fixture().Create<ThirdPartyACompanyDataResponse>();
        //Act
        var response = _mapper.Map(thirdPartyAData);
        //Assert
        response.CompanyInfoResponse?.CompanyName.Should().Be(thirdPartyAData.CompanyName);
    }
    
    [Fact]
    public void ShouldMapThirdPartyBData()
    {
        //Arrange
        var thirdPartyBData = new Fixture().Create<ThirdPartyBCompanyDataResponse>();
        //Act
        var response = _mapper.Map(thirdPartyBData);
        //Assert
        response.CompanyInfoResponse?.CompanyName.Should().Be(thirdPartyBData.CompanyName);
    }

    [Fact]
    public void ShouldMapCombinedThirdPartyData()
    {
        //Arrange
        var thirdPartyAData = new Fixture().Create<ThirdPartyACompanyDataResponse>();
        var thirdPartyBData = new Fixture().Create<ThirdPartyBCompanyDataResponse>();
        //Act
        var response = _mapper.Map(thirdPartyAData, thirdPartyBData);
        //Assert
        response.CompanyInfoResponse?.CompanyName.Should().Be(thirdPartyAData.CompanyName);
    }
}
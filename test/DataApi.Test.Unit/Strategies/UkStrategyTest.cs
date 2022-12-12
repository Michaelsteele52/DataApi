using AutoFixture;
using DataApi.Configuration;
using DataApi.Integrations.WebClients.ThirdPartyA;
using DataApi.Mapper;
using DataApi.Models.Response;
using DataApi.Strategies;
using FluentAssertions;
using Moq;

namespace DataApi.Test.Unit.Strategies;

public class UkStrategyTest
{
    private UkStrategy _sut;
    private Mock<IThirdPartyAClient> _thirdPartyAClientMock;
    private Mock<IStrategyResponseMapper> _strategyResponseMapperMock;
    private CompanyIdConfiguration _companyIdConfiguration;
    private ThirdPartyACompanyDataResponse _thirdPartyACompanyDataResponse;
    
    public UkStrategyTest()
    {
        _thirdPartyAClientMock = new Mock<IThirdPartyAClient>();
        _strategyResponseMapperMock = new Mock<IStrategyResponseMapper>();
        _companyIdConfiguration = new CompanyIdConfiguration();
        _thirdPartyACompanyDataResponse = new Fixture().Create<ThirdPartyACompanyDataResponse>();
    }
    
    [Theory]
    [InlineData(false, 321, "Uk", true)]
    [InlineData(true, 123, "uk", true)]
    [InlineData(true, 321, "uk", false)]
    [InlineData(false, 321, "somewhereElse", false)]
    public void ShouldReturnIsSupported(bool companyIdValidationFlag, int companyId, string jurisdictionCode, bool isSupported)
    {
        //Arrange
        _companyIdConfiguration.CompanyIdValidation = companyIdValidationFlag;
        _companyIdConfiguration.CompanyIdJurisdictionDictionary = new Dictionary<string, IEnumerable<int>?>() { {"Uk", new []{123}} };
        _sut = new UkStrategy(_thirdPartyAClientMock.Object, _strategyResponseMapperMock.Object, _companyIdConfiguration);

        //Act
        var response = _sut.IsSupported(jurisdictionCode, companyId);

        //Assert
        response.Should().Be(isSupported);
    }

    [Fact]
    public async Task GetInfoReturnsStrategyResponse()
    {
        //Arrange
        _thirdPartyAClientMock
            .Setup(x => x.GetCompanyData(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(_thirdPartyACompanyDataResponse);
        _strategyResponseMapperMock.Setup(x => x.Map(_thirdPartyACompanyDataResponse)).Returns(new StrategyResponse());
        
        _sut = new UkStrategy(_thirdPartyAClientMock.Object, _strategyResponseMapperMock.Object, _companyIdConfiguration);
        //Act
        var response = await _sut.GetInfo(123, CancellationToken.None);

        //Assert
        response.Should().NotBeNull();
    }
}
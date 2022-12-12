using AutoFixture;
using DataApi.Configuration;
using DataApi.Integrations.WebClients.ThirdPartyA;
using DataApi.Integrations.WebClients.ThirdPartyB;
using DataApi.Mapper;
using DataApi.Models.Response;
using DataApi.Strategies;
using FluentAssertions;
using Moq;

namespace DataApi.Test.Unit.Strategies;

public class DeStrategyTest
{
    private DeStrategy _sut;
    private Mock<IThirdPartyAClient> _thirdPartyAClientMock;
    private Mock<IThirdPartyBClient> _thirdPartyBClientMock;
    private Mock<IStrategyResponseMapper> _strategyResponseMapperMock;
    private CompanyIdConfiguration _companyIdConfiguration;
    private ThirdPartyACompanyDataResponse _thirdPartyACompanyDataResponse;
    private ThirdPartyBCompanyDataResponse _thirdPartyBCompanyDataResponse;
    
    public DeStrategyTest()
    {
        _thirdPartyAClientMock = new Mock<IThirdPartyAClient>();
        _thirdPartyBClientMock = new Mock<IThirdPartyBClient>();
        _strategyResponseMapperMock = new Mock<IStrategyResponseMapper>();
        _companyIdConfiguration = new CompanyIdConfiguration();
        _thirdPartyACompanyDataResponse = new Fixture().Create<ThirdPartyACompanyDataResponse>();
        _thirdPartyBCompanyDataResponse = new Fixture().Create<ThirdPartyBCompanyDataResponse>();
    }
    
    [Theory]
    [InlineData(false, 321, "De", true)]
    [InlineData(true, 123, "de", true)]
    [InlineData(true, 321, "De", false)]
    [InlineData(false, 321, "somewhereElse", false)]
    public void ShouldReturnIsSupported(bool companyIdValidationFlag, int companyId, string jurisdictionCode, bool isSupported)
    {
        //Arrange
        _companyIdConfiguration.CompanyIdValidation = companyIdValidationFlag;
        _companyIdConfiguration.CompanyIdJurisdictionDictionary = new Dictionary<string, IEnumerable<int>?>() { {"De", new []{123}} };
        _sut = new DeStrategy(_thirdPartyAClientMock.Object, _thirdPartyBClientMock.Object, _strategyResponseMapperMock.Object, _companyIdConfiguration);

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
        _thirdPartyBClientMock
            .Setup(x => x.GetCompanyData(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(_thirdPartyBCompanyDataResponse);
        
        _strategyResponseMapperMock.Setup(x => x.Map(_thirdPartyACompanyDataResponse, _thirdPartyBCompanyDataResponse)).Returns(new StrategyResponse());
        
        _sut = new DeStrategy(_thirdPartyAClientMock.Object, _thirdPartyBClientMock.Object, _strategyResponseMapperMock.Object, _companyIdConfiguration);
        //Act
        var response = await _sut.GetInfo(123, CancellationToken.None);

        //Assert
        response.Should().NotBeNull();
    }
}
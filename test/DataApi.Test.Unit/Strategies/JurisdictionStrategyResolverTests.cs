using DataApi.Strategies;
using FluentAssertions;
using Moq;

namespace DataApi.Test.Unit.Strategies;

public class JurisdictionStrategyResolverTests
{
    private readonly JurisdictionStrategyResolver _sut;
    private readonly Mock<IJurisdictionStrategy> _jurisdictionStrategyMock;
    
    public JurisdictionStrategyResolverTests()
    {
        _jurisdictionStrategyMock = new Mock<IJurisdictionStrategy>();
        _sut = new JurisdictionStrategyResolver(new List<IJurisdictionStrategy>(){_jurisdictionStrategyMock.Object});
    }
    
    [Fact]
    public void ResolveStragey()
    {
        //Arrange
        _jurisdictionStrategyMock.Setup(x => x.IsSupported(It.IsAny<string>(), It.IsAny<int>())).Returns(true);

        //Act
        var strategy = _sut.GetJurisdictionStrategy("a", 1);
        //Assert
        strategy.Should().Be(_jurisdictionStrategyMock.Object);
    }

    [Fact]
    public void ReturnNotSupportedStrategy()
    {
        //Arrange
        _jurisdictionStrategyMock.Setup(x => x.IsSupported(It.IsAny<string>(), It.IsAny<int>())).Returns(false);

        //Act
        var strategy = _sut.GetJurisdictionStrategy("a", 1);
        //Assert
        strategy.Should().BeOfType<NotSupportStrategy>();
    }
}
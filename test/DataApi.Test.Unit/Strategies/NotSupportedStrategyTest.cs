using System.Net;
using DataApi.Models.Response;
using DataApi.Strategies;
using FluentAssertions;

namespace DataApi.Test.Unit.Strategies;

public class NotSupportedStrategyTest
{
    [Fact]
    public async Task ShouldReturnProblemDetails()
    {
        //Arrange
        var strategy = new NotSupportStrategy();
        //Act&Assert
        var exception = await Assert.ThrowsAsync<JurisdictionNotSupportedException>(() => strategy.GetInfo(123, CancellationToken.None));

        //Assert
        exception.Details.Status.Should().Be((int)HttpStatusCode.UnprocessableEntity);
        exception.Details.Detail.Should().Be("This Jurisdiction is not supported");
        exception.Details.Title.Should().Be("This jurisdiction is not supported");
    }
}
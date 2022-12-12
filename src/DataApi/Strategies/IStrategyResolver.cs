namespace DataApi.Strategies;

public interface IStrategyResolver
{
    IJurisdictionStrategy GetJurisdictionStrategy(string countryCode, int companyId);
}
namespace DataApi.Strategies;

public class JurisdictionStrategyResolver : IStrategyResolver
{
    private readonly IOrderedEnumerable<IJurisdictionStrategy> _strategies;

    public JurisdictionStrategyResolver(IEnumerable<IJurisdictionStrategy> strategies)
    {
        _strategies = strategies.OrderBy(s => s.OrderNumber);
    }

    public IJurisdictionStrategy GetJurisdictionStrategy(string jurisdictionCode, int companyId)
    {
        foreach (var strategy in _strategies)
        {
            if (!strategy.IsSupported(jurisdictionCode, companyId)) continue;

            return strategy;
        }

        return new NotSupportStrategy();
    }
}
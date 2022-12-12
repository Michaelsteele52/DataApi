using DataApi.Models.Response;

namespace DataApi.Strategies;

public interface IJurisdictionStrategy
{
    int OrderNumber { get; }
    bool IsSupported(string jurisdictionCode, int companyId);
    Task<StrategyResponse> GetInfo(int companyId, CancellationToken cancellationToken);
}
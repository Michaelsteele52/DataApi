namespace DataApi.Integrations.WebClients.ThirdPartyB;

public interface IThirdPartyBClient
{
    Task<ThirdPartyBCompanyDataResponse> GetCompanyData(string countryCode, int companyId, CancellationToken cancellationToken);
}
namespace DataApi.Integrations.WebClients.ThirdPartyA;

public interface IThirdPartyAClient
{
    Task<ThirdPartyACompanyDataResponse> GetCompanyData(string countryCode, int companyId, CancellationToken cancellationToken);
}
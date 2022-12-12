namespace DataApi.Configuration;

public class CompanyIdConfiguration
{
    public bool CompanyIdValidation { get; set; }
    public Dictionary<string, IEnumerable<int>?>? CompanyIdJurisdictionDictionary { get; set; }
}
using System.Text.Json.Serialization;

namespace DataApi.Models.Response;

public class CompanyInfoResponse
{
    [JsonPropertyName("company_number")] public string? CompanyNumber { get; set; }
    [JsonPropertyName("company_name")] public string? CompanyName { get; set; }
    [JsonPropertyName("country")] public string? JurisdictionCode { get; set; }
    [JsonPropertyName("company_type")] public string? CompanyType { get; set; }
    [JsonPropertyName("status")] public string? Status { get; set; }
    [JsonPropertyName("date_established")] public string? DateEstablished { get; set; }
    [JsonPropertyName("date_dissolved")] public string? DateDissolved { get; set; }
    [JsonPropertyName("official_address")] public string? OfficialAddress { get; set; }
    [JsonPropertyName("related_persons")] public IEnumerable<People>? RelatedPersons { get; set; }
    [JsonPropertyName("related_companies")] public IEnumerable<AffiliatedCompany>? RelatedCompanies { get; set; }
}

public class People
{
    [JsonPropertyName("name")] public string? Name { get; set; }
    [JsonPropertyName("date_from")] public string? DateFrom { get; set; }
    [JsonPropertyName("date_to")] public string? DateTo { get; set; }
    [JsonPropertyName("type")] public string? Type { get; set; }
    [JsonPropertyName("date_of_birth")] public string? BirthDate { get; set; }
    [JsonPropertyName("nationality")] public string? Nationality { get; set; }
    [JsonPropertyName("ownership")] public string? Ownership { get; set; }
    [JsonPropertyName("shares_held")] public double? SharesHeld { get; set; }
}

public class AffiliatedCompany
{
    [JsonPropertyName("name")] public string? Name { get; set; }
    [JsonPropertyName("dateFrom")] public string? DateFrom { get; set; }
    [JsonPropertyName("dateTo")] public string? DateTo { get; set; }
    [JsonPropertyName("type")] public string? Type { get; set; }
    [JsonPropertyName("country")] public string? Country { get; set; }
    [JsonPropertyName("ownership")] public string? Ownership { get; set; }
}
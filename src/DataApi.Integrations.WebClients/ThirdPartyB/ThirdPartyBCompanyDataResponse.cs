using System.Text.Json.Serialization;

namespace DataApi.Integrations.WebClients.ThirdPartyB;

public record ThirdPartyBCompanyDataResponse
{
    [JsonPropertyName("companyNumber")] public string? CompanyNumber { get; set; }
    [JsonPropertyName("companyName")] public string? CompanyName { get; set; }
    [JsonPropertyName("country")] public string? Country { get; set; }
    [JsonPropertyName("dateFrom")] public string? DateFrom { get; set; }
    [JsonPropertyName("dateTo")] public string? DateTo { get; set; }
    [JsonPropertyName("address")] public string? Address { get; set; }
    [JsonPropertyName("activities")] public IReadOnlyList<Activity>? Activities { get; set; }
    [JsonPropertyName("relatedPersons")] public IReadOnlyList<RelatedPerson>? RelatedPersons { get; set; }
    [JsonPropertyName("relatedCompanies")] public IReadOnlyList<RelatedCompany>? RelatedCompanies { get; set; }
}

public record Activity
{
    [JsonPropertyName("activityCode")] public int? ActivityCode;
    [JsonPropertyName("activityDescription")] public string? ActivityDescription;
}

public record RelatedCompany
{
    [JsonPropertyName("name")] public string? Name { get; set; }
    [JsonPropertyName("dateFrom")] public string? DateFrom { get; set; }
    [JsonPropertyName("dateTo")] public string? DateTo { get; set; }
    [JsonPropertyName("type")] public string? Type { get; set; }
    [JsonPropertyName("country")] public string? Country { get; set; }
    [JsonPropertyName("ownership")] public string? Ownership { get; set; }
}

public record RelatedPerson{
    [JsonPropertyName("name")] public string? Name { get; set; }
    [JsonPropertyName("dateFrom")] public string? DateFrom { get; set; }
    [JsonPropertyName("dateTo")] public string? DateTo { get; set; }
    [JsonPropertyName("type")] public string? Type { get; set; }
    [JsonPropertyName("birthDate")] public string? BirthDate { get; set; }
    [JsonPropertyName("nationality")] public string? Nationality { get; set; }
    [JsonPropertyName("ownership")] public string? Ownership { get; set; }
}
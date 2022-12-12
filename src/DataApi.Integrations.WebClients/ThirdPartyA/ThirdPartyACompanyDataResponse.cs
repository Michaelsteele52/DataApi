using System.Text.Json.Serialization;

namespace DataApi.Integrations.WebClients.ThirdPartyA;

public record ThirdPartyACompanyDataResponse
{
    [JsonPropertyName("company_number")] public string? CompanyNumber { get; set; }
    [JsonPropertyName("company_name")] public string? CompanyName { get; set; }
    [JsonPropertyName("jurisdiction_code")] public string? JurisdictionCode { get; set; }
    [JsonPropertyName("company_type")] public string? CompanyType { get; set; }
    [JsonPropertyName("status")] public string? Status { get; set; }
    [JsonPropertyName("date_established")] public Date? DateEstablished { get; set; }
    [JsonPropertyName("date_dissolved")] public Date? DateDissolved { get; set; }
    [JsonPropertyName("official_address")] public OfficialAddress? OfficialAddress { get; set; }
    [JsonPropertyName("officers")] public IReadOnlyList<Officer>? Officers { get; set; }
    [JsonPropertyName("owners")] public IReadOnlyList<Owner>? Owners { get; set; }
}
public record Date
{
    [JsonPropertyName("year")] public int? Year { get; set; }
    [JsonPropertyName("month")] public int? Month { get; set; }
    [JsonPropertyName("day")] public int? Day { get; set; }
}

public record Officer{
    [JsonPropertyName("first_name")] public string? FirstName { get; set; }
    [JsonPropertyName("last_name")] public string? LastName { get; set; }
    [JsonPropertyName("date_from")] public Date? DateFrom { get; set; }
    [JsonPropertyName("date_to")] public Date? DateTo { get; set; }
    [JsonPropertyName("role")] public string? Role { get; set; }
    [JsonPropertyName("date_of_birth")] public Date? DateOfBirth { get; set; }
}

public record OfficialAddress{
    [JsonPropertyName("street")] public string? Street { get; set; }
    [JsonPropertyName("city")] public string? City { get; set; }
    [JsonPropertyName("country")] public string? Country { get; set; }
    [JsonPropertyName("postcode")] public string? Postcode { get; set; }
}

public record Owner{
    [JsonPropertyName("first_name")] public string? FirstName { get; set; }
    [JsonPropertyName("last_name")] public string? LastName { get; set; }
    [JsonPropertyName("date_from")] public Date? DateFrom { get; set; }
    [JsonPropertyName("date_to")] public Date? DateTo { get; set; }
    [JsonPropertyName("ownership_type")] public string? OwnershipType { get; set; }
    [JsonPropertyName("shares_held")] public double? SharesHeld { get; set; }
    [JsonPropertyName("date_of_birth")] public Date? DateOfBirth { get; set; }
}




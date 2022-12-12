using DataApi.Integrations.WebClients.ThirdPartyA;
using DataApi.Integrations.WebClients.ThirdPartyB;
using DataApi.Models.Response;

namespace DataApi.Mapper;

public class StrategyResponseMapper : IStrategyResponseMapper
{
    public StrategyResponse Map(ThirdPartyACompanyDataResponse response)
    {
        var companyInfoResponse = new CompanyInfoResponse()
        {
            CompanyNumber = response.CompanyNumber,
            CompanyName = response.CompanyName,
            JurisdictionCode = response.JurisdictionCode,
            CompanyType = response.CompanyType,
            DateDissolved = response.DateDissolved?.ToString(),
            DateEstablished = response.DateEstablished?.ToString(),
            OfficialAddress = response.OfficialAddress?.ToString(),
            Status = response.Status,
            RelatedPersons = MapTypeAPersons(response.Owners, response.Officers)
        };
        
        return new StrategyResponse
        {
            CompanyInfoResponse = companyInfoResponse
        };
    }

    public StrategyResponse Map(ThirdPartyBCompanyDataResponse response)
    {
        var companyInfoResponse = new CompanyInfoResponse()
        {
            CompanyNumber = response.CompanyNumber,
            CompanyName = response.CompanyName,
            JurisdictionCode = response.Country,
            CompanyType = ActivitiesToType(response.Activities?.Select(x => x.ActivityDescription)),
            DateDissolved = response.DateTo,
            DateEstablished = response.DateFrom,
            OfficialAddress = response.Address,
            RelatedPersons = MapTypeBPersons(response.RelatedPersons),
            RelatedCompanies = MapRelatedCompanies(response.RelatedCompanies)
        };

        return new StrategyResponse
        {
            CompanyInfoResponse = companyInfoResponse
        };
    }

    private static IEnumerable<AffiliatedCompany> MapRelatedCompanies(IEnumerable<RelatedCompany>? responseRelatedCompanies)
    {
        var affiliatedCompanies = new List<AffiliatedCompany>();
        if (responseRelatedCompanies == null || !responseRelatedCompanies.Any()) return affiliatedCompanies;

        foreach (var company in responseRelatedCompanies)
        {
            var affiliatedCompany = new AffiliatedCompany
            {
                Name = company.Name,
                DateFrom = company.DateFrom,
                DateTo = company.DateTo,
                Type = company.Type,
                Country = company.Country,
                Ownership = company.Ownership
            };
            
            affiliatedCompanies.Add(affiliatedCompany);
        }

        return affiliatedCompanies;
    }

    public StrategyResponse Map(ThirdPartyACompanyDataResponse thirdPartyACompanyDataResponse,
        ThirdPartyBCompanyDataResponse thirdPartyBCompanyDataResponse)
    {
        throw new NotImplementedException();
    }

    private static string ActivitiesToType(IEnumerable<string>? activityDescription) 
        => activityDescription != null && activityDescription.Any() ? string.Concat(activityDescription, ",") : string.Empty;

    private List<People> MapTypeAPersons(IEnumerable<Owner>? owners, IEnumerable<Officer>? officers)
    {
        var persons = MapTypeAOwnerToPersons(owners);
        var personsFromOfficers = MapTypeAOfficerToPersons(officers);
        return persons.Concat(personsFromOfficers).ToList();
    }

    private static IEnumerable<People> MapTypeAOfficerToPersons(IEnumerable<Officer>? officers)
    {
        var relatedPersons = new List<People>();
        if (officers == null || !officers.Any()) return relatedPersons;
        
        foreach (var officer in officers)
        {
            var person = new People
            {
                Name = officer.FirstName + officer.LastName,
                BirthDate = officer.DateOfBirth?.ToString(),
                DateFrom = officer.DateFrom?.ToString(),
                DateTo = officer.DateTo?.ToString(),
                Type = officer.Role
            };
            relatedPersons.Add(person);
        }

        return relatedPersons;
    }

    private static IEnumerable<People> MapTypeAOwnerToPersons(IEnumerable<Owner>? owners)
    {
        var relatedPersons = new List<People>();
        if (owners == null || !owners.Any()) return relatedPersons;
        
        foreach (var owner in owners)
        {
            var person = new People
            {
                Name = owner.FirstName + owner.LastName,
                BirthDate = owner.DateOfBirth?.ToString(),
                DateFrom = owner.DateFrom?.ToString(),
                DateTo = owner.DateTo?.ToString(),
                Type = owner.OwnershipType,
                SharesHeld = owner.SharesHeld
            };
            relatedPersons.Add(person);
        }
        return relatedPersons;
    }

    private static IEnumerable<People> MapTypeBPersons(IEnumerable<RelatedPerson>? relatedPeople)
    {
        var relatedPersons = new List<People>();
        if (relatedPeople == null || !relatedPeople.Any()) return relatedPersons;

        foreach (var person in relatedPeople)
        {
            var individual = new People
            {
                Name = person.Name,
                BirthDate = person.BirthDate,
                DateFrom = person.DateFrom,
                DateTo = person.DateTo,
                Nationality = person.Nationality,
                Ownership = person.Ownership,
                Type = person.Type
            };
            relatedPersons.Add(individual);
        }

        return relatedPersons;
    }
}
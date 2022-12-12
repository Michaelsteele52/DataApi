using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Mvc;

namespace DataApi.Models.Response;

public class JurisdictionNotSupportedException : ProblemDetailsException
{
    public JurisdictionNotSupportedException(ProblemDetails details) : base(details){}
}
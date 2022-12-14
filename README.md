# DataApi

## How to use (locally)

### Running in docker
```
docker-compose build
docker-compose up
```

## Running tests
dotnet test

## Additional information

### CompanyIdValidation feature flag
The service has a feature-flag which enables early validation of the company id and jurisdiction. This is false by default.
Not a particularly scalable solution in it's current form, hense the feature-flag.

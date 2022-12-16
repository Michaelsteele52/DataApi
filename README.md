# DataApi

## How to use (locally)

### Running in docker
```
dotnet dev-certs https -ep ${HOME}/.aspnet/https/aspnetapp.pfx -p { password here }
dotnet dev-certs https --trust

Update password in docker-compose.yaml with the password provided above ^ (likely you have certs already, if so add the password to the docker compose without running above commands.)

docker-compose build
docker-compose up
```

### Running outside of docker
```
cd src/DataApi/
dotnet run
```

https://localhost:7262/v1/company/<jurisdiction_code>/<company_id>

## Running tests
dotnet test

## Additional information

### CompanyIdValidation feature flag
The service has a feature-flag which enables early validation of the company id and jurisdiction. This is false by default.
Not a particularly scalable solution in it's current form, hense the feature-flag.

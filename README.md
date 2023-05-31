# SEP6 - De Farlige Tiger MDB
## https://farligtigermdb.azurewebsites.net/
## Teamet

| Github navn  | Rigtige navn | VIA ID |
| ------------- | ------------- | ------------- |
| Solaiman1991 | Solaiman Jalili | 304870 |
| Bimmerlynge | Mathias Lynge-Jacobsen | 304888 |
| BastianVIA | Bastian Thomsen | 305294 |
| Nissen99 | Mikkel Jacobsen | 304077 |

## Getting started
For at køre dette projekt skal man have en movie database og en TMDB Api key.
1. Klon projektet
2. [Optinal] Hvis projektet skal køres i Dev mode, skal "Backend/appsettings.json" copies og omdøbes til "appsettings.Development.json"
3. Ændre "WebApiDatabase" under connection strings i appsettings til en connection string din egen instance af databasen.
4. Ændre "TMDBApiKey" under connection strings i appsettings til jeres Api key.
5. Build og run projektet

## Additional Info
Vi har hemmelige data i denne github, som fx. connectionstring til vores Azure-database og API-nøgle til TMDB. Disse skulle selvfølgelig IKKE være her, men derimod i en Azure Key Vault, men på grund af en RBAC-tilladelsesfejl lykkedes det ikke at oprette en.

## External Dependencies
- [MediatR](https://www.nuget.org/packages/MediatR)
- [FirebaseAdmin](https://www.nuget.org/packages/FirebaseAdmin)
- [FirebaseAuthentication](https://www.nuget.org/packages/FirebaseAuthentication.net)
- [Blazorise](https://www.nuget.org/packages/Blazorise)
- [NSubstitute](https://www.nuget.org/packages/FirebaseAuthentication.net)
- [TMDbLib](https://www.nuget.org/packages/TMDbLib)
- [XUnit](https://www.nuget.org/packages/xunit)
- [NLog](https://www.nuget.org/packages/NLog)
- [Nuke](https://www.nuget.org/packages/Nuke.Common)
- [Swashbuckle](https://www.nuget.org/packages/Swashbuckle.AspNetCore)
- [AutoFixture](https://www.nuget.org/packages/AutoFixture)


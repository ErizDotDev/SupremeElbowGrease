QLESS

Prerequisites
- .NET 6.0 SDK

Technologies Used
- Database: SQL Server
- Language: C#
- ORM: Dapper
- Test Framework: Xunit
- Frontend: Blazor
- Frontend component library: MudBlazor

Additional Installation Steps:
- Create the QLESS database by running the scripts found in SourceCode/database in your SQL local instance
NOTE: There will be no need to create a database manually as the script to create the database is included in the scripts that will be run.

- Open the file SourceCode/src/QLess.Api/appsettings.json, find the node "ConnectionStrings/QLessDbConnection" and provide your local database connection string
- If there's a need to run the Integration Tests found in SourceCode/tests/QLess.Infrastructure.IntegrationTests, open the file SourceCode/QLess.Infrastructure.IntegrationTests/appsettings.json 
and find the node "ConnectionStrings/QLessDbConnection" and provide your local database connection string

How to Run
- You can open the solution in Visual Studio. Just ensure that the project src/QLess.Api is set as the Start-up Project. 
- You may also run the solution via console. On your terminal, cd to the directory of the QLess.Api project, and key in this command, "dotnet run".
- Visit https://localhost:7222 to view the output.
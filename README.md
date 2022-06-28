Instructions on how to run the project:
1. Make sure you've <a href="https://dotnet.microsoft.com/en-us/download/dotnet/5.0">installed the .NET 5 SDK.</a>
2. You can also follow the instructions <a href="https://www.c-sharpcorner.com/article/getting-started-with-net-5-0/">here</a> to run the app in Windows. 
3. <a href="https://docs.microsoft.com/en-us/troubleshoot/developer/webapps/aspnetcore/practice-troubleshoot-linux/2-1-create-configure-aspnet-core-applications">Instructions to run the application in Linux.</a>
4. Go to Noteapp > Noteapp.sln and run the program.

Instructions regarding the database:
1. The database used is a MSSQL database. To install SQL Server visit <a href="https://www.microsoft.com/en-us/sql-server/sql-server-downloads">this link.</a>
2. Create a new database for the app.
3. In the appsettings.json file of the project, replace the connection string with the accurate connection string. (You might need to simply modify the data source part and replace it with the name of your local server and the initial catalog part with the name of the database you created in step 2.)
4. Open Nuget console and enter the command: update-database
(If you're using Visual Studio as your IDE to open the Nuget console go to the Tools menu > NuGet Package Manager > Package Manager Console)
5. Wait for all migrations to apply to your database.

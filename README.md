# CwRetail.Api

### PreRequisites
- SQL Server
- C# .NET 6
- Visual Studio 2022

### Instructions

#### Database setup

1. Create a new [sql server database](https://www.sqlservertutorial.net/sql-server-basics/sql-server-create-database/) and name it `CwRetail`
2. Run [create object](https://github.com/hancyfancy/CwRetailProductManagementSolution/blob/master/CwRetail.Data/Resources/Queries/SampleDatabase-CreateObjects.sql) commands for the newly created `CwRetail` database
3. Run [load data](https://github.com/hancyfancy/CwRetailProductManagementSolution/blob/master/CwRetail.Data/Resources/Queries/SampleDatabase-LoadData.sql) commands for the newly created `CwRetail` database
4. Run [create users](https://github.com/hancyfancy/CwRetailProductManagementSolution/blob/master/CwRetail.Data/Resources/Authentication/SampleDatabase-CreateUsers.sql) commands for the newly created `CwRetail` database
5. Run [grant permissions](https://github.com/hancyfancy/CwRetailProductManagementSolution/blob/master/CwRetail.Data/Resources/Authentication/SampleDatabase-GrantPermissions.sql) commands for the newly created `CwRetail` database

#### Application setup

1. In Visual Studio, set [multiple start up](https://davecallan.com/running-multiple-projects-visual-studio/) projects, setting `CwRetail.Api` to start up first and `CwRetail.Ui` to start up last

### Running the solution

1. Highlight the solution in solution explorer
2. Click `Start` to run the solution


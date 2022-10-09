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

![Database](/assets/database.png)

![Login](/assets/login.png)

#### Application setup

In Visual Studio:
1. Set [multiple start up](https://davecallan.com/running-multiple-projects-visual-studio/) projects, setting `CwRetail.Api` to start up first and `CwRetail.Ui` to start up last

![MultipleStartUpProjects](/assets/applicationMultipleStartupProjects.png)

### Running the solution

In Visual Studio:
1. Highlight the `CwRetailProductManagement` solution in solution explorer
2. Click `Start` to run the `CwRetailProductManagement` solution

### Running the tests

In Visual Studio:
1. Navigate to the `Test` option in the toolbar
2. Select `Run All Tests`


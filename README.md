# CwRetail Product Management Solution

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

Demo:

[![Videos](https://img.youtube.com/vi/aiMErjpf4Ok/0.jpg)](https://www.youtube.com/playlist?list=PLVDeXGDdMhfZSX3dSLe3f9Y3zlMOzt5-Z)

### Running the tests

In Visual Studio:
1. Navigate to the `Test` option in the toolbar
2. Select `Run All Tests`

### Todo
3. Add authorisation for each controller api action **[Completed, to be deployed]**
    1. Need to test built functionality
    2. Need to ensure encrypted user is retrieved properly in ProductController and ProductAuditController
    3. Need to create data repository specifically for each api, offering limited calls to the database, only the calls which are consumed by the api should be offered by the repository
4. Make controller api action async
6. Check if mocked test cases need to be ammended
7. Write another frontend using react typescript
9. Change product id to string instead of long for better security
10. Use async/await instead of then/catch in angular
11. Use streaming to push notifications to app, such as an insert or delete notification (Return inserted or deleted rows from dml triggers)
12. Inject repositories as dependencies to controllers instead of creating new repository instances **[Done, only for projects where Program.cs file is available, such as Api projects]**
13. Split authentication and authorisation into separate solutions **[Done]**
14. Need to limit how many email and sms messages users can send

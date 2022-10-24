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
1. Change controller api actions to return IActionResult [Done]
2. Cater for invalid data in controller api actions [Done]
3. Add authorisation for each controller api action [In progress]
    1. Need to create AuthenticationController action method Create which would consume UserRepository and UserVerificationRepository
    2. Need to create angular component Register to hook in to  AuthenticationController.Create action method
    3. After inserting data into auth.users and auth.userverification need to send email to registered email address, need to send text message to registered phone
    4. User must verify both via link in email and text message
    5. As long as either Email or Phone is verified, user may continue using the application
4. Make controller api action async
5. Show history of updates sql statements via audit.products table for a particular product id [Done]
6. Check if mocked test cases need to be ammended
7. Write another frontend using react typescript
8. Move internal list of products to product service [Done]
9. Change product id to string instead of long for better security
10. Use async/await instead of then/catch in angular
11. Use streaming to push notifications to app, such as an insert or delete notification

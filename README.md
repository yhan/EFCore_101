
# Devart

1. Install packages
   - devart.data.postgresql.efcore\7.20.1836
   - microsoft.entityframeworkcore.design\5.0.3
   - microsoft.entityframeworkcore.tools\5.0.3

1. Install tool 
   ```
   # Disabled finexkap nuget source, otherwise 401
   dotnet tool install --global dotnet-ef

   # Proved "not working", stackoverflow when adding initial migration
   dotnet tool install --global dotnet-ef --version 6.0.0-preview.1.21102.2
   ```

   State check
   ```
   PS C:\Users\hanyi> dotnet ef

                     _/\__
               ---==/    \\
         ___  ___   |.    \|\
        | __|| __|  |  )   \\\
        | _| | _|   \_/ |  //|\\
        |___||_|       /   \\\/\\

   Entity Framework Core .NET Command-line Tools 6.0.0-preview.1.21102.2
   ```
1. Create initial migration

    ```.net core cli
    dotnet ef migrations add InitialCreate
    ```

1. Try to update DB
   ```
   PS E:\finexkap\poc\ConsoleApp.EF\ConsoleApp.EF> dotnet ef database update
    Build started...
    Build succeeded.
    An error occurred using the connection to database 'StoreDB' on server 'localhost'.
    Devart.Data.PostgreSql.PgSqlException (0x80004005): database "StoreDB" does not exist
       at ♣? .☻(PgSqlConnection ☻)
       at ♣? ..ctor(☻?  ☻, PgSqlConnection ♥)
       at ☼? .☻  ?☻(♥  ☻, Object ♥, DbConnectionBase ♣)
       at ☻ .☻(♣  ☻, ♥  ♥, DbConnectionBase ♣)
       at .☻(♣  ☻, DbConnectionBase ♥)
       at ♣ .♥(DbConnectionBase ☻)
       at ♣ .☻(DbConnectionBase ☻)
       at ☻ .♥(DbConnectionBase ☻)
       at ☼ .♫  ?☻(DbConnectionBase ☻)
       at Devart.Common.DbConnectionBase.Open()
       at Devart.Data.PostgreSql.PgSqlConnection.Open()
       at Devart.Common.Entity.c1.Open()
       at Microsoft.EntityFrameworkCore.Storage.RelationalConnection.OpenDbConnection(Boolean errorsExpected)
       at Microsoft.EntityFrameworkCore.Storage.RelationalConnection.OpenInternal(Boolean errorsExpected)
       at Microsoft.EntityFrameworkCore.Storage.RelationalConnection.Open(Boolean errorsExpected)
       at Microsoft.EntityFrameworkCore.Storage.RelationalCommand.ExecuteScalar(RelationalCommandParameterObject parameterObject)
       at Microsoft.EntityFrameworkCore.Migrations.HistoryRepository.Exists()
       at Microsoft.EntityFrameworkCore.Migrations.Internal.Migrator.Migrate(String targetMigration)
       at Microsoft.EntityFrameworkCore.Design.Internal.MigrationsOperations.UpdateDatabase(String targetMigration, String connectionString, String contextType)
       at Microsoft.EntityFrameworkCore.Design.OperationExecutor.UpdateDatabaseImpl(String targetMigration, String connectionString, String contextType)
       at Microsoft.EntityFrameworkCore.Design.OperationExecutor.UpdateDatabase.<>c__DisplayClass0_0.<.ctor>b__0()
       at Microsoft.EntityFrameworkCore.Design.OperationExecutor.OperationBase.Execute(Action action)
    database "StoreDB" does not exist
   ```

1. Create manually `StoreDB` dababase

1. Update DB failed
   ```
   PS E:\finexkap\poc\ConsoleApp.EF\ConsoleApp.EF> dotnet ef database update
    Build started...
    Build succeeded.
    Applying migration '20210226140246_InitialCreate'.
    An error occurred using the connection to database 'StoreDB' on server 'localhost'.
    Devart.Data.PostgreSql.PgSqlException (0x80004005): Unexpected server response.
       at? ?  ?☻(Char ☻, Boolean ♥, Boolean ♣)
       at ♫? ?  ?☻(Char ☻, Boolean ♥, Boolean ♣)
       at? .☻(Boolean ☻, Boolean ♥, Char ♣, Boolean)
       at? .♥(Boolean ☻, Boolean ♥)
       at? .♣(Boolean ☻)
       at? .☻(☻   ☻, Boolean ♥, Boolean ♣, Boolean)
       at? .☻(☻   ☻, Boolean ♥, Boolean ♣)
       at? .☻(☻   ☻, Boolean ♥)
       at ☻  .☻()
       at ♣? .☻(☻   ☻, Boolean ♥)
       at ♣? .♫()
       at ♣? .♫  ?☼()
       at ♫ .☻()
       at ☻ .♥(DbConnectionBase ☻)
       at ☼ .♫  ?☻(DbConnectionBase ☻)
       at Devart.Common.DbConnectionBase.Open()
       at Devart.Data.PostgreSql.PgSqlConnection.Open()
       at Devart.Common.Entity.c1.Open()
       at Microsoft.EntityFrameworkCore.Storage.RelationalConnection.OpenDbConnection(Boolean errorsExpected)
       at Microsoft.EntityFrameworkCore.Storage.RelationalConnection.OpenInternal(Boolean errorsExpected)
       at Microsoft.EntityFrameworkCore.Storage.RelationalConnection.Open(Boolean errorsExpected)
       at Microsoft.EntityFrameworkCore.Migrations.Internal.MigrationCommandExecutor.ExecuteNonQuery(IEnumerable`1 migrationCommands, IRelationalConnection connection)
       at Microsoft.EntityFrameworkCore.Migrations.Internal.Migrator.Migrate(String targetMigration)
       at Microsoft.EntityFrameworkCore.Design.Internal.MigrationsOperations.UpdateDatabase(String targetMigration, String connectionString, String contextType)
       at Microsoft.EntityFrameworkCore.Design.OperationExecutor.UpdateDatabaseImpl(String targetMigration, String connectionString, String contextType)
       at Microsoft.EntityFrameworkCore.Design.OperationExecutor.UpdateDatabase.<>c__DisplayClass0_0.<.ctor>b__0()
       at Microsoft.EntityFrameworkCore.Design.OperationExecutor.OperationBase.Execute(Action action)
       Unexpected server response.
   ```

1. Keep the nuget packages dependencies like this:
  ```csharp
   <PackageReference Include="Devart.Data" Version="5.0.2262" />
   <PackageReference Include="Devart.Data.Linq" Version="4.9.1807" />
   <PackageReference Include="Devart.Data.PostgreSql" Version="7.14.1470" />
   <PackageReference Include="Devart.Data.PostgreSql.EFCore" Version="7.14.1470" />
   <PackageReference Include="Devart.Data.PostgreSql.Linq" Version="4.9.1807" />
  ```
1. Use connection string 
  ```csharp
  User Id=postgres;Password=root;Host=localhost;Database={DbName};Port=5433;Persist Security Info=True;Initial Schema={Schema};Unicode=True;License Key=wMnQAqtt0hxxsDqJWvW9KxIj97Gok/73AlbVrLT0DD/uMFfe0qmajvZ7hwGLdgDsU0YXAbKCsQdefn/Tk2w4hP6ayVO14kefgaCTThN0Pserh6XISmNJiiEYrDxw/mlJfIbmJZovZts6ABoyLNOe24T/xWFPBVcL/dA8irfcBzxMXYEdH/IHfWenC2oN4DOh7lPTxS8zLJP7KXn1u85/8B6VoQtWnxcwDcLSKYfXHZGoVidjLYm8LiWYB7hTGU3dgx0AQsbDJN1CVVvXyC9BA/mWxvV+LRnXRYzr5jTW50U=
  ```
  Now,
  - `dotnet ef migrations` commands OK
  - `dotnet ef database` commands OK
  - `DbContext` reading OK


# Npgsql
1. Install packages
   - microsoft.entityframeworkcore.design\6.0.0-preview.1.21102.2
   - npgsql.entityframeworkcore.postgresql\6.0.0-preview1

1. Create manually `StoreDB` dababase

1. Create initial migration

    ```.net core cli
    dotnet ef migrations add InitialCreate
    ```
1. Update DB
    ```
    S E:\finexkap\poc\ConsoleApp.EF\ConsoleApp.EF.Npgsql> dotnet ef database update
    Build started...
    Build succeeded.
    Applying migration '20210226153830_InitialCreate'.
    Done.
    ```

    Great!

## General

1. I wanna revert already applied migration from DB
   ```
   dotnet ef database update {PreviousMigrationName}

   # Then, remove the last migration from code
   dotnet ef migrations remove

   ```
   > you can't remove a migration already applied (`update`) to DB
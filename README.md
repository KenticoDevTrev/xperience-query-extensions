This package provides a set of extension methods for Xperience by Kentico `ContentTypeQueryParameters`, `ContentItemQueryBuilder`, `WhereParameters`, `ObjectQuery`, and `ConnectionHelper`'s returned `IDataReader` / `DbDataReader` [data access APIs](https://docs.kentico.com/developers-and-admins/api).

## Dependencies

This package is compatible with ASP.NET Core 8 applications or libraries integrated with Xperience by Kentico Versions 29.5.2 +.

## Library Version Matrix

| Xperience Version | Library Version |
| ----------------- | --------------- |
| >= 30.0.0         | 3.x             |
|    29.7.*         | 2.1             |

## How to Use?

1. Install the NuGet package in your ASP.NET Core project (or class library)

   ```bash
   dotnet add package XperienceCommunity.DevTools.QueryExtensions
   ```

1. Add the correct `using` to have the extensions appear in intellisense

   `using XperienceCommunity.QueryExtensions.ContentItems;`

   `using XperienceCommunity.QueryExtensions.Objects;`

   `using XperienceCommunity.QueryExtensions.Collections;`

   > The extension methods are all in explicit namespaces to prevent conflicts with extensions that Xperience might add in the future or extensions that the developer might have already created.
   >
   > You can apply these globally with [C# implicit usings](https://docs.microsoft.com/en-us/dotnet/core/project-sdk/overview#implicit-using-directives)

## Extension Methods

### ObjectQuery

#### Prerequisites

```csharp
using XperienceCommunity.QueryExtensions.Objects;
```

#### Examples

```csharp
return UserInfo.Provider.Get()
    .Tap(q =>
    {
        // access the query
    });
```

```csharp
bool condition = ...

var query = UserInfo.Provider.Get()
    .If(condition, q => 
    {
        // when condition is true
    });
```

```csharp
bool condition = ...

var query = UserInfo.Provider.Get()
    .If(condition, 
    q => 
    {
        // when condition is true
    }, 
    q =>
    {
        // when condition is false
    });
```

```csharp
var query = UserInfo.Provider.Get()
    .OrderByDescending(nameof(UserInfo.UserLastModified))
    .TopN(1)
    .DebugQuery();

/*
--- BEGIN [path\to\your\app\Program.cs] QUERY ---


SELECT TOP 1 *
FROM CMS_User
ORDER BY UserLastModified DESC


--- END [path\to\your\app\Program.cs] QUERY ---
*/
```

```csharp
var query = UserInfo.Provider.Get()
    .OrderByDescending(nameof(UserInfo.UserLastModified))
    .TopN(1)
    .DebugQuery("User");

/*
--- QUERY [User] START ---


SELECT TOP 1 *
FROM CMS_User
ORDER BY UserLastModified DESC


--- QUERY [User] END ---
*/
```

```csharp
public void QueryDatabase(ILogger logger)
{
    var query = UserInfo.Provider.Get()
        .OrderByDescending(nameof(UserInfo.UserLastModified))
        .TopN(1)
        .LogQuery(logger, "Logged User Query");
}
```

```csharp
var query = UserInfo.Provider.Get()
    .TapQueryText(text =>
    {
        // do something with the query text
    });
```

```csharp
var query = UserInfo.Provider.Get()
    .Source(s => s.InnerJoin<UserSettingInfo>(
        "UserID", 
        "UserSettingUserID", 
        "MY_ALIAS",
        additionalCondition: new WhereCondition("MY_ALIAS.UserWaitingForApproval", QueryOperator.Equals, true),
        hints: new[] { SqlHints.NOLOCK }))
    .TopN(1)
    .DebugQuery("User");

/*
--- QUERY [User] START ---


SELECT TOP 1 *
FROM CMS_User
INNER JOIN CMS_UserSetting AS MY_ALIAS WITH (NOLOCK) ON UserID = MY_ALIAS.UserSettingUserID AND MY_ALIAS.UserWaitingForApproval = 1
ORDER BY UserLastModified DESC


--- QUERY [User] END ---
*/
```

```csharp
// ExecuteAsync returns a populated dataset with all the columns returned by the query.
// When there are no results, dataset.Tables[0] will still be populated with an empty DataTable.

var dataset = await UserInfo.Provider.Get()
    .Source(source => source
        .InnerJoin<UserSettingInfo>(
            "UserID", 
            "UserSettingUserID", 
            "MY_ALIAS")
        .InnerJoin<CustomerInfo>(
            "CustomerUserID",
            "UserID",
            "C",
            )
        )
    .Columns("UserID", "UserSettingID", "CustomerID")
    .ExecuteAsync();
    
foreach (var row in dataset.Tables[0].Rows)
{
    Console.WriteLine($"User: {row["UserID"]}, User Setting: {row["UserSettingID"]}, Customer: {row["CustomerID"]}");
}
```


### XperienceCommunityConnectionHelper

#### Examples

```csharp
var dataSet = await XperienceCommunityConnectionHelper.ExecuteQueryAsync("CMS.User", "GetAllUsersCustom");
```

```csharp
string queryText = @"
SELECT *
FROM CMS_User
WHERE UserID = @UserID
"

var queryParams = new QueryDataParameters
{
    { "UserID", 3 }
};

var dataSet = await XperienceCommunityConnectionHelper.ExecuteQueryAsync(queryText, queryParams, token: token);
```

## References

### .NET

- [Nullable reference types (C# reference)](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/nullable-reference-types)


# BlazorSecretManager
[한국어](./readme.md) [English](./readme.en.md)  
Blazor Secret Manager is a project for managing sensitive information in Blazor applications.  
This project is designed for on-premise and VM hosting purposes.

## Reason for Creation
Although AWS Secret Manager is a secure alternative, it incurs costs after a certain number of calls.  
This project was created to minimize costs while continuously obtaining configuration information.  
When using VM hosting, it is strongly recommended to configure VNet or VPC.

[AWS SECRET MANAGER Implementation Code]
```csharp
public class AwsSecretManagerLoader : IKeyValueLoader, IKeyValueLoopStarter
{
    public ConcurrentDictionary<string, string> Data { get; } = new();
    private Task _loopTask;
    private readonly CancellationToken _cancellationToken = default;
    private readonly ILogger _logger;
    
    /// <summary>
    /// ctor
    /// </summary>
    /// <param name="logger"></param>
    public AwsSecretManagerLoader(ILogger<AwsSecretManagerLoader> logger)
    {
        _logger = logger;
    }

    public Task Start(Dictionary<string, string> parameters)
    {
        if (_loopTask.xIsNotEmpty())
        {
            throw new InvalidOperationException("This loader has already been started.");
        }
        
        _loopTask = StartLoop(parameters["KEY_ID"]
            , parameters["ACCESS_KEY"]
            , parameters["REGION"]
            , parameters["SECRET_NAME"]
            ,parameters["VERSION_STAGE"]);

        return _loopTask;
    }
    
    private async Task StartLoop(string keyId, string accessKey, string region, string secretName, string versionStage)
    {
        while (!_cancellationToken.IsCancellationRequested)
        {
            try
            {
                var client = new AmazonSecretsManagerClient(keyId, accessKey, RegionEndpoint.GetBySystemName(region));
                var request = new GetSecretValueRequest()
                {
                    SecretId = secretName,  
                    VersionStage = versionStage, // VersionStage defaults to AWSCURRENT if unspecified.
                };

                var response = await client.GetSecretValueAsync(request, _cancellationToken);
                var map = response.SecretString.ToDeserialize<Dictionary<string, Object>>();
                foreach (var keyValuePair in map)
                {
                    Data.AddOrUpdate(
                        keyValuePair.Key,
                        key => keyValuePair.Value.ToString(),
                        (key, oldValue) => keyValuePair.Value.ToString());
                }
            }
            catch (OperationCanceledException e)
            {
                _logger.LogError(e, "An operation was canceled.");
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An unexpected exception was thrown.");
                throw;
            }
            
            await Task.Delay(5000, _cancellationToken);  
        }
    }
}
```

## TODO
- [x] Sign up
- [x] Login
- [ ] Account approval
- [ ] Account rejection
- [ ] Account-to-account notifications
- [ ] Account permissions
- [ ] Account lock
- [ ] Account deletion and withdrawal
- [x] SECRET retrieval
- [x] SECRET storage
- [x] SECRET deletion
- [x] SECRET update
- [x] SECRET URL issuance
- [x] SECRET issued URL retrieval
- [ ] SECRET key regeneration
- [ ] Retrieval optimization
- [ ] Caching
- [ ] SECRET type formatting
- [ ] Apply MudComposite (ViewModel Library [MudComposite](https://github.com/nameofSEOKWONHONG/MudComposite))

## Initial Setup
1. Create a `Data` folder and an `app.db` file in the root directory (can also be a .txt file).
2. Run the EF migration command:
   ```csharp
   "C:\Program Files\dotnet\dotnet.exe" ef migrations add --project src\BlazorSecretManager.csproj --startup-project src\BlazorSecretManager.csproj --context BlazorSecretManager.SecretDbContext --configuration Debug --verbose [migration file] --output-dir Migrations
   ```
3. Execute the EF migration database update:
   ```shell
   "C:\Program Files\dotnet\dotnet.exe" ef database update --project src\BlazorSecretManager.csproj --startup-project src\BlazorSecretManager.csproj --context BlazorSecretManager.SecretDbContext --configuration Debug --verbose [migration file]
   ```

## Caution
* In release mode, some code needs to be modified.  
  The relevant sections are marked with "#if DEBUG".
* Database connection strings
* API host URLs

## Execution Images
[image1](./images/Animation1.gif)  
[image2](./images/Animation2.gif)

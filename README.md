# BlazorSecretManager
Blazor Secret Manager는 Blazor 애플리케이션에서 민감한 정보를 관리하기 위한 프로젝트입니다.   
이 프로젝트는 on premise 및 vm 호스팅을 목적으로 합니다.

## 만든 이유
AWS Secret Manager라는 안전한 대안이 있지만, 일정 호출 이후에 비용이 발생합니다.  
따라서, 비용을 최소화하고 계속적인 설정 정보를 획득하고자 하는 이유로 만들게 되었습니다.
vm 호스팅을 한다면 꼭 vnet, vpc를 구성해서 사용하기를 바랍니다.

[AWS SECRET MANAGER 구현 코드]
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
- [x] 계정 가입
- [x] 로그인
- [ ] 계정 승인
- [ ] 계정 거절
- [ ] 계정간 알람
- [ ] 계정 권한
- [ ] 계정 잠금
- [ ] 계정 삭제 및 탈퇴
- [x] SECRET 조회
- [x] SECRET 저장
- [x] SECRET 삭제
- [x] SECRET 수정
- [x] SECRET URL 발급
- [x] SECRET 발급 URL 조회
- [ ] SECRET 키 재생성
- [ ] 조회 최적화
- [ ] 캐시
- [ ] SECRET 타입 형식화
- [ ] MudComposite 적용 (ViewModel Library [MudComposite](https://github.com/nameofSEOKWONHONG/MudComposite))

## 초기 설정
1. root 기준, Data 폴더 생성 및 app.db 파일 생성. (txt파일로 생성 가능.)
2. ef migration command 실행
   ```csharp
   "C:\Program Files\dotnet\dotnet.exe" ef migrations add --project src\BlazorSecretManager.csproj --startup-project src\BlazorSecretManager.csproj --context BlazorSecretManager.SecretDbContext --configuration Debug --verbose [migration file] --output-dir Migrations
   ```
3. ef migration db update 실행
   ```shell
   "C:\Program Files\dotnet\dotnet.exe" ef database update --project src\BlazorSecretManager.csproj --startup-project src\BlazorSecretManager.csproj --context BlazorSecretManager.SecretDbContext --configuration Debug --verbose [migration file]
   ```
   
## 주의사항
* Release 모드에서는 일부 코드가 변경되어야 합니다.   
대상은 "#if DEBUG"로 작성된 부분 입니다. 
* DB 연결 문자열
* API 호스트 URL
   
## 실행 이미지
[image1](./images/Animation1.gif)
[image2](./images/Animation2.gif)
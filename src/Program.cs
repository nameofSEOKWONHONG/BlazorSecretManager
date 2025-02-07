using System.Globalization;
using BlazorSecretManager;
using Hangfire;
using Microsoft.AspNetCore.Localization;

#pragma warning disable EXTEXP0018

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMudSecretManager(() => builder.Configuration);
    
var app = builder.Build();
app.UseMudSecretManager();

// 지원할 문화권 목록 설정
// var supportedCultures = new[]
// {
//     new CultureInfo("en-US"),
//     new CultureInfo("ko-KR"),
// };
//
// // 요청된 문화권을 처리하는 미들웨어 추가
// var localizationOptions = new RequestLocalizationOptions
// {
//     DefaultRequestCulture = new RequestCulture("ko-KR"), // 기본 문화권 설정
//     SupportedCultures = supportedCultures,               // 지원할 문화권 목록 (데이터 형식 적용)
//     SupportedUICultures = supportedCultures              // 지원할 UI 문화권 목록 (리소스 적용)
// };
//
// app.UseRequestLocalization(localizationOptions);

var lifetime = app.Services.GetService<IHostApplicationLifetime>();
lifetime.ApplicationStopping.Register(() =>
{
    Console.WriteLine("Hangfire 서버를 안전하게 종료합니다...");
    var backgroundJobServer = app.Services.GetRequiredService<BackgroundJobServer>();
    backgroundJobServer.Dispose();
    Console.WriteLine("Hangfire 서버 종료 완료.");
});

await using (var scope = app.Services.CreateAsyncScope())
{
    var initializer = scope.ServiceProvider.GetRequiredService<ProgramInitializer>();
    await initializer.InitializeAsync();
}

app.Run();
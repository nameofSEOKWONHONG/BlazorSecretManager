using Microsoft.AspNetCore.Components.Server.Circuits;

namespace BlazorSecretManager.Infrastructure;

public class CustomCircuitHandler : CircuitHandler
{
    public bool IsPrerendering { get; private set; } = true;

    public override Task OnConnectionUpAsync(Circuit circuit, CancellationToken cancellationToken)
    {
        // 클라이언트 연결이 이루어진 시점
        IsPrerendering = false;
        return Task.CompletedTask;
    }

    public override Task OnConnectionDownAsync(Circuit circuit, CancellationToken cancellationToken)
    {
        // 클라이언트 연결이 종료된 시점
        return Task.CompletedTask;
    }
}
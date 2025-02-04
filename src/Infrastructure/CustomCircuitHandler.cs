using Microsoft.AspNetCore.Components.Server.Circuits;

namespace BlazorSecretManager.Infrastructure;

public class CustomCircuitHandler : CircuitHandler
{
    private readonly IUserConnectionService _userConnectionService;
    public bool IsPrerendering { get; private set; } = true;
    public string ConnectionId { get; private set; }

    public CustomCircuitHandler(IUserConnectionService userConnectionService)
    {
        _userConnectionService = userConnectionService;
    }

    public override Task OnConnectionUpAsync(Circuit circuit, CancellationToken cancellationToken)
    {
        // 클라이언트 연결이 이루어진 시점
        IsPrerendering = false;
        ConnectionId = circuit.Id;
        return Task.CompletedTask;
    }

    public override Task OnConnectionDownAsync(Circuit circuit, CancellationToken cancellationToken)
    {
        // 클라이언트 연결이 종료된 시점
        //_userConnectionService.RemoveUser(circuit.Id);
        return Task.CompletedTask;
    }
}
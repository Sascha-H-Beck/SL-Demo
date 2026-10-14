using System;
using System.Threading.Tasks;
using App.Core.Services;

namespace App.Hardware;

public class SimulationMeasurementService : IMeasurementService
{
    private readonly Random _rnd = new();
    private bool _connected;

    public Task ConnectAsync(string port)
    {
        _connected = true;
        return Task.CompletedTask;
    }

    public Task DisconnectAsync()
    {
        _connected = false;
        return Task.CompletedTask;
    }

    public Task<double> ReadCurrentValueAsync()
    {
        if (!_connected) throw new InvalidOperationException("Not connected");
        double val = 12.0 + (_rnd.NextDouble() - 0.5) * 0.6;
        return Task.FromResult(val);
    }
}

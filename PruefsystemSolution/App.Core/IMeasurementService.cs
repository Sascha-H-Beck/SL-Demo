using System.Threading.Tasks;

namespace App.Core.Services;

public interface IMeasurementService
{
    Task ConnectAsync(string port);
    Task<double> ReadCurrentValueAsync();
    Task DisconnectAsync();
}
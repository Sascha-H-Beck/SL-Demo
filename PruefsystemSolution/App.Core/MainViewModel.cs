using System.Collections.ObjectModel;

using System.Collections.ObjectModel;
using App.Core.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace App.Core.ViewModels;

// [ObservableObject] macht die Klasse automatisch zu einem ViewModel mit INotifyPropertyChanged
[ObservableObject]
public partial class MainViewModel
{
    private readonly IMeasurementService _measurementService;

    // [ObservableProperty] generiert automatisch die Properties im Hintergrund (z.B. IsMeasuring, CurrentValue)
    // Wichtig: Die Felder müssen klein geschrieben sein und mit einem Unterstrich beginnen!
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(StartMeasurementCommand))]
    [NotifyCanExecuteChangedFor(nameof(StopMeasurementCommand))]
    private bool _isMeasuring;

    [ObservableProperty]
    private double _currentValue;

    [ObservableProperty]
    private string _statusMessage = "Bereit für Messung.";

    public ObservableCollection<double> LivePlotData { get; } = new();

    public MainViewModel(IMeasurementService measurementService)
    {
        _measurementService = measurementService;
    }

    // [RelayCommand] generiert automatisch ein "StartMeasurementCommand" (ICommand) für die UI
    [RelayCommand(CanExecute = nameof(CanStart))]
    private async Task StartMeasurementAsync()
    {
        try
        {
            IsMeasuring = true;
            StatusMessage = "Verbinde mit Prüfsystem...";
            await _measurementService.ConnectAsync("COM3");
            StatusMessage = "Messung läuft...";

            while (IsMeasuring)
            {
                double val = await _measurementService.ReadCurrentValueAsync();
                CurrentValue = val;
                LivePlotData.Add(val);

                if (LivePlotData.Count > 50)
                    LivePlotData.RemoveAt(0);

                await Task.Delay(200); // 5 Hz Abtastrate
            }
        }
        catch (Exception ex)
        {
            StatusMessage = $"Fehler: {ex.Message}";
            IsMeasuring = false;
        }
    }

    [RelayCommand(CanExecute = nameof(CanStop))]
    private async Task StopMeasurementAsync()
    {
        IsMeasuring = false;
        await _measurementService.DisconnectAsync();
        StatusMessage = "Messung gestoppt. Daten validiert.";
    }

    private bool CanStart() => !IsMeasuring;
    private bool CanStop() => IsMeasuring;
}

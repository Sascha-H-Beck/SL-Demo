using System;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Input;
using App.Core.Services;
using App.Core.ViewModels;
using App.Hardware;
using ScottPlot;

namespace App.UI;

public partial class MainWindow : Window
{
    private readonly MainViewModel _viewModel;

    public MainWindow()
    {
        InitializeComponent();

        IMeasurementService simulationService = new SimulationMeasurementService();
        _viewModel = new MainViewModel(simulationService);
        this.DataContext = _viewModel;

        LivePlotControl.Plot.Title("Spannungsverlauf (Simulation)");
        LivePlotControl.Plot.XLabel("Zeitbeobachtung (letzte 50 Werte)");
        LivePlotControl.Plot.YLabel("Spannung (V)");
        LivePlotControl.Plot.Axes.SetLimitsY(10, 14); 
        LivePlotControl.Refresh();

        _viewModel.LivePlotData.CollectionChanged += OnLivePlotDataChanged;
    }

    private void StartButton_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            if (sender is System.Windows.Controls.Button btn)
            {
                var cmd = btn.Command;
                if (cmd is ICommand command && command.CanExecute(null))
                {
                    command.Execute(null);
                }
                else
                {
                    MessageBox.Show("Start command is not available or cannot execute.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Start button not found.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Fehler beim Starten der Messung: {ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void OnLivePlotDataChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        
        if (e.Action == NotifyCollectionChangedAction.Add && e.NewItems != null)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                try
                {
                    LivePlotControl.Plot.Clear();
                    // Convert current data to array and draw a simple scatter
                    var ys = new double[_viewModel.LivePlotData.Count];
                    _viewModel.LivePlotData.CopyTo(ys, 0);
                    if (ys.Length > 0)
                    {
                        var xs = new double[ys.Length];
                        for (int i = 0; i < ys.Length; i++) xs[i] = i;
                        LivePlotControl.Plot.Add.Scatter(xs, ys);
                    }
                    LivePlotControl.Refresh();
                }
                catch { LivePlotControl.Refresh(); }
            }));
        }
        // Wenn die Liste am Anfang gecleared oder gekürzt wird
        else if (e.Action == NotifyCollectionChangedAction.Remove)
        {
            // DataStream verwaltet die Begrenzung auf 50 Werte intern, hier ist kein XAML-Eingriff nötig
        }
    }
}

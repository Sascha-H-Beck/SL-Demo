using System.Collections.ObjectModel;

namespace App.Core.ViewModels
{

    public class MainViewModelDesign
    {
        public bool IsMeasuring { get; set; }
        public double CurrentValue { get; set; }
        public string StatusMessage { get; set; }
        public ObservableCollection<double> LivePlotData { get; }

        public MainViewModelDesign()
        {
            IsMeasuring = false;
            CurrentValue = 3.30;
            StatusMessage = "Bereit für Messung (Designzeit).";
            LivePlotData = new ObservableCollection<double>
            {
                3.1, 3.2, 3.3, 3.25, 3.28, 3.3
            };
        }
    }
}

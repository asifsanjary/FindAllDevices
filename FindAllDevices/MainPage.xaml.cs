using System.Collections.ObjectModel;
using Windows.Devices.Bluetooth;
using Windows.Devices.Enumeration;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace FindAllDevices
{

    public sealed partial class MainPage : Page
    {
        private DeviceWatcherHelper deviceWatcherHelper;

        private ObservableCollection<DeviceInformationDisplay> resultCollection = new ObservableCollection<DeviceInformationDisplay>();

        public MainPage()
        {
            this.InitializeComponent();
            resultsListView.ItemsSource = resultCollection;
            deviceWatcherHelper = new DeviceWatcherHelper(resultCollection, Dispatcher);
        }

        private void StartWatcher(object sender, RoutedEventArgs e)
        {
            startWatcherButton.IsEnabled = false;
            resultCollection.Clear();

            // Only Bluetooth and Bluetooth LE devices will be detected
            string aqsQueryStringForBtAndBtLe = "(" + 
                BluetoothDevice.GetDeviceSelectorFromConnectionStatus(BluetoothConnectionStatus.Connected) +
                ") OR (" +
                 BluetoothLEDevice.GetDeviceSelectorFromConnectionStatus(BluetoothConnectionStatus.Connected) +
                 ")";

            DeviceWatcher deviceWatcher = DeviceInformation.CreateWatcher(aqsQueryStringForBtAndBtLe, null);
            deviceWatcherHelper.StartWatcher(deviceWatcher);
            stopWatcherButton.IsEnabled = true;
        }

        private void StopWatcher(object sender, RoutedEventArgs e)
        {
            stopWatcherButton.IsEnabled = false;
            deviceWatcherHelper.StopWatcher();
            startWatcherButton.IsEnabled = true;
        }
    }
}

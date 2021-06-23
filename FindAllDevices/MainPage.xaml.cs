using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Bluetooth;
using Windows.Devices.Enumeration;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace FindAllDevices
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
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

        private async void enumerateDevices()
        {
            //string bluetoothSelector = "System.Devices.Aep.ProtocolId:=\"{e0cbf06c-cd8b-4647-bb8a-263b43f0f974}\"";
            //string bluetoothSelector = "System.Devices.Aep.ProtocolId:=\"{bb7bb05e-5972-42b5-94fc-76eaa7084d49}\"";
            string bluetoothSelector = BluetoothLEDevice.GetDeviceSelectorFromPairingState(true);
            DeviceInformationCollection deviceInfoCollection = await DeviceInformation.FindAllAsync(bluetoothSelector, null);

            string deviceListText = "Below are the found devices:\n\n";

            int deviceNo = 1;

            foreach (DeviceInformation deviceInfo in deviceInfoCollection)
            {
                deviceListText += deviceNo + ". id: " + deviceInfo.Id + "\nKind: " + deviceInfo.Kind.ToString() + "\nName: " + deviceInfo.Name.ToString() + "\nProperty: " + deviceInfo.Properties.ToString() + "\n";
                //deviceListText += deviceNo + ". Name: " + deviceInfo.Name.ToString() + "\n";
                deviceNo++;
            }

            //DeviceList.Text = deviceListText;
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

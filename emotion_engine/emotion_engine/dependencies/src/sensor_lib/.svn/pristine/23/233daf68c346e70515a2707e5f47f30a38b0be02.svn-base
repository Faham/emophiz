using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using Tobii.Eyetracking.Sdk;
using Tobii.Eyetracking.Sdk.Exceptions;
using Tobii.Eyetracking.Sdk.Time;

namespace SDK_Basic_Eyetracking_Sample
{
    public partial class MainForm : Form
    {
        private readonly EyetrackerBrowser _trackerBrowser;
        private readonly Clock _clock;

        private IEyetracker _connectedTracker;
        private SyncManager _syncManager; 
        private string _connectionName;
        private bool _isTracking;

        public MainForm()
        {
            InitializeComponent();

            _clock = new Clock();

            _trackerBrowser = new EyetrackerBrowser();
            _trackerBrowser.EyetrackerFound += EyetrackerFound;
            _trackerBrowser.EyetrackerUpdated += EyetrackerUpdated;
            _trackerBrowser.EyetrackerRemoved += EyetrackerRemoved;
        }


        private void EyetrackerFound(object sender, EyetrackerInfoEventArgs e)
        {
            // When an eyetracker is found on the network we add it to the listview
            var trackerItem = CreateTrackerListViewItem(e.EyetrackerInfo);
            _trackerList.Items.Add(trackerItem);
            UpdateUIElements();
        }

        private void EyetrackerRemoved(object sender, EyetrackerInfoEventArgs e)
        {
            // When an eyetracker disappears from the network we remove it from the listview
            _trackerList.Items.RemoveByKey(e.EyetrackerInfo.ProductId);
            UpdateUIElements();
        }

        private void EyetrackerUpdated(object sender, EyetrackerInfoEventArgs e)
        {
            // When an eyetracker is updated we simply create a new 
            // listviewitem and replace the old one
            int index = _trackerList.Items.IndexOfKey(e.EyetrackerInfo.ProductId);
            if(index >= 0)
            {
                _trackerList.Items[index] = CreateTrackerListViewItem(e.EyetrackerInfo);
            }
        }


        private static ListViewItem CreateTrackerListViewItem(EyetrackerInfo info)
        {
            var trackerItem = new ListViewItem(info.ProductId);
            trackerItem.Name = info.ProductId;

            var sb = new StringBuilder();
            sb.AppendLine("Model: " + info.Model);
            sb.AppendLine("Status: " + info.Status);
            sb.AppendLine("Generation: " + info.Generation);
            sb.AppendLine("Product Id: " + info.ProductId);
            sb.AppendLine("Given Name: " + info.GivenName);
            sb.AppendLine("Firmware Version: " + info.Version);

            trackerItem.ToolTipText = sb.ToString();
            trackerItem.Tag = info;

            return trackerItem;
        }

        private ListViewItem GetSelectedItem()
        {
            if (_trackerList.SelectedItems.Count == 1)
            {
                return _trackerList.SelectedItems[0];
            }
            return null;
        }

        private void UpdateUIElements()
        {
            var selectedItemInfo = GetSelectedItem();

            if(selectedItemInfo != null)
            {
                _trackerInfoLabel.Text = selectedItemInfo.ToolTipText;
                _connectButton.Enabled = true;
            }
            else
            {
                _trackerInfoLabel.ResetText();
                _connectButton.Enabled = false;
            }

            if(_connectedTracker !=  null)
            {
                _connectionStatusLabel.Text = "Connected to " + _connectionName;
                _trackButton.Enabled = true;
                _calibrateButton.Enabled = true;
                _loadCalibrationMenuItem.Enabled = true;
                _saveCalibrationMenuItem.Enabled = true;
                _viewCalibrationMenuItem.Enabled = true;
                _framerateMenuItem.Enabled = true;
            }
            else
            {
                _connectionStatusLabel.Text = "Disconnected";
                _trackButton.Enabled = false;
                _calibrateButton.Enabled = false;
                _loadCalibrationMenuItem.Enabled = false;
                _saveCalibrationMenuItem.Enabled = false;
                _viewCalibrationMenuItem.Enabled = false;
                _framerateMenuItem.Enabled = false;
            }

            if(_isTracking)
            {
                _trackButton.Text = "Stop Tracking";
                _trackStatus.Enabled = true;
            }
            else
            {
                _trackButton.Text = "Start Tracking";
                _trackStatus.Enabled = false;
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // Start browsing for eyetrackers on the network
            _trackerBrowser.Start();
            UpdateUIElements();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Shutdown browser service
            _trackerBrowser.Stop();

            // Cleanup connections
            DisconnectTracker();
        }

        private void _trackerList_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateUIElements();
        }

        private void _connectButton_Click(object sender, EventArgs e)
        {
            // Disconnect existing connection
            DisconnectTracker();

            // Create new connection
            var selectedItem = GetSelectedItem();
            if(selectedItem != null)
            {
                var info = (EyetrackerInfo) selectedItem.Tag;
                ConnectToTracker(info);    
            }
            UpdateUIElements();
        }

        private void ConnectToTracker(EyetrackerInfo info)
        {
            try
            {
                _connectedTracker = EyetrackerFactory.CreateEyetracker(info);
                _connectedTracker.ConnectionError += HandleConnectionError;
                _connectionName = info.ProductId;

                _syncManager = new SyncManager(_clock, info);

                _connectedTracker.GazeDataReceived += _connectedTracker_GazeDataReceived;
                _connectedTracker.FramerateChanged += _connectedTracker_FramerateChanged;
            }
            catch (EyetrackerException ee)
            {
                if(ee.ErrorCode == 0x20000402)
                {
                    MessageBox.Show("Failed to upgrade protocol. " + 
                        "This probably means that the firmware needs" +
                        " to be upgraded to a version that supports the new sdk.","Upgrade Failed",MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("Eyetracker responded with error " + ee, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);    
                }

                DisconnectTracker();
            }
            catch(Exception)
            {
                MessageBox.Show("Could not connect to eyetracker.","Connection Failed",MessageBoxButtons.OK,MessageBoxIcon.Error);
                DisconnectTracker();
            }

            UpdateUIElements();
            
        }

        private void _connectedTracker_GazeDataReceived(object sender, GazeDataEventArgs e)
        {
            // Send the gaze data to the track status control.
            GazeDataItem gd = e.GazeDataItem;
            _trackStatus.OnGazeData(gd);

            if (_syncManager.SyncState.StateFlag == SyncStateFlag.Synchronized)
            {
                Int64 convertedTime = _syncManager.RemoteToLocal(gd.TimeStamp);
                Int64 localTime = _clock.GetTime();
            }
            else
            {
                Console.WriteLine("Warning. Sync state is " + _syncManager.SyncState.StateFlag);
            }
        }

        private static void _connectedTracker_FramerateChanged(object sender, FramerateChangedEventArgs e)
        {
            Console.WriteLine("Framerate changed " + e.Framerate);
        }

        private void HandleConnectionError(object sender, ConnectionErrorEventArgs e)
        {
            // If the connection goes down we dispose 
            // the IAsyncEyetracker instance. This will release 
            // all resources held by the connection
            DisconnectTracker();
            UpdateUIElements();
        }

        private void DisconnectTracker()
        {
            if(_connectedTracker != null)
            {
                _connectedTracker.GazeDataReceived -= _connectedTracker_GazeDataReceived;
                _connectedTracker.Dispose();
                _connectedTracker = null;
                _connectionName = string.Empty;
                _isTracking = false;

                _syncManager.Dispose();
            }
        }

        private void _trackButton_Click(object sender, EventArgs e)
        {
            if(_isTracking)
            {
                // Unsubscribe from gaze data stream
                _connectedTracker.StopTracking();
                _isTracking = false;
            }
            else
            {
                // Start subscribing to gaze data stream
                _connectedTracker.StartTracking();
                _isTracking = true;
            }
            UpdateUIElements();
        }

        private void _calibrateButton_Click(object sender, EventArgs e)
        {
            var runner = new CalibrationRunner();
            
            try
            {
                // Start a new calibration procedure
                var result = runner.RunCalibration(_connectedTracker);

                // Show a calibration plot if everything went OK
                if (result != null)
                {
                    var resultForm = new CalibrationResultForm();
                    resultForm.SetPlotData(result);
                    resultForm.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Not enough data to create a calibration (or calibration aborted).");
                }                
            }
            catch(EyetrackerException ee)
            {
                MessageBox.Show("Failed to calibrate. Got exception " + ee, 
                    "Calibration Failed", 
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }            
        }

        private void _saveCalibrationMenuItem_Click(object sender, EventArgs e)
        {
            if(_saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var calibration = _connectedTracker.GetCalibration();
                    
                    using(var stream = _saveFileDialog.OpenFile())
                    using(var writer = new BinaryWriter(stream))
                    {
                        writer.Write(calibration.RawData);
                    }
                }
                catch (EyetrackerException ee)
                {
                    MessageBox.Show("Failed to get calibration data. Got exception " + ee,
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }    
            }
        }

        private void _viewCalibrationMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var calibration =_connectedTracker.GetCalibration();
                var resultForm = new CalibrationResultForm();
                resultForm.SetPlotData(calibration);
                resultForm.ShowDialog();
            }
            catch(EyetrackerException ee)
            {
                MessageBox.Show("Failed to get calibration data. Got exception " + ee,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void _loadCalibrationMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (_openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    using (var stream = _openFileDialog.OpenFile())
                    using (var reader = new BinaryReader(stream))
                    {
                        byte[] data = reader.ReadBytes((int)stream.Length);
                        Calibration calibration = new Calibration(data);
                        _connectedTracker.SetCalibration(calibration);
                    }
                }
            }
            catch (EyetrackerException ee)
            {
                MessageBox.Show("Failed to load calibration data. Got exception " + ee,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            
        }

        private void _framerateMenuItem_Click(object sender, EventArgs e)
        {
            var framerate = _connectedTracker.GetFramerate();
            var availableFramerates = _connectedTracker.EnumerateFramerates();

            int fpsIndex = availableFramerates.IndexOf(framerate);

            FramerateDialog fpsDialog = new FramerateDialog(availableFramerates,fpsIndex);

            if(fpsDialog.ShowDialog() == DialogResult.OK)
            {
                _connectedTracker.SetFramerate(fpsDialog.CurrentFramerate);
            }
        }
    }
}

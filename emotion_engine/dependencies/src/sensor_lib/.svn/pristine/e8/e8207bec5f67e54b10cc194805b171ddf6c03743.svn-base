using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Tobii.Eyetracking.Sdk;
using Tobii.Eyetracking.Sdk.Upgrade;

namespace UpgradeSample
{
    public partial class Form1 : Form
    {
        private readonly EyetrackerBrowser _browser;
        private bool _upgrading;
        private ProgressReporter _reporter;

        public Form1()
        {
            InitializeComponent();

            _browser = new EyetrackerBrowser(EventThreadingOptions.CallingThread);
            _browser.EyetrackerFound += _browser_EyetrackerFound;
            _browser.EyetrackerRemoved += _browser_EyetrackerRemoved;
            _browser.EyetrackerUpdated += _browser_EyetrackerUpdated;
        }

        private void _browser_EyetrackerUpdated(object sender, EyetrackerInfoEventArgs e)
        {
            int index = _trackerCombo.Items.IndexOf(e.EyetrackerInfo);
            if(index >= 0)
            {
                _trackerCombo.Items[index] = e.EyetrackerInfo;
            }
        }

        private void _browser_EyetrackerRemoved(object sender, EyetrackerInfoEventArgs e)
        {
            _trackerCombo.Items.Remove(e.EyetrackerInfo);
        }

        private void _browser_EyetrackerFound(object sender, EyetrackerInfoEventArgs e)
        {
            _trackerCombo.Items.Add(e.EyetrackerInfo);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _browser.Start();
            UpdateUI();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            _browser.Stop();
        }

        private void _trackerCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateUI();
        }

        private void UpdateUI()
        {
            _upgradeButton.Enabled = !_upgrading && _trackerCombo.SelectedIndex >= 0;
            _cancelButton.Enabled = _reporter != null && _reporter.CanCancel;

            if(_reporter != null)
            {
                _progressBar.Value = (int) _reporter.Progress.Percent;
                SetProgressLabel(_reporter.Progress.CurrentStep, _reporter.Progress.NumberOfSteps);
            }
        }

        private void _upgradeButton_Click(object sender, EventArgs e)
        {
            if(_openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var trackerInfo = (EyetrackerInfo) _trackerCombo.SelectedItem;

                    Int32 errorCode = 0;
                    bool compatible = UpgradeManager.UpgradePackageIsCompatibleWithDevice(_openFileDialog.FileName, trackerInfo, out errorCode);

                    if (compatible)
                    {
                        Console.WriteLine("Package is compatible with the selected tracker");
                    }
                    else
                    {
                        Console.WriteLine("Package is NOT compatible with the selected tracker. Error code " + errorCode);
                    }

                    _upgrading = true;
                    _reporter = new ProgressReporter(EventThreadingOptions.CallingThread);
                    UpgradeManager.BeginUpgrade(_openFileDialog.FileName, trackerInfo, _reporter);

                    _reporter.UpgradeCompleted += _reporter_UpgradeCompleted;
                    _reporter.UpgradeProgress += _reporter_UpgradeProgress;
                    _reporter.CanCancelChanged += _reporter_CanCancelChanged;
                }
                catch (Exception)
                {
                    _upgrading = false;
                    throw;
                }

                UpdateUI();                    
            }
        }

        private void _reporter_CanCancelChanged(object sender, CanCancelEventArgs e)
        {
            _cancelButton.Enabled = e.CanCancel;
        }

        private void SetProgressLabel(int currentStep, int numberOfSteps)
        {
            _statusLabel.Text = "Upgrading: step " + currentStep + " of " + numberOfSteps;
        }

        private void _reporter_UpgradeProgress(object sender, ProgressEventArgs e)
        {
            Console.WriteLine(string.Format("Progress {0}/{1} {2}%", e.CurrentStep, e.NumberOfSteps, e.Percent));
            SetProgressLabel(e.CurrentStep, e.NumberOfSteps);
            _progressBar.Value = (int) e.Percent;
        }

        private void _reporter_UpgradeCompleted(object sender, UpgradeCompletedEventArgs e)
        {
            _reporter = null;
            _upgrading = false;
            _statusLabel.Text = "";
            UpdateUI();

            if(e.UpgradeSucceeded)
            {
                MessageBox.Show("Upgrade Succeeded!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Upgrade failed! Error: " + e.Message, "Error", MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
        }

        private void _cancelButton_Click(object sender, EventArgs e)
        {
            if(_reporter != null)
            {
                _reporter.Cancel();
            }
        }
    }
}

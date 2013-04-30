using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Tobii.Eyetracking.Sdk;

namespace SDK_Basic_Eyetracking_Sample
{
    public partial class CalibrationResultForm : Form
    {
        public CalibrationResultForm()
        {
            InitializeComponent();
        }

        public void SetPlotData(Tobii.Eyetracking.Sdk.Calibration calibration)
        {
            _leftPlot.Initialize(calibration.Plot);
            _rightPlot.Initialize(calibration.Plot);
        }

        private void _okButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
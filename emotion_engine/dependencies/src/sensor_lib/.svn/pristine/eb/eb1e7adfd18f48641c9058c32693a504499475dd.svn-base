using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SDK_Basic_Eyetracking_Sample
{
    public partial class FramerateDialog : Form
    {

        public FramerateDialog(IList<float> allFramerates, int currentFramerateIndex)
        {
            InitializeComponent();

            foreach (var f in allFramerates)
            {
                _fpsCombo.Items.Add(f);
            }

            _fpsCombo.SelectedItem = allFramerates[currentFramerateIndex];
            _fpsCombo.SelectedIndex = currentFramerateIndex;
        }

        public float CurrentFramerate
        {
            get
            {
                return (float) _fpsCombo.SelectedItem;
            }
        }

        private void _okButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void _cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}

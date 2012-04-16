using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Minigames.SingeltonClasses;

namespace Minigames.InterfaceClasses
{
    public partial class PlayerInformation : Form
    {
        public PlayerInformation()
        {
            //designer class
            InitializeComponent();
        }

        private void _okBtn_Click(object sender, EventArgs e)
        {
            //set the form status to enabaled
            USERINFORMATION info = USERINFORMATION.Instance;
            //save data
            try
            {
                info._participantID = Convert.ToInt16(this._participantIDtxtBox.Text);
                info._firstName = this._firstNameTxtBox.Text;
                info._lastName = this._lastNameTxtBox.Text;
                info._age = Convert.ToInt16(this._ageTxtBox.Text);
                info._DominantHand = this._rightHandRdBtn.Checked ? "right" : "left";
                info._fieldOfStudy = this._fielsOfStudyTxtBox.Text;
                info._gender = this._maleRdBtn.Checked ? "male" : "female";
            }
            catch (Exception)
            {
                MessageBox.Show("check the values again!");
                return;
            }
            
            //dispose the form
            this.Close();
        }

    }
}

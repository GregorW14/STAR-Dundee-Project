using System;
using System.Windows.Forms;

namespace IndustrialProject
{
    /// <summary>
    /// This class is for the 'About' option from the 'Help' menu in the application
    /// </summary>
    public partial class Help_About : Form
    {
        /// <summary>
        /// Set up 'About' window
        /// </summary>
        public Help_About()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Close form window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Help_About_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
        
        /// <summary>
        /// On form load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Help_About_Load(object sender, EventArgs e)
        {
            // Nothing
        }

        /// <summary>
        /// When the 'OK' button is pressed, close the window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonOK_Click_1(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK; // Setting the dialog of the form to ok in order to close it          
        }
    }
}

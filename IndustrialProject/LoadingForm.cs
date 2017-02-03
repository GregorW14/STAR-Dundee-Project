using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IndustrialProject
{
    /// <summary>
    /// This class is for the loading window that is displayed when something is being processed by a background worker
    /// </summary>
    public partial class LoadingForm : Form
    {
        public event EventHandler<EventArgs> Canceled;

        /// <summary>
        /// Constructor 
        /// </summary>
        public LoadingForm()
        {
            InitializeComponent();
            this.MaximizeBox = false;
        }
        
        /// <summary>
        /// Cancel button pressed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonCancel_Click(object sender, EventArgs e)
        {
            // Cancel the current background worker thread
            EventHandler<EventArgs> ea = Canceled;
            if (ea != null)
                ea(this, e);
        }

        /// <summary>
        /// Message for loading window
        /// </summary>
        public string Message
        {
            set { labelProgress.Text = value; }
        }

        /// <summary>
        /// Progression value for loading window
        /// </summary>
        public int ProgressValue
        {
            set { progressBar.Value = value; }
        }

        /// <summary>
        /// On window load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoadingForm_Load(object sender, EventArgs e)
        {
            // Nothing
        }
    }
}

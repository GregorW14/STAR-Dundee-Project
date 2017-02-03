using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace IndustrialProject
{
    public class PortTab : TabPage
    {
        /// <summary>
        /// The port that this tabs info is for
        /// </summary>
        private int portnum;
        /// <summary>
        /// The traffic sample that this tab contains
        /// </summary>
        private TrafficSample sample;
        /// <summary>
        /// All the controls that are on this form
        /// </summary>
        private GroupBox grpBox1_NewTab, grpBox2_NewTab, grpBox3_NewTab, grpBox4_NewTab;
        private Label lblNoOfPackets_NewTab, lblNoOfDataChars_NewTab, lblNoOfPacketErrors_NewTab, lblStartTime_NewTab, lblEndTime_NewTab, lblDuration_NewTab, lblAverageDataRate_NewTab, lblPacketCountResult_NewTab, lblErrorCountResult_NewTab, lblDataCharCountResult_NewTab, lblDataRateResult_NewTab, lblStartTimeResult_NewTab, lblEndTimeResult_NewTab, lblDurationTimeResult_NewTab, lblPacketRate_NewTab, lblPacketRateResult_NewTab, lblErrorRate_NewTab, lblErrorRateResult_NewTab;
        private Chart chartVisulation_NewTab;
        private WebBrowser webVisualisation_NewTab = new WebBrowser();
        private Button btnNextError_NewTab;
        private Button btnPreviousError_NewTab;
        private PacketListView lstviewPacketView_NewTab;
        private List<Panel> linepanels = new List<Panel>();
        private RichTextBox txtPacketDetails_NewTab;
        private List<ListViewItem> listitems = new List<ListViewItem>();
        private CheckBox chkBinaryHex_NewTab;
        /// <summary>
        /// Selected list view index, intialised as no item
        /// </summary>
        private int listViewSelectedIndex = -1;
        /// <summary>
        /// Public boolean representign whether or not not to display the packet contents as binary or not
        /// </summary>
        public bool binary = false;

        /// <summary>
        /// Constructor for this class
        /// </summary>
        /// <param name="portnum"></param>
        /// <param name="sample"></param>
        public PortTab(int portnum, TrafficSample sample)
        {
            this.portnum = portnum;
            this.sample = sample;
            //Intialise the enter event to show the red line panels
            this.Enter += new EventHandler(this.ShowPanels);
            IntitalizeComponent();
        }

        /// <summary>
        /// Intialise the port tab, intialising all the child controls and adding them to the form
        /// </summary>
        private void IntitalizeComponent()
        {
            ControlFactory controlfactory = new ControlFactory(sample, linepanels);
            //Instantiate the checkbox
            chkBinaryHex_NewTab = controlfactory.checkboxFactory(true, new Point(6, 15), "chckBinaryHexPort" + portnum, new Size(178, 17), 9, "Show packet contents as binary", true, new EventHandler(this.chkboxBinary_CheckedChanged));

            //Using the control factory we intialise all the controls for this port tab
            #region Intialise controls using the control factory class
            lblNoOfPacketErrors_NewTab = controlfactory.labelFactory(true, new Point(17, 26), "lblNoOfPacketErrorsPort" + portnum, new Size(100, 13), 0, "Number of packet errors:");
            lblErrorRate_NewTab = controlfactory.labelFactory(true, new Point(300, 26), "lblErrorRatePort" + portnum, new Size(127, 13), 11, "Error rate:");
            lblErrorRateResult_NewTab = controlfactory.labelFactory(true, new Point(450, 26), "lblErrorRateResultPort" + portnum, new Size(0, 13), 12, "");
            lblNoOfDataChars_NewTab = controlfactory.labelFactory(true, new Point(300, 51), "lblNoOfDataCharsPort" + portnum, new Size(100, 13), 0, "Number of data characters:");
            lblNoOfPackets_NewTab = controlfactory.labelFactory(true, new Point(17, 51), "lblNoOfPacketsPort" + portnum, new Size(129, 13), 1, "Number of packets:");
            lblStartTime_NewTab = controlfactory.labelFactory(true, new Point(17, 76), "lblStartTimePort" + portnum, new Size(54, 13), 4, "Start time:");
            lblEndTime_NewTab = controlfactory.labelFactory(true, new Point(17, 99), "lblEndTimePort" + portnum, new Size(51, 13), 5, "End time:");
            lblDuration_NewTab = controlfactory.labelFactory(true, new Point(17, 123), "lblDurationTimePort" + portnum, new Size(51, 13), 5, "Duration time:");
            lblAverageDataRate_NewTab = controlfactory.labelFactory(true, new Point(17, 150), "lblAverageDataRatePort" + portnum, new Size(95, 13), 6, "Data Rate:");
            lblPacketCountResult_NewTab = controlfactory.labelFactory(true, new Point(123, 51), "lblPacketCountResultPort" + portnum, new Size(0, 13), 2, "");
            lblErrorCountResult_NewTab = controlfactory.labelFactory(true, new Point(150, 26), "lblErrorCountResultPort" + portnum, new Size(0, 13), 3, "");
            lblDataCharCountResult_NewTab = controlfactory.labelFactory(true, new Point(450, 51), "lblDataCharCountResultPort" + portnum, new Size(0, 13), 3, "");
            lblDataRateResult_NewTab = controlfactory.labelFactory(true, new Point(111, 150), "lblDataRateResultPort" + portnum, new Size(0, 13), 10, "");
            lblStartTimeResult_NewTab = controlfactory.labelFactory(true, new Point(68, 76), "lblStartTimeResultPort" + portnum, new Size(0, 13), 7, "");
            lblEndTimeResult_NewTab = controlfactory.labelFactory(true, new Point(68, 99), "lblEndTimeResultPort" + portnum, new Size(0, 13), 8, "");
            lblDurationTimeResult_NewTab = controlfactory.labelFactory(true, new Point(95, 123), "lblDurationTimeResultPort" + portnum, new Size(0, 13), 8, "");
            lblPacketRate_NewTab = controlfactory.labelFactory(true, new Point(300, 150), "lblPacketRateResultPort" + portnum, new Size(127, 13), 11, "Packet rate (per second):");
            lblPacketRateResult_NewTab = controlfactory.labelFactory(true, new Point(450, 150), "lblPacketRate_Result" + portnum, new Size(0, 13), 12, "");
            //Instantitate button
            btnNextError_NewTab = controlfactory.buttonFactory(new Point(580, 365), "btnNextErrorPort" + portnum, new Size(90, 23), 15, "#", "Next Error", true, new EventHandler(this.nextErrorButton_Click));
            btnPreviousError_NewTab = controlfactory.buttonFactory(new Point(485, 365), "btnPreviousErrorPort" + portnum, new Size(90, 23), 15, "#", "Previous Error", true, new EventHandler(this.previousErrorButton_Click));
            //Instantiate Web Browser
            webVisualisation_NewTab = controlfactory.webbrowserFactory(new Point(6, 19), new Size(20, 20), "webVisualisationPort" + portnum, new Size(700, 340), 10);
            //Instantiate the textbox
            txtPacketDetails_NewTab = controlfactory.richtextboxFactory(new Point(6, 35), "txtPacketDetailsPort" + portnum, new Size(689, 215), 8, "");
            //Instantiate listview
            lstviewPacketView_NewTab = controlfactory.packetlistviewFactory(new Point(6, 19), "lstviewPacketViewPort" + portnum, new Size(664, 340), 10, false);
            lstviewPacketView_NewTab.View = View.Details;
            lstviewPacketView_NewTab.setPacketList(sample.getPackets());
            lstviewPacketView_NewTab.setPartnerBox(ref txtPacketDetails_NewTab);
            //Set the column headers for the packet list view
            string[] columns = { "Time", "Address", "Port", "Sequence Number", "Protocol", "Length", "Errors" };
            ColumnHeader columnHeader;
            foreach (string column in columns)
            {
                columnHeader = new ColumnHeader();
                columnHeader.Text = column;
                //Auto resize to column header text and content
                columnHeader.Width = -2;
                lstviewPacketView_NewTab.Columns.Add(columnHeader);
            }
            //Instantiate groupboxes
            grpBox1_NewTab = controlfactory.groupboxFactory(new List<Control> { lblAverageDataRate_NewTab, lblEndTime_NewTab, lblNoOfPacketErrors_NewTab, lblNoOfPackets_NewTab, lblStartTime_NewTab, lblDuration_NewTab, lblDataRateResult_NewTab, lblPacketCountResult_NewTab, lblDataCharCountResult_NewTab, lblErrorCountResult_NewTab, lblStartTimeResult_NewTab, lblEndTimeResult_NewTab, lblDurationTimeResult_NewTab, lblPacketRateResult_NewTab, lblPacketRate_NewTab, lblNoOfDataChars_NewTab, lblErrorRate_NewTab, lblErrorRateResult_NewTab }, new Point(6, 6), "grpBox1Port" + portnum, new Size(676, 183), 14, false, "Details");
            grpBox3_NewTab = controlfactory.groupboxFactory(new List<Control> { lstviewPacketView_NewTab, btnNextError_NewTab, btnPreviousError_NewTab }, new Point(6, 195), "grpBox2Port" + portnum, new Size(676, 394), 16, false, "Packet list");
            grpBox2_NewTab = controlfactory.groupboxFactory(new List<Control> { webVisualisation_NewTab }, new Point(694, 6), "grpBox3Port" + portnum, new Size(810, 382), 16, false, "Visualisation");
            grpBox4_NewTab = controlfactory.groupboxFactory(new List<Control> { txtPacketDetails_NewTab, chkBinaryHex_NewTab }, new Point(694, 394), "grpBox4Port" + portnum, new Size(900, 257), 18, false, "Packet contents");
            //Fill the things in we need filled in
            TabFiller tabfiller = new TabFiller(sample);
            //Fill in the packet details and the packet contents
            Object[] packetdetailsandcontents = tabfiller.fillPackeListAndContentBox(txtPacketDetails_NewTab, lstviewPacketView_NewTab, grpBox3_NewTab.Location, this.grpBox3_NewTab, linepanels/*,listitems*/);
            txtPacketDetails_NewTab = (RichTextBox)packetdetailsandcontents[0];
            lstviewPacketView_NewTab = (PacketListView)packetdetailsandcontents[1];
            Object[] webBrowserContents = tabfiller.fillVisualisationBox(webVisualisation_NewTab, grpBox2_NewTab.Location, this.grpBox2_NewTab);
            webVisualisation_NewTab = (WebBrowser)webBrowserContents[0];
            //Fill in the labels that need filling in
            Label[] filledlabels = tabfiller.fillTabLabels(lblDataRateResult_NewTab, lblErrorCountResult_NewTab, lblPacketCountResult_NewTab, lblStartTimeResult_NewTab, lblEndTimeResult_NewTab, lblDurationTimeResult_NewTab, lblPacketRateResult_NewTab, lblDataCharCountResult_NewTab, lblErrorRateResult_NewTab);
            lblDataRateResult_NewTab = filledlabels[0];
            lblPacketCountResult_NewTab = filledlabels[2];
            lblStartTimeResult_NewTab = filledlabels[3];
            lblEndTimeResult_NewTab = filledlabels[4];
            lblDurationTimeResult_NewTab = filledlabels[5];
            lblDataCharCountResult_NewTab = filledlabels[7];
            lblErrorRateResult_NewTab = filledlabels[8];
            #endregion Control Factory

            //And finally.. instantiate the tab page
            this.Controls.AddRange(new Control[] { grpBox1_NewTab, grpBox2_NewTab, grpBox3_NewTab, grpBox4_NewTab });
            this.Name = "tpPort" + portnum + "TabPage";
            this.Text = "Port " + portnum;
            this.TabIndex = 1;
            this.UseVisualStyleBackColor = true;
            this.Padding = new System.Windows.Forms.Padding(3);
        }

        /// <summary>
        /// An array of listview items that make up the packet list view
        /// </summary>
        /// <returns></returns>
        public ListViewItem[] getPacketListViewItems()
        {
            //Take all the items from the list view
            ListViewItem[] items = new ListViewItem[lstviewPacketView_NewTab.Items.Count];
            //Copy them and then return them
            lstviewPacketView_NewTab.Items.CopyTo(items, 0);
            return items;
        }



        /// <summary>
        /// Select either the next error or the previous one based off of the boolean passed
        /// </summary>
        /// <param name="direction"></param>
        private void selectError(bool direction)
        {
            int current = 0;
            //If there's a currently selected list view item
            if (lstviewPacketView_NewTab.SelectedIndices.Count > 0)
            {
                current = lstviewPacketView_NewTab.SelectedIndices[0];
            }
            //Deselect all the selected items
            foreach (ListViewItem i in lstviewPacketView_NewTab.SelectedItems)
            {
                i.Selected = false;
            }
            //Is there a traffic sample
            if (sample != null)
            {
                //If the user clicked next error, increment, otherwise go backwards
                if (direction == true) current++;
                else current--;

                int delta = 0;
                //Look until broken
                while (true)
                {
                    //If we're going forward and we get to the last packet
                    if (direction == true && current == sample.getPackets().Count)
                    {
                        //Go back to the first packet
                        current = 0;
                    }
                    //IF we're going backwards and the current item is at the front of the list
                    else if (direction == false && current == -1)
                    {
                        //Go to the last item in the list
                        current = sample.getPackets().Count - 1;
                    }
                    //Traversed the whole list? There's no errors then
                    if (delta > sample.getPackets().Count)
                    {
                        MessageBox.Show("The currently open traffic sample does not contain any errors.", "No errors found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;
                    }
                    //If the current packet has an error, focus and select it before breaking the loop
                    if (sample.getPackets()[current].hasError())
                    {
                        lstviewPacketView_NewTab.Items[current].Selected = true;
                        lstviewPacketView_NewTab.Items[current].Focused = true;
                        lstviewPacketView_NewTab.EnsureVisible(current);
                        lstviewPacketView_NewTab.Select();
                        break;
                    }
                    //Increment the count of packets traversed
                    delta++;
                    //If the user clicked next error, go to the next packet
                    if (direction == true) current++;
                    //If they clicked previous error, go backwards
                    else current--;
                }
            }
            else
            {
                //Display that there is no errors in this traffic
                MessageBox.Show("Please open a traffic recording in order to view the errors contained within.", "No errors to view", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        /// <summary>
        /// Show the red lines int the list view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ShowPanels(object sender, EventArgs e)
        {
            this.grpBox3_NewTab.Controls.AddRange(linepanels.ToArray());
            foreach (Panel panel in linepanels)
            {
                panel.BringToFront();
                panel.Visible = true;
            }
        }

        /// <summary>
        /// even for when the user pressess the next error button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void nextErrorButton_Click(object sender, EventArgs e)
        {
            selectError(true);
        }


        /// <summary>
        /// even for when the user pressess the previous error button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void previousErrorButton_Click(object sender, EventArgs e)
        {
            selectError(false);
        }

        /// <summary>
        /// Return the source port number
        /// </summary>
        /// <returns></returns>
        public int getSourcePort()
        {
            return sample.getSourcePort();
        }

        /// <summary>
        /// Return the web browser on this port tab
        /// </summary>
        /// <returns></returns>
        public WebBrowser getWebBrowser()
        {
            return webVisualisation_NewTab;
        }

        /// <summary>
        /// Return the current selected packets index in the list view
        /// </summary>
        /// <returns></returns>
        public int getSelectedIndex()
        {
            return this.listViewSelectedIndex;
        }

        /// <summary>
        /// Return the list view
        /// </summary>
        /// <returns></returns>
        public ListView getListView()
        {
            return this.lstviewPacketView_NewTab;
        }

        /// <summary>
        /// Event that is triggered when the checkbox is changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkboxBinary_CheckedChanged(object sender, EventArgs e)
        {
            //If the user selects binary, show them it
            if (binary == false)
            {
                binary = true;
                lstviewPacketView_NewTab.setCheckbox(true);
                lstviewPacketView_NewTab.changeTextToBinary();
            }
            //Otherwise show them hex
            else
            {
                binary = false;
                lstviewPacketView_NewTab.setCheckbox(false);
                lstviewPacketView_NewTab.changeBinaryToHex();
            }
        }

        /// <summary>
        /// Return whether or not the checkbox has been pressed
        /// </summary>
        /// <returns></returns>
        public bool getCheckState()
        {
            return binary;
        }

        /// <summary>
        /// Resize the controls, needed for when the form changes in size to facilitate responsive UI design
        /// </summary>
        public void resizeControls()
        {
            //The packet list groupbox should remain anchored to the bottom of the tab
            grpBox3_NewTab.Height = (int)(this.Height - 200);
            //Anchor the previous and next error buttons to the bottom of the group box
            btnNextError_NewTab.Location = new Point(btnNextError_NewTab.Location.X, grpBox3_NewTab.Height - 30);
            btnPreviousError_NewTab.Location = new Point(btnPreviousError_NewTab.Location.X, grpBox3_NewTab.Height - 30);
            //Set packet list view height based upon the parent groupbox height
            lstviewPacketView_NewTab.Height = grpBox3_NewTab.Height - 55;
            //Anchor the packet contents groupbox to the bottom and right of the tab
            grpBox4_NewTab.Location = new Point(grpBox4_NewTab.Location.X, this.Height - 263);
            grpBox4_NewTab.Width = this.Width - 700;
            //Anchor the packet contents textbox to the bottom and right of its parent groupbox
            txtPacketDetails_NewTab.Height = grpBox4_NewTab.Height - 43;
            txtPacketDetails_NewTab.Width = grpBox4_NewTab.Width - 12;
            //Anchor the visualisation groupbox to the top of the packet contents area and to the right of the tab
            grpBox2_NewTab.Height = this.Height - 273;
            grpBox2_NewTab.Width = this.Width - 700;
            //Fit the web browser control to the visualisation container
            webVisualisation_NewTab.Width = grpBox2_NewTab.Width - 20;
            webVisualisation_NewTab.Height = grpBox2_NewTab.Height - 30;
        }

        /// <summary>
        /// Return the packet list groupbox
        /// </summary>
        /// <returns></returns>
        public GroupBox getPacketListGroupbox()
        {
            return this.grpBox3_NewTab;
        }


        /// <summary>
        /// Return the Packet List View 
        /// </summary>
        /// <returns></returns>
        public ListView getPacketListView()
        {
            return this.lstviewPacketView_NewTab;
        }


        /// <summary>
        /// Return the line panels of the packet list view
        /// </summary>
        /// <returns></returns>
        public List<Panel> getLinePanels()
        {
            return this.linepanels;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Windows.Forms.DataVisualization.Charting;

namespace IndustrialProject
{
    /// <summary>
    /// This class is used to create the controls needed to populate the programatically created tabs.
    /// </summary>
    class ControlFactory
    {
        /// <summary>
        /// This is the traffic sample that an instant of this class will need to use.
        /// </summary>
        private TrafficSample sample;
        /// <summary>
        /// Alexander change this please.
        /// </summary>
        private bool increasedHeight = false;
        /// <summary>
        /// A list of red line panels that are drawns on the Packet List View
        /// </summary>
        private List<Panel> linePanels;


        /// <summary>
        /// This is the constructor class for this method.
        /// </summary>
        /// <param name="sample"></param>
        /// <param name="linepanels"></param>
        public ControlFactory(TrafficSample sample, List<Panel> linepanels)
        {
            this.linePanels = linepanels;
            this.sample = sample;
        }


        /// <summary>
        /// This returns a new label created with the parameters that are passed to it.
        /// </summary>
        /// <param name="AutoSize"></param>
        /// <param name="Location"></param>
        /// <param name="Name"></param>
        /// <param name="Size"></param>
        /// <param name="TabIndex"></param>
        /// <param name="Text"></param>
        /// <returns></returns>
        public Label labelFactory(bool AutoSize, Point Location, string Name, Size Size, int TabIndex, string Text)
        {
            Label lbltoreturn = new Label();
            lbltoreturn.AutoSize = AutoSize;
            lbltoreturn.Location = Location;
            lbltoreturn.Name = Name;
            lbltoreturn.Size = Size;
            lbltoreturn.TabIndex = TabIndex;
            lbltoreturn.Text = Text;
            return lbltoreturn;
        }


        /// <summary>
        /// This returns a groupbox with the parameters passed to it set, and a List of controls to add as it's child controls.
        /// </summary>
        /// <param name="Controls"></param>
        /// <param name="Location"></param>
        /// <param name="Name"></param>
        /// <param name="Size"></param>
        /// <param name="TabIndex"></param>
        /// <param name="Text"></param>
        /// <returns></returns>
        public GroupBox groupboxFactory(List<Control> Controls, Point Location, string Name, Size Size, int TabIndex, bool TabStop, string Text)
        {
            GroupBox grpboxtoreturn = new GroupBox();
            Controls.ForEach(ctrl => grpboxtoreturn.Controls.Add(ctrl));
            grpboxtoreturn.Location = Location;
            grpboxtoreturn.Name = Name;
            grpboxtoreturn.Size = Size;
            grpboxtoreturn.TabIndex = TabIndex;
            grpboxtoreturn.TabStop = TabStop;
            grpboxtoreturn.Text = Text;
            return grpboxtoreturn;
        }

        /// <summary>
        /// This returns a web browser with the parameters passed to it set. 
        /// </summary>
        /// <param name="Location"></param>
        /// <param name="MinimumSize"></param>
        /// <param name="Name"></param>
        /// <param name="Size"></param>
        /// <param name="TabIndex"></param>
        /// <returns></returns>
        public WebBrowser webbrowserFactory(Point Location, Size MinimumSize, string Name, Size Size, int TabIndex)
        {
            WebBrowser webtoreturn = new WebBrowser();
            webtoreturn.Location = Location;
            webtoreturn.MinimumSize = MinimumSize;
            webtoreturn.Name = Name;
            webtoreturn.Size = Size;
            webtoreturn.TabIndex = TabIndex;
            webtoreturn.ScriptErrorsSuppressed = true;
            webtoreturn.AllowWebBrowserDrop = false;
            return webtoreturn;
        }

        /// <summary>
        /// This returns a PacketListView control with the properties set by the parameters.
        /// </summary>
        /// <param name="Location"></param>
        /// <param name="Name"></param>
        /// <param name="Size"></param>
        /// <param name="TabIndex"></param>
        /// <param name="UseCompatibleStateImageBehaviour"></param>
        /// <returns></returns>
        public PacketListView packetlistviewFactory(Point Location, string Name, Size Size, int TabIndex, bool UseCompatibleStageImageBehaviour)
        {
            PacketListView listviewtoreturn = new PacketListView();
            //Set everything
            listviewtoreturn.Location = Location;
            listviewtoreturn.Name = Name;
            listviewtoreturn.Size = Size;
            listviewtoreturn.TabIndex = TabIndex;
            listviewtoreturn.UseCompatibleStateImageBehavior = UseCompatibleStageImageBehaviour;
            //Set the ColumWidthChanging event to be the method below, listViewColumnWidthChanging;
            listviewtoreturn.ColumnWidthChanging += listViewColumnWidthChanging;
            listviewtoreturn.FullRowSelect = true;
            listviewtoreturn.MultiSelect = false;
            //Return the object
            return listviewtoreturn;
        }

        /// <summary>
        /// Event handler, fired when the packet list view column width changes
        /// </summary>
        public void listViewColumnWidthChanging(object sender, ColumnWidthChangingEventArgs e)
        {
            ListView control = (ListView)sender;

            //Calculate the total width of the columns
            int width = 0;
            foreach (ColumnHeader header in control.Columns)
            {
                width += header.Width;
            }

            //If the total width of the columns exceeds the control's width, the red lines that
            //indicate error positions should be moved upwards as the horizontal scroll bar will
            //become visible, therefore reducing the height of the vertical scroll bar
            int delta = 0;
            if(width >= control.Width - 17)
            {
                if (increasedHeight == false)
                {
                    delta = 17;
                    increasedHeight = true;
                }
            }
            else
            {
                if(increasedHeight == true)
                {
                    delta = -17;
                    increasedHeight = false;
                }
            }

            if (delta != 0) //If a move up/down is required
            {
                for (int i = 0; i < sample.getPackets().Count(); i++) //Loop through all packets
                {
                    if (sample.getPackets()[i].hasError() == true) //If the packet has an error
                    {
                        foreach (Panel panel in linePanels)
                        {
                            //Find the line that corresponds with this packet
                            int x = control.Location.X + 647;
                            int y = control.Location.Y + 20;

                            int decrease = 0;
                            if(delta == -17)
                            {
                                decrease = 17;
                            }

                            int currentY = y + (int)((float)(control.Height - 35 - decrease) * ((float)i / (float)sample.getPackets().Count));

                            if (panel.Location.Y == currentY)
                            {
                                //Move the line up/down as required
                                int temp = delta;
                                if (temp == -17) temp = 0;
                                int drawY = (int)((float)(control.Height - 35 - temp) * ((float)i / (float)sample.getPackets().Count));
                                
                                y += drawY;

                                panel.Location = new Point(x, y);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// This returns a richtextbox created with the properties set by the parameters.
        /// </summary>
        /// <param name="Location"></param>
        /// <param name="Name"></param>
        /// <param name="Size"></param>
        /// <param name="TabIndex"></param>
        /// <param name="Text"></param>
        public RichTextBox richtextboxFactory(Point Location, string Name, Size Size, int TabIndex, string Text)
        {
            RichTextBox txtboxtoreturn = new RichTextBox();
            //Set everything
            txtboxtoreturn.Location = Location;
            txtboxtoreturn.Name = Name;
            txtboxtoreturn.Size = Size;
            txtboxtoreturn.TabIndex = TabIndex;
            txtboxtoreturn.Text = Text;
            //Return the object
            return txtboxtoreturn;
        }

        /// <summary>
        /// This returns a button with the properties set by the parameters.
        /// </summary>
        /// <param name="Location"></param>
        /// <param name="Name"></param>
        /// <param name="Size"></param>
        /// <param name="TabIndex"></param>
        /// <param name="Text"></param>
        /// <param name="UseVisualStyleBackColor"></param>
        /// <param name="ButtonClick"></param>
        public Button buttonFactory(Point Location, string Name, Size Size, int TabIndex, string Tag, string Text, bool UseVisualStyleBackColor, EventHandler ButtonClick)
        {
            Button btntoreturn = new Button();
            btntoreturn.Location = Location;
            btntoreturn.Name = Name;
            btntoreturn.Size = Size;
            btntoreturn.TabIndex = TabIndex;
            btntoreturn.Tag = Tag;
            btntoreturn.Text = Text;
            btntoreturn.UseVisualStyleBackColor = UseVisualStyleBackColor;
            //Set the click event to the passed event
            btntoreturn.Click += ButtonClick;
            return btntoreturn;
        }

        /// <summary>
        /// This returns a Tab Page created with the properties set by the parameters passed.
        /// </summary>
        /// <param name="groupboxes"></param>
        /// <param name="Name"></param>
        /// <param name="Text"></param>
        /// <param name="TabIndex"></param>
        /// <param name="UseVisualStyleBackColor"></param>
        /// <param name="Padding"></param>
        public TabPage tabpageFactory(Control[] groupboxes, string Name, string Text, int TabIndex, bool UseVisualStyleBackColor, Padding Padding)
        {
            TabPage tptoreturn = new TabPage();
            //Set everything
            tptoreturn.Controls.AddRange(groupboxes);
            tptoreturn.Name = Name;
            tptoreturn.Text = Text;
            tptoreturn.TabIndex = TabIndex;
            tptoreturn.UseVisualStyleBackColor = UseVisualStyleBackColor;
            //Return the tabpage
            return tptoreturn;
        }

        /// <summary>
        /// This returns a Check Box with the properties set by the parameters passed to it. 
        /// </summary>
        /// <param name="AutoSize"></param>
        /// <param name=Location"></param>
        /// <param name="Name"></param>
        /// <param name="Size"></param>
        /// <param name="TabIndex"></param>
        /// <param name="Text"></param>
        /// <param name="UseVisualStyleBackColor"></param>
        /// <param name="Checked"></param>
        public CheckBox checkboxFactory(bool AutoSize, Point Location, string Name, Size Size, int TabIndex, string Text, bool UseVisualStyleBackColor, EventHandler CheckedChanged)
        {
            CheckBox cbtoreturn = new CheckBox();
            //Set everything
            cbtoreturn.AutoSize = AutoSize;
            cbtoreturn.Location = Location;
            cbtoreturn.Name = Name;
            cbtoreturn.Size = Size;
            cbtoreturn.TabIndex = TabIndex;
            cbtoreturn.Text = Text;
            cbtoreturn.UseVisualStyleBackColor = UseVisualStyleBackColor;
            cbtoreturn.CheckedChanged += CheckedChanged;
            //Return the checkbox
            return cbtoreturn;
        }
    }
}
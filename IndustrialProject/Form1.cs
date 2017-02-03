using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace IndustrialProject
{
    /// <summary>
    /// The main form of the program.
    /// </summary>
    public partial class Form1 : Form
    {
        /// <summary>
        /// The loading form that will be used for async tasks.
        /// </summary>
        private LoadingForm alert;

        /// <summary>
        /// A string representing a filename, intialised as blank.
        /// </summary>
        private string fileName = "";

        /// <summary>
        /// The parser we will use.
        /// </summary>
        private Parser parser;

        /// <summary>
        /// The current traffic sample, this will changed several times.
        /// </summary>
        private TrafficSample sample;

        /// <summary>
        /// A list of packets, needed for the overview tab.
        /// </summary>
        private List<Packet> overviewPackets = new List<Packet>();

        /// <summary>
        /// The debug folder path.
        /// </summary>
        private string debugFolderPath;

        /// <summary>
        /// A list of tuples, each tuple having a Port Tab controls and its paired Traffic Sample.
        /// </summary>
        private List<Tuple<PortTab, TrafficSample>> tabpages = new List<Tuple<PortTab, TrafficSample>>();

        /// <summary>
        /// The console log, this will be added to every time a new operation is done.
        /// </summary>
        private string consolelog;

        /// <summary>
        /// Delegate that we will need to open a file from a provided location on the disk.
        /// </summary>
        /// <param name="s"></param>
        private delegate void DelegateOpenFile(String s);

        /// <summary>
        /// The delegate we will need to open files via drag and drop.
        /// </summary>
        private DelegateOpenFile _openFileDelegate;


        /// <summary>
        /// A list of the current tabs that are open.
        /// </summary>
        public List<int> currentTabs = new List<int>();

        /// <summary>
        /// Index of graph type based on position in view menu
        /// </summary>
        private int graphTypeIndex = 0;

        /// <summary>
        /// An array of the graph types.
        /// </summary>
        private string[] graphTypeArray = { "dataRateOverTime", "errorLocationsInTheTraffic", "unexpectedDataValues", "packetRate" };

        /// <summary>
        /// The current selected Tab Index
        /// </summary>
        private int selectedTabIndex = 0;

        /// <summary>
        /// A model for the graph content displayed on the overview tab
        /// </summary>
        private OverviewGraphContent overviewGraphContent = new OverviewGraphContent();


        /// <summary>
        /// A list of webbrowsers, needed for the tabs.
        /// </summary>
        private List<WebBrowser> webBrowserList = new List<WebBrowser>();


        /// <summary>
        /// Constructor for this form.
        /// </summary>
        public Form1()
        {
            InitializeComponent();
            //Allow drag drop opening of files
            this.AllowDrop = true;
            this.DragEnter += new DragEventHandler(Form1_DragEnter);
            this.DragDrop += new DragEventHandler(Form1_DragDrop);
            _openFileDelegate = new DelegateOpenFile(this.OpenFile);
        }

        /// <summary>
        /// Code to be run after the form is loaded.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            //Declare a new Icon, using the in built system Information icon
            System.Drawing.Icon myIcon = new System.Drawing.Icon(System.Drawing.SystemIcons.Information, 32, 32);
            //Draw this icon in the about tool strip
            aboutToolStripMenuItem.Image = myIcon.ToBitmap();

            //Set the first toolstrip menu item to be true for the multiple views (data rate is first)
            dataRateOverTimeToolStripMenuItem.Checked = true;
            //Remove the template tab that we only use to get information to progrmatically create other tabs.
            tabControl1.TabPages.Remove(tabPage2);
            //Set the SelectedIndexChanged event in the tabcontrol to the method provided
            tabControl1.SelectedIndexChanged += new EventHandler(tabViewSelectedIndexChanged);

            //Intiatlise the listview
            string[] columns = { "Time", "Address", "Port", "Sequence Number", "Protocol", "Length", "Errors", "Source Port" };
            ColumnHeader columnHeader;
            foreach (string column in columns)
            {
                columnHeader = new ColumnHeader();
                columnHeader.Text = column;
                this.listViewOV.Columns.Add(columnHeader);
            }
            listViewOV.View = View.Details;
            //Don't allow the user to select multiple rows
            listViewOV.MultiSelect = false;
            //When they click on a row, select the whole row and not just a subheading
            listViewOV.FullRowSelect = true;

            //Set up the background worker for loading in the files
            backgroundWorkerLoadFile.DoWork += new DoWorkEventHandler(backgroundWorkerLoadFile_DoWork);
            backgroundWorkerLoadFile.ProgressChanged += new ProgressChangedEventHandler(backgroundWorkerLoadFile_ProgressChanged);
            backgroundWorkerLoadFile.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorkerLoadFile_RunWorkerCompleted);
            //Set up the background worker for drawing the graphs
            backgroundWorkerCreateGraph.DoWork += new DoWorkEventHandler(backgroundWorkerCreateGraph_DoWork);
            backgroundWorkerCreateGraph.ProgressChanged += new ProgressChangedEventHandler(backgroundWorkerCreateGraph_ProgressChanged);

            //Set the debug folder path to the current directory of the project.
            debugFolderPath = System.AppDomain.CurrentDomain.BaseDirectory;
            //Don't allow users to drop files into the webbrowser
            webBrowser.AllowWebBrowserDrop = false;
            //Disable the view menu strip because no files have been loaded in yet
            viewToolStripMenuItem1.Enabled = false;
        }

        /// <summary>
        /// Background worker for parsing the files that are being loaded in
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundWorkerLoadFile_DoWork(object sender, DoWorkEventArgs e)
        {
            parser = new Parser();
            //Parse the file provided
            sample = parser.parse(fileName, backgroundWorkerLoadFile, currentTabs);
        }

        /// <summary>
        /// Method used when the above background worker runs, updates a progress bar in another form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundWorkerLoadFile_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            // Update label
            alert.Message = "In progress, please wait... " + e.ProgressPercentage.ToString() + "%";
            // Update progress bar
            alert.ProgressValue = e.ProgressPercentage;
        }

        /// <summary>
        /// The method that will be run when the file load background worker is finished
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundWorkerLoadFile_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //If the loading was canceled
            if (e.Cancelled == true)
            {
                MessageBox.Show("Cancelled!");
            }
            //If there was an error
            else if (e.Error != null)
            {
                MessageBox.Show("Error: " + e.Error.Message);
            }
            //If the user tried to select a recording for a port they already have open
            else if (sample == null)
            {
                MessageBox.Show("You already have a tab open for this source port! Please close it.");
            }
            else
            {
                try
                {
                    //Hide the big splash page
                    pictureBoxTutorialImage.Hide();
                    viewToolStripMenuItem1.Enabled = true;
                    //Add the new tab to the tabpages
                    tabpages.Add(new Tuple<PortTab, TrafficSample>(createNewTab(sample.getSourcePort(), sample), sample));
                    //Add it to the integer list of current tabs
                    currentTabs.Add(sample.getSourcePort());
                    //Fill in the overview page
                    TabFiller tf = new TabFiller(sample);
                    //Add the new packets into overview packets list
                    overviewPackets.AddRange(sample.getPackets());
                    //Set the current overview list control for packets to the return of the tabfillers overview 
                    this.listViewOV = tf.fillOverViewListBox(this.listViewOV, tabpages[tabpages.Count - 1].Item1.getPacketListViewItems());
                    this.listViewOV.setPacketList(overviewPackets);
                    //Set the packet contents text box as a partner to the above packetlist
                    this.listViewOV.setPartnerBox(ref richTextBoxOV);
                    //Set the current tab to the new port loaded in
                    tabControl1.SelectedIndex = tabControl1.TabCount - 1;
                    //Resize the group boxes on the form
                    resizeGroupBoxes();
                    //Resize controls within port tabs
                    foreach (Tuple<PortTab, TrafficSample> portTab in tabpages)
                    {
                        portTab.Item1.resizeControls();
                    }
                }
                catch (Exception err)
                {
                    //Write to the two consoles that there has been an error
                    Console.Write("\n### ERRROR ###\n" + err.Message);
                    txtConsoleLog.Text = "\n Failed to load file" + err.Message;
                }
                //Create the first graph
                createFirstGraph();
            }
            // Close the AlertForm
            alert.Close();
        }

        /// <summary>
        /// Background task for creating the graphs
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundWorkerCreateGraph_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                updateTabGraphs();
            }
            catch (Exception err)
            {
                Console.Write("\n### ERRROR backgroundWorkerCreateGraph_DoWork ###\n" + err.Message);
            }
        }

        /// <summary>
        /// Method used when the above background worker runs, updates a progress bar in another form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundWorkerCreateGraph_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            alert.Message = "Creating graph, please wait... " + e.ProgressPercentage.ToString() + "%"; // Update label
            alert.ProgressValue = e.ProgressPercentage; // Update progress bar
        }




        /// <summary>
        /// Handle the deleting of tabs, in particulart the one that is passed
        /// </summary>
        /// <param name="tab"></param>
        public void deleteTab(PortTab tab)
        {
            //For each of the the port tabs open
            foreach (Tuple<PortTab, TrafficSample> tuple in tabpages.ToList())
            {
                //If it's this tab
                if (tuple.Item1 == tab)
                {
                    // Delete HTML files associated with tab being removed
                    // source: http://stackoverflow.com/questions/1620366/delete-files-from-directory-if-filename-contains-a-certain-word
                    string rootFolderPath = debugFolderPath;
                    string filesToDelete = @"*" + tuple.Item2.getUniqueID() + "*.html";   // Only delete DOC files containing "DeleteMe" in their filenames
                    string[] fileList = System.IO.Directory.GetFiles(rootFolderPath, filesToDelete);
                    foreach (string file in fileList)
                    {
                        Console.WriteLine("\n### DELETING HTML FILES ###");
                        Console.WriteLine(file + " will be deleted");
                        System.IO.File.Delete(file);
                    }
                    //Remove this tuple from the tabpages list of tuples
                    tabpages.Remove(tuple);
                }
            }
        }

        /// <summary>
        /// This event handler cancels the backgroundworker, fired from Cancel button in AlertForm
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonCancel_Click(object sender, EventArgs e)
        {
            if (backgroundWorkerLoadFile.WorkerSupportsCancellation == true)
            {
                // Cancel the asynchronous operation.
                backgroundWorkerLoadFile.CancelAsync();
                // Close the AlertForm
                alert.Close();
            }
        }

        /// <summary>
        /// On the tab controls selected index being changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabControl1_SelectedIndexChanged(Object sender, EventArgs e)
        {
            int tabIndex = tabControl1.SelectedIndex;
            const int OVERVIEW_TAB_INDEX = 0;
            //If the user selected the overview tab
            if (tabIndex == OVERVIEW_TAB_INDEX)
            {
                //Draw the graph type that the user previously selected
                graphTypeIndex = overviewGraphContent.getSelectedGraphIndex();
            }
            else
            {
                //Otherwise draw the type of graph the user selected for that particular tab
                TrafficSample trafficSample = tabpages[tabIndex - 1].Item2;
                graphTypeIndex = trafficSample.getSelectedGraphIndex();
            }
            //Save the users graph type selection
            updateViewMenuSelectedGraph();
        }

        /// <summary>
        /// This method returns a new tab page and addits it to the tab control
        /// </summary>
        /// <param name="portnum"></param>
        /// <param name="trafficsample"></param>
        /// <param name="tabToCopy"></param>
        /// <returns></returns>
        private PortTab createNewTab(int portnum, TrafficSample trafficsample)
        {
            PortTab tpNewTabPage = new PortTab(portnum, trafficsample);
            try
            {
                Console.WriteLine("\nAdding to webBrowserList\n");
                //Set the current selected tab index to the number of web browsers on the page
                selectedTabIndex = webBrowserList.Count;
                //Add the new webbrowser to the list of webbrowsers
                webBrowserList.Add(tpNewTabPage.getWebBrowser());
            }
            catch (Exception e)
            {
                Console.WriteLine("\n### Error ###\n" + e.Message);
            }
            tabControl1.TabPages.Add(tpNewTabPage);
            return tpNewTabPage;
        }


        /// <summary>
        /// Method for handling changes to the current tab view selected index
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabViewSelectedIndexChanged(object sender, EventArgs e)
        {
            //Integer representing current tab
            int tabIndex = tabControl1.SelectedIndex - 1;
            //If it's not the overview tab
            if (tabIndex >= 0)
            {
                //Set tab equal to the port tab object stored in the tabpages list of tuples
                PortTab tab = tabpages[tabIndex].Item1;
                //Get the current selected index in that tab
                int listIndex = tab.getSelectedIndex();
                //Get the packet list view from that tab
                ListView listView = tab.getListView();
                //If the  user actually has selected a list item in that tab
                if (listIndex != -1)
                {
                    //Select it
                    listView.Items[listIndex].Selected = true;
                    listView.Items[listIndex].Focused = true;
                    listView.EnsureVisible(listIndex);
                    listView.Select();
                }
            }
        }

        /// <summary>
        /// Create the first graph for new tab
        /// </summary>
        private void createFirstGraph()
        {
            // Warning! This is not done asynchronously
            graphTypeIndex = 0;
            //Update the tab graphs
            updateTabGraphs();
            //Html for the overview graph page
            string html = createOverviewGraphHTML(graphTypeIndex);
            //Put that html in the browser
            displayHTML(webBrowser, html);
        }

        /// <summary>
        /// Return a string with the HTML needed to put in the overview graph webbrowser, using the selected graph type 
        /// </summary>
        /// <param name="graphType"></param>
        /// <returns></returns>
        private string createOverviewGraphHTML(int graphType)
        {
            //All the things we're going to beed to build up the highcharts chart
            string series = "series: [";
            string graphTitle = "";
            string axisLabel = "";
            string ports = "";
            string typeStr = "";
            string html = "";
            //Get all the names of the ports currently open and append them to the string
            for (int i = 0; i < tabpages.Count; i++)
            {
                ports += "'Port " + tabpages[i].Item2.getSourcePort() + "'";
                if (i + 1 != tabpages.Count)
                {
                    ports += ", ";
                }
            }
            //If the user wants a data rate over time graph
            if (graphType == 0)
            {
                //Set the infor for it
                graphTitle = "Data rate";
                axisLabel = "Time";
                typeStr = "spline";
                series = "";
                //Go through the tabpages
                for (int i = 0; i < tabpages.Count; i++)
                {
                    //And get the information needed to build a line graph for data over time using the traffic sample from this tab
                    string[] json = JSON.formatModelToJSON_DataRateOverTime(tabpages[i].Item2, this.Width);
                    ports += json[0];
                    series += "{ name: 'Port " + tabpages[i].Item2.getSourcePort() + "', tooltip: { valueSuffix: ' bits/s', valueDecimals: 2 }" + json[1];
                    //If there's still more tabpages, add them to the javascript information
                    if (i + 1 != tabpages.Count)
                    {
                        series += ", ";
                    }
                }
                //The HTML needed to build the page
                html = "<!DOCTYPE html><html><head> <meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\"/> <style>html, body{height: 100%; overflow:hidden;}#container{height: 100% !important;}</style> <script src=\"https://code.jquery.com/jquery-2.2.4.min.js\"></script> <script src=\"https://code.highcharts.com/highcharts.js\"></script> <script src=\"https://code.highcharts.com/modules/data.js\"></script> <script src=\"https://code.highcharts.com/modules/exporting.js\"></script> <script src=\"https://www.highcharts.com/samples/static/highslide-full.min.js\"></script> <script src=\"https://www.highcharts.com/samples/static/highslide.config.js\"></script> <link rel=\"stylesheet\" type=\"text/css\" href=\"https://www.highcharts.com/samples/static/highslide.css\"/></head><body> <script>$(document).ready(function(){var options={chart:{renderTo: 'container', type: 'spline'}, yAxis:{title:{text:\"Bits per second\"}}, xAxis:{title:{text:\"Time Interval\"}}, title:{text: 'Data rate over time'}, series:[" + series + "]}; var chart=new Highcharts.Chart(options);}); </script> <div id=\"container\" height:\"100%\" width=\"100%\"></div></body></html>";
            }
            //If it's a Error Locations in the traffic graph
            else if (graphType == 1)
            {
                //Set the information
                graphTitle = "Error locations in the traffic";
                axisLabel = "Packet Number";
                typeStr = "bar";
                //Create a list of lists of tuples
                List<List<Tuple<bool, int>>> segments = new List<List<Tuple<bool, int>>>();
                for (int i = 0; i < tabpages.Count; i++)
                {
                    segments.Add(new List<Tuple<bool, int>>());
                }

                for (int i = 0; i < tabpages.Count; i++)
                {
                    TrafficSample trafficSample = tabpages[i].Item2;
                    bool error = false;
                    int count = 0;
                    for (int j = trafficSample.getPackets().Count - 1; j >= 0; j--)
                    {
                        if (trafficSample.getPackets()[j].hasError() != error)
                        {
                            segments[i].Add(new Tuple<bool, int>(error, count));

                            error = trafficSample.getPackets()[j].hasError();
                            count = 0;

                            if (j == 0 && (trafficSample.getPackets()[j].hasError() != trafficSample.getPackets()[j + 1].hasError()))
                            {
                                segments[i].Add(new Tuple<bool, int>(error, 1));
                            }
                        }
                        else if (j == 0)
                        {
                            segments[i].Add(new Tuple<bool, int>(error, count));
                        }

                        count++;
                    }
                }

                int maxSegments = 0;
                maxSegments = segments[0].Count;
                for (int i = 1; i < segments.Count; i++)
                {
                    if (maxSegments < segments[i].Count)
                    {
                        maxSegments = segments[i].Count;
                    }
                }

                bool[] first = new bool[2];
                bool segmentState = false;
                for (int i = 0; i < maxSegments; i++)
                {
                    series += "{";
                    if (segmentState == false)
                    {
                        series += "name: 'Accepted packets',";
                        series += "color: '#56c648',";

                        if (first[0] == true)
                        {
                            series += "linkedTo:':previous',";
                        }

                        first[0] = true;
                    }
                    else
                    {
                        series += "name: 'Error packets',";
                        series += "color: '#d94343',";

                        if (first[1] == true)
                        {
                            series += "linkedTo:':previous',";
                        }

                        first[1] = true;
                    }

                    series += "data: [";

                    for (int j = 0; j < tabpages.Count; j++)
                    {
                        int count = 0;
                        if (i < segments[j].Count)
                        {
                            if (segments[j][i].Item1 == segmentState)
                            {
                                count = segments[j][i].Item2;
                            }
                        }

                        series += count.ToString() + ", ";
                    }

                    segmentState = !segmentState;

                    series = series.Remove(series.Length - 2);
                    series += "]}";
                    if (i + 1 < maxSegments)
                    {
                        series += ",";
                    }
                }
            }
            else if (graphType == 2)
            {
                graphTitle = "Unexpected Data Values";
                axisLabel = "Number of Errors";
                typeStr = "bar";

                series += "{";
                series += "name: 'Unexpected data errors',";
                series += "color: '#d94343',";

                series += "data: [";

                for (int i = 0; i < tabpages.Count; i++)
                {
                    //Count total number of crc/parity errors in this tab's sample
                    int errorCount = 0;
                    TrafficSample sample = tabpages[i].Item2;
                    foreach (Packet packet in sample.getPackets())
                    {
                        bool crc = false;
                        RMAP rmap = packet.getRMAP();
                        if (rmap != null)
                        {
                            if (rmap.isCRCValid() > 0)
                            {
                                crc = true;
                            }
                        }

                        if (packet.getParity() == true || crc == true || packet.getRepeat() == true || packet.getOutOfSequence() == true)
                        {
                            errorCount++;
                        }
                    }

                    series += errorCount.ToString();

                    if (i + 1 < tabpages.Count)
                    {
                        series += ",";
                    }
                }

                series += "]}";
            }
            //If the user selected the packet rate over time graph
            else if (graphType == 3)
            {
                //Set the needed information
                graphTitle = "Packet rate";
                axisLabel = "Time";
                typeStr = "spline";
                series = "";
                //For each of the traffic samples in the tabpages
                for (int i = 0; i < tabpages.Count; i++)
                {
                    //Get the json information for making a packet rate over time graph 
                    string[] json = JSON.formatModelToJSON_PacketRate(tabpages[i].Item2, this.Width);
                    ports += json[0];
                    series += "{ name: 'Port " + tabpages[i].Item2.getSourcePort() + "', tooltip: { valueSuffix: ' packets/s', valueDecimals: 2 }" + json[1];
                    //If there's still more tabs to come, account for that
                    if (i + 1 != tabpages.Count)
                    {
                        series += ", ";
                    }
                }
                //Return the HTML for this graph
                html = "<!DOCTYPE html><html><head> <meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\"/> <style>html, body{height: 100%; overflow:hidden;}#container{height: 100% !important;}</style> <script src=\"https://code.jquery.com/jquery-2.2.4.min.js\"></script> <script src=\"https://code.highcharts.com/highcharts.js\"></script> <script src=\"https://code.highcharts.com/modules/data.js\"></script> <script src=\"https://code.highcharts.com/modules/exporting.js\"></script> <script src=\"https://www.highcharts.com/samples/static/highslide-full.min.js\"></script> <script src=\"https://www.highcharts.com/samples/static/highslide.config.js\"></script> <link rel=\"stylesheet\" type=\"text/css\" href=\"https://www.highcharts.com/samples/static/highslide.css\"/></head><body> <script>$(document).ready(function(){var options={chart:{renderTo: 'container', type: 'spline'}, yAxis:{title:{text:\"Packets per second\"}}, xAxis:{title:{text:\"Time Interval\"}}, title:{text: 'Packet rate over time'}, series:[" + series + "]}; var chart=new Highcharts.Chart(options);}); </script> <div id=\"container\" height:\"100%\" width=\"100%\"></div></body></html>";
            }

            if (typeStr != "spline")
            {
                series += "]";
                html = "<!DOCTYPE html> <html> <head> <meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\" /> <style> html, body { height: 100%; overflow:hidden;} #container { height: 100% !important; } </style> <meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\" /> <script src=\"theme.js\"></script> <script src=\"https://code.jquery.com/jquery-3.1.0.min.js\" integrity=\"sha256-cCueBR6CsyA4/9szpPfrX3s49M9vUU5BgtiJj06wt/s=\" crossorigin=\"anonymous\"></script> <script src=\"https://code.highcharts.com/highcharts.js\"></script> <script src=\"https://code.highcharts.com/modules/data.js\"></script> <script src=\"https://code.highcharts.com/modules/exporting.js\"></script> <script src=\"https://www.highcharts.com/samples/static/highslide-full.min.js\"></script> <script src=\"https://www.highcharts.com/samples/static/highslide.config.js\" charset=\"utf-8\"></script> <link rel=\"stylesheet\" type=\"text/css\" href=\"https://www.highcharts.com/samples/static/highslide.css\" /> </head> <body> <script> $(document).ready(function() { var options = { chart: { renderTo: 'container', type: '" + typeStr + "' }, plotOptions: { series: { stacking: 'normal' } }, ";
                html += "yAxis: { min: 0, title: { text: '" + axisLabel + "' } }, title: { text: '" + graphTitle + "' }, xAxis: { categories: [" + ports + "] }, colors: [],";
                html += series;
                html += "}; var data = []; var chart = new Highcharts.Chart(options); }); </script> <div id=\"container\" style=\"min-width: 310px; height: 400px; margin: 0 auto\"></div> </body> </html>";
            }
            //Return the HTML for the graph
            return html;
        }


        /// <summary>
        /// This method goes through all the tabs, updating the graphs for them.
        /// </summary>
        private void updateTabGraphs()
        {
            //For each currently open tab
            for (int i = 0; i < tabpages.Count; i++)
            {
                //Get the traffic sample used in this tab
                TrafficSample trafficSample = tabpages[i].Item2;
                //Get the webbrowser used in this tab
                WebBrowser browser = tabpages[i].Item1.getWebBrowser();
                //The returned array in this tab
                String[] returnedArray;
                //Information needed for drawing the graphs
                string data = "";
                string colours = "";
                string fileName = graphTypeArray[graphTypeIndex] + trafficSample.getUniqueID() + ".html";
                string lines = "";
                //Get the the users currently selected graph
                trafficSample.setSelectedGraphIndex(graphTypeIndex);
                //If the users currently selected graph is a data over time graph
                if (graphTypeIndex == 0)
                {
                    //Return the JS needed for building a data rate over time graph
                    returnedArray = JSON.formatModelToJSON_DataRateOverTime(trafficSample, this.Width);
                    //Get the catagories of this graph
                    string catagories = returnedArray[0];
                    //Get the data for this graph
                    data = "{ name: 'Bits per second'" + returnedArray[1];
                    //HTML with in line JavaScript
                    lines = "<!DOCTYPE html><html><head> <meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\"/> <style>html, body{height: 100%; overflow:hidden;}#container{height: 100% !important;}</style> <script src=\"https://code.jquery.com/jquery-2.2.4.min.js\"></script> <script src=\"https://code.highcharts.com/highcharts.js\"></script> <script src=\"https://code.highcharts.com/modules/data.js\"></script> <script src=\"https://code.highcharts.com/modules/exporting.js\"></script> <script src=\"https://www.highcharts.com/samples/static/highslide-full.min.js\"></script> <script src=\"https://www.highcharts.com/samples/static/highslide.config.js\"></script> <link rel=\"stylesheet\" type=\"text/css\" href=\"https://www.highcharts.com/samples/static/highslide.css\"/></head><body> <script>$(document).ready(function(){var options={chart:{renderTo: 'container', type: 'spline'}, yAxis:{title:{text:\"Bits per second\"}}, xAxis:{title:{text:\"Seconds\"}, catagories: " + catagories + "}, title:{text: 'Data rate'}, series:[" + data + "]}; var chart=new Highcharts.Chart(options);}); </script> <div id=\"container\" height:\"100%\" width=\"100%\"></div></body></html>";
                }
                //If the user's currently selected graph is a error locations in the traffic graph
                else if (graphTypeIndex == 1)
                {
                    //Return the JS needed for building said graph
                    returnedArray = JSON.formatModelToJSON_ErrorLocationsInTheTraffic(trafficSample);
                    //Get the data needed to build the graph
                    data = returnedArray[0];
                    colours = returnedArray[1];
                    //HTML with in line JavaScript
                    lines =
                        "<!DOCTYPE html><html><head><meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\"/><style> html, body { 	height:100%; overflow:hidden;}  #container { 	height:100% !important; } </style><script src=\"theme.js\"></script><script   src=\"https://code.jquery.com/jquery-3.1.0.min.js\"   integrity=\"sha256-cCueBR6CsyA4/9szpPfrX3s49M9vUU5BgtiJj06wt/s=\"   crossorigin=\"anonymous\"></script><script src=\"https://code.highcharts.com/highcharts.js\"></script><script src=\"https://code.highcharts.com/modules/data.js\"></script><script src=\"https://code.highcharts.com/modules/exporting.js\"></script><script src=\"https://www.highcharts.com/samples/static/highslide-full.min.js\"></script><script src=\"https://www.highcharts.com/samples/static/highslide.config.js\" charset=\"utf-8\"></script><link rel=\"stylesheet\" type=\"text/css\" href=\"https://www.highcharts.com/samples/static/highslide.css\" /></head><body><script>$(document).ready(function() {var options = {chart: {renderTo: 'container',type: 'bar'}, legend: {enabled:false},plotOptions: {series: {stacking: 'normal'}},yAxis: {min: 0,title:{text:'Number of packets'}},title:{text:'Error locations in the traffic'}," +
                        colours +
                        ",series: [" +
                        data +
                        "]};var data =[];var chart = new Highcharts.Chart(options);});</script><div id=\"container\" style=\"min - width: 310px; height: 400px; margin: 0 auto\"></div></body></html>";
                }
                //If the users currently selected graph is an unexpected data values graph
                else if (graphTypeIndex == 2)
                {
                    //Return the JS  needed for building said graph
                    returnedArray = JSON.formatModelToJSON_UnexpectedDataValues(trafficSample);
                    //Get the data needed for building said graph
                    data = returnedArray[0];
                    colours = returnedArray[1];
                    //HTML with in line JavaScript
                    lines =
                        "<!DOCTYPE html><html><head><meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\"/><style> html, body { 	height:100%; overflow:hidden;}  #container { 	height:100% !important; } </style><script src=\"theme.js\"></script><script   src=\"https://code.jquery.com/jquery-3.1.0.min.js\"   integrity=\"sha256-cCueBR6CsyA4/9szpPfrX3s49M9vUU5BgtiJj06wt/s=\"   crossorigin=\"anonymous\"></script><script src=\"https://code.highcharts.com/highcharts.js\"></script><script src=\"https://code.highcharts.com/modules/data.js\"></script><script src=\"https://code.highcharts.com/modules/exporting.js\"></script><script src=\"https://www.highcharts.com/samples/static/highslide-full.min.js\"></script><script src=\"https://www.highcharts.com/samples/static/highslide.config.js\" charset=\"utf-8\"></script><link rel=\"stylesheet\" type=\"text/css\" href=\"https://www.highcharts.com/samples/static/highslide.css\" /></head><body><script>$(document).ready(function() {var options = {chart: {renderTo: 'container',type: 'bar'}, legend: {enabled:false},plotOptions: {series: {stacking: 'normal'}},xAxis: {categories: ['Incorrect sequence numbers', 'Header CRC', 'Data CRC', 'Header and data CRC']},yAxis: {min: 0,title:{text:'Number of packets'}},title:{text:'Unexpected data values'}," +
                        colours +
                        ",series: [" +
                        data +
                        "]};var data =[];var chart = new Highcharts.Chart(options);});</script><div id=\"container\" style=\"min - width: 310px; height: 400px; margin: 0 auto\"></div></body></html>";
                }
                //If the user selected a packet rate over time graph
                else if (graphTypeIndex == 3)
                {
                    //Return the JS needed to build the packet rate over time graph
                    returnedArray = JSON.formatModelToJSON_PacketRate(trafficSample, this.Width);
                    //Get the data needed for building said graph
                    string catagories = returnedArray[0];
                    data = "{ name: 'Packets per second'" + returnedArray[1];
                    //HTML with in line JavaScript
                    lines = "<!DOCTYPE html><html><head> <meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\"/> <style>html, body{height: 100%;overflow:hidden;}#container{height: 100% !important;}</style> <script src=\"https://code.jquery.com/jquery-2.2.4.min.js\"></script> <script src=\"https://code.highcharts.com/highcharts.js\"></script> <script src=\"https://code.highcharts.com/modules/data.js\"></script> <script src=\"https://code.highcharts.com/modules/exporting.js\"></script> <script src=\"https://www.highcharts.com/samples/static/highslide-full.min.js\"></script> <script src=\"https://www.highcharts.com/samples/static/highslide.config.js\"></script> <link rel=\"stylesheet\" type=\"text/css\" href=\"https://www.highcharts.com/samples/static/highslide.css\"/></head><body> <script>$(document).ready(function(){var options={chart:{renderTo: 'container', type: 'spline'}, yAxis:{title:{text:\"Packets per second\"}}, xAxis:{title:{text:\"Seconds\"}, catagories: " + catagories + "}, title:{text: 'Packet rate'}, series:[" + data + "]}; var chart=new Highcharts.Chart(options);}); </script> <div id=\"container\" height:\"100%\" width=\"100%\"></div></body></html>";
                }
                //If the browser isn't null
                if (browser != null)
                {
                    //Show the graph
                    displayHTML(browser, lines);
                }
            }
        }

        /// <summary>
        /// Save the graph to a file
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="lines"></param>
        private void saveGraphToFile(string fileName, string lines)
        {
            // Write the string to a file
            System.IO.StreamWriter file = new System.IO.StreamWriter(debugFolderPath + fileName);
            file.WriteLine(lines);
            file.Close();
            Console.WriteLine("\n\n\"" + "Created graph:" + debugFolderPath + fileName);
        }

        /// <summary>
        /// Method that handles what to do if something is dragged into
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            //We're only interested if a FILE was dropped on the form
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                //Show the efect for dragging a file
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        /// <summary>
        /// Method that handles what to do after something has been dragged and dropped on the form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                //An array of all the data in that file
                Array a = (Array)e.Data.GetData(DataFormats.FileDrop);
                //If the file wasn't empty
                if (a != null)
                {
                    //Treat it like opening a file normally
                    string s = a.GetValue(0).ToString();
                    this.BeginInvoke(_openFileDelegate, new Object[] { s });
                    this.Activate();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Loading file: " + ex.Message);
            }
        }

        /// <summary>
        /// This method handles the opening of files by filepath
        /// </summary>
        /// <param name="sFile"></param>
        private void OpenFile(string myFilePath)
        {
	    fileName = myFilePath;
	
            //Get the extension of the file
            string ext = Path.GetExtension(myFilePath);
            //If it's a .rec file
            if (ext == ".rec")
            {
                //Load in the file if the background worker for loading files isn't busy
                if (backgroundWorkerLoadFile.IsBusy != true)
                {
                    //LoadingForm
                    alert = new LoadingForm();
                    //Event handler for the Cancel button in LoadingForm
                    alert.Canceled += new EventHandler<EventArgs>(buttonCancel_Click);
                    // Show LoadingForm
                    alert.Show();
                    Console.WriteLine("Before background worker");
                    //Start the asynchronous operation
                    backgroundWorkerLoadFile.RunWorkerAsync();
                }
            }
            else
            {
                //Tell the user they selected the wrong type of file
                MessageBox.Show("Error!\nChosen file is not a valid recorded traffic file (.rec). You selected a \"" + ext + "\" file.\n\nPlease try again.");
            }
        }


        /// <summary>
        /// Clear the overview list, deleting all information for a specific tab
        /// </summary>
        /// <param name="porttodelete"></param>
        public void cleanOverviewList(int porttodelete)
        {
            //Go through the packet list on the overview page, deleting every item for this tab
            foreach (ListViewItem item in listViewOV.Items)
            {
                if (item.SubItems[7].Text == porttodelete.ToString())
                {
                    this.listViewOV.Items.Remove(item);
                }
            }
            //Clear the packet contents box
            richTextBoxOV.Clear();
            //Update the chart on the overview tab. This will remove the data that was within the closed tab.
            string html = createOverviewGraphHTML(graphTypeIndex);
            displayHTML(webBrowser, html);
        }

        /// <summary>
        /// Retrieve the path of the resources folder and return it
        /// </summary>
        /// <returns></returns>
        public string retrieveResourcePath()
        {
            //Source of code :- https://msdn.microsoft.com/en-us/library/system.appdomain.basedirectory(v=vs.110).aspx
            string debugFolderPath = System.AppDomain.CurrentDomain.BaseDirectory;
            debugFolderPath += @"Resources\";
            Console.WriteLine(debugFolderPath);
            return debugFolderPath;
        }


        /// <summary>
        /// What to do when the about menu item has been clicked
        /// </summary>
        private void menuAboutclicked()
        {
            // Hides the current form until the Dialog result from the new form returns ok; Then this form is unhidden
            Help_About help_about = new Help_About();
            help_about.ShowDialog();
            this.Show();
        }

        /// <summary>
        /// What happens when the user presses load file from the menu
        /// </summary>
        private void menuLoadFile()
        {
            //Open the file dialog 
            OpenFileDialog packetCaptureDialog = new OpenFileDialog();
            //Add a file filter
            packetCaptureDialog.Filter = "Packet Capture Files (.rec) | *.rec";
            //Set it to the default one
            packetCaptureDialog.FilterIndex = 1;
            //Disable the ability to select multiple files at once
            packetCaptureDialog.Multiselect = false;

            //Call the ShowDialog method to show the dialog box.
            DialogResult result = packetCaptureDialog.ShowDialog();

            //Process input if the user clicked OK.
            if (result == DialogResult.OK)
            {
                fileName = packetCaptureDialog.FileName;
                txtConsoleLog.AppendText("Succesfully opened " + fileName + "\n");
                if (backgroundWorkerLoadFile.IsBusy != true)
                {
                    //LoadingForm
                    alert = new LoadingForm();
                    //Event handler for the Cancel button in LoadingForm
                    alert.Canceled += new EventHandler<EventArgs>(buttonCancel_Click);
                    //Show LoadingForm
                    alert.Show();
                    //Start the asynchronous operation
                    backgroundWorkerLoadFile.RunWorkerAsync();
                }
            }
        }


        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            menuLoadFile();
        }

        /// <summary>
        /// This method deals with what happens when the user presses the quit menu item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //Setup Messagebox to yes/no format
                MessageBoxButtons confirm = MessageBoxButtons.YesNo;
                //Giving the Messagebox a custom message
                string exitconfirm = "Are you sure you want to quit?";
                //Assigning the messagebox a header/title
                string exitconfirm2 = "Quit";
                //Creates a form dialog result for recording the result from the button press
                DialogResult confirmexit;
                //Giving the dialog result a value based on the messagebox results
                confirmexit = MessageBox.Show(exitconfirm, exitconfirm2, confirm);
                //Checking if the yes button was pressed and closing the application if so.
                if (confirmexit == System.Windows.Forms.DialogResult.Yes)
                {
                    Application.Exit();
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// What to do when the user clicks the user manual tool strip item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void userManualToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
        }

        /// <summary>
        /// What to do if the about toolstrip menu item is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            menuAboutclicked();
        }

        /// <summary>
        /// Resize the groupboxes on the form to allow for a responsive design
        /// </summary>
        private void resizeGroupBoxes()
        {
            // GB1
            double gbW1 = tabControl1.Width * 0.6;                  // Setting the width to 60% of tabControl1
            double gbH1 = tabControl1.Height * 0.95;                // Setting the height to 90% tabControl1
            double gbx1 = 1;                                        // X coord for GroupBox 1
            double gby1 = 1;                                        // Y Coord for GroupBox 1
            Point gb1 = new Point((int)gbx1, (int)gby1);             // Location point for GroupBox 1
            groupBoxPort1.Width = (int)gbW1;                        // Assigning gb1's width
            groupBoxPort1.Height = (int)gbH1;                       // Assigning gb1's height
            groupBoxPort1.Location = gb1;                           // Assigning GroupBox1's location

            //GB3
            double gbW2 = (tabControl1.Width * 0.4) - 29;            // Setting the width of Gb3 to 40% of the tabControl1
            double gbH2 = tabControl1.Height * 0.95;                 // Setting the height of Gb3 to 90% of the tabControl1
            double gbx2 = (tabControl1.Width * 0.6) + 10;           // X coord for GroupBox3, in relation to tabControl1
            double gby2 = 1;                                        // Y coord for GroupBox3, in relation to tabControl1
            Point gb = new Point((int)gbx2, (int)gby2);              // Creating a point to be used as the location of gb3 based on gb1's position
            groupBoxOV2.Width = (int)gbW2;                          // Assigning gb3's width
            groupBoxOV2.Height = (int)gbH2;                         // Assigning gb3's height
            groupBoxOV2.Location = gb;                              // Assigning gb3's location to the point GB

            //ListviewOV
            double lvOVx = (groupBoxOV2.Height * 0.01) + 2;         // Variable to hold ListView x coord, in relation to GroupBox3
            double lvOVy = (groupBoxOV2.Width * 0.05) - 8;          // Variable to hold ListView y coord, in relation to GroupBox3
            double lvOVH = groupBoxOV2.Height * 0.6;                // Varable to hold Listview Height, in relation to GroupBox3
            double lvOVW = groupBoxOV2.Width * 0.97;                // Variable to hold List view Width, in relation to GroupBox3
            Point lvOV = new Point((int)lvOVx, (int)lvOVy);          // Point object to store ListView's Location
            listViewOV.Location = lvOV;                             // Setting listview location
            listViewOV.Height = (int)lvOVH;                         // Setting Listview Height
            listViewOV.Width = (int)lvOVW;                          // Setting ListView Width

            // RichTextBoxOV
            double rtbOVx = 7;                                      // Variable to hold RichTextBox x coord, in relation to GroupBox3
            double rtbOVy = lvOVH + lvOVy + 10;                     // Variable to hold RichTextBox y coord, in relation to GroupBox3
            double rtbOVH = groupBoxOV2.Height * 0.3 + 20;            // Variable to hold RichTextBox Height, in relation to GroupBox3
            double rtbOVW = groupBoxOV2.Width * 0.97;               // Variable to hold RichTextBox Width, in relation to GroupBox3
            Point rtbOV = new Point((int)rtbOVx, (int)rtbOVy);       // Point object to store the RichTextBox location
            richTextBoxOV.Location = rtbOV;                         // Setting RTB location
            richTextBoxOV.Height = (int)rtbOVH;                     // Setting RTB Height
            richTextBoxOV.Width = (int)rtbOVW;                      // Setting RTB Width

            //Anchor console text area to the right
            txtConsoleLog.Width = groupBoxConsole.Width - 20;

            //Resize controls within port tabs
            foreach (Tuple<PortTab, TrafficSample> portTab in tabpages)
            {
                portTab.Item1.resizeControls();
            }
            //Current selected tab as an integer
            int selected = tabControl1.SelectedIndex - 1;
            //If the selected tab isn't the overview and isn't an invalid number
            if (selected >= 0 && tabpages.Count > selected)
            {
                //Get the red line panels and packet list groupbox from the port tab object
                List<Panel> linePanels = tabpages[selected].Item1.getLinePanels();
                GroupBox packetListGroupBox = tabpages[selected].Item1.getPacketListGroupbox();
                //Remove all the red lines
                foreach (Panel panel in linePanels)
                {
                    packetListGroupBox.Controls.Remove(panel);
                }
                //Clear the red lines
                linePanels.Clear();
                //Counter
                int count = 0;
                //Go through all the packets in the traffic sample for this tab
                foreach (Packet packet in tabpages[selected].Item2.getPackets())
                {
                    //If the packet has an error
                    if (packet.hasError() == true)
                    {
                        //Get the list view for this specific tab
                        ListView listView = tabpages[selected].Item1.getPacketListView();
                        //Draw a red line on top of the scrollbar
                        int x = listView.Location.X + 647;
                        int y = listView.Location.Y + 20;
                        int drawY = (int)((float)(listView.Height - 35) * ((float)count / (float)sample.getPackets().Count));
                        y += drawY;
                        Panel pan = new Panel();
                        pan.Enabled = false;
                        pan.Width = 15;
                        pan.Height = 1;
                        pan.Location = new Point(x, y);
                        pan.BackColor = Color.Red;
                        packetListGroupBox.Controls.Add(pan);
                        pan.BringToFront();
                        //Store the panel as such that it can be hidden when we switch tabs
                        linePanels.Add(pan);
                    }
                    count++;
                }
            }
        }

        /// <summary>
        /// Method that handles the resizing of the form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
	    private void Form1_Resize(object sender, EventArgs e)
        {
            //Resize the tabpage
            resizeGroupBoxes();
        }

        /// <summary>
        /// Layout event for Form1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Layout(object sender, LayoutEventArgs e)
        {
            //Resize the tabpage
            resizeGroupBoxes();
        }


        /// <summary>
        /// Method that handles what happen when the user selects average data rate in view menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataRateOverTimeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            graphTypeIndex = 0;
            startBackgroundWorkerCreateGraph();
        }

        /// <summary>
        /// Method that handles what happen when the user selects error locations in view menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void errorLocationsInTeTrafficToolStripMenuItem_Click(object sender, EventArgs e)
        {
            graphTypeIndex = 1;
            startBackgroundWorkerCreateGraph();
        }

        /// <summary>
        /// Method that handles what happen when the user selects unxepcted data values in view menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void unexpectedDataValuesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            graphTypeIndex = 2;
            startBackgroundWorkerCreateGraph();
        }

        /// <summary>
        /// Method that handles what happen when the user selects packet rate in view menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void packetRateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            graphTypeIndex = 3;
            startBackgroundWorkerCreateGraph();
        }

        /// <summary>
        /// 
        /// </summary>
        private void startBackgroundWorkerCreateGraph()
        {
            //Save the users selected graph
            updateViewMenuSelectedGraph();
            //Display the graphs
            displayHTML(webBrowser, createOverviewGraphHTML(graphTypeIndex));
            //Update the graphs in the tabs
            updateTabGraphs();
        }

        /// <summary>
        /// Save the users selected graph choice in the view menu toolstrip
        /// </summary>
        private void updateViewMenuSelectedGraph()
        {
            //Save the users selected graph index
            overviewGraphContent.setSelectedGraphIndex(graphTypeIndex);
            //Change which menu items are selected based off of what the user selected
            switch (graphTypeIndex)
            {
                case 0:
                    {
                        dataRateOverTimeToolStripMenuItem.Checked = true;
                        errorLocationsInTheTrafficToolStripMenuItem.Checked = false;
                        unexpectedDataValuesToolStripMenuItem.Checked = false;
                        packetRateToolStripMenuItem.Checked = false;
                        break;
                    }
                case 1:
                    {
                        dataRateOverTimeToolStripMenuItem.Checked = false;
                        errorLocationsInTheTrafficToolStripMenuItem.Checked = true;
                        unexpectedDataValuesToolStripMenuItem.Checked = false;
                        packetRateToolStripMenuItem.Checked = false;
                        break;
                    }
                case 2:
                    {
                        dataRateOverTimeToolStripMenuItem.Checked = false;
                        errorLocationsInTheTrafficToolStripMenuItem.Checked = false;
                        unexpectedDataValuesToolStripMenuItem.Checked = true;
                        packetRateToolStripMenuItem.Checked = false;
                        break;
                    }
                case 3:
                    {
                        dataRateOverTimeToolStripMenuItem.Checked = false;
                        errorLocationsInTheTrafficToolStripMenuItem.Checked = false;
                        unexpectedDataValuesToolStripMenuItem.Checked = false;
                        packetRateToolStripMenuItem.Checked = true;
                        break;
                    }
            }
        }

        /// <summary>
        /// Display the HTML in the provided browser
        /// </summary>
        /// <param name="browser"></param>
        /// <param name="html"></param>
        private void displayHTML(WebBrowser browser, string html)
        {
            browser.Navigate("about:blank");
            browser.Document.OpenNew(false);
            browser.Document.Write(html);
            browser.Refresh();
        }

        private void userManualToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            userManual(); // Calls the user manual function
        }

        void userManual()
        {
            try
            {
                MessageBoxButtons userManconfirm = MessageBoxButtons.YesNoCancel;
                //Giving the Messagebox a custom message
                string exitconfirm = "This file will open in the broswer. \n Continue?";
                //Assigning the messagebox a header/title
                string exitconfirm2 = "Confirm";
                //Creates a form dialog result for recording the result from the button press
                DialogResult confirmexit;
                //Giving the dialog result a value based on the messagebox results
                confirmexit = MessageBox.Show(exitconfirm, exitconfirm2, userManconfirm);
                //Checking if the yes button was pressed and closing the application if so.
                if (confirmexit == System.Windows.Forms.DialogResult.Yes)
                {
                    System.Diagnostics.Process.Start(debugFolderPath + "user.html");
                    // Causes the system to start the users default browser
                }

            }
            catch { }
        }

    }
}

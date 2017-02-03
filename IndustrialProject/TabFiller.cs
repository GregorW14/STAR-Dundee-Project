using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IndustrialProject
{
    /// <summary>
    /// The class that returns controls filled with data for the port tab to display
    /// </summary>
    class TabFiller
    {
        /// <summary>
        /// The traffic sample this class will pull the data from
        /// </summary>
        private TrafficSample sample;
        /// <summary>
        /// The list of packets from the sample
        /// </summary>
        private List<Packet> packets;

        /// <summary>
        /// Constructor of the class, setting passed sample to the instants variable
        /// </summary>
        /// <param name="sample"></param>
        public TabFiller(TrafficSample sample)
        {
            this.sample = sample;
            this.packets = sample.getPackets();
        }

        /// <summary>
        /// Pass a set of labels in and set the text information in them before passing them back.
        /// </summary>
        /// <param name="dataRateLabel"></param>
        /// <param name="errorCountLabel"></param>
        /// <param name="packetCountLabel"></param>
        /// <param name="startTimeLabel"></param>
        /// <param name="endTimeLabel"></param>
        /// <param name="durationLabel"></param>
        /// <param name="packetRateLabel"></param>
        /// <param name="dataCharLabel"></param>
        /// <param name="errorRateLabel"></param>
        /// <returns></returns>
        public Label[] fillTabLabels(Label dataRateLabel, Label errorCountLabel, Label packetCountLabel, Label startTimeLabel, Label endTimeLabel, Label durationLabel, Label packetRateLabel, Label dataCharLabel, Label errorRateLabel)
        {
            //Fill in all the labels
            packetCountLabel.Text = packets.Count.ToString();
            dataRateLabel.Text = sample.getDataRate().ToString() + " (bit/s)";
            packetCountLabel.Text = sample.getPackets().Count.ToString();
            durationLabel.Text = sample.getDuration().ToString(@"hh\:mm\:ss\.fff");
            errorCountLabel.Text = sample.getErrorCount().ToString();
            packetRateLabel.Text = sample.getPacketRate().ToString();
            //Get the error rate, packet count and errorcount
            float errorcount = sample.getErrorCount();
            float packetcount = sample.getPackets().Count;
            float errorRate = (errorcount / packetcount) * 100;
            //Display the error rate and number of data characters
            errorRateLabel.Text = errorRate.ToString() + "%";
            dataCharLabel.Text = (sample.getTotalPacketListSize() / 8).ToString();
            //If the sample recording isn't valid/missing
            if (sample.getStartTime().Equals(new DateTime(0)))
            {
                //Use the first packets time
                startTimeLabel.Text = sample.getPackets()[0].getTime() + " (Missing, used first packet time instead)";
            }
            else
            {
                startTimeLabel.Text = sample.getStartTime().ToString();
            }
            //Same thing with the end date
            if (sample.getEndTime().Equals(new DateTime(0)))
            {
                endTimeLabel.Text = sample.getPackets()[packets.Count - 1].getTime() + " (Missing, used last packet time instead)";
            }
            else
            {
                endTimeLabel.Text = sample.getEndTime().ToString();
            }

            //Return the filled in labels
            return new Label[] { dataRateLabel, errorCountLabel, packetCountLabel, startTimeLabel, endTimeLabel, durationLabel, packetRateLabel, dataCharLabel, errorRateLabel };
        }

        /// <summary>
        /// Fill in the visualisation box for that tab
        /// </summary>
        /// <param name="webBrowser"></param>
        /// <param name="parentLocation"></param>
        /// <param name="groupBox"></param>
        /// <returns></returns>
        public Object[] fillVisualisationBox(WebBrowser webBrowser, Point parentLocation, GroupBox groupBox)
        {
            //If the webbrowser isn't 
            return webBrowser != null ? new Object[] { webBrowser } : null;
        }

        /// <summary>
        /// Return a packet list view filled with list items
        /// </summary>
        /// <param name="lstViewOV"></param>
        /// <param name="listItems"></param>
        /// <returns></returns>
        public PacketListView fillOverViewListBox(PacketListView lstViewOV, ListViewItem[] listItems)
        {
            //Go through each of the items and clone them, adding them to the new listview
            foreach (ListViewItem item in listItems)
            {
                ListViewItem cloneditem = (ListViewItem)item.Clone();
                cloneditem.SubItems.Add(sample.getSourcePort().ToString());
                lstViewOV.Items.Add(cloneditem);
            }
            //Sort the listview
            lstViewOV.Sort();
            return lstViewOV;
        }

        /// <summary>
        /// Fill the packet list and the packet context box
        /// </summary>
        /// <param name="packetContentTextBox"></param>
        /// <param name="packetListView"></param>
        /// <param name="parentLocation"></param>
        /// <param name="groupBox"></param>
        /// <param name="linepanels"></param>
        /// <returns></returns>
        public Object[] fillPackeListAndContentBox(RichTextBox packetContentTextBox, ListView packetListView, Point parentLocation, GroupBox groupBox, List<Panel> linepanels/*List<ListViewItem> listitems*/)
        {
            //If the packet content text box and the packet list view aren't null
            if (packetContentTextBox != null && packetListView != null)
            {
                //Set the count and error count to 0
                int count = 0;
                int errorCount = 0;
                //Go through every packet
                foreach (Packet packet in packets)
                {
                    //create a new list view item
                    ListViewItem item = new ListViewItem();
                    //Add the time to it
                    item.Text = packet.getTime().ToString() + "." + packet.getTime().Millisecond.ToString();
                    //Declare a new list of sublistviewitems
                    List<ListViewItem.ListViewSubItem> subItems = new List<ListViewItem.ListViewSubItem>();
                    //Add a new sublistviewitem
                    subItems.Add(new ListViewItem.ListViewSubItem());
                    //Add the addresss string to it
                    subItems[0].Text = packet.getAddressStr();
                    subItems.Add(new ListViewItem.ListViewSubItem());
                    int sourcePort = packet.getPort();

                    //Add the source port as a subitem
                    if (sourcePort == -1)
                    {
                        subItems[1].Text = "Not found (missing from recording file)";
                    }
                    else
                    {
                        subItems[1].Text = sourcePort.ToString();
                    }

                    //Add the sequence number as a sublistview item if possible
                    subItems.Add(new ListViewItem.ListViewSubItem());
                    if (packet.getSequenceNumber() == -1)
                    {
                        subItems[2].Text = "N/A";
                    }
                    else
                    {
                        subItems[2].Text = packet.getSequenceNumber().ToString();
                    }

                    //Add the protocol as a sublist view item
                    subItems.Add(new ListViewItem.ListViewSubItem());
                    subItems[3].Text = sample.getProtocolStr(packet.getProtocol());
                    RMAP rmap = packet.getRMAP();
                    if (rmap != null)
                    {
                        subItems[3].Text += " (" + rmap.getShortTypeStr() + ")";
                    }

                    //Add the data length as a sublist view item
                    subItems.Add(new ListViewItem.ListViewSubItem());
                    subItems[4].Text = packet.getDataLength().ToString();

                    //Add the type of error as a sublist view item
                    subItems.Add(new ListViewItem.ListViewSubItem());
                    string errorStr = "";
                    if (packet.getEEP() == true)
                    {
                        errorStr += "EEP, ";
                    }
                    if (packet.getParity() == true)
                    {
                        errorStr += "Parity, ";
                    }
                    else if (packet.getNone() == true)
                    {
                        errorStr += "Timeout, ";
                    }
                    //If it's an rmap packet
                    if (rmap != null)
                    {
                        //Check if crc is valid
                        int crcResult = rmap.isCRCValid();
                        if (crcResult != 0)
                        {
                            //If if its, add the heeader, data or data and header
                            errorStr += "CRC (";

                            if (crcResult == 1)
                            {
                                errorStr += "header";
                            }
                            else if (crcResult == 2)
                            {
                                errorStr += "data";
                            }
                            else if (crcResult == 3)
                            {
                                errorStr += "header, data";
                            }
                            errorStr += "), ";
                        }
                    }
                    if (packet.getInvalidAddress() == true)
                    {
                        errorStr += "Invalid Address, ";
                    }
                    if (packet.getInvalid() == true)
                    {
                        errorStr += "Invalid data, ";
                    }
                    if (packet.getInvalidProtocol() == true)
                    {
                        errorStr += "Invalid protocol, ";
                    }
                    //If we're no longer on the first packet
                    if (count > 0)
                    {
                        //If the current packets squence number doesn't equal this one's minus one AND this packets sequce number isn't invalid
                        if (packets[count - 1].getSequenceNumber() != packet.getSequenceNumber() - 1 && packet.getSequenceNumber() != -1)
                        {
                            //If the previouss packets sequence number is the same as this one's
                            if (packets[count - 1].getSequenceNumber() == packet.getSequenceNumber())
                            {
                                //It's a repeat packet
                                errorStr += "Repeat, ";
                                packet.setRepeat(true);
                            }
                            else
                            {
                                //Otherwise it's an out of sequence packet
                                errorStr += "Out of sequence, ";
                                packet.setOutOfSequence(true);
                            }
                        }
                    }
                    //If the error string ends with a comma, remove it
                    if (errorStr.EndsWith(", "))
                    {
                        errorStr = errorStr.Remove(errorStr.Length - 2, 2);
                    }
                    //Add the error string for this subitem
                    subItems[5].Text = errorStr;
                    //If the packet has an error
                    if (errorStr != "")
                    {
                        //Highlight the list item red
                        item.BackColor = Color.Red;

                        //Draw a red line above the scrollbar
                        int x = packetListView.Location.X + 647;
                        int y = packetListView.Location.Y + 20;
                        int drawY = (int)((float)(packetListView.Height - 35) * ((float)count / (float)sample.getPackets().Count));
                        y += drawY;
                        /*
                        http://www.codeproject.com/Questions/301044/Drawing-line-above-all-the-controls-in-the-form
                        */
                        Panel pan = new Panel();
                        pan.Enabled = false;
                        pan.Width = 15;
                        pan.Height = 1;
                        pan.Location = new Point(x, y);
                        pan.BackColor = Color.Red;
                        //Add the panel to the groupbox
                        groupBox.Controls.Add(pan);
                        //Store the panel as such that it can be hidden when we switch tabs
                        linepanels.Add(pan);
                    }
                    //Add all the subitems to the listview item
                    foreach (ListViewItem.ListViewSubItem subItem in subItems)
                    {
                        item.SubItems.Add(subItem);
                    }
                    //If the packet has an error, increase the overall error count by tone
                    if (packet.hasError()) { errorCount++; }
                    //Add the list view item to the packet list view
                    packetListView.Items.Add(item);
                    //Increment the counter
                    count++;
                }
                //Todo: Display average data rate (After data rate has been found)
                sample.setErrorCount(errorCount);
                //Todo: Display average data rate (After data rate has been found)
                return new Object[] { packetContentTextBox, packetListView, linepanels/*listitems*//*linePanels, tabControl1*/};
            }
            else
            {
                return null;
            }
        }
    }
}

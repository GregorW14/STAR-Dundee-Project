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
    /// This is a custom controm that inherits from the ListView control
    /// </summary>
    class PacketListView : ListView
    {
        /// <summary>
        /// The text box that will show the contents of the packets in this pist view
        /// </summary>
        private RichTextBox partnerTextBox;
        /// <summary>
        /// The list of packets this list view has
        /// </summary>
        private List<Packet> packets;
        /// <summary>
        /// Checkbox that will dictate whether or not to show binary or hex in the partner text box
        /// </summary>
        private bool checkboxChecked = false;

        /// <summary>
        /// Constants used for identifying errors
        /// </summary>
        private const int TYPE_WRITE_COMMAND_FORMAT = 0;
        private const int TYPE_WRITE_REPLY_FORMAT = 1;
        private const int TYPE_READ_COMMAND_FORMAT = 2;
        private const int TYPE_READ_REPLY_FORMAT = 3;

        /// <summary>
        /// Integer representing the selected index of this list view, set to the equivalent of null really
        /// </summary>
        private int SelectedIndex = -1;


        /// <summary>
        /// Constructor for the packet list view
        /// </summary>
        public PacketListView()
        {
            //Register the event handler for selecting a packet
            this.SelectedIndexChanged += new EventHandler(this.SelectPacket);
        }


        /// <summary>
        /// Set the packet list in this packet list view, using the parameter
        /// </summary>
        /// <param name="packets"></param>
        public void setPacketList(List<Packet> packets)
        {
            this.packets = packets;
        }


        /// <summary>
        /// Set the partner text box, using the passed referenced textbox
        /// </summary>
        /// <param name="txtbox"></param>
        public void setPartnerBox(ref RichTextBox txtbox)
        {
            this.partnerTextBox = txtbox;
        }

        /// <summary>
        /// Method for converting Hex to binary
        /// </summary>
        /// <param name="part"></param>
        /// <returns></returns>
        public string convertToBinary(string part)
        {
            byte[] byteStr = Encoding.UTF8.GetBytes(part);
            string binaryStr = Convert.ToString(byteStr[0], 2).PadLeft(8, '0');
            return binaryStr;
        }

        /// <summary>
        /// Method of setting the checkbox
        /// </summary>
        /// <param name="state"></param>
        public void setCheckbox(bool state)
        {
            checkboxChecked = state;
        }


        /// <summary>
        /// Event handler for deciding what to do when a packet is selected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectPacket(object sender, EventArgs e)
        {
            //If there actually is packets and there is a partner text box
            if (packets != null && partnerTextBox != null)
            {
                //If there actually is a selected selected index
                if (this.SelectedIndices.Count > 0)
                {
                    //Get the first selected index
                    this.SelectedIndex = this.SelectedIndices[0];
                    //Set the textbox contents to blank
                    partnerTextBox.Text = "";
                    //Get the packet at this index
                    Packet packet = packets[this.SelectedIndices[0]];
                    //Get the byte string from this packet and make it capital letters
                    string byteStr = packet.getOriginalData().ToUpper();
                    //Split the string apart by white spaces
                    string[] parts = byteStr.Split(' ');

                    int count = 0;
                    //Foreach byte
                    foreach (string part in parts)
                    {
                        //
                        partnerTextBox.Select(partnerTextBox.TextLength, 0);
                        //If the byte is longer than two characters mark it as erroneous (red)
                        if (part.Length > 2 || !packet.isByteStrValid(part))
                        {
                            partnerTextBox.SelectionBackColor = Color.Red;
                        }
                        //If it's a sequence number packet, make it green
                        else if (count == packet.getSequenceNumberPosition())
                        {
                            partnerTextBox.SelectionBackColor = Color.LightGreen;
                        }
                        //If the user has not checked the checkbox
                        if (checkboxChecked == false)
                        {
                            //Put the hex byte into it
                            partnerTextBox.AppendText(part);
                        }
                        else
                        {
                            //Otherwise they want the binary byte
                            partnerTextBox.AppendText(convertToBinary(part));
                        }
                        //Make the background colour for the byte white
                        partnerTextBox.Select(partnerTextBox.TextLength, 0);
                        partnerTextBox.SelectionBackColor = Color.White;
                        //Add a space
                        partnerTextBox.AppendText(" ");
                        count++;
                    }
                    //Get the RMAP info for this packet
                    RMAP rmap = packet.getRMAP();
                    //If the packet is actually rmap
                    if (rmap != null)
                    {
                        //The rmap packet info
                        bool[] packetInfo = rmap.getPacketInfo();
                        //Get the packet length as an array of shorts
                        short[] packetInfoShort = new short[packetInfo.Length];
                        //For the length of the packet info, convert the contents to 16 bit integers
                        for (int i = 0; i < packetInfo.Length; i++)
                        {
                            packetInfoShort[i] = Convert.ToInt16(packetInfo[i]);
                        }
                        //Write RMAP information into the text box
                        partnerTextBox.AppendText("\n\n");
                        partnerTextBox.AppendText("RMAP Packet Type: " + rmap.getTypeStr());
                        partnerTextBox.AppendText("\nBits in packet type/command/address byte:");
                        partnerTextBox.AppendText("\n\tReserved: " + packetInfoShort[0]);
                        //Get the RMAP type
                        int type = rmap.getType();
                        //Dependining on the RMAP type, add more information into the text box
                        if (type == TYPE_WRITE_COMMAND_FORMAT)
                        {
                            partnerTextBox.AppendText("\n\tCommand: " + packetInfoShort[1]);
                            partnerTextBox.AppendText("\n\tWrite: " + packetInfoShort[2]);
                            partnerTextBox.AppendText("\n\tVerify Data: " + packetInfoShort[3]);
                            partnerTextBox.AppendText("\n\tACK: " + packetInfoShort[4]);
                        }
                        else if (type == TYPE_WRITE_REPLY_FORMAT)
                        {
                            partnerTextBox.AppendText("\n\tResponse: " + packetInfoShort[1]);
                            partnerTextBox.AppendText("\n\tWrite: " + packetInfoShort[2]);
                            partnerTextBox.AppendText("\n\tVerify data: " + packetInfoShort[3]);
                            partnerTextBox.AppendText("\n\tACK: " + packetInfoShort[4]);
                        }
                        else if (type == TYPE_READ_COMMAND_FORMAT)
                        {
                            partnerTextBox.AppendText("\n\tCommand: " + packetInfoShort[1]);
                            partnerTextBox.AppendText("\n\tRead (1): " + packetInfoShort[2]);
                            partnerTextBox.AppendText("\n\tRead (2): " + packetInfoShort[3]);
                            partnerTextBox.AppendText("\n\tRead (3 (Ack/No Ack)): " + packetInfoShort[4]);
                        }
                        else if (type == TYPE_READ_REPLY_FORMAT)
                        {
                            partnerTextBox.AppendText("\n\tResponse: " + packetInfoShort[1]);
                            partnerTextBox.AppendText("\n\tRead (1): " + packetInfoShort[2]);
                            partnerTextBox.AppendText("\n\tRead (2): " + packetInfoShort[3]);
                            partnerTextBox.AppendText("\n\tRead (3): " + packetInfoShort[4]);
                        }
                        //Add final information from the short array to the text box
                        partnerTextBox.AppendText("\n\tIncrement address: " + packetInfoShort[5]);
                        partnerTextBox.AppendText("\n\tSource path address length (LS): " + packetInfoShort[6]);
                        partnerTextBox.AppendText("\n\tSource path address length (MS): " + packetInfoShort[7]);
                        partnerTextBox.AppendText("\n\nRMAP Fields:");

                        //Go through each of the info key value pairs in the rmap get content return
                        foreach (KeyValuePair<string, byte> info in rmap.getContent())
                        {
                            //Add it to the texbox
                            string key = info.Key;
                            if (key != "packetInfo")
                            {
                                partnerTextBox.AppendText("\n" + info.Key + ": " + info.Value);
                            }
                        }
                        //List of bytes that is the rmap data
                        List<Byte> rmapData = rmap.getData();
                        //Show these bytes if there is any
                        if (rmapData.Count > 0)
                        {
                            partnerTextBox.AppendText("\n\nRMAP Data:\n");
                            for (int i = 0; i < rmapData.Count; i++)
                            {
                                partnerTextBox.AppendText(rmapData[i].ToString("X"));

                                if (i != rmapData.Count - 1)
                                {
                                    partnerTextBox.AppendText(" ");
                                }
                            }
                        }
                    }
                }
                else
                {
                    //No selected item
                    SelectedIndex = -1;
                }
            }
        }

        /// <summary>
        /// Method that converts the hex text into binary and highlights it
        /// </summary>
        public void changeTextToBinary()
        {
            //If there is packets and a valid packet text box
            if (packets != null && partnerTextBox != null)
            {
                //And the user has selected a packet
                if (this.SelectedIndices.Count > 0)
                {
                    //Get the users selected packet
                    this.SelectedIndex = this.SelectedIndices[0];

                    partnerTextBox.Text = "";
                    Packet packet = packets[this.SelectedIndices[0]];
                    string byteStr = packet.getOriginalData().ToUpper();
                    string[] parts = byteStr.Split(' ');

                    int count = 0;
                    //For each part of the byte, colour it red if it's an error, green if it's a sequence number or white if neither
                    foreach (string part in parts)
                    {
                        partnerTextBox.Select(partnerTextBox.TextLength, 0);

                        if (part.Length > 2 || !packet.isByteStrValid(part))
                        {
                            partnerTextBox.SelectionBackColor = Color.Red;
                        }
                        else if (count == packet.getSequenceNumberPosition())
                        {
                            partnerTextBox.SelectionBackColor = Color.LightGreen;
                        }

                        partnerTextBox.AppendText(convertToBinary(part));
                        partnerTextBox.Select(partnerTextBox.TextLength, 0);
                        partnerTextBox.SelectionBackColor = Color.White;
                        partnerTextBox.AppendText(" ");
                        count++;
                    }
                    //Same as the above, method, get rmap info
                    RMAP rmap = packet.getRMAP();
                    if (rmap != null)
                    {
                        //If it's not null, get the RMAP packet info
                        bool[] packetInfo = rmap.getPacketInfo();
                        short[] packetInfoShort = new short[packetInfo.Length];

                        for (int i = 0; i < packetInfo.Length; i++)
                        {
                            packetInfoShort[i] = Convert.ToInt16(packetInfo[i]);
                        }

                        //Print it out
                        partnerTextBox.AppendText("\n\n");
                        partnerTextBox.AppendText("RMAP Packet Type: " + rmap.getTypeStr());
                        partnerTextBox.AppendText("\nBits in packet type/command/address byte:");
                        partnerTextBox.AppendText("\n\tReserved: " + packetInfoShort[0]);
                        //Depending ong the RMAP type, print some more information about the packet out
                        int type = rmap.getType();
                        if (type == TYPE_WRITE_COMMAND_FORMAT)
                        {
                            partnerTextBox.AppendText("\n\tCommand: " + packetInfoShort[1]);
                            partnerTextBox.AppendText("\n\tWrite: " + packetInfoShort[2]);
                            partnerTextBox.AppendText("\n\tVerify Data: " + packetInfoShort[3]);
                            partnerTextBox.AppendText("\n\tACK: " + packetInfoShort[4]);
                        }
                        else if (type == TYPE_WRITE_REPLY_FORMAT)
                        {
                            partnerTextBox.AppendText("\n\tResponse: " + packetInfoShort[1]);
                            partnerTextBox.AppendText("\n\tWrite: " + packetInfoShort[2]);
                            partnerTextBox.AppendText("\n\tVerify data: " + packetInfoShort[3]);
                            partnerTextBox.AppendText("\n\tACK: " + packetInfoShort[4]);
                        }
                        else if (type == TYPE_READ_COMMAND_FORMAT)
                        {
                            partnerTextBox.AppendText("\n\tCommand: " + packetInfoShort[1]);
                            partnerTextBox.AppendText("\n\tRead (1): " + packetInfoShort[2]);
                            partnerTextBox.AppendText("\n\tRead (2): " + packetInfoShort[3]);
                            partnerTextBox.AppendText("\n\tRead (3 (Ack/No Ack)): " + packetInfoShort[4]);
                        }
                        else if (type == TYPE_READ_REPLY_FORMAT)
                        {
                            partnerTextBox.AppendText("\n\tResponse: " + packetInfoShort[1]);
                            partnerTextBox.AppendText("\n\tRead (1): " + packetInfoShort[2]);
                            partnerTextBox.AppendText("\n\tRead (2): " + packetInfoShort[3]);
                            partnerTextBox.AppendText("\n\tRead (3): " + packetInfoShort[4]);
                        }
                        //Final information about packet
                        partnerTextBox.AppendText("\n\tIncrement address: " + packetInfoShort[5]);
                        partnerTextBox.AppendText("\n\tSource path address length (LS): " + packetInfoShort[6]);
                        partnerTextBox.AppendText("\n\tSource path address length (MS): " + packetInfoShort[7]);
                        //Print out the RMAP fields
                        partnerTextBox.AppendText("\n\nRMAP Fields:");
                        foreach (KeyValuePair<string, byte> info in rmap.getContent())
                        {
                            string key = info.Key;
                            if (key != "packetInfo")
                            {
                                partnerTextBox.AppendText("\n" + info.Key + ": " + info.Value);
                            }
                        }
                        //Print out a list of all the bytes in the RMAP packet
                        List<Byte> rmapData = rmap.getData();
                        if (rmapData.Count > 0)
                        {
                            partnerTextBox.AppendText("\n\nRMAP Data:\n");
                            for (int i = 0; i < rmapData.Count; i++)
                            {
                                partnerTextBox.AppendText(rmapData[i].ToString("X"));

                                if (i != rmapData.Count - 1)
                                {
                                    partnerTextBox.AppendText(" ");
                                }
                            }
                        }
                    }
                }
                else
                {
                    //No selected list item
                    SelectedIndex = -1;
                }
            }
        }

        /// <summary>
        /// Same as bove but Binary to Hex
        /// </summary>
        public void changeBinaryToHex()
        {
            if (packets != null && partnerTextBox != null)
            {
                if (this.SelectedIndices.Count > 0)
                {
                    this.SelectedIndex = this.SelectedIndices[0];

                    partnerTextBox.Text = "";
                    Packet packet = packets[this.SelectedIndices[0]];
                    string byteStr = packet.getOriginalData().ToUpper();
                    string[] parts = byteStr.Split(' ');

                    int count = 0;
                    foreach (string part in parts)
                    {
                        partnerTextBox.Select(partnerTextBox.TextLength, 0);

                        if (part.Length > 2 || !packet.isByteStrValid(part))
                        {
                            partnerTextBox.SelectionBackColor = Color.Red;
                        }
                        else if (count == packet.getSequenceNumberPosition())
                        {
                            partnerTextBox.SelectionBackColor = Color.LightGreen;
                        }

                        partnerTextBox.AppendText(part);
                        partnerTextBox.Select(partnerTextBox.TextLength, 0);
                        partnerTextBox.SelectionBackColor = Color.White;
                        partnerTextBox.AppendText(" ");

                        count++;
                    }

                    RMAP rmap = packet.getRMAP();
                    if (rmap != null)
                    {
                        bool[] packetInfo = rmap.getPacketInfo();
                        short[] packetInfoShort = new short[packetInfo.Length];

                        for (int i = 0; i < packetInfo.Length; i++)
                        {
                            packetInfoShort[i] = Convert.ToInt16(packetInfo[i]);
                        }

                        partnerTextBox.AppendText("\n\n");
                        partnerTextBox.AppendText("RMAP Packet Type: " + rmap.getTypeStr());
                        partnerTextBox.AppendText("\nBits in packet type/command/address byte:");
                        partnerTextBox.AppendText("\n\tReserved: " + packetInfoShort[0]);

                        int type = rmap.getType();
                        if (type == TYPE_WRITE_COMMAND_FORMAT)
                        {
                            partnerTextBox.AppendText("\n\tCommand: " + packetInfoShort[1]);
                            partnerTextBox.AppendText("\n\tWrite: " + packetInfoShort[2]);
                            partnerTextBox.AppendText("\n\tVerify Data: " + packetInfoShort[3]);
                            partnerTextBox.AppendText("\n\tACK: " + packetInfoShort[4]);
                        }
                        else if (type == TYPE_WRITE_REPLY_FORMAT)
                        {
                            partnerTextBox.AppendText("\n\tResponse: " + packetInfoShort[1]);
                            partnerTextBox.AppendText("\n\tWrite: " + packetInfoShort[2]);
                            partnerTextBox.AppendText("\n\tVerify data: " + packetInfoShort[3]);
                            partnerTextBox.AppendText("\n\tACK: " + packetInfoShort[4]);
                        }
                        else if (type == TYPE_READ_COMMAND_FORMAT)
                        {
                            partnerTextBox.AppendText("\n\tCommand: " + packetInfoShort[1]);
                            partnerTextBox.AppendText("\n\tRead (1): " + packetInfoShort[2]);
                            partnerTextBox.AppendText("\n\tRead (2): " + packetInfoShort[3]);
                            partnerTextBox.AppendText("\n\tRead (3 (Ack/No Ack)): " + packetInfoShort[4]);
                        }
                        else if (type == TYPE_READ_REPLY_FORMAT)
                        {
                            partnerTextBox.AppendText("\n\tResponse: " + packetInfoShort[1]);
                            partnerTextBox.AppendText("\n\tRead (1): " + packetInfoShort[2]);
                            partnerTextBox.AppendText("\n\tRead (2): " + packetInfoShort[3]);
                            partnerTextBox.AppendText("\n\tRead (3): " + packetInfoShort[4]);
                        }

                        partnerTextBox.AppendText("\n\tIncrement address: " + packetInfoShort[5]);
                        partnerTextBox.AppendText("\n\tSource path address length (LS): " + packetInfoShort[6]);
                        partnerTextBox.AppendText("\n\tSource path address length (MS): " + packetInfoShort[7]);

                        partnerTextBox.AppendText("\n\nRMAP Fields:");
                        foreach (KeyValuePair<string, byte> info in rmap.getContent())
                        {
                            string key = info.Key;
                            if (key != "packetInfo")
                            {
                                partnerTextBox.AppendText("\n" + info.Key + ": " + info.Value);
                            }
                        }

                        List<Byte> rmapData = rmap.getData();
                        if (rmapData.Count > 0)
                        {
                            partnerTextBox.AppendText("\n\nRMAP Data:\n");
                            for (int i = 0; i < rmapData.Count; i++)
                            {
                                partnerTextBox.AppendText(rmapData[i].ToString("X"));

                                if (i != rmapData.Count - 1)
                                {
                                    partnerTextBox.AppendText(" ");
                                }
                            }
                        }
                    }
                }
                else
                {
                    SelectedIndex = -1;
                }
            }
        }



    }

}



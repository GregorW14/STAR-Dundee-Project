using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndustrialProject
{
    /// <summary>
    /// An instance of this class represents a single traffic sample
    /// Packets are stored as a list of Packet objects
    /// </summary>
    public class TrafficSample
    {
        string uniqueID;
        int selectedGraphIndex;

        //Information found within the traffic sample
        DateTime startTime;
        DateTime endTime;
        TimeSpan duration;
        int sourcePort;

        int errorCount;

        //A list of Packet objects to represent each packet in the sample
        List<Packet> packets = new List<Packet>();

        const int PROTOCOL_RMAP = 1;

        /// <summary>
        /// An empty constructor, used when external access to isByteStrValid() is required
        /// </summary>
        public TrafficSample()
        {
            
        }

        /// <summary>
        /// Initialise the basic overview of the sample
        /// </summary>
        /// <param name="startTime">The start time found within the parsed sample</param>
        /// <param name="endTime">The end time found within the traffic sample</param>
        /// <param name="duration">The duration (time difference) from start to end</param>
        /// <param name="sourcePort">The port found in the sample file</param>
        public TrafficSample(DateTime startTime, DateTime endTime, TimeSpan duration, int sourcePort)
        {
            this.startTime = startTime;
            this.endTime = endTime;
            this.duration = duration;
            this.sourcePort = sourcePort;

            this.uniqueID = Guid.NewGuid().ToString("N"); //Generate a unique ID to identify this sample
            this.selectedGraphIndex = 0;
        }

        /// <summary>
        /// Set this sample's packet list and attempt to find the sequence number position
        /// </summary>
        /// <param name="packets">The list of packets for this sample</param>
        public void setPackets(List<Packet> packets)
        {
            //-1 = not found
            int sequencePosition = -1;

            //Attempt to find a sequence number
            for (int i = 0; i < packets.Count; i++)
            {
                if (packets[i].getProtocol() != PROTOCOL_RMAP)
                {
                    //Check each byte in the packet until we find it
                    List<int> bytes = packets[i].getBytes();
                    for (int j = 0; j < bytes.Count; j++)
                    {
                        //Look for incrementing bytes in the next 5 packets
                        int sequence = bytes[j];
                        for (int k = 0; k < 5; k++)
                        {
                            if (i + k < packets.Count)
                            {
                                if (packets[i + k].getBytes()[j] == sequence + 1)
                                {
                                    if (k == 4)
                                    {
                                        //Sequence number found
                                        sequencePosition = j;
                                        break;
                                    }

                                    sequence++;
                                }
                            }
                            else
                            {
                                break;
                            }
                        }

                        if (sequencePosition != -1) break;
                    }
                }

                if (sequencePosition != -1) break;
            }

            if (sequencePosition != -1)
            {
                foreach (Packet packet in packets)
                {
                    packet.setSequenceNumber(packet.getBytes()[sequencePosition]);
                    packet.setSeuqnceNumberPosition(sequencePosition);
                }
            }

            this.packets = packets.ToList();
        }

        /// <summary>
        /// Accessor method for the packets field
        /// </summary>
        /// <returns>A list of packets for this sample</returns>
        public List<Packet> getPackets()
        {
            return this.packets;
        }

        /// <summary>
        /// Accessor method for the startTime field
        /// </summary>
        /// <returns>The start time for this sample</returns>
        public DateTime getStartTime()
        {
            return this.startTime;
        }

        /// <summary>
        /// Accessor method for the duration field
        /// </summary>
        /// <returns>The duration (time difference) from the start to the end of this sample</returns>
        public TimeSpan getDuration()
        {
            return this.duration;
        }

        /// <summary>
        ///  This function retuns the data rate in bitx per second. 
        /// </summary>
        /// <returns>The sample's data rate in bits/s</returns>
        public double getDataRate()
        {
            return Math.Round((getTotalPacketListSize() / ((packets[packets.Count - 1].getTime()) - packets[0].getTime()).TotalMilliseconds) * 1000, 2);
        }

        /// <summary>
        /// This method returns the total size of all the packets in the traffic sample in bits
        /// </summary>
        /// <returns>the total size of all the packets in the traffic sample in bits</returns>
        public int getTotalPacketListSize()
        {
            int totalsize = 0;
            packets.ForEach(packet => totalsize += packet.getTotalBits());
            return totalsize;
        }

        /// <summary>
        /// Accessor method for the endTime field
        /// </summary>
        /// <returns>The time at which this sample ended</returns>
        public DateTime getEndTime()
        {
            return this.endTime;
        }

        /// <summary>
        /// Calculate the packet rate in packets per second for this sample
        /// </summary>
        /// <returns>The number of packets per second in this sample</returns>
        public double getPacketRate()
        {
            return Math.Round((packets.Count / (packets[packets.Count - 1].getTime()- packets[0].getTime()).TotalMilliseconds) * 1000, 2);
        }


        /// <summary>
        /// Set this sample's error count
        /// </summary>
        /// <param name="errorCount">The value to set</param>
        public void setErrorCount(int errorCount)
        {
            this.errorCount = errorCount;
        }

        /// <summary>
        /// Accessor method for the errorCount field
        /// </summary>
        /// <returns>The number of errors found within the sample</returns>
        public int getErrorCount()
        {
            return this.errorCount;
        }

        /// <summary>
        /// Get this sample's active port
        /// </summary>
        /// <returns>The active port for this sample</returns>
        public int getSourcePort()
        {
            return this.sourcePort;
        }

        /// <summary>
        /// Determine whether or not the given hexidecimal byte string is valid
        /// </summary>
        /// <param name="str">The string to validate</param>
        /// <returns>True if the string is valid</returns>
        public bool isByteStrValid(String str)
        {
            str = str.ToLower();
            char[] validHex = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f' };

            if (str.Length == 2)
            {
                foreach (char c in str)
                {
                    if (validHex.Contains(c) == false)
                    {
                        return false;
                    }
                }
            }
            else
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Get a human readable string representation of the protocol for a given protocol ID
        /// </summary>
        /// <param name="protocol">The protocol ID to find a string for</param>
        /// <returns>A human readable string for the protocol name when available, otherwise the protocol ID as a string</returns>
        public string getProtocolStr(int protocol)
        {
            Dictionary<int, string> protocolNames = new Dictionary<int, string>();
            protocolNames.Add(0, "Extended Protocol Identifier");
            protocolNames.Add(1, "RMAP");
            protocolNames.Add(2, "CCSDS Packet Transfer Protocol");
            protocolNames.Add(238, "GOES-R Reliable Data Delivery Protocol");
            protocolNames.Add(239, "Serial Transfer Universal Protocol");

            if (protocolNames.ContainsKey(protocol))
            {
                return protocolNames[protocol];
            }

            return protocol.ToString();
        }

        /// <summary>
        /// Accessor method for the uniqueID field
        /// </summary>
        /// <returns>The unique ID for this sample</returns>
        public string getUniqueID()
        {
            return uniqueID;
        }

        /// <summary>
        /// Get the index of the graph type to be displayed for this sample
        /// </summary>
        /// <returns>The graph type index for this sample</returns>
        public int getSelectedGraphIndex()
        {
            return selectedGraphIndex;
        }

        /// <summary>
        /// Set the currently displayed graph type for this sample
        /// </summary>
        /// <param name="selectedGraphIndex">The graph index to set</param>
        public void setSelectedGraphIndex(int selectedGraphIndex)
        {
            this.selectedGraphIndex = selectedGraphIndex;
        }
    }
}

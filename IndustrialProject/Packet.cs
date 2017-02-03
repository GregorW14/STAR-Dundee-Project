using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndustrialProject
{
    /// <summary>
    /// An instance of this class parses and stores information about a single packet
    /// </summary>
    public class Packet
    {
        DateTime timeReceived;
        List<int> bytes = new List<int>(); //A list of bytes that represent the packet
        List<int> address = new List<int>(); //A list of addresses found within the packet
        string originalBytes = ""; //A hexidecimal string representation of the data

        //Booleans to indicate if particular error types are present
        bool eep = false;
        bool none = false;
        bool invalidAddress = false;
        bool outOfSequence = false;
        bool repeat = false;
        bool invalid = false;
        bool parity = false;
        bool invalidProtocol = false;
	    
        int port;
        int protocol = -1;
        int protocolLocation = 0;
        int sequenceNumber = -1;
        int sequenceNumberPosition = -1;

        RMAP rmap = null;

        const int ADDRESS_TYPE_PATH = 0;
        const int ADDRESS_TYPE_LOGICAL = 1;

        const int PROTOCOL_RMAP = 1;

        /// <summary>
        /// Parse and store information about the provided packet
        /// </summary>
        /// <param name="timeReceived">The time at which the packet was received</param>
        /// <param name="bytes">The list of bytes representing the packet</param>
        /// <param name="byteStr">The string of hexidecimal bytes that represent the packet</param>
        /// <param name="port">The port number on which the packet was sent/received</param>
        public Packet(DateTime timeReceived, List<int> bytes, string byteStr, int port)
        {
            this.timeReceived = timeReceived;
            this.bytes = bytes;
            this.port = port;
            this.originalBytes = byteStr;

            if (bytes[0] < 32)
            {
                //Path address byte, look for 254

                int count = 0;
                foreach (int value in bytes)
                {
                    address.Add(value);

                    if (value == 254)
                    {
                        protocol = bytes[count + 1];
                        protocolLocation = count + 1;
                        break;
                    }

                    count++;
                }
            }
            else if (bytes[0] < 256)
            {
                //Logical address
                address.Add(bytes[0]);
                protocol = bytes[1];
                protocolLocation = 1;
            }
            else
            {
                //Invalid address
                invalidAddress = true;
            }

            if(protocol != -1)
            {
                if(protocol == PROTOCOL_RMAP)
                {
                    rmap = new RMAP(bytes, protocolLocation);
                }
            }
            else
            {
                invalidProtocol = true;
            }
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
        /// Construct a decimal string representation of the packet data
        /// </summary>
        /// <returns>A decimal string representation of the packet data</returns>
        public string getByteStr()
        {
            string byteStr = "";
            foreach (byte b in bytes)
            {
                byteStr += b.ToString() + " ";
            }

            return byteStr;
        }

        /// <summary>
        /// Construct a hexidecimal string representation of the packet data
        /// </summary>
        /// <returns>A hexidecimal string representation of the packet data</returns>
        public string getHexStr()
        {
            string hexStr = "";

            foreach (int b in bytes)
            {
                hexStr += Convert.ToByte(b).ToString("X") + " ";
            }

            return hexStr.Trim();
        }

        /// <summary>
        /// Get the original string
        /// </summary>
        /// <returns>The data string as found within the original traffic sample file</returns>
        public string getOriginalData()
        {
            return this.originalBytes;
        }

        /// <summary>
        /// Get the length of the data (number of bytes)
        /// </summary>
        /// <returns>The number of bytes that the data consists of</returns>
        public int getDataLength()
        {
            return bytes.Count;
        }

        /// <summary>
        /// Get the time at which the packet was received
        /// </summary>
        /// <returns>The time at which the packet was received</returns>
        public DateTime getTime()
        {
            return timeReceived;
        }

        /// <summary>
        /// Setter method for the eep field
        /// </summary>
        /// <param name="value">The value to set</param>
        public void setEEP(bool value)
        {
            this.eep = value;
        }

        /// <summary>
        /// Accessor method for the eep field
        /// </summary>
        /// <returns>Whether or not the packet has an EEP error</returns>
        public bool getEEP()
        {
            return this.eep;
        }

        /// <summary>
        /// Setter method for the none field
        /// </summary>
        /// <param name="value">The value to set</param>
        public void setNone(bool value)
        {
            this.none = value;
        }

        /// <summary>
        /// Get the total number of bits within the packet
        /// </summary>
        /// <returns>The total number of bits that the packet contains</returns>
        public int getTotalBits()
        {
            return this.bytes.Count * 10;
        }

        /// <summary>
        /// Accessor method for the none field
        /// </summary>
        /// <returns>Whether or not a timeout occurred after this packet</returns>
        public bool getNone()
        {
            return this.none;
        }

        /// <summary>
        /// Accessor method for the port field
        /// </summary>
        /// <returns>Get the port that this packet was recorded on</returns>
        public int getPort()
        {
            return this.port;
        }

        /// <summary>
        /// Accessor method for the protocol field
        /// </summary>
        /// <returns>The protocol ID that corresponds wtih the data in this packet</returns>
        public int getProtocol()
        {
            return protocol;
        }

        /// <summary>
        /// Get a list of addresses that are found within the packet
        /// </summary>
        /// <returns>A comma separated string of address values</returns>
        public string getAddressStr()
        {
            string addresses = "";

            foreach (int a in address)
            {
                addresses += a.ToString() + ", ";
            }

            if (addresses.EndsWith(", "))
            {
                addresses = addresses.Remove(addresses.Length - 2, 2);
            }

            return addresses;
        }

        /// <summary>
        /// Accessor method for the sequence number field
        /// </summary>
        /// <returns>The sequence number of this packet</returns>
        public int getSequenceNumber()
        {
            return sequenceNumber;
        }

        /// <summary>
        /// Setter method for the sequenceNumber field
        /// </summary>
        /// <param name="number">The sequence number to set</param>
        public void setSequenceNumber(int number)
        {
            this.sequenceNumber = number;
        }

        /// <summary>
        /// Accessor method for the sequenceNumberPosition field
        /// </summary>
        /// <returns>The poition of the sequence number in this packet's data</returns>
        public int getSequenceNumberPosition()
        {
            return sequenceNumberPosition;
        }

        /// <summary>
        /// Set this packet's sequence number position
        /// </summary>
        /// <param name="sequenceNumberPosition">The position to set</param>
        public void setSeuqnceNumberPosition(int sequenceNumberPosition)
        {
            this.sequenceNumberPosition = sequenceNumberPosition;
        }

        /// <summary>
        /// Accessor method for the invalidAddress field
        /// </summary>
        /// <returns>Whether or not this packet contains an invalid address</returns>
        public bool getInvalidAddress()
        {
            return invalidAddress;
        }
        
        /// <summary>
        /// Accessor method for the outOfSequence field
        /// </summary>
        /// <returns>Whether or not this packet arrived out of sequence</returns>
        public bool getOutOfSequence()
        {
            return outOfSequence;
        }

        /// <summary>
        /// Accessor method for the repeat field
        /// </summary>
        /// <returns>Whether or not this packet has been sent repeatedly</returns>
        public bool getRepeat()
        {
            return repeat;
        }
        
        /// <summary>
        /// Setter method for the repeat field
        /// </summary>
        /// <param name="value">The value to set</param>
        public void setRepeat(bool value)
        {
            this.repeat = value;
        }

        /// <summary>
        /// Setter method for the outOfSequence field
        /// </summary>
        /// <param name="value">The value to set</param>
        public void setOutOfSequence(bool value)
        {
            outOfSequence = value;
        }

        /// <summary>
        /// Accessor method for the invalid field
        /// </summary>
        /// <returns>Whether or not this packet contains invalid data</returns>
        public bool getInvalid()
        {
            return this.invalid;
        }

        /// <summary>
        /// Setter method for the invalid field
        /// </summary>
        /// <param name="value">The value to set</param>
        public void setInvalid(bool value)
        {
            this.invalid = value;
        }

        /// <summary>
        /// Accessor method for the invalidProtocol field
        /// </summary>
        /// <returns>Whether or not this packet contained an invalid protocol</returns>
        public bool getInvalidProtocol()
        {
            return this.invalidProtocol;
        }

        /// <summary>
        /// Setter method for the invalidProtocol field
        /// </summary>
        /// <param name="value">The value to set</param>
        public void setInvalidProtocol(bool value)
        {
            this.invalidProtocol = value;
        }

        /// <summary>
        /// Accessor method for the parity field
        /// </summary>
        /// <returns>Whether or not this packet contains a parity error</returns>
        public bool getParity()
        {
            return this.parity;
        }

        /// <summary>
        /// Setter method for the parity field
        /// </summary>
        /// <param name="value">The value to set</param>
        public void setParity(bool value)
        {
            this.parity = value;
        }

        /// <summary>
        /// Check if this packet has any errors
        /// </summary>
        /// <returns>True if this packet has an error of any type</returns>
        public bool hasError()
        {
            if(rmap != null)
            {
                if(rmap.isCRCValid() != 0)
                {
                    return true;
                }
            }

            return (invalidAddress || outOfSequence || eep || none || repeat || invalid || invalidProtocol || parity);
        }

        /// <summary>
        /// Accessor method for the bytes field
        /// </summary>
        /// <returns>A list object containing each byte in the packet's data</returns>
        public List<int> getBytes()
        {
            return this.bytes;
        }

        /// <summary>
        /// Accessor method for the rmap field
        /// </summary>
        /// <returns>The rmap object for this packet or null if it is not an RMAP packet</returns>
        public RMAP getRMAP()
        {
            return rmap;
        }
    }
}

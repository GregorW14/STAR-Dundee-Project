using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace IndustrialProject
{
    /// <summary>
    /// This class parses and stores the RMAP fields found within a given packet
    /// </summary>
    public class RMAP
    {
        //Store a key, value pair for each RMAP packet field
        Dictionary<string, byte> content = new Dictionary<string, byte>();

        List<byte> dataBytes = new List<byte>(); //The RMAP packet payload
        List<byte> rmapBytes = new List<byte>(); //All bytes that the RMAP portion of the packet consists of
        List<byte> packetData = new List<byte>(); //The bytes for the entire packet

        int type = -1; //The RMAP packet type (RCF, RRF, WCF or WRF)
        int location = -1; //The position of the first RMAP packet byte
        int protocolLocation = -1; //Protocol number position

        //RMAP packet type identifiers
        const int TYPE_WRITE_COMMAND_FORMAT = 0;
        const int TYPE_WRITE_REPLY_FORMAT = 1;
        const int TYPE_READ_COMMAND_FORMAT = 2;
        const int TYPE_READ_REPLY_FORMAT = 3;

        //CRC lookup table used during CRC calculations
        private byte[] table =
        {
            0x00, 0x91, 0xe3, 0x72, 0x07, 0x96, 0xe4, 0x75,
            0x0e, 0x9f, 0xed, 0x7c, 0x09, 0x98, 0xea, 0x7b,
            0x1c, 0x8d, 0xff, 0x6e, 0x1b, 0x8a, 0xf8, 0x69,
            0x12, 0x83, 0xf1, 0x60, 0x15, 0x84, 0xf6, 0x67,
            0x38, 0xa9, 0xdb, 0x4a, 0x3f, 0xae, 0xdc, 0x4d,
            0x36, 0xa7, 0xd5, 0x44, 0x31, 0xa0, 0xd2, 0x43,
            0x24, 0xb5, 0xc7, 0x56, 0x23, 0xb2, 0xc0, 0x51,
            0x2a, 0xbb, 0xc9, 0x58, 0x2d, 0xbc, 0xce, 0x5f,
            0x70, 0xe1, 0x93, 0x02, 0x77, 0xe6, 0x94, 0x05,
            0x7e, 0xef, 0x9d, 0x0c, 0x79, 0xe8, 0x9a, 0x0b,
            0x6c, 0xfd, 0x8f, 0x1e, 0x6b, 0xfa, 0x88, 0x19,
            0x62, 0xf3, 0x81, 0x10, 0x65, 0xf4, 0x86, 0x17,
            0x48, 0xd9, 0xab, 0x3a, 0x4f, 0xde, 0xac, 0x3d,
            0x46, 0xd7, 0xa5, 0x34, 0x41, 0xd0, 0xa2, 0x33,
            0x54, 0xc5, 0xb7, 0x26, 0x53, 0xc2, 0xb0, 0x21,
            0x5a, 0xcb, 0xb9, 0x28, 0x5d, 0xcc, 0xbe, 0x2f,
            0xe0, 0x71, 0x03, 0x92, 0xe7, 0x76, 0x04, 0x95,
            0xee, 0x7f, 0x0d, 0x9c, 0xe9, 0x78, 0x0a, 0x9b,
            0xfc, 0x6d, 0x1f, 0x8e, 0xfb, 0x6a, 0x18, 0x89,
            0xf2, 0x63, 0x11, 0x80, 0xf5, 0x64, 0x16, 0x87,
            0xd8, 0x49, 0x3b, 0xaa, 0xdf, 0x4e, 0x3c, 0xad,
            0xd6, 0x47, 0x35, 0xa4, 0xd1, 0x40, 0x32, 0xa3,
            0xc4, 0x55, 0x27, 0xb6, 0xc3, 0x52, 0x20, 0xb1,
            0xca, 0x5b, 0x29, 0xb8, 0xcd, 0x5c, 0x2e, 0xbf,
            0x90, 0x01, 0x73, 0xe2, 0x97, 0x06, 0x74, 0xe5,
            0x9e, 0x0f, 0x7d, 0xec, 0x99, 0x08, 0x7a, 0xeb,
            0x8c, 0x1d, 0x6f, 0xfe, 0x8b, 0x1a, 0x68, 0xf9,
            0x82, 0x13, 0x61, 0xf0, 0x85, 0x14, 0x66, 0xf7,
            0xa8, 0x39, 0x4b, 0xda, 0xaf, 0x3e, 0x4c, 0xdd,
            0xa6, 0x37, 0x45, 0xd4, 0xa1, 0x30, 0x42, 0xd3,
            0xb4, 0x25, 0x57, 0xc6, 0xb3, 0x22, 0x50, 0xc1,
            0xba, 0x2b, 0x59, 0xc8, 0xbd, 0x2c, 0x5e, 0xcf,
        };

        /// <summary>
        /// Constructs an RMAP object, parsing and storing each field in the packet
        /// </summary>
        /// <param name="data">A list of bytes found within the packet to be parsed</param>
        /// <param name="protocolLocation">The location of the protocol number in data</param>
        public RMAP(List<int> data, int protocolLocation)
        {
            this.protocolLocation = protocolLocation;
            List<byte> byteData = new List<byte>();

            //Convert the packet data from int to byte
            for (int i = 0; i < data.Count; i++)
            {
                if (i > protocolLocation)
                {
                    rmapBytes.Add(Convert.ToByte(data[i]));
                }

                byteData.Add(Convert.ToByte(data[i]));
                packetData.Add(Convert.ToByte(data[i]));
            }

            //The packet info field is always the first byte
            content.Add("packetInfo", rmapBytes[0]);

            //Convert the packet info field into an 8 bit binary sequence
            bool[] packetInfo = getPacketInfo();

            //Using the sequence, determine the RMAP packet type
            if (packetInfo[0] == false && packetInfo[1] == false && packetInfo[2] == false && packetInfo[3] == false && packetInfo[4] == true)
            {
                type = TYPE_READ_REPLY_FORMAT;
            }
            else if (packetInfo[0] == false && packetInfo[1] == true && packetInfo[2] == false && packetInfo[3] == false && packetInfo[4] == true)
            {
                type = TYPE_READ_COMMAND_FORMAT;
            }
            else if (packetInfo[0] == false && packetInfo[1] == false && packetInfo[2] == true && packetInfo[4] == true)
            {
                type = TYPE_WRITE_REPLY_FORMAT;
            }
            else if (packetInfo[0] == false && packetInfo[1] == true && packetInfo[2] == true)
            {
                type = TYPE_WRITE_COMMAND_FORMAT;
            }

            try
            {
                //Name and store each RMAP field based upon the RMAP packet type
                if (type == TYPE_READ_COMMAND_FORMAT)
                {
                    content.Add("Destination Key", rmapBytes[1]);

                    location = parseAddress(rmapBytes);

                    content.Add("Transaction Identifier (MS)", rmapBytes[location]);
                    content.Add("Transaction Identifier (LS)", rmapBytes[location + 1]);
                    content.Add("Extended Read Address", rmapBytes[location + 2]);
                    content.Add("Read Address 1 (MS)", rmapBytes[location + 3]);
                    content.Add("Read Address 2", rmapBytes[location + 4]);
                    content.Add("Read Address 3", rmapBytes[location + 5]);
                    content.Add("Read Address 4 (LS)", rmapBytes[location + 6]);
                    content.Add("Data Length 1 (MS)", rmapBytes[location + 7]);
                    content.Add("Data Length 2", rmapBytes[location + 8]);
                    content.Add("Data Length 3 (LS)", rmapBytes[location + 9]);
                    content.Add("Header CRC", rmapBytes[location + 10]);
                }
                else if (type == TYPE_READ_REPLY_FORMAT)
                {
                    location = 3;

                    content.Add("Status", rmapBytes[1]);
                    content.Add("Destination Logical Address", rmapBytes[2]);
                    content.Add("Transaction Identifier (MS)", rmapBytes[3]);
                    content.Add("Transaction Identifier (LS)", rmapBytes[4]);
                    content.Add("Reserved", rmapBytes[5]);
                    content.Add("Data Length 1 (MS)", rmapBytes[6]);
                    content.Add("Data Length 2", rmapBytes[7]);
                    content.Add("Data Length 3 (LS)", rmapBytes[8]);
                    content.Add("Header CRC", rmapBytes[9]);

                    //Convert the 24 bit data length into a single 32 bit integer
                    int dataLengthi = content["Data Length 3 (LS)"] + (content["Data Length 2"] << 8) + (content["Data Length 1 (MS)"] << 16);

                    //Using the determined length, store each byte of the data
                    int i;
                    for (i = 0; i < dataLengthi; i++)
                    {
                        dataBytes.Add(Convert.ToByte(rmapBytes[10 + i]));
                    }

                    content.Add("Data CRC", rmapBytes[10 + i]);
                }
                else if (type == TYPE_WRITE_REPLY_FORMAT)
                {
                    content.Add("Status", rmapBytes[1]);
                    content.Add("Destination Logical Address", rmapBytes[2]);
                    content.Add("Transaction Identifier 1", rmapBytes[3]);
                    content.Add("Transaction Identifier 2", rmapBytes[4]);
                    content.Add("Reply CRC", rmapBytes[5]);
                }
                else if (type == TYPE_WRITE_COMMAND_FORMAT)
                {
                    content.Add("Destination Key", rmapBytes[1]);

                    location = parseAddress(byteData);

                    content.Add("Transaction Identifier 1", rmapBytes[location]);
                    content.Add("Transaction Identifier 2", rmapBytes[location + 1]);
                    content.Add("Extended Write Address", rmapBytes[location + 2]);
                    content.Add("Write Address 1 (MS)", rmapBytes[location + 3]);
                    content.Add("Write Address 2", rmapBytes[location + 4]);
                    content.Add("Write Address 3", rmapBytes[location + 5]);
                    content.Add("Write Address 4 (LS)", rmapBytes[location + 6]);
                    content.Add("Data Length 1 (MS)", rmapBytes[location + 7]);
                    content.Add("Data Length 2", rmapBytes[location + 8]);
                    content.Add("Data Length 3 (LS)", rmapBytes[location + 9]);
                    content.Add("Header CRC", rmapBytes[location + 10]);

                    int dataLengthi = content["Data Length 3 (LS)"] + (content["Data Length 2"] << 8) + (content["Data Length 1 (MS)"] << 16);

                    int i;
                    for (i = 0; i < dataLengthi; i++)
                    {
                        dataBytes.Add(rmapBytes[location + 11 + i]);
                    }

                    content.Add("Data CRC", rmapBytes[location + 11 + i]);
                }
            }
            catch (ArgumentOutOfRangeException e)
            {

            }
        }

        /// <summary>
        /// Locates the path and/or logical addresses within the provided packet
        /// </summary>
        /// <param name="bytes">A list of bytes found within the packet to be parsed</param>
        /// <returns>The position of the first byte after the last address byte</returns>
        private int parseAddress(List<byte> bytes)
        {
            int index = 2;
            if (bytes[index] < 32)
            {
                //Path address byte, look for 254
                content.Add("Source Path Address 1", bytes[index]);

                int count = 0;
                for (index = index + 1; index < bytes.Count; index++)
                {
                    if (bytes[index] == 254)
                    {
                        content["Source Logical Address"] = Convert.ToByte(bytes[index]);
                        index++;
                        break;
                    }

                    content["Source Path Address " + (count + 2).ToString()] = Convert.ToByte(bytes[index]);
                    count++;
                }
            }
            else
            {
                //Logical address
                content.Add("Source Logical Address", bytes[index]);
                index++;
            }

            return index;
        }

        /// <summary>
        /// Get RMAP packet type ID
        /// </summary>
        /// <returns>The type of this RMAP packet</returns>
        public int getType()
        {
            return type;
        }

        /// <summary>
        /// Get RMAP packet type as a human readable string
        /// </summary>
        /// <returns>The type of this RMAP packet as a human readable string</returns>
        public string getTypeStr()
        {
            if (type == TYPE_WRITE_COMMAND_FORMAT) return "WCF (Write Command Format)";
            if (type == TYPE_WRITE_REPLY_FORMAT) return "WRF (Write Reply Format)";
            if (type == TYPE_READ_COMMAND_FORMAT) return "RCF (Read Command Format)";
            if (type == TYPE_READ_REPLY_FORMAT) return "RRF (Read Reply Format)";

            return "Unknown";
        }

        /// <summary>
        /// Get RMAP packet type as a short string
        /// </summary>
        /// <returns>The type of this RMAP packet as a short string</returns>
        public string getShortTypeStr()
        {
            if (type == TYPE_WRITE_COMMAND_FORMAT) return "WCF";
            if (type == TYPE_WRITE_REPLY_FORMAT) return "WRF";
            if (type == TYPE_READ_COMMAND_FORMAT) return "RCF";
            if (type == TYPE_READ_REPLY_FORMAT) return "RRF";

            return "Unknown";
        }

        /// <summary>
        /// Convert this packet's status field to an 8 bit binary sequence
        /// </summary>
        /// <returns>An 8 bit binary sequence representing the status field</returns>
        public bool[] getPacketInfo()
        {
            if (content.ContainsKey("packetInfo"))
            {
                //http://stackoverflow.com/questions/6758196/convert-int-to-a-bit-array-in-net
                BitArray infoFields = new BitArray(new byte[] { content["packetInfo"] });
                bool[] bits = new bool[infoFields.Count];
                infoFields.CopyTo(bits, 0);
                bool[] reversed = new bool[bits.Length];

                int count = 0;
                for (int i = (bits.Length - 1); i >= 0; i--)
                {
                    reversed[count] = bits[i];
                    count++;
                }

                return reversed;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Get this RMAP packet's fields
        /// </summary>
        /// <returns>A key value pair representing each field in the RMAP packet</returns>
        public Dictionary<string, byte> getContent()
        {
            return content;
        }

        /// <summary>
        /// Get this RMAP packet's payload
        /// </summary>
        /// <returns>The RMAP packet's payload as a list of bytes</returns>
        public List<Byte> getData()
        {
            return dataBytes;
        }

        /// <summary>
        /// Calculate the CRC value of the given byte array
        /// </summary>
        /// <returns>The result of the CRC calculation</returns>
        private byte ComputeChecksum(params byte[] buffer)
        {
            if (buffer == null) throw new ArgumentNullException();
            byte crc = 0;
            for (int i = 0; i < buffer.Length; ++i)
            {
                crc = (byte)(table[(crc ^ buffer[i])]);
            }
            return crc;
        }

        /// <summary>
        /// An internal function to check get this packet's data CRC result
        /// </summary>
        /// <returns>The CRC result for this packet's data</returns>
        private byte checkDataCRC()
        {
            byte[] buffer = new byte[dataBytes.Count];
            for (int i = 0; i < dataBytes.Count; i++)
            {
                buffer[i] = dataBytes[i];
            }

            return ComputeChecksum(buffer);
        }
        
        /// <summary>
        /// Check if this packet's header or data CRC is invalid
        /// </summary>
        /// <returns>
        /// 0: All checks passed or CRCs were not present
        /// 1: Header CRC check failed
        /// 2: Data CRC check failed
        /// 3: Data and header CRC check failed
        /// </returns>
        public int isCRCValid()
        {
            if (!content.ContainsKey("Data CRC") && !content.ContainsKey("Header CRC") && !content.ContainsKey("Reply CRC"))
            {
                return 0;
            }

            //Depending upon the RMAP packet type, the data and/or header should be checked when present
            int result = 0;
            if (type == TYPE_READ_COMMAND_FORMAT)
            {
                byte expected = content["Header CRC"];
                byte[] headerBuffer = new byte[packetData.Count - protocolLocation];

                int count = 0;
                for (int i = protocolLocation - 1; count < packetData.Count - (protocolLocation - 1) - 1; i++)
                {
                    headerBuffer[count] = packetData[i];
                    count++;
                }

                if (ComputeChecksum(headerBuffer) != expected)
                {
                    result += 1;
                }
            }
            else if (type == TYPE_READ_REPLY_FORMAT)
            {
                byte expected = content["Header CRC"];
                int crcLocation = (location + 10);
                byte[] headerBuffer = new byte[packetData.Count - protocolLocation - dataBytes.Count - 1];

                int count = 0;
                for (int i = protocolLocation - 1; count < headerBuffer.Length; i++)
                {
                    headerBuffer[count] = packetData[i];
                    count++;
                }

                if (ComputeChecksum(headerBuffer) != expected)
                {
                    result += 1;
                }

                expected = content["Data CRC"];
                if (checkDataCRC() != expected)
                {
                    result += 2;
                }
            }
            else if (type == TYPE_WRITE_COMMAND_FORMAT)
            {
                byte expected = content["Header CRC"];
                int crcLocation = (location + 10);
                byte[] headerBuffer = new byte[packetData.Count - protocolLocation - dataBytes.Count - 1];

                int count = 0;
                for (int i = protocolLocation - 1; count < headerBuffer.Length; i++)
                {
                    headerBuffer[count] = packetData[i];
                    count++;
                }

                if (ComputeChecksum(headerBuffer) != expected)
                {
                    result += 1;
                }

                expected = content["Data CRC"];
                if (checkDataCRC() != expected)
                {
                    result += 2;
                }
            }
            else if (type == TYPE_WRITE_REPLY_FORMAT)
            {
                byte expected = content["Reply CRC"];
                byte[] headerBuffer = new byte[packetData.Count - protocolLocation];

                int count = 0;
                for (int i = protocolLocation - 1; count < packetData.Count - (protocolLocation - 1) - 1; i++)
                {
                    headerBuffer[count] = packetData[i];
                    count++;
                }

                if (ComputeChecksum(headerBuffer) != expected)
                {
                    result += 1;
                }
            }

            return result;
        }
    }
}
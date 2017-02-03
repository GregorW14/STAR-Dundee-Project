using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace
IndustrialProject
{
    /// <summary>
    /// Provides the logic to parse a traffic sample, therefore constructing a TrafficSample object
    /// </summary>
    class Parser
    {
        public Parser()
        {
        }

        /// <summary>
        /// Parse the given traffic sample file
        /// </summary>
        /// <param name="filePath">The path to the file that should be parsed</param>
        /// <param name="backgroundWorker1">A backgroundworked object for progress bar updates</param>
        /// <param name="currenttabs">A list of currently open port numbers</param>
        /// <returns>A traffic sample on sucess or null on failure</returns>
        public TrafficSample parse(string filePath, BackgroundWorker backgroundWorker1, List<int> currenttabs)
        {
            TrafficSample sample = new TrafficSample();
            StreamReader reader = new StreamReader(filePath);
            DateTime fileTime = DateTime.Now;
            DateTime endTime = DateTime.Now;
            int sourcePort = 0;
            string tempLine;

            ArrayList lines = new ArrayList();

            //Remove blank lines as well as newlines after EOP and EEP markers
            while ((tempLine = reader.ReadLine()) != null)
            {
                if (tempLine.Length > 0)
                {
                    if (tempLine != "P")
                    {
                        tempLine += "\n";
                    }
                    if (tempLine == "EOP\n" || tempLine == "EEP\n")
                    {
                        lines[lines.Count - 1] = lines[lines.Count - 1].ToString().Trim();
                    }
                    lines.Add(tempLine);
                }
            }

            List<Packet> packets = new List<Packet>();
            int lineCount = 0;
            foreach (string line in lines)
            {
                //Update the loading progress bar
                int percentage = (int)Math.Floor(((float)lineCount / lines.Count) * 100);
                backgroundWorker1.ReportProgress(percentage);

                if (line.Length > 0)
                {
                    if (lineCount == 0)
                    {
                        //The first line should be a timestamp (start of packet capture)
                        try
                        {
                            fileTime = DateTime.Parse(line);
                        }
                        catch (FormatException e)
                        {
                            fileTime = new DateTime(0);
                        }
                    }
                    else if (lineCount == 1)
                    {
                        //The second line should be the port number of the file
                        try
                        {
                            sourcePort = Convert.ToInt32(line);
                            //If there's already a tab for this sourceport
                            if(currenttabs.Contains(sourcePort))
                            {
                                return null;
                            }
                        }
                        catch (FormatException e)
                        {
                            sourcePort = -1;
                        }
                    }
                    else
                    {
                        DateTime packetTime = DateTime.Now;
                        try
                        {
                            if (line.Trim() != "E")
                            {
                                //Not end of file
                                packetTime = DateTime.Parse(line);

                                List<int> bytes = new List<int>();
                                if (lines.Count - lineCount > 3)
                                {
                                    //Not the last three lines
                                    string dataLine = lines[lineCount + 1].ToString() + lines[lineCount + 2].ToString() + lines[lineCount + 3].ToString().Trim();

                                    if (dataLine.StartsWith("P") && (dataLine.EndsWith("EOP") || dataLine.EndsWith("EEP") || dataLine.EndsWith("None")))
                                    {
                                        //Packet data can be found between the start of packet marker and an end of packet/error marker
                                        bool eep = false;
                                        bool none = false;
                                        bool invalid = false;

                                        if (dataLine.EndsWith("EEP"))
                                        {
                                            eep = true;
                                        }
                                        else if (dataLine.EndsWith("None"))
                                        {
                                            none = true;
                                            dataLine = dataLine.Remove(dataLine.Length - 1, 1);
                                        }

                                        //Remove the end marker from the string
                                        dataLine = dataLine.Remove(0, 1);
                                        dataLine = dataLine.Remove(dataLine.Length - 3, 3);

                                        //Parse each individual byte of data
                                        string[] byteParts = dataLine.Split(' ');
                                        foreach (string byteStr in byteParts)
                                        {
                                            String thisByte = byteStr.Trim().ToLower();

                                            if (invalid == false)
                                            {
                                                invalid = !sample.isByteStrValid(thisByte);
                                            }

                                            int dataByte = 0;
                                            try
                                            {
                                                dataByte = Convert.ToInt32(thisByte, 16);
                                            }
                                            catch (FormatException exception)
                                            {
                                                //Invalid format such as "ZZ"
                                                //Leave this byte as 0
                                            }
                                            catch (ArgumentOutOfRangeException exception)
                                            {
                                                //Empty strings after splitting due to extra spaces in the data string
                                            }

                                            bytes.Add(dataByte);
                                        }

                                        //Setup and store the Packet object
                                        Packet packet = new Packet(packetTime, bytes, dataLine, sourcePort);
                                        packet.setEEP(eep);
                                        packet.setNone(none);
                                        packet.setInvalid(invalid);

                                        packets.Add(packet);
                                    }
                                    else
                                    {
                                        //Invalid line
                                    }
                                }
                            }

                            if (line.Trim() == "E") //End of file expected
                            {
                                string word = lines[lineCount + 1].ToString().Trim();
                                if(word == "Disconnect")
                                {
                                    //Disconnect found
                                    try
                                    {
                                        if (lineCount + 2 > lines.Count - 1)
                                        {
                                            throw new FormatException("End date missing");
                                        }
                                        endTime = DateTime.Parse(lines[lineCount + 2].ToString().Trim());
                                    }
                                    catch (FormatException ex)
                                    {
                                        endTime = new DateTime(0);
                                    }
                                }
                                else if (word == "Parity")
                                {
                                    packets[packets.Count - 1].setParity(true);
                                }
                            }
                        }
                        catch (FormatException exception)
                        {
                            Console.Write("Parse Exception: " + exception.Message);
                            //this.Close();
                        }
                    }
                }
                lineCount++;
            }
            reader.Close();

            TimeSpan duration = endTime.Subtract(fileTime);
            sample = new TrafficSample(fileTime, endTime, duration, sourcePort);
            sample.setPackets(packets);
            return sample;
        }
    }
}

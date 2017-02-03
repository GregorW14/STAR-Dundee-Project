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
    /// This class is used to format the data to be read into the JavaScript visualisations
    /// </summary>
    static class JSON
    {
        /// <summary>
        /// This formats the given TrafficSample into a format for the data rate over time visualisation (line graph)
        /// </summary>
        /// <param name="trafficSample"></param>
        /// <param name="Width"></param>
        /// <returns></returns>
        public static string[] formatModelToJSON_DataRateOverTime(TrafficSample trafficSample, int Width)
        {
            List <Packet> packets = trafficSample.getPackets();
            string[] output = new string[2];
            float interval = (float) Math.Round((packets[packets.Count - 1].getTime() - packets[0].getTime()).TotalMilliseconds,2) / getIntervalNumber(Width);
        
            //The datapoints 
            List<Tuple<float,List<float>>> datapoints = new List<Tuple<float, List<float>>>();
            //For every interval...
            for (int i= 0; i <= (getIntervalNumber(Width) + 1); i++)
            {
                //Store what that interval actually is in the datapoints tuple
                datapoints.Add(new Tuple<float,List<float>>((float) Math.Round(interval * i,2), new List<float>()));
            }
            //For each of the packets
            foreach (Packet packet in trafficSample.getPackets())
            {
                //Go through the datapoints
                for (int i = 0; i < datapoints.Count - 1; i++)
                {
                    //MessageBox.Show(datapoints[i].Item1.ToString());
                    ////The time of the packets occurance in MS
                    float timeofpacket = (float)(packet.getTime() - packets[0].getTime()).TotalMilliseconds;
                    ////If it's in the interval
                    if (timeofpacket >= datapoints[i].Item1 &&  timeofpacket < datapoints[i + 1].Item1)
                    {
                        //MessageBox.Show(timeofpacket.ToString());
                        //Add the total number of bits of that packet to the list for that interval
                        datapoints[i].Item2.Add(packet.getTotalBits());
                        break;
                    }
                }
            }
            //The array of data rate points that we will use to populate the graph
            float[,] dataratepoints = new float[datapoints.Count,2];
            //Counter for the foreach
            int x = 0;
            //Calculate the datarate
            foreach (Tuple<float,List<float>> tuple in datapoints)
            {
                float totalbits = 0;
                tuple.Item2.ForEach(item => totalbits += item);
                //MessageBox.Show(totalbits.ToString());
                //The data rate per second for this particular area of code 
                dataratepoints[x, 0] = tuple.Item1;
                dataratepoints[x,1]= totalbits * (100 / interval);  //This line is probably wrong
                x++;
            }

            //Okay, now let's start actually making the important parts of the JSON 
            string catagories = "[";
            string series = ", data: [";

            //MessageBox.Show(packetscount.ToString());
            for (int i = 0; i < dataratepoints.GetLength(0); i++) {
                //If it's the first item in the arrya
                if (i != 0)
                {
                    //If it's the last index, close the JSON array, otherwise add to it
                    catagories += (i == dataratepoints.GetLength(0) - 1) ? "," + dataratepoints[i, 0].ToString() + "]" : "," + dataratepoints[i, 0].ToString();
                    series += (i == dataratepoints.GetLength(0) - 1) ? "," + dataratepoints[i, 1].ToString() + "]}" : "," + dataratepoints[i, 1].ToString();
                } else
                {
                    //Just add it to the JSON array
                    catagories += dataratepoints[i, 0].ToString();
                    series += dataratepoints[i, 1].ToString();
                }
            }
            string[] returnjson = { catagories, series };
            return returnjson;
        }

        /// <summary>
        /// Get the interval number, used on the xaxis of the graphs
        /// </summary>
        /// <param name="Width"></param>
        /// <returns></returns>
        private static int getIntervalNumber(int Width)
        {
            float widthpercentage = ((float) Width / 1920) * 10;
            return (int) Math.Round(widthpercentage,0);
        }

        /// <summary>
        /// This formats the given TrafficSample into a format for the error locations in the traffic visualisation (bar graph)
        /// </summary>
        /// <param name="trafficSample"></param>
        /// <returns></returns>
        public static string[] formatModelToJSON_ErrorLocationsInTheTraffic(TrafficSample trafficSample)
        {
            // HTML file format, data example:
            /*
            colors:['#FF2922', '#2269FF', '#90ed7d', '#f7a35c', '#8085e9','#f15c80', '#e4d354', '#2b908f', '#f45b5b', '#91e8e1']
                {
                    name: 'Error packets',
                    data: [2]
                }, {
                    name: 'Accepted packets',
                    data: [14]
                }
            */

            string[] output = new string[2];
            string data = "{";
            string colours = "colors:[";
            List<Packet> packets = trafficSample.getPackets();

            // Start format data string
            if (packets[packets.Count - 1].hasError())
            {
                data += "name: 'Error packets', data: [";
            }
            else
            {
                data += "name: 'Accepted packets',data: [";
            }

            // For every packet
            int counter = 0;
            Boolean previousError = false;
            for (int i = packets.Count - 1; i > -1; i--)
            {
                // If the current packet contains an error
                if (packets[i].hasError())
                {
                    if (!previousError)
                    {
                        data += counter;

                        counter = 0;
                        previousError = true;

                        data += "]}, {name: 'Error packets', data: [";
                        colours += ",'#d94343'";
                    }

                    counter++;
                }
                else
                {
                    if (previousError)
                    {
                        data += counter;

                        counter = 0;
                        previousError = false;

                        data += "]}, {name: 'Valid packets',data: [";

                        colours += ",'#56c648'";
                    }

                    counter++;
                }
            }

            data += counter + "]}";
            colours += "]";

            output[0] = data;
            output[1] = colours;

            return output;
        }


        /// <summary>
        /// This formats the given TrafficSample into a format for any unexpected data values visualisation (bar graph)
        /// </summary>
        /// <param name="trafficSample"></param>
        /// <returns></returns>
        public static string[] formatModelToJSON_UnexpectedDataValues(TrafficSample trafficSample)
        {
            // HTML file format, data example:
            /*
            colors:['#FF2922', '#2269FF', '#90ed7d', '#f7a35c', '#8085e9','#f15c80', '#e4d354', '#2b908f', '#f45b5b', '#91e8e1']

                {
                    name: 'Error packets',
                    data: [2]
                }, {
                    name: 'Accepted packets',
                    data: [14]
                }
            */

            string[] output = new string[2];
            string data = "";
            string colours = "";
            List<Packet> packets = new List<Packet>();
                
            // Get packets
            packets = trafficSample.getPackets();

            int incorrectSequenceNumber = 0;
            int headerCRC = 0;
            int dataCRC = 0;
            int headerAndDataCRC = 0;
            
            // For every packet
            for (int i = packets.Count - 1; i > -1; i--)
            {
                // If the current packet has an out of sequence error
                if (packets[i].getOutOfSequence()) // Out of sequence error
                {
                    incorrectSequenceNumber++;
                }
                
                try
                {
                    RMAP rmap = packets[i].getRMAP();

                    // Depending on the output of the isCRCValid method for the current packet
                    /*
                       * Return values:
                       * 0: All checks passed or CRCs were not present
                       * 1: Header CRC check failed
                       * 2: Data CRC check failed
                       * 3: Data and header CRC check failed
                   */
                    switch (rmap.isCRCValid())
                    {
                        case 0:
                            {
                                // No problems
                                break;
                            }
                        case 1:
                            {
                                // Header CRC error
                                headerCRC++;
                                break;
                            }
                        case 2:
                            {
                                // Data CRC error
                                dataCRC++;
                                break;
                            }
                        case 3:
                            {
                                // Header and data CRC
                                headerAndDataCRC++;
                                break;
                            }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("### ERROR rmap ###\n" + e.Message);
                }
            }

            // Format data for graph
            data = "{name: 'Incorrect sequence numbers',data: [" +
                incorrectSequenceNumber
                + "]}, {name: 'Header CRC',data: [" +
                headerCRC +
                "]}, {name: 'Data CRC',data: [" +
                dataCRC +
                "]}, {name: 'Header and data CRC',data: [" +
                headerAndDataCRC +
                "]}";
            
            colours += "colors:['#FF2922', '#2269FF', '#FF2922', '#FF2922']";

            output[0] = data;
            output[1] = colours;

            return output;
        }

        /// <summary>
        /// This formats the given TrafficSample into a format for the packet rate over time visualisation (line graph)
        /// </summary>
        /// <param name="trafficSample"></param>
        /// <param name="Width"></param>
        /// <returns></returns>
        public static string[] formatModelToJSON_PacketRate(TrafficSample trafficSample, int Width)
        {
            List<Packet> packets = trafficSample.getPackets();
            string[] output = new string[2];
            //The interval in miliseconds, based off the width of the form
            float interval = (float)Math.Round((packets[packets.Count - 1].getTime() - packets[0].getTime()).TotalMilliseconds, 2) / getIntervalNumber(Width);
            //The datapoints 
            float[,] dataratepoints = new float[getIntervalNumber(Width) + 1, 2];
            
            //For every interval...
            for (int i = 0; i < dataratepoints.GetLength(0); i++)
            {
                //Store the information we need in our 2d array
                dataratepoints[i, 0] = (float)Math.Round(interval * i, 2);
                dataratepoints[i, 1] = 0; 
            }
            
            //For each of the packets
            foreach (Packet packet in trafficSample.getPackets())
            {
                
                //Go through the datapoints
                for (int i = 0; i < dataratepoints.GetLength(0) - 1; i++)
                {
                    //The time of the packets occurance in MS
                    float timeofpacket = (float)(packet.getTime() - packets[0].getTime()).TotalMilliseconds;
                    //If it's in the interval
                    if (timeofpacket >= dataratepoints[i,0] && timeofpacket < dataratepoints[i + 1,0])
                    {
                        //Increment the number of packets passed during this interval
                        dataratepoints[i, 1]++;
                        break;
                    }
                }
            }
            for (int i = 0; i < dataratepoints.GetLength(0); i++)
            {
                dataratepoints[i, 1] = (dataratepoints[i, 1] * (100 / interval)) * 10;
            }

            //Okay, now let's start actually making the important parts of the JSON 
            string catagories = "[";
            string series = ", data: [";
            //MessageBox.Show(packetscount.ToString());
            for (int i = 0; i < dataratepoints.GetLength(0); i++)
            {
                //If it's the first item in the arrya
                if (i != 0)
                {
                    //If it's the last index, close the JSON array, otherwise add to it
                    catagories += (i == dataratepoints.GetLength(0) - 1) ? "," + dataratepoints[i, 0].ToString() + "]" : "," + dataratepoints[i, 0].ToString();
                    series += (i == dataratepoints.GetLength(0) - 1) ? "," + dataratepoints[i, 1].ToString() + "]}" : "," + dataratepoints[i, 1].ToString();
                }
                else
                {
                    //Just add it to the JSON array
                    catagories += dataratepoints[i, 0].ToString();
                    series += dataratepoints[i, 1].ToString();
                }
            }
            string[] returnjson = { catagories, series };

            return returnjson;
        }

        /// <summary>
        /// This formats the given TrafficSample into a format for error locations for the multiple graph visualisation (bar graph)
        /// </summary>
        /// <param name="trafficSample"></param>
        /// <returns></returns>
        public static List<string>[] formatModelToJSON_ErrorLocationsInTheTraffic_ForOverview(TrafficSample trafficSample)
        {
            // HTML file format, data example:
            /*
            [{             name: 'Error packets',             data: [2,4]         }, {             name: 'Accepted packets',             data: [14,10]         }]
            */

            List<string>[] output = new List<string>[2];
            output[0] = new List<string>();
            output[1] = new List<string>();
            
            List<Packet> packets = trafficSample.getPackets();
            
            int counter = 0;
            Boolean previousError = false;
            for (int i = packets.Count - 1; i > -1; i--)
            {
                if (packets[i].hasError())
                {
                    if (!previousError)
                    {
                        output[0].Add("false");
                        output[1].Add(counter.ToString());
                        
                        counter = 0;
                        previousError = true;
                    }

                    counter++;
                }
                else
                {
                    if (previousError)
                    {
                        output[0].Add("true");
                        output[1].Add(counter.ToString());

                        counter = 0;
                        previousError = false;
                    }

                    counter++;
                }
            }
            return output;
        }
    }
}

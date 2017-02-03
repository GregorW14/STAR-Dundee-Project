using System;
using System.Collections.Generic;

namespace IndustrialProject
{
    /// <summary>
    /// This class is used as a model for the overview tab
    /// </summary>
    class OverviewGraphContent
    {
        // Fields
        string uniqueID = "";
        int selectedGraphIndex = 0;
        List<string>[] series = new List<string>[4];
        List<string>[] colours = new List<string>[4];
        
        /// <summary>
        /// Constructor
        /// </summary>
        public OverviewGraphContent()
        {
            uniqueID = Guid.NewGuid().ToString("N");
            selectedGraphIndex = 0;
            series[0] = new List<string>();
            series[1] = new List<string>();
            series[2] = new List<string>();
            series[3] = new List<string>();

            colours[0] = new List<string>();
            colours[1] = new List<string>();
            colours[2] = new List<string>();
            colours[3] = new List<string>();
        }

        /// <summary>
        /// Get unique id
        /// </summary>
        /// <returns></returns>
        public string getUniqueID()
        {
            return uniqueID;
        }
        
        /// <summary>
        /// Set selected graph index
        /// </summary>
        /// <param name="selectedGraphIndex"></param>
        public void setSelectedGraphIndex(int selectedGraphIndex)
        {
            this.selectedGraphIndex = selectedGraphIndex;
        }

        /// <summary>
        /// Get selected graph index
        /// </summary>
        /// <returns></returns>
        public int getSelectedGraphIndex()
        {
            return selectedGraphIndex;
        }

        /// <summary>
        /// Add to item to series using index and the item to add
        /// </summary>
        /// <param name="graphIndex"></param>
        /// <param name="toAdd"></param>
        public void addToSeries(int graphIndex, string toAdd)
        {
            series[graphIndex].Add(toAdd);
        }

        /// <summary>
        /// Get item from series using graph index and item index
        /// </summary>
        /// <param name="graphIndex"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public string getItemFromSeries(int graphIndex, int index)
        {
            return series[graphIndex][index];
        }

        /// <summary>
        /// Delete item from series using graph index and item index
        /// </summary>
        /// <param name="graphIndex"></param>
        /// <param name="index"></param>
        public void deleteItemFromSeries(int graphIndex, int index)
        {
            series[graphIndex].RemoveAt(index);
        }

        /// <summary>
        /// Get series
        /// </summary>
        /// <param name="graphIndex"></param>
        /// <returns></returns>
        public List<string> getSeries(int graphIndex)
        {
            return series[graphIndex];
        }

        /// <summary>
        /// Clear all items from series
        /// </summary>
        public void clearAllSeries()
        {
            series[0].Clear();
            series[1].Clear();
            series[2].Clear();
            series[3].Clear();
        }

        /// <summary>
        /// Add to colours
        /// </summary>
        /// <param name="graphIndex"></param>
        /// <param name="toAdd"></param>
        public void addToColours(int graphIndex, string toAdd)
        {
            colours[graphIndex].Add(toAdd);
        }

        /// <summary>
        /// Get item from colours
        /// </summary>
        /// <param name="graphIndex"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public string getItemFromColours(int graphIndex, int index)
        {
            return colours[graphIndex][index];
        }

        /// <summary>
        /// Delete item from colours
        /// </summary>
        /// <param name="graphIndex"></param>
        /// <param name="index"></param>
        public void deleteItemFromColours(int graphIndex, int index)
        {
            colours[graphIndex].RemoveAt(index);
        }

        /// <summary>
        /// Get colours
        /// </summary>
        /// <param name="graphIndex"></param>
        /// <returns></returns>
        public List<string> getColours(int graphIndex)
        {
            return colours[graphIndex];
        }

        /// <summary>
        /// Clear all items from colours
        /// </summary>
        public void clearAllColours()
        {
            colours[0].Clear();
            colours[1].Clear();
            colours[2].Clear();
            colours[3].Clear();
        }
    }
}

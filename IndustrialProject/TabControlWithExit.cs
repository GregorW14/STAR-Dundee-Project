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
    /// Custom user control, extends from the TabControl and adds a close button to it
    /// </summary>
    public class TabControlWithExit : TabControl
    {

        /// <summary>
        /// Constructor for this class
        /// </summary>
        public TabControlWithExit()
        {
            this.Padding = new System.Drawing.Point(21, 3);
            this.DrawMode = TabDrawMode.OwnerDrawFixed;
        }


        /// <summary>
        /// Override void that is triggered on the drawitem event
        /// </summary>
        /// <param name="e"></param>
        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            //Modified version of Fun Mun Pieng's code here 
            //http://stackoverflow.com/questions/5338587/set-tabpage-header-color
            //And andycted code here
            //http://stackoverflow.com/questions/3183352/close-button-in-tabcontrol

            //Background color
            Color bgcolor;
            //Fill in the background either white or grey depending on whether or not the tab is selected
            using (Brush br = new SolidBrush(bgcolor = e.Index == this.SelectedIndex ? Color.White : Color.Transparent))
            {
                e.Graphics.FillRectangle(br, e.Bounds);
                SizeF sz = e.Graphics.MeasureString(this.TabPages[e.Index].Text, e.Font);
                Rectangle rect = e.Bounds;
                rect.Offset(0, 1);
                rect.Inflate(0, -1);
                e.DrawFocusRectangle();
                //Don't draw the X for the overview tab
                if (e.Index != 0)
                {
                    //Draw an X on the tab so the user knows where to click to close it
                    e.Graphics.DrawString("x", e.Font, Brushes.Black, e.Bounds.Right - 15, e.Bounds.Top + 4);
                    //Draw the name of the port on the tab header
                    e.Graphics.DrawString(this.TabPages[e.Index].Text, e.Font, Brushes.Black, e.Bounds.Left + 12, e.Bounds.Top + 4);
                    e.DrawFocusRectangle();
                }
                else
                {
                    //Just draw the name of the tab if it's the overview
                    e.Graphics.DrawString(this.TabPages[e.Index].Text, e.Font, Brushes.Black, e.Bounds.Left + 12, e.Bounds.Top + 4);
                }
            }
        }

        /// <summary>
        ///  Event for when the mouse is clicked on the tab control
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseClick(MouseEventArgs e)
        {
            //If the user didn't select the overview page
            if (this.SelectedIndex != 0)
            {
                //Heavily modified version of this code by jonny5
                //http://stackoverflow.com/questions/3183352/close-button-in-tabcontrol

                //Get the tab rectange for the tab control header
                Rectangle r = this.GetTabRect(this.SelectedIndex);
                //Define a rectange region of where the close button would be 
                Rectangle closeButton = new Rectangle(r.Right - 15, r.Top + 4, 10, 20);
                //If the place the user clicked was in this rectangle
                if (closeButton.Contains(e.Location))
                {
                    //Find the form
                    Form1 form = (Form1)this.FindForm();
                    //Delete tab from said form
                    PortTab thistab = (PortTab)this.SelectedTab;
                    form.deleteTab(thistab);
                    form.currentTabs.Remove(thistab.getSourcePort());
                    this.TabPages.Remove(this.SelectedTab);
                    //Clean the overview list
                    form.cleanOverviewList(thistab.getSourcePort());
                }
            }
        }
    }
}

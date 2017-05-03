using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CPM.app
{
    public partial class Form2: Form
    {
        private ListActiv _l;
        private List<int> arr;
        public Form2(ListActiv l, List<int> criticaList)
        {
            InitializeComponent();
            Text = "CPM - wykres Gantta";
            this.arr = criticaList;
            this._l = l;


            tableLayoutPanel1.ColumnCount = _l.listActivity[_l.listActivity.Count-1].Next.EarliestTime ;
            tableLayoutPanel1.RowCount = _l.listActivity.Count;
            
            tableLayoutPanel1.ColumnStyles[0].SizeType = SizeType.Absolute;
            tableLayoutPanel1.ColumnStyles[0].Width = 40;
            for (int i = 0; i < tableLayoutPanel1.ColumnCount; i++)
            {
                
                tableLayoutPanel1.ColumnStyles.Add( new ColumnStyle(SizeType.Absolute, 40));
                tableLayoutPanel1.Controls.Add(new Label() { Text = Convert.ToString(i+1) }, i, 0);
            }

            tableLayoutPanel1.RowStyles[0].SizeType = SizeType.Absolute;
            tableLayoutPanel1.RowStyles[0].Height = 20;

            int h = (tableLayoutPanel1.Height-20) / (tableLayoutPanel1.RowCount + 1);
            for( int i = 0; i < tableLayoutPanel1.RowCount; i++ )
            {
                tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, h));
            }


            


            for( int j = 1; j < tableLayoutPanel1.RowCount+1; j++ )
            {
                for( int i = _l.listActivity[j-1].First.EarliestTime; i < _l.listActivity[j-1].First.EarliestTime + _l.listActivity[j-1].Duration; i++ )
                {
                    PictureBox pictureBox = new PictureBox();
                    foreach (var ar in criticaList)
                    {
                        if (ar == j - 1)
                        {
                            pictureBox.BackColor = Color.Brown;
                            break;
                        }
                        else
                            pictureBox.BackColor = Color.DimGray;

                    }
                    
                    tableLayoutPanel1.Controls.Add(pictureBox, i, j);
                    pictureBox.Dock = DockStyle.Fill;
                    pictureBox.Margin = new Padding(0);
                }

            }

        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}

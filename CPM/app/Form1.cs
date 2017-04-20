using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Providers.Entities;
using System.Windows.Forms;

namespace CPM
{
   
    public partial class Form1: Form
    {
        private ListActiv l = new ListActiv();
        
        public Form1()
        {
            InitializeComponent();
            InitializeOwnerDrawnListBox();

        }
 
        private void InitializeOwnerDrawnListBox()
        {
            
            // Set the DrawMode property to the OwnerDrawVariable value. 
            // This means the MeasureItem and DrawItem events must be 
            // handled.
           listBox1.DrawMode = DrawMode.OwnerDrawVariable;
            //listBox1.MeasureItem +=
            //    new MeasureItemEventHandler(ListBox1_MeasureItem);
            listBox1.DrawItem += new DrawItemEventHandler(ListBox1_DrawItem);
            this.Controls.Add(this.listBox1);

        }


        // Handle the DrawItem event for an owner-drawn ListBox.
        private void ListBox1_DrawItem(object sender, DrawItemEventArgs e)
        {

            // If the item is the selected item, then draw the rectangle
            // filled in blue. The item is selected when a bitwise And  
            // of the State property and the DrawItemState.Selected 
            // property is true.
            if( ( e.State & DrawItemState.Selected ) == DrawItemState.Selected )
            {
                e.Graphics.FillRectangle(Brushes.Red, e.Bounds);
            }
            else
            {
                // Otherwise, draw the rectangle filled in beige.
                e.Graphics.FillRectangle(Brushes.Beige, e.Bounds);
            }

            // Draw a rectangle in blue around each item.
            //e.Graphics.DrawRectangle(Pens.Red, e.Bounds);

            // Draw the text in the item.
            e.Graphics.DrawString(listBox1.Items[e.Index].ToString(),
                this.Font, Brushes.Black, e.Bounds.X, e.Bounds.Y);

            // Draw the focus rectangle around the selected item.
            e.DrawFocusRectangle();
        }

        
        private void enterData_Click(object sender, EventArgs e)
        {

            if (name.Text == "")
            {
                MessageBox.Show("Wprowadź nazwę...");
                return;
            }
            else if (duration.Text == "")
            {
                MessageBox.Show("Wprowadź czas...");
                return;
            }
            else if( begin.Text == "" )
            {
                MessageBox.Show("Wprowadź poczatek...");
                return;
            }
            else if( end.Text == "" )
            {
                MessageBox.Show("Wprowadź koniec...");
                return;
            }

            listBox1.Items.Add("Nazwa: " + name.Text + "  Czas trwania: " + duration.Text + "  Początek: " +
                                   begin.Text + "  Koniec: " + end.Text);

                l.listActivity.Add(new Activity()
                {
                    Name = name.Text,
                    Duration = Convert.ToInt16(duration.Text),
                    Begin = Convert.ToInt16(begin.Text),
                    End = Convert.ToInt16(end.Text)
                });
            
           
        }

        private void findCMP_Click(object sender, EventArgs e)
        {
            
            // ZAD 1
            //example1();

            //ZAD 2
            example2();
            try
            {
                for (int i = 0; i < listBox1.Items.Count; i++)
                {
                    listBox1.SetSelected(i,false);
                }

                

                l.listActivity[0].First.EarliestTime = 0;
                l.listActivity[0].First.LatestTime = 0;
                l.listActivity[0].First.SpareTime = 0;
            

            // WERSJA Z WYSZUKIWANIEM NAJDLUZSZEGO CZASU (IMPROVED)

            int j;
            for( int i = 0; i < l.listActivity.Count; i++ )
            {
                for( j = i + 1; j < l.listActivity.Count; j++ )
                {
                    if( l.listActivity[i].End == l.listActivity[j].Begin )
                    {
                        l.listActivity[j].First.EarliestTime = l.listActivity[i].Duration + l.listActivity[i].First.EarliestTime;

                        var maxDur = maxDuration(l.listActivity[j].Begin);
                        if( l.listActivity[j].First.EarliestTime != maxDur )
                            l.listActivity[j].First.EarliestTime = maxDur;

                    }
                }
            }
            // SPRAWDZANIE DLA OSTATNIEJ CZYNNOSCI W NEXT ZDARZENIU
            j = l.listActivity.Count - 1;

            l.listActivity[j].Next.LatestTime = l.listActivity[j].Next.EarliestTime = maxDuration(l.listActivity[j].End);

            // OBLICZANIE NAJPOZNIJESZYCH MOŻLIWYCH CZASÓW ZAJŚCIA ZDARZENIA
            // DLA PIERWSZEJ CZYNNOSCI OD KONCA:

            l.listActivity[j].First.LatestTime = l.listActivity[j].Next.LatestTime - l.listActivity[j].Duration;

            // DLA POZOSTALYCH
            int sub;
            for( int i = 1; i < l.listActivity.Count - 1; i++ )
            {
                l.listActivity[i].First.LatestTime = 999999;
            }

            for( int i = l.listActivity.Count - 1; i > 0; i-- )
            {
                for( j = 0; j < l.listActivity.Count; j++ )
                {
                    if( l.listActivity[j].End == l.listActivity[i].Begin )
                    {
                        sub = l.listActivity[i].First.LatestTime - l.listActivity[j].Duration;
                        if( l.listActivity[j].First.LatestTime > sub )
                        {
                            l.listActivity[j].First.LatestTime = sub;
                            updateLatestTime(l.listActivity[j].Begin, sub);
                        }
                    }
                }
            }

            // UZUPELNIENIE ZDARZEN
            for( int i = 0; i < l.listActivity.Count; i++ )
            {
                for( int k = i + 1; k < l.listActivity.Count; k++ )
                {
                    if( l.listActivity[i].End == l.listActivity[k].Begin )
                    {
                        l.listActivity[i].Next.EarliestTime = l.listActivity[k].First.EarliestTime;
                        l.listActivity[i].Next.LatestTime = l.listActivity[k].First.LatestTime;
                        l.listActivity[i].Next.SpareTime = l.listActivity[k].First.SpareTime;
                    }
                }
            }

            // UZUPEŁNIANIE/OBLICZENIE ZAPASOWEGO CZASU
            for( int i = 0; i < l.listActivity.Count; i++ )
            {
                l.listActivity[i].First.SpareTime = l.listActivity[i].First.LatestTime - l.listActivity[i].First.EarliestTime;
                l.listActivity[i].Next.SpareTime = l.listActivity[i].Next.LatestTime - l.listActivity[i].Next.EarliestTime;
            }

            foreach( var activ in l.listActivity )
            {
                Console.WriteLine("Nazwa czynnosci: " + activ.Name);
                Console.WriteLine("Czas trwania: " + activ.Duration + "\n");
                Console.WriteLine("Poczatek: " + activ.Begin);
                Console.WriteLine("Najwczesniejszy czas trwania: " + activ.First.EarliestTime);
                Console.WriteLine("Najpozniejszyniejszy czas trwania: " + activ.First.LatestTime);
                Console.WriteLine("Zapas czasu: " + activ.First.SpareTime + "\n");
                Console.WriteLine("Koniec: " + activ.End);
                Console.WriteLine("Najwczesniejszy czas trwania: " + activ.Next.EarliestTime);
                Console.WriteLine("Najpozniejszyniejszy czas trwania: " + activ.Next.LatestTime);
                Console.WriteLine("Zapas czasu: " + activ.Next.SpareTime + "\n\n");
            }

            // WYBIERANIE CZYNNOSCI NA SCIEZKE KRYTYCZNA

            var temp = 0;
            var temp2 = 0;
            string tab = "";
            List<int> arr = new List<int>();
            
            for( int i = 0; i < l.listActivity.Count; i++ )
            {
                temp += l.listActivity[i].Duration;
                if( ( l.listActivity[i].First.SpareTime + l.listActivity[i].Next.SpareTime ) == 0 &&
                    ( temp - l.listActivity[i].Next.EarliestTime ) == 0 )
                {
                    tab += l.listActivity[i].Name + " ";
                    arr.Add(i);
                    temp2 = temp;
                }
                else
                {
                    temp = temp2;
                }
            }
            label10.Text = tab;
            label2.Text = Convert.ToString(temp2);
            
            foreach (var ar in arr)
            {
                listBox1.SetSelected(ar,true);
            }
            }
            catch( Exception )
            {
                MessageBox.Show("Wproawdź czynności!");
            }
        }
       
      

        private void example1()
        {
            l.listActivity.Add(new Activity() {
                Name = "A",
                Duration = 3,
                Begin = 1,
                End = 2
            });
            l.listActivity.Add(new Activity() {
                Name = "B",
                Duration = 4,
                Begin = 2,
                End = 3
            });
            l.listActivity.Add(new Activity() {
                Name = "C",
                Duration = 6,
                Begin = 2,
                End = 4
            });
            l.listActivity.Add(new Activity() {
                Name = "D",
                Duration = 7,
                Begin = 3,
                End = 5
            });
            l.listActivity.Add(new Activity() {
                Name = "E",
                Duration = 1,
                Begin = 5,
                End = 7
            });
            l.listActivity.Add(new Activity() {
                Name = "F",
                Duration = 2,
                //Duration = 4,
                Begin = 4,
                End = 7
            });
            l.listActivity.Add(new Activity() {
                Name = "G",
                Duration = 3,
                Begin = 4,
                End = 6
            });
            l.listActivity.Add(new Activity() {
                Name = "H",
                //Duration = 2,
                Duration = 4,
                Begin = 6,
                End = 7
            });
            l.listActivity.Add(new Activity() {
                Name = "I",
                Duration = 1,
                Begin = 7,
                End = 8
            });
            l.listActivity.Add(new Activity() {
                Name = "J",
                Duration = 2,
                Begin = 8,
                End = 9
            });
            foreach (var act in l.listActivity)
            {
                listBox1.Items.Add("Nazwa: " + act.Name + "  Czas trwania: " + act.Duration + "  Początek: " +
                                   act.Begin + "  Koniec: " + act.End);
            }
        }

        private void example2()
        {
            l.listActivity.Add(new Activity() {
                Name = "A",
                Duration = 6,
                Begin = 1,
                End = 2
            });
            l.listActivity.Add(new Activity() {
                Name = "B",
                Duration = 10,
                Begin = 1,
                End = 3
            });
            l.listActivity.Add(new Activity() {
                Name = "C",
                Duration = 6,
                Begin = 2,
                End = 3
            });
            l.listActivity.Add(new Activity() {
                Name = "D",
                Duration = 12,
                Begin = 2,
                End = 5
            });
            l.listActivity.Add(new Activity() {
                Name = "E",
                Duration = 5,
                Begin = 3,
                End = 4
            });
            l.listActivity.Add(new Activity() {
                Name = "F",
                Duration = 8,
                //Duration = 4,
                Begin = 3,
                End = 5
            });
            l.listActivity.Add(new Activity() {
                Name = "G",
                Duration = 8,
                Begin = 4,
                End = 6
            });
            l.listActivity.Add(new Activity() {
                Name = "H",
                //Duration = 2,
                Duration = 7,
                Begin = 5,
                End = 6
            });
            l.listActivity.Add(new Activity() {
                Name = "I",
                Duration = 8,
                Begin = 5,
                End = 7
            });
            l.listActivity.Add(new Activity() {
                Name = "J",
                Duration = 6,
                Begin = 6,
                End = 7
            });
            l.listActivity.Add(new Activity() {
                Name = "K",
                Duration = 7,
                Begin = 7,
                End = 8
            });
            foreach( var act in l.listActivity )
            {
                listBox1.Items.Add("Nazwa: " + act.Name + "  Czas trwania: " + act.Duration + "  Początek: " +
                                   act.Begin + "  Koniec: " + act.End);
            }
        }

        private int maxDuration(int val)
        {
            var max = 0;

            for( var i = 0; i < l.listActivity.Count; i++ )
            {
                if( l.listActivity[i].End == val )
                {
                    var sum = l.listActivity[i].First.EarliestTime + l.listActivity[i].Duration;
                    if( sum > max )
                        max = sum;
                }
            }
            return max;
        }

        private void updateLatestTime(int val, int dur)
        {
            for( int i = 0; i < l.listActivity.Count; i++ )
            {
                if( l.listActivity[i].Begin == val )
                {
                    l.listActivity[i].First.LatestTime = dur;
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }
        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            // The Remove button was clicked.
            var selectedIndex = listBox1.SelectedIndex;

            try
            {
                // Remove the item in the List.
                listBox1.Items.RemoveAt(selectedIndex);
                l.listActivity.RemoveAt(selectedIndex);
            }
            catch
            {
                MessageBox.Show("Brak elementów...");
            }

            listBox1.DataSource = null;
        }
    }
}

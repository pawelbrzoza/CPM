using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CPM
{
   
    public partial class Form1: Form
    {
        private class Event
        {
            public int EarliestTime { get; set; }

            public int LatestTime { get; set; }

            public int SpareTime { get; set; }
        }

        private class Activity
        {

            public string Name { get; set; }
            public int Duration { get; set; }
            public int Begin { get; set; }
            public int End { get; set; }

            public Event First { get; private set; }
            public Event Next { get; private set; }

            public Activity()
            {
                First = new Event();
                Next = new Event();
            }
        }

      
        List<Activity> listActivity;

        public Form1()
        {
            InitializeComponent();
            listActivity = new List<Activity>();
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void enterData_Click(object sender, EventArgs e)
        {
            
            listBox1.Items.Add(name.Text + ": " + duration.Text + "    " + begin.Text + "    -    " + end.Text);

            listActivity.Add(new Activity() { Name = name.Text, Duration = Convert.ToInt16(duration.Text), Begin = Convert.ToInt16(begin.Text), End = Convert.ToInt16(end.Text)});

        }

        private string tab ="";

        private int maxDuration(int val)
        {
            var max = 0;

            for ( var i = 0; i < listActivity.Count; i++ )
            {
                if ( listActivity[i].End == val)
                {
                    var sum = listActivity[i].First.EarliestTime + listActivity[i].Duration;
                    if (sum > max)
                    {
                        max = sum;
                        
                    }
                }
            }
            return max;
        }

        private int maxPath(int val)
        {
            var max = 0;
            var maxID = 0;

            for( var i = 0; i < listActivity.Count; i++ )
            {
                if( listActivity[i].End == val )
                {
                    var sum = listActivity[i].First.LatestTime + listActivity[i].Duration;
                    if (sum > max)
                    {
                        max = sum;
                        maxID = i;
                    }
                }
            }
            return maxID;
        }

        private void updateLatestTime(int val, int dur)
        {
            for (int i = 0; i < listActivity.Count; i++)
            {
                if (listActivity[i].Begin == val)
                {
                    listActivity[i].First.LatestTime = dur;
                }
            }
        }

        private void findCMP_Click(object sender, EventArgs e)
        {
            // ZAD 1
            listActivity.Add(new Activity() {
                Name = "A",
                Duration = 3,
                Begin = 1,
                End = 2
            });
            listActivity.Add(new Activity() {
                Name = "B",
                Duration = 4,
                Begin = 2,
                End = 3
            });
            listActivity.Add(new Activity() {
                Name = "C",
                Duration = 6,
                Begin = 2,
                End = 4
            });
            listActivity.Add(new Activity() {
                Name = "D",
                Duration = 7,
                Begin = 3,
                End = 5
            });
            listActivity.Add(new Activity() {
                Name = "E",
                Duration = 1,
                Begin = 5,
                End = 7
            });
            listActivity.Add(new Activity() {
                Name = "F",
                Duration = 2,
                //Duration = 4,
                Begin = 4,
                End = 7
            });
            listActivity.Add(new Activity() {
                Name = "G",
                Duration = 3,
                Begin = 4,
                End = 6
            });
            listActivity.Add(new Activity() {
                Name = "H",
                //Duration = 2,
                Duration = 4,
                Begin = 6,
                End = 7
            });
            listActivity.Add(new Activity() {
                Name = "I",
                Duration = 1,
                Begin = 7,
                End = 8
            });
            listActivity.Add(new Activity() {
                Name = "J",
                Duration = 2,
                Begin = 8,
                End = 9
            });

            // ZAD 2
            //listActivity.Add(new Activity() {
            //    Name = "A",
            //    Duration = 6,
            //    Begin = 1,
            //    End = 2
            //});
            //listActivity.Add(new Activity() {
            //    Name = "B",
            //    Duration = 10,
            //    Begin = 1,
            //    End = 3
            //});
            //listActivity.Add(new Activity() {
            //    Name = "C",
            //    Duration = 6,
            //    Begin = 2,
            //    End = 3
            //});
            //listActivity.Add(new Activity() {
            //    Name = "D",
            //    Duration = 12,
            //    Begin = 2,
            //    End = 5
            //});
            //listActivity.Add(new Activity() {
            //    Name = "E",
            //    Duration = 5,
            //    Begin = 3,
            //    End = 4
            //});
            //listActivity.Add(new Activity() {
            //    Name = "F",
            //    Duration = 8,
            //    //Duration = 4,
            //    Begin = 3,
            //    End = 5
            //});
            //listActivity.Add(new Activity() {
            //    Name = "G",
            //    Duration = 8,
            //    Begin = 4,
            //    End = 6
            //});
            //listActivity.Add(new Activity() {
            //    Name = "H",
            //    //Duration = 2,
            //    Duration = 7,
            //    Begin = 5,
            //    End = 6
            //});
            //listActivity.Add(new Activity() {
            //    Name = "I",
            //    Duration = 8,
            //    Begin = 5,
            //    End = 7
            //});
            //listActivity.Add(new Activity() {
            //    Name = "J",
            //    Duration = 6,
            //    Begin = 6,
            //    End = 7
            //});
            //listActivity.Add(new Activity() {
            //    Name = "K",
            //    Duration = 7,
            //    Begin = 7,
            //    End = 8
            //});

            listActivity[0].First.EarliestTime = 0;
            listActivity[0].First.LatestTime = 0;
            listActivity[0].First.SpareTime = 0;
           

            // WERSJA Z WYSZUKIWANIEM NAJDLUZSZEGO CZASU (IMPROVED)

            int j;
            for( int i = 0; i < listActivity.Count; i++ )
            {
                for(j = i + 1; j < listActivity.Count; j++ )
                {
                    if( listActivity[i].End == listActivity[j].Begin )
                    {
                        listActivity[j].First.EarliestTime = listActivity[i].Duration + listActivity[i].First.EarliestTime;

                        var maxDur = maxDuration(listActivity[j].Begin);
                        if( listActivity[j].First.EarliestTime != maxDur )
                        {
                            listActivity[j].First.EarliestTime = maxDur;
                            
                        }
                    }
                }
            }
            // SPRAWDZANIE DLA OSTATNIEJ CZYNNOSCI W NEXT ZDARZENIU
            j = listActivity.Count - 1;
            
            listActivity[j].Next.LatestTime = listActivity[j].Next.EarliestTime = maxDuration(listActivity[j].End);

            // OBLICZANIE NAJPOZNIJESZYCH MOŻLIWYCH CZASÓW ZAJŚCIA ZDARZENIA

            // DLA PIERWSZEJ CZYNNOSCI OD KONCA:

            listActivity[j].First.LatestTime = listActivity[j].Next.LatestTime - listActivity[j].Duration;

            // DLA POZOSTALYCH
            int sub;
            for( int i = 1; i < listActivity.Count - 1; i++ )
            {
                listActivity[i].First.LatestTime = 999999;
            }

            for( int i = listActivity.Count-1; i > 0; i-- )
            {
                for( j = 0; j<listActivity.Count; j++ )
                {
                    if (listActivity[j].End == listActivity[i].Begin)
                    {
                        sub = listActivity[i].First.LatestTime - listActivity[j].Duration;
                        if (listActivity[j].First.LatestTime > sub)
                        {
                            listActivity[j].First.LatestTime = sub;
                            updateLatestTime(listActivity[j].Begin, sub);
                        }
                    }
                   
                }
            }

            // UZUPELNIENIE ZDARZEN
            for (int i = 0; i < listActivity.Count; i++)
            {
                for (int k = i+1; k < listActivity.Count; k++)
                {
                    if (listActivity[i].End == listActivity[k].Begin)
                    {
                        listActivity[i].Next.EarliestTime = listActivity[k].First.EarliestTime;
                        listActivity[i].Next.LatestTime = listActivity[k].First.LatestTime;
                        listActivity[i].Next.SpareTime = listActivity[k].First.SpareTime;
                    }
                }
            }

            // UZUPEŁNIANIE/OBLICZENIE ZAPASOWEGO CZASU
            for( int i = 0; i < listActivity.Count; i++ )
            {
                listActivity[i].First.SpareTime = listActivity[i].First.LatestTime - listActivity[i].First.EarliestTime;
                listActivity[i].Next.SpareTime = listActivity[i].Next.LatestTime - listActivity[i].Next.EarliestTime;
            }

            foreach (var activ in listActivity)
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
            
            int temp = 0;
            int temp2 = 0;
            for( int i = 0; i < listActivity.Count; i++ )
            {
                temp += listActivity[i].Duration;
                if ((listActivity[i].First.SpareTime + listActivity[i].Next.SpareTime) == 0 &&
                    (temp - listActivity[i].Next.EarliestTime) == 0)
                {
                    tab += listActivity[i].Name + " ";
                    temp2 = temp;
                }
                else
                {
                    temp = temp2;
                }
                
            }
            label10.Text = tab;
            

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}

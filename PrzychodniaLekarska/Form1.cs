using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PrzychodniaLekarska
{
    public partial class Form1 : Form
    {
        public LinkedList<Pacjent> wszyscyPacjenci = new LinkedList<Pacjent>();
        public Semaphore dostepGrafikaSemaphore = new Semaphore(1,1);

        public List<Lekarz> lekarze = new List<Lekarz>();

        public int sleepVal = 20;
        /*
        Lekarz lekarz1;
        Lekarz lekarz2;
        Lekarz lekarz3;
        Lekarz lekarz4;*/


        public Form1()
        {
            InitializeComponent();
            int LICZBA_LEKARZY = 3;
            int k = 240;

            for (int i = 0; i < LICZBA_LEKARZY; i++)
            {
                
                Lekarz lekarz = new Lekarz(this, new Point(k, 180), new Point(k, 90));
                lekarz.praca = new Thread(new ThreadStart(lekarz.Pracuj));
                lekarze.Add(lekarz);
                k = k + 300;
            }
            for (int i = 0; i < LICZBA_LEKARZY; i++)
            {
                Lekarz lekarz = lekarze.ElementAt(i);
                Thread generator = new Thread(new ThreadStart(() => generatorPacjentowLekarz(lekarz)));
                generator.Name = "lekarz" + (i + 1);
                generator.Start();
                System.Console.WriteLine(generator.Name);
            }
            foreach (Lekarz lekarz in lekarze)
            {
                lekarz.praca.Start();
            }
            /*
            //Lekarz z lewej
            lekarz1 = new Lekarz(this, new Point(240, 180), new Point(240, 90));
            //this.lekarze.AddLast(lekarz1);
            lekarz1.praca = new Thread(new ThreadStart( lekarz1.Pracuj ));
            //Lekarz w srodku
            lekarz2 = new Lekarz(this, new Point(560, 180), new Point(560, 90));
            //this.lekarze.AddLast(lekarz2);
            lekarz2.praca = new Thread(new ThreadStart(lekarz2.Pracuj));
            //Lekarz z prawej
            lekarz3 = new Lekarz(this, new Point(860, 180), new Point(860, 90));
            //this.lekarze.AddLast(lekarz3);
            lekarz3.praca = new Thread(new ThreadStart(lekarz3.Pracuj));
            //xD
            lekarz4 = new Lekarz(this, new Point(1160, 180), new Point(1160, 90));
           // this.lekarze.AddLast(lekarz4);
            lekarz4.praca = new Thread(new ThreadStart(lekarz4.Pracuj));

            Thread generator1 = new Thread(new ThreadStart(() => generatorPacjentowLekarz(lekarz1)));
            generator1.Name = "lekarz1";
            Thread generator2 = new Thread(new ThreadStart(() => generatorPacjentowLekarz(lekarz2)));
            generator2.Name = "lekarz2";
            Thread generator3 = new Thread(new ThreadStart(() => generatorPacjentowLekarz(lekarz3)));
            generator3.Name = "lekarz3";
            Thread generator4 = new Thread(new ThreadStart(() => generatorPacjentowLekarz(lekarz4)));
            generator4.Name = "lekarz4";

            generator1.Start();
            generator2.Start();
            generator3.Start();
            generator4.Start();

            lekarz1.praca.Start();
            lekarz2.praca.Start();
            lekarz3.praca.Start();
            lekarz4.praca.Start();*/
        }

        private void generatorPacjentowLekarz(Lekarz _lekarz2)
        {
            
            while (true)
            {
                Random rand = new Random();
                int i = 0; //iterator testow
                int bO = 0;
                while (i < 5)
                {
                    //TODO zmienic na losowa ilosc przypisywanych lekarzy
                    Pacjent temp = new Pacjent(_lekarz2, this);
                    //temp.lekarzeDoOdwiedzenia.AddLast( lekarze.Last() );
                    //Badania okresowe


                    bO = rand.Next(1,5);
                    
                    foreach (Lekarz lekarz in lekarze) {
                        
                        for (int j = 0; j < bO;j++)
                        {
                            int randIndex = rand.Next(lekarze.Count);
                            Lekarz kandydat = lekarze.ElementAt(randIndex);
                            if (Thread.CurrentThread.Name.Equals(lekarz.praca.Name))
                            {
                                j--;
                            }
                           
                            if (!temp.lekarzeDoOdwiedzenia.Contains(kandydat))
                            {
                                temp.lekarzeDoOdwiedzenia.AddLast(kandydat);
                                
                            }
                        }

                    }
                   /* 
                    if (Thread.CurrentThread.Name.Equals("lekarz1"))
                    {
                        if (bO == 1) temp.lekarzeDoOdwiedzenia.AddLast(this.lekarze.ElementAt(1));
                        if (bO == 2)
                        {
                            temp.lekarzeDoOdwiedzenia.AddLast(this.lekarze.ElementAt(1));
                            temp.lekarzeDoOdwiedzenia.AddLast(this.lekarze.ElementAt(2));
                        }
                        
                    }
                    else if(Thread.CurrentThread.Name.Equals("lekarz2"))
                    {
                        if (bO == 1) temp.lekarzeDoOdwiedzenia.AddLast(this.lekarze.ElementAt(0));
                        if (bO == 2)
                        {
                            temp.lekarzeDoOdwiedzenia.AddLast(this.lekarze.ElementAt(0));
                            temp.lekarzeDoOdwiedzenia.AddLast(this.lekarze.ElementAt(2));
                        }
                    }
                    else if (Thread.CurrentThread.Name.Equals("lekarz3"))
                    {
                        if (bO == 1) temp.lekarzeDoOdwiedzenia.AddLast(this.lekarze.ElementAt(1));
                        if (bO == 2)
                        {
                            temp.lekarzeDoOdwiedzenia.AddLast(this.lekarze.ElementAt(0));
                            
                        }
                    }
                    */
                    this.dostepGrafikaSemaphore.WaitOne();
                    wszyscyPacjenci.AddLast(temp);
                    this.dostepGrafikaSemaphore.Release();

                    temp.idzT = new Thread(new ThreadStart(() => idzDoKolejki(temp)));
                    temp.idzT.Name = "Pacjent";
                    // sprawdzamy czy kolejka lekarza nie jest przepelniona
                    temp.idzT.Start();
                    Thread.Sleep(3000);

                    i++;
                }
                Thread.Sleep(60000);
            }
        }

        public void idzDoKolejki(Pacjent temp)
        {
            Point dest = new Point();
            Lekarz l = temp.lekarzeDoOdwiedzenia.First();
            
            l.lekarzPusty.WaitOne();
            temp.aktualnyLekarz = l;
            dest.X = l.punktKolejki.X;
            dest.Y = l.punktKolejki.Y + 20*l.kolejkaPacjentow.Count;

            while (!temp.Idz(dest.X, dest.Y))
            {
                Thread.Sleep(sleepVal);
                this.Invalidate();
            }

            //
            l.lekarzKolejkaSem.WaitOne();

            if (!temp.przywilej)
            {
                l.kolejkaPacjentow.AddLast(temp);
            }
            else 
            {
                l.kolejkaPacjentow.AddFirst(temp);
            }
            l.UstawKolejke();
            
            // tutaj nieciepliwosc trzebaby odpalic
            if (!temp.przywilej) temp.kolor = new SolidBrush(Color.FromArgb(135, 58, 7));
            else temp.kolor = new SolidBrush(Color.FromArgb(240, 15, 15));
            temp.niecierpliwosc = new Thread(new ThreadStart(temp.NiecierpliwoscStart));
            l.lekarzKolejkaSem.Release();
            l.lekarzPelny.Release();
            
            try
            {
                if( temp.lekarzeDoOdwiedzenia.Count>1 ) temp.niecierpliwosc.Start();
            }catch(ThreadStartException)
            {
            
            }
            
 
        }

        private void testChodzenia(Pacjent temp)
        {
            while (!temp.Idz(521, 725))
            {
                Thread.Sleep(sleepVal);
                this.Invalidate();
            }

            while (!temp.Idz(520, 600))
            {
                Thread.Sleep(sleepVal);
                this.Invalidate();
            }
            // p pierwszej kolejki
            while (!temp.Idz(240, 180))
            {
                Thread.Sleep(sleepVal);
                this.Invalidate();
            }
            //pierwszy lekarz
            while (!temp.Idz(240, 90))
            {
                Thread.Sleep(sleepVal);
                this.Invalidate();
            }
            // p drugiej kolejki
            while (!temp.Idz(560, 180))
            {
                Thread.Sleep(sleepVal);
                this.Invalidate();
            }

            while (!temp.Idz(560, 90))
            {
                Thread.Sleep(sleepVal);
                this.Invalidate();
            }
            //pkty treciej kolejki
            while (!temp.Idz(860, 180))
            {
                Thread.Sleep(sleepVal);
                this.Invalidate();
            }

            while (!temp.Idz(860, 90))
            {
                Thread.Sleep(sleepVal);
                this.Invalidate();
            }
            // wyjscie
            while (!temp.Idz(1230, 400))
            {
                Thread.Sleep(sleepVal);
                this.Invalidate();
            }

        }


        private void Form1_Paint(object sender, PaintEventArgs e)
        {

            Graphics g = e.Graphics;
            // tutaj podaj sobie sciezka zeby pasowala, nie pamietam jak sie robilo z poziomu aplikacji
            g.DrawImage(Image.FromFile(@"C:\Users\Kacper\Desktop\Programowanie współbieżne\Projekt\PrzychodniaLekarska\tlo.jpg"), 0, 0);

            this.dostepGrafikaSemaphore.WaitOne();
            foreach (Pacjent a in wszyscyPacjenci)
            {
                a.Rysuj(g);
            }
            this.dostepGrafikaSemaphore.Release();
        }




        private void Form1_Load(object sender, EventArgs e)
        {

        }

    }
}

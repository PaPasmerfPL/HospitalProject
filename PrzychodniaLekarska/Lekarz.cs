using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PrzychodniaLekarska
{
    public class Lekarz
    {
        public String nazwa;
        public LinkedList<Pacjent> kolejkaPacjentow = new LinkedList<Pacjent>();
        public Point punktKolejki;
        public Point punktObslugi;

        //Synchronizacja
        private static int maskDlgoscKolejek = 200;
        //public Semaphore lekarzPracaSem = new Semaphore(0,200);
        public Semaphore lekarzKolejkaSem = new Semaphore(1,1);
        public Semaphore lekarzPusty = new Semaphore(maskDlgoscKolejek, maskDlgoscKolejek);
        public Semaphore lekarzPelny = new Semaphore(0,maskDlgoscKolejek);

        public Thread praca;

        public Form1 okno;

        public Lekarz(Form1 _form1, Point _punktKolejki, Point _punktObslugi)
        {
            this.okno = _form1;
            this.punktKolejki = _punktKolejki;
            this.punktObslugi = _punktObslugi;

        
        }

        public void Pracuj()
        {
            
            while (true)
            {
                //synch
                lekarzPelny.WaitOne();
                //synch
                lekarzKolejkaSem.WaitOne();
                Pacjent obslugiwanyPacjent = new Pacjent();
                
                //wylaczyc aborta na pacjencie
                obslugiwanyPacjent = kolejkaPacjentow.First();
                kolejkaPacjentow.Remove(obslugiwanyPacjent);
                obslugiwanyPacjent.niecierpliwosc.Abort();
                //synch

                this.UstawKolejke();

               

                lekarzKolejkaSem.Release();
                while (!obslugiwanyPacjent.Idz( punktObslugi.X, punktObslugi.Y ))
                {
                    Thread.Sleep(okno.sleepVal);
                    okno.Invalidate();
                }
                obslugiwanyPacjent.lekarzeDoOdwiedzenia.Remove(this);
                
                

                Thread.Sleep(obslugiwanyPacjent.czasWizyty);
                if (obslugiwanyPacjent.lekarzeDoOdwiedzenia.Count == 0)
                {
                    //lekarzPusty.Release();
                    obslugiwanyPacjent.idzT = new Thread(new ThreadStart(() => wyjdzZPrzychodni(obslugiwanyPacjent)));
                    obslugiwanyPacjent.idzT.Start();
                    
                }
                else
                {
                    
                    obslugiwanyPacjent.idzT = new Thread(new ThreadStart(() => okno.idzDoKolejki(obslugiwanyPacjent)));
                    obslugiwanyPacjent.idzT.Start();

                }
                lekarzPusty.Release();
            }
        
        }

        public void UstawKolejke()
        {
            int i = 0;
            foreach (Pacjent p in this.kolejkaPacjentow)
            {
                p.koordynaty.X = this.punktKolejki.X;
                p.koordynaty.Y = this.punktKolejki.Y + 20 * i;
                i++;
            }
        }

        private void wyjdzZPrzychodni(Pacjent obslugiwanyPacjent)
        {
            
            while (!obslugiwanyPacjent.Idz(1230, 400))
            {
                Thread.Sleep(okno.sleepVal);
                okno.Invalidate();
            }
        }

    }
}

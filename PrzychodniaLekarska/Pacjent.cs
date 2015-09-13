using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PrzychodniaLekarska
{
    public class Pacjent
    {
        public Point koordynaty;
        public LinkedList<Lekarz> lekarzeDoOdwiedzenia = new LinkedList<Lekarz>();
        public Lekarz aktualnyLekarz;
        public Form1 okno;

        public float wielkosc = 20;
        public int czasWizyty;
        public Brush kolor;
        public Thread idzT;
        public Thread niecierpliwosc;
        public int czasNiecierpliwosci = 4000;
        private static int predkosc = 5;
        private bool flagDesX = false;
        private bool flagDesY = false;
        public bool przywilej=false;


        public Pacjent()
        {
            kolor = new SolidBrush(Color.FromArgb(135, 58, 7));
            koordynaty = new Point( 0 , 725 );
        
        }

        public Pacjent(Lekarz l1, Form1 _form1)
        {
            Random rand = new Random();
            
            koordynaty = new Point(0, 725);
            lekarzeDoOdwiedzenia.AddLast(l1);
            this.czasWizyty = rand.Next(7000,10000);
            okno = _form1;
            if (rand.Next(1, 11) < 3)
            {
                kolor = new SolidBrush(Color.FromArgb(240, 15, 15));
                this.przywilej = true;
            }
            else 
            {
                kolor = new SolidBrush(Color.FromArgb(135, 58, 7));
                this.przywilej = false;
            }
            this.czasNiecierpliwosci = rand.Next(7000, 14000);

        }




        public bool Idz(int xDes, int yDes)
        {
            if (flagDesX == false)
            {
                if (koordynaty.X > xDes - 8 && koordynaty.X < xDes + 8)
                {
                    flagDesX = true;
                }
                else
                {
                    if (koordynaty.X < xDes)
                    {
                        koordynaty.X += predkosc;
                    }
                    else
                    {
                        koordynaty.X -= predkosc;
                    }
                }
            }
            if (flagDesY == false)
            {
                if (koordynaty.Y > yDes - 8 && koordynaty.Y < yDes + 8)
                {
                    flagDesY = true;
                }
                else
                {
                    if (koordynaty.Y < yDes)
                    {
                        koordynaty.Y += predkosc;
                    }
                    else
                    {
                        koordynaty.Y -= predkosc;
                    }
                }
            }
            //wyjscie funkcji
            if (flagDesX == true && flagDesY == true)
            {
                flagDesX = false;
                flagDesY = false;
                return true;
            }
            else return false;
        }

        // funkcja przeciazona zeby mozna bylo zarowno caly obiekt punktu przekazywac jak i konkretne koordynat w int
        public bool Idz(Point _destination)
        {
            int xDes = _destination.X;
            int yDes = _destination.Y;

            return this.Idz(xDes, yDes);
        }

        public void Rysuj(Graphics g)
        {
                g.FillRectangle(this.kolor, new RectangleF(koordynaty.X, koordynaty.Y, wielkosc, wielkosc));
        }


        internal void NiecierpliwoscStart()
        {

            //aktualnyLekarz.lekarzPelny.Release();
            try
            {
                Thread.Sleep(czasNiecierpliwosci);
                //wypisujemy sie z kolejki
                aktualnyLekarz.lekarzPelny.WaitOne();
                aktualnyLekarz.lekarzKolejkaSem.WaitOne();

                this.kolor = new SolidBrush(Color.FromArgb(0, 0, 0));
                aktualnyLekarz.kolejkaPacjentow.Remove(this);

                aktualnyLekarz.lekarzKolejkaSem.Release();
                aktualnyLekarz.lekarzPusty.Release();

                this.lekarzeDoOdwiedzenia.Remove(aktualnyLekarz);
                this.lekarzeDoOdwiedzenia.AddLast(aktualnyLekarz);
                
                idzT = new Thread(new ThreadStart(() => okno.idzDoKolejki(this)));
                idzT.Start();
            }
            catch (ThreadAbortException)
            {
                Console.WriteLine("Obsługa przerwania");
            }
        }
    }
}

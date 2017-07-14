using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleMinesveiper
{
    class Program
    {
        static int size;
        static int antallMiner = 15;
        static List<Miner> mineliste = new List<Miner>();

        static void Main(string[] args)
        {
            bool fortsett = true;
            while (fortsett)
            {
                bool gameOver = false, vinn = false;
                Console.WriteLine("Velkommen til konsollbasert minesveiper. \nVennligst velg side-størrelse på brettet (f.eks 10x10 spillbrett, tast '10'.");
                while (fortsett)
                {
                    if (int.TryParse(Console.ReadLine(), out size))
                        if (size > 2)
                            fortsett = false;
                        else
                            Console.WriteLine("\nUgyldig valg. Størrelse må være større enn 2. Prøv igjen");
                    else
                        Console.WriteLine("\nUgyldig valg. Prøv på nytt å taste et positivt heltall");
                }

                fortsett = true;
                Console.WriteLine("Vennligst tast inn antall miner");
                while (fortsett)
                {
                    if (int.TryParse(Console.ReadLine(), out antallMiner))
                        if (antallMiner < size * size && antallMiner > 0)
                            fortsett = false;
                        else
                            Console.WriteLine("\nUgyldig valg. Antall miner må være mindre enn " + size * size + " og større enn 0. Prøv igjen");
                    else
                        Console.WriteLine("\nUgyldig valg. Prøv på nytt å taste et positivt heltall");
                }

                Minefelt minefelt = new Minefelt(size, antallMiner);
                minefelt.SettOppSpill();
                Update(minefelt);
                Console.WriteLine("Velg rad og kolonne for å åpne et minefelt, for eksempel slik: '4 7'\nFor å flagge et felt, tast koordinatene etterfulgt av 'F', f.eks slik: '2 9 F'");

                while (!gameOver)
                {
                    try
                    {
                        string[] inn = Console.ReadLine().Split(' ');
                        int rad, kolonne;
                        if (Int32.TryParse(inn[0], out rad) && Int32.TryParse(inn[1], out kolonne))
                        {
                            bool flagg = false;
                            if (inn.Length == 3) //Mulig flagg
                            {
                                if (inn[2].ToUpper() == "F") //Definitivt flagg
                                {
                                    flagg = true;
                                }
                            }

                            minefelt.Klikk(rad, kolonne, flagg, out gameOver, out vinn);
                            Update(minefelt);
                            if (vinn)
                            {
                                Console.WriteLine("Hurra. Du vant!!");
                            }
                            else if (gameOver)
                            {
                                Console.WriteLine("Huffda. Du tapte visst så det sang.");
                            }
                        }
                        else
                            throw new FormatException("Koordinatene må være heltall adskilt av mellomrom. ");
                    }

                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                fortsett = true;
                Console.WriteLine("Vil du spille på nytt? <Y/N>");
                char a = Console.ReadKey().KeyChar;
                if (a != 'y' && a != 'Y')
                    fortsett = false;
                Console.Clear();
            }
        }

        private static void Update(Minefelt minefelt)
        {
            Console.Clear();
            Console.Write(minefelt.ToString());
            Console.WriteLine("Antall uflaggede miner igjen: " + (minefelt.AntallMiner - minefelt.AntallFlagg));
            Console.WriteLine("Antall minefrie felter igjen: " + minefelt.GjenståendeFelter);
        }
    }
}

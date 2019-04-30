using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleMinesveiper
{
    class Minefelt
    {
        private Miner[,] minetabell;

        private int gjenståendeFelter;
        private int størrelse;
        private int antallMiner;

        public int GjenståendeFelter
        {
            get
            {
                return gjenståendeFelter;
            }

            private set
            {
                gjenståendeFelter = value;
            }
        }

        public int Størrelse
        {
            get
            {
                return størrelse;
            }

            private set
            {
                størrelse = value;
            }
        }

        public int AntallMiner
        {
            get
            {
                return antallMiner;
            }

            private set
            {
                antallMiner = value;
            }
        }

        public int AntallFlagg { get; internal set; }

        public Minefelt(int size, int antallMiner)  //Konstruktør
        {
            if (antallMiner >= size * size)
                throw new Exception("Det kan ikke være flere miner enn felter.");
            else
            {
                Størrelse = size;
                AntallMiner = antallMiner;
                GjenståendeFelter = size * size - antallMiner;
                AntallFlagg = 0;
            }
        }
        public void SettOppSpill()
        {
            minetabell = new Miner[størrelse, størrelse];
            GjenståendeFelter = størrelse * størrelse - antallMiner;

            for (int i = 0; i < størrelse; i++) //Oppretter mineobjekter i listen
            {
                for (int j = 0; j < størrelse; j++)
                    minetabell[i, j] = new Miner();
            }

            int antallPlasserteMiner = 0;
            Random r = new Random();
            while (antallPlasserteMiner < antallMiner) //Plasserer miner
            {
                int rad = r.Next(0, størrelse);
                int kolonne = r.Next(0, størrelse);
                if (minetabell[rad, kolonne].Mine == false)
                {
                    minetabell[rad, kolonne].Mine = true;
                    antallPlasserteMiner++;
                    minetabell[rad, kolonne].AntallNæreMiner = 1;
                }
            }

            for (int i = 0; i < størrelse; i++) //Finner antall nære miner
            {
                for (int j = 0; j < størrelse; j++)
                    if (minetabell[i, j].Mine == false)

                    {
                        //Teller miner på samme rad
                        if (rangeCheck(j + 1, størrelse))
                            if (minetabell[i, j + 1].Mine)
                                minetabell[i, j].AntallNæreMiner++;
                        if (rangeCheck(j - 1, størrelse))
                            if (minetabell[i, j - 1].Mine)
                                minetabell[i, j].AntallNæreMiner++;

                        //Teller miner på forrige rad
                        if (rangeCheck(i - 1, størrelse))
                        {
                            if (rangeCheck(j + 1, størrelse))
                                if (minetabell[i - 1, j + 1].Mine)
                                    minetabell[i, j].AntallNæreMiner++;
                            if (rangeCheck(j - 1, størrelse))
                                if (minetabell[i - 1, j - 1].Mine)
                                    minetabell[i, j].AntallNæreMiner++;
                            if (minetabell[i - 1, j].Mine)
                                minetabell[i, j].AntallNæreMiner++;
                        }

                        //Teller miner på neste rad
                        if (rangeCheck(i + 1, størrelse))
                        {
                            if (rangeCheck(j + 1, størrelse))
                                if (minetabell[i + 1, j + 1].Mine)
                                    minetabell[i, j].AntallNæreMiner++;
                            if (rangeCheck(j - 1, størrelse))
                                if (minetabell[i + 1, j - 1].Mine)
                                    minetabell[i, j].AntallNæreMiner++;
                            if (minetabell[i + 1, j].Mine)
                                minetabell[i, j].AntallNæreMiner++;
                        }
                    }
            }

        }

        internal void Klikk(int rad, int kolonne, bool flagg, out bool gameOver, out bool vinn)
        {
            gameOver = false; vinn = false;
            rad--;
            kolonne--;

            if (rad >= 0 && rad < Størrelse && kolonne >= 0 && kolonne < Størrelse) //Om denne if-testen er true er inntastet data gyldig
            {
                if (!flagg)
                {
                    if (!minetabell[rad, kolonne].Clicked) //Om feltet ikke er klikket på fra før av, reduserer det gjenstående felt med 1. 
                        GjenståendeFelter--;
                    minetabell[rad, kolonne].Clicked = true;
                    if (minetabell[rad, kolonne].AntallNæreMiner == 0)
                        ÅpneAlleNaboer(rad, kolonne);
                }
                //Slår flagg av eller på
                else if (minetabell[rad, kolonne].Flagg == false)
                {
                    minetabell[rad, kolonne].Flagg = true;
                    AntallFlagg++;
                }
                else if (minetabell[rad, kolonne].Flagg == true)
                {
                    minetabell[rad, kolonne].Flagg = false;
                    AntallFlagg--;
                }

                if (minetabell[rad, kolonne].Mine && minetabell[rad, kolonne].Clicked) //Bruker har valgt en mine
                {
                    gameOver = true;
                    minetabell[rad, kolonne].Clicked = true;
                }

                else if (GjenståendeFelter == 0) //Bruker har vunnet.
                {
                    vinn = true;
                    gameOver = true;
                }

            }
            else
            {
                throw new ArgumentOutOfRangeException("Inntastet data er utenfor gyldighetsområdet. Tast inn heltall mellom 1 og " + Størrelse);
            }
        }

        private void ÅpneAlleNaboer(int rad, int kolonne)
        {

            //Åpner miner på samme rad
            if (rangeCheck(kolonne + 1, størrelse))
            {

                if (!minetabell[rad, kolonne + 1].Clicked)
                {
                    minetabell[rad, kolonne + 1].Clicked = true;
                    GjenståendeFelter--;
                    if(minetabell[rad, kolonne + 1].AntallNæreMiner == 0)
                        ÅpneAlleNaboer(rad, kolonne + 1);
                }

            }
            if (rangeCheck(kolonne - 1, størrelse))
            {
                if (!minetabell[rad, kolonne - 1].Clicked)
                {
                    minetabell[rad, kolonne - 1].Clicked = true;
                    GjenståendeFelter--;
                    if (minetabell[rad, kolonne - 1].AntallNæreMiner == 0)
                        ÅpneAlleNaboer(rad, kolonne - 1);
                }
            }

            //Åpner miner på forrige rad
            if (rangeCheck(rad - 1, Størrelse))
            {
                if (rangeCheck(kolonne + 1, størrelse))
                {
                    if (!minetabell[rad - 1, kolonne + 1].Clicked)
                    {
                        minetabell[rad - 1, kolonne + 1].Clicked = true;
                        GjenståendeFelter--;
                        if (minetabell[rad - 1, kolonne + 1].AntallNæreMiner == 0)
                            ÅpneAlleNaboer(rad - 1, kolonne + 1);
                    }
                    
                }
                if (rangeCheck(kolonne - 1, størrelse))
                {
                    if (!minetabell[rad - 1, kolonne - 1].Clicked)
                    {
                        minetabell[rad - 1, kolonne - 1].Clicked = true;
                        GjenståendeFelter--;
                        if (minetabell[rad - 1, kolonne - 1].AntallNæreMiner == 0)
                            ÅpneAlleNaboer(rad - 1, kolonne - 1);
                    }
                }
                if (!minetabell[rad - 1, kolonne].Clicked)
                {
                    minetabell[rad - 1, kolonne].Clicked = true;
                    GjenståendeFelter--;
                    if (minetabell[rad - 1, kolonne].AntallNæreMiner == 0)
                        ÅpneAlleNaboer(rad - 1, kolonne);
                }

            }

            //Teller miner på neste rad
            if (rangeCheck(rad + 1, Størrelse))
            {
                if (rangeCheck(kolonne + 1, størrelse))
                {
                    if (!minetabell[rad + 1, kolonne + 1].Clicked)
                    {
                        minetabell[rad + 1, kolonne + 1].Clicked = true;
                        GjenståendeFelter--;
                        if (minetabell[rad + 1, kolonne + 1].AntallNæreMiner == 0)
                            ÅpneAlleNaboer(rad + 1, kolonne + 1);
                    }
                }
                if (rangeCheck(kolonne - 1, størrelse))
                {
                    if (!minetabell[rad + 1, kolonne - 1].Clicked)
                    {
                        minetabell[rad + 1, kolonne - 1].Clicked = true;
                        GjenståendeFelter--;
                        if (minetabell[rad + 1, kolonne - 1].AntallNæreMiner == 0)
                            ÅpneAlleNaboer(rad + 1, kolonne - 1);
                    }
                }
                if (!minetabell[rad + 1, kolonne].Clicked)
                {
                    minetabell[rad + 1, kolonne].Clicked = true;
                    GjenståendeFelter--;
                    if (minetabell[rad + 1, kolonne].AntallNæreMiner == 0)
                        ÅpneAlleNaboer(rad + 1, kolonne);
                }
            }
        }


        internal static bool rangeCheck(int testTall, int størrelse) //Hjelpemetode for å finne nabofelter under telling av nabo-miner.
        {
            if (testTall < størrelse && testTall >= 0)
                return true;
            else return false;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder("");
            //Console.Clear();

            //Skriv ut minefeltet
            sb.Append(" \t");
            for (int i = 1; i <= Størrelse; i++)
                sb.Append(i + " ");
            sb.Append("\n\n");
            for (int i = 0; i < Størrelse; i++)
            {
                sb.Append(i + 1 + "  \t");
                for (int j = 0; j < Størrelse; j++)
                {
                    Miner felt = minetabell[i, j];
                    if (felt.Flagg)
                        sb.Append("F ");
                    else if (!felt.Clicked)
                        sb.Append("# ");
                    else if (felt.Mine)
                        sb.Append("X ");
                    else if (felt.AntallNæreMiner == 0)
                        sb.Append("  ");
                    else
                        sb.Append(felt.AntallNæreMiner + " ");
                }
                sb.Append("\n");
            }
            return sb.ToString();
        }
    }
}

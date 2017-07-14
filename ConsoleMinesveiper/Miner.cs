using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleMinesveiper
{
    class Miner
    {
        bool mine;
        bool flagg;
        int antallNæreMiner;
        bool clicked;
        
        internal bool Mine
        {
            get
            {
                return mine;
            }

            set
            {
                mine = value;
            }
        }

        internal int AntallNæreMiner
        {
            get
            {
                return antallNæreMiner;
            }

            set
            {
                antallNæreMiner = value;
            }
        }

        internal bool Clicked
        {
            get
            {
                return clicked;
            }

            set
            {
                clicked = value;
            }
        }
        
        public bool Flagg
        {
            get
            {
                return flagg;
            }

            set
            {
                flagg = value;
            }
        }

        public Miner()
        {
            Mine = false;
            Clicked = false;
            Flagg = false;
        }

        
    }
}

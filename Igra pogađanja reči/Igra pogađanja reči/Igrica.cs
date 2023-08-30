using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igra_pogađanja_reči
{
    class Igrica
    {
        public string x;

        public bool ValidacijaIskoristenihSlova(List<Igrica> iskoristenoSlovo)
        {

            foreach (Igrica a in iskoristenoSlovo)
            {
                if (this.x == a.x)
                {
                    return false;
                }
            }
            return true;
        }
    }
}

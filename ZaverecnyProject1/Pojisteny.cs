using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZaverecnyProject1
{

    internal class Pojisteny
    {
        public string Jmeno { get; private set; }
        public string Prijmeni { get; private set; }
        public string TelefonniCislo { get; private set; }
        public string Vek { get; private set; }

        public Pojisteny(string jmeno, string prijmeni, string telefonniCislo, string vek)
        {
            Jmeno = jmeno;
            Prijmeni = prijmeni;
            TelefonniCislo = telefonniCislo;
            Vek = vek;
        }

        public override string ToString()
        {
            return string.Format($"{Jmeno}\t\t{Prijmeni}\t\t{Vek}\t\t{TelefonniCislo}");
        }
    }
}

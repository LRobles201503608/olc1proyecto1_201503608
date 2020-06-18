using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLC1Proyecto1_201503608
{
    class Columna
    {
        string nombre { get; set; }
        List<Tupla> tuplas { get; set; }
        public Columna()
        {
            tuplas = new List<Tupla>();
        }
        
    }
}

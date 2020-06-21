using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLC1Proyecto1_201503608
{
    class Columna
    {
        public string nombre { get; set; }
        public string tipo { get; set; }
        public List<Tupla> tuplas { get; set; }
        public Columna(string nombre,string tipo)
        {
            this.tipo = tipo;
            this.nombre=nombre;
            tuplas = new List<Tupla>();
        }
        
    }
}

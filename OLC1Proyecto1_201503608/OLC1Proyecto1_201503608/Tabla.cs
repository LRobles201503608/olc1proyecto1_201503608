using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLC1Proyecto1_201503608
{
    class Tabla
    {
        public string nombre { get; set; }
        public List<Columna> campo { get; set; } 
        public Tabla(string nombre)
        {
            this.nombre = nombre;
            campo = new List<Columna>();
        }

    }
}

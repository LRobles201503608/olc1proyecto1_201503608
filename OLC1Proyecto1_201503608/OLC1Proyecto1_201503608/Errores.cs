using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLC1Proyecto1_201503608
{
    class Errores
    {
        public string tipo { get; set; }
        public string error { get; set; }
        public int fila { get; set; }
        public int columna { get; set; }
    public Errores(string tipo, string error, int fila, int columna)
        {
            this.tipo = tipo;
            this.error = error;
            this.fila = fila;
            this.columna = columna;
        }
    }
}

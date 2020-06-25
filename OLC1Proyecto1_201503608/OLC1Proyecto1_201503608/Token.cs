using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLC1Proyecto1_201503608
{
    class Token
    {
        public string lexemaval { get; set; }
        public int id { get; set; }
        public string tokentipo { get; set; }
        public int linea { get; set; }
        public int columna { get; set; }
        public Token(string lexemaval,int id,string tokentipo,int linea, int columna)
        {
            this.lexemaval = lexemaval;
            this.id = id;
            this.tokentipo = tokentipo;
            this.linea = linea;
            this.columna = columna;
        }
        
    }
}

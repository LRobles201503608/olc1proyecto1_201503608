using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLC1Proyecto1_201503608
{
    class Token
    {
        string lexemaval;
        int id;
        string tokentipo;
        int linea, columna;
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

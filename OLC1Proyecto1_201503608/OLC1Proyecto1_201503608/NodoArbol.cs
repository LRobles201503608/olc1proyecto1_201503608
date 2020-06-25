using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLC1Proyecto1_201503608
{
    public class NodoArbol
    {
        public string etiqueta { get; set; }
        public List<NodoArbol> hijos { get; set; }
        string valor { get; set; }
        int idNod { get; set; }
        int columna { get; set; }
        int fila { get; set; }

        public NodoArbol(String Etiqueta, String valor, int idNod)
        {
            this.etiqueta = Etiqueta;
            this.valor = valor;
            this.idNod = idNod;
            this.columna = -1;
            this.fila = -1;
            this.hijos = new List<NodoArbol>();
        }

        public List<NodoArbol> getHijos()
        {
            return hijos;
        }
        public void AddHijos(NodoArbol hijo)
        {
            getHijos().Add(hijo);
        }

        public NodoArbol(String Etiqueta, String valor, int idNod, int Columna, int Fila)
        {
            this.etiqueta = Etiqueta;
            this.valor = valor;
            this.idNod = idNod;
            this.columna = Columna;
            this.fila = Fila;
            this.hijos = new List<NodoArbol>();
        }
        public void setColumna(int Columna)
        {
            this.columna = Columna;
        }
        /**
         * @param hijos the hijos to set
         */
        public void setHijos(List<NodoArbol> hijos)
        {
            this.hijos = hijos;
        }
        public void setFila(int Fila)
        {
            this.fila = Fila;
        }

        public int getColumna()
        {
            return columna;
        }

        public int getFila()
        {
            return fila;
        }


        /**
         * @return the Etiqueta
         */
        public String getEtiqueta()
        {
            return etiqueta;
        }

        /**
         * @param Etiqueta the Etiqueta to set
         */
        public void setEtiqueta(String Etiqueta)
        {
            this.etiqueta = Etiqueta;
        }

        /**
         * @return the valor
         */
        public String getValor()
        {
            return valor;
        }

        /**
         * @param valor the valor to set
         */
        public void setValor(String valor)
        {
            this.valor = valor;
        }

        /**
         * @return the idNod
         */
        public int getIdNod()
        {
            return idNod;
        }

        /**
         * @param idNod the idNod to set
         */
        public void setIdNod(int idNod)
        {
            this.idNod = idNod;
        }
    }
}

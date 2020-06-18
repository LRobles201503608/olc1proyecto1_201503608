using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLC1Proyecto1_201503608
{
    class RecorrerArbol
    {
        List<Tabla> tablas;
        public RecorrerArbol(List<Tabla> tablas)
        {
            this.tablas = tablas;
        }
        public void Ejecutar(NodoArbol raiz)
        {
            this.INICIO(raiz);
        }
        private void INICIO(NodoArbol raiz)
        {
            foreach (NodoArbol hijos in raiz.getHijos())
            {
                if (hijos!=null)
                {
                    this.INSTRUCCION(hijos);
                }
            }
        }
        private void INSTRUCCION(NodoArbol raiz)
        {
            foreach (NodoArbol hijos in raiz.getHijos())
            {
                if (hijos!=null)
                {
                    this.OPERACION(hijos);
                }
            }
        }
        private void OPERACION(NodoArbol raiz)
        {
            foreach (NodoArbol hijos in raiz.getHijos())
            {
                if (hijos!=null)
                {
                    if (hijos.getEtiqueta().Equals("CREACION"))
                    {
                        this.CREACION(hijos);
                    }else if (hijos.getEtiqueta().Equals("OPERACION"))
                    {
                        this.OPERACION(hijos);
                    }
                }
            }
        }

        private void CREACION(NodoArbol raiz)
        {
            if (raiz!=null)
            {
                string id = raiz.getHijos().ElementAt(2).getValor();
                tablas.Add(new Tabla(id));
                if (raiz.getHijos().ElementAt(3).getEtiqueta().Equals("CAMPOS_CREACION"))
                {
                    this.CAMPOS_CREACION(raiz.getHijos().ElementAt(3));
                }
            }
        }
        private void CAMPOS_CREACION(NodoArbol raiz)
        {

        }
    }
}

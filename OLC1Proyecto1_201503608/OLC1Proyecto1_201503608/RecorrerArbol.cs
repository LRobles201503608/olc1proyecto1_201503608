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
        List<Columna> temporal = new List<Columna>();
        List<int> posiciones = new List<int>();
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
                    } else if (hijos.getEtiqueta().Equals("INSERCION")) {
                        this.INSERCION(hijos);
                    } else if (hijos.getEtiqueta().Equals("ELIMINACION"))
                    {
                        this.ELIMINACION(hijos);
                    } else if (hijos.getEtiqueta().Equals("OPERACION"))
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
                    this.CAMPOS_CREACION(raiz.getHijos().ElementAt(3),id);
                }
            }
        }
        private void CAMPOS_CREACION(NodoArbol raiz, string id)
        {
            string nombre = "";
            foreach(Tabla tabla in tablas)
            {
                if (tabla.nombre.Equals(id))
                {
                    foreach (NodoArbol hijos in raiz.getHijos())
                    {
                        if (hijos!=null)
                        {
                            if (hijos.getValor().Equals("cadena") || hijos.getValor().Equals("entero")
                            || hijos.getValor().Equals("fecha") || hijos.getValor().Equals("flotante"))
                            {
                                tabla.campo.Add(new Columna(nombre, hijos.getValor()));
                                
                            }
                            else if (hijos.getEtiqueta().Equals("CAMPOS_CREACION"))
                            {
                                this.CAMPOS_CREACION(hijos, id);
                            }
                            else
                            {
                                nombre = hijos.getValor();
                            }
                        }
                    }
                }
            }
        }

        private void INSERCION(NodoArbol raiz)
        {
            if (raiz!=null)
            {
                string id = raiz.getHijos().ElementAt(2).getValor();
                int posicion = 0;
                this.PARAMETROS_INSERTAR(raiz.getHijos().ElementAt(4),id,posicion);
            }
        }
        public void PARAMETROS_INSERTAR(NodoArbol raiz, string id, int posicion)
        {
            if (raiz!=null)
            {
                foreach (Tabla tabla in tablas)
                {
                    foreach (NodoArbol hijos in raiz.getHijos())
                    {
                        if (hijos!=null)
                        {
                            if (hijos.getEtiqueta().Equals("PARAMETROS_INSERTAR"))
                            {
                                int posi = posicion + 1;
                                this.PARAMETROS_INSERTAR(hijos, id, posi);
                            }
                            else
                            {
                                if (tabla.nombre.Equals(id))
                                {
                                    tabla.campo.ElementAt(posicion).tuplas.Add(new Tupla(hijos.getValor()));
                                }
                            }
                        }
                    }
                }
            }
        }

        public void ELIMINACION(NodoArbol raiz)
        {
            if (raiz.getHijos().Count==4 && raiz.getHijos().ElementAt(3)==null)
            {
                ELIMINACION_TOTAL(raiz);
            }else if (raiz.getHijos().Count==4 && raiz.getHijos().ElementAt(3) != null)
            {
                ELIMINACION_CONDICIONAL(raiz);
            }
        }
        public void ELIMINACION_TOTAL(NodoArbol raiz)
        {
            string id = raiz.getHijos().ElementAt(2).getValor();
            foreach (Tabla tabla in tablas)
            {
                if (tabla.nombre.Equals(id))
                {
                    foreach (Columna campo in tabla.campo)
                    {
                        campo.tuplas.Clear();
                    }
                }
            }
        }

        public void ELIMINACION_CONDICIONAL(NodoArbol raiz)
        {
            string id = raiz.getHijos().ElementAt(2).getValor();
            CONDICION_ELIMINACION(raiz.getHijos().ElementAt(3),id);

        }
        public void CONDICION_ELIMINACION(NodoArbol raiz , string tabla)
        {

            CONDICIONES(raiz.getHijos().ElementAt(1),tabla);
        }

        public void CONDICIONES(NodoArbol raiz, string tabla)
        {
            if (raiz.getHijos().Count==4  && raiz.getHijos().ElementAt(3) == null)
            {
                foreach (Tabla tab in tablas)
                {
                    if (tab.nombre.Equals(tabla))
                    {
                        string id = raiz.getHijos().ElementAt(0).getValor();
                        string condicional = raiz.getHijos().ElementAt(1).getHijos().ElementAt(0).getValor();
                        string valor = raiz.getHijos().ElementAt(2).getValor();
                        RESOLVER_CONDICION_SIMPLE(id,condicional,valor,tab);
                    }
                }
            }
            if (raiz.getHijos().Count == 4 && raiz.getHijos().ElementAt(3) != null)
            {
                foreach (Tabla tab in tablas)
                {
                    if (tab.nombre.Equals(tabla))
                    {
                        string id = raiz.getHijos().ElementAt(0).getValor();
                        string condicional = raiz.getHijos().ElementAt(1).getHijos().ElementAt(0).getValor();
                        string valor = raiz.getHijos().ElementAt(2).getValor();
                        string condicion = raiz.getHijos().ElementAt(3).getHijos().ElementAt(0).getValor();
                        RESOLVER_CONDICION_MEZCLADA(id,condicional,valor,tab, raiz.getHijos().ElementAt(3),condicion);
                    }
                }
            }
            else if (raiz.getHijos().Count==5 && raiz.getHijos().ElementAt(4) != null)
            {
                foreach (Tabla tab in tablas)
                {
                    if (tab.nombre.Equals(tabla))
                    {

                    }
                }
            }
            else if (raiz.getHijos().Count == 5 && raiz.getHijos().ElementAt(4) == null)
            {
                foreach (Tabla tab in tablas)
                {
                    if (tab.nombre.Equals(tabla))
                    {

                    }
                }
            }
        }

        public void RESOLVER_CONDICION_SIMPLE(string id, string condicional, string valor, Tabla tabla)
        {
            int posicion = 0;
            foreach (Columna col in tabla.campo)
            {
                if (col.nombre.Equals(id))
                {
                    if (condicional.Equals("="))
                    {
                        foreach (Tupla datos in col.tuplas)
                        {
                            if (datos.valores.ElementAt(0).Equals(valor) && !posiciones.Contains(posicion))
                            {
                                posiciones.Add(posicion);
                                break;
                            }
                            else
                            {
                                posicion++;
                            }
                        }
                        foreach (Columna col2 in tabla.campo)
                        {
                            for (int i = posiciones.Count()-1; i>=0; i--)
                            {
                                col2.tuplas.RemoveAt(posiciones.ElementAt(i));
                            }
                        }
                        posicion = 0;
                        posiciones.Clear();
                    } 
                    else if (condicional.Equals("!="))
                    {
                        foreach (Tupla datos in col.tuplas)
                        {
                            if (datos.valores.ElementAt(0).Equals(valor) && !posiciones.Contains(posicion))
                            {
                                posiciones.Add(posicion);
                                break;
                            }
                            else
                            {
                                posicion++;
                            }
                        }
                        foreach (Columna col2 in tabla.campo)
                        {
                            for (int i = posiciones.Count() - 1; i >= 0; i--)
                            {
                                col2.tuplas.RemoveRange((posiciones.ElementAt(i) + 1), (col2.tuplas.Count() - 1 - posiciones.ElementAt(i))); //revisar el no igual
                                col2.tuplas.RemoveRange(0, (posiciones.ElementAt(i) - 1));
                            }
                        }
                        posicion = 0;
                        posiciones.Clear();
                    }
                    else if (condicional.Equals("<="))
                    {
                        if (col.tipo.ToLower().Equals("entero"))
                        {
                            foreach (Tupla datos in col.tuplas)
                            {
                                int dato = Int32.Parse(datos.valores.ElementAt(0));
                                if (dato <= Int32.Parse(valor) && !posiciones.Contains(posicion))
                                {
                                    posiciones.Add(posicion);
                                    posicion++;
                                }
                                else
                                {
                                    posicion++;
                                }
                            }
                        }
                        else if (col.tipo.ToLower().Equals("flotante"))
                        {
                            foreach (Tupla datos in col.tuplas)
                            {
                                float dato = float.Parse(datos.valores.ElementAt(0));
                                if (dato <= float.Parse(valor) && !posiciones.Contains(posicion))
                                {
                                    posiciones.Add(posicion);
                                    posicion++;
                                }
                                else
                                {
                                    posicion++;
                                }
                            }
                        }
                        else
                        {

                        }
                        foreach (Columna col2 in tabla.campo)
                        {
                            for (int i = posiciones.Count() - 1; i >= 0; i--)
                            {
                                col2.tuplas.RemoveAt(posiciones.ElementAt(i));
                            }
                        }
                        posicion = 0;
                        posiciones.Clear();
                    }
                    else if (condicional.Equals(">="))
                    {
                        if (col.tipo.ToLower().Equals("entero"))
                        {
                            foreach (Tupla datos in col.tuplas)
                            {
                                int dato = Int32.Parse(datos.valores.ElementAt(0));
                                if (dato >= Int32.Parse(valor) && !posiciones.Contains(posicion))
                                {
                                    posiciones.Add(posicion);
                                    posicion++;
                                }
                                else
                                {
                                    posicion++;
                                }
                            }
                        }
                        else if (col.tipo.ToLower().Equals("flotante"))
                        {
                            foreach (Tupla datos in col.tuplas)
                            {
                                float dato = float.Parse(datos.valores.ElementAt(0));
                                if (dato >= float.Parse(valor) && !posiciones.Contains(posicion))
                                {
                                    posiciones.Add(posicion);
                                    posicion++;
                                }
                                else
                                {
                                    posicion++;
                                }
                            }
                        }
                        else
                        {

                        }
                        foreach (Columna col2 in tabla.campo)
                        {
                            for (int i = posiciones.Count() - 1; i >= 0; i--)
                            {
                                col2.tuplas.RemoveAt(posiciones.ElementAt(i));
                            }
                        }
                        posicion = 0;
                        posiciones.Clear();
                    }
                }
                else if (condicional.Equals("<"))
                {
                    if (col.tipo.ToLower().Equals("entero"))
                    {
                        foreach (Tupla datos in col.tuplas)
                        {
                            int dato = Int32.Parse(datos.valores.ElementAt(0));
                            if (dato < Int32.Parse(valor) && !posiciones.Contains(posicion))
                            {
                                posiciones.Add(posicion);
                                posicion++;
                            }
                            else
                            {
                                posicion++;
                            }
                        }
                    }
                    else if (col.tipo.ToLower().Equals("flotante"))
                    {
                        foreach (Tupla datos in col.tuplas)
                        {
                            float dato = float.Parse(datos.valores.ElementAt(0));
                            if (dato < float.Parse(valor) && !posiciones.Contains(posicion))
                            {
                                posiciones.Add(posicion);
                                posicion++;
                            }
                            else
                            {
                                posicion++;
                            }
                        }
                    }
                    else
                    {

                    }
                    foreach (Columna col2 in tabla.campo)
                    {
                        for (int i = posiciones.Count() - 1; i >= 0; i--)
                        {
                            col2.tuplas.RemoveAt(posiciones.ElementAt(i));
                        }
                    }
                    posicion = 0;
                    posiciones.Clear();
                }
                else if (condicional.Equals(">"))
                {
                    if (col.tipo.ToLower().Equals("entero"))
                    {
                        foreach (Tupla datos in col.tuplas)
                        {
                            int dato = Int32.Parse(datos.valores.ElementAt(0));
                            if (dato > Int32.Parse(valor) && !posiciones.Contains(posicion))
                            {
                                posiciones.Add(posicion);
                                posicion++;
                            }
                            else
                            {
                                posicion++;
                            }
                        }
                    }
                    else if (col.tipo.ToLower().Equals("flotante"))
                    {
                        foreach (Tupla datos in col.tuplas)
                        {
                            float dato = float.Parse(datos.valores.ElementAt(0));
                            if (dato > float.Parse(valor) && !posiciones.Contains(posicion))
                            {
                                posiciones.Add(posicion);
                                posicion++;
                            }
                            else
                            {
                                posicion++;
                            }
                        }
                    }
                    else
                    {

                    }
                    foreach (Columna col2 in tabla.campo)
                    {
                        for (int i = posiciones.Count() - 1; i >= 0; i--)
                        {
                            col2.tuplas.RemoveAt(posiciones.ElementAt(i));
                        }
                    }
                    posicion = 0;
                    posiciones.Clear();
                }
            }
        }

        public void RESOLVER_CONDICION_MEZCLADA(string id, string condicional, string valor, Tabla tabla,NodoArbol raiz, string condicion)
        {
            if (raiz != null)
            {
                if (raiz.getHijos() != null)
                {
                    if (raiz.getHijos().ElementAt(4) != null)
                    {
                        if (condicion.ToLower().Equals("o"))
                        {
                            AGREGAR_TODAS_POSICIONES(id, condicional, valor, tabla, raiz);
                        }
                    }else if (raiz.getHijos().ElementAt(4) == null)
                    {
                        AGREGAR_TODAS_POSICIONES_FIN(id, condicional, valor, tabla, raiz);
                    }
                }
            }
            
        }
        public void AGREGAR_TODAS_POSICIONES_FIN(string id, string condicional, string valor, Tabla tabla, NodoArbol raiz)
        {

        }
        public void AGREGAR_TODAS_POSICIONES(string id, string condicional, string valor, Tabla tabla, NodoArbol raiz)
        {
            int posicion = 0;
            NodoArbol hijo = null;
            if (raiz != null)
            {
               hijo  = raiz.getHijos().ElementAt(4);
            }
            foreach (Columna col in tabla.campo)
            {
                if (col.nombre.Equals(id))
                {
                    if (condicional.Equals("="))
                    {
                        foreach (Tupla datos in col.tuplas)
                        {
                            if (datos.valores.ElementAt(0).Equals(valor) && !posiciones.Contains(posicion))
                            {
                                posiciones.Add(posicion);
                                break;
                            }
                            else
                            {
                                posicion++;
                            }
                        }
                        if (raiz.getHijos().ElementAt(4).getHijos() != null)
                        {
                            this.RESOLVER_CONDICION_MEZCLADA(raiz.getHijos().ElementAt(1).getValor(), raiz.getHijos().ElementAt(2).getHijos().ElementAt(0).getValor(), raiz.getHijos().ElementAt(3).getValor(), tabla, raiz.getHijos().ElementAt(4), raiz.getHijos().ElementAt(4).getHijos().ElementAt(0).getValor());
                        }
                    }
                    else if (condicional.Equals("!="))
                    {
                        foreach (Tupla datos in col.tuplas)
                        {
                            if (!datos.valores.ElementAt(0).Equals(valor) && !posiciones.Contains(posicion))
                            {
                                posiciones.Add(posicion);
                            }
                            else
                            {
                                posicion++;
                            }
                        }
                        if (raiz.getHijos().ElementAt(4).getHijos() != null)
                        {
                            this.RESOLVER_CONDICION_MEZCLADA(raiz.getHijos().ElementAt(1).getValor(), raiz.getHijos().ElementAt(2).getHijos().ElementAt(0).getValor(), raiz.getHijos().ElementAt(3).getValor(), tabla, raiz.getHijos().ElementAt(4), raiz.getHijos().ElementAt(4).getHijos().ElementAt(0).getValor());
                        }
                    }
                    else if (condicional.Equals("<="))
                    {
                        if (col.tipo.ToLower().Equals("entero"))
                        {
                            foreach (Tupla datos in col.tuplas)
                            {
                                int dato = Int32.Parse(datos.valores.ElementAt(0));
                                if (dato <= Int32.Parse(valor) && !posiciones.Contains(posicion))
                                {
                                    posiciones.Add(posicion);
                                    posicion++;
                                }
                                else
                                {
                                    posicion++;
                                }
                            }
                        }
                        else if (col.tipo.ToLower().Equals("flotante"))
                        {
                            foreach (Tupla datos in col.tuplas)
                            {
                                float dato = float.Parse(datos.valores.ElementAt(0));
                                if (dato <= float.Parse(valor) && !posiciones.Contains(posicion))
                                {
                                    posiciones.Add(posicion);
                                    posicion++;
                                }
                                else
                                {
                                    posicion++;
                                }
                            }
                        }
                        else
                        {

                        }
                        if (raiz.getHijos().ElementAt(4).getHijos() != null)
                        {
                            this.RESOLVER_CONDICION_MEZCLADA(raiz.getHijos().ElementAt(1).getValor(), raiz.getHijos().ElementAt(2).getHijos().ElementAt(0).getValor(), raiz.getHijos().ElementAt(3).getValor(), tabla, raiz.getHijos().ElementAt(4), raiz.getHijos().ElementAt(4).getHijos().ElementAt(0).getValor());
                        }
                    }
                    else if (condicional.Equals(">="))
                    {
                        if (col.tipo.ToLower().Equals("entero"))
                        {
                            foreach (Tupla datos in col.tuplas)
                            {
                                int dato = Int32.Parse(datos.valores.ElementAt(0));
                                if (dato >= Int32.Parse(valor) && !posiciones.Contains(posicion))
                                {
                                    posiciones.Add(posicion);
                                    posicion++;
                                }
                                else
                                {
                                    posicion++;
                                }
                            }
                        }
                        else if (col.tipo.ToLower().Equals("flotante"))
                        {
                            foreach (Tupla datos in col.tuplas)
                            {
                                float dato = float.Parse(datos.valores.ElementAt(0));
                                if (dato >= float.Parse(valor) && !posiciones.Contains(posicion))
                                {
                                    posiciones.Add(posicion);
                                    posicion++;
                                }
                                else
                                {
                                    posicion++;
                                }
                            }
                        }
                        else
                        {

                        }
                        if (raiz.getHijos().ElementAt(4).getHijos() != null)
                        {
                            this.RESOLVER_CONDICION_MEZCLADA(raiz.getHijos().ElementAt(1).getValor(), raiz.getHijos().ElementAt(2).getHijos().ElementAt(0).getValor(), raiz.getHijos().ElementAt(3).getValor(), tabla, raiz.getHijos().ElementAt(4), raiz.getHijos().ElementAt(4).getHijos().ElementAt(0).getValor());
                        }
                    }
                    else if (condicional.Equals("<"))
                    {
                        if (col.tipo.ToLower().Equals("entero"))
                        {
                            foreach (Tupla datos in col.tuplas)
                            {
                                int dato = Int32.Parse(datos.valores.ElementAt(0));
                                if (dato < Int32.Parse(valor) && !posiciones.Contains(posicion))
                                {
                                    posiciones.Add(posicion);
                                    posicion++;
                                }
                                else
                                {
                                    posicion++;
                                }
                            }
                        }
                        else if (col.tipo.ToLower().Equals("flotante"))
                        {
                            foreach (Tupla datos in col.tuplas)
                            {
                                float dato = float.Parse(datos.valores.ElementAt(0));
                                if (dato < float.Parse(valor) && !posiciones.Contains(posicion))
                                {
                                    posiciones.Add(posicion);
                                    posicion++;
                                }
                                else
                                {
                                    posicion++;
                                }
                            }
                        }
                        else
                        {

                        }
                        if (raiz.getHijos().ElementAt(4).getHijos() != null)
                        {
                            this.RESOLVER_CONDICION_MEZCLADA(raiz.getHijos().ElementAt(1).getValor(), raiz.getHijos().ElementAt(2).getHijos().ElementAt(0).getValor(), raiz.getHijos().ElementAt(3).getValor(), tabla, raiz.getHijos().ElementAt(4), raiz.getHijos().ElementAt(4).getHijos().ElementAt(0).getValor());
                        }
                    }
                    else if (condicional.Equals(">"))
                    {
                        if (col.tipo.ToLower().Equals("entero"))
                        {
                            foreach (Tupla datos in col.tuplas)
                            {
                                int dato = Int32.Parse(datos.valores.ElementAt(0));
                                if (dato > Int32.Parse(valor) && !posiciones.Contains(posicion))
                                {
                                    posiciones.Add(posicion);
                                    posicion++;
                                }
                                else
                                {
                                    posicion++;
                                }
                            }
                        }
                        else if (col.tipo.ToLower().Equals("flotante"))
                        {
                            foreach (Tupla datos in col.tuplas)
                            {
                                float dato = float.Parse(datos.valores.ElementAt(0));
                                if (dato > float.Parse(valor) && !posiciones.Contains(posicion))
                                {
                                    posiciones.Add(posicion);
                                    posicion++;
                                }
                                else
                                {
                                    posicion++;
                                }
                            }
                        }
                        else
                        {

                        }
                        if (raiz.getHijos().ElementAt(4).getHijos() != null)
                        {
                            this.RESOLVER_CONDICION_MEZCLADA(raiz.getHijos().ElementAt(1).getValor(), raiz.getHijos().ElementAt(2).getHijos().ElementAt(0).getValor(), raiz.getHijos().ElementAt(3).getValor(), tabla, raiz.getHijos().ElementAt(4), raiz.getHijos().ElementAt(4).getHijos().ElementAt(0).getValor());
                        }
                    }
                }
            }
        }
    }
}

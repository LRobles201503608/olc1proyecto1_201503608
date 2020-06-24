using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
        List<int> posiA = new List<int>();
        List<int> posiB = new List<int>();
        List<string> parametros = new List<string>();
        List<string> ltablas = new List<string>();
        List<string> alias = new List<string>();
        int consulta = 0;
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
                    } else if (hijos.getEtiqueta().Equals("ACTUALIZACION"))
                    {
                        this.ACTUALIZACION(hijos);
                    } else if (hijos.getEtiqueta().Equals("SELECCION")) {
                        this.SELECCION(hijos,consulta);
                        consulta++;
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
                foreach (Tabla tabla in tablas)
                {
                    if (tabla.nombre.Equals(id))
                    {
                        this.PARAMETROS_INSERTAR(raiz.getHijos().ElementAt(4), id, posicion,tabla);
                    }
                }
            }
        }
        public void PARAMETROS_INSERTAR(NodoArbol raiz, string id, int posicion, Tabla tabla)
        {
            if (raiz!=null)
            {
                foreach (NodoArbol hijos in raiz.getHijos())
                {
                    if (hijos!=null)
                    {
                        if (hijos.getEtiqueta().Equals("PARAMETROS_INSERTAR"))
                        {
                            int posi = posicion + 1;
                            this.PARAMETROS_INSERTAR(hijos, id, posi,tabla);
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
                        posiciones.Clear();
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
                        posiciones = posiciones.OrderBy(v => v).ToList();
                        foreach (Columna col in tab.campo)
                        {
                            for (int i = posiciones.Count() - 1; i >= 0; i--)
                            {
                                col.tuplas.RemoveAt(posiciones.ElementAt(i));
                            }
                        }
                        posiciones.Clear();
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
                        else if (condicion.ToLower().Equals("y"))
                        {
                            VERIFICAR_POSICIONES(id,condicional,valor,tabla);
                            VERIFICAR_POSICIONES(raiz.getHijos().ElementAt(1).getValor(), raiz.getHijos().ElementAt(2).getHijos().ElementAt(0).getValor(), raiz.getHijos().ElementAt(3).getValor(), tabla);
                            foreach (int a in posiA)
                            {
                                foreach (int b in posiB)
                                {
                                    if (a == b)
                                    {
                                        posiciones.Add(b);
                                    }
                                }
                            }
                            posiciones = posiciones.OrderBy(v => v).ToList();
                            foreach (Columna col in tabla.campo)
                            {
                                for (int i = posiciones.Count() - 1; i >= 0; i--)
                                {
                                    col.tuplas.RemoveAt(posiciones.ElementAt(i));
                                }
                            }
                            posiciones.Clear();
                            this.RESOLVER_CONDICION_MEZCLADA(raiz.getHijos().ElementAt(4).getHijos().ElementAt(1).getValor(), raiz.getHijos().ElementAt(4).getHijos().ElementAt(2).getHijos().ElementAt(0).getValor(), raiz.getHijos().ElementAt(4).getHijos().ElementAt(3).getValor(), tabla, raiz.getHijos().ElementAt(4), raiz.getHijos().ElementAt(4).getHijos().ElementAt(0).getValor());
                        }
                    }else if (raiz.getHijos().ElementAt(4) == null)
                    {
                        AGREGAR_TODAS_POSICIONES_FIN(id, condicional, valor, tabla, raiz);
                    }
                }
            }   
        }

        public void VERIFICAR_POSICIONES(string id, string condicional, string valor, Tabla tabla)
        {
            if (posiA.Count()==0)
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
                                    posiA.Add(posicion);
                                    break;
                                }
                                else
                                {
                                    posicion++;
                                }
                            }
                            posicion = 0;
                        }
                        else if (condicional.Equals("!="))
                        {
                            foreach (Tupla datos in col.tuplas)
                            {
                                if (datos.valores.ElementAt(0).Equals(valor) && !posiciones.Contains(posicion))
                                {
                                    posiA.Add(posicion);
                                    break;
                                }
                                else
                                {
                                    posicion++;
                                }
                            }
                            posicion = 0;
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
                                        posiA.Add(posicion);
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
                                        posiA.Add(posicion);
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
                            posicion = 0;
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
                                        posiA.Add(posicion);
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
                                        posiA.Add(posicion);
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
                            posicion = 0;
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
                                        posiA.Add(posicion);
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
                                        posiA.Add(posicion);
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
                            posicion = 0;
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
                                        posiA.Add(posicion);
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
                                        posiA.Add(posicion);
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
                            posicion = 0;
                        }
                    }
                }
            }
            else
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
                                    posiB.Add(posicion);
                                    break;
                                }
                                else
                                {
                                    posicion++;
                                }
                            }
                            posicion = 0;
                        }
                        else if (condicional.Equals("!="))
                        {
                            foreach (Tupla datos in col.tuplas)
                            {
                                if (datos.valores.ElementAt(0).Equals(valor) && !posiciones.Contains(posicion))
                                {
                                    posiB.Add(posicion);
                                    break;
                                }
                                else
                                {
                                    posicion++;
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
                                        posiB.Add(posicion);
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
                                        posiB.Add(posicion);
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
                            posicion = 0;
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
                                        posiB.Add(posicion);
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
                                        posiB.Add(posicion);
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
                            posicion = 0;
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
                                        posiB.Add(posicion);
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
                                        posiB.Add(posicion);
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
                            posicion = 0;
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
                                        posiB.Add(posicion);
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
                                        posiB.Add(posicion);
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
                            posicion = 0;
                        }
                    }
                   
                }
            }
        }
        public void AGREGAR_TODAS_POSICIONES_FIN(string id, string condicional, string valor, Tabla tabla, NodoArbol raiz)
        {
            string id2 = raiz.getHijos().ElementAt(1).getValor();
            string condicional2 = raiz.getHijos().ElementAt(2).getHijos().ElementAt(0).getValor();
            string valor2 = raiz.getHijos().ElementAt(3).getValor();
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
                        posicion = 0;
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
                        posicion = 0;
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
                        posicion = 0;
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
                        posicion = 0;
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
                        posicion = 0;
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
                        posicion = 0;
                    }
                }
            }
//-------------------------------------------------------------------------------------------------------------------------------------
//------------------------------------este es para agregar los ultimos valores de la condicion-----------------------------------------
            foreach (Columna col in tabla.campo)
            {
                if (col.nombre.Equals(id2))
                {
                    if (condicional2.Equals("="))
                    {
                        foreach (Tupla datos in col.tuplas)
                        {
                            if (datos.valores.ElementAt(0).Equals(valor2) && !posiciones.Contains(posicion))
                            {
                                posiciones.Add(posicion);
                                break;
                            }
                            else
                            {
                                posicion++;
                            }
                        }
                        posicion = 0;
                    }
                    else if (condicional2.Equals("!="))
                    {
                        foreach (Tupla datos in col.tuplas)
                        {
                            if (!datos.valores.ElementAt(0).Equals(valor2) && !posiciones.Contains(posicion))
                            {
                                posiciones.Add(posicion);
                            }
                            else
                            {
                                posicion++;
                            }
                        }
                        posicion = 0;
                    }
                    else if (condicional2.Equals("<="))
                    {
                        if (col.tipo.ToLower().Equals("entero"))
                        {
                            foreach (Tupla datos in col.tuplas)
                            {
                                int dato = Int32.Parse(datos.valores.ElementAt(0));
                                if (dato <= Int32.Parse(valor2) && !posiciones.Contains(posicion))
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
                                if (dato <= float.Parse(valor2) && !posiciones.Contains(posicion))
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
                        posicion = 0;
                    }
                    else if (condicional2.Equals(">="))
                    {
                        if (col.tipo.ToLower().Equals("entero"))
                        {
                            foreach (Tupla datos in col.tuplas)
                            {
                                int dato = Int32.Parse(datos.valores.ElementAt(0));
                                if (dato >= Int32.Parse(valor2) && !posiciones.Contains(posicion))
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
                                if (dato >= float.Parse(valor2) && !posiciones.Contains(posicion))
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
                        posicion = 0;
                    }
                    else if (condicional2.Equals("<"))
                    {
                        if (col.tipo.ToLower().Equals("entero"))
                        {
                            foreach (Tupla datos in col.tuplas)
                            {
                                int dato = Int32.Parse(datos.valores.ElementAt(0));
                                if (dato < Int32.Parse(valor2) && !posiciones.Contains(posicion))
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
                                if (dato < float.Parse(valor2) && !posiciones.Contains(posicion))
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
                        posicion = 0;
                    }
                    else if (condicional2.Equals(">"))
                    {
                        if (col.tipo.ToLower().Equals("entero"))
                        {
                            foreach (Tupla datos in col.tuplas)
                            {
                                int dato = Int32.Parse(datos.valores.ElementAt(0));
                                if (dato > Int32.Parse(valor2) && !posiciones.Contains(posicion))
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
                                if (dato > float.Parse(valor2) && !posiciones.Contains(posicion))
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
                        posicion = 0;
                    }
                }
            }
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
                        posicion = 0;
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
                        posicion = 0;
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
                        posicion = 0;
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
                        posicion = 0;
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
                        posicion = 0;
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
                        posicion = 0;
                        if (raiz.getHijos().ElementAt(4).getHijos() != null)
                        {
                            this.RESOLVER_CONDICION_MEZCLADA(raiz.getHijos().ElementAt(1).getValor(), raiz.getHijos().ElementAt(2).getHijos().ElementAt(0).getValor(), raiz.getHijos().ElementAt(3).getValor(), tabla, raiz.getHijos().ElementAt(4), raiz.getHijos().ElementAt(4).getHijos().ElementAt(0).getValor());
                        }
                    }
                }
            }
        }

        public void ACTUALIZACION(NodoArbol raiz)
        {
            string id = raiz.getHijos().ElementAt(1).getValor();
            if (raiz.getHijos().Count() == 5 && raiz.getHijos().ElementAt(4) != null)
            {
                foreach (Tabla tabla in tablas)
                {
                    if (tabla.nombre.Equals(id))
                    {
                        PARAMETROS_ESTABLECER(raiz.getHijos().ElementAt(3), tabla);
                        CONDICION_SELECCION(raiz.getHijos().ElementAt(4), tabla);
                        posiciones = posiciones.OrderBy(v => v).ToList();

                        foreach (Columna col in tabla.campo)
                        {
                            foreach (Columna col2 in temporal)
                            {
                                if (col.nombre.Equals(col2.nombre))
                                {
                                    for (int i = posiciones.Count() - 1; i >= 0; i--)
                                    {
                                        col.tuplas[posiciones.ElementAt(i)] = col2.tuplas.ElementAt(0);
                                    }
                                }
                            }
                        }
                        posiciones.Clear();
                        temporal.Clear();
                    }
                }
            }
            else if (raiz.getHijos().Count() == 5 && raiz.getHijos().ElementAt(4) == null)
            {
                foreach (Tabla tabla in tablas)
                {
                    if (tabla.nombre.Equals(id))
                    {
                        PARAMETROS_ESTABLECER(raiz.getHijos().ElementAt(3), tabla);
                        foreach (Columna col in tabla.campo)
                        {
                            foreach (Columna col2 in temporal)
                            {
                                if (col.nombre.Equals(col2.nombre))
                                {
                                    for (int i = col.tuplas.Count() - 1; i >= 0; i--)
                                    {
                                        col.tuplas[i] = col2.tuplas.ElementAt(0);
                                    }
                                }
                            }
                        }
                        posiciones.Clear();
                        temporal.Clear();
                    }
                }
            }
        }

        public void CONDICION_SELECCION(NodoArbol raiz, Tabla tabla)
        {
            CONDICIONES2(raiz.getHijos().ElementAt(1),tabla);
        }

        public void PARAMETROS_ESTABLECER(NodoArbol raiz, Tabla tabla)
        {
            if (raiz.getHijos().Count == 4 && raiz.getHijos().ElementAt(3) != null)
            {
                string id = raiz.getHijos().ElementAt(0).getValor();
                string valor = raiz.getHijos().ElementAt(2).getValor();
                if (!EXISTE_COLUMNA(id))
                {
                    temporal.Add(new Columna(id, ""));
                }
                else
                {

                }
                foreach (Columna col in temporal) {
                    if (col.nombre.Equals(id))
                    {
                        col.tuplas.Add(new Tupla(valor));
                    }
                }
                PARAMETROS_ESTABLECER(raiz.getHijos().ElementAt(3), tabla);
            } else if (raiz.getHijos().Count == 4 && raiz.getHijos().ElementAt(3) == null)
            {
                string id = raiz.getHijos().ElementAt(0).getValor();
                string valor = raiz.getHijos().ElementAt(2).getValor();
                if (!EXISTE_COLUMNA(id))
                {
                    temporal.Add(new Columna(id, ""));
                }
                else
                {

                }
                foreach (Columna col in temporal)
                {
                    if (col.nombre.Equals(id))
                    {
                        col.tuplas.Add(new Tupla(valor));
                    }
                }
            }
        }
        public bool EXISTE_COLUMNA(string id)
        {
            foreach (Columna col in temporal)
            {
                if (col.nombre.Equals(id))
                {
                    return true;
                }
            }
            return false;
        }

        public void CONDICIONES2(NodoArbol raiz, Tabla tabla)
        {
            if (raiz.getHijos().Count == 4 && raiz.getHijos().ElementAt(3) == null)
            {
                string id = raiz.getHijos().ElementAt(0).getValor();
                string condicional = raiz.getHijos().ElementAt(1).getHijos().ElementAt(0).getValor();
                string valor = raiz.getHijos().ElementAt(2).getValor();
                UNA_CONDICION(id, condicional, valor, tabla) ;
            }
            if (raiz.getHijos().Count == 4 && raiz.getHijos().ElementAt(3) != null)
            {
                string id = raiz.getHijos().ElementAt(0).getValor();
                string condicional = raiz.getHijos().ElementAt(1).getHijos().ElementAt(0).getValor();
                string valor = raiz.getHijos().ElementAt(2).getValor();
                string condicion = raiz.getHijos().ElementAt(3).getHijos().ElementAt(0).getValor();
                MUCHAS_CONDICIONES(id, condicional, valor, tabla, raiz.getHijos().ElementAt(3), condicion);
            }
        }
        public void MUCHAS_CONDICIONES(string id, string condicional, string valor, Tabla tabla, NodoArbol raiz, string condicion)
        {
            if (raiz != null)
            {
                if (raiz.getHijos() != null)
                {
                    if (raiz.getHijos().ElementAt(4) != null)
                    {
                        if (condicion.ToLower().Equals("o"))
                        {
                            AGREGAR_TODAS_POSICIONES2(id, condicional, valor, tabla, raiz);
                        }
                        else if (condicion.ToLower().Equals("y"))
                        {
                            VERIFICAR_POSICIONES(id, condicional, valor, tabla);
                            VERIFICAR_POSICIONES(raiz.getHijos().ElementAt(1).getValor(), raiz.getHijos().ElementAt(2).getHijos().ElementAt(0).getValor(), raiz.getHijos().ElementAt(3).getValor(), tabla);
                            foreach (int a in posiA)
                            {
                                foreach (int b in posiB)
                                {
                                    if (a == b)
                                    {
                                        posiciones.Add(b);
                                    }
                                }
                            }
                            posiciones = posiciones.OrderBy(v => v).ToList();
                            this.MUCHAS_CONDICIONES(raiz.getHijos().ElementAt(4).getHijos().ElementAt(1).getValor(), raiz.getHijos().ElementAt(4).getHijos().ElementAt(2).getHijos().ElementAt(0).getValor(), raiz.getHijos().ElementAt(4).getHijos().ElementAt(3).getValor(), tabla, raiz.getHijos().ElementAt(4), raiz.getHijos().ElementAt(4).getHijos().ElementAt(0).getValor());
                        }
                    }
                    else if (raiz.getHijos().ElementAt(4) == null)
                    {
                        if (condicion.ToLower().Equals("o"))
                        {
                            AGREGAR_TODAS_POSICIONES2(id, condicional, valor, tabla, raiz);
                        }
                        else if (condicion.ToLower().Equals("y"))
                        {
                            VERIFICAR_POSICIONES(id, condicional, valor, tabla);
                            foreach (int a in posiA)
                            {
                                foreach (int b in posiB)
                                {
                                    if (a == b)
                                    {
                                        posiciones.Add(b);
                                    }
                                }
                            }
                            posiciones = posiciones.OrderBy(v => v).ToList();
                        }
                    }
                }
            }
        }
        public void AGREGAR_TODAS_POSICIONES2(string id, string condicional, string valor, Tabla tabla, NodoArbol raiz)
        {
            int posicion = 0;
            NodoArbol hijo = null;
            if (raiz != null)
            {
                hijo = raiz.getHijos().ElementAt(4);
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
                        posicion = 0;
                        if (raiz!=null)
                        {
                            if (raiz.getHijos().ElementAt(4).getHijos() != null)
                            {
                                this.MUCHAS_CONDICIONES(raiz.getHijos().ElementAt(1).getValor(), raiz.getHijos().ElementAt(2).getHijos().ElementAt(0).getValor(), raiz.getHijos().ElementAt(3).getValor(), tabla, raiz.getHijos().ElementAt(4), raiz.getHijos().ElementAt(4).getHijos().ElementAt(0).getValor());
                            }
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
                        posicion = 0;
                        if (raiz != null)
                        {
                            if (raiz.getHijos().ElementAt(4).getHijos() != null)
                            {
                                this.MUCHAS_CONDICIONES(raiz.getHijos().ElementAt(1).getValor(), raiz.getHijos().ElementAt(2).getHijos().ElementAt(0).getValor(), raiz.getHijos().ElementAt(3).getValor(), tabla, raiz.getHijos().ElementAt(4), raiz.getHijos().ElementAt(4).getHijos().ElementAt(0).getValor());
                            }
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
                        posicion = 0;
                        if (raiz != null)
                        {
                            if (raiz.getHijos().ElementAt(4).getHijos() != null)
                            {
                                this.MUCHAS_CONDICIONES(raiz.getHijos().ElementAt(1).getValor(), raiz.getHijos().ElementAt(2).getHijos().ElementAt(0).getValor(), raiz.getHijos().ElementAt(3).getValor(), tabla, raiz.getHijos().ElementAt(4), raiz.getHijos().ElementAt(4).getHijos().ElementAt(0).getValor());
                            }
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
                        posicion = 0;
                        if (raiz != null)
                        {
                            if (raiz.getHijos().ElementAt(4).getHijos() != null)
                            {
                                this.MUCHAS_CONDICIONES(raiz.getHijos().ElementAt(1).getValor(), raiz.getHijos().ElementAt(2).getHijos().ElementAt(0).getValor(), raiz.getHijos().ElementAt(3).getValor(), tabla, raiz.getHijos().ElementAt(4), raiz.getHijos().ElementAt(4).getHijos().ElementAt(0).getValor());
                            }
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
                        posicion = 0;
                        if (raiz != null)
                        {
                            if (raiz.getHijos().ElementAt(4).getHijos() != null)
                            {
                                this.MUCHAS_CONDICIONES(raiz.getHijos().ElementAt(1).getValor(), raiz.getHijos().ElementAt(2).getHijos().ElementAt(0).getValor(), raiz.getHijos().ElementAt(3).getValor(), tabla, raiz.getHijos().ElementAt(4), raiz.getHijos().ElementAt(4).getHijos().ElementAt(0).getValor());
                            }
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
                        posicion = 0;
                        if (raiz != null)
                        {
                            if (raiz.getHijos().ElementAt(4).getHijos() != null)
                            {
                                this.MUCHAS_CONDICIONES(raiz.getHijos().ElementAt(1).getValor(), raiz.getHijos().ElementAt(2).getHijos().ElementAt(0).getValor(), raiz.getHijos().ElementAt(3).getValor(), tabla, raiz.getHijos().ElementAt(4), raiz.getHijos().ElementAt(4).getHijos().ElementAt(0).getValor());
                            }
                        }
                    }
                }
            }
        }
        public void VERIFICAR_POSICIONES2(string id, string condicional, string valor, Tabla tabla)
        {
            posiA = posiciones;
            if (posiA.Count() == 0)
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
                                    posiA.Add(posicion);
                                    break;
                                }
                                else
                                {
                                    posicion++;
                                }
                            }
                            posicion = 0;
                        }
                        else if (condicional.Equals("!="))
                        {
                            foreach (Tupla datos in col.tuplas)
                            {
                                if (datos.valores.ElementAt(0).Equals(valor) && !posiciones.Contains(posicion))
                                {
                                    posiA.Add(posicion);
                                    break;
                                }
                                else
                                {
                                    posicion++;
                                }
                            }
                            posicion = 0;
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
                                        posiA.Add(posicion);
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
                                        posiA.Add(posicion);
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
                            posicion = 0;
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
                                        posiA.Add(posicion);
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
                                        posiA.Add(posicion);
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
                            posicion = 0;
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
                                        posiA.Add(posicion);
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
                                        posiA.Add(posicion);
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
                            posicion = 0;
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
                                        posiA.Add(posicion);
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
                                        posiA.Add(posicion);
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
                            posicion = 0;
                        }
                    }
                }
            }
            else
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
                                    posiB.Add(posicion);
                                    break;
                                }
                                else
                                {
                                    posicion++;
                                }
                            }
                            posicion = 0;
                        }
                        else if (condicional.Equals("!="))
                        {
                            foreach (Tupla datos in col.tuplas)
                            {
                                if (datos.valores.ElementAt(0).Equals(valor) && !posiciones.Contains(posicion))
                                {
                                    posiB.Add(posicion);
                                    break;
                                }
                                else
                                {
                                    posicion++;
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
                                        posiB.Add(posicion);
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
                                        posiB.Add(posicion);
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
                            posicion = 0;
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
                                        posiB.Add(posicion);
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
                                        posiB.Add(posicion);
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
                            posicion = 0;
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
                                        posiB.Add(posicion);
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
                                        posiB.Add(posicion);
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
                            posicion = 0;
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
                                        posiB.Add(posicion);
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
                                        posiB.Add(posicion);
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
                            posicion = 0;
                        }
                    }

                }
            }
        }
        public void UNA_CONDICION(string id, string condicional, string valor, Tabla tabla)
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
                        posicion = 0;
                    }
                    else if (condicional.Equals("!="))
                    {
                        foreach (Tupla datos in col.tuplas)
                        {
                            if (datos.valores.ElementAt(0).Equals(valor) && !posiciones.Contains(posicion))
                            {
                                posicion++;
                            }
                            else
                            {
                                posiciones.Add(posicion);
                                posicion++;
                            }
                        }
                        posicion = 0;
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
                        posicion = 0;
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
                        posicion = 0;
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
                        posicion = 0;
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
                        posicion = 0;
                    }
                }

            }
        }

        public void SELECCION(NodoArbol raiz, int consulta)
        {
            if (raiz.getHijos().Count() == 5 && raiz.getHijos().ElementAt(4) == null) //sin condiciones
            {
                PARAMETROS_SELECCION(raiz.getHijos().ElementAt(1));
                LISTA_TABLAS(raiz.getHijos().ElementAt(3));
                if (parametros.Count() == 1 && parametros.ElementAt(0).Equals("*"))
                {
                    GENERAR_TODOS();
                }
                else
                {
                    GENERAR_ESPECIFICOS();
                }
                parametros.Clear();
                ltablas.Clear();
            }else if (raiz.getHijos().Count() == 5 && raiz.getHijos().ElementAt(4) != null)//con condiciones
            {
                PARAMETROS_SELECCION(raiz.getHijos().ElementAt(1));
                LISTA_TABLAS(raiz.getHijos().ElementAt(3));
                foreach (Tabla tab in tablas)
                {
                    foreach (string id in ltablas)
                    {
                        if (tab.nombre.Equals(id))
                        {
                            this.CONDICION_SELECCION(raiz.getHijos().ElementAt(4), tab);
                        }
                    }
                }
                posiciones = posiciones.OrderBy(v => v).ToList();
                GENERAR_CONDICIONAL();
                parametros.Clear();
                ltablas.Clear();
                posiA.Clear();
                posiB.Clear();
                posiciones.Clear();
            }
        }

        public void PARAMETROS_SELECCION(NodoArbol raiz)
        {
            if (raiz.getHijos().Count == 1 && raiz.getHijos().ElementAt(0).getValor().Equals("*"))// condicion para cuando vienen todos
            {
                parametros.Add("*");
            }
            else if (raiz.getHijos().Count == 4 && (raiz.getHijos().ElementAt(1) == null && raiz.getHijos().ElementAt(2) == null && raiz.getHijos().ElementAt(3) == null))// un solo parametro, sin alias
            {
                parametros.Add(raiz.getHijos().ElementAt(0).getValor());
                alias.Add("");
            }
            else if (raiz.getHijos().Count == 4 && (raiz.getHijos().ElementAt(1) == null && raiz.getHijos().ElementAt(2) == null && raiz.getHijos().ElementAt(3) != null))// n parametros, sin alias en el primero
            {
                parametros.Add(raiz.getHijos().ElementAt(0).getValor());
                alias.Add("");
                PARAMETROS_SELECCION(raiz.getHijos().ElementAt(3));
            }
            else if (raiz.getHijos().Count == 4 && (raiz.getHijos().ElementAt(1) == null && raiz.getHijos().ElementAt(2) != null && raiz.getHijos().ElementAt(3) == null))// un solo parametro, con alias
            {
                parametros.Add(raiz.getHijos().ElementAt(0).getValor());
                alias.Add(raiz.getHijos().ElementAt(2).getHijos().ElementAt(1).getValor());
            }
            else if (raiz.getHijos().Count == 4 && (raiz.getHijos().ElementAt(1) == null && raiz.getHijos().ElementAt(2) != null && raiz.getHijos().ElementAt(3) != null))// n parametro, con alias en el primero
            {
                parametros.Add(raiz.getHijos().ElementAt(0).getValor());
                alias.Add(raiz.getHijos().ElementAt(2).getHijos().ElementAt(1).getValor());
                PARAMETROS_SELECCION(raiz.getHijos().ElementAt(3));
            }
            else if (raiz.getHijos().Count == 4 && (raiz.getHijos().ElementAt(1) != null && raiz.getHijos().ElementAt(2) == null && raiz.getHijos().ElementAt(3) == null))// un solo parametro, sin alias y con especificacion de tabla
            {

            }
            else if (raiz.getHijos().Count == 4 && (raiz.getHijos().ElementAt(1) != null && raiz.getHijos().ElementAt(2) == null && raiz.getHijos().ElementAt(3) == null))// n parametro, sin alias y con especificacion de tabla en el primero
            {

            }
            else if (raiz.getHijos().Count == 4 && (raiz.getHijos().ElementAt(1) != null && raiz.getHijos().ElementAt(2) != null && raiz.getHijos().ElementAt(3) == null))// un solo parametro, con alias y con especificacion de tabla
            {

            }
            else if (raiz.getHijos().Count == 4 && (raiz.getHijos().ElementAt(1) != null && raiz.getHijos().ElementAt(2) != null && raiz.getHijos().ElementAt(3) != null))// n parametros, con alias y con especificacion de tabla
            {

            }
        }

        
        public void LISTA_TABLAS(NodoArbol raiz)
        {
            if (raiz.getHijos().Count() == 2 && raiz.getHijos().ElementAt(1) == null)
            {
                ltablas.Add(raiz.getHijos().ElementAt(0).getValor());
            }
            else if (raiz.getHijos().Count() == 2 && raiz.getHijos().ElementAt(1) != null)
            {
                ltablas.Add(raiz.getHijos().ElementAt(0).getValor());
                LISTA_TABLAS(raiz.getHijos().ElementAt(1));
            }
        }

        public void GENERAR_CONDICIONAL()
        {
            String texto = "";
            texto += "<html>";
            texto += "\n<head><title>CONSULTA No." + consulta + "</title></head>\n";
            texto += "<body bgcolor=\"aqua\"> \n <table border=\"2\" align=\"center\">\n";
            foreach (Tabla tab in tablas)
            {
                foreach (string tabact in ltablas)
                {
                    if (tab.nombre.Equals(tabact))
                    {
                        texto += "<tr><td>" + tab.nombre + "</td></tr>";
                        texto += "<colgroup span=" + (tab.campo.Count() - 1) + "></colgroup >";
                        foreach (Columna campos in tab.campo)
                        {
                            texto += "<tr><td>" + campos.nombre + "</td></tr>";
                            texto += "<tr>";
                            for (int i = campos.tuplas.Count() - 1; i >= 0; i--)
                            {
                                try
                                {
                                    texto += "<td>" + campos.tuplas.ElementAt(posiciones.ElementAt(i)).valores.ElementAt(0) + "</td>";
                                }
                                catch (Exception ex)
                                {

                                }
                            }
                            texto += "</tr>";
                        }
                    }
                }
            }
            texto += "</table>\n</body>\n</html>";
            string fileName = "consulta" + consulta + ".html";
            StreamWriter writer = File.CreateText(fileName);
            writer.WriteLine(texto);
            writer.Close();
            Process.Start(@"" + "consulta" + consulta + ".html");
        }
        public void GENERAR_TODOS()
        {
            String texto = "";
            texto += "<html>";
            texto += "\n<head><title>CONSULTA No."+consulta+"</title></head>\n";
            texto += "<body bgcolor=\"aqua\"> \n <table border=\"2\" align=\"center\">\n";
            foreach (Tabla tab in tablas)
            {
                foreach (string tabact in ltablas)
                {
                    if (tab.nombre.Equals(tabact))
                    {
                        texto += "<tr><td>" + tab.nombre + "</td></tr>";
                        texto += "<colgroup span=" + (tab.campo.Count() - 1) + "></colgroup >";
                        foreach (Columna campos in tab.campo)
                        {
                            texto += "<tr><td>" + campos.nombre + "</td></tr>";
                            texto += "<tr>";
                            foreach (Tupla datos in campos.tuplas)
                            {

                                for (int i = 0; i < datos.valores.Count(); i++)
                                {
                                    texto += "<td>" + datos.valores.ElementAt(i) + "</td>";
                                }
                            }
                            texto += "</tr>";
                        }
                    }
                }
            }
            texto += "</table>\n</body>\n</html>";
            string fileName = "consulta"+consulta+".html";
            StreamWriter writer = File.CreateText(fileName);
            writer.WriteLine(texto);
            writer.Close();
            Process.Start(@"" + "consulta" + consulta + ".html");
        }
        public void GENERAR_ESPECIFICOS()
        {
            String texto = "";
            texto += "<html>";
            texto += "\n<head><title>CONSULTA No." + consulta + "</title></head>\n";
            texto += "<body bgcolor=\"aqua\"> \n <table border=\"2\" align=\"center\">\n";
            foreach (Tabla tab in tablas)
            {
                foreach (string tabact in ltablas)
                {
                    if (tab.nombre.Equals(tabact))
                    {
                        texto += "<tr><td>" + tab.nombre + "</td></tr>";
                        texto += "<colgroup span=" + (tab.campo.Count() - 1) + "></colgroup >";
                        foreach (Columna campos in tab.campo)
                        {
                            foreach (string camp in parametros)
                            {
                                if (campos.nombre.Equals(camp))
                                {
                                    if (alias.ElementAt(parametros.IndexOf(camp)) == "")
                                    {
                                        texto += "<tr><td>" + campos.nombre + "</td></tr>";
                                        texto += "<tr>";
                                        foreach (Tupla datos in campos.tuplas)
                                        {

                                            for (int i = 0; i < datos.valores.Count(); i++)
                                            {
                                                texto += "<td>" + datos.valores.ElementAt(i) + "</td>";
                                            }
                                        }
                                        texto += "</tr>";
                                    }
                                    else
                                    {
                                        texto += "<tr><td>" + alias.ElementAt(parametros.IndexOf(camp)) + "</td></tr>";
                                        texto += "<tr>";
                                        foreach (Tupla datos in campos.tuplas)
                                        {

                                            for (int i = 0; i < datos.valores.Count(); i++)
                                            {
                                                texto += "<td>" + datos.valores.ElementAt(i) + "</td>";
                                            }
                                        }
                                        texto += "</tr>";
                                    }
                                }
                            }
                        }
                    }
                }
            }
            texto += "</table>\n</body>\n</html>";
            string fileName = "consulta" + consulta + ".html";
            StreamWriter writer = File.CreateText(fileName);
            writer.WriteLine(texto);
            writer.Close();
            Process.Start(@"" + "consulta" + consulta + ".html");
        }
    }
}

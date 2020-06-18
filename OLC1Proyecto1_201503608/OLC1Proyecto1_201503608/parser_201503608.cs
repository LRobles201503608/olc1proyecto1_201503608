using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OLC1Proyecto1_201503608
{
    class parser_201503608
    {
        List<Token> tokensASintactico = new List<Token>();
        List<Errores> error= new List<Errores>();
        NodoArbol padre = null;

        public NodoArbol getPadre()
        {
            return padre;
        }
        int count = 0;

        int tokenactual = 0;
        int i = 0;
        int reservada = 1;
        int identificador = 2;
        int parentesisa = 3;
        int parentesisc = 4;
        int coma = 5;
        int pycoma = 6;
        int numero = 7;
        int cadena = 8;
        int fecha = 9;
        int asterisco = 10;
        int punto = 11;
        int comparador = 12;
        int tipo = 14;

        public parser_201503608(List<Token> tokensASintactico, List<Errores> error)
        {
            this.tokensASintactico = tokensASintactico;
            this.error = error;
            tokenactual = this.tokensASintactico.ElementAt(i).id;
        }
        public void Match(int tokenterminal)
        {
            if (tokenactual==tokenterminal)
            {
                i++;
                if (i==tokensASintactico.Count)
                {

                }
                else
                {
                    tokenactual = tokensASintactico.ElementAt(i).id;
                }
                
            }
            else
            {
                
                error.Add(new Errores("Error Sintactico","Caracter"+this.tokensASintactico.ElementAt(i).lexemaval, this.tokensASintactico.ElementAt(i).linea, this.tokensASintactico.ElementAt(i).columna));
                i++;
                if (this.tokensASintactico.ElementAt(i).id==pycoma)
                {
                    i--;
                }
                if (i == tokensASintactico.Count)
                {

                }
                else
                {
                    tokenactual = tokensASintactico.ElementAt(i).id;
                }
            }
        }

        public void INICIO()
        {
            NodoArbol nd = new NodoArbol("INICIO","",count);
            count++;
            NodoArbol a=INSTRUCCION();
            nd.AddHijos(a);
            padre = nd;
            MessageBox.Show("ANALISIS SINTACTICO COMPLETADO");
        }
        public NodoArbol INSTRUCCION()
        {
            NodoArbol a = new NodoArbol("INSTRUCCION","",count);
            count++;
            NodoArbol c = OPERACION();
            a.AddHijos(c);
            return a;
        }
        public NodoArbol OPERACION()
        {
            if (i>=this.tokensASintactico.Count)
            {
                return null;
            }
            else
            {
                if (this.tokensASintactico.ElementAt(i).lexemaval.ToLower().Equals("crear"))
                {
                    NodoArbol a = new NodoArbol("OPERACION", "", count);
                    count++;
                    NodoArbol creacion = new NodoArbol("CREACION", "", count);
                    count++;
                    NodoArbol crear = new NodoArbol("crear", this.tokensASintactico.ElementAt(i).lexemaval.ToLower(),count);
                    count++;
                    Match(reservada);
                    NodoArbol tabla = new NodoArbol("tabla", this.tokensASintactico.ElementAt(i).lexemaval.ToLower(), count);
                    count++;
                    Match(reservada);
                    NodoArbol id = new NodoArbol("id", this.tokensASintactico.ElementAt(i).lexemaval.ToLower(), count);
                    count++;
                    Match(identificador);
                    Match(parentesisa);
                    NodoArbol c= CAMPOS_CREACION();
                    Match(parentesisc);
                    Match(pycoma);
                    NodoArbol d =OPERACION();

                    a.AddHijos(creacion);
                    creacion.AddHijos(crear);
                    creacion.AddHijos(tabla);
                    creacion.AddHijos(id);
                    creacion.AddHijos(c);
                    a.AddHijos(d);
                    return a;
                }
                else if (this.tokensASintactico.ElementAt(i).lexemaval.ToLower().Equals("insertar"))
                {
                    NodoArbol a = new NodoArbol("OPERACION", "", count);
                    count++;
                    NodoArbol insercion = new NodoArbol("INSERCION", "", count);
                    count++;
                    NodoArbol insertar = new NodoArbol("insertar", this.tokensASintactico.ElementAt(i).lexemaval.ToLower(), count);
                    count++;
                    Match(reservada);
                    NodoArbol en = new NodoArbol("en", this.tokensASintactico.ElementAt(i).lexemaval.ToLower(), count);
                    count++;
                    Match(reservada);
                    NodoArbol id = new NodoArbol("id", this.tokensASintactico.ElementAt(i).lexemaval.ToLower(), count);
                    count++;
                    Match(identificador);
                    NodoArbol valores = new NodoArbol("valores", this.tokensASintactico.ElementAt(i).lexemaval.ToLower(), count);
                    count++;
                    Match(reservada);
                    Match(parentesisa);
                    NodoArbol c=PARAMETROS_INSERTAR();
                    Match(parentesisc);
                    Match(pycoma);
                    NodoArbol d=OPERACION();
                    a.AddHijos(insercion);
                    insercion.AddHijos(insertar);
                    insercion.AddHijos(en);
                    insercion.AddHijos(id);
                    insercion.AddHijos(valores);
                    insercion.AddHijos(c);
                    a.AddHijos(d);
                    return a;
                }
                else if (this.tokensASintactico.ElementAt(i).lexemaval.ToLower().Equals("seleccionar"))
                {
                    NodoArbol a = new NodoArbol("OPERACION", "", count);
                    count++;
                    NodoArbol SELECCION = new NodoArbol("SELECCION", "", count);
                    count++;
                    NodoArbol seleccionar = new NodoArbol("seleccionar", this.tokensASintactico.ElementAt(i).lexemaval.ToLower(), count);
                    count++;
                    Match(reservada);
                    NodoArbol c = PARAMETROS_SELECCION();
                    NodoArbol de = new NodoArbol("de", this.tokensASintactico.ElementAt(i).lexemaval.ToLower(), count);
                    count++;
                    Match(reservada);
                    NodoArbol d = LISTA_TABLAS();
                    NodoArbol e=CONDICION_SELECCION();
                    Match(pycoma);
                    NodoArbol f=OPERACION();
                    a.AddHijos(SELECCION);
                    SELECCION.AddHijos(seleccionar);
                    SELECCION.AddHijos(c);
                    SELECCION.AddHijos(de);
                    SELECCION.AddHijos(d);
                    SELECCION.AddHijos(e);
                    a.AddHijos(f);
                    return a;
                }
                else if (this.tokensASintactico.ElementAt(i).lexemaval.ToLower().Equals("eliminar"))
                {
                    NodoArbol a = new NodoArbol("OPERACION", "", count);
                    count++;
                    NodoArbol eliminacion = new NodoArbol("ELIMINACION", "", count);
                    count++;
                    NodoArbol eliminar = new NodoArbol("eliminar", this.tokensASintactico.ElementAt(i).lexemaval.ToLower(), count);
                    count++;
                    Match(reservada);
                    NodoArbol de = new NodoArbol("de", this.tokensASintactico.ElementAt(i).lexemaval.ToLower(), count);
                    count++;
                    Match(reservada);
                    NodoArbol id = new NodoArbol("id", this.tokensASintactico.ElementAt(i).lexemaval.ToLower(), count);
                    count++;
                    Match(identificador);
                    NodoArbol c=CONDICIONES();
                    Match(pycoma);
                    NodoArbol d=OPERACION();
                    a.AddHijos(eliminacion);
                    eliminacion.AddHijos(eliminar);
                    eliminacion.AddHijos(de);
                    eliminacion.AddHijos(id);
                    eliminacion.AddHijos(c);
                    a.AddHijos(d);
                    return a;
                }
                else if (this.tokensASintactico.ElementAt(i).lexemaval.ToLower().Equals("actualizar"))
                {
                    NodoArbol a = new NodoArbol("OPERACION", "", count);
                    count++;
                    NodoArbol actualizacion = new NodoArbol("ACTUALIZACION", "", count);
                    count++;
                    NodoArbol actualizar = new NodoArbol("actualizar", this.tokensASintactico.ElementAt(i).lexemaval.ToLower(), count);
                    count++;
                    Match(reservada);
                    NodoArbol id = new NodoArbol("id", this.tokensASintactico.ElementAt(i).lexemaval.ToLower(), count);
                    count++;
                    Match(identificador);
                    NodoArbol establecer = new NodoArbol("establecer", this.tokensASintactico.ElementAt(i).lexemaval.ToLower(), count);
                    count++;
                    Match(reservada);
                    Match(parentesisa);
                    NodoArbol c=PARAMETROS_ESTABLECER();
                    Match(parentesisc);
                    NodoArbol d = CONDICION_SELECCION();
                    Match(pycoma);
                    NodoArbol e = OPERACION();
                    a.AddHijos(actualizacion);
                    actualizacion.AddHijos(actualizar);
                    actualizacion.AddHijos(id);
                    actualizacion.AddHijos(establecer);
                    actualizacion.AddHijos(c);
                    actualizacion.AddHijos(d);
                    a.AddHijos(e);
                    return a;
                }
                else
                {
                    return null;
                }
            }
        }
        public NodoArbol CAMPOS_CREACION()
        {
            if (this.tokensASintactico.ElementAt(i).id==identificador)
            {
                NodoArbol a = new NodoArbol("CAMPOS_CREACION", "", count);
                count++;
                NodoArbol id = new NodoArbol("id", this.tokensASintactico.ElementAt(i).lexemaval.ToLower(), count);
                count++;
                Match(identificador);
                NodoArbol c = TIPOS();
                NodoArbol d = CAMPOS_CREACION();
                a.AddHijos(id);
                a.AddHijos(c);
                a.AddHijos(d);
                return a;
            }
            else if (this.tokensASintactico.ElementAt(i).id==coma)
            {
                NodoArbol a = new NodoArbol("CAMPOS_CREACION", "", count);
                count++;
                Match(coma);
                NodoArbol id = new NodoArbol("id", this.tokensASintactico.ElementAt(i).lexemaval.ToLower(), count);
                count++;
                Match(identificador);
                NodoArbol c = TIPOS();
                NodoArbol d = CAMPOS_CREACION();
                a.AddHijos(id);
                a.AddHijos(c);
                a.AddHijos(d);
                return a;
            }
            else
            {
                return null;
            }
        }
        public NodoArbol TIPOS()
        {
            if (this.tokensASintactico.ElementAt(i).lexemaval.ToLower().Equals("cadena"))
            {
                NodoArbol tipos = new NodoArbol("tipo", this.tokensASintactico.ElementAt(i).lexemaval.ToLower().Replace("\"",""), count);
                count++;
                Match(tipo);
                return tipos;
            }else if(this.tokensASintactico.ElementAt(i).lexemaval.ToLower().Equals("flotante"))
            {
                NodoArbol tipos = new NodoArbol("tipo", this.tokensASintactico.ElementAt(i).lexemaval.ToLower(), count);
                count++;
                Match(tipo);
                return tipos;
            }
            else if (this.tokensASintactico.ElementAt(i).lexemaval.ToLower().Equals("entero"))
            {
                NodoArbol tipos = new NodoArbol("tipo", this.tokensASintactico.ElementAt(i).lexemaval.ToLower(), count);
                count++;
                Match(tipo);
                return tipos;
            }
            else if (this.tokensASintactico.ElementAt(i).lexemaval.ToLower().Equals("fecha"))
            {
                NodoArbol tipos = new NodoArbol("tipo", this.tokensASintactico.ElementAt(i).lexemaval.ToLower(), count);
                count++;
                Match(tipo);
                return tipos;
            }
            return null;
        }
        public NodoArbol PARAMETROS_INSERTAR()
        {
            if (this.tokensASintactico.ElementAt(i).id==numero|| this.tokensASintactico.ElementAt(i).id==fecha|| this.tokensASintactico.ElementAt(i).id==cadena)
            {
                NodoArbol a = new NodoArbol("PARAMETROS_INSERTAR", "", count);
                count++;
                NodoArbol c = EXPRESION();
                NodoArbol d = PARAMETROS_INSERTAR();
                a.AddHijos(c);
                a.AddHijos(d);
                return a;
            }
            else if (this.tokensASintactico.ElementAt(i).id==coma)
            {
                NodoArbol a = new NodoArbol("PARAMETROS_INSERTAR", "", count);
                count++;
                Match(coma);
                NodoArbol c = EXPRESION();
                NodoArbol d = PARAMETROS_INSERTAR();
                a.AddHijos(c);
                a.AddHijos(d);
                return a;
            }
            else
            {
                return null;
            }
            
        }
        public NodoArbol EXPRESION()
        {
            if (this.tokensASintactico.ElementAt(i).id == numero )
            {
                NodoArbol value = new NodoArbol("value", this.tokensASintactico.ElementAt(i).lexemaval.ToLower(), count);
                count++;
                Match(numero);
                return value;
            }else if (this.tokensASintactico.ElementAt(i).id == fecha )
            {
                NodoArbol value = new NodoArbol("value", this.tokensASintactico.ElementAt(i).lexemaval.ToLower(), count);
                count++;
                Match(fecha);
                return value;
            }else if (this.tokensASintactico.ElementAt(i).id == cadena)
            {
                NodoArbol value = new NodoArbol("value", this.tokensASintactico.ElementAt(i).lexemaval.ToLower().Replace("\"",""), count);
                count++;
                Match(cadena);
                return value;
            }
            return null;
        }
        public NodoArbol PARAMETROS_SELECCION()
        {
            

            if (this.tokensASintactico.ElementAt(i).id == identificador)
            {
                NodoArbol a = new NodoArbol("PARAMETROS_SELECCION", "", count);
                count++;
                NodoArbol id = new NodoArbol("id", this.tokensASintactico.ElementAt(i).lexemaval.ToLower(), count);
                count++;
                Match(identificador);
                NodoArbol c = PARAMETROS_SELECCION2();
                NodoArbol c2 = ALIAS();
                NodoArbol d = PARAMETROS_SELECCION();
                a.AddHijos(id);
                a.AddHijos(c);
                a.AddHijos(c2);
                a.AddHijos(d);
                return a;
            }
            else if (this.tokensASintactico.ElementAt(i).id == coma)
            {
                NodoArbol a = new NodoArbol("PARAMETROS_SELECCION", "", count);
                count++;
                Match(coma);
                NodoArbol id = new NodoArbol("id", this.tokensASintactico.ElementAt(i).lexemaval.ToLower(), count);
                count++;
                Match(identificador);
                NodoArbol c = PARAMETROS_SELECCION2();
                NodoArbol c2 = ALIAS();
                NodoArbol d = PARAMETROS_SELECCION();
                a.AddHijos(id);
                a.AddHijos(c);
                a.AddHijos(c2);
                a.AddHijos(d);
                return a;
            }
            else if (this.tokensASintactico.ElementAt(i).id == asterisco)
            {
                NodoArbol a = new NodoArbol("PARAMETROS_SELECCION", "", count);
                count++;
                NodoArbol asteriscos = new NodoArbol("asterisco", this.tokensASintactico.ElementAt(i).lexemaval.ToLower(), count);
                count++;
                Match(asterisco);
                a.AddHijos(asteriscos);
                return a;
            }
            else
            {
                return null;
            }
        }
        public NodoArbol PARAMETROS_SELECCION2()
        {   
            if (this.tokensASintactico.ElementAt(i).id == punto)
            {
                NodoArbol a = new NodoArbol("PARAMETROS_SELECCION2", "", count);
                count++;
                Match(punto);
                NodoArbol id = new NodoArbol("id", this.tokensASintactico.ElementAt(i).lexemaval.ToLower(), count);
                count++;
                Match(identificador);
                a.AddHijos(id);
                return a;
            }
            else
            {
                return null;
            }
        }

        public NodoArbol ALIAS()
        {
            if (this.tokensASintactico.ElementAt(i).id == reservada)
            {
                NodoArbol alias = new NodoArbol("ALIAS", "", count);
                NodoArbol reservadas = new NodoArbol("reservada", this.tokensASintactico.ElementAt(i).lexemaval.ToLower(), count);
                Match(reservada);
                NodoArbol id = new NodoArbol("id", this.tokensASintactico.ElementAt(i).lexemaval.ToLower(), count);
                Match(identificador);
                alias.AddHijos(reservadas);
                alias.AddHijos(id);
                return alias;
            }
            else
            {
                return null;
            }
        }
        public NodoArbol LISTA_TABLAS()
        {
            if (this.tokensASintactico.ElementAt(i).id == identificador)
            {
                NodoArbol a = new NodoArbol("LISTA_TABLAS", "", count);
                count++;
                NodoArbol id = new NodoArbol("id", this.tokensASintactico.ElementAt(i).lexemaval.ToLower(), count);
                count++;
                Match(identificador);
                NodoArbol c = LISTA_TABLAS();
                a.AddHijos(id);
                a.AddHijos(c);
                return a;
            }
            else if (this.tokensASintactico.ElementAt(i).id == coma)
            {
                NodoArbol a = new NodoArbol("LISTA_TABLAS", "", count);
                count++;
                Match(coma);
                NodoArbol id = new NodoArbol("id", this.tokensASintactico.ElementAt(i).lexemaval.ToLower(), count);
                count++;
                Match(identificador);
                NodoArbol c = LISTA_TABLAS();
                a.AddHijos(id);
                a.AddHijos(c);
                return a;
            }
            else
            {
                return null;
            }
        }
        public NodoArbol CONDICION_SELECCION()
        {
            if (this.tokensASintactico.ElementAt(i).id == reservada)
            {
                NodoArbol a = new NodoArbol("CONDICION_SELECCION", "", count);
                count++;
                NodoArbol donde = new NodoArbol("donde", this.tokensASintactico.ElementAt(i).lexemaval.ToLower(), count);
                count++;
                Match(reservada);
                NodoArbol c= CONDICIONES();
                a.AddHijos(donde);
                a.AddHijos(c);
                return a;
            }
            else
            {
                return null;
            }
        }
        public NodoArbol CONDICIONES()
        {
            if (this.tokensASintactico.ElementAt(i).id == identificador)
            {
                NodoArbol a = new NodoArbol("CONDICIONES", "", count);
                count++;
                NodoArbol id = new NodoArbol("id", this.tokensASintactico.ElementAt(i).lexemaval.ToLower(), count);
                count++;
                Match(identificador);
                NodoArbol c = CONDICIONALES();
                NodoArbol d = EXPRESION();
                NodoArbol e = CONDICIONES();
                a.AddHijos(id);
                a.AddHijos(c);
                a.AddHijos(d);
                a.AddHijos(e);
                return a;
            }
            else if (this.tokensASintactico.ElementAt(i).id == reservada)
            {
                NodoArbol a = new NodoArbol("CONDICIONES", "", count);
                count++;
                NodoArbol reservadas = new NodoArbol("reservada", this.tokensASintactico.ElementAt(i).lexemaval.ToLower(), count);
                count++;
                Match(reservada);
                NodoArbol id = new NodoArbol("id", this.tokensASintactico.ElementAt(i).lexemaval.ToLower(), count);
                count++;
                Match(identificador);
                NodoArbol c = CONDICIONALES();
                NodoArbol d = EXPRESION();
                NodoArbol e = CONDICIONES();
                a.AddHijos(reservadas);
                a.AddHijos(id);
                a.AddHijos(c);
                a.AddHijos(d);
                a.AddHijos(e);
                return a;
            }
            else
            {
                return null;
            }
        }
        public NodoArbol CONDICIONALES()
        {
            NodoArbol a = new NodoArbol("CONDICIONAL", "", count);
            count++;
            NodoArbol comparador1 = new NodoArbol("comparador", this.tokensASintactico.ElementAt(i).lexemaval.ToLower(), count);
            count++;
            Match(comparador);
            a.AddHijos(comparador1);
            return a;
        }
        public NodoArbol PARAMETROS_ESTABLECER()
        {
            if (this.tokensASintactico.ElementAt(i).id == identificador)
            {
                NodoArbol a = new NodoArbol("PARAMETROS_ESTABLECER", "", count);
                count++;
                NodoArbol id = new NodoArbol("id", this.tokensASintactico.ElementAt(i).lexemaval.ToLower(), count);
                count++;
                Match(identificador);
                NodoArbol comparador1 = new NodoArbol("comparador", this.tokensASintactico.ElementAt(i).lexemaval.ToLower(), count);
                count++;
                Match(comparador);
                NodoArbol c = EXPRESION();
                NodoArbol d = PARAMETROS_ESTABLECER();
                a.AddHijos(id);
                a.AddHijos(comparador1);
                a.AddHijos(c);
                a.AddHijos(d);
                return a;
            }
            else if (this.tokensASintactico.ElementAt(i).id == coma)
            {
                NodoArbol a = new NodoArbol("PARAMETROS_ESTABLECER", "", count);
                count++;
                Match(coma);
                NodoArbol id = new NodoArbol("id", this.tokensASintactico.ElementAt(i).lexemaval.ToLower(), count);
                count++;
                Match(identificador);
                NodoArbol comparador1 = new NodoArbol("comparador", this.tokensASintactico.ElementAt(i).lexemaval.ToLower(), count);
                count++;
                Match(comparador);
                NodoArbol c = EXPRESION();
                NodoArbol d = PARAMETROS_ESTABLECER();
                a.AddHijos(id);
                a.AddHijos(comparador1);
                a.AddHijos(c);
                a.AddHijos(d);
                return a;
            }
            else
            {
                return null;
            }
        }
    }
}

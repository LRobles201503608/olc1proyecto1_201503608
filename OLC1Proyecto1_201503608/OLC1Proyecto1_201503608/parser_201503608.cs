using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLC1Proyecto1_201503608
{
    class parser_201503608
    {
        List<Token> tokensASintactico = new List<Token>();
        List<Errores> error= new List<Errores>();
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
            INSTRUCCION();
        }
        public void INSTRUCCION()
        {
                OPERACION();   
        }
        public void OPERACION()
        {
            if (i>=this.tokensASintactico.Count)
            {
                
            }
            else
            {
                if (this.tokensASintactico.ElementAt(i).lexemaval.ToLower().Equals("crear"))
                {
                    Match(reservada);
                    Match(reservada);
                    Match(identificador);
                    Match(parentesisa);
                    CAMPOS_CREACION();
                    Match(parentesisc);
                    Match(pycoma);
                    OPERACION();
                }
                else if (this.tokensASintactico.ElementAt(i).lexemaval.ToLower().Equals("insertar"))
                {
                    Match(reservada);
                    Match(reservada);
                    Match(identificador);
                    Match(reservada);
                    Match(parentesisa);
                    PARAMETROS_INSERTAR();
                    Match(parentesisc);
                    Match(pycoma);
                    OPERACION();
                }
                else if (this.tokensASintactico.ElementAt(i).lexemaval.ToLower().Equals("seleccionar"))
                {
                    Match(reservada);
                    PARAMETROS_SELECCION();
                    Match(reservada);
                    LISTA_TABLAS();
                    Match(reservada);
                    CONDICIONES();
                    Match(pycoma);
                    OPERACION();
                }
                else if (this.tokensASintactico.ElementAt(i).lexemaval.ToLower().Equals("eliminar"))
                {
                    Match(reservada);
                    Match(reservada);
                    Match(identificador);
                    CONDICIONES();
                    Match(pycoma);
                    OPERACION();
                }
                else if (this.tokensASintactico.ElementAt(i).lexemaval.ToLower().Equals("actualizar"))
                {
                    Match(reservada);
                    Match(identificador);
                    Match(reservada);
                    Match(parentesisa);
                    PARAMETROS_ESTABLECER();
                    Match(parentesisc);
                    Match(reservada);
                    CONDICIONES();
                    Match(pycoma);
                    OPERACION();
                }
                else
                {

                }
            }
            
        }
        public void CAMPOS_CREACION()
        {
            if (this.tokensASintactico.ElementAt(i).id==identificador)
            {
                Match(identificador);
                TIPOS();
                CAMPOS_CREACION();
            }else if (this.tokensASintactico.ElementAt(i).id==coma)
            {
                Match(coma);
                Match(identificador);
                TIPOS();
                CAMPOS_CREACION();
            }
            else
            {

            }
        }
        public void TIPOS()
        {
            if (this.tokensASintactico.ElementAt(i).lexemaval.ToLower().Equals("cadena"))
            {
                Match(tipo);
            }else if(this.tokensASintactico.ElementAt(i).lexemaval.ToLower().Equals("flotante"))
            {
                Match(tipo);
            }else if (this.tokensASintactico.ElementAt(i).lexemaval.ToLower().Equals("entero"))
            {
                Match(tipo);
            }else if (this.tokensASintactico.ElementAt(i).lexemaval.ToLower().Equals("fecha"))
            {
                Match(tipo);
            }
        }
        public void PARAMETROS_INSERTAR()
        {
            if (this.tokensASintactico.ElementAt(i).id==numero|| this.tokensASintactico.ElementAt(i).id==fecha|| this.tokensASintactico.ElementAt(i).id==cadena)
            {
                EXPRESION();
                PARAMETROS_INSERTAR();
            }else if (this.tokensASintactico.ElementAt(i).id==coma)
            {
                Match(coma);
                EXPRESION();
                PARAMETROS_INSERTAR();
            }
            else
            {

            }
        }
        public void EXPRESION()
        {
            if (this.tokensASintactico.ElementAt(i).id == numero )
            {
                Match(numero);
            }else if (this.tokensASintactico.ElementAt(i).id == fecha )
            {
                Match(fecha);
            }else if (this.tokensASintactico.ElementAt(i).id == cadena)
            {
                Match(cadena);
            }
        }
        public void PARAMETROS_SELECCION()
        {
            if (this.tokensASintactico.ElementAt(i).id == identificador)
            {
                Match(identificador);
                PARAMETROS_SELECCION2();
                PARAMETROS_SELECCION();
            }else if (this.tokensASintactico.ElementAt(i).id == coma)
            {
                Match(coma);
                Match(identificador);
                PARAMETROS_SELECCION2();
                PARAMETROS_SELECCION();
            }else if (this.tokensASintactico.ElementAt(i).id == asterisco)
            {
                Match(asterisco);
            }
            else
            {

            }
        }
        public void PARAMETROS_SELECCION2()
        {
            if (this.tokensASintactico.ElementAt(i).id == punto)
            {
                Match(punto);
                Match(identificador);
            }
            else
            {

            }
        }
        public void LISTA_TABLAS()
        {
            if (this.tokensASintactico.ElementAt(i).id == identificador)
            {
                Match(identificador);
                LISTA_TABLAS();
            }
            else if (this.tokensASintactico.ElementAt(i).id == coma)
            {
                Match(coma);
                Match(identificador);
                LISTA_TABLAS();
            }
            else
            {

            }
        }
        public void CONDICIONES()
        {
            if (this.tokensASintactico.ElementAt(i).id == identificador)
            {
                Match(identificador);
                CONDICIONALES();
                EXPRESION();
                CONDICIONES();
            }
            else if (this.tokensASintactico.ElementAt(i).id == reservada)
            {
                Match(reservada);
                Match(identificador);
                CONDICIONALES();
                EXPRESION();
                CONDICIONES();
            }
            else
            {

            }
        }
        public void CONDICIONALES()
        {
            Match(comparador);
        }
        public void PARAMETROS_ESTABLECER()
        {
            if (this.tokensASintactico.ElementAt(i).id == identificador)
            {
                Match(identificador);
                Match(comparador);
                EXPRESION();
                PARAMETROS_ESTABLECER();
            }
            else if (this.tokensASintactico.ElementAt(i).id == coma)
            {
                Match(coma);
                Match(identificador);
                Match(comparador);
                EXPRESION();
                PARAMETROS_ESTABLECER();
            }
            else
            {

            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLC1Proyecto1_201503608
{
    class scanner_201503608
    {
        private string cadena="";
        private List<Token> tokens;
        private List<Token> tokensASintactico;
        public scanner_201503608(string cadena, List<Token>tokens, List<Token> tokensASintactico)
        {
            this.cadena = cadena;
            this.tokens = tokens;
            this.tokensASintactico = tokensASintactico;
        }
    public void AnalisisLexico(string cadena)
        {
            int linea = 1;
            int columna = 0;
            int contacomillas = 0;
            String cadconcat = "";
            int estado = 0;
            int caracter = 0;
            char caracteractual;
            String nombreExpre = "";
            for (int i=0; i<cadena.Length;i++)
            {
                caracteractual = cadena.ElementAt(i);
                caracter=(int)cadena.ElementAt(i);
                columna++;
                if (caracter==10)
                {
                    linea++;
                    columna = 0;
                }
                switch (estado)
                {
                    case 0:
                        {
                            if ((caracter>64 && caracter<91)||(caracter>96 && caracter<123))
                            {
                                cadconcat += caracteractual;
                                estado = 1;
                            }else if (caracter ==40)
                            {
                                cadconcat += caracteractual;
                                estado = 2;
                            }else if (caracter==41)
                            {
                                cadconcat += caracteractual;
                                estado = 3;
                            }else if (caracter==44)
                            {
                                cadconcat += caracteractual;
                                estado = 4;
                            }else if (caracter==59)
                            {
                                cadconcat += caracteractual;
                                estado = 5;
                            }else if (caracter>47||caracter<58)
                            {
                                cadconcat += caracteractual;
                                estado = 6;
                            }else if (caracter==34)
                            {
                                cadconcat += caracteractual;
                                estado = 7;
                            }else if (caracter==39)
                            {
                                cadconcat += caracteractual;
                                estado = 8;
                            }else if (caracter==42)
                            {
                                cadconcat += caracteractual;
                                estado = 9;
                            }else if (caracter==46)
                            {
                                cadconcat += caracteractual;
                                estado = 10;
                            }else if (caracter==62)
                            {
                                cadconcat += caracteractual;
                                estado = 11;
                            }else if (caracter==60)
                            {
                                cadconcat += caracteractual;
                                estado = 12;
                            }else if (caracter==33)
                            {
                                cadconcat += caracteractual;
                                estado = 13;
                            }else if (caracter==61)
                            {
                                cadconcat += caracteractual;
                                estado = 14;
                            }else if (caracter==45)
                            {
                                cadconcat += caracteractual;
                                estado = 15;
                            }else if (caracter==47)
                            {
                                cadconcat += caracteractual;
                                estado = 16;
                            }else if (caracter==32||caracter==10||caracter==13)
                            {

                            }
                            else
                            {
                                //error
                            }
                            break;
                        }
                    case 1:
                        {
                            if ((caracter > 64 && caracter < 91) || (caracter > 96 && caracter < 123))
                            {
                                cadconcat += caracteractual;
                                estado = 1;
                            }
                            else if (caracter > 47 && caracter < 58)
                            {
                                cadconcat += caracteractual;
                                estado = 1;
                            }else if (caracter==95)
                            {
                                cadconcat += caracteractual;
                                estado = 1;
                            } else if (i + 1 == cadena.Length || cadena.ElementAt(i+1)==32|| cadena.ElementAt(i + 1) == 40|| cadena.ElementAt(i + 1) == 44)
                            {
                                if (cadconcat.ToLower().Equals("tabla")|| cadconcat.ToLower().Equals("insertar")|| cadconcat.ToLower().Equals("eliminar")
                                    || cadconcat.ToLower().Equals("modificar"))
                                {
                                    tokens.Add(new Token(cadconcat, 1, "Reservada", linea, columna));
                                    tokensASintactico.Add(new Token(cadconcat, 1, "Reservada", linea, columna));
                                    //agregar cambio de colores
                                }
                                else if (cadconcat.ToLower().Equals("crear")|| cadconcat.ToLower().Equals("entero")|| cadconcat.ToLower().Equals("cadena")
                                    || cadconcat.ToLower().Equals("flotante")|| cadconcat.ToLower().Equals("fecha")|| cadconcat.ToLower().Equals("en")
                                    || cadconcat.ToLower().Equals("valores")|| cadconcat.ToLower().Equals("seleccionar")|| cadconcat.ToLower().Equals("de")
                                    || cadconcat.ToLower().Equals("donde")|| cadconcat.ToLower().Equals("y")|| cadconcat.ToLower().Equals("establecer"))
                                {
                                    tokens.Add(new Token(cadconcat, 1, "Reservada", linea, columna));
                                    tokensASintactico.Add(new Token(cadconcat, 1, "Reservada", linea, columna));
                                    //agregar cambio de colores
                                }
                                else
                                {
                                    tokens.Add(new Token(cadconcat, 2, "identificador", linea, columna));
                                    tokensASintactico.Add(new Token(cadconcat, 2, "identificador", linea, columna));
                                    //agregar cambio de colores
                                }
                                
                                estado = 0;
                                cadconcat = "";
                            }
                            break;
                        }
                    case 2:
                        {
                            //aceptar
                            tokens.Add(new Token(cadconcat, 3, "ParentesisA", linea, columna));
                            tokensASintactico.Add(new Token(cadconcat, 3, "ParentesisA", linea, columna));
                            i--;
                            estado = 0;
                            cadconcat = "";
                            break;
                        }
                    case 3:
                        {
                            //aceptar
                            tokens.Add(new Token(cadconcat, 4, "ParentesisC", linea, columna));
                            tokensASintactico.Add(new Token(cadconcat, 4, "ParentesisC", linea, columna));
                            i--;
                            estado = 0;
                            cadconcat = "";
                            break;
                        }
                    case 4:
                        {
                            //aceptar
                            tokens.Add(new Token(cadconcat, 5, "Coma", linea, columna));
                            tokensASintactico.Add(new Token(cadconcat, 5, "Coma", linea, columna));
                            i--;
                            estado = 0;
                            cadconcat = "";
                            break;
                        }
                    case 5:
                        {
                            //aceptar
                            tokens.Add(new Token(cadconcat, 6, "PyComa", linea, columna));
                            tokensASintactico.Add(new Token(cadconcat, 6, "PyComa", linea, columna));
                            i--;
                            estado = 0;
                            cadconcat = "";
                            break;
                        }
                    case 6:
                        {
                            if (caracter > 47 && caracter < 58)
                            {
                                cadconcat += caracteractual;
                                estado = 6;
                            }else if (caracter==46)
                            {
                                cadconcat += caracteractual;
                                estado = 18;
                            }else if (i+1==cadena.Length||cadena.ElementAt(i+1)==44|| cadena.ElementAt(i + 1) == 41|| cadena.ElementAt(i + 1) == 32)
                            {
                                //aceptar
                                tokens.Add(new Token(cadconcat, 7, "numero", linea, columna));
                                tokensASintactico.Add(new Token(cadconcat, 7, "numero", linea, columna));
                                estado = 0;
                                cadconcat = "";
                            }
                            break;
                        }
                    case 7:
                        {
                            cadconcat += caracteractual;
                            estado = 18;
                            break;
                        }
                    case 8:
                        {
                            if (caracter==48)
                            {
                                cadconcat += caracteractual;
                                estado = 19;
                            }else if (caracter==49||caracter==50)
                            {
                                cadconcat += caracteractual;
                                estado = 20;
                            }
                            else if (caracter==51)
                            {
                                cadconcat += caracteractual;
                                estado = 21;
                            }
                            else
                            {
                                cadconcat += caracteractual;
                                estado = 20;
                                //agregar error
                            }
                            break;
                        }
                    case 9:
                        {
                            //aceptar
                            tokens.Add(new Token(cadconcat, 10, "asterisco", linea, columna));
                            tokensASintactico.Add(new Token(cadconcat, 10, "asterisco", linea, columna));
                            i--;
                            estado = 0;
                            cadconcat = "";
                            break;
                        }
                    case 10:
                        {
                            //aceptar
                            tokens.Add(new Token(cadconcat, 11, "punto", linea, columna));
                            tokensASintactico.Add(new Token(cadconcat, 11, "punto", linea, columna));
                            i--;
                            estado = 0;
                            cadconcat = "";
                            break;
                        }
                    case 11:
                        {
                            if (caracter==61)
                            {
                                cadconcat += caracteractual;
                                estado = 22;
                            }else if (caracter==32||(caracter > 64 && caracter < 91) || (caracter > 96 && caracter < 123)||(caracter > 47 && caracter < 58))
                            {
                                //aceptar
                                tokens.Add(new Token(cadconcat, 12, "comparador", linea, columna));
                                tokensASintactico.Add(new Token(cadconcat, 12, "comparador", linea, columna));
                                i--;
                                estado = 0;
                                cadconcat = "";
                                break;
                            }
                            break;
                        }
                    case 12:
                        {
                            if (caracter == 61)
                            {
                                cadconcat += caracteractual;
                                estado = 22;
                            }
                            else if (caracter == 32 || (caracter > 64 && caracter < 91) || (caracter > 96 && caracter < 123) || (caracter > 47 && caracter < 58))
                            {
                                //aceptar
                                tokens.Add(new Token(cadconcat, 12, "comparador", linea, columna));
                                tokensASintactico.Add(new Token(cadconcat, 12, "comparador", linea, columna));
                                i--;
                                estado = 0;
                                cadconcat = "";
                                break;
                            }
                            break;
                        }
                    case 13:
                        {
                            if (caracter == 61)
                            {
                                cadconcat += caracteractual;
                                estado = 22;
                            }
                            else
                            {
                                //error
                                estado = 0;
                                cadconcat = "";
                                break;
                            
                            }
                            break;
                        }
                    case 14:
                        {
                            //aceptar
                            tokens.Add(new Token(cadconcat, 12, "comparador", linea, columna));
                            tokensASintactico.Add(new Token(cadconcat, 12, "comparador", linea, columna));
                            i--;
                            estado = 0;
                            cadconcat = "";
                            break;
                        }
                    case 15:
                        {
                            if (caracter==45)
                            {
                                cadconcat += caracteractual;
                                estado = 24;
                            }
                            else
                            {
                                //error
                                estado = 24;
                                cadconcat = "";
                                break;
                            }
                            break;
                        }
                    case 16:
                        {
                            if (caracter==42)
                            {
                                cadconcat += caracteractual;
                                estado = 25;
                            }
                            else
                            {
                                //error
                                estado = 24;
                                cadconcat = "";
                                break;
                            }
                            break;
                        }
                    case 17:
                        {
                            if (caracter > 47 && caracter < 58)
                            {
                                cadconcat += caracteractual;
                                estado = 6;
                            }
                            else if (i + 1 == cadena.Length || cadena.ElementAt(i + 1) == 44 || cadena.ElementAt(i + 1) == 41 || cadena.ElementAt(i + 1) == 32)
                            {
                                //aceptar
                                tokens.Add(new Token(cadconcat, 7, "numero", linea, columna));
                                tokensASintactico.Add(new Token(cadconcat, 7, "numero", linea, columna));
                                estado = 0;
                                cadconcat = "";
                            }
                            break;
                        }
                    case 18:
                        {
                            if (caracter==34)
                            {
                                cadconcat += caracteractual;
                                estado = 26;
                            }
                            else
                            {
                                cadconcat += caracteractual;
                                estado = 18;
                            }
                            break;
                        }
                    case 19:
                        {
                            if (caracter>48 && caracter<58)
                            {
                                cadconcat += caracteractual;
                                estado = 27;
                            }
                            else
                            {
                                //error
                                estado = 27;
                            }
                            break;
                        }
                    case 20:
                        {
                            if (caracter > 47 && caracter < 58)
                            {
                                cadconcat += caracteractual;
                                estado = 27;
                            }
                            else
                            {
                                //error
                                estado = 27;
                            }
                            break;
                        }
                    case 21:
                        {
                            if (caracter==48||caracter==49)
                            {
                                cadconcat += caracteractual;
                                estado = 27;
                            }
                            else
                            {
                                //error
                                estado = 27;
                            }
                            break;
                        }
                    case 22:
                        {
                            //aceptar
                            tokens.Add(new Token(cadconcat, 12, "comparador", linea, columna));
                            tokensASintactico.Add(new Token(cadconcat, 12, "comparador", linea, columna));
                            i--;
                            estado = 0;
                            cadconcat = "";
                            break;
                        }
                    case 23:
                        {
                            //aceptar
                            tokens.Add(new Token(cadconcat, 12, "comparador", linea, columna));
                            tokensASintactico.Add(new Token(cadconcat, 12, "comparador", linea, columna));
                            i--;
                            estado = 0;
                            cadconcat = "";
                            break;
                        }
                    case 24:
                        {
                            if (caracter==10||caracter==13)
                            {
                                //aceptar
                                tokens.Add(new Token(cadconcat, 13, "Comentario UL", linea, columna));
                                estado = 0;
                                cadconcat = "";
                                break;
                            }
                            else
                            {
                                cadconcat += caracteractual;
                            }
                            break;
                        }
                    case 25:
                        {
                            if (caracter==42)
                            {
                                cadconcat += caracteractual;
                                estado = 28;
                            }
                            else
                            {
                                cadconcat += caracteractual;
                            }
                            break;
                        }
                    case 26:
                        {
                            if (caracter == 32|| caracter == 10|| caracter == 13|| caracter == 41|| caracter == 44)
                            {
                                //aceptar
                                tokens.Add(new Token(cadconcat, 8, "Cadena", linea, columna));
                                tokensASintactico.Add(new Token(cadconcat, 8, "Cadena", linea, columna));
                                estado = 0;
                                cadconcat = "";
                            }
                            break;
                        }
                    case 27:
                        {
                            if (caracter==47)
                            {
                                cadconcat += caracteractual;
                                estado = 29;
                            }
                            else
                            {
                                //error
                                estado = 29;
                            }
                            break;
                        }
                    case 28:
                        {
                            if (caracter==47)
                            {
                                cadconcat += caracteractual;
                                estado = 30;
                            }
                            else
                            {
                                cadconcat += caracteractual;
                                estado = 25;
                            }
                            break;
                        }
                    case 29:
                        {
                            if (caracter==48)
                            {
                                cadconcat += caracteractual;
                                estado = 31;
                            }else if (caracter==49)
                            {
                                cadconcat += caracteractual;
                                estado = 32;
                            }
                            else
                            {
                                //error
                                estado = 31;
                            }
                            break;
                        }
                    case 30:
                        {
                            if (caracter == 32 || caracter == 10 || caracter == 13)
                            {
                                //aceptar
                                tokens.Add(new Token(cadconcat, 13, "Comentario ML", linea, columna));
                                estado = 0;
                                cadconcat = "";
                            }
                            break;
                        }
                    case 31:
                        {
                            if (caracter>48 && caracter<58)
                            {
                                cadconcat += caracteractual;
                                estado = 33;
                            }
                            else
                            {
                                //error
                                estado = 33;
                            }
                            break;
                        }
                    case 32:
                        {
                            if (caracter==48||caracter==49||caracter==50)
                            {
                                cadconcat += cadconcat;
                                estado = 33;
                            }
                            else
                            {
                                //error
                                estado = 33;
                            }
                            break;
                        }
                    case 33:
                        {
                            if (caracter == 47)
                            {
                                cadconcat += caracteractual;
                                estado = 34;
                            }
                            else
                            {
                                //error
                                estado = 34;
                            }
                            break;
                        }
                    case 34:
                        {
                            if (caracter > 47 && caracter < 58)
                            {
                                cadconcat += caracteractual;
                                estado = 35;
                            }
                            else
                            {
                                //error
                                estado = 35;
                            }
                            break;
                        }
                    case 35:
                        {
                            if (caracter > 47 && caracter < 58)
                            {
                                cadconcat += caracteractual;
                                estado = 35;
                            }else if (caracter == 39)
                            {
                                cadconcat += caracteractual;
                                estado = 36;
                            }
                            else
                            {
                                //error
                                estado = 36;
                            }
                            break;
                        }
                    case 36:
                        {
                            if (caracter == 32 || caracter == 10 || caracter == 13 || caracter == 41 || caracter == 44)
                            {
                                //aceptar
                                tokens.Add(new Token(cadconcat, 9, "fecha", linea, columna));
                                tokensASintactico.Add(new Token(cadconcat, 9, "fecha", linea, columna));
                                estado = 0;
                                cadconcat = "";
                            }
                            break;
                        }
                    case 38:
                        {
                            //aceptar
                            tokens.Add(new Token(cadconcat, 12, "comparador", linea, columna));
                            tokensASintactico.Add(new Token(cadconcat, 12, "comparador", linea, columna));
                            i--;
                            estado = 0;
                            cadconcat = "";
                            break;
                        }
                }
            }
        }
    }
}

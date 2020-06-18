using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OLC1Proyecto1_201503608
{
    public partial class Form1 : Form
    {
        
        string archivoact = "";
        List<Token> tokens = new List<Token>();
        List<Token> tokensAsintactico = new List<Token>();
        List<Errores> error = new List<Errores>();
        List<Tabla> tablas = new List<Tabla>();
        scanner_201503608 scan;
        parser_201503608 parser;
        RecorrerArbol recorrido;
        public Form1()
        {
            InitializeComponent();
            obtenerLineasColumnas();
        }
        public void obtenerLineasColumnas()
        {

            var a = richTextBox1;
            a.PreviewKeyDown += (ob, ev) =>
            {
                //-----Obtener Fila-------
                int inicio = a.SelectionStart;
                int linea = a.GetLineFromCharIndex(inicio);

                //-----Obtener Columna----
                int primero = a.GetFirstCharIndexFromLine(linea);
                int columna = inicio - primero;
                this.lblconteo.Text = "Columna: " + columna + " Linea: " + linea;
            };

        }
        public void GenerarHTMLTokens()
        {
            String texto = "";
            int i = 1;
            texto += "<html>";
            texto += "\n<head><title>Tokens</title></head>\n";
            texto += "<body bgcolor=\"aqua\"> \n <table border=\"2\" align=\"center\">\n";
            texto += "<tr><td>NO.</td><td>ID</td><td>TOKEN</td><td>LEXEMA</td><td>FILA</td><td>COLUMNA</td></tr>";
            /*for (int i = 0; i < this.tokens.Count(); i++)
            {
                texto += "<tr><td>";
                texto += this.tokens.ElementAt(i);
                texto += "</tr></td>";
            }*/
            foreach (Token token in this.tokens)
            {
                texto += "<tr><td>";
                texto += i;
                texto += "</td><td>";
                texto += token.id;
                texto += "</td><td>";
                texto +=  token.tokentipo;
                texto += "</td><td>";
                texto += token.lexemaval;
                texto += "</td><td>";
                texto += token.linea;
                texto += "</td><td>";
                texto += token.columna;
                texto += "</td></tr>";
                i++;
            }
            texto += "</table>\n</body>\n</html>";
            string fileName = "Tokens.html";
            StreamWriter writer = File.CreateText(fileName);
            writer.WriteLine(texto);
            writer.Close();
        }
        public void GenerarHTMLErrores()
        {
            String texto = "";
            int i = 1;
            texto += "<html>";
            texto += "\n<head><title>Errores</title></head>\n";
            texto += "<body bgcolor=\"aqua\"> \n <table border=\"2\" align=\"center\">\n";
            texto += "<tr><td>NO.</td><td>TIPO</td><td>ERROR</td><td>FILA</td><td>COLUMNA</td></tr>";
            /*for (int i = 0; i < this.error.Count(); i++)
            {
                texto += "<tr><td>";
                texto += this.error.ElementAt(i);
                texto += "</tr></td>";
            }*/
            foreach (Errores error in this.error)
            {
                texto += "<tr><td>";
                texto += i;
                texto += "</td><td>";
                texto += error.tipo;
                texto += "</td><td>";
                texto += error.error;
                texto += "</td><td>";
                texto += error.fila;
                texto += "</td><td>";
                texto += error.columna;
                texto += "</td></tr>";
                i++;
            }
            texto += "</table>\n</body>\n</html>";
            string fileName = "Errores.html";
            StreamWriter writer = File.CreateText(fileName);
            writer.WriteLine(texto);
            writer.Close();
        }
        private void AcercaDeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(" SQLE version 0.1.0 \n JUAN LUIS ROBLES MOLINA \n 201503608");
        }

        private void NuevoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "";
            richTextBox2.Text = "";
        }

        private void AbrirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog open = new OpenFileDialog();
                open.Filter = "Archivo SQLE (*.sqle)|*.sqle|Archivo JLRM (*.jlrm) |*.jlrm|All files (*.*)|*.*";
                open.Title = "Abrir Archivos";
                //  abrir.FileName = "Archivo TXT";
                var resultado = open.ShowDialog();//guarda resultado de clic en variable resultado
                if (resultado == DialogResult.OK)//si hace click en abrir
                {
                    StreamReader leer = new StreamReader(open.FileName);
                    RichTextBox caja = richTextBox1;

                    Font f = new Font("Arial", 10, FontStyle.Regular);
                    caja.Font = f;
                    caja.Text = leer.ReadToEnd();
                    archivoact = open.FileName;
                    leer.Close();
                    obtenerLineasColumnas();
                    //MessageBox.Show(archivoact);
                }

            }
            catch (Exception)
            {
                MessageBox.Show("No existe ninguna pestaña");
            }
        }

        private void GuardarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (File.Exists(archivoact))
                {
                    StreamWriter escribir = new StreamWriter(archivoact);
                    var caja = richTextBox1;
                    foreach (object line in caja.Lines)//crea objeto line 
                    {//para cada linea en caja
                        escribir.WriteLine(line);
                    }
                    escribir.Close();
                    MessageBox.Show("Guardado");
                }
                else
                {
                    SaveFileDialog guardar = new SaveFileDialog();
                    guardar.Filter = "Archivo SQLE (*.sqle)|*.sqle|Archivo JLRM (*.jlrm) |*.jlrm|All files (*.*)|*.*";
                    guardar.Title = "Guardar Archivo";
                    guardar.FileName = "";
                    var resultado = guardar.ShowDialog();
                    if (resultado == DialogResult.OK)
                    {
                        StreamWriter escribe = new StreamWriter(guardar.FileName);
                        var caja = richTextBox1;
                        foreach (object line in caja.Lines)
                        {
                            escribe.WriteLine(line);
                        }
                        escribe.Close();

                    }
                }


            }
            catch (Exception)
            {
                
            }
        }

        private void GuardarComoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                    SaveFileDialog guardar = new SaveFileDialog();
                    guardar.Filter = "Archivo SQLE (*.sqle)|*.sqle|Archivo JLRM (*.jlrm) |*.jlrm|All files (*.*)|*.*";
                    guardar.Title = "Guardar Como";
                    guardar.FileName = "";
                    var resultado = guardar.ShowDialog();
                    if (resultado == DialogResult.OK)
                    {
                        StreamWriter escribe = new StreamWriter(guardar.FileName);
                        var caja = richTextBox1;
                        foreach (object line in caja.Lines)
                        {
                            escribe.WriteLine(line);
                        }
                        escribe.Close();
                    }
                }
            catch (Exception err) { }
        }

        private void SalirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult respuesta;
            respuesta = MessageBox.Show("¿Está seguro que desea salir?","Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
            if (respuesta == DialogResult.Yes)
            {
                this.Close();
            }
            else
            {

            }
        }

        private void EjecutarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            scan = new scanner_201503608(richTextBox1.Text,tokens,tokensAsintactico,error);
            scan.AnalisisLexico(richTextBox1.Text);
            parser = new parser_201503608(tokensAsintactico, error);
            parser.INICIO();
            MessageBox.Show("Analisis finalizado");
            recorrido = new RecorrerArbol(this.tablas);
            NodoArbol raiz = parser.getPadre();
            recorrido.Ejecutar(raiz);
        }

        private void MostrarErroresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GenerarHTMLErrores();
        }

        private void VerTokensToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GenerarHTMLTokens();
        }

        private void MostrarArbolDeDerivacionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NodoArbol arbol =parser.getPadre();
            Graficar(recorrerarbol(arbol),"arbol");

        }

        private void Graficar(String cadena, String cad)
        {
            
            try
            {
                FileStream stream = new FileStream("Arbol.dot", FileMode.Create, FileAccess.Write);
                StreamWriter writer = new StreamWriter(stream);
                writer.WriteLine(
                    " digraph G {\n"
                    + "     rankdir=TB; "
                    + "" + " node[ shape=oval,  style=filled ,fillcolor=red, fontcolor=black, color=black];  \n"
                    + "edge[color=black] \n"
                    );
                writer.WriteLine(cadena);
                writer.WriteLine("\n}");
                writer.Close();
                //Ejecuta el codigo
                var command = string.Format("dot -Tsvg " + "Arbol.dot -o" + "Arbol.svg");
                var procStartInfo = new System.Diagnostics.ProcessStartInfo("cmd", "/C " + command);
                var proc = new System.Diagnostics.Process();
                proc.StartInfo = procStartInfo;
                proc.Start();
                proc.WaitForExit();
                Thread.Sleep(2000);
                Process.Start(@"" + "Arbol.svg");
            }
            catch (Exception x)
            {
                Console.WriteLine("Error inesperado cuando se intento graficar: " + x.ToString(), "error");
            }
        }

            public static String recorrerarbol(NodoArbol raiz)
        {
            String cuerpo = "";
            foreach (NodoArbol hijos in raiz.getHijos())
            {
                if (hijos!=null)
                {
                    if (hijos.getValor() != null)
                    {
                        cuerpo += "\"" + raiz.getIdNod() + "\"" + " [label=\"" + raiz.getEtiqueta() + "\"]";
                        cuerpo += "\"" + hijos.getIdNod() + "\"" + " [label=\"" + hijos.getValor() + "\"]";
                        cuerpo += "\"" + raiz.getIdNod() + "\" -> " + "\"" + hijos.getIdNod() + "\"";
                        cuerpo += recorrerarbol(hijos);
                    }
                }
            }
            return cuerpo;
        }
    }
}

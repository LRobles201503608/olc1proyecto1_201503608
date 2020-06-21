using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLC1Proyecto1_201503608
{
    class Tabla
    {
        public string nombre { get; set; }
        public List<Columna> campo { get; set; } 
        public Tabla(string nombre)
        {
            this.nombre = nombre;
            
            campo = new List<Columna>();
        }

    }
    /*
     * TABLA :  <NOMBRE> : 
     *                      0x00            0x01
     *                      [id; 1 valor:1][id:2,valor:1]
     *                      
     *                      AND -> DEPENDIENTE [] 
     *                      OR -> INDEDPENDIENTE 1,2
     *                      
     *                      id = 'a' OR a ='1' AND b='2'
     *                      OR
     *                      TABLA.Tupla - >1
     *                                                                          [1,2,3,4,5,6]
     *                                                                           0,1,2,3,4,5
     *                                                                          [1,3,4,5,6]
     *                                                                           0,1,2,3,4
     *                                              
     *                      foreach Campo campo in Lista
     *                      campo -> 0x00
     *                      campo -> 0x01
     *                          val =  validar(campo,ListOperaciones);
     *                          if val 
     *                              Lista.remove(campo);
     *                              
     *                      
     *                      
     *                      [OR [[][]]]
     *                      ------------------------------------------
     *                      validar(campo, List<Operaciones>)
     *                     
     *                      Boolean retorno = True;                                                         [idp = a][OR [[idp = b][ AND [[ipd = c][OR idp = d]]]         [or [][] -> [][] -> [] []            
     *                      
     *                      List<Operacion> ops
     *                      foreach Operacion op in ops
     *                          if more != Null: 
     *                              if more == enumerador.OR:
     *                                  if operacion.count > 1:
     *                                      van = validar()
     *                                      retorno = retorno || van
     *                                  else:
     *                                  van = method(Campo,op);
     *                                  if van != null
     *                                      retorno = retorno || van;
     *                         else:
     *                          if operacion.count > 1:
     *                                      van = validar()
     *                                      retorno =  van
     *                                  else:
     *                                  van = method(Campo,op);
     *                                  if van != null
     *                                      retorno =  van;
     *                                      
     *                                      
     *                      return retorno
     *                      
     *                      ---------------------------------------------
     *                      method(Campo campo, Operacion op) -> retorna Boolean
     *                  
     *                          campo.nombreCampo ==op.id
     *                              switch(op.comparation) -   = > < <= >= 
     *                                  > - int 
     *                                  //regla semantica
     *                                  if (campo is float || campo is int)
     *                                      if (op.namvevar is float || op.namevar is int)
     *                                          return int.parse(campo.toString()) > int.parse(campo.toString())
     */

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProyectoDatos
{
    //Clase para V07_LINQ1 "Inicializaciones"
    class Cliente
    {
        public int Id { get; set; }
        public string Nombre {get; set;}

        public Cliente() 
        {}
        
        public Cliente(int Id)
        {
            this.Id = Id;
        }
    }
}

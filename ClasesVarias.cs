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
        public string Nombre { get; set; }

        public Cliente()
        { }

        public Cliente(int Id)
        {
            this.Id = Id;
        }
    }

    //Clase para V10_LINQ4
    class InfoPedido
    {
        public int IdProducto { get; set; }
        public int Cantidad { get; set; }
        public decimal Precio { get; set; }

        //Al sobreescribir el método Equals ya funciona la comparación
        //Con OVERRIDE nos muestra todos los metodos que podemos sobreescribir
        public override bool Equals(object obj)
        {
            if (obj is InfoPedido)
            {
                InfoPedido o = obj as InfoPedido;

                //Solo vamos a comparar IdProducto y Cantidad
                if (this.IdProducto == o.IdProducto &&
                    this.Cantidad == o.Cantidad)
                    return true;

                ////Otro método
                //if (this.IdProducto.Equals(o.IdProducto) &&
                //    this.Cantidad.Equals(o.Cantidad))
                //return true;
            }

            return false;
        }

        //Es un número entero y no se duplica
        public override int GetHashCode()
        {
            //En este caso con el hascode de IdProducto sumado al hascode de Cantidad
            //y luego le pedimos el hascode del resultado dará un número único por IdProducto y Cantidad
            //que para comparar nos viene bien.
            return (IdProducto.GetHashCode() + Cantidad.GetHashCode()).GetHashCode();
        }
    }

    //Comparadores
    class ComparadorInfoPedido
        : System.Collections.Generic.IEqualityComparer<InfoPedido>
    {
        public bool Equals(InfoPedido x, InfoPedido y)
        {
            if (x.IdProducto == y.IdProducto &&
                x.Cantidad == y.Cantidad &&
                x.Precio == y.Precio)
                return true;

            return false;
        }

        public int GetHashCode(InfoPedido obj)
        {
            return obj.IdProducto.GetHashCode() + 
                   obj.Cantidad.GetHashCode() + 
                   obj.Precio.GetHashCode();
        }
    }
}

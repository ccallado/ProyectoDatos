using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

using System.Data.SqlClient;
using System.Data.Objects;

namespace ProyectoDatos
{
    class ClaseDatos
    {
        public static SqlConnection CreaConexionSql(string cadenaConexion)
        {
            return new SqlConnection(cadenaConexion);
        }

        public static SqlCommand CreaComandoSql(SqlConnection cnn)
        {
            //SqlCommand cmd = new SqlCommand();
            //cmd.Connection = cnn;
            //return cmd;

            //Más fácil
            return cnn.CreateCommand();
        }

        public static SqlCommand CreaComandoSql(string cadenaConexion)
        {
            SqlConnection cnn = CreaConexionSql(cadenaConexion);
            return cnn.CreateCommand();
        }

        public static ObjectQuery<Order> PedidosEntreFechas(ObjectContext contexto)
        { 
            string cad = "SELECT VALUE pedidos FROM " + contexto.DefaultContainerName +".Orders as pedidos " +
                         "WHERE pedidos.OrderDate >= @fchini AND pedidos.OrderDate <= @fchfin";
            ObjectQuery<Order> consulta = new ObjectQuery<Order>(cad, contexto);
            consulta.Parameters.Add(new ObjectParameter("fchini", typeof(DateTime)));
            consulta.Parameters.Add(new ObjectParameter("fchfin", typeof(DateTime)));
            return consulta;
        }
    }
}

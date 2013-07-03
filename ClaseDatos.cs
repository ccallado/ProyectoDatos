using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

using System.Data.SqlClient;

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
    }
}

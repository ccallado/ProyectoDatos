using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ProyectoDatos
{
    /// <summary>
    /// Lógica de interacción para V02_Conectado.xaml
    /// </summary>
    public partial class V02_Conectado : Window
    {
        System.Data.Common.DbConnection cnn;
        System.Data.Common.DbCommand cmd;

        enumTipoConexion tipo = enumTipoConexion.NoAsignado;

        public V02_Conectado()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (radioButton1.IsChecked.Value)
            {
                tipo = enumTipoConexion.Access;
                //System.Data.OleDb.OleDbConnection cnn;
                cnn = new System.Data.OleDb.OleDbConnection();
                cnn.ConnectionString = Properties.Settings.Default.ccAccessLocal;

                //System.Data.OleDb.OleDbCommand cmd;
                cmd = new System.Data.OleDb.OleDbCommand();
            }
            else
                if (radioButton2.IsChecked.Value)
                {
                    tipo = enumTipoConexion.Sql;
                    cnn = new System.Data.SqlClient.SqlConnection();
                    cnn.ConnectionString = Properties.Settings.Default.ccSqlEscritorio;

                    cmd = new System.Data.SqlClient.SqlCommand(); ;
                }
            if (tipo != enumTipoConexion.NoAsignado)
//            if (cnn != null)
            {
                cmd.Connection = cnn;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = "Select count(*) from Categories";

                cnn.Open();
                int cant = (int)cmd.ExecuteScalar();
                cnn.Close();
                MessageBox.Show("Cantidad: " + cant,  " Categorías " + tipo );
            }
            else
                MessageBox.Show("Seleccione tipo");
        }
    }
}

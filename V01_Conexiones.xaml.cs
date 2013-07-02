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
    /// Lógica de interacción para V01_Conexiones.xaml
    /// </summary>
    public partial class V01_Conexiones : Window
    {
        System.Data.OleDb.OleDbConnection cnnAccess;
        System.Data.SqlClient.SqlConnection cnnSql;

        public V01_Conexiones()
        {
            InitializeComponent();
            cnnAccess = new System.Data.OleDb.OleDbConnection();
            //Con el caracter @"\n" quita los caracteres de escape de la cadena
            //Puesto manualmente. No se podría cambiar
            //cnnAccess.ConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\Users\Formacion\\Desktop\SQL\Northwind.mdb";
            
            //tomo la conexion de un fichero de configuración
            //cnnAccess.ConnectionString = Properties.Settings.Default.ccAccessEscritorio;
            
            //conexión del directorio local Datos
            cnnAccess.ConnectionString = Properties.Settings.Default.ccAccessLocal ;

            //Conexión a SQL Express
            cnnSql = new System.Data.SqlClient.SqlConnection();
            cnnSql.ConnectionString = Properties.Settings.Default.ccSqlEscritorio;

            label1.Content = "Cerrada";
            label2.Content = "Cerrada";

            cnnAccess.StateChange += new System.Data.StateChangeEventHandler(cnnAccess_StateChange);
            cnnSql.StateChange += new System.Data.StateChangeEventHandler(cnnSql_StateChange);
        }

        void cnnSql_StateChange(object sender, System.Data.StateChangeEventArgs e)
        {
            label2.Content=(e.CurrentState == System.Data.ConnectionState.Open ? "Abierta" : "Cerrada");
        }

        void cnnAccess_StateChange(object sender, System.Data.StateChangeEventArgs e)
        {
            label1.Content = (e.CurrentState == System.Data.ConnectionState.Open ? "Abierta" : "Cerrada");
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            //solo se pueden usar los estados Open o Closed
            if (cnnAccess.State == System.Data.ConnectionState.Closed)
            {
                cnnAccess.Open();
                //MessageBox.Show("Abierta");
            }

        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            cnnAccess.Close();
            //MessageBox.Show("Cerrada");
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            //solo se pueden usar los estados Open o Closed
            if (cnnSql.State == System.Data.ConnectionState.Closed)
            {
                cnnSql.Open();
                //MessageBox.Show("Abierta");
            }

        }

        private void button4_Click(object sender, RoutedEventArgs e)
        {
            cnnSql.Close();
            //MessageBox.Show("Cerrada");
        }
    }
}

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

        public V01_Conexiones()
        {
            InitializeComponent();
            cnnAccess = new System.Data.OleDb.OleDbConnection();
            //Con el caracter @"\n" quita los caracteres de escape de la cadena
            cnnAccess.ConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\Users\Formacion\\Desktop\SQL\Northwind.mdb";
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            //solo se pueden usar los estados Open o Closed
            if (cnnAccess.State == System.Data.ConnectionState.Closed)
            {
                cnnAccess.Open();
                MessageBox.Show("Abierta");
            }

        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            cnnAccess.Close();
            MessageBox.Show("Cerrada");
        }
    }
}

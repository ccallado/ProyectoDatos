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

using System.Data.SqlClient;

namespace ProyectoDatos
{
    /// <summary>
    /// Lógica de interacción para V03_Conectado2.xaml
    /// </summary>
    public partial class V03_Conectado2 : Window
    {
        SqlCommand cmdProductos;

        public V03_Conectado2()
        {
            InitializeComponent();

            //Carga en el Combo1 del CategoryName
            SqlConnection cnn = 
                ClaseDatos.CreaConexionSql(
                    Properties.Settings.Default.ccSqlEscritorio);
            SqlCommand cmd1 = ClaseDatos.CreaComandoSql(cnn);
            cmd1.CommandText = "SELECT CategoryName, CategoryID, Description FROM Categories";
            cnn.Open();
            SqlDataReader dr1 = cmd1.ExecuteReader();
            //Asigno a la propiedad ItemsSource el objeto que le va a suministrar los datos
            comboBox1.ItemsSource = dr1;
            //Le decimos que campo queremos sacar en el combo
            comboBox1.DisplayMemberPath = "CategoryName";
            comboBox1.SelectedValuePath = "CategoryID";
            comboBox1.SelectedIndex = 1;

            //No puedo cerrar la conexión porque no lee la información en el
            //combo hasta que no se despliegue. Y si hemos cerrado la conexión
            //aquí da error en esa situación.
            //cnn.Close();

            //Cargamos en el Combo2 TODO....
            SqlCommand cmd2 = 
                ClaseDatos.CreaComandoSql(Properties.Settings.Default.ccSqlEscritorio);
            cmd2.CommandText = "SELECT * FROM Categories";
            cmd2.Connection.Open();
            SqlDataReader dr2 = cmd2.ExecuteReader();
            //Asigno a la propiedad ItemsSource el objeto que le va a suministrar los datos
            comboBox2.ItemsSource = dr2;
            //Asigno campo la tomar valor
            comboBox2.SelectedValuePath = "CategoryID";

            //Comando para rellenar la grid
            cmdProductos =
                ClaseDatos.CreaComandoSql(Properties.Settings.Default.ccSqlEscritorio);
            cmdProductos.CommandText = "SELECT * FROM Products WHERE CategoryID = @Cat";
            cmdProductos.Parameters.Add(new SqlParameter("@Cat", System.Data.SqlDbType.Int));

        }

        private void comboBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            System.Data.Common.DbDataRecord r =
                (System.Data.Common.DbDataRecord )comboBox1.SelectedItem;
            MessageBox.Show("Elemento: " + r["CategoryName"].ToString() + 
                " Valor: " + comboBox1.SelectedValue.ToString() + 
                "\nDescripcion: " + r["Description"].ToString());
        }

        private void comboBox2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cmdProductos.Parameters["@Cat"].Value = comboBox2.SelectedValue;
            cmdProductos.Connection.Close();
            cmdProductos.Connection.Open();
            dataGrid1.ItemsSource = cmdProductos.ExecuteReader();
            dataGrid2.ItemsSource = dataGrid1.ItemsSource;
        }
    }
}

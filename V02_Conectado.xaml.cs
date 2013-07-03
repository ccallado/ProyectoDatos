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

using System.Data.OleDb;
using System.Data.SqlClient;

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

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            string cad = "Select UnitsInStock from Products WHERE ProductID = ";

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
                cmd.CommandText = cad;
                cmd.CommandText += textBox1.Text;

                cnn.Open();
                object oAux = cmd.ExecuteScalar();
                if (oAux != null)
                {
                    short stock = (short)cmd.ExecuteScalar();
                    cnn.Close();
                    MessageBox.Show("Stock: " + stock, " Productos " + tipo);
                }
                else
                {
                    cnn.Close();
                    MessageBox.Show("Producto no encontrado", " Productos " + tipo);
                }
            }
            else
                MessageBox.Show("Seleccione tipo");
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            string cad = "Select UnitsInStock from Products WHERE ProductID = ";

            if (radioButton1.IsChecked.Value)
            {
                tipo = enumTipoConexion.Access;
                //System.Data.OleDb.OleDbConnection cnn;
                cnn = new System.Data.OleDb.OleDbConnection();
                cnn.ConnectionString = Properties.Settings.Default.ccAccessLocal;

                //System.Data.OleDb.OleDbCommand cmd;
                cmd = new System.Data.OleDb.OleDbCommand();
                cad += "?";
                //Creo e instancio con valor y luego añado
                //OleDbParameter param = new OleDbParameter("Id", textBox2.Text);
                //cmd.Parameters.Add(param );
                //Añado directamente la instanciacion
                //cmd.Parameters.Add(new OleDbParameter("Id", textBox2.Text));
                //Creo e instancio con tipo y luego lo añado
                //OleDbParameter param = new OleDbParameter("Id", OleDbType.SmallInt);
                //cmd.Parameters.Add(param);
                //param.Value = textBox2.Text;
                //Añadir con AddWithValue
                ((OleDbCommand )cmd).Parameters.AddWithValue("Id", textBox2.Text);
            }
            else
                if (radioButton2.IsChecked.Value)
                {
                    tipo = enumTipoConexion.Sql;
                    cnn = new System.Data.SqlClient.SqlConnection();
                    cnn.ConnectionString = Properties.Settings.Default.ccSqlEscritorio;

                    cmd = new System.Data.SqlClient.SqlCommand(); 
                    cad += "@Id";
                    //Creo e instancio con valor y luego lo añado
                    //SqlParameter param = new SqlParameter("@Id", textBox2.Text);
                    //cmd.Parameters.Add(param);
                    //Añado directamente la instanciación
                    //cmd.Parameters.Add(new SqlParameter("@Id", textBox2.Text));
                    //Creo e instancio con valor y luego lo añado
                    //SqlParameter param = new SqlParameter("@Id", System.Data.SqlDbType.SmallInt);
                    //cmd.Parameters.Add(param);
                    //param.Value = textBox2.Text;
                    //Añadir con AddWithValue
                    ((SqlCommand)cmd).Parameters.AddWithValue("@Id", textBox2.Text);
                }
            if (tipo != enumTipoConexion.NoAsignado)
            //            if (cnn != null)
            {
                cmd.Connection = cnn;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = cad;

                cnn.Open();
                object oAux = cmd.ExecuteScalar();
                if (oAux != null)
                {
                    short stock = (short)oAux ;
                    cnn.Close();
                    MessageBox.Show("Stock: " + stock, " Productos " + tipo);
                }
                else
                {
                    cnn.Close();
                    MessageBox.Show("Producto no encontrado", " Productos " + tipo);
                }
            }
            else
                MessageBox.Show("Seleccione tipo");
        }

        private void button4_Click(object sender, RoutedEventArgs e)
        {
            string paramId, paramStock;

            if (radioButton1.IsChecked.Value)
            {
                tipo = enumTipoConexion.Access;
                cnn = new System.Data.OleDb.OleDbConnection();
                cnn.ConnectionString = Properties.Settings.Default.ccAccessLocal;

                cmd = new System.Data.OleDb.OleDbCommand();
                //Añadir con AddWithValue
                ((OleDbCommand)cmd).Parameters.AddWithValue("Stock", textBox4.Text);
                ((OleDbCommand)cmd).Parameters.AddWithValue("Id", textBox3.Text);
            }
            else
                if (radioButton2.IsChecked.Value)
                {
                    tipo = enumTipoConexion.Sql;
                    cnn = new System.Data.SqlClient.SqlConnection();
                    cnn.ConnectionString = Properties.Settings.Default.ccSqlEscritorio;

                    cmd = new System.Data.SqlClient.SqlCommand();
                    //Añadir con AddWithValue
                    ((SqlCommand)cmd).Parameters.AddWithValue("@Stock", textBox4.Text);
                    ((SqlCommand)cmd).Parameters.AddWithValue("@Id", textBox3.Text);
                }
            if (tipo != enumTipoConexion.NoAsignado)
            //            if (cnn != null)
            {
                string cad = "Update Products Set UnitsInStock = " + cmd.Parameters[0].ParameterName  +
                    " WHERE ProductID = " + cmd.Parameters[1].ParameterName;

                cmd.Connection = cnn;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = cad;

                cnn.Open();
                int cant = cmd.ExecuteNonQuery();
                if (cant != 0)
                {
                    //short stock = (short)cmd.ExecuteScalar();
                    cnn.Close();
                    MessageBox.Show("Stock actualizado ", " Productos " + tipo);
                }
                else
                {
                    cnn.Close();
                    MessageBox.Show("Producto no encontrado", " Productos " + tipo);
                }
            }
            else
                MessageBox.Show("Seleccione tipo");
        }

        private void button5_Click(object sender, RoutedEventArgs e)
        {
            string cad = "Select * from Products WHERE CategoryID = ";

            if (radioButton1.IsChecked.Value)
            {
                tipo = enumTipoConexion.Access;
                //System.Data.OleDb.OleDbConnection cnn;
                cnn = new System.Data.OleDb.OleDbConnection();
                cnn.ConnectionString = Properties.Settings.Default.ccAccessLocal;

                //System.Data.OleDb.OleDbCommand cmd;
                cmd = new System.Data.OleDb.OleDbCommand();
                cad += "?";
                //Añadir con AddWithValue
                ((OleDbCommand)cmd).Parameters.AddWithValue("IdCat", textBox5.Text);
            }
            else
                if (radioButton2.IsChecked.Value)
                {
                    tipo = enumTipoConexion.Sql;
                    cnn = new System.Data.SqlClient.SqlConnection();
                    cnn.ConnectionString = Properties.Settings.Default.ccSqlEscritorio;

                    cmd = new System.Data.SqlClient.SqlCommand();
                    cad += "@IdCat";
                    //Añadir con AddWithValue
                    ((SqlCommand)cmd).Parameters.AddWithValue("@IdCat", textBox5.Text);
                }
            if (tipo != enumTipoConexion.NoAsignado)
            //            if (cnn != null)
            {
                cmd.Connection = cnn;
                //cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = cad;

                
                cnn.Open();
                System.Data.Common.DbDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows )
                {
                    //Leer datos....
                    string cad2 = "ProductID\tProductName\tUnitPrice\tUnitsInStock";

                    while (dr.Read())
                    {
                        //Busca la posición del campo, busca el tipo de dato, Saca el valor (más cómodo)
                        //cad2 += "\n" + dr["ProductID"].ToString();
                        //Busca el tipo de dato, Saca el valor
                        cad2 += "\n" + dr[0].ToString();
                        //Saca el valor (más rápido)
                        //cad2 += "\t" + dr.GetInt32(0).ToString();
                        cad2 += "\t" + dr[1].ToString();
                        cad2 += "\t" + dr[5].ToString();
                        cad2 += "\t" + dr[6].ToString();

                    }
                    MessageBox.Show(cad2, "Productos de la categoría " + textBox5.Text );
                }
                else
                {
                    cnn.Close();
                    MessageBox.Show("No hay Productos de esta categoría.", " Productos " + tipo);
                }
            }
            else
                MessageBox.Show("Seleccione tipo");
        }
    }
}

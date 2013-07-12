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
    /// Lógica de interacción para V14_Entity3.xaml
    /// </summary>
    public partial class V14_Entity3 : Window
    {
        System.Data.EntityClient.EntityConnection cnn;

        public V14_Entity3()
        {
            InitializeComponent();

            cnn = new System.Data.EntityClient.EntityConnection();
            cnn.ConnectionString = "name=northwindEntities";
        }

        //EntityClient  (Como ADO conectado
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            //using (System.Data.EntityClient.EntityConnection cnn =
            //        new System.Data.EntityClient.EntityConnection())
            //{ 
                //No esta en las propiedades conexiones
                //Mirar en app.config

                //cnn.ConnectionString = "name=northwindEntities";
                if (cnn.State== System.Data.ConnectionState.Closed)
                    cnn.Open();
                
                cnn.Close();
                MessageBox.Show("Abierta y cerrada");
            //}
        }

        //Info Pedidos
        //En la caja de texto irá en código de un cliente
        private void button2_Click(object sender, RoutedEventArgs e)
        {
            //System.Data.EntityClient.EntityCommand cmd =
            //    new System.Data.EntityClient.EntityCommand();
            //cmd.Connection = cnn;

            System.Data.EntityClient.EntityCommand cmd = cnn.CreateCommand();

            string cadcmd = "SELECT p.OrderId, p.OrderDate, " +
                         "p.Employee.FirstName + \" \" + p.Employee.LastName as nombre, " +
                         "p.Shipper.CompanyName " +
                         "FROM northwindEntities.Orders as p " +
                         "WHERE p.CustomerID = @Cli";
            
            cmd.CommandText = cadcmd;
            cmd.Parameters.AddWithValue("Cli", textBox1.Text);

            cnn.Open();
            //Si no se pone comportamiento da error
            //
            var datos = cmd.ExecuteReader(System.Data.CommandBehavior.SequentialAccess );

            string cad="";

            if (datos.HasRows)
            {
                while (datos.Read())
                {
                    cad += datos[0] + " - " +
                          Convert.ToDateTime(datos[1]).ToShortDateString() + " Empleado: ";
                    cad += datos[2] + " (Transportista: ";
                    cad += datos[3] + ")\n";
                }
            }
            else
                cad = "Sin datos...";

            cnn.Close();
            MessageBox.Show(cad);
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            System.Data.EntityClient.EntityCommand cmd = cnn.CreateCommand();

            string cadcmd = "SELECT p.OrderId, p.OrderDate, " +
                         "p.Employee.FirstName + \" \" + p.Employee.LastName as nombre, " +
                         "p.Shipper.CompanyName, " +
                         //Así no funciona
                         //"COUNT(p.Order_Details) " +
                         "COUNT(SELECT VALUE det.OrderID FROM p.Order_Details as det), " +
                         //Para que no de problemas la multiplicación hacemos un CAST a tipo single
                         "SUM(SELECT VALUE CAST (det.UnitPrice AS Edm.Single) * det.Quantity * ( 1.0 - det.Discount ) FROM p.Order_Details as det) " +
                         "FROM northwindEntities.Orders as p " +
                         "WHERE p.CustomerID = @Cli";

            cmd.CommandText = cadcmd;
            cmd.Parameters.AddWithValue("Cli", textBox1.Text);

            cnn.Open();
            //Si no se pone comportamiento da error
            //
            var datos = cmd.ExecuteReader(System.Data.CommandBehavior.SequentialAccess);

            string cad = "";

            if (datos.HasRows)
            {
                while (datos.Read())
                {
                    cad += datos[0] + " - " +
                          Convert.ToDateTime(datos[1]).ToShortDateString() + " Empleado: ";
                    cad += datos[2] + " (Transportista: ";
                    cad += datos[3] + ")\n";
                    cad += "\tLíneas de detalle: " + datos[4] + 
                           " Total... " + Convert.ToSingle(datos[5]).ToString("C") + "\n";
                }
            }
            else
                cad = "Sin datos...";

            cnn.Close();
            MessageBox.Show(cad);
        }
    }
}

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
    /// Lógica de interacción para V13_Entity2.xaml
    /// </summary>
    public partial class V13_Entity2 : Window
    {
        public V13_Entity2()
        {
            InitializeComponent();

            //Fecha del pedido 1
            using (northwindEntities ne = new northwindEntities())
            {
                datePicker1.SelectedDate = ne.Orders.First().OrderDate;
                datePicker2.SelectedDate = datePicker1.SelectedDate.Value.AddMonths(1);
            }
        }

        //Entity SQL
        //Vamos a dar un string en las consultas
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            string cadena = "";

            //Esto se define de nuestro modelo no de la base de datos
            //No puedo utilizar "*" se utiliza "VALUE"
            
            //Es lo mismo que pone cno SELECT VALUE
            //cadena = "northwindEntities.Categorias";

            cadena = "SELECT VALUE ListaCategorias " +
                     "FROM northwindEntities.Categorias AS ListaCategorias";

            using (northwindEntities ne = new northwindEntities())
            {
                System.Data.Objects.ObjectQuery<Categoria> cats;
                //La creo sin mergeOption y en la ejecución le indico como quiero ejecutarla
                //y le pongo el mergeOption en el Execute
                cats = new System.Data.Objects.ObjectQuery<Categoria>(cadena, ne,
                                                System.Data.Objects.MergeOption.NoTracking);

                comboBox1.ItemsSource = cats;
                comboBox1.DisplayMemberPath = "CategoryName";
                comboBox1.SelectedValuePath = "CategoryID";

            }
        }

        private void comboBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Cuando no hay ninguno seleccionado vale -1
            if (comboBox1.SelectedIndex != -1)
            {
                Categoria cat = comboBox1.SelectedItem as Categoria;
                if (cat != null)
                    MessageBox.Show(cat.Description);
            }
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            string cadena = "";

            //Esto se define de nuestro modelo no de la base de datos
            //No puedo utilizar "*" se utiliza "VALUE"
            cadena  = "SELECT ListaCategorias.CategoryName, ListaCategorias.CategoryID";
            if (checkBox1.IsChecked.Value)
                cadena += ", ListaCategorias.Description";
            cadena += " FROM northwindEntities.Categorias AS ListaCategorias";

            using (northwindEntities ne = new northwindEntities())
            {
                //Ahora no es el registro completo
                System.Data.Objects.ObjectQuery<System.Data.Common.DbDataRecord > cats;
                //La creo sin mergeOption y en la ejecución le indico como quiero ejecutarla
                //y le pongo el mergeOption en el Execute
                cats = new System.Data.Objects.ObjectQuery<System.Data.Common.DbDataRecord>(cadena, ne,
                                                System.Data.Objects.MergeOption.NoTracking);

                comboBox2.ItemsSource = cats;
                comboBox2.DisplayMemberPath = "CategoryName";
                comboBox2.SelectedValuePath = "CategoryID";

            }
        }

        private void comboBox2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Cuando no hay ninguno seleccionado vale -1
            if (comboBox2.SelectedIndex != -1)
            {
                string desc = "";
                System.Data.Common.DbDataRecord cat;
                cat = comboBox2.SelectedItem as System.Data.Common.DbDataRecord;
                if (cat != null)
                {
                    if (cat.FieldCount == 3)
                        desc = cat[2].ToString();
                    else
                        //Tengo un anómimo
                        using (northwindEntities ne = new northwindEntities())
                        {
                            int id = (int)cat["CategoryID"];
                            desc = ne.Categorias
                                         .Where(c => c.CategoryID == id)
                        //Como es una colección debemos seleccionar uno, con single, last, first, etc...
                                         .Select(c => c.Description).SingleOrDefault();
                        }
                        MessageBox.Show(desc);
                }
            }
        }

        //Parámetros
        private void button3_Click(object sender, RoutedEventArgs e)
        {
            string cadena = "SELECT VALUE productos FROM northwindEntities.Productos as productos " +
                            "WHERE productos.CategoryID = @cat";
            using (northwindEntities ne = new northwindEntities())
            {
                System.Data.Objects.ObjectQuery<Producto> consulta;
                consulta = new System.Data.Objects.ObjectQuery<Producto>(cadena, ne, System.Data.Objects.MergeOption.NoTracking);
                //Añadir el parámetro
                consulta.Parameters.Add(
                    //Caja de texto1
                    new System.Data.Objects.ObjectParameter("cat", int.Parse(textBox1.Text))
                    );

                //Compruebo el resultado
                string cad = "";
                foreach (var x in consulta)
                {
                    cad += x.ProductID + " - " +
                           x.ProductName + " (" +
                           x.Categoria.CategoryName + ")\n";
                }

                MessageBox.Show(cad, "Productos de la categoría " + consulta.Parameters["cat"].Value);
            }

        }

        //Pedidos entre fechas
        private void button4_Click(object sender, RoutedEventArgs e)
        {
            using (northwindEntities ne = new northwindEntities())
            {
                var consulta = ClaseDatos.PedidosEntreFechas(ne);
                consulta.Parameters["fchini"].Value = datePicker1.SelectedDate.Value;
                consulta.Parameters["fchfin"].Value = datePicker2.SelectedDate.Value;

                string cad = "";

                foreach (var x in consulta)
                {
                    cad += x.OrderID + " - " +
                           x.CustomerID + " - " +
                           x.OrderDate.Value.ToShortDateString() + "\n";
                }
                MessageBox.Show(cad);
            }
        }

        //Info Pedidos entre fechas (CON carga diferida)
        private void button5_Click(object sender, RoutedEventArgs e)
        {
            using (northwindEntities ne = new northwindEntities())
            {
                var consulta = ClaseDatos.PedidosEntreFechas(ne);
                consulta.Parameters["fchini"].Value = datePicker1.SelectedDate.Value;
                consulta.Parameters["fchfin"].Value = datePicker2.SelectedDate.Value;

                string cad = "";

                foreach (var x in consulta)
                {
                    cad += 
                        //aprovechando las propiedades de navegación
                        //Esto carga mucho la máquina. Tener cuidado al usarlo.
                           x.OrderDate.Value.ToShortDateString()+ "  " +
                           x.OrderID + " - " + 
                           x.Customer.CompanyName + " (" +
                           x.Employee.FirstName + " " +
                           x.Employee.LastName + ") Lineas de detalle: " +
                           x.Order_Details.Count() + "\n";
                }
                MessageBox.Show(cad);
            }
        }

        //Info Pedidos entre fechas (SIN carga diferida)
        private void button6_Click(object sender, RoutedEventArgs e)
        {
            using (northwindEntities ne = new northwindEntities())
            {
                //SIN carga diferida
                ne.ContextOptions.LazyLoadingEnabled = false;

                var empleados = ne.Employees.Where(emp => emp.FirstName.StartsWith("M")).ToList();

                var consulta = ClaseDatos.PedidosEntreFechas(ne);
                consulta.Parameters["fchini"].Value = datePicker1.SelectedDate.Value;
                consulta.Parameters["fchfin"].Value = datePicker2.SelectedDate.Value;

                string cad = "";

                foreach (var x in consulta)
                {
                    cad +=
                        //aprovechando las propiedades de navegación
                        //Esto carga mucho la máquina. Tener cuidado al usarlo.
                           x.OrderDate.Value.ToShortDateString() + "  " +
                           x.OrderID + " - " +
                           //x.Customer.CompanyName + " (" +
                           (x.Employee != null ?
                           x.Employee.FirstName + " " +
                           x.Employee.LastName : 
                           //") Lineas de detalle: " 
                           "") +
                           //x.Order_Details.Count() + 
                           "\n";
                }
                MessageBox.Show(cad);
            }
        }

        //Info Pedidos entre fechas (CON Includes)
        private void button7_Click(object sender, RoutedEventArgs e)
        {
            using (northwindEntities ne = new northwindEntities())
            {
                //SIN carga diferida
                ne.ContextOptions.LazyLoadingEnabled = false;

                //Forzamos las propiedades de navegación con Include
                var consulta = ClaseDatos.PedidosEntreFechas(ne)
                             .Include("Customer")
                             .Include("Employee")
                             .Include("Order_Details");

                consulta.Parameters["fchini"].Value = datePicker1.SelectedDate.Value;
                consulta.Parameters["fchfin"].Value = datePicker2.SelectedDate.Value;

                string cad = "";

                foreach (var x in consulta)
                {
                    cad +=
                        //aprovechando las propiedades de navegación
                        //Esto carga mucho la máquina. Tener cuidado al usarlo.
                           x.OrderDate.Value.ToShortDateString() + "  " +
                           x.OrderID + " - " +
                           x.Customer.CompanyName + " (" +
                           x.Employee.FirstName + " " +
                           x.Employee.LastName + ") Lineas de detalle: " +
                           x.Order_Details.Count() + "\n";
                }
                MessageBox.Show(cad);
            }
        }

        //Con LoadProperty
        private void button8_Click(object sender, RoutedEventArgs e)
        {
            using (northwindEntities ne = new northwindEntities())
            {
                var consulta = ClaseDatos.PedidosEntreFechas(ne);

                //SIN carga diferida
                ne.ContextOptions.LazyLoadingEnabled = false;


                consulta.Parameters["fchini"].Value = datePicker1.SelectedDate.Value;
                consulta.Parameters["fchfin"].Value = datePicker2.SelectedDate.Value;

                string cad = "";

                foreach (var x in consulta)
                {
                    //Forzamos las propiedades de navegación con Include
                    if (x.ShipCountry.StartsWith("F") ||
                        x.ShipCountry.StartsWith("S"))
                    {
                        ne.LoadProperty(x, "Customer");
                        ne.LoadProperty(x, "Employee");
                        ne.LoadProperty(x, "Order_Details");
                    }

                    cad +=
                        //aprovechando las propiedades de navegación
                        //Esto carga mucho la máquina. Tener cuidado al usarlo.
                           x.OrderDate.Value.ToShortDateString() + "  " +
                           x.OrderID + " - " +
                           (x.Customer != null ? x.Customer.CompanyName : "Sin nombre")
                           + " (" +
                           (x.Employee != null ? x.Employee.FirstName + " " + x.Employee.LastName : "Sin empleado") 
                           + ") Lineas de detalle: " +
                           //x.ShipCountry + " - " +
                           (x.Order_Details.Count() != 0 ? x.Order_Details.Count().ToString() : "Sin detalle") 
                           + "\n";
                }
                MessageBox.Show(cad);
            }
        }
    }
}

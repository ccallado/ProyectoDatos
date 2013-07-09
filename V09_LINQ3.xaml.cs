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
    /// Lógica de interacción para V09_LINQ3.xaml
    /// </summary>
    public partial class V09_LINQ3 : Window
    {
        NWDataSet ds;
        string cad;

        public V09_LINQ3()
        {
            InitializeComponent();

            ds = NWDataSet.CargaDatos();

            comboBox1.ItemsSource = ds.Products;
            comboBox1.DisplayMemberPath = "ProductName";
            comboBox1.SelectedValuePath = "ProductID";
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            //Con 
            //var Prods = from p in ds.Products
            //            orderby p.CategoryID 
            //            group p by p.CategoryID;

            //Con métodos de extensión
            //var Prods = ds.Products
            //            .OrderBy(p => p.CategoryID)
            //            //Con un solo campo
            //            //.GroupBy(p => p.CategoryID);
            //            //Con más de un campo
            //            .GroupBy(p => {p.CategoryID), p.SupplierID };

            //Algo más optimizado cuando hay muchos registros
            //Se hace into en una variable que me reduce la agrupación
            //Y luego hago un order sobre la variable que tiene menos registros
            //var Prods = from p in ds.Products
            //            group p by p.CategoryID
            //            into productos
            //            orderby productos.Key
            //            select productos;

            //Con métodos de extensión Optimizado para muchos registros
            var Prods = ds.Products
                .OrderBy(p => p.CategoryID)
                .GroupBy(p => p.CategoryID)
                .OrderBy(p => p.Key);

            cad = "";
            foreach (var pAux in Prods)
            {
                cad += "Categoría: " + pAux.Key + "\n";
                foreach (var p in pAux)
                {
                    cad += "  " + p.ProductName + "\n";
                }
            }

            MessageBox.Show(cad);
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            //var datos = from c in ds.Categories
            //            from p in ds.Products
            //            where c.CategoryID==p.CategoryID 
            //            //Así proyecto objetos y no los puedo asignar a un GRID
            //            //select new { c, p };
            //            select new { c.CategoryID, c.CategoryName, p.ProductID, p.ProductName };

            var datos = from c in ds.Categories
                        where c.CategoryID < 3
                        from p in ds.Products
                        where p.ProductName.StartsWith("C")
                        where c.CategoryID == p.CategoryID
                        //Así proyecto objetos y no los puedo asignar a un GRID
                        //select new { c, p };
                        select new { c.CategoryID, c.CategoryName, p.ProductID, p.ProductName };

            cad = "";
            foreach (var x in datos)
            {
                cad += x.CategoryName + "\t" + x.ProductName + "\n";
            }

            MessageBox.Show(cad, "Registros: " + datos.Count());
        }

        //Inner Join
        private void button3_Click(object sender, RoutedEventArgs e)
        {
            ////Medoto normal
            //var datos = from c in ds.Categories
            //            where c.CategoryID < 3
            //            join p in ds.Products
            //            on c.CategoryID equals p.CategoryID
            //            //where p.ProductName.StartsWith("C")
            //            select new { c.CategoryID, c.CategoryName, p.ProductID, p.ProductName };

            //Con metodos de extensión
            var datos = ds.Categories
                        .Where(cat => cat.CategoryID < 3)
                        .Join(ds.Products.Where(p => p.ProductName.StartsWith("C")),
                              (c => c.CategoryID),
                              p => p.CategoryID,
                              (c, p) => new { c.CategoryID, c.CategoryName, p.ProductID, p.ProductName }
                              );

            cad = "";
            foreach (var x in datos)
            {
                cad += x.CategoryName + "\t" + x.ProductName + "\n";
            }

            MessageBox.Show(cad, "Registros: " + datos.Count());
        }

        //Outter Join
        private void button4_Click(object sender, RoutedEventArgs e)
        {
            var datos = from c in ds.Categories
                        where c.CategoryID < 3
                        join p in ds.Products
                        on c.CategoryID equals p.CategoryID
                        into aTemp
                        from a3 in aTemp.DefaultIfEmpty()
                        where a3 == null
                        select new
                        {
                            c.CategoryID,
                            c.CategoryName,
                            ProductID = (a3 != null ? a3.ProductID.ToString() : ""),
                            ProductName = (a3 != null ? a3.ProductName : "")
                        };

            cad = "";
            foreach (var x in datos)
            {
                cad += x.CategoryName + "\t" + x.ProductName + "\n";
            }

            MessageBox.Show(cad, "Registros: " + datos.Count());
        }

        //Outer Join 2
        private void button5_Click(object sender, RoutedEventArgs e)
        {
            //Consulta previa para filtrar los datos del pedido de la caja de texto
            //var PedidoCaja = from det in ds.Order_Details
            //                 where det.OrderID == int.Parse(textBox1.Text)
            //                 select det;

            var datos = from p in ds.Products
                        //Sin filtro en número de pedido
                        //join det in ds.Order_Details 

                        //Consulta previa con las lineas de pedido que me interesa
                        //join det in PedidoCaja

                        //Con filtro en número de pedido con una Lambda
                        //Si fuese un entityFramework no admitiría esta Lambda
                        join det in ds.Order_Details.Where(detalle => detalle.OrderID == int.Parse(textBox1.Text))
                        on p.ProductID equals det.ProductID
                        into aTemp
                        from a3 in aTemp.DefaultIfEmpty()
                        //Para ver solo los no pertenecientes al pedido
                        //where a3 == null 
                        select new
                        {
                            //Pedido =  a3.OrderID,
                            Id = p.ProductID,
                            Nombre = p.ProductName,
                            Venta = (a3 != null ? a3.Quantity.ToString() : "No vendido")
                        };

            cad = "";
            foreach (var x in datos)
            {
                cad += x.Id + " - " +
                       x.Nombre + " (Unidades: " + x.Venta + ")\n";
            }

            MessageBox.Show(cad, "Pedido: " + textBox1.Text);

        }

        //Algunos métodos de extensión Count, Sum, Min, Max, Average
        private void button6_Click(object sender, RoutedEventArgs e)
        {
            //Filtro las líneas de detalle de los pedidos en donde está este producto
            var info = ds.Order_Details.Where(det => det.ProductID == int.Parse(textBox2.Text));

            string cad = "";

            cad += "Pedidos: " + info.Count();
            cad += "\nUnidades vendidas: " + info.Sum(det => det.Quantity);
            cad += "\nPrecio mínimo: " + info.Min(det => det.UnitPrice).ToString("C");
            cad += "\nPrecio máximo: " + info.Max(det => det.UnitPrice).ToString("C");
            cad += "\nPrecio medio: " + info.Average(det => det.UnitPrice).ToString("C");

            MessageBox.Show(cad, "Producto: " + textBox2.Text);

            //Todo en la consulta
            //Filtro las líneas de detalle de los pedidos en donde está este producto
            var info2 = ds.Order_Details
                            .Where(det => det.ProductID == int.Parse(textBox2.Text))
                            .Select(det => det.Quantity);
            //Es una columna que tiene un dato IEnumerable de tipo short
            cad = "Unidades vendidas: " + info2.Sum(det => det);

            var info3 = ds.Order_Details
                            .Count(det => det.ProductID == int.Parse(textBox2.Text));

            //Un entero
            cad += "\nCantidad: " + info3;
            MessageBox.Show(cad);
        }

        //Any / All (Con expresión)
        private void button7_Click(object sender, RoutedEventArgs e)
        {
            var info = from det in ds.Order_Details
                       where det.OrderID == int.Parse(textBox3.Text)
                       select det;

            string cad = "";
            foreach (var d in info)
            {
                //No tenemos el nombre del producto
                //Gracias a las relaciones tenemos el puntero al registro de una tabla de productos
                cad += d.ProductID + " - " +
                       d.ProductsRow.ProductName + "\t" +
                       d.ProductsRow.UnitsInStock + "\n";
            }

            cad += "\nAlguno sin stock (ANY): " +
                (info.Any(d => d.ProductsRow.UnitsInStock == 0) ? "Sí" : "No");

            cad += "\nTodos con stock (ALL): " +
                (info.All(d => d.ProductsRow.UnitsInStock != 0) ? "Sí" : "No");
            MessageBox.Show(cad);
        }

        //First / Last / ElementAt
        private void button8_Click(object sender, RoutedEventArgs e)
        {
            //Sin control de errores
            //var primero = ds.Order_Details
            //                .First(d => d.ProductID == int.Parse(textBox4.Text));

            //Con control de errores
            //var primero = ds.Order_Details
            //                .FirstOrDefault (d => d.ProductID == int.Parse(textBox4.Text));
            var primero = ds.Order_Details
                            .FirstOrDefault(d => d.ProductID == (int)comboBox1.SelectedValue);

            if (primero != null)
            {
                //Sin control de errores
                //var ultimo = ds.Order_Details
                //               .Last(d => d.ProductID == int.Parse(textBox4.Text));

                //Con control de errores
                //var ultimo = ds.Order_Details
                //               .LastOrDefault(d => d.ProductID == int.Parse(textBox4.Text));
                var ultimo = ds.Order_Details
               .LastOrDefault(d => d.ProductID == (int)comboBox1.SelectedValue);

                cad = "Primer pedido: " + primero.OrderID +
                      "\t\tFecha: " + primero.OrdersRow.OrderDate.ToLongDateString() +
                      "\nÚltimo pedido: " + ultimo.OrderID +
                      "\tFecha: " + ultimo.OrdersRow.OrderDate.ToLongDateString();
            }
            else
            {
                //var cant = ds.Products.Count(p => p.ProductID == int.Parse(textBox4.Text));
                var cant = ds.Products.Count(p => p.ProductID == (int)comboBox1.SelectedValue);
                if (cant == 1)
                    cad = "Producto no vendido";
                else
                    cad = "Producto inexistente";
            }

            MessageBox.Show(cad);
        }

        private void button9_Click(object sender, RoutedEventArgs e)
        {
            cad = "";
            //Páginas de 20 productos
            int pagina = 0;
            int TamanoPag = 20;
            int cant = 0;

            do
            {
                var prods = ds.Products
                            .Skip(pagina * TamanoPag)
                            .Take(TamanoPag );
                pagina++;
                cant = prods.Count();
                cad = "";
                foreach (var p in prods)
                    cad += p.ProductID + " - " + p.ProductName + "\n";

                MessageBox.Show(cad, "Página: " + pagina + " (Registros: " + cant + ")"); 
            }
            while (cant == TamanoPag);

        }

        private void button10_Click(object sender, RoutedEventArgs e)
        {
            //Lo normal es hacer una ordenación por el campo que usaremos en el TakeWhile
            var prods = ds.Products
                .OrderByDescending (p=>p.UnitPrice)
                .TakeWhile(p => p.UnitPrice >= decimal.Parse(textBox4.Text));

            cad = "";
            foreach (var p in prods)
                cad += p.ProductID + " - " + 
                    p.ProductName + "\t" + 
                    p.UnitPrice.ToString("C") + "\n";

            MessageBox.Show(cad); 

        }
    }
}

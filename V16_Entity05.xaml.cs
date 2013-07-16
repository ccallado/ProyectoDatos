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
    /// Lógica de interacción para V16_Entity05.xaml
    /// </summary>
    public partial class V16_Entity05 : Window
    {
        //ObjectContext
        ProyectoDatos.northwindEntities northwindEntities;
        //Consulta de productos
        System.Data.Objects.ObjectQuery<ProyectoDatos.Producto> productosQuery;
        //Vista asociada al DataGrid creada como recurso en XAML
        System.Windows.Data.CollectionViewSource productosViewSource;

        public V16_Entity05()
        {
            InitializeComponent();

            northwindEntities = new ProyectoDatos.northwindEntities();
            productosQuery = northwindEntities.Productos;
            productosViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("productosViewSource")));
            label1.Content = "";
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            productosViewSource.Source = productosQuery.Execute(System.Data.Objects.MergeOption.AppendOnly)
                                                       .Where(p => p.CategoryID == int.Parse(textBox1.Text));
        }

        private void productosDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Producto p = productosDataGrid.SelectedItem as Producto;
            if (p != null)
            {
                label1.Content = p.ProductID;
                textBox2.Text = p.ProductName;
                textBox3.Text = p.UnitPrice.ToString();
            }
            else
            {
                label1.Content = "";
                textBox2.Text = "";
                textBox3.Text = "";
            }
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            object opr;
            System.Data.EntityKey clave =
                new System.Data.EntityKey("northwindEntities.Productos",
                                          "ProductID",
                                          Convert.ToInt32(label1.Content));
            if (northwindEntities.TryGetObjectByKey(clave, out opr))
            {
                //Si entra por verdadero sabemos que tenemos la entidad, que es de tipo Producto
                Producto p = opr as Producto;
                p.UnitPrice = decimal.Parse(textBox3.Text);
                northwindEntities.SaveChanges();
                button1_Click(null, null);
                MessageBox.Show("BBDD Actualizada");
            }
            else
                MessageBox.Show("No existe el producto");
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            Producto p;
            p = Producto.CreateProducto(-1, textBox2.Text, false);
            p.SupplierID = 1;
            p.CategoryID = int.Parse(textBox1.Text);
            p.UnitPrice = decimal.Parse(textBox3.Text);
            p.UnitsInStock = 100;
            //La instancia pasa de Detached a Inserted (Estados monitorizados)
            northwindEntities.AddToProductos(p);
            northwindEntities.SaveChanges();
            button1_Click(null, null);
            //Me muevo al último, así se actualiza la label
            productosViewSource.View.MoveCurrentToLast();

            MessageBox.Show("BBDD Actualizada");
        }

        private void button4_Click(object sender, RoutedEventArgs e)
        {
            object opr;
            System.Data.EntityKey clave =
                new System.Data.EntityKey("northwindEntities.Productos",
                                          "ProductID",
                                          Convert.ToInt32(label1.Content));
            if (northwindEntities.TryGetObjectByKey(clave, out opr))
            {
                //Si entra por verdadero sabemos que tenemos la entidad, que es de tipo Producto
                Producto pr = opr as Producto;
                MessageBoxResult res;
                res = MessageBox.Show("Seguro que desea borrar '" + pr.ProductName + "'?",
                                      "Borrado",
                                      MessageBoxButton.YesNo,
                                      MessageBoxImage.Exclamation,
                                      MessageBoxResult.No);

                if (res == MessageBoxResult.Yes)
                {
                    northwindEntities.DeleteObject(pr);
                    northwindEntities.SaveChanges();
                    button1_Click(null, null);
                    MessageBox.Show("BBDD Actualizada");
                }
            }
            else
                MessageBox.Show("No existe el producto");
        }


        //private void button2_Click(object sender, RoutedEventArgs e)
        //{
        //    //Para probar esto hay que modificar el Modelo y establecer EN CADA CAMPO DE LAS ENTIDADES DEL MODELO en que queramos chequeo, 
        //    //la propiedad "Modo de Simultaneidad" ("ConcurrencyMode") a FIXED, ya que por defecto viene a NONE y no se chequea concurrencia.

        //    //Además el método SaveChanges() crea una transacción IMPLICITA que autogestiona.
        //    //Si falla la operación del método, además de saltar la excepción deshace lo que haya hecho.
        //    try
        //    {
        //        northwindEntities.SaveChanges();
        //        northwindEntities.Refresh(System.Data.Objects.RefreshMode.StoreWins, northwindEntities.Productos);
        //        MessageBox.Show("BBDD actualizada.");
        //    }
        //    catch (System.Data.OptimisticConcurrencyException ex)
        //    {
        //        //MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        MessageBoxResult res;
        //        res = MessageBox.Show("Se han producido errores de concurrencia.\n¿Desea sobreescribir los valores de la BBDD?", 
        //                              "Error de concurrencia.", 
        //                              MessageBoxButton.YesNo,
        //                              MessageBoxImage.Exclamation,
        //                              MessageBoxResult.No);
        //        if (res == MessageBoxResult.Yes)
        //        {
        //            //Recargo pero doy prioridad a mis datos. Las entidades quedan preparadas para ser actualizadas.
        //            northwindEntities.Refresh(System.Data.Objects.RefreshMode.ClientWins, northwindEntities.Productos);
        //            northwindEntities.SaveChanges();
        //            MessageBox.Show("BBDD actualizada con los datos locales.");
        //        }
        //        else
        //        {
        //            //Recargo pero doy prioridad a los datos de la BBDD.
        //            northwindEntities.Refresh(System.Data.Objects.RefreshMode.StoreWins, northwindEntities.Productos);
        //            MessageBox.Show("Recargado con los datos de la BBDD.");
        //        }
        //    }

        //}

       
    }
}

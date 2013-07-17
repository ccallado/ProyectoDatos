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
    /// Lógica de interacción para V17_Entity06.xaml
    /// </summary>
    public partial class V17_Entity06 : Window
    {
        ProyectoDatos.northwindEntities northwindEntities;
        
        //Formulario orientado a trabajar en concurrencia
        public V17_Entity06()
        {
            InitializeComponent();

            northwindEntities = new ProyectoDatos.northwindEntities();
            //En esta no he usado ViewSource. No hay vistar por en medio.
            //Directamente el ItemsSource
            //De esta manera se puede EDITAR
            productosDataGrid.ItemsSource = northwindEntities.Productos.Execute( System.Data.Objects.MergeOption.AppendOnly);
        }

        //Recargar (gana BBDD)
        //Se trae todos los registros incluyendo los nuevos, MACHACANDO mis cambios
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            northwindEntities.Refresh(System.Data.Objects.RefreshMode.StoreWins, northwindEntities.Productos);
            MessageBox.Show("Recargado con los datos de la BBDD.", "Gana BBDD");
        }

        //Recargar (gano yo)
        //Se trae todos los registros incluyendo los nuevos, MANTENIENDO mis cambios
        private void button2_Click(object sender, RoutedEventArgs e)
        {
            northwindEntities.Refresh(System.Data.Objects.RefreshMode.ClientWins, northwindEntities.Productos);
            MessageBox.Show("Recargado con los datos de la BBDD.", "Gano yo");
        }

        //Actualizar BBDD
        private void button3_Click(object sender, RoutedEventArgs e)
        {
            //Para probar esto hay que modificar el Modelo y establecer EN CADA CAMPO DE LAS ENTIDADES DEL MODELO en que queramos chequeo, 
            //la propiedad "Modo de Simultaneidad" ("ConcurrencyMode") a FIXED, ya que por defecto viene a NONE y no se chequea concurrencia.

            //Además el método SaveChanges() crea una transacción IMPLICITA que autogestiona.
            //Si falla la operación del método, además de saltar la excepción deshace lo que haya hecho.
            try
            {
                northwindEntities.SaveChanges();
                //Actualizo la pantalla
                northwindEntities.Refresh(System.Data.Objects.RefreshMode.StoreWins, northwindEntities.Productos);
                MessageBox.Show("BBDD actualizada.");
            }
            //Error de concurrencia
            catch (System.Data.OptimisticConcurrencyException ex)
            {
                MessageBoxResult res;
                res = MessageBox.Show("Se han producido errores de concurrencia.\n¿Desea recargar manteniendo los cambios?",
                                      "Error de concurrencia.",
                                      MessageBoxButton.YesNo,
                                      MessageBoxImage.Exclamation,
                                      MessageBoxResult.No);
                if (res == MessageBoxResult.Yes)
                {
                    //Recargo pero doy prioridad a mis datos. Las entidades quedan preparadas para ser actualizadas.
                    //Podrían haber cambiado datos en campos diferentes a los que yo he cambiado.
                    northwindEntities.Refresh(System.Data.Objects.RefreshMode.ClientWins, northwindEntities.Productos);
                    //Si falla ya no lo tenemos controlado con un try catch y daría una excepción
                    northwindEntities.SaveChanges();
                    MessageBox.Show("BBDD actualizada con los datos locales.");
                }
                else
                {
                    //Recargo pero doy prioridad a los datos de la BBDD.
                    northwindEntities.Refresh(System.Data.Objects.RefreshMode.StoreWins, northwindEntities.Productos);
                    MessageBox.Show("Recargado con los datos de la BBDD.");
                }
            }
                //Errores normales
            catch (Exception ex)
            {
                MessageBox.Show("Se produjo una excepción.\n\nError recibido: " + ex.Message +
                    //Si haya varias excepciones
                    (ex.InnerException != null ? "\n\nError original: " + ex.GetBaseException().Message : ""),
                                "Excepción de tipo " + ex.GetType(),
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                //Como no existe .RejectChanges() tengo que recargar los datos sobreescribiendo con los de la BBDD                
                productosDataGrid.ItemsSource = northwindEntities.Productos.Execute(System.Data.Objects.MergeOption.OverwriteChanges);
            }
        }

    }
}

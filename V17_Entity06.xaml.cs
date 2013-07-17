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
            productosDataGrid.ItemsSource = northwindEntities.Productos.Execute(System.Data.Objects.MergeOption.AppendOnly);

            label1.Content = "";
        }

        //Recargar (gana BBDD)
        //Se trae todos los registros incluyendo los nuevos, MACHACANDO mis cambios
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            //Esto actualiza tanto los valores originales como los actuales para que coincidan
            //con los actuales de la BBDD (por eso refresca los datos del DataGrid)
            //Por lo tanto TODAS las entidades quedan Unchanged.
            //Pero OJO: No trae entidades nuevas. Sólo refresca las que YA tiene.
            northwindEntities.Refresh(System.Data.Objects.RefreshMode.StoreWins, northwindEntities.Productos);
            //Esto es necesario para que los eliminados vuelvan a aparecer, ya que aunque cambiaban
            //su estado de Deleted a Unchanged, no reaparecen gráficamente en el grid si no reasignamos ItemsSource.
            productosDataGrid.ItemsSource = northwindEntities.Productos;
            MessageBox.Show("Recargado con los datos de la BBDD.", "Gana BBDD");
        }

        //Recargar (gano yo)
        //Se trae todos los registros incluyendo los nuevos, MANTENIENDO mis cambios
        private void button2_Click(object sender, RoutedEventArgs e)
        {
            //En entidades NO MODIFICADAS (Unchanged), actualiza originales y actuales.
            //En entidades MODIFIED, sólo actualiza los datos originales, no los actuales, con lo que el DataGrid no se verá actualizado.
            //  Además deja TODAS las propiedades de la entidad como modificadas (comprobar con Button4)
            //Como en ambos casos sí se actualizan los valores originales con los que vienen de la BBDD, no vuelve a dar errores 
            //de concurrencia al hacer el segundo SaveChanges, porque los originales pasan a ser los de la BBDD.
            //Pero OJO: No trae entidades nuevas. Sólo refresca las que YA tiene.
            //Por lo tanto cada entidad mantiene su estado y no hay que reasignar el ItemSource porque las Deleted siguen borradas.
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
                //Esto refresca sólo las entidades cargadas. No trae nuevas entidades si las hay. Para eso, habría que reejecutar la consulta.
                northwindEntities.Refresh(System.Data.Objects.RefreshMode.StoreWins, northwindEntities.Productos);
                MessageBox.Show("BBDD actualizada.");
            }
            //Error de concurrencia
            catch (System.Data.OptimisticConcurrencyException ex)
            {
                //Esta excepción sólo salta cuando se chequean ACTUALIZACIONES de campos a lo que les hayamos
                //establecido el Modo de Simultaneidad a "Fixed". 
                //NO salta nunca en borrados o inserciones de entidades.
                MessageBoxResult res;
                res = MessageBox.Show("Se han producido errores de concurrencia. Se van a recargar los datos para reestablecer de nuevo los valores originales de mis entidades.\n¿Desea mantener los cambios al recargar desde la BBDD?",
                                      "Error de concurrencia.",
                                      MessageBoxButton.YesNo,
                                      MessageBoxImage.Exclamation,
                                      MessageBoxResult.No);
                if (res == MessageBoxResult.Yes)
                {
                    //ccs Recargo pero doy prioridad a mis datos. Las entidades quedan preparadas para ser actualizadas.
                    //ccs Podrían haber cambiado datos en campos diferentes a los que yo he cambiado.
                    
                    //Actualizo las entidades pero doy prioridad a mis datos. Las entidades quedan preparadas para ser actualizadas
                    //sin provocar otro error de concurrencia, ya que lo que hace es cambiar los datos originales 
                    //de MIS entidades para que coincidan con los de la BBDD, deja TODAS las propiedades con estado modificado 
                    //y así ya puede salvar los datos actuales en el siguiente SaveChanges.
                    northwindEntities.Refresh(System.Data.Objects.RefreshMode.ClientWins, northwindEntities.Productos);
                    //Si falla ya no lo tenemos controlado con un try catch y daría una excepción
                    //Aquí como las entidades han mantenido los cambios, pero ahora tienen los datos de la BBDD como valores originales,
                    //y todas las propiedades como modificadas, así que no debería dar error de concurrencia.
                    northwindEntities.SaveChanges();
                    MessageBox.Show("BBDD actualizada con los datos locales.");
                }
                else
                {
                    //Actualizo pero doy prioridad a los datos de la BBDD.
                    northwindEntities.Refresh(System.Data.Objects.RefreshMode.StoreWins, northwindEntities.Productos);
                    MessageBox.Show("Recargado con los datos de la BBDD.");
                }
            }
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

        private void button4_Click(object sender, RoutedEventArgs e)
        {
            //Leer este link para explicación de MSDN
            //http://msdn.microsoft.com/query/dev10.query?appId=Dev10IDEF1&l=ES-ES&k=k(SYSTEM.DATA.OBJECTS.MERGEOPTION.PRESERVECHANGES);k(PRESERVECHANGES);k(TargetFrameworkMoniker-%22.NETFRAMEWORK%2cVERSION%3dV4.0%22);k(DevLang-CSHARP)&rd=true

            //Reejecuto la consulta, pero manteniendo los posibles cambios que yo tuviera.
            //MergeOption.PreserveChanges se comporta como RefreshMode.ClientWins, pero con dos diferencias:
            //  - Se traen TODAS las entidades, incluyendo las nuevas (en vez de sólo refrescar las que ya teníamos)
            //  - Se cambia el estado a modificada sólo de las propiedades que quedan original y actual distintos (RefreshMode.ClientWins marcaba TODAS como modificadas)

            productosDataGrid.ItemsSource = northwindEntities.Productos.Execute(System.Data.Objects.MergeOption.PreserveChanges);
        }

        private void button5_Click(object sender, RoutedEventArgs e)
        {
            string cad = "";
            string cad2 = "";

            //Esto me permite recorrer las entidades en función de su estado y sacar información.
            var osm = northwindEntities.ObjectStateManager.GetObjectStateEntries(System.Data.EntityState.Modified);
            cad += "Modificadas: " + osm.Count() + "\n";
            foreach (var p in osm)
            {
                cad += "Modificada: " + p.EntityKey.EntityKeyValues[0] +
                       " - " + ((Producto)p.Entity).ProductID + " - " + ((Producto)p.Entity).ProductName + "\n";
                cad += "\tCampos modificados: ";
                cad2 = "";
                foreach (string prop in p.GetModifiedProperties())
                    cad2 += (cad2 != "" ? ", " : "") + prop;
                cad += cad2 + "\n";
            }

            osm = northwindEntities.ObjectStateManager.GetObjectStateEntries(System.Data.EntityState.Deleted);
            cad += "\nBorradas: " + osm.Count() + "\n";
            foreach (var p in osm)
            {
                //En las marcadas para borrar NO puedo acceder a los datos, sólo a la EntityKey
                //o a los valores originales, pero NUNCA a los actuales...
                cad += "Borrada: " + p.EntityKey.EntityKeyValues[0] +
                       " -> Datos originales: " + p.GetUpdatableOriginalValues()["ProductName"] + "\n";
            }

            osm = northwindEntities.ObjectStateManager.GetObjectStateEntries(System.Data.EntityState.Added);
            cad += "\nInsertadas: " + osm.Count() + "\n";
            foreach (var p in osm)
            {
                //Esta todavía NO tiene EntityKey, así que pongo el ProductId (será 0 si no hemos puesto valor)
                cad += "Nueva: " + ((Producto)p.Entity).ProductID + " - " + 
                    ((Producto)p.Entity).ProductName + "\n";
            }

            osm = northwindEntities.ObjectStateManager.GetObjectStateEntries(System.Data.EntityState.Unchanged);
            //De estas sólo ponemos la cantidad para ver el total.
            cad += "\nSin cambios: " + osm.Count();

            MessageBox.Show(cad, "Productos totales: " + northwindEntities.Productos.Count());
        }

        private void productosDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Producto p = productosDataGrid.SelectedItem as Producto;
            if (p != null)
                label1.Content = p.EntityState;
            else
                label1.Content = "";
        }
    }
}

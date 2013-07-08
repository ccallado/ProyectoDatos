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

//Añado el subespacio de nombre Extensiones donde esta la clase estática
using ProyectoDatos.Extensiones;
namespace ProyectoDatos
{
    /// <summary>
    /// Lógica de interacción para V07_LINQ1.xaml
    /// </summary>
    public partial class V07_LINQ1 : Window
    {
        public V07_LINQ1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            int i1 = 0;
            //Da error porque no es nullable
            //i1 = null;

            //Estas dos definiciones son identicas
            //int? i2 = 0;
            Nullable<int> i2 = 0;
            i2 = null;
            if (i2.HasValue)
                i1 = i2.Value;
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            //cosa muy absurda
            object O1;

            //Inferencia
            var O2 = 4;
            var O5 = 4.00;
            var O6 = 4F;
            var O3 = "Pepe";
            var O4 = new NWDataSet.CategoriesDataTable();


        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            string cad = "Hola";
            MessageBox.Show("Primero: " + cad.First() + "\nÚltimo: " + cad.Last());

            int x = 9;

            //Los enteros no tienen metodos de extensión
            MessageBox.Show("Valor absoluto de x = " + x.ValorAbsoluto(), "x = " + x);

            x = -23;

            MessageBox.Show("Valor absoluto de x = " + x.ValorAbsoluto(), "x = " + x);

            float n = 12345.678F;
            MessageBox.Show("Español: " + n.Moneda(enumPaises.España) +
                "\nEEUU: " + n.Moneda(enumPaises.EEUU) +
                "\nInglaterra: " + n.Moneda(enumPaises.Inglaterra),
                "Valor: " + n);
        }

        private void button4_Click(object sender, RoutedEventArgs e)
        {
            //Utilización de la clase Cliente "SIN NOVEDAD"
            Cliente c1 = new Cliente();
            c1.Id = 3;
            c1.Nombre = "Pepe";

            Cliente c2 = new Cliente(4);
            c2.Nombre = "Juan";

            Cliente c3 = new Cliente(56) { Nombre = "Antonio" };
        }

        private void button5_Click(object sender, RoutedEventArgs e)
        {
            Cliente C1 = new Cliente();
            C1.Id = 3;
            C1.Nombre = "Pepe";

            var C2 = C1;

            var c3 = new Cliente() { Id= 6, Nombre = "Andrés" };
            var c4 = new { Id = 7, Nombre = "Andrés" };
            var c5 = new { Id = 8, Nombre = "María", Localidad= "Madrid" };
        }
    }
}

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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProyectoDatos
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Window v = new V01_Conexiones();
            v.Show();
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            Window v = new V02_Conectado();
            v.Show();
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            Window v = new V03_Conectado2();
            v.Show();
        }

        private void button4_Click(object sender, RoutedEventArgs e)
        {
            Window v = new V04_Desconectado1();
            v.Show();
        }

        private void button5_Click(object sender, RoutedEventArgs e)
        {
            //DataGrid, todo gráficamente
            Window v = new V05_Desconectado2();
            v.Show();
        }

        private void button6_Click(object sender, RoutedEventArgs e)
        {
            //Maestro Detalle, casi todo gráficamente
            Window v = new V06_Desconectado3();
            v.Show();
        }

        private void button7_Click(object sender, RoutedEventArgs e)
        {
            //Maestro Detalle, casi todo gráficamente
            Window v = new V07_LINQ1();
            v.Show();
        }

        private void button8_Click(object sender, RoutedEventArgs e)
        {
            Window v = new V08_LINQ2();
            v.Show();
        }

        private void button9_Click(object sender, RoutedEventArgs e)
        {
            Window v = new V09_LINQ3();
            v.Show();
        }

        private void button10_Click(object sender, RoutedEventArgs e)
        {
            Window v = new V10_LINQ4();
            v.Show();
        }

        private void button11_Click(object sender, RoutedEventArgs e)
        {
            Window v = new V11_LINQ5();
            v.Show();
        }

        private void button12_Click(object sender, RoutedEventArgs e)
        {
            Window v = new V12_Entity1();
            v.Show();
        }

        private void button13_Click(object sender, RoutedEventArgs e)
        {
            Window v = new V13_Entity2();
            v.Show();
        }
    }
}

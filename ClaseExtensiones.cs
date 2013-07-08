using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//Creaamos un subespacio de nombre para ella
//La usamos en V07_LINQ1

namespace ProyectoDatos.Extensiones
{
    //La clase tiene que ser estática
    public static class ClaseExtensiones
    {
        //Metodo estático en clase estática
        //Primer parámetro es obligatorio precedido de "THIS"
        public static int ValorAbsoluto(this int valor)
        {
            return (valor >= 0 ? valor : -valor);
        }
        //Vamos a usar una enumeración
        //Metodo de estensión sobre un float que va ha recibir un segundo parámetro que va a ser
        //una enumeración de paises para que me devuelva con formata de moneda del pais.
        public static string Moneda(this float valor, enumPaises pais)
        {
            switch (pais)
            {
                case enumPaises.España:
                    return valor.ToString("c", new System.Globalization.CultureInfo("es-ES"));
                case enumPaises.EEUU:
                    return valor.ToString("c", new System.Globalization.CultureInfo("en-US"));
                case enumPaises.Inglaterra:
                default:
                    return valor.ToString("c", new System.Globalization.CultureInfo("en-GB"));
            }
        }
    }
}

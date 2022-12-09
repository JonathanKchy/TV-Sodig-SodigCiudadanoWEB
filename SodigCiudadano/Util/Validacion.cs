using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SodigCiudadano.Util
{
    public class Validacion
    {
        public static bool verificarIdentificacion(string identificacion)
        {
            bool respuesta = false;
            if (!String.IsNullOrEmpty(identificacion))
            {
                if (identificacion.Length == 10)
                {
                    int digito_region = int.Parse(identificacion.Substring(0, 2));
                    if (digito_region >= 1 && digito_region <= 30)
                    {
                        int ultimo_digito = int.Parse(identificacion.Substring(9, 1));
                        int pares = int.Parse(identificacion.Substring(1, 1)) + int.Parse(identificacion.Substring(3, 1)) + int.Parse(identificacion.Substring(5, 1)) + int.Parse(identificacion.Substring(7, 1));
                        int numero1 = int.Parse(identificacion.Substring(0, 1)) * 2;
                        if (numero1 > 9) { numero1 -= 9; }
                        int numero3 = int.Parse(identificacion.Substring(2, 1)) * 2;
                        if (numero3 > 9) { numero3 -= 9; }
                        int numero5 = int.Parse(identificacion.Substring(4, 1)) * 2;
                        if (numero5 > 9) { numero5 -= 9; }
                        int numero7 = int.Parse(identificacion.Substring(6, 1)) * 2;
                        if (numero7 > 9) { numero7 -= 9; }
                        int numero9 = int.Parse(identificacion.Substring(8, 1)) * 2;
                        if (numero9 > 9) { numero9 -= 9; }
                        int impares = numero1 + numero3 + numero5 + numero7 + numero9;
                        int suma_total = (pares + impares);
                        int primer_digito_suma = 0;
                        string contar = "";
                        contar = suma_total.ToString();
                        string contar2 = contar.Length.ToString();
                        if (contar2 == "1")
                            primer_digito_suma = int.Parse(('0' + suma_total.ToString()).Substring(0, 1));
                        else { primer_digito_suma = int.Parse(suma_total.ToString().Substring(0, 1)); }

                        int decena = (primer_digito_suma + 1) * 10;
                        int digito_validador = decena - suma_total;
                        if (digito_validador == 10)
                            digito_validador = 0;
                        if (digito_validador == ultimo_digito)
                            respuesta = true;

                    }

                }
            }
            return respuesta;
        }
    }
}
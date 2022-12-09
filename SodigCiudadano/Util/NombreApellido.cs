using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SodigCiudadano.Util
{
    public class NombreApellido
    {
        public string primerNombre { get; set; }
        public string segundoNombre { get; set; }
        public string primerApellido { get; set; }
        public string segundoApellido { get; set; }

        public NombreApellido GetNombreApellido(string nombre)
        {
            NombreApellido result = new NombreApellido();

            string[] nombres = nombre.Split(new char[]
                {
                         ' '
                });


            int p = nombres.Length;
            int j = 0;
            bool pase = false;

            while (pase != true && j < p)
            {
                if (nombres[j].Length > 3)
                {
                    pase = true;
                }

                if (string.IsNullOrEmpty(result.primerApellido))
                {
                    result.primerApellido += nombres[j];
                }
                else
                {
                    result.primerApellido += " " + nombres[j];
                }

                j++;
            }

            pase = false;

            while (pase != true && j < p)
            {
                if (nombres[j].Length > 3)
                {
                    pase = true;
                }

                if (string.IsNullOrEmpty(result.segundoApellido))
                {
                    result.segundoApellido += nombres[j];
                }
                else
                {
                    result.segundoApellido += " " + nombres[j];
                }
                j++;
            }

            pase = false;

            while (pase != true && j < p)
            {
                if (nombres[j].Length > 3)
                {
                    pase = true;
                }

                if (string.IsNullOrEmpty(result.primerNombre))
                {
                    result.primerNombre += nombres[j];
                }
                else
                {
                    result.primerNombre += " " + nombres[j];
                }
                j++;
            }

            pase = false;

            while (j < p)
            {
                if (string.IsNullOrEmpty(result.segundoNombre))
                {
                    result.segundoNombre += nombres[j];
                }
                else
                {
                    result.segundoNombre += " " + nombres[j];
                }
                j++;
            }



            return result;
        }

    }
}
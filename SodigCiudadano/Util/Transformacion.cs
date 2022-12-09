using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;


namespace SodigCiudadano.Util
{
    public class Transformacion
    {
        public static byte[] convertToByte(string cadena)
        {
            string convert = cadena.Replace("data:image/png;base64,", String.Empty);
            var cadenaByte = Convert.FromBase64String(convert);
            return cadenaByte;
        }

        public static Image ConvertFrameToImg(byte[] frame)
        {
            Image Imagen;
            byte[] img = frame;
            MemoryStream Memoria = new MemoryStream(img);
            Imagen = Image.FromStream(Memoria);
            Memoria.Close();
            return Imagen;
        }

        public static Bitmap Base64StringToBitmap(string base64String)
        {
            Bitmap bmpReturn = null;
            byte[] byteBuffer = Convert.FromBase64String(base64String);
            MemoryStream memoryStream = new MemoryStream(byteBuffer);
            memoryStream.Position = 0;
            bmpReturn = (Bitmap)Bitmap.FromStream(memoryStream);
            memoryStream.Close();
            memoryStream = null;
            byteBuffer = null;
            return bmpReturn;
        }

        public string reemplazarCaracteres(string palabra)
        {
            palabra = palabra.Replace('ñ', 'n');
            palabra = palabra.Replace('Ñ', 'N');
            palabra = palabra.Replace('á', 'a');
            palabra = palabra.Replace('Á', 'A');
            palabra = palabra.Replace('é', 'e');
            palabra = palabra.Replace('É', 'E');
            palabra = palabra.Replace('í', 'i');
            palabra = palabra.Replace('Í', 'i');
            palabra = palabra.Replace('ó', 'o');
            palabra = palabra.Replace('Ó', 'O');
            palabra = palabra.Replace('Ú', 'U');
            palabra = palabra.Replace('ú', 'u');
            return palabra;
        }
    }
}
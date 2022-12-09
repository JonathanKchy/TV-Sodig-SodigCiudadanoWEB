using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SodigCiudadano.Models.Entidades
{
    public class Rostro
    {
        public string requestId { get; set; }
        public string code { get; set; }
        public string message { get; set; }
        public List<Faces> faces { get; set; }
    }

    public class Faces
    {
        public int age { get; set; }
        public string gender { get; set; }
        public FaceRectangle faceRectangle { get; set; }
    }

    public class FaceRectangle
    {
        public int left { get; set; }
        public int top { get; set; }
        public int width { get; set; }
        public int height { get; set; }
    }

    public class RostroResponse
    {
        public bool estado { get; set; }
        public Rostro obj { get; set; }
        public string mensajes { get; set; }
    }
}
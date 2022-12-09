using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SodigCiudadano.Models.Entidades
{
    public class Ocr
    {
        public string status { get; set; }
        public string createdDateTime { get; set; }
        public string lastUpdatedDateTime { get; set; }
        public AnalyzeResult analyzeResult { get; set; }
        public string error { get; set; }
    }

    public class AnalyzeResult
    {
        public string version { get; set; }
        public List<ReadResults> readResults { get; set; }
    }

    public class ReadResults
    {
        public string page { get; set; }
        public string language { get; set; }
        public string angle { get; set; }
        public string width { get; set; }
        public string height { get; set; }
        public string unit { get; set; }
        public List<Lines> lines { get; set; }
    }

    public class Lines
    {
        public string text { get; set; }
    }

    public class OcrResponse
    {
        public bool estado { get; set; }
        public Ocr obj { get; set; }
        public string mensajes { get; set; }
    }
}
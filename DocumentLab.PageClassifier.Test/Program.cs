using ImageMagick;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using DocumentLab.Core.Extensions;
using System.Drawing;
using MathNet;
using SharpLearning.Neural;
using System.Diagnostics;
using System.IO;
using System.Linq;
using SharpLearning.Containers.Matrices;
using SharpLearning.InputOutput.Csv;
using SharpLearning.Metrics.Classification;
using SharpLearning.Neural.Activations;
using SharpLearning.Neural.Layers;
using SharpLearning.Neural.Learners;
using SharpLearning.Neural.Loss;
using MathNet.Numerics.Properties;
using System.IO;
using PdfiumViewer;
using System;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Drawing.Imaging;

namespace DocumentLab.PageClassifier.Test
{
    public class Program
    {
        static void Main(string[] args)
        {

            var c = new Classifying();

            MagickImage magickImage = new MagickImage(File.ReadAllBytes("test.png"));
            MagickImage templateMagickImage = new MagickImage(File.ReadAllBytes("template.png"));

            c.CreateNeuralNetwork("PageClassifier");

            Debug.WriteLine(c.Predict("documentLabPictureClassifier.xml",
                (Bitmap)System.Drawing.Image.FromFile("D:\\Invoices\\apotea\\11616243.pdf2_1.png4_1.png")));

          /*  using (var tms = new MemoryStream(templateMagickImage.ToByteArray()))
            using (var ms = new MemoryStream(magickImage.ToByteArray()))
            {
                var b = new Bitmap(ms);
                var tb = new Bitmap(tms);
                c.Classify(b, tb);
            }*/
        
        
        
        
        
        
        
        }
    }
}

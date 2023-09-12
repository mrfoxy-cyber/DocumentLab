using ImageMagick;
using System.IO;

namespace DocumentLab.PageClassifier.Test
{
  public class Program
  {
    static void Main(string[] args)
    {

      var c = new Classifying();

      MagickImage magickImage = new MagickImage(File.ReadAllBytes("test.png"));
      MagickImage templateMagickImage = new MagickImage(File.ReadAllBytes("template.png"));

      c.CreateNeuralNetwork("PageClassifier", "D:\\Invoices");

      // Debug.WriteLine(c.Predict("documentLabPictureClassifier.xml",
      //   (Bitmap)System.Drawing.Image.FromFile("D:\\Invoices\\apotea\\11616243.pdf2_1.png4_1.png")));

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

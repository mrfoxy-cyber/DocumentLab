namespace DocumentLab.PageClassifier
{
  using ImageMagick;
  using SharpLearning.Containers.Matrices;
  using SharpLearning.InputOutput.Csv;
  using SharpLearning.Metrics.Classification;
  using SharpLearning.Neural;
  using SharpLearning.Neural.Activations;
  using SharpLearning.Neural.Layers;
  using SharpLearning.Neural.Learners;
  using SharpLearning.Neural.Loss;
  using System;
  using System.Diagnostics;
  using System.Drawing;
  using System.IO;
  using System.Linq;
  using System.Text;

  public class Classifying
  {
    public Classifying()
    {
    }

    public void Classify(Bitmap image, Bitmap bmpSnip)
    {
      /* var img = new MagickImage(image.ToByteArray(ImageFormat.Png).ToArray());

       using (var cvImage = image.ToCvImage())
       {
           //Image<Bgr, byte> templateImage = bmpSnip.ToCvImage();
           var imgMatch = cvImage.MatchTemplate(bmpSnip.ToCvImage(), Emgu.CV.CvEnum.TemplateMatchingType.CcorrNormed);
           CascadeClassifier cascadeClassifier = new CascadeClassifier();

           float[,,] matches = imgMatch.Data;
           for (int x = 0; x < matches.GetLength(0); x++)
           {
               for (int y = 0; y < matches.GetLength(1); y++)
               {
                   double matchScore = matches[x, y, 0];
                   if (matchScore > 0.91)
                   {
                       cvImage.Draw(new Rectangle(x, y, 2, 2), new Bgr(255, 127, 0), 2);
                   }

               }

           }


           File.WriteAllBytes("out.png", cvImage.ToBitmap().ToByteArray(ImageFormat.Png).ToArray());
           File.WriteAllBytes("out2.png", imgMatch.ToBitmap().ToByteArray(ImageFormat.Png).ToArray());
       }*/
    }

    public void CreateDataSet(Bitmap image)
    {
    }

    public void CreateNeuralNetwork(String name, String DirecoryWhereTheInvoicesAre)
    {
      if (File.Exists(DirecoryWhereTheInvoicesAre + "\\small_test.csv"))
      {
        File.Delete(DirecoryWhereTheInvoicesAre + "\\small_test.csv");
      }

      if (File.Exists(DirecoryWhereTheInvoicesAre + "\\small_train.csv"))
      {
        File.Delete(DirecoryWhereTheInvoicesAre + "\\small_train.csv");
      }

      MagickImage magickImage1;
      int i = 0;
      int j = 0;
      StringBuilder title = new StringBuilder();
      StringBuilder indexInterpreter = new StringBuilder();
      foreach (string directory in Directory.GetDirectories(DirecoryWhereTheInvoicesAre))
      {
        j++;

        indexInterpreter.AppendLine(j + " = " + directory);
        foreach (string file in Directory.EnumerateFiles(directory, "*.png"))
        {
          StringBuilder sb = new StringBuilder();

          sb.Append(j + "; ");
          title.Append("Class" + "; ");
          ;

          Bitmap b = (Bitmap)System.Drawing.Image.FromFile(file);
          b = new Bitmap(b, new Size(300, 100));

          int k = 0;
          for (int y = 0; y < b.Height; y++)
          {
            for (int x = 0; x < b.Width; x++)
            {
              if (x == b.Width - 1 && y == b.Height - 1)
              {
                sb.Append(b.GetPixel(x, y).R);
                title.Append("Pixel" + k);
                k++;
              }
              else
              {
                sb.Append(b.GetPixel(x, y).R);
                sb.Append("; ");
                title.Append("Pixel" + k + "; ");
                k++;
              }

            }
          }

          sb.AppendLine(" ");
          title.AppendLine(" ");
          if (i == 0)
          {
            File.AppendAllText(DirecoryWhereTheInvoicesAre + "\\small_test.csv", title.ToString());
            File.AppendAllText(DirecoryWhereTheInvoicesAre + "\\small_train.csv", title.ToString());
          }

          if (i % 4 == 0)
          {
            File.AppendAllText(DirecoryWhereTheInvoicesAre + "\\small_test.csv", sb.ToString());
          }
          else
          {
            File.AppendAllText(DirecoryWhereTheInvoicesAre + "\\small_train.csv", sb.ToString());
          }

          File.AppendAllText(DirecoryWhereTheInvoicesAre + "\\indexInterpreter.csv", indexInterpreter.ToString());

          i++;
        }
      }

      var trainingParser = new CsvParser(() => new StringReader(File.ReadAllText(@DirecoryWhereTheInvoicesAre + "\\small_train.csv")));
      var testParser = new CsvParser(() => new StringReader(File.ReadAllText(@DirecoryWhereTheInvoicesAre + "\\small_test.csv")));

      var targetName = "Class";

      var featureNames = trainingParser.EnumerateRows(k => k != targetName).First().ColumnNameToIndex.Keys.ToArray();

      // read feature matrix (training)
      var trainingObservations = trainingParser
          .EnumerateRows(featureNames)
          .ToF64Matrix();
      // read classification targets (training)
      var trainingTargets = trainingParser.EnumerateRows(targetName)
          .ToF64Vector();

      // read feature matrix (test) 
      var testObservations = testParser
          .EnumerateRows(featureNames)
          .ToF64Matrix();
      // read classification targets (test)
      var testTargets = testParser.EnumerateRows(targetName)
          .ToF64Vector();

      // transform pixel values to be between 0 and 1.
      trainingObservations.Map(p => p / 255);
      testObservations.Map(p => p / 255);

      // the output layer must know the number of classes.
      var numberOfClasses = trainingTargets.Distinct().Count();

      // define the neural net.
      var net = new NeuralNet();
      net.Add(new InputLayer(width: 300, height: 100, depth: 1)); // MNIST data is 28x28x1.
      net.Add(new Conv2DLayer(filterWidth: 5, filterHeight: 5, filterCount: 32));
      net.Add(new MaxPool2DLayer(poolWidth: 2, poolHeight: 2));
      net.Add(new DropoutLayer(0.5));
      net.Add(new DenseLayer(256, Activation.Relu));
      net.Add(new DropoutLayer(0.5));
      net.Add(new SoftMaxLayer(numberOfClasses));

      // using only 10 iteration to make the example run faster.
      // using classification accuracy as error metric. This is only used for reporting progress.
      var learner = new ClassificationNeuralNetLearner(net, iterations: 10, loss: new AccuracyLoss());
      var model = learner.Learn(trainingObservations, trainingTargets);

      var metric = new TotalErrorClassificationMetric<double>();
      var predictions = model.Predict(testObservations);

      Debug.WriteLine("Test Error: " + metric.Error(testTargets, predictions));

      for (int z = 0; z < predictions.Length; z++)
      {
        Debug.WriteLine(predictions[z] + " " + testTargets[z]);
      }

      model.Save(() => new StreamWriter(DirecoryWhereTheInvoicesAre + "\\" + name + ".xml"));
    }

    public String Predict(string modelFullPath, String modelName, Bitmap image)
    {
      var loadedModel = SharpLearning.Neural.Models.ClassificationNeuralNetModel.Load(() => new StreamReader(Constants.NeuralNetworkClassifier));

      StringBuilder sb = new StringBuilder();
      int i = 0;
      int j = 0;
      StringBuilder title = new StringBuilder();

      sb.Append(j + "; ");
      title.Append("Class" + "; ");

      Bitmap b = image;
      //b.Save(file + i + "_" + j + ".png");
      b = new Bitmap(b, new Size(300, 100));

      int k = 0;
      for (int y = 0; y < b.Height; y++)
      {
        for (int x = 0; x < b.Width; x++)
        {
          if (x == b.Width - 1 && y == b.Height - 1)
          {
            sb.Append(b.GetPixel(x, y).R);
            title.Append("Pixel" + k);
            k++;
          }
          else
          {
            sb.Append(b.GetPixel(x, y).R);
            sb.Append("; ");
            title.Append("Pixel" + k + "; ");
            k++;
          }

        }
      }
      sb.AppendLine(" ");
      title.AppendLine(" ");

      if (File.Exists(modelFullPath + "\\" + "small_image.csv"))
      {
        File.Delete(modelFullPath + "\\" + "small_image.csv");
      }

      File.AppendAllText(modelFullPath + "\\" + "small_image.csv", title.ToString());
      File.AppendAllText(modelFullPath + "\\" + "small_image.csv", sb.ToString());

      var testParser = new CsvParser(() => new StringReader(File.ReadAllText(@modelFullPath + "\\" + "small_image.csv")));

      var targetName = "Class";

      var featureNames = testParser.EnumerateRows(m => m != targetName).First().ColumnNameToIndex.Keys.ToArray();

      // read feature matrix (test) 
      var testObservations = testParser
          .EnumerateRows(featureNames)
          .ToF64Matrix();
      // read classification targets (test)
      var testTargets = testParser.EnumerateRows(targetName)
          .ToF64Vector();

      testObservations.Map(p => p / 255);

      var predictions = loadedModel.Predict(testObservations);

      Constants.InitRuleSetPredictionDirectory();
      if (DocumentLab.PageClassifier.Constants.RuleSetPredictionDirectory[Convert.ToInt32(predictions[0])] != null)
      {
        return DocumentLab.PageClassifier.Constants.ReadFromRuleSetPredictionDirectory(Convert.ToInt32(predictions[0]));
      }
      else return "no preditcion ruleset exist";
      // return 1.0;
    }

  }
}

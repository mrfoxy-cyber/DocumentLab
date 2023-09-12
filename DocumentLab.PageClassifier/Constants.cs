using System;
using System.IO;

namespace DocumentLab.PageClassifier
{
  internal class Constants
  {
    public static string NeuralNetworkClassifier = "D:\\Invoices\\PageClassifier.xml";

    public static String[] RuleSetPredictionDirectory = new string[9999];

    public static void InitRuleSetPredictionDirectory()
    {
      RuleSetPredictionDirectory[0] = "";
      RuleSetPredictionDirectory[1] = "D:\\Invoices\\apotea\\ruleset.txt";
    }

    public void AddtoRuleSetPredictionDirectory(int a, String ruleSetPath)
    {
      RuleSetPredictionDirectory[a] = ruleSetPath;
    }

    public static string ReadFromRuleSetPredictionDirectory(int a)
    {
      string contents = File.ReadAllText(@RuleSetPredictionDirectory[a]);

      return contents;
    }

  }
}

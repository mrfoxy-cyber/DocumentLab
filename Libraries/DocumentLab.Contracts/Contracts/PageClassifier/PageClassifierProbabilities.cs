using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentLab.Contracts.PageClassifier
{
    public class PageClassifierProbabilities
    {
        int RulesetId { get; set; }
        double Probability { get; set; }

    }
}

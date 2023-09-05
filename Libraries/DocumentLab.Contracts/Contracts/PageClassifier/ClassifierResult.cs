using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentLab.Contracts.PageClassifier
{
    public class ClassifierResult
    {
        public List<PageRule>[] PageRules { get; set; }
    }
}

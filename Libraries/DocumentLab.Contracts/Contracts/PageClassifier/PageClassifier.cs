using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;

namespace DocumentLab.Contracts.PageClassifier
{
    public class PageClassifier
    {
        public List<PageClassifierProbabilities>[] ClassifierProbabilities { get; set; }
        Bitmap Bitmap { get; set; }
    }
}

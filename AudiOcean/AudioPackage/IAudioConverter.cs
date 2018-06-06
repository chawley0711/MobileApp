using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioPackage
{
    public interface IAudioConverter
    {
        void Convert(string inputFilePath, string outputFilePath);
    }
}

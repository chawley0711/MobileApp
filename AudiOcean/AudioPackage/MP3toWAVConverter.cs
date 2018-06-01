using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;

namespace AudioPackage
{
    public class MP3toWAVConverter : IAudioConverter
    {
        public void Convert(string inputFilePath, string outputFilePath)
        {
            using (Mp3FileReader m = new Mp3FileReader(inputFilePath))
            {
                using (WaveStream w = WaveFormatConversionStream.CreatePcmStream(m))
                {
                    WaveFileWriter.CreateWaveFile(outputFilePath, w);
                }
            }
        }
    }
}

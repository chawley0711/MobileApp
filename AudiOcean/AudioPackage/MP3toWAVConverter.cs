using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;

namespace AudioPackage
{
    public class MP3toWAVConverter// : IAudioConverter
    {
        public static Stream Convert(byte[] inputFilePath, string outputFilePath)
        {
            MemoryStream ms = new MemoryStream(inputFilePath);
            Mp3FileReader m = new Mp3FileReader(ms);
 
            //using ()
            //{
            //using ()
            //{
            WaveStream w = WaveFormatConversionStream.CreatePcmStream(m);
            //WaveFileWriter.CreateWaveFile(outputFilePath, w);
            return w;
                //}
            }
        }
    }


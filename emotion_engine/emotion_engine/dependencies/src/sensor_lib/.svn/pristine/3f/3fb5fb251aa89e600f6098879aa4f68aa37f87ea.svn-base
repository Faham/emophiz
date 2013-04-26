using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SensorLib.Util;

namespace SensorLib.Filters
{
    public class Fft
    {

        public static Complex[] ConvertToFreq(double[] timeData)
        {

            //Get the size of next power of 2
            int length = timeData.Length;
            int root = (int)Math.Log(length, 2);
            root += 1;
            int paddedLength = (int)Math.Pow(2, root);

            //Create an array that is a power of 2 and copy the values over.
            //Pad the end of the array with zeros
            double[] paddedData = new double[paddedLength];
            for(int i=0; i<paddedLength; i++)
            {
                if(i < length)
                    paddedData[i] = timeData[i];
                else
                    paddedData[i] = 0;
            }

            //Do the FFT
            Lomont.LomontFFT lomontFft = new Lomont.LomontFFT();
            lomontFft.RealFFT(paddedData, true);

            //Convert everything to the complex data type
            Complex[] freqData = new Complex[paddedLength / 2];
            for (int i = 0; i < freqData.Length; i++)
            {
                freqData[i] = new Complex(paddedData[i], paddedData[i + 1]);
            }

            return freqData;
        }

        
        /*
        static Complex ConvertToTime()
        {

        }
        */

    }
}

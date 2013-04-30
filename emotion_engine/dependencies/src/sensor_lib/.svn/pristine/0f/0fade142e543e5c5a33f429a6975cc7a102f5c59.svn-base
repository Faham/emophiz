using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Diagnostics;

using SensorLib.Util;

namespace SensorLib.Filters
{

    /// <summary>
    /// A generic filter transforms the data according a specified function.
    /// </summary>
    public class GenericFilter<T> : Filter
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="transformation">The transformation to apply.  The parameters are (input, filter data, output).</param>
        /// <param name="samples"></param>
        public GenericFilter(Func<float, float[], float> transformation, int samples)
        {
            this.transformation = transformation;
            this.data = new float[samples];
            return;
        }

        public override bool IsRealTime { get { throw new NotImplementedException(); } }

        //!!Are the values that get put into data the outputs or inputs?
        public override float[] FilterData(float[] input)
        {
            float[] output = new float[input.Length];

            throw new NotImplementedException();
            /*
            for (int i = 0; i < input.Length; i++)
            {
                // Get data for transformation
                float[] transformationData = data.Skip(i).Skip(data.Length - i).Take(data.Length - i).ToArray();

                // Transform data
                output[i] = transformation(transformationData);
            }

            // Set data
            data.Skip(input.Length).Take(input.Length - input.Length).ToArray();
            */
            return output;
        }

        public override void Reset()
        {
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = 0;
            }

            return;
        }

        readonly Func<float, float[], float> transformation;
        readonly float[] data;




        static public float[] Square(float[] data)
        {
            if (data == null)
                return null;

            float[] newData = new float[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                newData[i] = data[i] * data[i];
            }
            return newData;
        }

        public float[] FilterData(float value)
        {
            float[] data = { value };
            return FilterData(data);
        }


    }
}

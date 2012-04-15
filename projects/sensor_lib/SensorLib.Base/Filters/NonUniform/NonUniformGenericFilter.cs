using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Diagnostics;

using SensorLib.Util;

namespace SensorLib.Filters.NonUniform
{
    /*
    /// <summary>
    /// A non-uniform generic filter transforms non-uniformally spaced data according a specified function.
    /// </summary>
    public class NonUniformGenericFilter<T> : Filter
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="transformation">The transformation to apply.  The parameters are (input, filter data, output).</param>
        /// <param name="samples"></param>
        public NonUniformGenericFilter(Func<Sample<T>, Sample<T>[], Sample<T>> transformation, TimeSpan filterTimeSpan)
        {
            this.transformation = transformation;
            this.data = new Queue<Sample<T>>();
            return;
        }

        public override Sample<T>[] FilterData(Sample<T>[] input)
        {
            Sample<T>[] output = new Sample<T>[input.Length];

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
            *//*
            return output;
        }

        public override void Reset()
        {
            data.Clear();
            return;
        }

        readonly Func<Sample<T>, Sample<T>[], Sample<T>> transformation;
        readonly Queue<Sample<T>> data;

    }
               * */
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Diagnostics;

using SensorLib.Filters;

namespace SensorLib
{
    
    class ZeroPoleFilter_old : Filter
    {
        public ZeroPoleFilter_old(float[] zeros_, float[] poles_)
        {
            zeros = zeros_;
            poles = poles_;

            dataZeros = new Queue<double>(zeros.Length);
            dataPoles = new Queue<double>(poles.Length-1);

            InitializeQueue(dataZeros, zeros.Length);
            InitializeQueue(dataPoles, poles.Length-1);

            return;
        }


        public override void Reset()
        {
            InitializeQueue(dataZeros, zeros.Length);
            InitializeQueue(dataPoles, poles.Length);
            return;
        }

        public override float[] FilterData(float[] data)
        {
            if (data == null)
                return null;

            float[] newData = new float[data.Length];
            int elementNum = 0;

            foreach (float value in data)
            {

                //Zeros
                double zeroOutput = 0.0f;

                dataZeros.Enqueue(value * 0.00001); //!!
                dataZeros.Dequeue();

                Queue<double>.Enumerator zeroEnumerator = dataZeros.GetEnumerator();
                Debug.Assert(zeros.Length == dataZeros.Count);
                int zerosLength = zeros.Length;
                for (int i = 0; i < data.Length && zeroEnumerator.MoveNext(); i++)
                {
                    zeroOutput += zeros[(zerosLength-1) - i] * zeroEnumerator.Current;
                }



                //Poles
                double output = zeroOutput;  //mid-gain

                Queue<double>.Enumerator poleEnumerator = dataPoles.GetEnumerator();
                Debug.Assert(dataPoles.Count == poles.Length - 1);
                int polesLength = poles.Length;
                for (int i = 0; (i < poles.Length-1) && poleEnumerator.MoveNext(); i++)
                {
                    output += poles[(polesLength-1) - i] * poleEnumerator.Current;
                }
                output *= poles[0];

                dataPoles.Enqueue(output);
                dataPoles.Dequeue();

                //Add to output array
                Debug.Assert(!double.IsInfinity(output));
                Debug.Assert(!float.IsInfinity((float)output));
                newData[elementNum] = (float)output;
                elementNum++;
            }

            return newData;
        }


        void InitializeQueue(Queue<double> queue, int size)
        {
            queue.Clear();
            for (int i = 0; i < size; i++)
                queue.Enqueue(0.0f);
            return;
        }


        readonly Queue<double> dataZeros;
        readonly Queue<double> dataPoles;

        readonly float[] zeros;
        readonly float[] poles;
    }
}

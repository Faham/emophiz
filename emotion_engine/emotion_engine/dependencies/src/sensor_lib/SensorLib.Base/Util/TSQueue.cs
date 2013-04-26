using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using System.Diagnostics;


namespace SensorLib.Util
{
    //!!Could use timeout and max size
    //Enqueueing does not wait because we would rather drop packets then block the thread
    public class TSQueue<Type>
    {
        public TSQueue()
        {
            queue = new Queue<Type>();
            return;
        }

        public void Clear()
        {
            lock (queue)
            {
                queue.Clear();
            }
            return;
        }

        public bool Enqueue(Type element)
        {
            lock (queue)
            {
                if (queue.Count + 1 > MAX_SIZE)
                    return false;

                queue.Enqueue(element);
                Monitor.Pulse(queue);
            }
            return true;
        }

        public bool Enqueue(Type[] elements)
        {
            Debug.Assert(elements.Length != 0);

            //Check state of queue
            lock(queue)
            {
                if (queue.Count + elements.Length > MAX_SIZE)
                    return false;

                foreach(Type element in elements)
                    queue.Enqueue(element);

                Monitor.Pulse(queue);
            }
            
            return true;
        }

        public Type Dequeue()
        {
            Type element;
            lock (queue)
            {
                if (queue.Count == 0)
                    Monitor.Wait(queue);
                
                if (threadExit)
                    return default(Type);

                Debug.Assert(queue.Count != 0);

                element = queue.Dequeue();
            }
            return element;
        }

        public Type[] Dequeue(int max)
        {
            Type[] elements;
            lock (queue)
            {
                int num = Math.Min(queue.Count, max);
                if (num == 0)
                    Monitor.Wait(queue);

                if (threadExit)
                    return null;

                num = Math.Min(queue.Count, max);
                Debug.Assert(num != 0);

                elements = new Type[num];
                for (int i = 0; i < num; i++)
                    elements[i] = queue.Dequeue();
            }

            return elements;
        }

        //Should be called when we want to end the threads waiting in the queue
        public void SignalExit()
        {
            lock (queue)
            {
                threadExit = true;
                Monitor.PulseAll(queue);
            }
            return;
        }

        bool threadExit = false;
        Queue<Type> queue;
        const long MAX_SIZE = 65000;

    }

}

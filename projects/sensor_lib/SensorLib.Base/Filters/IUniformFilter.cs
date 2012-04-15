using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Diagnostics;

using SensorLib.Util;

namespace SensorLib.Filters
{

    /// <summary>
    /// Transforms uniformly spaced data.
    /// </summary>
    public interface IUniformFilter
    {

        //!!public IEnumerable<T> FilterData(IEnumerable<T> data);
        float[] FilterData(float[] data);

    }
}

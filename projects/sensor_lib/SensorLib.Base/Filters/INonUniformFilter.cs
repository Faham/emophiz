using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Diagnostics;

using SensorLib.Util;

namespace SensorLib.Filters
{

    /// <summary>
    /// Transforms non-uniformly spaced data.
    /// </summary>
    public interface INonUniformFilter
    {

        //!!public IEnumerable<Sample<T>> FilterData(IEnumerable<Sample<T>> data);
        Sample<float>[] FilterData(Sample<float>[] data);

    }
}

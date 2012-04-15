using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SensorLib.Nomenclature
{


    /// <summary>
    /// A real-time filter is a filter in which you can look at the output signal before 
    /// you have received all the input signal.  The output signal is not dependent on the 
    /// way you split up the input signal.  Since the input signal can be filtered in parts, 
    /// real-time filters are ideal for applications where you need to use the output signal 
    /// before receiving all of the input signal.
    /// </summary>
    public static class RealTimeFilter { }


    /// <summary>
    /// A bulk filter is a filter in which all the data must be filtered in one piece.
    /// Put another way, the output signal is dependent on how you split up the signal.
    /// This makes bulk filters unusable for real-time signal processing in which you are
    /// looking at the output signal before you have recieved all the input signal.
    /// An example of a bulk filter is a normalizer in which it normalizes the signal by 
    /// the maximum value found in the signal.
    /// </summary>
    public static class BulkFilter { }


    /// <summary>
    /// An IIR (Infinite Impulse Response) filter is a filter which includes feedback.
    /// These filters are better than FIR (Finite Impulse Response) filters in that their filter length 
    /// (and thus delay time) is much less.  IIR filters are not linear-phase filters, though, so they can 
    /// skew a signal when filtering it.  If you want to filter data with an IIR filter and preserve the 
    /// signal shape, then you must filter the signal from front to back and then again from back to front.
    /// This is called bidirectional filtering.
    /// </summary>
    public static class IIRFilter { }


    /// <summary>
    /// An FIR (Finite Impulse Response) filter is a filter which does not include feedback.
    /// These filters usually have much larger length (and thus delay) than IIR filters, but
    /// FIR filters have linear phase which means they preserve the shape of a signal that is being filtered.
    /// This can be useful in real-time processing applications where you want to preserve the signal 
    /// but don't have all the data to do bidirectional filtering.
    /// </summary>
    public static class FIRFilter { }


    /// <summary>
    /// A Butterworth filter is a non-linear IIR filter which is maximally flat (no ripple) in the pass and stop bands.
    /// </summary>
    public static class ButterworthFilter { }


    /// <summary>
    /// A Chebyshev Type I filter is a non-linear IIR filter which has ripple in the pass band and is maximally flat (no ripple) in the stop band.
    /// This filter is steeper than the Butterworth filter and only a bit steeper than the Chebyshev Type II filter.
    /// </summary>
    public static class ChebyshevTypeIFilter { }


    /// <summary>
    /// A Chebyshev Type II filter is a non-linear IIR filter which is maximally flat (no ripple) in the pass band and has ripple in the stop band.
    /// This filter is steeper than the Butterworth filter and a bit more gradual than the Chebyshev Type I filter.
    /// </summary>
    public static class ChebyshevTypeIIFilter { }


}

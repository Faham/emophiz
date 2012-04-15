
/* Used code snippets from http://msdn.microsoft.com/en-us/library/6fbs5e2h.aspx */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Diagnostics;

namespace SensorLib.Util
{

    //Note: Optimized for speed, not memory.  Will save the components in the coordinate type last set by user.

    /// <summary>
    /// A complex number.  Optimized for speed, not memory.
    /// </summary>
    public struct Complex
    {

        /// <summary>
        /// Specifies the real component.
        /// </summary>
        public double Real
        {
            get
            {
                switch(components.Coordinates)
                {
                    case Coordinates.Cartesian: return components.Real;
                    case Coordinates.Polar: return GetReal(components.Magnitude, components.Angle);
                    default: Debug.Assert(false); break;
                }

                return double.NaN;
            }
            
            set
            {
                switch (components.Coordinates)
                {
                    case Coordinates.Cartesian: components.Real = value; break;
                    case Coordinates.Polar: SetCartesian(value, GetImaginary(components.Magnitude, components.Angle)); break;
                    default: Debug.Assert(false); break;
                }

                return;
            }

        }


        /// <summary>
        /// Specifies the imaginary component.
        /// </summary>
        public double Imaginary
        {
            get
            {
                switch (components.Coordinates)
                {
                    case Coordinates.Cartesian: return components.Imaginary;
                    case Coordinates.Polar: return GetImaginary(components.Magnitude, components.Angle);
                    default: Debug.Assert(false); break;
                }

                return double.NaN;
            }
            
            set
            {
                switch (components.Coordinates)
                {
                    case Coordinates.Cartesian: components.Imaginary = value; break;
                    case Coordinates.Polar: SetCartesian(GetReal(components.Magnitude, components.Angle), value); break;
                    default: Debug.Assert(false); break;
                }

                return;
            }

        }

        /// <summary>
        /// Specifies the magnitude component.  A negative magnitude will increase the angle field.
        /// </summary>
        public double Magnitude
        {
            get
            {
                switch (components.Coordinates)
                {
                    case Coordinates.Cartesian: return GetMagnitude(components.Real, components.Imaginary);
                    case Coordinates.Polar: return components.Magnitude;
                    default: Debug.Assert(false); break;
                }

                return double.NaN;
            }
            
            set
            {
                switch(components.Coordinates)
                {
                    case Coordinates.Cartesian: SetPolar(value, GetAngle(components.Real, components.Imaginary)); break;
                    case Coordinates.Polar: components.Magnitude = value; break;
                    default: Debug.Assert(false); break;
                }

                return;
            }

        }

        /// <summary>
        /// Gets the magnitude component squared.
        /// </summary>
        public double MagnitudeSquared { get { return Magnitude * Magnitude; } }

        /// <summary>
        /// Gets the square root.  The other root is -1 * (square root).
        /// </summary>
        public Complex SquareRoot
        {
            get
            {
                switch (components.Coordinates)
                {
                    case Coordinates.Cartesian:
                    {
                        double r = Magnitude;
                        int sign = (Imaginary == 0) ? 1 : Math.Sign(Imaginary);
                        return new Complex(Math.Sqrt((r + Real) / 2), sign * Math.Sqrt((r - Real) / 2));
                    }
                    case Coordinates.Polar:
                    {
                        return Complex.FromPolar(Math.Sqrt(Magnitude), Angle / 2);
                    }
                    default: Debug.Assert(false); break;
                }

                return Complex.Zero;
            }
        }

        /// <summary>
        /// Specifies the angle component in radians.  Will wrap values to between 0 and 2*PI.
        /// </summary>
        public double Angle
        {
            get
            {
                switch (components.Coordinates)
                {
                    case Coordinates.Cartesian: return GetAngle(components.Real, components.Imaginary);
                    case Coordinates.Polar: return components.Angle;
                    default: Debug.Assert(false); throw new NotImplementedException();
                }
            }
            
            set
            {
                switch (components.Coordinates)
                {
                    case Coordinates.Cartesian: SetPolar(GetMagnitude(components.Real, components.Imaginary), value); break;
                    case Coordinates.Polar: components.Angle = value; break;
                    default: Debug.Assert(false); throw new NotImplementedException();
                }
            }

        }

        /// <summary>
        /// Returns the complex conjugate.
        /// </summary>
        public Complex Conjugate
        {
            get { return new Complex(Real, -Imaginary); }
        }



        /// <summary>
        /// Determines whether a complex number is purely real.
        /// </summary>
        public bool IsReal
        {
            get { return Imaginary == 0; }
        }


        /// <summary>
        /// Determines whether a complex number is purely real based on a tolerance.
        /// </summary>
        public bool IsRealWithTolerance
        {
            get { return Math.Abs(Imaginary) < tolerance; }
        }


        public static Complex Zero { get { return new Complex(0, 0); } }
        public static Complex One { get { return new Complex(1, 0); } }
        public static Complex Eye { get { return new Complex(0, 1); } }

        public override bool Equals(object obj)
        {
            return this == (Complex)obj;
        }

        public override int GetHashCode()
        {
            return Magnitude.GetHashCode();
        }


        #region Operator Overloads

        public static Complex operator +(Complex c1, Complex c2)
        {
            return new Complex(c1.Real + c2.Real, c1.Imaginary + c2.Imaginary);
        }

        public static Complex operator +(double num, Complex c)
        {
            return new Complex(num + c.Real, c.Imaginary);
        }

        public static Complex operator +(Complex c, double num)
        {
            return num + c;
        }

        public static Complex operator -(Complex c1, Complex c2)
        {
            return new Complex(c1.Real - c2.Real, c1.Imaginary - c2.Imaginary);
        }

        public static Complex operator -(double num, Complex c)
        {
            return new Complex(num, 0) - c;
        }

        public static Complex operator -(Complex c, double num)
        {
            return new Complex(c.Real - num, c.Imaginary);
        }

        public static Complex operator *(Complex c1, Complex c2)
        {
            switch(c1.components.Coordinates)
            {
                case Coordinates.Cartesian:
                {
                    double a = c1.Real;
                    double b = c1.Imaginary;
                    double c = c2.Real;
                    double d = c2.Imaginary;

                    return new Complex(a*c - b*d, a*d + b*c);
                }
                
                case Coordinates.Polar: return Complex.FromPolar(c1.Magnitude * c2.Magnitude, c1.Angle + c2.Angle);

                default: Debug.Assert(false); throw new Exception();
            }
        }

        public static Complex operator *(double scale, Complex c)
        {
            switch (c.components.Coordinates)
            {
                case Coordinates.Cartesian: return new Complex(scale * c.Real, scale * c.Imaginary);
                case Coordinates.Polar: return Complex.FromPolar(scale * c.Magnitude, c.Angle);
                default: Debug.Assert(false); throw new Exception();
            }
        }

        public static Complex operator *(Complex c, double scale)
        {
            return scale * c;
        }

        public static Complex operator -(Complex c)
        {
            switch (c.components.Coordinates)
            {
                case Coordinates.Cartesian: return new Complex(-c.Real, -c.Imaginary);
                case Coordinates.Polar: return Complex.FromPolar(c.Magnitude, c.Angle + Math.PI);
                default: Debug.Assert(false); throw new Exception();
            }
        }

        public static Complex operator /(Complex c, double scale)
        {
            return (1 / scale) * c;
        }

        
        public static Complex operator /(Complex c1, Complex c2)
        {
            //Complex division is the multiplication of the numerator and denominator by the complex conjugate of the denominator.
            double a = c1.Real;
            double b = c1.Imaginary;
            double c = c2.Real;
            double d = c2.Imaginary;

            Complex numerator = new Complex( (a*c + b*d), (b*c - a*d));
            double denominator = c*c + d*d;

            return numerator / denominator;
        }

        public static Complex operator /(double num, Complex c)
        {
            return new Complex(num,0) / c;
        }


        public static bool operator ==(Complex c1, Complex c2)
        {
            switch (c1.components.Coordinates)
            {
                case Coordinates.Cartesian: return (c1.components.Real == c2.Real) && (c1.components.Imaginary == c2.Imaginary);
                case Coordinates.Polar: return (c1.components.Magnitude == c2.Magnitude) && (c1.components.Angle == c2.Angle);
                default: Debug.Assert(false); throw new SystemException();
            }
        }

        public static bool operator !=(Complex c1, Complex c2)
        {
            return !(c1 == c2);
        }

        #endregion


        #region String Methods

        public String ToStringCartesian()
        {
            return ToStringCartesian("0.0000");
        }

        public String ToStringCartesian(String numberFormat)
        {
            int sign = Math.Sign(Imaginary);
            
            String signString;
            if (sign == 1 || sign == 0)
                signString = "+";
            else
                signString = "-";

            return String.Format("{0:" + numberFormat + "}{1}i{2:" + numberFormat + "}", Real, signString, Math.Abs(Imaginary));
        }



        public String ToStringPolar()
        {
            return ToStringPolar("0.0000");
        }

        public String ToStringPolar(String numberFormat)
        {
            return String.Format("({0:" + numberFormat + "}, {1:" + numberFormat + "})", Magnitude, Angle);
        }

        


        public override String ToString()
        {
            return ToString("0.0000");
        }

        public String ToString(String numberFormat)
        {
            switch (components.Coordinates)
            {
                case Coordinates.Cartesian: return ToStringCartesian(numberFormat);
                case Coordinates.Polar: return ToStringPolar(numberFormat);
                default: Debug.Assert(false); throw new Exception();
            }
        }

        #endregion


        #region Creators

        /// <summary>
        /// Creates a complex number in cartesian coordinates.
        /// </summary>
        /// <param name="real"> The real component. </param>
        /// <param name="imaginary"> The imaginary component. </param>
        public Complex(double real, double imaginary)
        {
            //Initialize variables
            components = new Union();

            //Set the complex value
            SetCartesian(real, imaginary);

            return;
        }

        /// <summary>
        /// Creates a complex number from an existing one.
        /// </summary>
        /// <param name="complex"></param>
        public Complex(Complex complex)
        {
            //Initialize variables
            components = new Union();

            switch (complex.components.Coordinates)
            {
                case Coordinates.Cartesian: SetCartesian(complex.Real, complex.Imaginary); break;
                case Coordinates.Polar: SetPolar(complex.Magnitude, complex.Angle); break;
                default: Debug.Assert(false); break;
            }

            return;
        }


        /// <summary>
        /// Creates a complex number in polar coordinates.
        /// </summary>
        /// <param name="real"> The magnitude component. </param>
        /// <param name="imaginary"> The angle component. </param>
        public static Complex FromPolar(double magnitude, double angle)
        {
            Complex complex = new Complex(0.0, 0.0);
            complex.SetPolar(magnitude, angle);
            return complex;
        }

        #endregion



        #region Private Methods

        void SetCartesian(double real, double imaginary)
        {
            components.Coordinates = Coordinates.Cartesian;

            components.Real = real;
            components.Imaginary = imaginary;
            
            return;
        }

        void SetPolar(double magnitude, double angle)
        {
            components.Coordinates = Coordinates.Polar;

            //Angle needs to be set first or else we will overwrite the offset if magnitude is negative.
            components.Angle = angle;
            components.Magnitude = magnitude;

            return;
        }



        #region Conversion Methods

        static double GetAngle(double real, double imaginary)
        {
            return Math.Atan2(imaginary, real);
        }

        static double GetMagnitude(double real, double imaginary)
        {
            return Math.Sqrt(real*real + imaginary*imaginary);
        }

        static double GetReal(double magnitude, double angle)
        {
            return magnitude * Math.Cos(angle);
        }

        static double GetImaginary(double magnitude, double angle)
        {
            return magnitude * Math.Sin(angle);
        }

        #endregion

        #endregion


        
        const double tolerance = 1e-10;


        Union components;



        enum Coordinates
        {
            Cartesian,
            Polar
        }


        struct Union
        {

            public double Real
            {
                get
                {
                    if(Coordinates != Coordinates.Cartesian)
                        Debug.Assert(false);
                    
                    return value1;
                }

                set
                {
                    if(Coordinates != Coordinates.Cartesian)
                        Debug.Assert(false);

                    value1 = value;
                }
            }

            public double Imaginary
            {
                get
                {
                    if(Coordinates != Coordinates.Cartesian)
                        Debug.Assert(false);
                    
                    return value2;
                }

                set
                {
                    if(Coordinates != Coordinates.Cartesian)
                        Debug.Assert(false);

                    value2 = value;
                }
            }


            public double Magnitude
            {
                get
                {
                    if(Coordinates != Coordinates.Polar)
                        Debug.Assert(false);
                    
                    return value1;
                }

                set
                {
                    if(Coordinates != Coordinates.Polar)
                        Debug.Assert(false);

                    if (value < 0)
                        Angle += Math.PI;

                    value1 = Math.Abs(value);
                }
            }



            public double Angle
            {
                get
                {
                    if(Coordinates != Coordinates.Polar)
                        Debug.Assert(false);
                    
                    return value2;
                }

                set
                {
                    if(Coordinates != Coordinates.Polar)
                        Debug.Assert(false);

                    value2 = value % (2 * Math.PI);
                }
            }


            public Coordinates Coordinates { get; set; }

            //NOTE: These should never be accessed directly except by 'real', 'imaginary', 'magnitude', and 'angle'!!!
            double value1;
            double value2;

        }
        
        

    }
}

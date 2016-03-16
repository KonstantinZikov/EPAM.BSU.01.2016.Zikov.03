using System;
using System.Text;
using static System.Math;

namespace Task1
{
    public sealed class Polynomial : IFormattable
    {
        private const double ACCURACY = 1E-8;


        /// <summary>
        /// Returns the max power in polynomial + 1;
        /// </summary>
        public int PowerCount
        {
            get { return _multiplers.Length; }
        }

        /// <summary>
        /// Returns the array, which contains multiplers of current polynomial in order from lower to greater powers. 
        /// </summary>
        public double[] Multiplers
        {
            get { return (double[])_multiplers.Clone(); }
        }

        /// <summary>
        /// Return the multipler of selected power.
        /// </summary>
        /// <param name="index">The power, which multipler will be returned.</param>
        /// <returns>multipler</returns>
        public double this[int power]
        {
            get
            {
                if (power < 0)
                {
                    throw new ArgumentOutOfRangeException("power must be 0 or positive.");
                }
                if (power >= PowerCount)
                {
                    return 0.0;
                }
                return _multiplers[power];
            }
        }

        private double[] _multiplers;

        /// <summary>
        /// Create the polynomial from multiplers array in order from lower to greater powers.
        /// </summary>
        /// <param name="multiplers">multiplers of creating polynomial</param>
        public Polynomial(params double[] multiplers)
        {
            if (multiplers == null)
            {
                throw new ArgumentNullException("multiplers is null");
            }
            int i;
            for (i = multiplers.Length; i --> 0;)
            {
                if (Abs(multiplers[i]) > ACCURACY) break;
            }
            _multiplers = new double[i + 1];
            Array.Copy(multiplers, _multiplers, i + 1);
        }

        #region Operators

        static public Polynomial operator +(Polynomial lhv, Polynomial rhv)
        {
            return Add(lhv, rhv);
        }

        static public Polynomial operator -(Polynomial lhv, Polynomial rhv)
        {
            return Substract(lhv, rhv);
        }

        static public Polynomial operator *(Polynomial lhv, Polynomial rhv)
        {
            return Multiply(lhv, rhv);
        }

        static public Polynomial Add(Polynomial lhv, Polynomial rhv)
        {
            if (lhv == null) throw new ArgumentNullException("lhv is null");
            if (rhv == null) throw new ArgumentNullException("rhv is null");

            return lhv.ElementOperation(rhv, (l, r) => l + r);
        }

        static public Polynomial Substract(Polynomial lhv, Polynomial rhv)
        {
            if (lhv == null) throw new ArgumentNullException("lhv is null");
            if (rhv == null) throw new ArgumentNullException("rhv is null");

            return lhv.ElementOperation(rhv, (l, r) => l - r);
        }

        static public Polynomial Multiply(Polynomial lhv, Polynomial rhv)
        {
            if (lhv == null) throw new ArgumentNullException("lhv is null");
            if (rhv == null) throw new ArgumentNullException("rhv is null");

            double[] multiplers = new double[lhv._multiplers.Length + rhv._multiplers.Length];
            for (int i = 0; i < lhv._multiplers.Length; i++)
            {
                for (int j = 0; j < rhv._multiplers.Length; j++)
                {
                    multiplers[i + j] += lhv._multiplers[i] * rhv._multiplers[j];
                }
            }
            return new Polynomial(multiplers);
        }

        /// <summary>
        /// Apply the specified operation to all multiplers in polynomial.
        /// </summary>
        /// <param name="polynomial"></param>
        /// <param name="operation"></param>
        /// <returns>New polynomial with changed elements.</returns>
        public Polynomial ElementOperation(Func<double,double> operation)
        {
            if (operation == null) throw new ArgumentNullException("operation is null");

            double[] multiplers = Multiplers;
            for (int i = multiplers.Length; i --> 0;)
            {
                multiplers[i] = operation(multiplers[i]);
            }
            return new Polynomial(multiplers);
        }

        /// <summary>
        /// Apply the specified operation between multiplers pairs in two polynomials.
        /// </summary>
        /// <param name="polynomial"></param>
        /// <param name="operation"></param>
        /// <returns>New polynomial with changed elements.</returns>
        public Polynomial ElementOperation(Polynomial polynomial, Func<double, double, double> operation)
        {
            if (polynomial == null) throw new ArgumentNullException("polynomial is null");

            double[] multiplers = new double[Max(PowerCount, polynomial.PowerCount)];
            for (int i = multiplers.Length; i --> 0;)
            {
                multiplers[i] = operation(_multiplers[i], polynomial[i]);
            }
            return new Polynomial(multiplers);
        }

        #endregion

        #region Object methods

        public override bool Equals(object obj)
        {
            if (obj?.GetType() != typeof(Polynomial)) return false;
            return Equals((Polynomial)obj);
        }

        public bool Equals(Polynomial polynomial) 
        {         
            if (PowerCount != polynomial?.PowerCount) return false;
            for (int i = 0; i < _multiplers.Length; i++)
            {
                if (Abs(_multiplers[i] - polynomial._multiplers[i]) > ACCURACY) return false;
            }
            return true;
        }

        public override int GetHashCode()
        {
            int result = 0;
            for (int i = 0; i < _multiplers.Length; i++)
            {
                result ^= _multiplers[i].GetHashCode();
            }
            return result;
        }

        public override string ToString()
        {
            return ToString("XF0");
        }


        /// <summary>
        /// Returns the formatted string view of the polynomial, using specified format provider.
        /// </summary>
        /// <param name="format">Format string. First letter used as a variable name in polynomial.
        /// Other symbols apply to format polynomial multiplers.
        /// </param>
        /// <param name="provider"></param>
        /// <returns></returns>
        public string ToString(string format, IFormatProvider provider = null)
        {
            if (string.IsNullOrEmpty(format))
            {
                format = "XF0";
            }
            char formatLetter = format[0];
            format = format.Substring(1);
            var sb = new StringBuilder();
            for (int i = _multiplers.Length; i --> 0;)
            {
                if (Abs(_multiplers[i]) > ACCURACY)
                {
                    if (_multiplers[i] > 0.0 && i != _multiplers.Length-1)
                    {
                        sb.Append('+');
                    }
                    sb.Append(_multiplers[i].ToString(format,provider));
                    if (i != 0)
                    {
                        sb.Append(formatLetter);
                        if (i != 1)
                        {
                            sb.Append('^');
                            sb.Append(i);
                        }
                    }
                }
            }
            return sb.ToString();
        }

        #endregion    
    }
}

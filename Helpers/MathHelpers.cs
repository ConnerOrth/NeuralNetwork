using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers
{
    public static class MathHelpers
    {
        /// <summary>
        ///   Computes the Softmax function (also known as normalized Exponencial
        ///   function) that "squashes"a vector or arbitrary real values into a 
        ///   vector of real values in the range (0, 1) that add up to 1.
        /// </summary>
        /// 
        /// <param name="input">The real values to be converted into the unit interval.</param>
        /// 
        /// <returns>A vector with the same number of dimensions as <paramref name="input"/>
        ///   but where values lie between 0 and 1.</returns>
        ///   
#if NET45 || NET46 || NET462 || NETSTANDARD2_0
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static double[] Softmax(double[] input)
        {
            return Softmax(input, new double[input.Length]);
        }

        /// <summary>
        ///   Computes the Softmax function (also known as normalized Exponencial
        ///   function) that "squashes"a vector or arbitrary real values into a 
        ///   vector of real values in the range (0, 1) that add up to 1.
        /// </summary>
        /// 
        /// <param name="input">The real values to be converted into the unit interval.</param>
        /// <param name="result">The location where to store the result of this operation.</param>
        /// 
        /// <returns>A vector with the same number of dimensions as <paramref name="input"/>
        ///   but where values lie between 0 and 1.</returns>
        ///   
#if NET45 || NET46 || NET462 || NETSTANDARD2_0
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static double[] Softmax(double[] input, double[] result)
        {
            double sum = MathHelpers.LogSumExp(input);

            for (int i = 0; i < input.Length; i++)
                result[i] = Math.Exp(input[i] - sum);

            return result;
        }
        /// <summary>
        ///   Computes sum(x) without losing precision using ln(x_0) ... ln(x_n).
        /// </summary>
        /// 
#if NET45 || NET46 || NET462 || NETSTANDARD2_0
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static double LogSumExp(this double[] array)
        {
            double sum = Double.NegativeInfinity;
            for (int i = 0; i < array.Length; i++)
                sum = MathHelpers.LogSum(array[i], sum);
            return sum;
        }

        /// <summary>
        ///   Computes x + y without losing precision using ln(x) and ln(y).
        /// </summary>
        /// 
#if NET45 || NET46 || NET462 || NETSTANDARD2_0
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static double LogSum(double lnx, double lny)
        {
            if (lnx == Double.NegativeInfinity)
                return lny;
            if (lny == Double.NegativeInfinity)
                return lnx;

            if (lnx > lny)
                return lnx + MathHelpers.Log1p(Math.Exp(lny - lnx));

            return lny + MathHelpers.Log1p(Math.Exp(lnx - lny));
        }

        /// <summary>
        ///   Computes x + y without losing precision using ln(x) and ln(y).
        /// </summary>
        /// 
#if NET45 || NET46 || NET462 || NETSTANDARD2_0
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static double LogSum(float lnx, float lny)
        {
            if (lnx == Single.NegativeInfinity)
                return lny;
            if (lny == Single.NegativeInfinity)
                return lnx;

            if (lnx > lny)
                return lnx + MathHelpers.Log1p(Math.Exp(lny - lnx));

            return lny + MathHelpers.Log1p(Math.Exp(lnx - lny));
        }

        /// <summary>
        ///   Computes x + y without losing precision using ln(x) and ln(y).
        /// </summary>
        /// 
#if NET45 || NET46 || NET462 || NETSTANDARD2_0
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static double LogSum(double[] values)
        {
            double logsum = Double.NegativeInfinity;
            for (int i = 0; i < values.Length; i++)
                logsum = MathHelpers.LogSum(logsum, values[i]);
            return logsum;
        }

        /// <summary>
        ///   Computes log(1+x) without losing precision for small values of x.
        /// </summary>
        /// 
        /// <remarks>
        ///   References:
        ///   - http://www.johndcook.com/csharp_log_one_plus_x.html
        /// </remarks>
        /// 
#if NET45 || NET46 || NET462 || NETSTANDARD2_0
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static double Log1p(double x)
        {
            if (x <= -1.0)
                return Double.NaN;

            if (Math.Abs(x) > 1e-4)
                return Math.Log(1.0 + x);

            // Use Taylor approx. log(1 + x) = x - x^2/2 with error roughly x^3/3
            // Since |x| < 10^-4, |x|^3 < 10^-12, relative error less than 10^-8
            return (-0.5 * x + 1.0) * x;
        }
    }
}

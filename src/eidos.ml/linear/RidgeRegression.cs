using System;
using System.Linq;
using eidos.ml.abstractions;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.Statistics;

namespace eidos.ml.linear
{
    public class RidgeRegression : IRegression<RidgeRegression, RidgeRegressionEstimator>
    {
        public bool Intercept { get; }

        public double Alpha { get; }

        public RidgeRegression(bool intercept = true, double alpha = 1.0)
        {
            Intercept = intercept;
            Alpha = alpha;
        }

        public RidgeRegressionEstimator Fit(Matrix<double> x, Vector<double> y)
        {
            var (x1, y1, xMean, yMean) = CenterData(x, y);
            (x, y) = (x1, y1);
            //if (Intercept)
            //{
            //    x = x.InsertColumn(0, Vector<double>.Build.Dense(x.RowCount, Vector<double>.One));
            //}
            var w = Lsqr(x, y, Alpha);
            if (Intercept)
            {
                var intercept = GetIntercept(xMean, yMean, w);
                var w1 = Vector<double>.Build.Dense(w.Count + 1);
                w.CopySubVectorTo(w1, 0, 1, w.Count);
                w1[0] = intercept;
                w = w1;
                
            }
            return new RidgeRegressionEstimator(this, w);
        }

        private (Matrix<double>, Vector<double>, Vector<double>, double) CenterData(Matrix<double> x, Vector<double> y)
        {
            if (Intercept)
            {
                var xMean = Vector<double>.Build.Dense(x.EnumerateColumns().Select(column => column.Mean()).ToArray());
                var yMean = y.Mean();
                return (Subtract(x, xMean), y - yMean, xMean, yMean);
            }
            else
            {
                return (x, y, Vector<double>.Build.Dense(x.ColumnCount), 0);
            }
        }

        private double GetIntercept(Vector<double> xMean, double yMean, Vector<double> weights)
        {
            return xMean.PointwiseMultiply(weights).Sum() * -1.0 + yMean;
        }
        
        private static Matrix<double> Subtract(Matrix<double> a, Vector<double> b)
        {
            // TODO: do a pull request to Mathnet.Numerics
            var result = a.Clone();
            for (int i = 0; i < a.RowCount; i++)
            {
                for (int j = 0; j < a.ColumnCount; j++)
                {
                    result[i, j] -= b[j];
                }
            }
            return result;
        }

        private static Vector<double> Lsqr(Matrix<double> _x, Vector<double> y, double alpha, bool show = false, bool useSymOrtho = true)
        {
            // Initialize.
            var m = _x.RowCount;
            var n = _x.ColumnCount;
            var A = _x;
            var b = y;
            var damp = Math.Sqrt(alpha);
            var atol = 1e-3;
            var btol = 1e-3;
            var conlim = 1e+8;
            var itnlim = (int?) null;
            var explicitA = true;

            var wantvar = true;
            Vector<double> var = null;
            if (wantvar)
            {
                var = Vector<double>.Build.Dense(n, 0.0);
            }

            var msg = new[]
            {
                "",
                "The exact solution is  x = 0                              ",
                "Ax - b is small enough, given atol, btol                  ",
                "The least-squares solution is good enough, given atol     ",
                "The estimate of cond(Abar) has exceeded conlim            ",
                "Ax - b is small enough for this machine                   ",
                "The least-squares solution is good enough for this machine",
                "Cond(Abar) seems to be too large for this machine         ",
                "The iteration limit has been reached                      "
            };

            if (show)
            {
                Console.WriteLine(" ");
                Console.WriteLine("LSQR            Least-squares solution of  Ax = b");
                var str1 = $"The matrix A has {m} rows  and {n} cols";
                var str2 = $"damp = {damp}    wantvar = {wantvar}";
                var str3 = $"atol = {atol}                conlim = {conlim}";
                var str4 = $"btol = {btol}                itnlim = {itnlim}";
                Console.WriteLine(str1);
                Console.WriteLine(str2);
                Console.WriteLine(str3);
                Console.WriteLine(str4);
            }

            var itn = 0;
            var istop = 0;
            var ctol = 0.0;
            if (conlim > 0)
                ctol = 1.0 / conlim;
            var aNorm = 0.0;
            var Acond = 0.0;
            var dampsq = Math.Pow(damp, 2);
            var ddnorm = 0.0;
            var res2 = 0.0;
            var xnorm = 0.0;
            var xxnorm = 0.0;
            var z = 0.0;
            var cs2 = -1.0;
            var sn2 = 0.0;

            // Set up the first vectors u and v for the bidiagonalization.
            // These satisfy  beta* u = b, alfa* v = A'u.

            var u = b; //b(1:m); 
            var x = Vector<double>.Build.Dense(n, 0.0);
            var alfa = 0.0;
            var beta = u.L2Norm();
            Vector<double> v = null;
            if (beta > 0)
            {
                u = 1 / beta * u;
                if (explicitA)
                    v = A.Transpose() * u;
                //else
                //{ 
                //    v = A(u, 2);
                //}
                alfa = v.L2Norm();
            }
            Vector<double> w = null;
            if (alfa > 0)
            {
                v = 1 / alfa * v;
                w = v;
            }

            var Arnorm = alfa * beta;
            if (Arnorm == 0)
            {
                Console.WriteLine(msg[1]);
                // The exact solution is  x = 0 
                return Vector<double>.Build.Dense(0, 0.0);
            }

            var rhobar = alfa;
            var phibar = beta;
            var bnorm  = beta;
            var rnorm  = beta;
            var r1norm = rnorm;
            var r2norm = rnorm;
            var head1  = $"   Itn      x[0]       r1norm     r2norm ";
            var head2  = $" Compatible   LS      Norm A   Cond A";

            if (show)
            {
                Console.WriteLine(" ");
                Console.WriteLine(head1, head2);
                var test1 = 1;
                var test2 = alfa / beta;
                var str1 = $"{itn} {x[0]}";
                var str2 = $"{r1norm} {r2norm}";
                var str3 = $"  {test1} {test2}";
                Console.WriteLine(str1, str2, str3);
            }

            //------------------------------------------------------------------
            //     Main iteration loop.
            //------------------------------------------------------------------
            while (itn < (itnlim ?? int.MaxValue))  // TODO: add default value for intlim (2*n)
            {
                itn = itn + 1;

                // Perform the next step of the bidiagonalization to obtain the
                // next beta, u, alfa, v.  These satisfy the relations
                //      beta* u = A * v - alfa * u,
                //      alfa* v = A'*u - beta*v.

                if (explicitA)
                {
                    u = A * v - alfa * u;
                }
                //else
                //{
                //    u = A(v, 1) - alfa * u;
                //}
                beta = u.L2Norm();
                if (beta > 0)
                {
                    u = (1 / beta) * u;
                    aNorm = Vector<double>.Build.Dense(new[] {aNorm, alfa, beta, damp}).L2Norm();
                    if (explicitA)
                    {
                        v = A.Transpose() * u - beta * v;
                    }
                    //else
                    //{
                    //    v = A(u, 2) - beta * v;
                    //}
                    alfa = v.L2Norm();
                    if (alfa > 0)
                    {
                        v = (1 / alfa) * v;
                    }
                }

                // Use a plane rotation to eliminate the damping parameter.
                // This alters the diagonal(rhobar) of the lower-bidiagonal matrix.

                var rhobar1 = Vector<double>.Build.Dense(new[] {rhobar, damp}).L2Norm();
                var cs1 = rhobar / rhobar1;
                var sn1 = damp / rhobar1;
                var psi = sn1 * phibar;
                phibar = cs1 * phibar;

                // Use a plane rotation to eliminate the subdiagonal element(beta)
                // of the lower-bidiagonal matrix, giving an upper-bidiagonal matrix.

                // See http://pages.cs.wisc.edu/~kline/cvxopt/lsqr_py (try to compare iteration number and quality)
                double rho, cs, sn;
                if (useSymOrtho)
                {
                    (cs, sn, rho) = SymOrtho(rhobar1, beta);
                }
                else
                {
                    rho = Vector<double>.Build.Dense(new[] {rhobar1, beta}).L2Norm();
                    cs = rhobar1 / rho;
                    sn = beta / rho;
                }
                var theta = sn * alfa;
                rhobar = -cs * alfa;
                var phi = cs * phibar;
                phibar = sn * phibar;
                var tau = sn * phi;

                // Update x and w.

                var t1 = phi / rho;
                var t2 = -theta / rho;
                var dk = (1 / rho) * w;

                x = x + t1 * w;
                w = v + t2 * w;
                ddnorm = ddnorm + Math.Pow(dk.L2Norm(), 2);
                if (wantvar)
                {
                    var = var + dk.PointwisePower(2);
                }

                // Use a plane rotation on the right to eliminate the
                // super-diagonal element(theta) of the upper-bidiagonal matrix.
                // Then use the result to estimate  norm(x).

                var delta = sn2 * rho;
                var gambar = -cs2 * rho;
                var rhs = phi - delta * z;
                var zbar = rhs / gambar;
                xnorm = Math.Sqrt(xxnorm + Math.Pow(zbar, 2));
                var gamma = Vector<double>.Build.Dense(new[] {gambar, theta}).L2Norm();
                cs2 = gambar / gamma;
                sn2 = theta / gamma;
                z = rhs / gamma;
                xxnorm = xxnorm + Math.Pow(z, 2);

                // Test for convergence.
                // First, estimate the condition of the matrix  Abar,
                // and the norms of  rbar and  Abar'rbar.

                Acond = aNorm * Math.Sqrt(ddnorm);
                var res1 = Math.Pow(phibar, 2);
                res2 = res2 + Math.Pow(psi, 2);
                rnorm = Math.Sqrt(res1 + res2);
                Arnorm = alfa * Math.Abs(tau);

                // 07 Aug 2002:
                // Distinguish between
                //    r1norm = ||b - Ax|| and
                //    r2norm = rnorm in current code
                //           = sqrt(r1norm^2 + damp^2*||x||^2).
                //    Estimate r1norm from
                //    r1norm = sqrt(r2norm^2 - damp^2*||x||^2).
                // Although there is cancellation, it might be accurate enough.

                var r1sq = Math.Pow(rnorm, 2) - dampsq * xxnorm;
                r1norm = Math.Sqrt(Math.Abs(r1sq));
                if (r1sq < 0)
                {
                    r1norm = -r1norm;
                }
                r2norm = rnorm;

                // Now use these norms to estimate certain other quantities,
                // some of which will be small near a solution.

                var test1 = rnorm / bnorm;
                var test2 = Arnorm / (aNorm * rnorm);
                var test3 = 1 / Acond;
                t1 = test1 / (1 + aNorm * xnorm / bnorm);
                var rtol = btol + atol * aNorm * xnorm / bnorm;

                // The following tests guard against extremely small values of
                // atol, btol or  ctol.  (The user may have set any or all of
                // the parameters  atol, btol, conlim to 0.)
                // The effect is equivalent to the normal tests using
                // atol = eps,  btol = eps,  conlim = 1/eps.

                if (itn >= itnlim) istop = 7;
                if (1 + test3 <= 1) istop = 6;
                if (1 + test2 <= 1) istop = 5;
                if (1 + t1 <= 1) istop = 4;

                // Allow for tolerances set by the user.

                if (test3 <= ctol) istop = 3;
                if (test2 <= atol) istop = 2;
                if (test1 <= rtol) istop = 1;

                // See if it is time to print something.

                var prnt = false;
                if (n <= 40) prnt = true;
                if (itn <= 10) prnt = true;
                if (itn >= itnlim - 10) prnt = true;
                if (itn % 10 == 0) prnt = true;
                if (test3 <= 2 * ctol) prnt = true;
                if (test2 <= 10 * atol) prnt = true;
                if (test1 <= 10 * rtol) prnt = true;
                if (istop != 0) prnt = true;

                if (prnt)
                {
                    if (show)
                    { 
                        var str1 = $"{itn} {x[0]}";
                        var str2 = $" {r1norm} {r2norm}";
                        var str3 = $" {test1} {test2}";
                        var str4 = $" {aNorm} {Acond}";
                        Console.WriteLine(str1, str2, str3, str4);
                    }
                }
                if (istop > 0)
                {
                    break;
                }
            }
            // End of iteration loop.
            // Print the stopping condition.

            if (show)
            {
                Console.WriteLine("\nlsqrSOL finished\n");
                Console.WriteLine(msg[istop + 1]);
                Console.WriteLine(" ");
                var str1 = $"istop ={istop}   r1norm ={r1norm}";
                var str2 = $"Anorm ={aNorm}   Arnorm ={Arnorm}";
                var str3 = $"itn   ={itn}   r2norm ={r2norm}";
                var str4 = $"Acond ={Acond}   xnorm  ={xnorm}";
                Console.WriteLine(str1, " ", str2);
                Console.WriteLine(str3, " ", str4);
                Console.WriteLine(" ");
            }
            return x;
        }

        // TODO: find the article with this method
        private static (double, double, double) SymOrtho(double a, double b)
        {
            double c = 0, s = 0, r = 0;
            if (b == 0)
            {
                s = 0.0;
                r = Math.Abs(a);
                if (a == 0)
                {
                    c = 1.0;
                }
                else
                {
                    c = Math.Sign(a);
                }
            }
            else if (a == 0)
            {
                c = 0;
                s = Math.Sign(b);
                r = Math.Abs(b);
            }
            else if (Math.Abs(b) >= Math.Abs(a))
            {
                var tau = a / b;
                s = Math.Sign(b) / Math.Sqrt(1 + Math.Pow(tau, 2));
                c = s * tau;
                r = b / s;
            }
            else if (Math.Abs(a) > Math.Abs(b))
            {
                var tau = b / a;
                c = Math.Sign(a) / Math.Sqrt(1 + Math.Pow(tau, 2));
                s = c * tau;
                r = a / c;
            }
            return (c, s, r);
        }
    }
}

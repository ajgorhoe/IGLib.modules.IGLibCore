// Copyright (c) Igor Grešovnik (2009), IGLib license; http://www2.arnes.si/~ljc3m2/igor/ioptlib/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;


// From Math.Net:

//using Matrix_MathNet = MathNet.Numerics.LinearAlgebra.Matrix;
//using QRDecomposition_MathNet = MathNet.Numerics.LinearAlgebra.QRDecomposition;
//using LUDecomposition_MathNet = MathNet.Numerics.LinearAlgebra.LUDecomposition;

// From Math.Net Numerics:

using DenseMatrix = MathNet.Numerics.LinearAlgebra.Double.DenseMatrix;

using MathNet.Numerics.LinearAlgebra.Double;


using IG.Lib;


namespace IG.Num
{

    /// <summary>Various utilities for testing computational speed of the current system.</summary>
    /// $A Igor xx Feb08;
    public class SpeedTestCpu
    {

        #region Static

        
        /// <summary>Test of QR decomposition. Writes times necessary for all steps.</summary>
        /// <param name="numEq">Number of equations to be solved with decomposition.</param>        
        /// <param name="outLevel">Level of output.</param>
        /// <returns>Total time spent for all operations.</returns>
        public static double TestComputationalTimesQR(int numEq, int outLevel)
        {
            return TestComputationalTimesQR_IGLib(numEq, outLevel);
        }

        
        /// <summary>Test of QR decomposition, also measures time necessary fo rindividual operations.</summary>
        /// <param name="outLevel">Level of output.</param>
        /// <param name="numEq">Number of equations to be solved with decomposition.</param>
        /// <param name="testProduct">If true then it is tested if the product of factors gives the original 
        /// matrix. Otherwise, this test is skipped.</param>
        /// <returns>Total wallclock time (in seconds) spent for the test.</returns>
        public static double TestComputationalTimesQR(int numEq, int outLevel, bool testProduct)
        {
            return TestComputationalTimesQR_IGLib(numEq, outLevel, testProduct);
        }

        
        /// <summary>Test of LU decomposition.</summary>
        /// <param name="outLevel">Level of output.</param>
        /// <param name="numEq">Number of equations to be solved with decomposition.</param>
        /// <returns>Total wallclock time (in seconds) spent for the test.</returns>
        public static double TestComputationalTimesLU(int numEq, int outLevel)
        {
            return TestComputationalTimesLU_IGLib(numEq, outLevel);
        }

        /// <summary>Test of LU decomposition.</summary>
        /// <param name="outLevel">Level of output.</param>
        /// <param name="numEq">Number of equations to be solved with decomposition.</param>
        /// <param name="testProduct">If true then it is tested if the product of factors gives the original 
        /// matrix. Otherwise, this test is skipped.</param>
        /// <returns>Total wallclock time (in seconds) spent for the test.</returns>
        public static double TestComputationalTimesLU(int numEq, int outLevel, bool testProduct)
        {
            return TestComputationalTimesLU_IGLib(numEq, outLevel, testProduct);
        } // TestComputationalTimesLU()

        
        /// <summary>Test of Cholesky decomposition, also measures time necessary fo rindividual operations.</summary>
        /// <param name="outLevel">Level of output.</param>
        /// <param name="numEq">Number of equations to be solved with decomposition.</param>
        /// <returns>Total wallclock time (in seconds) spent for the test.</returns>
        public static double TestComputationalTimesCholesky(int numEq, int outLevel)
        {
            return TestComputationalTimesCholesky_IGLib(numEq, outLevel);
        }

        /// <summary>Test of Cholesky decomposition, also measures time necessary fo rindividual operations.</summary>
        /// <param name="outLevel">Level of output.</param>
        /// <param name="numEq">Number of equations to be solved with decomposition.</param>
        /// <param name="testProduct">If true then it is tested if the product of factors gives the original 
        /// matrix. Otherwise, this test is skipped.</param>
        /// <returns>Total wallclock time (in seconds) spent for the test.</returns>
        public static double TestComputationalTimesCholesky(int numEq, int outLevel, bool testProduct)
        {
            return TestComputationalTimesCholesky_IGLib(numEq, outLevel, testProduct);
        }

        #region IGLib


        ///// <summary>Test of LU decomposition.</summary>
        ///// <param name="outLevel">Level of output.</param>
        ///// <param name="numEq">Number of equations to be solved with decomposition.</param>
        ///// <returns>Total wallclock time (in seconds) spent for the test.</returns>
        //public static double TestComputationalTimesLU_IGLib(int numEq, int outLevel)
        //{
        //    return TestComputationalTimesLU_IGLib(numEq, outLevel, false /* testProduct */);
        //}

        /// <summary>Test of LU decomposition.</summary>
        /// <param name="outLevel">Level of output.</param>
        /// <param name="numEq">Number of equations to be solved with decomposition.</param>
        /// <param name="testProduct">If true then it is tested if the product of factors gives the original 
        /// matrix. Otherwise, this test is skipped.</param>
        /// <returns>Total wallclock time (in seconds) spent for the test.</returns>
        public static double TestComputationalTimesLU_IGLib(int numEq, int outLevel, bool testProduct = false)
        {
            if (outLevel > 0)
            {
                Console.WriteLine();
                Console.WriteLine("Measuring time needed for LU decomposition:");
                Console.WriteLine();
            }
            if (numEq < 1)
                throw new ArgumentException("Number of equations must be greater than 0.");
            // Naredimo štoparico, s katero merimo čas. V konstruktorju z argumentom podamo ime, po katerem
            // lahko identificiramo narejeno štoparico. To pride prav, če jih imamo več.
            StopWatch1 t = new StopWatch1("Decomposition");
            // Naredimo še eno štoparico, ki meri čisti čas računanja brez preizkusov:
            StopWatch1 tbare = new StopWatch1("Decomposition, pure time");

            if (outLevel > 0)
            {
                Console.Write(Environment.NewLine + Environment.NewLine);
                Console.WriteLine("========================================================================");
                Console.WriteLine("Solution of system of " + numEq.ToString() + " equations with LU decomposition: " + Environment.NewLine);
            }
            t.Start();
            // Form system of equations:
            Matrix A = Matrix.Random(numEq, numEq);  // naredimo kvadratno matriko z naključnimi elementi
            A[0, 0] = 0.0;  // Element [1,1] is set to 0, so pivoting is needed in LU (swapping of lines)
            Vector b = Vector.Random(numEq);
            t.Stop();
            if (outLevel > 0)
            {
                Console.WriteLine();
                Console.WriteLine("Time necessary for CREATION of system matrix and right-hand side vector:");
                Console.WriteLine("    t = " + t.Time + " s (CPU: " + t.CpuTime + " s)");
                Console.WriteLine();
            }
            t.Start();
            tbare.Start();
            LUDecomposition LU = new LUDecomposition(A);  // Calculation of LU decomposition
            t.Stop();
            tbare.Stop();
            if (outLevel > 0)
            {
                Console.WriteLine();
                Console.WriteLine("Time necessary for LU DECOMPOSITION:");
                Console.WriteLine("    t = " + t.Time + " s (CPU: " + t.CpuTime + " s)");
                Console.WriteLine();
            }
            if (testProduct)
            {
                t.Start();
                IMatrix P = LU.GetProduct();  // product of lower ant upper triangular matrix
                IMatrix dif = null;
                Matrix.Subtract(A, P, ref dif);
                double NormProductDifference = dif.NormForbenius;
                t.Stop();
                if (outLevel > 0)
                {
                    Console.WriteLine();
                    Console.WriteLine("Error of decomposition - norm of the difference ||A-L*U|| = " + NormProductDifference.ToString());
                    Console.WriteLine();
                    Console.WriteLine("Time necessary for verification of decomposition:");
                    Console.WriteLine("    t = " + t.Time + " s (CPU: " + t.CpuTime + " s)");
                    Console.WriteLine();
                }
            }
            // Solution of system of equations with decomposition (back substitution):
            t.Start();
            tbare.Start();
            IVector x = LU.Solve(b);
            t.Stop();
            tbare.Stop();
            if (outLevel > 0)
            {
                Console.WriteLine();
                Console.WriteLine("Time necessary for SOLUTION with decomposed matrix:");
                Console.WriteLine("    t = " + t.Time + " s (CPU: " + t.CpuTime + " s)");
                Console.WriteLine();
            }

            // Verification: calculation of error:
            t.Start();
            IVector residuum = null;
            Matrix.Multiply(A, x, ref residuum);
            Vector.Subtract(residuum, b, ref residuum);
            double normResiduum = residuum.Norm;
            t.Stop();
            if (outLevel > 0)
            {
                Console.WriteLine();
                Console.WriteLine("Error of solution: Norm of the difference ||A x - b|| = " + normResiduum.ToString());
                Console.WriteLine();
                Console.WriteLine("Time necessary for calculating solution error:");
                Console.WriteLine("    t = " + t.Time + " s (CPU: " + t.CpuTime + " s)");
                Console.WriteLine();

                Console.WriteLine();
                Console.WriteLine("Total time for all operations (LU, " + numEq + " equations): ");
                Console.WriteLine("    t = " + t.TotalTime + " s (CPU: " + t.TotalCpuTime + " s)");
                Console.WriteLine();

                //Console.WriteLine();
                //Console.WriteLine("Data from stopwatch:");
                //Console.WriteLine(t.ToString());
                //Console.WriteLine();
                //Console.WriteLine("Stopwatch for pure execution time (without testing):");
                //Console.WriteLine(tbare.ToString());
                Console.WriteLine("____________________________________________________________");

                Console.WriteLine(Environment.NewLine);
            }

            //t.Reset();
            //numEq *= 10;

            return t.TotalTime;
        }  // TestComputationalTimesLU()


        ///// <summary>Test of QR decomposition, also measures time necessary fo rindividual operations.</summary>
        ///// <param name="outLevel">Level of output.</param>
        ///// <param name="numEq">Number of equations to be solved with decomposition.</param>
        ///// <returns>Total wallclock time (in seconds) spent for the test.</returns>
        //public static double TestComputationalTimesQR_IGLib(int numEq, int outLevel)
        //{
        //    return TestComputationalTimesQR_IGLib(numEq, outLevel, false /* testProduct */);
        //}

        /// <summary>Test of QR decomposition, also measures time necessary fo rindividual operations.</summary>
        /// <param name="outLevel">Level of output.</param>
        /// <param name="numEq">Number of equations to be solved with decomposition.</param>
        /// <param name="testProduct">If true then it is tested if the product of factors gives the original 
        /// matrix. Otherwise, this test is skipped.</param>
        /// <returns>Total wallclock time (in seconds) spent for the test.</returns>
        public static double TestComputationalTimesQR_IGLib(int numEq, int outLevel, bool testProduct = false)
        {
            if (outLevel > 0)
            {
                Console.WriteLine();
                Console.WriteLine("Measuring time needed for QR decomposition:");
                Console.WriteLine();
            }
            if (numEq < 1)
                throw new ArgumentException("Number of equations must be greater than 0.");
            StopWatch1 t = new StopWatch1("Decomposition");
            // Naredimo še eno štoparico, ki meri čisti čas računanja brez preizkusov:
            StopWatch1 tbare = new StopWatch1("Decomposition, pure time");

            if (outLevel > 0)
            {
                Console.Write(Environment.NewLine + Environment.NewLine);
                Console.WriteLine("========================================================================");
                Console.WriteLine("Solution of system of " + numEq.ToString() + " equations with QR decomposition: " + Environment.NewLine);
            }
            t.Start();
            // Form system of equations:
            Matrix A = Matrix.Random(numEq, numEq);  
            A[0, 0] = 0.0;  // Element [1,1] is intentionally set to 0
            Vector b = Vector.Random(numEq);
            t.Stop();
            if (outLevel > 0)
            {
                Console.WriteLine();
                Console.WriteLine("Time necessary for CREATION of system matrix and right-hand side vector:");
                Console.WriteLine("    t = " + t.Time + " s (CPU: " + t.CpuTime + " s)");
                Console.WriteLine();
            }
            t.Start();
            tbare.Start();
            QRDecomposition QR = new QRDecomposition(A);  // Calculation of QR decomposition
            t.Stop();
            tbare.Stop();
            if (outLevel > 0)
            {
                Console.WriteLine();
                Console.WriteLine("Time necessary for QR DECOMPOSITION:");
                Console.WriteLine("    t = " + t.Time + " s (CPU: " + t.CpuTime + " s)");
                Console.WriteLine();
            }
            if (testProduct)
            {
                t.Start();
                IMatrix P = QR.GetProduct();  // product of orthogonal and upper triangular factors
                IMatrix dif = null;
                Matrix.Subtract(A, P, ref dif);
                double NormProductDifference = dif.NormForbenius;
                t.Stop();
                if (outLevel > 0)
                {
                    Console.WriteLine();
                    Console.WriteLine("Error of decomposition - norm of the difference ||A-Q*R|| = " + NormProductDifference.ToString());
                    Console.WriteLine();
                    Console.WriteLine("Time necessary for verification of decomposition:");
                    Console.WriteLine("    t = " + t.Time + " s (CPU: " + t.CpuTime + " s)");
                    Console.WriteLine();
                }
            }
            // Solution of system of equations with decomposition (back substitution):
            t.Start();
            tbare.Start();
            IVector x = QR.Solve(b);
            t.Stop();
            tbare.Stop();
            if (outLevel > 0)
            {
                Console.WriteLine();
                Console.WriteLine("Time necessary for SOLUTION with decomposed matrix:");
                Console.WriteLine("    t = " + t.Time + " s (CPU: " + t.CpuTime + " s)");
                Console.WriteLine();
            }

            // Verification: calculation of error:
            t.Start();
            IVector residuum = null;
            Matrix.Multiply(A, x, ref residuum);
            Vector.Subtract(residuum, b, ref residuum);
            double normResiduum = residuum.Norm;
            t.Stop();
            if (outLevel > 0)
            {
                Console.WriteLine();
                Console.WriteLine("Error of solution: Norm of the difference ||A x - b|| = " + normResiduum.ToString());
                Console.WriteLine();
                Console.WriteLine("Time necessary for calculating solution error:");
                Console.WriteLine("    t = " + t.Time + " s (CPU: " + t.CpuTime + " s)");
                Console.WriteLine();

                Console.WriteLine();
                Console.WriteLine("Total time for all operations (QR, " + numEq + " equations): ");
                Console.WriteLine("    t = " + t.TotalTime + " s (CPU: " + t.TotalCpuTime + " s)");
                Console.WriteLine();

                //Console.WriteLine();
                //Console.WriteLine("Data from stopwatch:");
                //Console.WriteLine(t.ToString());
                //Console.WriteLine();
                //Console.WriteLine("Stopwatch for pure execution time (without testing):");
                //Console.WriteLine(tbare.ToString());
                Console.WriteLine("____________________________________________________________");

                Console.WriteLine(Environment.NewLine);
            }

            return t.TotalTime;
        }  // TestComputationalTimesQR()


        ///// <summary>Test of Cholesky decomposition, also measures time necessary fo rindividual operations.</summary>
        ///// <param name="outLevel">Level of output.</param>
        ///// <param name="numEq">Number of equations to be solved with decomposition.</param>
        ///// <returns>Total wallclock time (in seconds) spent for the test.</returns>
        //public static double TestComputationalTimesCholesky_IGLib(int numEq, int outLevel)
        //{
        //    return TestComputationalTimesCholesky_IGLib(numEq, outLevel, false /* testProduct */);
        //}

        /// <summary>Test of Cholesky decomposition, also measures time necessary fo rindividual operations.</summary>
        /// <param name="outLevel">Level of output.</param>
        /// <param name="numEq">Number of equations to be solved with decomposition.</param>
        /// <param name="testProduct">If true then it is tested if the product of factors gives the original 
        /// matrix. Otherwise, this test is skipped.</param>
        /// <returns>Total wallclock time (in seconds) spent for the test.</returns>
        public static double TestComputationalTimesCholesky_IGLib(int numEq, int outLevel, bool testProduct = false)
        {
            if (outLevel > 0)
            {
                Console.WriteLine();
                Console.WriteLine("Measuring time needed for Cholesky decomposition:");
                Console.WriteLine();
            }
            if (numEq < 1)
                throw new ArgumentException("Number of equations must be greater than 0.");
            StopWatch1 t = new StopWatch1("Decomposition");
            // Naredimo še eno štoparico, ki meri čisti čas računanja brez preizkusov:
            StopWatch1 tbare = new StopWatch1("Decomposition, pure time");

            if (outLevel > 0)
            {
                Console.Write(Environment.NewLine + Environment.NewLine);
                Console.WriteLine("========================================================================");
                Console.WriteLine("Solution of system of " + numEq.ToString() + " equations with Cholesky decomposition: " + Environment.NewLine);
            }
            t.Start();
            // Form system of equations:
            IG.Num.Matrix A = new IG.Num.Matrix(numEq, numEq);
            // Matrix.SetRandomSymmetricPositiveDefinite(A);
            Matrix.SetRandomPositiveDiagonallyDominantSymmetric(A, null /* randomGenerator - take global */, 2.0);
            Vector b = Vector.Random(numEq);
            t.Stop();
            if (outLevel > 0)
            {
                Console.WriteLine();
                Console.WriteLine("Time necessary for CREATION of system matrix and right-hand side vector:");
                Console.WriteLine("    t = " + t.Time + " s (CPU: " + t.CpuTime + " s)");
                Console.WriteLine();
            }
            t.Start();
            tbare.Start();
            CholeskyDecomposition Cholesky = new CholeskyDecomposition(A);  // Calculation of QR decomposition
            t.Stop();
            tbare.Stop();
            if (outLevel > 0)
            {
                Console.WriteLine();
                Console.WriteLine("Time necessary for Cholesky DECOMPOSITION:");
                Console.WriteLine("    t = " + t.Time + " s (CPU: " + t.CpuTime + " s)");
                Console.WriteLine();
            }
            if (testProduct)
            {
                t.Start();
                IMatrix P = Cholesky.GetProduct();  // product of orthogonal and upper triangular factors
                IMatrix dif = null;
                Matrix.Subtract(A, P, ref dif);
                double NormProductDifference = dif.NormForbenius;
                t.Stop();
                if (outLevel > 0)
                {
                    Console.WriteLine();
                    Console.WriteLine("Error of decomposition - norm of the difference ||A-Q*R|| = " + NormProductDifference.ToString());
                    Console.WriteLine();
                    Console.WriteLine("Time necessary for verification of decomposition:");
                    Console.WriteLine("    t = " + t.Time + " s (CPU: " + t.CpuTime + " s)");
                    Console.WriteLine();
                }
            }
            // Solution of system of equations with decomposition (back substitution):
            t.Start();
            tbare.Start();
            IVector x = Cholesky.Solve(b);
            t.Stop();
            tbare.Stop();
            if (outLevel > 0)
            {
                Console.WriteLine();
                Console.WriteLine("Time necessary for SOLUTION with decomposed matrix:");
                Console.WriteLine("    t = " + t.Time + " s (CPU: " + t.CpuTime + " s)");
                Console.WriteLine();
            }

            // Verification: calculation of error:
            t.Start();
            IVector residuum = null;
            Matrix.Multiply(A, x, ref residuum);
            Vector.Subtract(residuum, b, ref residuum);
            double normResiduum = residuum.Norm;
            t.Stop();
            if (outLevel > 0)
            {
                Console.WriteLine();
                Console.WriteLine("Error of solution: Norm of the difference ||A x - b|| = " + normResiduum.ToString());
                Console.WriteLine();
                Console.WriteLine("Time necessary for calculating solution error:");
                Console.WriteLine("    t = " + t.Time + " s (CPU: " + t.CpuTime + " s)");
                Console.WriteLine();

                Console.WriteLine();
                Console.WriteLine("Total time for all operations (Cholesky, " + numEq + " equations): ");
                Console.WriteLine("    t = " + t.TotalTime + " s (CPU: " + t.TotalCpuTime + " s)");
                Console.WriteLine();

                //Console.WriteLine();
                //Console.WriteLine("Data from stopwatch:");
                //Console.WriteLine(t.ToString());
                //Console.WriteLine();
                //Console.WriteLine("Stopwatch for pure execution time (without testing):");
                //Console.WriteLine(tbare.ToString());
                Console.WriteLine("____________________________________________________________");

                Console.WriteLine(Environment.NewLine);
            }

            return t.TotalTime;
        }  // TestComputationalTimesQR()


        #endregion IGLib


        #region IgBase

        /// <summary>Test of LU decomposition.</summary>
        /// <param name="outLevel">Level of output.</param>
        /// <param name="numEq">Number of equations to be solved with decomposition.</param>
        /// <param name="testProduct">If true then it is tested if the product of factors gives the original 
        /// matrix. Otherwise, this test is skipped.</param>
        /// <returns>Total wallclock time (in seconds) spent for the test.</returns>
        public static double TestComputationalTimesLU_Base(int numEq, int outLevel, bool testProduct = false)
        {
            if (outLevel > 0)
            {
                Console.WriteLine();
                Console.WriteLine("Measuring time needed for LU decomposition (from MatrixBase):");
                Console.WriteLine();
            }
            if (numEq < 1)
                throw new ArgumentException("Number of equations must be greater than 0.");
            // Naredimo štoparico, s katero merimo čas. V konstruktorju z argumentom podamo ime, po katerem
            // lahko identificiramo narejeno štoparico. To pride prav, če jih imamo več.
            StopWatch1 t = new StopWatch1("Decomposition");
            // Naredimo še eno štoparico, ki meri čisti čas računanja brez preizkusov:
            StopWatch1 tbare = new StopWatch1("Decomposition, pure time");

            if (outLevel > 0)
            {
                Console.Write(Environment.NewLine + Environment.NewLine);
                Console.WriteLine("========================================================================");
                Console.WriteLine("Solution of system of " + numEq.ToString() + " equations with LU decomposition: " + Environment.NewLine);
            }
            t.Start();
            // Form system of equations:
            IMatrix A = Matrix.Random(numEq, numEq);  // naredimo kvadratno matriko z naključnimi elementi
            A[0, 0] = 0.0;  // Element [1,1] is set to 0, so pivoting is needed in LU (swapping of lines)
            IVector b = Vector.Random(numEq);
            t.Stop();
            if (outLevel > 0)
            {
                Console.WriteLine();
                Console.WriteLine("Time necessary for CREATION of system matrix and right-hand side vector:");
                Console.WriteLine("    t = " + t.Time + " s (CPU: " + t.CpuTime + " s)");
                Console.WriteLine();
            }
            t.Start();
            int[] permutations = null;
            int toggle = 0;
            IMatrix LU = null;
            tbare.Start();
            // LUDecomposition LU = new LUDecomposition(A);  // Calculation of LU decomposition
            Matrix.LuDecompose(A, out toggle, ref permutations, ref LU);
            t.Stop();
            tbare.Stop();
            if (outLevel > 0)
            {
                Console.WriteLine();
                Console.WriteLine("Time necessary for LU DECOMPOSITION:");
                Console.WriteLine("    t = " + t.Time + " s (CPU: " + t.CpuTime + " s)");
                Console.WriteLine();
            }
            if (testProduct)
            {
                t.Start();

                // Check the product:
                IMatrix product = null;
                IMatrix diffMat = null;
                IMatrix lower = null;
                IMatrix upper = null;
                Matrix.LuExtractLower(LU, ref lower);
                Matrix.LuExtractUpper(LU, ref upper);
                Matrix.Multiply(lower, upper, ref product);
                IMatrix restored = null;
                int[] auxArray1 = null;
                Matrix.UnPermute(product, permutations, ref auxArray1, ref restored);  // use a custom method with the perm array to unscramble LU
                MatrixBase.Subtract(restored, A, ref diffMat);

                double NormProductDifference = diffMat.NormForbenius;
                t.Stop();
                if (outLevel > 0)
                {
                    Console.WriteLine();
                    Console.WriteLine("Error of decomposition - norm of the difference ||A-L*U|| = " + NormProductDifference.ToString());
                    Console.WriteLine();
                    Console.WriteLine("Time necessary for verification of decomposition:");
                    Console.WriteLine("    t = " + t.Time + " s (CPU: " + t.CpuTime + " s)");
                    Console.WriteLine();
                }
            }
            // Solution of system of equations with decomposition (back substitution):
            t.Start();
            tbare.Start();

            IVector auxVec = null;
            IVector x = null;
            Matrix.LuSolve(LU, permutations, b, ref auxVec, ref x);

            t.Stop();
            tbare.Stop();
            if (outLevel > 0)
            {
                Console.WriteLine();
                Console.WriteLine("Time necessary for SOLUTION with decomposed matrix:");
                Console.WriteLine("    t = " + t.Time + " s (CPU: " + t.CpuTime + " s)");
                Console.WriteLine();
            }

            // Verification: calculation of error:
            t.Start();
            IVector residuum = null;
            Matrix.Multiply(A, x, ref residuum);
            Vector.Subtract(residuum, b, ref residuum);
            double normResiduum = residuum.Norm;
            t.Stop();
            if (outLevel > 0)
            {
                Console.WriteLine();
                Console.WriteLine("Error of solution: Norm of the difference ||A x - b|| = " + normResiduum.ToString());
                Console.WriteLine();
                Console.WriteLine("Time necessary for calculating solution error:");
                Console.WriteLine("    t = " + t.Time + " s (CPU: " + t.CpuTime + " s)");
                Console.WriteLine();

                Console.WriteLine();
                Console.WriteLine("Total time for all operations (LU, " + numEq + " equations): ");
                Console.WriteLine("    t = " + t.TotalTime + " s (CPU: " + t.TotalCpuTime + " s)");
                Console.WriteLine();

                //Console.WriteLine();
                //Console.WriteLine("Data from stopwatch:");
                //Console.WriteLine(t.ToString());
                //Console.WriteLine();
                //Console.WriteLine("Stopwatch for pure execution time (without testing):");
                //Console.WriteLine(tbare.ToString());
                Console.WriteLine("____________________________________________________________");

                Console.WriteLine(Environment.NewLine);
            }

            //t.Reset();
            //numEq *= 10;

            return t.TotalTime;
        }  // TestComputationalTimesLU_Base()

        /// <summary>Test of Cholesky decomposition, also measures time necessary fo rindividual operations.</summary>
        /// <param name="outLevel">Level of output.</param>
        /// <param name="numEq">Number of equations to be solved with decomposition.</param>
        /// <param name="testProduct">If true then it is tested if the product of factors gives the original 
        /// matrix. Otherwise, this test is skipped.</param>
        /// <returns>Total wallclock time (in seconds) spent for the test.</returns>
        public static double TestComputationalTimesCholesky_Base(int numEq, int outLevel, bool testProduct = false)
        {
            if (outLevel > 0)
            {
                Console.WriteLine();
                Console.WriteLine("Measuring time needed for Cholesky decomposition (from MatrixBase):");
                Console.WriteLine();
            }
            if (numEq < 1)
                throw new ArgumentException("Number of equations must be greater than 0.");
            StopWatch1 t = new StopWatch1("Decomposition");
            // Naredimo še eno štoparico, ki meri čisti čas računanja brez preizkusov:
            StopWatch1 tbare = new StopWatch1("Decomposition, pure time");

            if (outLevel > 0)
            {
                Console.Write(Environment.NewLine + Environment.NewLine);
                Console.WriteLine("========================================================================");
                Console.WriteLine("Solution of system of " + numEq.ToString() + " equations with Cholesky decomposition: " + Environment.NewLine);
            }
            t.Start();
            // Form system of equations:
            IMatrix A = new Matrix(numEq, numEq);
            // Matrix.SetRandomSymmetricPositiveDefinite(A);
            Matrix.SetRandomPositiveDiagonallyDominantSymmetric(A, null /* randomGenerator - take global */, 2.0);
            IVector b = Vector.Random(numEq);
            t.Stop();
            if (outLevel > 0)
            {
                Console.WriteLine();
                Console.WriteLine("Time necessary for CREATION of system matrix and right-hand side vector:");
                Console.WriteLine("    t = " + t.Time + " s (CPU: " + t.CpuTime + " s)");
                Console.WriteLine();
            }
            t.Start();
            tbare.Start();
            IMatrix Cholesky = null;
            MatrixBase.CholeskyDecompose(A, ref Cholesky);  // Calculation of QR decomposition
            t.Stop();
            tbare.Stop();
            if (outLevel > 0)
            {
                Console.WriteLine();
                Console.WriteLine("Time necessary for Cholesky DECOMPOSITION:");
                Console.WriteLine("    t = " + t.Time + " s (CPU: " + t.CpuTime + " s)");
                Console.WriteLine();
            }
            if (testProduct)
            {
                t.Start();
                IMatrix product = null;
                IMatrix diffMat = null;
                IMatrix lower = null;
                IMatrix upper = null;
                MatrixBase.CholeskyExtractLower(Cholesky, ref lower);
                MatrixBase.CholeskyExtractUpper(Cholesky, ref upper);
                MatrixBase.Multiply(lower, upper, ref product);
                MatrixBase.Subtract(product, A, ref diffMat);
                double NormProductDifference = diffMat.NormForbenius;
                t.Stop();
                if (outLevel > 0)
                {
                    Console.WriteLine();
                    Console.WriteLine("Error of decomposition - norm of the difference ||A-Q*R|| = " + NormProductDifference.ToString());
                    Console.WriteLine();
                    Console.WriteLine("Time necessary for verification of decomposition:");
                    Console.WriteLine("    t = " + t.Time + " s (CPU: " + t.CpuTime + " s)");
                    Console.WriteLine();
                }
            }
            // Solution of system of equations with decomposition (back substitution):
            t.Start();
            tbare.Start();
            IVector x = null;
            MatrixBase.CholeskySolve(Cholesky, b, ref x);
            t.Stop();
            tbare.Stop();
            if (outLevel > 0)
            {
                Console.WriteLine();
                Console.WriteLine("Time necessary for SOLUTION with decomposed matrix:");
                Console.WriteLine("    t = " + t.Time + " s (CPU: " + t.CpuTime + " s)");
                Console.WriteLine();
            }

            // Verification: calculation of error:
            t.Start();
            IVector residuum = null;
            Matrix.Multiply(A, x, ref residuum);
            Vector.Subtract(residuum, b, ref residuum);
            double normResiduum = residuum.Norm;
            t.Stop();
            if (outLevel > 0)
            {
                Console.WriteLine();
                Console.WriteLine("Error of solution: Norm of the difference ||A x - b|| = " + normResiduum.ToString());
                Console.WriteLine();
                Console.WriteLine("Time necessary for calculating solution error:");
                Console.WriteLine("    t = " + t.Time + " s (CPU: " + t.CpuTime + " s)");
                Console.WriteLine();

                Console.WriteLine();
                Console.WriteLine("Total time for all operations (Cholesky, " + numEq + " equations): ");
                Console.WriteLine("    t = " + t.TotalTime + " s (CPU: " + t.TotalCpuTime + " s)");
                Console.WriteLine();

                //Console.WriteLine();
                //Console.WriteLine("Data from stopwatch:");
                //Console.WriteLine(t.ToString());
                //Console.WriteLine();
                //Console.WriteLine("Stopwatch for pure execution time (without testing):");
                //Console.WriteLine(tbare.ToString());
                Console.WriteLine("____________________________________________________________");

                Console.WriteLine(Environment.NewLine);
            }

            return t.TotalTime;
        }  // TestComputationalTimesCholesky_Base()

        /// <summary>Test of Ldlt decomposition, also measures time necessary fo rindividual operations.</summary>
        /// <param name="outLevel">Level of output.</param>
        /// <param name="numEq">Number of equations to be solved with decomposition.</param>
        /// <param name="testProduct">If true then it is tested if the product of factors gives the original 
        /// matrix. Otherwise, this test is skipped.</param>
        /// <returns>Total wallclock time (in seconds) spent for the test.</returns>
        public static double TestComputationalTimesLdlt_Base(int numEq, int outLevel, bool testProduct = false)
        {
            if (outLevel > 0)
            {
                Console.WriteLine();
                Console.WriteLine("Measuring time needed for Ldlt decomposition (from MatrixBase):");
                Console.WriteLine();
            }
            if (numEq < 1)
                throw new ArgumentException("Number of equations must be greater than 0.");
            StopWatch1 t = new StopWatch1("Decomposition");
            // Naredimo še eno štoparico, ki meri čisti čas računanja brez preizkusov:
            StopWatch1 tbare = new StopWatch1("Decomposition, pure time");

            if (outLevel > 0)
            {
                Console.Write(Environment.NewLine + Environment.NewLine);
                Console.WriteLine("========================================================================");
                Console.WriteLine("Solution of system of " + numEq.ToString() + " equations with Ldlt decomposition: " + Environment.NewLine);
            }
            t.Start();
            // Form system of equations:
            IMatrix A = new Matrix(numEq, numEq);
            // Matrix.SetRandomSymmetricPositiveDefinite(A);
            Matrix.SetRandomPositiveDiagonallyDominantSymmetric(A, null /* randomGenerator - take global */, 2.0);
            IVector b = Vector.Random(numEq);
            t.Stop();
            if (outLevel > 0)
            {
                Console.WriteLine();
                Console.WriteLine("Time necessary for CREATION of system matrix and right-hand side vector:");
                Console.WriteLine("    t = " + t.Time + " s (CPU: " + t.CpuTime + " s)");
                Console.WriteLine();
            }
            t.Start();
            tbare.Start();
            IMatrix Ldlt = null;
            MatrixBase.LdltDecompose(A, ref Ldlt);  // Calculation of QR decomposition
            t.Stop();
            tbare.Stop();
            if (outLevel > 0)
            {
                Console.WriteLine();
                Console.WriteLine("Time necessary for Ldlt DECOMPOSITION:");
                Console.WriteLine("    t = " + t.Time + " s (CPU: " + t.CpuTime + " s)");
                Console.WriteLine();
            }
            if (testProduct)
            {
                t.Start();
                IMatrix product = null;
                IMatrix diffMat = null;
                IMatrix lower = null;
                IMatrix upper = null;
                MatrixBase.LdltExtractLower(Ldlt, ref lower);
                MatrixBase.LdltExtractUpper(Ldlt, ref upper);
                MatrixBase.Multiply(lower, upper, ref product);
                MatrixBase.Subtract(product, A, ref diffMat);
                double NormProductDifference = diffMat.NormForbenius;
                t.Stop();
                if (outLevel > 0)
                {
                    Console.WriteLine();
                    Console.WriteLine("Error of decomposition - norm of the difference ||A-Q*R|| = " + NormProductDifference.ToString());
                    Console.WriteLine();
                    Console.WriteLine("Time necessary for verification of decomposition:");
                    Console.WriteLine("    t = " + t.Time + " s (CPU: " + t.CpuTime + " s)");
                    Console.WriteLine();
                }
            }
            // Solution of system of equations with decomposition (back substitution):
            t.Start();
            tbare.Start();
            IVector x = null;
            MatrixBase.LdltSolve(Ldlt, b, ref x);
            t.Stop();
            tbare.Stop();
            if (outLevel > 0)
            {
                Console.WriteLine();
                Console.WriteLine("Time necessary for SOLUTION with decomposed matrix:");
                Console.WriteLine("    t = " + t.Time + " s (CPU: " + t.CpuTime + " s)");
                Console.WriteLine();
            }

            // Verification: calculation of error:
            t.Start();
            IVector residuum = null;
            Matrix.Multiply(A, x, ref residuum);
            Vector.Subtract(residuum, b, ref residuum);
            double normResiduum = residuum.Norm;
            t.Stop();
            if (outLevel > 0)
            {
                Console.WriteLine();
                Console.WriteLine("Error of solution: Norm of the difference ||A x - b|| = " + normResiduum.ToString());
                Console.WriteLine();
                Console.WriteLine("Time necessary for calculating solution error:");
                Console.WriteLine("    t = " + t.Time + " s (CPU: " + t.CpuTime + " s)");
                Console.WriteLine();

                Console.WriteLine();
                Console.WriteLine("Total time for all operations (Ldlt, " + numEq + " equations): ");
                Console.WriteLine("    t = " + t.TotalTime + " s (CPU: " + t.TotalCpuTime + " s)");
                Console.WriteLine();

                //Console.WriteLine();
                //Console.WriteLine("Data from stopwatch:");
                //Console.WriteLine(t.ToString());
                //Console.WriteLine();
                //Console.WriteLine("Stopwatch for pure execution time (without testing):");
                //Console.WriteLine(tbare.ToString());
                Console.WriteLine("____________________________________________________________");

                Console.WriteLine(Environment.NewLine);
            }

            return t.TotalTime;
        }  // TestComputationalTimesLdlt_Base()

        #endregion IgBase


        #endregion Static

        #region ExamplesMathNetNumerics

        /// <summary>Example of how to use LU decomposition from Math.NET numerics.</summary>
        /// <remarks>
        /// <para>See also: </para>
        /// <para>LU decomposition: http://en.wikipedia.org/wiki/LU_decomposition </para>
        /// <para>Invertible matrix: http://en.wikipedia.org/wiki/Invertible_matrix </para></remarks>
        public static void ExampleMathNetNumericsLU()
        {
            // Format matrix output to console
            var formatProvider = (CultureInfo)CultureInfo.InvariantCulture.Clone();
            formatProvider.TextInfo.ListSeparator = " ";

            // Create square matrix
            DenseMatrix matrix =  DenseMatrix.OfArray(new[,] { { 1.0, 2.0 }, { 3.0, 4.0 } });
            Console.WriteLine(@"Initial square matrix");
            Console.WriteLine(matrix.ToString("#0.00\t", formatProvider));
            Console.WriteLine();

            // Perform LU decomposition
            var lu = matrix.LU();
            Console.WriteLine(@"Perform LU decomposition");

            // 1. Lower triangular factor
            Console.WriteLine(@"1. Lower triangular factor");
            Console.WriteLine(lu.L.ToString("#0.00\t", formatProvider));
            Console.WriteLine();

            // 2. Upper triangular factor
            Console.WriteLine(@"2. Upper triangular factor");
            Console.WriteLine(lu.U.ToString("#0.00\t", formatProvider));
            Console.WriteLine();

            // 3. Permutations applied to LU factorization
            Console.WriteLine(@"3. Permutations applied to LU factorization");
            for (var i = 0; i < lu.P.Dimension; i++)
            {
                if (lu.P[i] > i)
                {
                    Console.WriteLine(@"Row {0} permuted with row {1}", lu.P[i], i);
                }
            }

            Console.WriteLine();

            // 4. Reconstruct initial matrix: PA = L * U
            var reconstruct = lu.L * lu.U;

            // The rows of the reconstructed matrix should be permuted to get the initial matrix
            reconstruct.PermuteRows(lu.P.Inverse());
            Console.WriteLine(@"4. Reconstruct initial matrix: PA = L*U");
            Console.WriteLine(reconstruct.ToString("#0.00\t", formatProvider));
            Console.WriteLine();

            // 5. Get the determinant of the matrix
            Console.WriteLine(@"5. Determinant of the matrix");
            Console.WriteLine(lu.Determinant);
            Console.WriteLine();

            // 6. Get the inverse of the matrix
            var matrixInverse = lu.Inverse();
            Console.WriteLine(@"6. Inverse of the matrix");
            Console.WriteLine(matrixInverse.ToString("#0.00\t", formatProvider));
            Console.WriteLine();

            // 7. Matrix multiplied by its inverse 
            var identity = matrix * matrixInverse;
            Console.WriteLine(@"7. Matrix multiplied by its inverse ");
            Console.WriteLine(identity.ToString("#0.00\t", formatProvider));
            Console.WriteLine();
        } // ExampleLU()


        /// <summary>Example of how to use QR decomposition from Math.NET numerics.</summary>
        /// <remarks> http://en.wikipedia.org/wiki/QR_decomposition QR decomposition </remarks>
        public static void ExampleMathNetNumericsQR()
        {
            // Format matrix output to console
            var formatProvider = (CultureInfo)CultureInfo.InvariantCulture.Clone();
            formatProvider.TextInfo.ListSeparator = " ";

            // Create 3 x 2 matrix
            var matrix = DenseMatrix.OfArray(new[,] { { 1.0, 2.0 }, { 3.0, 4.0 }, { 5.0, 6.0 } });
            Console.WriteLine(@"Initial 3x2 matrix");
            Console.WriteLine(matrix.ToString("#0.00\t", formatProvider));
            Console.WriteLine();

            // Perform QR decomposition (Householder transformations)
            var qr = matrix.QR();
            Console.WriteLine(@"QR decomposition (Householder transformations)");

            // 1. Orthogonal Q matrix
            Console.WriteLine(@"1. Orthogonal Q matrix");
            Console.WriteLine(qr.Q.ToString("#0.00\t", formatProvider));
            Console.WriteLine();

            // 2. Multiply Q matrix by its transpose gives identity matrix
            Console.WriteLine(@"2. Multiply Q matrix by its transpose gives identity matrix");
            Console.WriteLine(qr.Q.TransposeAndMultiply(qr.Q).ToString("#0.00\t", formatProvider));
            Console.WriteLine();

            // 3. Upper triangular factor R
            Console.WriteLine(@"3. Upper triangular factor R");
            Console.WriteLine(qr.R.ToString("#0.00\t", formatProvider));
            Console.WriteLine();

            // 4. Reconstruct initial matrix: A = Q * R
            var reconstruct = qr.Q * qr.R;
            Console.WriteLine(@"4. Reconstruct initial matrix: A = Q*R");
            Console.WriteLine(reconstruct.ToString("#0.00\t", formatProvider));
            Console.WriteLine();

            // Perform QR decomposition (Gram–Schmidt process)
            var gramSchmidt = matrix.GramSchmidt();
            Console.WriteLine(@"QR decomposition (Gram–Schmidt process)");

            // 5. Orthogonal Q matrix
            Console.WriteLine(@"5. Orthogonal Q matrix");
            Console.WriteLine(gramSchmidt.Q.ToString("#0.00\t", formatProvider));
            Console.WriteLine();

            // 6. Multiply Q matrix by its transpose gives identity matrix
            Console.WriteLine(@"6. Multiply Q matrix by its transpose gives identity matrix");
            Console.WriteLine((gramSchmidt.Q.Transpose() * gramSchmidt.Q).ToString("#0.00\t", formatProvider));
            Console.WriteLine();

            // 7. Upper triangular factor R
            Console.WriteLine(@"7. Upper triangular factor R");
            Console.WriteLine(gramSchmidt.R.ToString("#0.00\t", formatProvider));
            Console.WriteLine();

            // 8. Reconstruct initial matrix: A = Q * R
            reconstruct = gramSchmidt.Q * gramSchmidt.R;
            Console.WriteLine(@"8. Reconstruct initial matrix: A = Q*R");
            Console.WriteLine(reconstruct.ToString("#0.00\t", formatProvider));
            Console.WriteLine();
        } // ExampleQR()


        /// <summary>Example of how to use EVD (eigenvalue decomposition) from Math.NET Numerics.</summary>
        /// <remarks> http://en.wikipedia.org/wiki/Eigenvalue,_eigenvector_and_eigenspace EVD decomposition</remarks>
        public static void ExampleMathNetNumericsEVD()
        {
            // Format matrix output to console
            var formatProvider = (CultureInfo)CultureInfo.InvariantCulture.Clone();
            formatProvider.TextInfo.ListSeparator = " ";

            // Create square symmetric matrix
            var matrix = DenseMatrix.OfArray(new[,] { { 1.0, 2.0, 3.0 }, { 2.0, 1.0, 4.0 }, { 3.0, 4.0, 1.0 } });
            Console.WriteLine(@"Initial square symmetric matrix");
            Console.WriteLine(matrix.ToString("#0.00\t", formatProvider));
            Console.WriteLine();

            // Perform eigenvalue decomposition of symmetric matrix
            var evd = matrix.Evd();
            Console.WriteLine(@"Perform eigenvalue decomposition of symmetric matrix");

            // 1. Eigen vectors
            Console.WriteLine(@"1. Eigen vectors");
            Console.WriteLine(evd.EigenVectors.ToString("#0.00\t", formatProvider));
            Console.WriteLine();

            // 2. Eigen values as a complex vector
            Console.WriteLine(@"2. Eigen values as a complex vector");
            Console.WriteLine(evd.EigenValues.ToString("N", formatProvider));
            Console.WriteLine();

            // 3. Eigen values as the block diagonal matrix
            Console.WriteLine(@"3. Eigen values as the block diagonal matrix");
            Console.WriteLine(evd.D.ToString("#0.00\t", formatProvider));
            Console.WriteLine();

            // 4. Multiply V by its transpose VT
            var identity = evd.EigenVectors.TransposeAndMultiply(evd.EigenVectors);
            Console.WriteLine(@"4. Multiply V by its transpose VT: V*VT = I");
            Console.WriteLine(identity.ToString("#0.00\t", formatProvider));
            Console.WriteLine();

            // 5. Reconstruct initial matrix: A = V*D*V'
            var reconstruct = evd.EigenVectors * evd.D * evd.EigenVectors.Transpose();
            Console.WriteLine(@"5. Reconstruct initial matrix: A = V*D*V'");
            Console.WriteLine(reconstruct.ToString("#0.00\t", formatProvider));
            Console.WriteLine();

            // 6. Determinant of the matrix
            Console.WriteLine(@"6. Determinant of the matrix");
            Console.WriteLine(evd.Determinant);
            Console.WriteLine();

            // 7. Rank of the matrix
            Console.WriteLine(@"7. Rank of the matrix");
            Console.WriteLine(evd.Rank);
            Console.WriteLine();

            // Fill matrix by random values
            var rnd = new Random(1);
            for (var i = 0; i < matrix.RowCount; i++)
            {
                for (var j = 0; j < matrix.ColumnCount; j++)
                {
                    matrix[i, j] = rnd.NextDouble();
                }
            }

            Console.WriteLine(@"Fill matrix by random values");
            Console.WriteLine(matrix.ToString("#0.00\t", formatProvider));
            Console.WriteLine();

            // Perform eigenvalue decomposition of non-symmetric matrix
            evd = matrix.Evd();
            Console.WriteLine(@"Perform eigenvalue decomposition of non-symmetric matrix");

            // 8. Eigen vectors
            Console.WriteLine(@"8. Eigen vectors");
            Console.WriteLine(evd.EigenVectors.ToString("#0.00\t", formatProvider));
            Console.WriteLine();

            // 9. Eigen values as a complex vector
            Console.WriteLine(@"9. Eigen values as a complex vector");
            Console.WriteLine(evd.EigenValues.ToString("N", formatProvider));
            Console.WriteLine();

            // 10. Eigen values as the block diagonal matrix
            Console.WriteLine(@"10. Eigen values as the block diagonal matrix");
            Console.WriteLine(evd.D.ToString("#0.00\t", formatProvider));
            Console.WriteLine();
			
			
			// WARNING - TODO:
			// Lines below are commented because of errors in MonoDevelop. 
			// Uncomment when this is solved!
			
            // 11. Multiply A * V
            //var av = matrix * evd.EigenVectors();
            //Console.WriteLine(@"11. Multiply A * V");
            //Console.WriteLine(av.ToString("#0.00\t", formatProvider));
            //Console.WriteLine();

            // 12. Multiply V * D
            var vd = evd.EigenVectors * evd.D;
            Console.WriteLine(@"12. Multiply V * D");
            Console.WriteLine(vd.ToString("#0.00\t", formatProvider));
            Console.WriteLine();

            // 13. Reconstruct non-symmetriv matrix A = V * D * Vinverse
            reconstruct = evd.EigenVectors * evd.D * evd.EigenVectors.Inverse();
            Console.WriteLine(@"13. Reconstruct non-symmetriv matrix A = V * D * Vinverse");
            Console.WriteLine(reconstruct.ToString("#0.00\t", formatProvider));
            Console.WriteLine();

            // 14. Determinant of the matrix
            Console.WriteLine(@"14. Determinant of the matrix");
            Console.WriteLine(evd.Determinant);
            Console.WriteLine();

            // 15. Rank of the matrix
            Console.WriteLine(@"15. Rank of the matrix");
            Console.WriteLine(evd.Rank);
            Console.WriteLine();
        } // ExampleEVD()

        /// <summary>Example of how to use SVD ( singular value decomposition) from Math.NET numerics.</summary>
        /// <remarks> http://en.wikipedia.org/wiki/Singular_value_decomposition SVD decomposition</remarks>
        public static void ExampleMathNetNumericsSVD()
        {
            // Format matrix output to console
            var formatProvider = (CultureInfo)CultureInfo.InvariantCulture.Clone();
            formatProvider.TextInfo.ListSeparator = " ";

            // Create square matrix
            var matrix = DenseMatrix.OfArray(new[,] { { 4.0, 1.0 }, { 3.0, 2.0 } });
            Console.WriteLine(@"Initial square matrix");
            Console.WriteLine(matrix.ToString("#0.00\t", formatProvider));
            Console.WriteLine();

            // Perform full SVD decomposition
            var svd = matrix.Svd(true);
            Console.WriteLine(@"Perform full SVD decomposition");

            // 1. Left singular vectors
            Console.WriteLine(@"1. Left singular vectors");
            Console.WriteLine(svd.U.ToString("#0.00\t", formatProvider));
            Console.WriteLine();

            // 2. Singular values as vector
            Console.WriteLine(@"2. Singular values as vector");
            Console.WriteLine(svd.S.ToString("#0.00\t", formatProvider));
            Console.WriteLine();

            // 3. Singular values as diagonal matrix
            Console.WriteLine(@"3. Singular values as diagonal matrix");
            Console.WriteLine(svd.W.ToString("#0.00\t", formatProvider));
            Console.WriteLine();

            // 4. Right singular vectors
            Console.WriteLine(@"4. Right singular vectors");
            Console.WriteLine(svd.VT.ToString("#0.00\t", formatProvider));
            Console.WriteLine();

            // 5. Multiply U matrix by its transpose
            var identinty = svd.U * svd.U.Transpose();
            Console.WriteLine(@"5. Multiply U matrix by its transpose");
            Console.WriteLine(identinty.ToString("#0.00\t", formatProvider));
            Console.WriteLine();

            // 6. Multiply V matrix by its transpose
            identinty = svd.VT.TransposeAndMultiply(svd.VT);
            Console.WriteLine(@"6. Multiply V matrix by its transpose");
            Console.WriteLine(identinty.ToString("#0.00\t", formatProvider));
            Console.WriteLine();

            // 7. Reconstruct initial matrix: A = U*Σ*VT
            var reconstruct = svd.U * svd.W * svd.VT;
            Console.WriteLine(@"7. Reconstruct initial matrix: A = U*S*VT");
            Console.WriteLine(reconstruct.ToString("#0.00\t", formatProvider));
            Console.WriteLine();

            // 8. Condition Number of the matrix
            Console.WriteLine(@"8. Condition Number of the matrix");
            Console.WriteLine(svd.ConditionNumber);
            Console.WriteLine();

            // 9. Determinant of the matrix
            Console.WriteLine(@"9. Determinant of the matrix");
            Console.WriteLine(svd.Determinant);
            Console.WriteLine();

            // 10. 2-norm of the matrix
            Console.WriteLine(@"10. 2-norm of the matrix");
            Console.WriteLine(svd.L2Norm); 
            Console.WriteLine();

            // 11. Rank of the matrix
            Console.WriteLine(@"11. Rank of the matrix");
            Console.WriteLine(svd.Rank);
            Console.WriteLine();

            // Perform partial SVD decomposition, without computing the singular U and VT vectors
            svd = matrix.Svd(false);
            Console.WriteLine(@"Perform partial SVD decomposition, without computing the singular U and VT vectors");

            // 12. Singular values as vector
            Console.WriteLine(@"12. Singular values as vector");
            Console.WriteLine(svd.S.ToString("#0.00\t", formatProvider));
            Console.WriteLine();

            // 13. Singular values as diagonal matrix
            Console.WriteLine(@"13. Singular values as diagonal matrix");
            Console.WriteLine(svd.W.ToString("#0.00\t", formatProvider));
            Console.WriteLine();

            // 14. Access to left singular vectors when partial SVD decomposition was performed
            try
            {
                Console.WriteLine(@"14. Access to left singular vectors when partial SVD decomposition was performed");
                Console.WriteLine(svd.U.ToString("#0.00\t", formatProvider));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine();
            }

            // 15. Access to right singular vectors when partial SVD decomposition was performed
            try
            {
                Console.WriteLine(@"15. Access to right singular vectors when partial SVD decomposition was performed");
                Console.WriteLine(svd.VT.ToString("#0.00\t", formatProvider));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine();
            }
        } // ExampleSVD()


        /// <summary>Example of how to use Choleski decomposition from Math.NET Numerics.</summary>
        /// <remarks> http://en.wikipedia.org/wiki/Cholesky_decomposition </remarks> 
        public static void ExampleMathNetNumericsCholesky()
        {
            // Format matrix output to console
            var formatProvider = (CultureInfo)CultureInfo.InvariantCulture.Clone();
            formatProvider.TextInfo.ListSeparator = " ";

            // Create square, symmetric, positive definite matrix
            DenseMatrix matrix = DenseMatrix.OfArray(new[,] { { 2.0, 1.0 }, { 1.0, 2.0 } });
            Console.WriteLine(@"Initial square, symmetric, positive definite matrix");
            Console.WriteLine(matrix.ToString("#0.00\t", formatProvider));
            Console.WriteLine();

            // Perform Cholesky decomposition
            var cholesky = matrix.Cholesky();
            Console.WriteLine(@"Perform Cholesky decomposition");

            // 1. Lower triangular form of the Cholesky matrix
            Console.WriteLine(@"1. Lower triangular form of the Cholesky matrix");
            Console.WriteLine(cholesky.Factor.ToString("#0.00\t", formatProvider));
            Console.WriteLine();

            // 2. Reconstruct initial matrix: A = L * LT
            var reconstruct = cholesky.Factor * cholesky.Factor.Transpose();
            Console.WriteLine(@"2. Reconstruct initial matrix: A = L*LT");
            Console.WriteLine(reconstruct.ToString("#0.00\t", formatProvider));
            Console.WriteLine();

            // 3. Get determinant of the matrix
            Console.WriteLine(@"3. Determinant of the matrix");
            Console.WriteLine(cholesky.Determinant);
            Console.WriteLine();

            // 4. Get log determinant of the matrix
            Console.WriteLine(@"4. Log determinant of the matrix");
            Console.WriteLine(cholesky.DeterminantLn);
            Console.WriteLine();
        } // ExampleCholeski() 

        #endregion ExamplesMathNetNumerics


    }  // class TestSpeedCpu
}

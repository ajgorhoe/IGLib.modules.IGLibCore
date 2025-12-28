
#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using IG.Lib;

//using F = IG.Num.M;

namespace IG.Num
{

    /// <summary>2D vector, struct implementation.</summary>
    /// <remarks>Name of this struct is not in line with conventions. This is intentional in order to
    /// prevent mistaking struct implementation for class interpretation.</remarks>
    /// $A Igor Jul08; Oct10;
    public struct vec2
    {
        public double x, y;

        #region Initialization

        /// <summary>Copy constructor.
        /// Initializes components of a 2D vector with components of the specified vector.</summary>
        /// <param name="v">Vectr whose components are copied to the initialized vector.</param>
        public vec2(vec2 v)
        {
            x = v.x; y = v.y; 
        }

        /// <summary>Initializes components of a 2D vector with the specified values.</summary>
        /// <param name="x">Value assigned to the first component.</param>
        /// <param name="y">Value assigned to the second component.</param>
        public vec2(double x, double y)
        {
            this.x = x; this.y = y; 
        }

        /// <summary>Initializes all component of a 2D vector with the specified value.</summary>
        /// <param name="comp">Value assigned to all vector components.</param>
        public vec2(double comp)
        {
            x = y = comp;
        }

        /// <summary>Sets components of the vector to 0.0.</summary>
        public void Zero()
        { x = y = 0.0; }

        /// <summary>Sets components of the vector to the specified value.</summary>
        public void Set(double comp)
        { x = y = comp; }

        #endregion Initialization


        #region Access

        /// <summary>Index operator.</summary>
        /// <param name="i">Component index.</param>
        /// <returns>The specified component of a 2D vector.</returns>
        public double this[int i]
        {
            get
            {
                if (i == 0)
                    return x;
                else if (i == 1)
                    return y;
                else
                    throw new IndexOutOfRangeException("2D vector does not have component [" + i + "]");
            }
            set
            {
                if (i == 0)
                    x = value;
                else if (i == 1)
                    y = value;
                else
                    throw new IndexOutOfRangeException("2D vector does not have component [" + i + "]");
            }
        }  // this[i]

        #endregion Access


        #region Auxiliary

        /// <summary>Returns the hash code (hash function) of the current vector.</summary>
        /// <remarks>
        /// <para>This method should be consistent with the <see cref="VectorBase.GetHashCode()"/> method, 
        /// which is standard for implementations of the <see cref="IVector"/> interface.</para>
        /// <para>Two vectors that have equal all elements will produce the same hash codes.</para>
        /// <para>Probability that two different vectors will produce the same hash code is small but it exists.</para>
        /// <para>Overrides the <see cref="object.GetHashCode"/> method.</para>
        /// </remarks>
        public override int GetHashCode()
        {
            int ret = 0;
            ret ^= x.GetHashCode();
            ret ^= y.GetHashCode();
            return ret;
        }

        /// <summary>Returns a value indicating whether the specified object is equal to the current vector.
        /// <para>True is returned if the object of type <see cref="vec2"/>) and has equal elements as the current vector.</para></summary>
        /// <remarks> <para>Overrides the <see cref="object.Equals(object)"/> method.</para></remarks>
        public override bool Equals(Object obj)
        {
            if (obj is vec2)
                if (this == (vec2)obj)
                    return true; 
            return false;
        }

        #endregion Auxiliary


        #region Operations

        /// <summary>Gets a 2 norm of the current 2D vector.</summary>
        public double Norm
        { get { return Norm2; } }

        /// <summary>Gets a 2 norm of the current 2D vector.</summary>
        public double Norm2 { get { return Math.Sqrt(x * x + y * y); } }

        /// <summary>Gets an 1 norm of the current 2D vector -
        /// sum of absolute values of components.</summary>
        public double Norm1 { get { return Math.Abs(x) + Math.Abs(y); } }

        /// <summary>Gets an infinity norm of the current 2D vector -
        /// maximal absolute component value.</summary>
        public double NormInf
        {
            get
            {
                double ret = 0, a;
                if ((a = Math.Abs(x)) > ret) ret = a;
                if ((a = Math.Abs(y)) > ret) ret = a;
                return ret;
            }
        }

        /// <summary>Returns normalized this 2D vector.</summary>
        public vec2 Normalized()
        { return Multiply(1 / Norm); }

        /// <summary>Returns normalized this 2D vector in 1 norm.</summary>
        public vec2 Normalized1()
        { return Multiply(1 / Norm1); }

        /// <summary>Returns normalized this 2D vector in 2 norm.</summary>
        public vec2 Normalized2()
        { return Multiply(1 / Norm2); }

        /// <summary>Returns normalized this 2D vector in infinity norm.</summary>
        public vec2 NormalizedInfinity()
        { return Multiply(1 / NormInf); }


        /// <summary>Returns scalar product of the current and the specified vector.</summary>
        public double ScalarProduct(vec2 v)
        {
            return x * v.x + y * v.y;
        }

        /// <summary>Returns vector product of the current and the specified vector.</summary>
        public vec3 VectorProduct(vec2 v)
        {
            return new vec3(
                    0.0,
                    0.0,
                    x * v.y - y * v.x
                );
        }

        /// <summary>Returns vector product of the current and the specified vector.</summary>
        public vec3 CrossProduct(vec2 v)
        { return VectorProduct(v); }

        /// <summary>Returns vector product of the current and the specified vector.</summary>
        public vec3 Cross(vec2 v)
        { return VectorProduct(v); }

        /// <summary>Returns dyadic product of the current and the specified vector.</summary>
        public mat2 DyadicProduct(vec2 v)
        {
            return new mat2(
                    x * v.x, x * v.y,
                    y * v.x, y * v.y
                );
        }


        /// <summary>Returns the current vector multiplied by the specified scalar.</summary>
        /// <param name="k">Factor by which the current vector is multiplied.</param>
        public vec2 Multiply(double k)
        {
            return new vec2(k * x, k * y);
        }

        /// <summary>Returns sum of the current vector and the specified vector.</summary>
        public vec2 Add(vec2 a)
        {
            return new vec2(x + a.x, y + a.y);
        }

        /// <summary>Returns difference between the current vector and the specified vector.</summary>
        public vec2 Subtract(vec2 a)
        {
            return new vec2(x - a.x, y - a.y);
        }


        #endregion Operations


        #region StaticMethods

        /// <summary>Returns a copy of the specified 2D vector.</summary>
        /// <param name="v">Vector whose copy is returned.</param>
        public static vec2 Copy(vec2 v)
        {
            return new vec2(v);
        }

        /// <summary>Negates the specified vector and stores its copy in the resulting vector.</summary>
        /// <param name="v">Vectr to be negated.</param>
        /// <param name="res">Vector where the result is stored.</param>
        public static void Negate(vec2 v, ref vec2 res)
        {
            res.x = -v.x; res.y = -v.y; 
        }

        #endregion StaticMethods


        #region OperatorsOverloading

        /// <summary>Unary plus, returns the operand.</summary>
        public static vec2 operator +(vec2 v)
        {
            return vec2.Copy(v);
        }

        /// <summary>Unary negation, returns the negative operand.</summary>
        public static vec2 operator -(vec2 v)
        {
            return new vec2(-v.x, -v.y);
        }

        /// <summary>Vector addition.</summary>
        public static vec2 operator +(vec2 a, vec2 b)
        {
            return a.Add(b);
        }


        /// <summary>Vector subtraction.</summary>
        public static vec2 operator -(vec2 a, vec2 b)
        {
            return a.Subtract(b); ;
        }


        /// <summary>Scalar product of two 2D vectors.</summary>
        public static double operator *(vec2 a, vec2 b)
        {
            return a.ScalarProduct(b);
        }

        /// <summary>Product of a 2D vector by a scalar.</summary>
        public static vec2 operator *(vec2 a, double b)
        {
            return a.Multiply(b);
        }

        /// <summary>Product of a 2D vector by a scalar.</summary>
        public static vec2 operator *(double a, vec2 b)
        {
            return b.Multiply(a);
        }

        /// <summary>Vector subtraction.</summary>
        public static vec2 operator /(vec2 a, double b)
        {
            return new vec2(a.x / b, a.y / b);
        }


        // REMARK: For reference types it is not a good idea to overload the == and != operators because 
        // in this case we can not use these operators for comparison of references!
        // Since this is a value type, we can to this without fear.

        /// <summary>Vector comparison.</summary>
        public static bool operator ==(vec2 a, vec2 b)
        {
            return a.x == b.x && a.y == b.y ;
        }

        /// <summary>Vector comparison, returns true if vectors are different.</summary>
        public static bool operator !=(vec2 a, vec2 b)
        {
            return a.x != b.x || a.y != b.y ;
        }


        #endregion  OperatorsOverloading


        #region InputOutput

        /// <summary>Returns string representation of the 2D vector.</summary>
        public override string ToString()
        {
            return
            "{" + x + ", " + y + "}";
        }

        /// <summary>Reads 2D vector components from a console.</summary>
        public void Read()
        {
            Read(null);
        }


        /// <summary>Reads 2D vector components from a console.</summary>
        /// <param name="name">Name of the vector to be read; it is written as orientation to the user
        /// and can be null.</param>
        public void Read(string name)
        {
            Console.WriteLine();
            Console.Write("  " + name + ".x: "); Cons.Read(ref x);
            Console.Write("  " + name + ".y: "); Cons.Read(ref y);
        }

        #endregion InputOutput

    }  // struct vec2


    /// <summary>2D matrix, struct implementation.</summary>
    /// <remarks>Name of this struct is not in line with conventions. This is intentional in order to
    /// prevent mistaking struct implementation for class interpretation.</remarks>
    /// $A Igor Jul08, Oct10;
    public struct mat2
    {
        public double xx, xy, yx, yy;

        #region Initialization

        /// <summary>Copy constructor.
        /// Initializes components of a 2D matrix with components of the specified matrix.</summary>
        /// <param name="m">Matrix whose components are copied to the initialized matrix.</param>
        public mat2(mat2 m)
        {
            xx = m.xx; xy = m.xy;
            yx = m.yx; yy = m.yy;
        }

        /// <summary>Initializes 2D matrix structure with the specified components.</summary>
        public mat2(double xx, double xy, 
                    double yx, double yy )
        {
            this.xx = xx; this.xy = xy; 
            this.yx = yx; this.yy = yy; 
        }

        /// <summary>Initializes 2D with the specified component.</summary>
        /// <param name="component">Value that is assigned to all matrix components.</param>
        public mat2(double component)
        {
            this.xx = component; this.xy = component; 
            this.yx = component; this.yy = component; 
        }

        /// <summary>Initializes 2D matrix structure with the specified rows.</summary>
        public mat2(vec2 xrow, vec2 yrow)
        {
            this.xx = xrow.x; this.xy = xrow.y; 
            this.yx = yrow.x; this.yy = yrow.y; 
        }

        /// <summary>Sets components of the vector to 0.0.</summary>
        public void Zero()
        {
            xx = xy =
            yx = yy = 0.0;
        }

        /// <summary>Sets components of the vector to the specified value.</summary>
        public void Set(double comp)
        {
            xx = xy = 
            yx = yy = comp;
        }


        #endregion Initialization


        #region Access

        /// <summary>Index operator.</summary>
        /// <param name="i">Component index.</param>
        /// <param name="j">Component index.</param>
        /// <returns>The specified component of a 2D vector.</returns>
        public double this[int i, int j]
        {
            get
            {
                if (i == 0)
                {
                    if (j == 0)
                        return xx;
                    else if (j == 1)
                        return xy;
                    throw new IndexOutOfRangeException("2D matrix does not have component ["
                            + i + ", " + j + "]");
                } 
                else if (i == 1)
                {
                    if (j == 0)
                        return yx;
                    else if (j == 1)
                        return yy;
                    throw new IndexOutOfRangeException("2D matrix does not have component ["
                        + i + ", " + j + "]");
                }
                throw new IndexOutOfRangeException("2D vector does not have component [" + i + "]");
            }
            set
            {
                if (i == 0)
                {
                    if (j == 0)
                        xx = value;
                    else if (j == 1)
                        xy = value;
                    else
                        throw new IndexOutOfRangeException("2D matrix does not have component ["
                            + i + ", " + j + "]");
                }
                else if (i == 1)
                {
                    if (j == 0)
                        yx = value;
                    else if (j == 1)
                        yy = value;
                    throw new IndexOutOfRangeException("2D matrix does not have component ["
                        + i + ", " + j + "]");
                }
                else
                    throw new IndexOutOfRangeException("2D matrix does not have component ["
                        + i + ", " + j + "]");
            }
        } // this[i,j]

        /// <summary>Gets or sets the x-row of the 2D matrix.</summary>
        public vec2 rowx
        {
            get { return new vec2(xx, xy); }
            set { xx = value.x; xy = value.y; }
        }

        /// <summary>Gets or sets the y-row of the 2D matrix.</summary>
        public vec2 rowy
        {
            get { return new vec2(yx, yy); }
            set { yx = value.x; yy = value.y; }
        }

        /// <summary>Gets or sets the x-column of the 2D matrix.</summary>
        public vec2 columnx
        {
            get { return new vec2(xx, yx); }
            set { xx = value.x; yx = value.y; }
        }

        /// <summary>Gets or sets the y-column of the 2D matrix.</summary>
        public vec2 columny
        {
            get { return new vec2(xy, yy); }
            set { xy = value.x; yy = value.y; }
        }

        #endregion Access


        #region Auxiliary


        /// <summary>Returns the hash code (hash function) of the current matrix.</summary>
        /// <remarks>
        /// <para>This method should be consistent with the <see cref="MatrixBase.GetHashCode()"/> method, 
        /// which is standard for implementations of the <see cref="IMatrix"/> interface.</para>
        /// <para>Two matrices that have the same equal all elements will produce the same hash codes.</para>
        /// <para>Probability that two different matrixes will produce the same hash code is small but it exists.</para>
        /// <para>Overrides the <see cref="object.GetHashCode"/> method.</para>
        /// </remarks>
        public override int GetHashCode()
        {
            int ret = 0;
            ret ^= xx.GetHashCode();
            ret ^= xy.GetHashCode();
            ret ^= yx.GetHashCode();
            ret ^= yy.GetHashCode();
            return ret;
        }

        /// <summary>Returns a value indicating whether the specified object is equal to the current matrix.
        /// <para>True is returned if the object is of type <see cref="mat2"/>) and has equal elements as the current matrix.</para></summary>
        /// <remarks> <para>Overrides the <see cref="object.Equals(object)"/> method.</para></remarks>
        public override bool Equals(Object obj)
        {
            if (obj is mat2)
                if (this == (mat2)obj)
                    return true;
            return false;
        }

        #endregion Auxiliary


        #region Operations

        /// <summary>Get Forbenius (or euclidean) norm of the matrix - 
        /// square root of sum of squares of components.</summary>
        public double NormForbenius
        {
            get
            {
                return Math.Sqrt(
                      xx * xx + xy * xy
                    + yx * yx + yy * yy
                    );
            }
        }

        /// <summary>Get Forbenius (or euclidean) norm of the matrix - 
        /// square root of sum of squares of components.</summary>
        public double NormEuclidean
        { get { return NormForbenius; } }

        /// <summary>Get Forbenius (or euclidean) norm of the matrix - 
        /// square root of sum of squares of components.</summary>
        public double Norm
        { get { return NormForbenius; } }

        /// <summary>Get the 1 norm of the matrix -
        /// maximum over columns of sum of absolute values of components.</summary>
        public double Norm1
        {
            get
            {
                double ret = 0.0, s;
                s = Math.Abs(xx) + Math.Abs(yx);
                if (s > ret)
                    ret = s;
                s = Math.Abs(xy) + Math.Abs(yy);
                if (s > ret)
                    ret = s;
                return ret;
            }
        }


        /// <summary>Get the infinity norm of the matrix -
        /// maximum over rows of sum of absolute values of components.</summary>
        public double NormInfinity
        {
            get
            {
                double ret = 0.0, s;
                s = Math.Abs(xx) + Math.Abs(xy);
                if (s > ret)
                    ret = s;
                s = Math.Abs(yx) + Math.Abs(yy);
                if (s > ret)
                    ret = s;
                return ret;
            }
        }

        /// <summary>Returns this matrix normalized with Euclidean norm.</summary>
        public mat2 NormalizedEuclidean()
        { return Multiply(1 / NormEuclidean); }

        /// <summary>Returns this matrix normalized with Euclidean norm.</summary>
        public mat2 NormalizedForbenius()
        { return Multiply(1 / NormForbenius); }

        /// <summary>Returns this matrix normalized with 1 norm.</summary>
        public mat2 Normalized1()
        { return Multiply(1 / Norm1); }

        /// <summary>Returns this matrix normalized with infinity norm.</summary>
        public mat2 NormalizedInfinity()
        { return Multiply(1 / NormInfinity); }



        /// <summary>Gets matrix determinant.</summary>
        public double Determinant
        {
            get
            {
                return xx * yy - xy * yx;
            }
        }

        /// <summary>Gets matrix determinant.</summary>
        public double Det
        { get { return Determinant; } }

        /// <summary>Gets matrix trace (sum of diagonal elements).</summary>
        public double Trace
        { get { return xx + yy; } }

        /// <summary>Gets transpose of the current matrix.</summary>
        public mat2 Transpose
        {
            get
            {
                mat2 ret;
                ret.xx = xx; ret.xy = yx;
                ret.yx = xy; ret.yy = yy;
                return ret;
            }
        }


        /// <summary>Gets transpose of the current matrix.</summary>
        public mat2 T
        { get { return Transpose; } }


        /// <summary>Gets inverse of the current matrix.</summary>
        public mat2 Inverse
        {
            get
            {
                double d = this.Determinant;
                if (d == 0)
                    throw new InvalidOperationException("2D matrix is not invertible.");
                mat2 ret;
                ret.xx = yy / d;
                ret.xy = -xy / d;
                ret.yx = -yx / d;
                ret.yy = xx / d;
                return ret;
            }
        }

        /// <summary>Gets inverse of the current matrix.</summary>
        public mat2 Inv { get { return Inverse; } }

        /// <summary>Returns solution of system of equations with the current system matrix and 
        /// the specified right-hand sides.</summary>
        /// <param name="b">Vector of right-hand sides of equations.</param>
        /// <returns></returns>
        public vec2 Solve(vec2 b)
        { return Inv * b; }


        /// <summary>Returns sum of the current matrix and the specified matrix.</summary>
        public mat2 Add(mat2 a)
        {
            mat2 ret;
            ret.xx = xx + a.xx; ret.xy = xy + a.xy; 
            ret.yx = yx + a.yx; ret.yy = yy + a.yy; 
            return ret;
        }

        /// <summary>Returns difference between the current matrix and the specified matrix.</summary>
        public mat2 Subtract(mat2 a)
        {
            mat2 ret;
            ret.xx = xx - a.xx; ret.xy = xy - a.xy; 
            ret.yx = yx - a.yx; ret.yy = yy - a.yy; 
            return ret;
        }

        /// <summary>Right-multiplies the current 2D matrix with the specified matrix and returns the product.</summary>
        /// <param name="b">Right-hand side factor of multiplication.</param>
        /// <returns>this*b</returns>
        public mat2 MultiplyRight(mat2 b)
        {
            mat2 prod;
            prod.xx = xx * b.xx + xy * b.yx;
            prod.xy = xx * b.xy + xy * b.yy;

            prod.yx = yx * b.xx + yy * b.yx;
            prod.yy = yx * b.xy + yy * b.yy;

            return prod;
        }

        /// <summary>Left-multiplies the current 2D matrix with the specified matrix and returns the product.</summary>
        /// <param name="b">Left-hand side factor of multiplication.</param>
        /// <returns>b*this</returns>
        public mat2 MultiplyLeft(mat2 b)
        {
            return b.MultiplyRight(this);
        }


        /// <summary>Right-multiplies the current 2D matrix with the specified 2D vector and returns the product.</summary>
        /// <param name="b">Right-hand side factor of multiplication.</param>
        /// <returns>this*b</returns>
        public vec2 Multiply(vec2 b)
        {
            vec2 prod;
            prod.x = xx * b.x + xy * b.y;
            prod.y = yx * b.x + yy * b.y;
            return prod;
        }

        /// <summary>Multiplies the current 2D matrix with the specified scalar and returns the product.</summary>
        /// <param name="b">Factor of multiplication.</param>
        /// <returns>this*b</returns>
        public mat2 Multiply(double b)
        {
            return new mat2(
                xx * b, xy * b, 
                yx * b, yy * b
                );
        }

        #endregion Operations


        #region StaticMethods

        /// <summary>Returns a copy of the specified 2D matrix.</summary>
        /// <param name="m">Matrix whose copy is returned.</param>
        public static mat2 Copy(mat2 m)
        {
            return new mat2(m);
        }

        /// <summary>Negates the specified 2D matrix and stores its copy in the resulting matrix.</summary>
        /// <param name="m">Matrix to be negated.</param>
        /// <param name="res">Matrix where the result is stored.</param>
        public static void Negate(mat2 m, ref mat2 res)
        {
            res.xx = -m.xx; res.xy = -m.xy; 
            res.yx = -m.yx; res.yy = -m.yy; 
        }



        /// <summary>Calculates eigenvectors and eigenvalues of a 2x2 matrix a and stores
        /// eigenvectors to lines of eigenvec and eigenvalues to eigenval. eigenvec
        /// can be the same matrix as a. The number of different real eigenvalues is
        /// returned.
        ///   Ref.: linalg.nb
        /// Not tested yet!
        /// </summary>
        /// <param name="a">2D Matrix whose eigenvalues and eigenvectors are calculated.</param>
        /// <param name="eigenvec">Matrix where eigenvectors are stored as rows.</param>
        /// <param name="eigenval">Vector where eigenvalues are stored.</param>
        /// <returns>The number of different real eigenvalues.</returns>
        /// $A Igor Aug08, Oct10;
        public static int EigenSystem2d(mat2 a, ref mat2 eigenvec, ref vec2 eigenval)
        {
            double D2 = a.xx * a.xx + 4 * a.xy * a.yx - 2 * a.xx * a.yy + a.yy * a.yy;
            if (D2 < 0)
                throw new ArgumentException("2D eigensystem has no real solutions.");
            double D = Math.Sqrt(D2);
            double norm;
            eigenval.x = 0.5*(a.xx + a.yy - D);
            eigenval.y = 0.5*(a.xx + a.yy + D);
            // First eigenvector (stored as row)
            eigenvec.xx = (a.xx-a.yy-D)/(2.0*a.yx);
            eigenvec.xy=1;
            norm = Math.Sqrt(eigenvec.xx*eigenvec.xx+eigenvec.xy*eigenvec.xy);
            if (norm > 0)
            {
                eigenvec.xx /= norm;
                eigenvec.xy /= norm;
            }
            // Second eigenvector (stored as row):
            eigenvec.yx = (a.xx-a.yy+D)/(2.0*a.yx);
            eigenvec.yy = 1;
            norm = Math.Sqrt(eigenvec.yx*eigenvec.yx+eigenvec.yy*eigenvec.yy);
            if (norm > 0)
            {
                eigenvec.yx /= norm;
                eigenvec.yy /= norm;
            }
            return 2;
        }  // EigenSystem2d



        #endregion StaticMethods


        #region OperatorsOverloading

        /// <summary>Unary plus for 2D matrices, returns the operand.</summary>
        public static mat2 operator +(mat2 m)
        {
            return mat2.Copy(m);
        }

        /// <summary>Unary negation for 2D matrices, returns the negative operand.</summary>
        public static mat2 operator -(mat2 a)
        {
            return new mat2(
                -a.xx, -a.xy,
                -a.yx, -a.yy
                );
        }

        /// <summary>Matrix addition in 2D.</summary>
        public static mat2 operator +(mat2 a, mat2 b)
        {
            return a.Add(b);
        }


        /// <summary>Matrix subtraction in 2D.</summary>
        public static mat2 operator -(mat2 a, mat2 b)
        {
            return a.Subtract(b); ;
        }

        /// <summary>Matrix multiplication in 2D.</summary>
        public static mat2 operator *(mat2 a, mat2 b)
        {
            return a.MultiplyRight(b); ;
        }

        /// <summary>Matrix with vector multiplication in 2D.</summary>
        public static vec2 operator *(mat2 a, vec2 b)
        {
            return a.Multiply(b); ;
        }


        /// <summary>Product of a 2D matrix by a scalar.</summary>
        public static mat2 operator *(mat2 a, double b)
        {
            return a.Multiply(b);
        }

        /// <summary>Product of a 2D matrix by a scalar.</summary>
        public static mat2 operator *(double a, mat2 b)
        {
            return b.Multiply(a);
        }

        /// <summary>Division of a 2D matrix by a scalar.</summary>
        public static mat2 operator /(mat2 a, double b)
        {
            return a.Multiply(1 / b);
        }


        // REMARK: For reference types it is not a good idea to overload the == and != operators because 
        // in this case we can not use these operators for comparison of references!
        // Since this is a value type, we can to this without fear.

        /// <summary>Vector comparison.</summary>
        public static bool operator ==(mat2 a, mat2 b)
        {
            return
                a.xx == b.xx && a.xy == b.xy && 
                a.yx == b.yx && a.yy == b.yy;
        }

        /// <summary>Vector comparison, returns true if vectors are different.</summary>
        public static bool operator !=(mat2 a, mat2 b)
        {
            return
                a.xx != b.xx && a.xy != b.xy &&
                a.yx != b.yx && a.yy != b.yy;
        }


        #endregion  OperatorsOverloading


        #region InputOutput

        /// <summary>Returns a string representation of this 2D matrix.</summary>
        public override string ToString()
        {
            return
                  "{" + Environment.NewLine
                + "  {" + xx + ", " + xy +  "}, " + Environment.NewLine
                + "  {" + yx + ", " + yy +  "}, " + Environment.NewLine
                + "} ";
        }

        /// <summary>Reads this 2D matrix components from a console.</summary>
        public void Read()
        {
            Read(null);
        }


        /// <summary>Reads this 2D matrix components from a console.</summary>
        /// <param name="name">Name of the matrix to be read; it is written as orientation to the user
        /// and can be null.</param>
        public void Read(string name)
        {
            Console.Write("  " + name + ".xx: "); Cons.Read(ref xx);
            Console.Write("  " + name + ".xy: "); Cons.Read(ref xy);
            Console.Write("  " + name + ".yx: "); Cons.Read(ref yx);
            Console.Write("  " + name + ".yy: "); Cons.Read(ref yy);
        }

        #endregion InputOutput


        /// <summary>A short example of how to use mat2 and vec2 structs.</summary>
        public static void Example()
        {
            mat2 A = new mat2(
                5, 1,
                0.12, 4
                );
            mat2 B = A.T;
            mat2 AInv = A.Inv;
            mat2 P = A * AInv;
            Console.WriteLine("Original matrix A: " + Environment.NewLine + A);
            Console.WriteLine("Determinant of A: " + A.Determinant);
            Console.WriteLine("Transpose of A: " + Environment.NewLine + B);
            Console.WriteLine("Inverse of A: " + Environment.NewLine + AInv);
            Console.WriteLine("Product of A and its inverse: " + Environment.NewLine + P);
            vec2 a = new vec2(1.111, 2.222);
            vec2 p, p2;
            p = A * a;
            p2 = AInv * p;
            Console.WriteLine("Vector a: " + Environment.NewLine + a);
            Console.WriteLine("Product A*a: " + Environment.NewLine + p);
            Console.WriteLine("Product A-1*A*a: " + Environment.NewLine + p2);
            Console.WriteLine("");

            bool answer = true;
            do
            {
                Console.Write("Do you want to insert a matrix to analyse it (0/1)? ");
                Cons.Read(ref answer);
                if (answer)
                {
                    vec2 b = new vec2(), x;
                    Console.WriteLine("Insert 2D matrix A to be analysed!");
                    A.Read("A");
                    Console.WriteLine("Insert right-hand side 2D vector b!");
                    b.Read("b");
                    Console.WriteLine();
                    Console.WriteLine("Matrix A: " + Environment.NewLine + A);
                    Console.WriteLine("Vector B: " + Environment.NewLine + b);
                    Console.WriteLine("Det A: " + A.Determinant);
                    Console.WriteLine("||A^-1*A||: " + (A.Inv * A).Norm);
                    Console.WriteLine("Solution x of A*x=b: " + (x = A.Inverse * b));
                    Console.WriteLine("||A*x-b||: ", (A * (A.Inv * b) - b).Norm);
                    Console.WriteLine();
                }
            } while (answer);
        }  // mat2.example

    }  // struct mat2


}

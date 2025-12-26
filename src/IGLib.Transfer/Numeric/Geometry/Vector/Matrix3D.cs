// Copyright (c) Igor Grešovnik (2009), IGLib license; http://www2.arnes.si/~ljc3m2/igor/ioptlib/



    /****************************************************/
    /*                                                  */
    /*  STRUCT IMPLEMENTATION OF 3D MATRICES & VECTORS  */
    /*                                                  */
    /****************************************************/


using System;
using System.Collections.Generic;
using System.Text;

using IG.Lib;

using F = IG.Num.M;

namespace IG.Num
{

    /// <summary>3D vector, struct implementation.</summary>
    /// <remarks>Name of this struct is not in line with conventions. This is intentional in order to
    /// prevent mistaking struct implementation for class interpretation.</remarks>
    /// $A Igor Jul08; Oct10;
    public struct vec3
    {
        public double x, y, z;

        #region Initialization

        /// <summary>Copy constructor.
        /// Initializes components of a 3D vector with components of the specified vector.</summary>
        /// <param name="v">Vectr whose components are copied to the initialized vector.</param>
        public vec3(vec3 v)
        {
            x = v.x; y = v.y; z = v.z;
        }

        /// <summary>Initializes components of a 3D vector with the specified values.</summary>
        /// <param name="x">Value to be assigned to the 1st element of the vector.</param>
        /// <param name="y">Value to be assigned to the 2nd element of the vector.</param>
        /// <param name="z">Value to be assigned to the 3rd element of the vector.</param>
        public vec3(double x, double y, double z)
        {
            this.x = x; this.y = y; this.z = z;
        }

        /// <summary>Initializes all component of a 3D vector with the specified value.</summary>
        /// <param name="comp">Value assigned to all vector elements.</param>
        public vec3(double comp)
        {
            x = y = z = comp;
        }

        /// <summary>Sets components of the vector to 0.0.</summary>
        public void Zero()
        { x = y = z = 0.0; }

        /// <summary>Sets components of the vector to the specified value.</summary>
        public void Set(double comp)
        { x = y = z = comp; }

        #endregion Initialization


        #region Access

        /// <summary>Index operator.</summary>
        /// <param name="i">Component index.</param>
        /// <returns>The specified component of a 3D vector.</returns>
        public double this[int i]
        {
            get
            {
                if (i == 0)
                    return x;
                else if (i == 1)
                    return y;
                else if (i == 2)
                    return z;
                else
                    throw new IndexOutOfRangeException("3D vector does not have component [" + i + "]");
            }
            set
            {
                if (i == 0)
                    x = value;
                else if (i == 1)
                    y = value;
                else if (i == 2)
                    z = value;
                else
                    throw new IndexOutOfRangeException("3D vector does not have component [" + i + "]");
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
            ret ^= z.GetHashCode();
            return ret;

        }

        /// <summary>Returns a value indicating whether the specified object is equal to the current vector.
        /// <para>True is returned if the object is of type <see cref="vec3"/>) and has equal elements as the current vector.</para></summary>
        /// <remarks> <para>Overrides the <see cref="object.Equals(object)"/> method.</para></remarks>
        public override bool Equals(Object obj)
        {
            if (obj is vec3)
                if (this == (vec3)obj)
                    return true;
            return false;
        }

        #endregion Auxiliary


        #region Operations

        /// <summary>Gets a 2 norm of the current 3D vector.</summary>
        public double Norm
        { get { return Norm2; } }

        /// <summary>Gets a 2 norm of the current 3D vector.</summary>
        public double Norm2 { get { return Math.Sqrt(x * x + y * y + z * z); } }

        /// <summary>Gets an 1 norm of the current 3D vector -
        /// sum of absolute values of components.</summary>
        public double Norm1 { get { return Math.Abs(x) + Math.Abs(y) + Math.Abs(z); } }

        /// <summary>Gets an infinity norm of the current 3D vector -
        /// maximal absolute component value.</summary>
        public double NormInf { 
            get {
                double ret = 0, a;
                if ((a=Math.Abs(x)) > ret) ret = a;
                if ((a=Math.Abs(y)) > ret) ret = a;
                if ((a=Math.Abs(z)) > ret) ret = a;
                return ret;
            } 
        }

        /// <summary>Returns normalized this 3D vector.</summary>
        public vec3 Normalized()
        { return Multiply(1 / Norm); }

        /// <summary>Returns normalized this 3D vector in 1 norm.</summary>
        public vec3 Normalized1()
        { return Multiply(1 / Norm1); }

        /// <summary>Returns normalized this 3D vector in 2 norm.</summary>
        public vec3 Normalized2()
        { return Multiply(1 / Norm2); }

        /// <summary>Returns normalized this 3D vector in infinity norm.</summary>
        public vec3 NormalizedInfinity()
        { return Multiply(1 / NormInf); }


        /// <summary>Returns scalar product of the current and the specified vector.</summary>
        public double ScalarProduct(vec3 v)
        {
            return x * v.x + y * v.y + z * v.z;
        }

        /// <summary>Returns vector product of the current and the specified vector.</summary>
        public vec3 VectorProduct(vec3 v)
        {
            return new vec3(
                    y * v.z - z * v.y,
                    z * v.x - x * v.z,
                    x * v.y - y * v.x
                );
        }

        /// <summary>Returns vector product of the current and the specified vector.</summary>
        public vec3 CrossProduct(vec3 v)
        { return VectorProduct(v); }

        /// <summary>Returns vector product of the current and the specified vector.</summary>
        public vec3 Cross(vec3 v)
        { return VectorProduct(v); }

        /// <summary>Returns dyadic product of the current and the specified vector.</summary>
        public mat3 DyadicProduct(vec3 v)
        {
            return new mat3(
                    x * v.x, x * v.y, x * v.z,
                    y * v.x, y * v.y, y * v.z,
                    z * v.x, z * v.y, z * v.z
                );
        }

        /// <summary>Returns mixed product of the current and two other specified vectors.
        /// This equals the volume of the parallelepiped spanned by these vectors.</summary>
        public double MixedProduct(vec3 b, vec3 c)
        {
            return y * b.z * c.x + z * b.x * c.y + x * b.y * c.z
                 - z * b.y * c.x - x * b.z * c.y - y * b.x * c.z;

        }


        /// <summary>Returns the current vector multiplied by the specified scalar.</summary>
        /// <param name="k">Factor by which the current vector is multiplied.</param>
        public vec3 Multiply(double k)
        {
            return new vec3(k * x, k * y, k * z);
        }

        /// <summary>Returns sum of the current vector and the specified vector.</summary>
        public vec3 Add(vec3 a)
        {
            return new vec3(x + a.x, y + a.y, z + a.z);
        }

        /// <summary>Returns difference between the current vector and the specified vector.</summary>
        public vec3 Subtract(vec3 a)
        {
            return new vec3(x - a.x, y - a.y, z - a.z);
        }


        #endregion Operations



        #region StaticMethods

        /// <summary>Returns a copy of the specified 3D vector.</summary>
        /// <param name="v">Vector whose copy is returned.</param>
        public static vec3 Copy(vec3 v)
        {
            return new vec3(v);
        }

        /// <summary>Negates the specified vector and stores its copy in the resulting vector.</summary>
        /// <param name="v">Vectr to be negated.</param>
        /// <param name="res">Vector where the result is stored.</param>
        public static void Negate(vec3 v, ref vec3 res)
        {
            res.x = -v.x; res.y = -v.y; res.z = -v.z;
        }

        #endregion StaticMethods


        #region OperatorsOverloading

        /// <summary>Unary plus, returns the operand.</summary>
        public static vec3 operator +(vec3 v)
        {
            return vec3.Copy(v);
        }

        /// <summary>Unary negation, returns the negative operand.</summary>
        public static vec3 operator -(vec3 v)
        {
            return new vec3(-v.x, -v.y, -v.z);
        }

        /// <summary>Vector addition.</summary>
        public static vec3 operator +(vec3 a, vec3 b)
        {
            return a.Add(b);
        }


        /// <summary>Vector subtraction.</summary>
        public static vec3 operator -(vec3 a, vec3 b)
        {
            return a.Subtract(b); ;
        }


        /// <summary>Scalar product of two 3D vectors.</summary>
        public static double operator *(vec3 a, vec3 b)
        {
            return a.ScalarProduct(b);
        }

        /// <summary>Product of a 3D vector by a scalar.</summary>
        public static vec3 operator *(vec3 a, double b)
        {
            return a.Multiply(b);
        }

        /// <summary>Product of a 3D vector by a scalar.</summary>
        public static vec3 operator *(double a, vec3 b)
        {
            return b.Multiply(a) ;
        }

        /// <summary>Vector subtraction.</summary>
        public static vec3 operator /(vec3 a, double b)
        {
            return new vec3(a.x / b, a.y / b, a.z / b);
        }


        // REMARK: For reference types it is not a good idea to overload the == and != operators because 
        // in this case we can not use these operators for comparison of references!
        // Since this is a value type, we can to this without fear.

        /// <summary>Vector comparison.</summary>
        public static bool operator == (vec3 a, vec3 b)
        {
            return a.x==b.x && a.y==b.y && a.z==b.z;
        }

        /// <summary>Vector comparison, returns true if vectors are different.</summary>
        public static bool operator != (vec3 a, vec3 b)
        {
            return a.x!=b.x || a.y!=b.y || a.z!=b.z;
        }


        #endregion  OperatorsOverloading


        #region InputOutput

        /// <summary>Returns string representation of the 3D vector.</summary>
        public override string ToString()
        {
            return
            "{" + x + ", " + y + ", " + z + "}";
        }

        /// <summary>Reads 3D vector components from a console.</summary>
        public void Read()
        {
            Read(null);
        }


        /// <summary>Reads 3D vector components from a console.</summary>
        /// <param name="name">Name of the vector to be read; it is written as orientation to the user
        /// and can be null.</param>
        public void Read(string name)
        {
            Console.WriteLine();
            Console.Write("  " + name + ".x: "); Cons.Read(ref x);
            Console.Write("  " + name + ".y: "); Cons.Read(ref y);
            Console.Write("  " + name + ".z: "); Cons.Read(ref z);
        }

        #endregion InputOutput

    }  // struct vec3


    /// <summary>3D matrix, struct implementation.</summary>
    /// <remarks>Name of this struct is not in line with conventions. This is intentional in order to
    /// prevent mistaking struct implementation for class interpretation.</remarks>
    /// $A Igor Jul08, Oct10;
    public struct mat3
    {
        public double xx, xy, xz, yx, yy, yz, zx, zy, zz;

        #region Initialization

        /// <summary>Copy constructor.
        /// Initializes components of a 3D matrix with components of the specified matrix.</summary>
        /// <param name="m">Matrix whose components are copied to the initialized matrix.</param>
        public mat3(mat3 m)
        {
            xx = m.xx;  xy = m.xy;  xz = m.xz;
            yx = m.yx;  yy = m.yy;  yz = m.yz;
            zx = m.zx;  zy = m.zy;  zz = m.zz;
        }

        /// <summary>Initializes 3D matrix structure with the specified components.</summary>
        public mat3(double xx, double xy, double xz, 
                    double yx, double yy, double yz,
                    double zx, double zy, double zz)
        {
            this.xx = xx; this.xy = xy; this.xz = xz;
            this.yx = yx; this.yy = yy; this.yz = yz;
            this.zx = zx; this.zy = zy; this.zz = zz;
        }

        /// <summary>Initializes 3D with the specified component.</summary>
        /// <param name="component">Value that is assigned to all matrix components.</param>
        public mat3(double component)
        {
            this.xx = component; this.xy = component; this.xz = component;
            this.yx = component; this.yy = component; this.yz = component;
            this.zx = component; this.zy = component; this.zz = component;
        }

        /// <summary>Initializes 3D matrix structure with the specified components.</summary>
        public mat3(vec3 xrow, vec3 yrow, vec3 zrow)
        {
            this.xx = xrow.x; this.xy = xrow.y; this.xz = xrow.z;
            this.yx = yrow.x; this.yy = yrow.y; this.yz = yrow.z;
            this.zx = zrow.x; this.zy = zrow.y; this.zz = zrow.z;
        }

        /// <summary>Sets components of the vector to 0.0.</summary>
        public void Zero()
        { 
            xx = xy = xz = 
            yx = yy = yz = 
            zx = zy = zz = 0.0; 
        }

        /// <summary>Sets components of the vector to the specified value.</summary>
        public void Set(double comp)
        {
            xx = xy = xz =
            yx = yy = yz =
            zx = zy = zz = comp;
        }


        #endregion Initialization


        #region Access

        /// <summary>Index operator.</summary>
        /// <param name="i">Component index 1.</param>
        /// <param name="j">Component index 2.</param>
        /// <returns>The specified component of a 3D matrix.</returns>
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
                    else if (j == 2)
                        return xz;
                    else
                        throw new IndexOutOfRangeException("3D matrix does not have component ["
                            + i + ", " + j + "]");
                }
                else if (i == 1)
                {
                    if (j == 0)
                        return yx;
                    else if (j == 1)
                        return yy;
                    else if (j == 2)
                        return yz;
                    throw new IndexOutOfRangeException("3D matrix does not have component ["
                        + i + ", " + j + "]");
                }
                else if (i == 2)
                {
                    if (j == 0)
                        return zx;
                    else if (j == 1)
                        return zy;
                    else if (j == 2)
                        return zz;
                    throw new IndexOutOfRangeException("3D matrix does not have component ["
                        + i + ", " + j + "]");
                }
                else
                    throw new IndexOutOfRangeException("3D vector does not have component [" + i + "]");
            }
            set
            {
                if (i == 0)
                {
                    if (j == 0)
                        xx = value;
                    else if (j == 1)
                        xy = value;
                    else if (j == 2)
                        xz = value;
                    else
                        throw new IndexOutOfRangeException("3D matrix does not have component ["
                            + i + ", " + j + "]");
                }
                else if (i == 1)
                {
                    if (j == 0)
                        yx = value;
                    else if (j == 1)
                        yy = value;
                    else if (j == 2)
                        yz = value;
                    throw new IndexOutOfRangeException("3D matrix does not have component ["
                        + i + ", " + j + "]");
                }
                else if (i == 2)
                {
                    if (j == 0)
                        zx = value;
                    else if (j == 1)
                        zy = value;
                    else if (j == 2)
                        zz = value;
                    throw new IndexOutOfRangeException("3D matrix does not have component ["
                        + i + ", " + j + "]");
                }
                else
                    throw new IndexOutOfRangeException("3D matrix does not have component ["
                        + i + ", " + j + "]");
            }
        } // this[i,j]

        /// <summary>Gets or sets the x-row of the 3D matrix.</summary>
        public vec3 rowx
        {
            get { return new vec3(xx, xy, xz); }
            set { xx=value.x; xy=value.y; xz=value.z; }
        }

        /// <summary>Gets or sets the y-row of the 3D matrix.</summary>
        public vec3 rowy
        {
            get { return new vec3(yx, yy, yz); }
            set { yx=value.x; yy=value.y; yz=value.z; }
        }

        /// <summary>Gets or sets the z-row of the 3D matrix.</summary>
        public vec3 rowz
        {
            get { return new vec3(zx, zy, zz); }
            set { zx=value.x; zy=value.y; zz=value.z; }
        }

        /// <summary>Gets or sets the x-column of the 3D matrix.</summary>
        public vec3 columnx
        {
            get { return new vec3(xx, yx, zx); }
            set { xx = value.x;  yx = value.y;  zx = value.z; }
        }

        /// <summary>Gets or sets the y-column of the 3D matrix.</summary>
        public vec3 columny
        {
            get { return new vec3(xy, yy, zy); }
            set { xy = value.x;  yy = value.y;  zy = value.z; }
        }

        /// <summary>Gets or sets the z-column of the 3D matrix.</summary>
        public vec3 columnz
        {
            get { return new vec3(xz, yz, zz); }
            set { xz = value.x;  yz = value.y;  zz = value.z; }
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
            ret ^= xz.GetHashCode();
            ret ^= yx.GetHashCode();
            ret ^= yy.GetHashCode();
            ret ^= yz.GetHashCode();
            ret ^= zx.GetHashCode();
            ret ^= zy.GetHashCode();
            ret ^= zz.GetHashCode();
            return ret;
        }

        /// <summary>Returns a value indicating whether the specified object is equal to the current matrix.
        /// <para>True is returned if the object is of type <see cref="mat3"/>) and has equal elements as the current matrix.</para></summary>
        /// <remarks> <para>Overrides the <see cref="object.Equals(object)"/> method.</para></remarks>
        public override bool Equals(Object obj)
        {
            if (obj is mat3)
                if (this == (mat3)obj)
                    return true;
            return false;
        }

        #endregion Auxiliary


        #region Operations

        /// <summary>Get Forbenius (or euclidean) norm of the matrix - 
        /// square root of sum of squares of components.</summary>
        public double NormForbenius
        { 
            get {
            return Math.Sqrt(
                  xx*xx + xy*xy + xz*xz 
                + yx*yx + yy*yy + yz*yz
                + zx*zx + zy*zy + zz*zz
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
                double ret = 0, s;
                s = Math.Abs(xx) + Math.Abs(yx) + Math.Abs(zx);
                if (s > ret)
                    ret = s;
                s = Math.Abs(xy) + Math.Abs(yy) + Math.Abs(zy);
                if (s > ret)
                    ret = s;
                s = Math.Abs(xz) + Math.Abs(yz) + Math.Abs(zz);
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
                double ret = 0, s;
                s = Math.Abs(xx) + Math.Abs(xy) + Math.Abs(xz);
                if (s > ret)
                    ret = s;
                s = Math.Abs(yx) + Math.Abs(yy) + Math.Abs(yz);
                if (s > ret)
                    ret = s;
                s = Math.Abs(zx) + Math.Abs(zy) + Math.Abs(zz);
                if (s > ret)
                    ret = s;
                return ret;
            }
        }

        /// <summary>Returns this matrix normalized with Euclidean norm.</summary>
        public mat3 NormalizedEuclidean()
        { return Multiply(1/NormEuclidean); }

        /// <summary>Returns this matrix normalized with Euclidean norm.</summary>
        public mat3 NormalizedForbenius()
        { return Multiply(1/NormForbenius); }

        /// <summary>Returns this matrix normalized with 1 norm.</summary>
        public mat3 Normalized1()
        { return Multiply(1/Norm1); }

        /// <summary>Returns this matrix normalized with infinity norm.</summary>
        public mat3 NormalizedInfinity()
        { return Multiply(1/NormInfinity); }



        /// <summary>Gets matrix determinant.</summary>
        public double Determinant
        {
            get
            {
                return xx * yy * zz + xy * yz * zx + xz * yx * zy
                    - xz * yy * zx - xx * yz * zy - xy * yx * zz;
            }
        }

        /// <summary>Gets matrix determinant.</summary>
        public double Det
        { get { return Determinant; } }

        /// <summary>Gets matrix trace (sum of diagonal elements).</summary>
        public double Trace
        { get { return xx + yy + zz; } }

        /// <summary>Gets transpose of the current matrix.</summary>
        public mat3 Transpose
        {
            get
            {
                mat3 ret;
                ret.xx = xx; ret.xy = yx; ret.xz = zx;
                ret.yx = xy; ret.yy = yy; ret.yz = zy;
                ret.zx = xz; ret.zy = yz; ret.zz = zz;
                return ret;
            }
        }


        /// <summary>Gets transpose of the current matrix.</summary>
        public mat3 T
        { get { return Transpose; } }


        /// <summary>Gets inverse of the current matrix.</summary>
        public mat3 Inverse
        {
            get
            {
                double d = this.Determinant;
                if (d == 0)
                    throw new InvalidOperationException("3D matrix is not invertible.");
                mat3 ret;
                ret.xx = (yy * zz - zy * yz) / d;
                ret.xy = -(xy * zz - zy * xz) / d;
                ret.xz = (xy * yz - yy * xz) / d;
                ret.yx = -(yx * zz - zx * yz) / d;
                ret.yy = (xx * zz - zx * xz) / d;
                ret.yz = -(xx * yz - yx * xz) / d;
                ret.zx = (yx * zy - zx * yy) / d;
                ret.zy = -(xx * zy - zx * xy) / d;
                ret.zz = (xx * yy - yx * xy) / d;
                return ret;
            }
        }

        /// <summary>Gets inverse of the current matrix.</summary>
        public mat3 Inv { get { return Inverse; } }

        /// <summary>Returns solution of system of equations with the current system matrix and 
        /// the specified right-hand sides.</summary>
        /// <param name="b">Vector of right-hand sides of equations.</param>
        /// <returns></returns>
        public vec3 Solve(vec3 b)
        { return Inv * b; }


        /// <summary>Returns sum of the current matrix and the specified matrix.</summary>
        public mat3 Add(mat3 a)
        {
            mat3 ret;
            ret.xx = xx + a.xx;  ret.xy = xy + a.xy;  ret.xz = xz + a.xz;
            ret.yx = yx + a.yx;  ret.yy = yy + a.yy;  ret.yz = yz + a.yz;
            ret.zx = zx + a.zx;  ret.zy = zy + a.zy;  ret.zz = zz + a.zz;
            return  ret;
        }

        /// <summary>Returns difference between the current matrix and the specified matrix.</summary>
        public mat3 Subtract(mat3 a)
        {
            mat3 ret;
            ret.xx = xx - a.xx; ret.xy = xy - a.xy; ret.xz = xz - a.xz;
            ret.yx = yx - a.yx; ret.yy = yy - a.yy; ret.yz = yz - a.yz;
            ret.zx = zx - a.zx; ret.zy = zy - a.zy; ret.zz = zz - a.zz;
            return ret;
        }

        /// <summary>Right-multiplies the current 3D matrix with the specified matrix and returns the product.</summary>
        /// <param name="b">Right-hand side factor of multiplication.</param>
        /// <returns>this*b</returns>
        public mat3 MultiplyRight(mat3 b)
        {
            mat3 prod;
            prod.xx = xx*b.xx + xy*b.yx + xz*b.zx;
            prod.xy = xx*b.xy + xy*b.yy + xz*b.zy;
            prod.xz = xx*b.xz + xy*b.yz + xz*b.zz;

            prod.yx = yx*b.xx + yy*b.yx + yz*b.zx;
            prod.yy = yx*b.xy + yy*b.yy + yz*b.zy;
            prod.yz = yx*b.xz + yy*b.yz + yz*b.zz;

            prod.zx = zx*b.xx + zy*b.yx + zz*b.zx;
            prod.zy = zx*b.xy + zy*b.yy + zz*b.zy;
            prod.zz = zx*b.xz + zy*b.yz + zz*b.zz;
            return prod;
        }

        /// <summary>Left-multiplies the current 3D matrix with the specified matrix and returns the product.</summary>
        /// <param name="b">Left-hand side factor of multiplication.</param>
        /// <returns>b*this</returns>
        public mat3 MultiplyLeft(mat3 b)
        {
            return b.MultiplyRight(this);
        }


        /// <summary>Right-multiplies the current 3D matrix with the specified 3D vector and returns the product.</summary>
        /// <param name="b">Right-hand side factor of multiplication.</param>
        /// <returns>this*b</returns>
        public vec3 Multiply(vec3 b)
        {
            vec3 prod;
            prod.x = xx * b.x + xy * b.y + xz * b.z;
            prod.y = yx * b.x + yy * b.y + yz * b.z;
            prod.z = zx * b.x + zy * b.y + zz * b.z;
            return prod;
        }

        /// <summary>Multiplies the current 3D matrix with the specified scalar and returns the product.</summary>
        /// <param name="b">Factor of multiplication.</param>
        /// <returns>this*b</returns>
        public mat3 Multiply(double b)
        {
            return new mat3(
                xx * b, xy * b, + xz * b,
                yx * b, yy * b, + yz * b,
                zx * b, zy * b, + zz * b
                );
        }

        #endregion Operations


        #region StaticMethods

        /// <summary>Returns a copy of the specified 3D matrix.</summary>
        /// <param name="m">Matrix whose copy is returned.</param>
        public static mat3 Copy(mat3 m)
        {
            return new mat3(m);
        }

        /// <summary>Negates the specified 3D matrix and stores its copy in the resulting matrix.</summary>
        /// <param name="m">Matrix to be negated.</param>
        /// <param name="res">Matrix where the result is stored.</param>
        public static void Negate(mat3 m, ref mat3 res)
        {
            res.xx = -m.xx; res.xy = -m.xy; res.xz = -m.xz;
            res.yx = -m.yx; res.yy = -m.yy; res.yz = -m.yz;
            res.zx = -m.zx; res.zy = -m.zy; res.zz = -m.zz;
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
        public static int EigenSystem2d(mat3 a, ref mat3 eigenvec, ref vec3 eigenval)
        {
            double axx, axy, ayx, ayy, d;
            axx = a.xx; axy = a.xy; ayx = a.yx; ayy = a.yy;
            d = axx * axx + 4 * axy * ayx - 2 * axx * ayy + ayy * ayy;
            if (d < 0)
            {
                /* The eigensystem has no real solutions: */
                eigenvec.Zero();
                eigenval.Zero();
                return 0;
            }
            else
            {
                eigenvec.xz = eigenvec.yz = eigenvec.zz = eigenvec.zx = eigenvec.zy =
                  eigenval.z = 0;
                d = Math.Sqrt(d);
                if (d > 1e-10 * Math.Abs(axx + ayy))
                {
                    /* We have two real eigenvalues: */
                    if (Math.Abs(ayx) / (Math.Abs(axx) + Math.Abs(ayy) + Math.Abs(d)) > 1e-20)
                    {
                        eigenval.x = (axx + ayy - d) / 2;
                        eigenvec.xx = -(-axx + ayy + d) / (2 * ayx);
                        eigenvec.xy = 1;
                        eigenval.y = (axx + ayy + d) / 2;
                        eigenvec.yx = -(-axx + ayy - d) / (2 * ayx);
                        eigenvec.yy = 1;
                    }
                    else
                    {
                        /* When ayx==0 we must adopt another formula: */
                        eigenval.x = axx;
                        eigenvec.xx = 1;
                        eigenvec.xy = 0;
                        eigenval.y = ayy;
                        if (Math.Abs(axx - ayy) > 1e-20)
                        {
                            eigenvec.yx = -(axy / (axx - ayy));
                            eigenvec.yy = 1;
                        }
                        else
                        {
                            /* To ni striktno, velja pa za simetricne matrike (zaradi ortog. last.
                            vektorjev): */
                            eigenvec.yx = 0;
                            eigenvec.yy = 1;
                        }
                    }
                    // Normalize eigenvectors:
                    eigenvec.rowx = eigenvec.rowx.Normalized2();
                    eigenvec.rowy = eigenvec.rowy.Normalized2();
                    return 2;
                    //normvec3d(&(eigenvec.x));
                    //normvec3d(&(eigenvec.y));
                }
                else
                {
                    /* We have only one (double) eigenvalue: */
                    if (Math.Abs(ayx) / (Math.Abs(axx) + Math.Abs(ayy) + Math.Abs(d)) > 1e-20)
                    {
                        eigenval.x = (axx + ayy - d) / 2;
                        eigenvec.xx = -(-axx + ayy + d) / (2 * ayx);
                        eigenvec.xy = 1;
                        eigenval.y = (axx + ayy + d) / 2;
                        /* We set the second eigenvector orthogonal to the first one (this is OK
                        for symmetric matrices): */
                        eigenvec.yx = -eigenvec.xy;
                        eigenvec.yy = eigenvec.xx;
                    }
                    else
                    {
                        /* When ayx==0 we must adopt another formula: */
                        eigenval.x = axx;
                        eigenvec.xx = 1;
                        eigenvec.xy = 0;
                        eigenval.y = ayy;
                        eigenvec.yx = 0;
                        eigenvec.yy = 1;
                    }
                    // Normalize eigenvectors:
                    eigenvec.rowx = eigenvec.rowx.Normalized2();
                    eigenvec.rowy = eigenvec.rowy.Normalized2();
                    //normvec3d(&(eigenvec.x));
                    //normvec3d(&(eigenvec.y));
                    return 1;
                }
            }
        }  // EigenSystem2d



        #endregion StaticMethods


        #region OperatorsOverloading

        /// <summary>Unary plus for 3D matrices, returns the operand.</summary>
        public static mat3 operator +(mat3 m)
        {
            return mat3.Copy(m);
        }

        /// <summary>Unary negation for 3D matrices, returns the negative operand.</summary>
        public static mat3 operator -(mat3 a)
        {
            return new mat3(
                -a.xx, -a.xy, -a.xz,
                -a.yx, -a.yy, -a.yz,
                -a.zx, -a.zy, -a.zz
                );
        }

        /// <summary>Matrix addition in 3D.</summary>
        public static mat3 operator +(mat3 a, mat3 b)
        {
            return a.Add(b);
        }


        /// <summary>Matrix subtraction in 3D.</summary>
        public static mat3 operator -(mat3 a, mat3 b)
        {
            return a.Subtract(b); ;
        }

        /// <summary>Matrix multiplication in 3D.</summary>
        public static mat3 operator *(mat3 a, mat3 b)
        {
            return a.MultiplyRight(b); ;
        }

        /// <summary>Matrix with vector multiplication in 3D.</summary>
        public static vec3 operator *(mat3 a, vec3 b)
        {
            return a.Multiply(b); ;
        }


        /// <summary>Product of a 3D matrix by a scalar.</summary>
        public static mat3 operator *(mat3 a, double b)
        {
            return a.Multiply(b);
        }

        /// <summary>Product of a 3D matrix by a scalar.</summary>
        public static mat3 operator *(double a, mat3 b)
        {
            return b.Multiply(a);
        }

        /// <summary>Division of a 3D matrix by a scalar.</summary>
        public static mat3 operator /(mat3 a, double b)
        {
            return a.Multiply(1/b);
        }


        // REMARK: For reference types it is not a good idea to overload the == and != operators because 
        // in this case we can not use these operators for comparison of references!
        // Since this is a value type, we can to this without fear.

        /// <summary>Vector comparison.</summary>
        public static bool operator ==(mat3 a, mat3 b)
        {
            return
                a.xx == b.xx && a.xy == b.xy && a.xz == b.xz &&
                a.yx == b.yx && a.yy == b.yy && a.yz == b.yz &&
                a.zx == b.zx && a.zy == b.zy && a.zz == b.zz;
        }

        /// <summary>Vector comparison, returns true if vectors are different.</summary>
        public static bool operator !=(mat3 a, mat3 b)
        {
            return
                a.xx != b.xx && a.xy != b.xy && a.xz != b.xz &&
                a.yx != b.yx && a.yy != b.yy && a.yz != b.yz &&
                a.zx != b.zx && a.zy != b.zy && a.zz != b.zz;
        }


        #endregion  OperatorsOverloading


        #region InputOutput

        /// <summary>Returns a string representation of this 3D matrix.</summary>
        public override string ToString()
        {
            return 
                  "{" + Environment.NewLine
                + "  {" + xx + ", " + xy + ", " + xz + "}, " + Environment.NewLine
                + "  {" + yx + ", " + yy + ", " + yz + "}, " + Environment.NewLine
                + "  {" + zx + ", " + zy + ", " + zz + "}" + Environment.NewLine
                + "} ";
        }

        /// <summary>Reads this 3D matrix components from a console.</summary>
        public void Read()
        {
            Read(null);
        }


        /// <summary>Reads this 3D matrix components from a console.</summary>
        /// <param name="name">Name of the matrix to be read; it is written as orientation to the user
        /// and can be null.</param>
        public void Read(string name)
        {
            Console.Write("  " + name + ".xx: "); Cons.Read(ref xx);
            Console.Write("  " + name + ".xy: "); Cons.Read(ref xy);
            Console.Write("  " + name + ".xz: "); Cons.Read(ref xz);
            Console.Write("  " + name + ".yx: "); Cons.Read(ref yx);
            Console.Write("  " + name + ".yy: "); Cons.Read(ref yy);
            Console.Write("  " + name + ".yz: "); Cons.Read(ref yz);
            Console.Write("  " + name + ".zx: "); Cons.Read(ref zx);
            Console.Write("  " + name + ".zy: "); Cons.Read(ref zy);
            Console.Write("  " + name + ".zz: "); Cons.Read(ref zz);
        }

        #endregion InputOutput


        /// <summary>A short example of how to use mat3 and vec3 structs.</summary>
        public static void Example()
        {
            mat3 A = new mat3(
                5, 1, 3,
                0.12, 4, -1,
                0.55, 0.2, 2
                );
            mat3 B = A.T;
            mat3 AInv = A.Inv;
            mat3 P = A*AInv;
            Console.WriteLine("Original matrix A: " + Environment.NewLine + A);
            Console.WriteLine("Determinant of A: " + A.Determinant);
            Console.WriteLine("Transpose of A: " + Environment.NewLine + B);
            Console.WriteLine("Inverse of A: " + Environment.NewLine + AInv);
            Console.WriteLine("Product of A and its inverse: " + Environment.NewLine + P);
            vec3 a = new vec3(1.111, 2.222, 3.333);
            vec3 p, p2;
            p = A*a;
            p2 = AInv*p;
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
                    vec3 b = new vec3(), x;
                    Console.WriteLine("Insert 3D matrix A to be analysed!");
                    A.Read("A");
                    Console.WriteLine("Insert right-hand side 3D vector b!");
                    b.Read("b");
                    Console.WriteLine();
                    Console.WriteLine("Matrix A: " + Environment.NewLine + A);
                    Console.WriteLine("Vector B: " + Environment.NewLine + b);
                    Console.WriteLine("Det A: " + A.Determinant);
                    Console.WriteLine("||A^-1*A||: " + (A.Inv*A).Norm );
                    Console.WriteLine("Solution x of A*x=b: " + (x = A.Inverse*b));
                    Console.WriteLine("||A*x-b||: ", (A * (A.Inv * b) - b).Norm);
                    Console.WriteLine();
                }
            } while (answer);
        }  // mat3.example

    }  // struct mat3


} 




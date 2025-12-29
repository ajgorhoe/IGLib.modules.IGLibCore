// Copyright (c) Igor Grešovnik (2008 - present), IGLib license; http://www2.arnes.si/~ljc3m2/igor/iglib/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Drawing;

using IG.Num;


namespace IG.Lib
{

    ///// <summary>Graphic coordinates in 3D.</summary>
    ///// $A Igor Feb10;
    //public struct vec3_to_delete
    //{
    //    public double x, y, z;

    //    #region Constructors

    //    /// <summary>Creates 3D coordinates initialized with specified values.</summary>
    //    /// <param name="x"></param>
    //    /// <param name="y"></param>
    //    /// <param name="z"></param>
    //    public vec3_to_delete(double x, double y, double z)
    //    {
    //        this.x = x;  this.y = y; this.z = z;
    //    }

    //    /// <summary>Constructs 2D coordinates, the third coordinate is 0.</summary>
    //    public vec3_to_delete(double x, double y)
    //    {
    //        this.x = x; this.y = y; this.z = 0.0d;
    //    }

    //    /// <summary>Constructs 2D coordinates from  Coord2D, the third coordinate is 0.</summary>
    //    public vec3_to_delete(vec2 a)
    //    {
    //        this.x = a.x; this.y = a.y; this.z = 0.0d;
    //    }

    //    #endregion

    //    #region Operators


    //    public static bool operator !=(vec3_to_delete a, vec3_to_delete b)
    //    {
    //        return (a.x != b.x || a.y != b.y || a.z!=b.z);
    //    }


    //    public static bool operator ==(vec3_to_delete a, vec3_to_delete b)
    //    { 
    //        return (a.x == b.x && a.y == b.y && a.z==b.z); 
    //    }


    //    public static vec3_to_delete operator +(vec3_to_delete a)
    //    {
    //        return a;
    //    }

    //    public static vec3_to_delete operator -(vec3_to_delete a)
    //    {
    //        return new vec3_to_delete(-a.x, -a.y, -a.z);
    //    }

    //    public static vec3 operator +(vec3_to_delete a, vec3_to_delete b)
    //    {
    //        return new vec3(a.x+b.x, a.y+b.y, a.z+b.z);
    //    }

    //    public static vec3_to_delete operator -(vec3_to_delete a, vec3_to_delete b)
    //    {
    //        return new vec3_to_delete(a.x - b.x, a.y - b.y, a.z - b.z);
    //    }

    //    public static vec3_to_delete operator *(vec3_to_delete a, double s)
    //    {
    //        return new vec3_to_delete(a.x * s, a.y * s, a.z * s);
    //    }

    //    public static vec3_to_delete operator *(double s, vec3_to_delete a)
    //    {
    //        return new vec3_to_delete(a.x * s, a.y * s, a.z * s);
    //    }
    //    public static vec3 operator /(vec3_to_delete a, double s)
    //    {
    //        return new vec3(a.x/s, a.y/s, a.z/s);
    //    }


    //    ///// <summary>Implicit conversion from 2D to 3D coordinates.</summary>
    //    //public static implicit operator Coord3D(Coord2D a)
    //    //{
    //    //    return new Coord3D(a);
    //    //}


    //    #endregion  // Operators


    //    /// <summary>Gets the hashcode of this <c>Scalar</c>.</summary>
    //    public override int GetHashCode()
    //    {
    //        return (x.GetHashCode() ^ y.GetHashCode()) ^ z.GetHashCode();
    //    }



    //    #region Equality_Comparison_Hashing


    //    /// <summary>Indicates whether <c>obj</c> is equal to this instance.</summary>
    //    public override bool Equals(object obj)
    //    {
    //        return (obj is vec3) && this.Equals((vec3)obj);
    //    }

    //    /// <summary>Indicates whether <c>a</c> is equal to this instance. </summary>
    //    public bool Equals(vec3 a)
    //    {
    //        return (x == a.x && y == a.y && z == a.z);
    //    }

    //    #endregion Equality_Comparison_Hashing

    //    /// <summary>Returns norm of the Coordinate vector.</summary>
    //    public double Norm { get { return Math.Sqrt(x*x + y*y+z*z); } }

    //    public double Distance(vec3_to_delete b)
    //    {
    //        return (this - b).Norm;
    //    }

    //    public static double Distance(vec3 a, vec3 b)
    //    {
    //        return (a - b).Norm;
    //    }

    //} // struct Coord3D


    ///// <summary>Graphic coordinates in 2D.</summary>
    ///// $A Igor Feb10;
    //public struct vec2_to_delete
    //{
    //    public double x, y;

    //    #region Constructors

    //    /// <summary>Creates 2D coordinates initialized with specified values.</summary>
    //    public vec2_to_delete(double x, double y)
    //    {
    //        this.x = x; this.y = y;
    //    }

    //    #endregion

    //    #region Operators


    //    public static bool operator !=(vec2_to_delete a, vec2_to_delete b)
    //    {
    //        return (a.x != b.x || a.y != b.y);
    //    }


    //    public static bool operator ==(vec2_to_delete a, vec2_to_delete b)
    //    {
    //        return (a.x == b.x && a.y == b.y);
    //    }


    //    public static vec2_to_delete operator +(vec2_to_delete a)
    //    {
    //        return a;
    //    }

    //    public static vec2_to_delete operator -(vec2_to_delete a)
    //    {
    //        return new vec2_to_delete(-a.x, -a.y);
    //    }

    //    public static vec2_to_delete operator +(vec2_to_delete a, vec2_to_delete b)
    //    {
    //        return new vec2_to_delete(a.x + b.x, a.y + b.y);
    //    }

    //    public static vec2 operator -(vec2_to_delete a, vec2_to_delete b)
    //    {
    //        return new vec2(a.x - b.x, a.y - b.y);
    //    }

    //    public static vec2 operator *(vec2_to_delete a, double s)
    //    {
    //        return new vec2(a.x * s, a.y * s);
    //    }

    //    public static vec2 operator *(double s, vec2_to_delete a)
    //    {
    //        return new vec2(a.x * s, a.y * s);
    //    }
    //    public static vec2 operator /(vec2_to_delete a, double s)
    //    {
    //        return new vec2(a.x / s, a.y / s);
    //    }


    //    #endregion  // Operators




    //    /// <summary>Gets the hashcode of this <c>Scalar</c>.</summary>
    //    public override int GetHashCode()
    //    {
    //        return (x.GetHashCode() ^ y.GetHashCode());
    //    }



    //    #region Equality_Comparison_Hashing


    //    /// <summary>Indicates whether <c>obj</c> is equal to this instance.</summary>
    //    public override bool Equals(object obj)
    //    {
    //        return (obj is vec2) && this.Equals((vec2)obj);
    //    }

    //    /// <summary>Indicates whether <c>a</c> is equal to this instance. </summary>
    //    public bool Equals(vec2 a)
    //    {
    //        return (x == a.x && y == a.y);
    //    }

    //    #endregion Equality_Comparison_Hashing

    //    /// <summary>Returns norm of the Coordinate vector.</summary>
    //    public double Norm { get { return Math.Sqrt(x * x + y * y); } }

    //    public double Distance(vec2_to_delete b)
    //    {
    //        return (this - b).Norm;
    //    }


    //    public static double Distance(vec2 a, vec2 b)
    //    {
    //        return (a - b).Norm;
    //    }


    //} // struct Coord2D




}


#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LearnCs.Lib
{


    /// <summary>Acts mainly as marker interface for vector classes. Used everywhere one cannot specify the 
    /// concrete type of vector elements (for example, in type constraints or type specifications in generic 
    /// vector factories and factory registries).
    /// <para>For some examples, see: <see cref="VectorFactoryBase{VectorType}"/>, <see cref="VectorFactoryFromDelegate{VectorType}"/>,
    /// <see cref="VectorFactoryRegistry{BaseVectorType}"/>, <see cref="VectorFactoryRegistry"/></para></summary>
    public interface IVector
    {
    
    }

    /// <summary>Basic vector interface; does not include operations that should generate new vectors.</summary>
    /// <typeparam name="ElementType"></typeparam>
    public interface IVector<ElementType> : IVector
        where ElementType : struct
    {

        #region DataAccess
        
        /// <summary>Returns vector's dimension (nmber of components / elements).</summary>
        int Dim { get; }

        /// <summary>Gets vector component at the specified index.</summary>
        /// <param name="elementIndex">Index of vector component (element) to be retuurned.</param>
        /// <returns></returns>
        ElementType this[int elementIndex] { get; set; }

        #endregion DataAccess



        #region OperationsInstance

        /// <summary>Adds the specified vecor <paramref name="b"/> to the current vector and stores the result in <paramref name="result"/>.
        /// <para>Exception is thrown if any of parameters is null or their dimensions are incompattible (all vectors
        /// must have the same dimension than the current vector). The <paramref name="result"/> vector must be allocated with correct dimension.</para></summary>
        /// <param name="b">Vector summand that is added to the current vector.</param>
        /// <param name="result">Vector where the result of the operation is stored.</param>
        /// <exception cref="ArgumentNullException">When any of parameters is null.</exception>
        /// <exception cref="ArgumentException">When parameters have incomplatible dimensions.</exception>
        void AddVector(IVector<ElementType> b, IVector<ElementType> result);

        /// <summary>Subtracts the specified vecor <paramref name="b"/> from the current vector and stores the result in <paramref name="result"/>.
        /// <para>Exception is thrown if any of parameters is null or their dimensions are incompatible (all vectors
        /// must have the same dimension than the current vector). The <paramref name="result"/> vector must be allocated with correct dimension.</para></summary>
        /// <param name="b">Vector that is subtracted from the currrent vector.</param>
        /// <param name="result">Vector where the result of the operation is stored.</param>
        /// <exception cref="ArgumentNullException">When any of parameters is null.</exception>
        /// <exception cref="ArgumentException">When parameters have incomplatible dimensions.</exception>
        void SubtractVector(IVector<ElementType> b, IVector<ElementType> result);

        /// <summary>Multiplies the current vector by a scalar and stores the result to <paramref name="result"/>.</summary>
        /// <param name="s">Scalar by which the current vector <paramref name="v"/> is multiplied.</param>
        /// <param name="result">Vector wheree the result of the scalar-vector multiplication is stored. It must be
        /// allocated and of the correct dimension.</param>
        /// <exception cref="ArgumentNullException">When <paramref name="result"/> is null.</exception>
        /// <exception cref="ArgumentException">When <paramref name="result"/> has different dimension than the currrent vector.</exception>
        void MultiplyByScalar(ElementType s, IVector<ElementType> result);

        /// <summary>Calculates dot product of the specified vector <paramref name="b"/> and the current vector,
        /// i.e., the current vector is left-multiplied by <paramref name="b"/>.
        /// <para>Note that dot product for complex numbers is the sum of products of element of the first vector
        /// multiplied by the corresponding complex conjugate of the element of the second vector, and is not commutative.</para></summary>
        /// <param name="b">Vector by which the current vector is left=multiplied.</param>
        /// <returns>Dot product of <paramref name="b"/> and the currennt vector.</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="b"/> is null.</exception>
        /// <exception cref="ArgumentException">When <paramref name="b"/> has different dimension as the current vector.</exception>
        ElementType MultiplyLeft(IVector<ElementType> b);

        /// <summary>Calculates dot product of the current vector and the specified vector <paramref name="b"/>,
        /// i.e., the current vector is right-multiplied by <paramref name="b"/>.
        /// <para>Note that dot product for complex numbers is the sum of products of element of the first vector
        /// multiplied by the corresponding complex conjugate of the element of the second vector, and is not commutative.</para></summary>
        /// <param name="b">Vector by which the current vector is right-multiplied.</param>
        /// <returns>Dot product of the current vector and <paramref name="b"/>.</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="b"/> is null.</exception>
        /// <exception cref="ArgumentException">n <paramref name="b"/> has different dimension as the current vector.</exception>
        ElementType MultiplyRight(IVector<ElementType> b);

        /// <summary>Stores the copy of the current vector into <paramref name="result"/></summary>
        /// <param name="result">Vector into which the vector is stored.</param>
        /// <exception cref="ArgumentNullException">When <paramref name="result"/> is null.</exception>
        /// <exception cref="ArgumentNullException">When <paramref name="result"/> has a different dimension than the current vector.</exception>
        void CopyVector(IVector<ElementType> result);

        /// <summary>Stores the negative of the current vector into <paramref name="result"/></summary>
        /// <param name="result">Vector into which the result of the operation (calculating a negative value) is stored.</param>
        /// <exception cref="ArgumentNullException">When <paramref name="result"/> is null.</exception>
        /// <exception cref="ArgumentNullException">When <paramref name="result"/> has a different dimension than the current vector.</exception>
        void NegativeVector(IVector<ElementType> result);

        /// <summary>Stores the complex conjugate of the current vector into <paramref name="result"/></summary>
        /// <param name="result">Vector into which the result of the operation (calculating a negative complex conjugate) is stored.</param>
        /// <exception cref="ArgumentNullException">When <paramref name="result"/> is null.</exception>
        /// <exception cref="ArgumentNullException">When <paramref name="result"/> has a different dimension than the current vector.</exception>
        void ConjugateVector(IVector<ElementType> result);

        /// <summary>Returns the Euclidean norm of the current vector, i.e., the square root of sum of 
        /// squares of absolute values of its components.</summary>
        public ElementType Norm();

        #endregion

        #region InstanceFactoryMethods


        ///// <summary>Creates and returns a new vector of the specified type (<typeparamref name="ElementType"/>)
        ///// and dimension (<paramref name="dim"/>), in the same way as <see cref="Create{VectorType}(int)"/>.</summary>
        ///// <typeparam name="VectorType">Type of the vector to be created.</typeparam>
        ///// <param name="dim">Dimension of the vector to be created.</param>
        ///// <returns>The newly created vector of the specified type and dimension..</returns>
        //public VectorType CreateVector<VectorType>(int dim)
        //    where VectorType : class, IVector;

        /// <summary>Creeates and returns a new vector of dimension <paramref name="dim"/>, whose actual type is the 
        /// same as the type of the current vector object (which means it inherits from <see cref="VectorBase{ElementType}"/>).
        /// <para>The method throws exception if a factory for the precise vector type of the current object is not registered on 
        /// <see cref="Factories"/>.</para>
        /// <para>The declared type of the returned vector is VectorBase{ElementType} (where <typeparamref name="ElementType"/>
        /// will already be specified, because the method will be called on an object of a concrete inherited class), and the 
        /// object can be cast to other assignable type if necessary.</para></summary>
        /// <param name="dim">Dimension of the created vector object.</param>
        /// <remarks>For precise description of method behavior, see also <see cref="VectorFactoryRegistry{BaseVectorType}.CreateVector(Type, int)"/>.</remarks>
        public VectorBase<ElementType> CreateThisVectorType(int dim);


        /// <summary>Creeates and returns a new vector of dimension <paramref name="dim"/>, whose actual type is the 
        /// same as the type of the current vector object (which means it inherits from <see cref="VectorBase{ElementType}"/>).
        /// <para>The method throws exception if a factory for the precise vector type of the current object is not registered on 
        /// <see cref="Factories"/>.</para>
        /// <para>The declared type of the returned vector (<typeparamref name="VectorType"/>) must be assignable from the 
        /// actual type of the current object, otherwise the method throws exception.</para></summary>
        /// <param name="dim">Dimension of the created vector object.</param>
        /// <remarks>For precise description of method behavior, see also <see cref="VectorFactoryRegistry{BaseVectorType}.CreateVector(Type, int)"/>.</remarks>
        public VectorType CreateThisVectorType<VectorType>(int dim)
            where VectorType : VectorBase<ElementType>;

        #endregion InstanceFactoryMethods


        #region OperatorsOverloaded

        // Remarks:
        // Since C# 8 / .NET 3.1, static methods and operator overloads are allowed in interfaces.
        // Overloaded operators for vectors are defined in this interface rather than the base class VectorBase<ElementType>,
        // such that parameters can be of the more basic interface type and can therefore be used where vector is contained
        // in a variable of an interface type.

        // ToDo: add the missing overloaed operators!

#if false


        /// <summary>Overloaded operator + where parameters are <see cref="IVector{ElementType}"/> and the return value
        /// is <see cref="VectorBase{ElementType}"/> (so it can be used with variables of interface type).</param>
        /// <param name="a">The first operand.</param>
        /// <param name="b">The second operand.</param>
        /// <returns>Difference between the two vectors.</returns>
        /// <exception cref="ArgumentNullException">When any of parameterrs is null.</exception>
        public static VectorBase<ElementType> operator +(IVector<ElementType> a, IVector<ElementType> b)
        {
            if (a == null) throw new ArgumentNullException("Vector summation: the first operand is null.");
            if (b == null) throw new ArgumentNullException("Vector summation: the first operand is null.");
            VectorBase<ElementType> result = VectorBase<ElementType>.Create<VectorBase<ElementType>>(a.Dim);
            a.AddVector(b, result);
            return result;
        }


        /// <summary>Overloaded operator -.</summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static VectorBase<ElementType> operator -(IVector<ElementType> a, IVector<ElementType> b)
        {
            if (a == null) throw new ArgumentNullException("Vector subtraction: the first operand is null.");
            if (b == null) throw new ArgumentNullException("Vector subtraction: the first operand is null.");
            VectorBase<ElementType> result = VectorBase<ElementType>.Create<VectorBase<ElementType>>(a.Dim);
            a.SubtractVector(b, result);
            return result;
        }

        public static VectorBase<ElementType> operator *(ElementType s, IVector<ElementType> a)
        {
            if (a == null) throw new ArgumentNullException("Vector by scalar multiplication: vector operand is null.");
            VectorBase<ElementType> result = VectorBase<ElementType>.Create<VectorBase<ElementType>>(a.Dim);
            a.MultiplyByScalar(s, result);
            return result;
        }

        public static VectorBase<ElementType> operator *(IVector<ElementType> a, ElementType s)
        {
            if (a == null) throw new ArgumentNullException("Vector by scalar multiplication: vector operand is null.");
            VectorBase<ElementType> result = VectorBase<ElementType>.Create<VectorBase<ElementType>>(a.Dim);
            a.MultiplyByScalar(s, result);
            return result;
        }

        public static ElementType operator *(IVector<ElementType> a, IVector<ElementType> b)
        {
            if (a == null) throw new ArgumentNullException("Dot product (scalar product of vectors): first operand is null.");
            if (b == null) throw new ArgumentNullException("Dot product (scalar product of vectors): second operand is null.");
            return a.MultiplyRight(b);
        }

        public static VectorBase<ElementType> operator -(IVector<ElementType> a)
        {
            if (a == null) throw new ArgumentNullException("Negative vector (unary -): the operand is null.");
            VectorBase<ElementType> result = VectorBase<ElementType>.Create<VectorBase<ElementType>>(a.Dim);
            a.NegativeVector(result);
            return result;
        }

        public static VectorBase<ElementType> operator +(IVector<ElementType> a)
        {
            if (a == null) throw new ArgumentNullException("Positive vector (unary +): the operand is null.");
            VectorBase<ElementType> result = VectorBase<ElementType>.Create<VectorBase<ElementType>>(a.Dim);
            a.CopyVector(result);
            return result;
        }

        public static VectorBase<ElementType> Conjugate(IVector<ElementType> a)
        {
            if (a == null) throw new ArgumentNullException("Conjugate vector: the operand is null.");
            VectorBase<ElementType> result = VectorBase<ElementType>.Create<VectorBase<ElementType>>(a.Dim);
            a.ConjugateVector(result);
            return result;
        }
#endif


        #endregion OperatorsOverloaded


    }


    /// <summary>Generic vecor interface that various vector types should implement.</summary>
    /// <typeparam name="ElementType">Type of elements of the the vector.</typeparam>
    /// <typeparam name="VectorRetType">Actual type of the vector.</typeparam>
    public interface IVector<ElementType, VectorRetType>: IVector<ElementType>
        where ElementType : struct
        where VectorRetType : IVector<ElementType>
    {

        /// <summary>Creates and returns a vector of the specified dimension <paramref name="dim"/>, or, if the 
        /// parameter is less than 0, of the same dimension as the current vector.</summary>
        /// <param name="dim">Dimension of the vecto to be created. If less than 0 then a vector of the 
        /// same dimension as the current vector is created.</param>
        /// <returns>Newly created vector.</returns>
        VectorRetType CreateVector(int dim = -1);

    }

}

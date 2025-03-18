using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Linq;


namespace LearnCs.Lib
{

    /// <summary>Abstract base class for vectors with the specific element type (<typeparamref name="ElementType"/>).
    /// <para>This completely abstract class serves mainly as a placeholder for the implementation of classes that
    /// implement the <see cref="IVector{ElementType}"/> interface, which is the most practical interface of vector 
    /// classes. It is used in some rare cases where one cannot directy use an interface and needs to use a class
    /// (e.g., in definitions of explicit and implicit type conversions). All members of this class are abstract.</para>
    /// <para>As actual generic base class that contains actual implementations where possible, use the 
    /// <see cref="VectorBase{ElementType, VectorRetType}"/> class.</para></summary>
    /// <typeparam name="ElementType">Specifies the type of vector elements.</typeparam>
    public abstract class VectorBase<ElementType> : NumberArrayLikeClass<ElementType>, IVector<ElementType>
        where ElementType : struct // constraint means that ElementType is a value type (not a reference type).
    {

        #region Constructors

        protected VectorBase() { }

        public VectorBase(int dim)
        {
            _components = new ElementType[dim];
        }

        public VectorBase(ElementType[] elements, bool copyElements = false)
        {
            if (copyElements)
            {
                // Don't reuse the specified array; instead, allocate a new one and copy elements:
                _components = new ElementType[elements.Length];
                for (int i = 0; i < elements.Length; i++)
                {
                    _components[i] = elements[i];
                }
            }
            else
            {
                // Just move elements to Components:
                _components = elements;
            }
        }

        public VectorBase(params ElementType[] elements)
        {
            if (elements == null)
            {
                throw new ArgumentNullException(nameof(elements), "Vector cannot be initialized with null array.");
            }
            _components = new ElementType[elements.Length];
            for (int i = 0; i < elements.Length; i++)
            {
                _components[i] = elements[i];
            }
        }

        public VectorBase(IVector<ElementType> otherVector)
        {
            int dim = otherVector.Dim;
            _components = new ElementType[dim];
            for (int i = 0; i < dim; i++)
            {
                _components[i] = otherVector[i];
            }
        }

        #endregion Constructors


        #region Data

        [JsonInclude]
        protected ElementType[] _components;

        /// <summary>Returns vector's dimension (nmber of components / elements).</summary>
        // [JsonIgnore]
        public virtual int Dim => _components?.Length ?? 0;  // short form expression body definition can be used for simple read-only properties (no setter)
        // Longer form of the above:
        //{
        //    get
        //    {
        //        if (_components == null)
        //        {
        //            return 0;
        //        }
        //        else
        //        {
        //            return _components.Length;
        //        }
        //    }
        //}

        /// <summary>Gets vector component at the specified index.</summary>
        /// <param name="elementIndex">Index of vector component (element) to be retuurned.</param>
        /// <returns></returns>
        public virtual ElementType this[int elementIndex]
        {
            get
            {
                return _components[elementIndex];
            }
            set
            {
                _components[elementIndex] = value;
            }
        }

#if false
        ///// <summary>Returns the rank - the number of dimensions of the potentially multidimensional array-like structure 
        ///// of numbers, which contains data for the current object.</summary>
        //// [JsonIgnore]
        //public override int Rank { get; } = 1;

        ///// <summary>Gets a number that represents the number of elements along the specified dimension of the 
        ///// abstracted multidimensional array that contains data for the object defined by the current class.</summary>
        ///// <param name="dimension">The consecutive number of the dimension (starting at 0) along which the corresponding
        ///// number of elements is returned.</param>
        //public override int GetLength(int dimension)
        //{
        //    if (dimension == 0)
        //    {
        //        return _components?.Length ?? 0;
        //    }
        //    throw new IndexOutOfRangeException($"Provided dimension iindex {dimension} is out of range: it can only be 0 because {this.GetType().Name} has only one dimension.");
        //}

        ///// <summary>Gets a long integer that represents the total number of elements in all the dimensions of the
        ///// array that contains the number elements of the current array-like class.</summary>
        //// [JsonIgnore]
        //public override int Length => _components?.Length ?? 0;
#endif

        #region IENumerable

        // ToDo: test implementation of IENumerable<ElementType>, e,g, by testing foreach(...)

        public IEnumerator<ElementType> GetEnumerator()
        {
            return new VectorElementEnumerator(_components);
        }

        public class VectorElementEnumerator: IEnumerator<ElementType>
        {

            private ElementType[] _elements;
            private int _currentIndex;
            private ElementType _currentElement;


            public VectorElementEnumerator(ElementType[] elementCollection)
            {
                _elements = elementCollection;
                _currentIndex = -1;
                _currentElement = default;
            }

            public bool MoveNext()
            {
                if (++_currentIndex >= _elements.Length)
                {
                    return false;
                }
                else
                {
                    // Set current element to the next item in collection.
                    _currentElement = _elements[_currentIndex];
                }
                return true;
            }

            public void Reset() { _currentIndex = -1; }

            public ElementType Current
            {
                get { return _currentElement; }
            }

            object IEnumerator.Current
            {
                get { return Current; }
            }

            void IDisposable.Dispose() { }

        }


        #endregion IENumerable


#endregion Data


        #region TypeConversions

        /// <summary>Implicit conversion of an array with the same element type (<typeparamref name="ElementType"/>)
        /// to the current generic base vector type (<see cref="VectorBase{ElementType}"/>).
        /// <para>This implicit conversion method uses the static <see cref="VectorBase{ElementType}.Create{VectorType}(int)"/>
        /// method to create the vector that is converted from an array. This works via registered factories for vector
        /// types, therefore it has some performance penalty, and it may fail if the factory for the specific 
        /// vector type (<see cref="VectorBase{ElementType}"/>) is not registered.</para></summary>
        /// <param name="array">array that is implicitly converted to the current vector base type.</param>
        /// <exception cref="ArgumentNullException">Wheen <paramref name="array"/> is null.</exception>
        public static implicit operator VectorBase<ElementType>(ElementType[] array)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array), $"Array to be converted to {typeof(VectorBase<ElementType>)} is null.");
            }
            VectorBase<ElementType> ret = Create<VectorBase<ElementType>>(array.Length);
            for (int i = 0; i < array.Length; ++i)
            {
                ret[i] = array[i];
            }
            return ret;
        }

        /// <summary>Implicit conversion of an array with the same element type (<typeparamref name="ElementType"/>)
        /// to the current generic base vector type (<see cref="VectorBase{ElementType}"/>).</summary>
        /// <param name="vec">Vector that is implicitly converted to the current vector base type.</param>
        public static implicit operator ElementType[](VectorBase<ElementType> vec)
        {
            if (vec == null)
            {
                return null;
            }
            ElementType[] ret = new ElementType[vec.Dim];
            for (int i = 0; i < vec.Dim; ++i)
            {
                ret[i] = vec[i];
            }
            return ret;
        }

        #endregion TypeConversions


        #region Operations_Instance_Plain

        // Remarks:
        // Plain operations do not require creation of a new vector object, and can thus operate merely on objects of
        // interface IVector<ElementType>.
        // Methods in this section form the basis for vector operations. There will be a need for different kinds of methods
        // supporting vector operations, such as operator overloads, but all shoul call these methods for maximal code reuse.
        // Code reuse in this case also makes it easier to guard against regressions.
        //
        // Operations in this section use the inherited abstract property ElementOperations of type 


        /// <inheritdoc/>
        public void AddVector(IVector<ElementType> b, IVector<ElementType> result)
        {
            if (b == null)
            {
                throw new ArgumentNullException($"{nameof(AddVector)}: The second vector sumand is null.");
            }
            if (result == null)
            {
                throw new ArgumentNullException($"{nameof(AddVector)}: The resulting vector to store the sum of vectors is null.");
            }
            if (b.Dim != Dim)
            {
                throw new ArgumentException($"{nameof(AddVector)}: Dimensions of vector summands do not match: {b.Dim} vs. {Dim}");
            }
            if (result.Dim != Dim)
            {
                throw new ArgumentException($"{nameof(AddVector)}: Dimension of vector to store the result is wrong: {result.Dim} vs. {Dim}");
            }
            for (int i = 0; i < Dim; i++)
            {
                result[i] = NumberOperations.AddNumbers(this[i], b[i]);
            }
        }


        /// <inheritdoc/>
        public void SubtractVector(IVector<ElementType> b, IVector<ElementType> result)
        {
            if (b == null)
            {
                throw new ArgumentNullException($"{nameof(SubtractVector)}: The second vector to subtract (subtrahend) is null.");
            }
            if (result == null)
            {
                throw new ArgumentNullException($"{nameof(SubtractVector)}: The resulting vector to store the difference of vectors is null.");
            }
            if (b.Dim != Dim)
            {
                throw new ArgumentException($"{nameof(SubtractVector)}: The dimensions of vectors to be subtracted do not match: {b.Dim} vs. {Dim}");
            }
            if (result.Dim != Dim)
            {
                throw new ArgumentException($"{nameof(SubtractVector)}: Dimension of vector to store the result is wrong: {result.Dim} vs. {Dim}");
            }
            for (int i = 0; i < Dim; i++)
            {
                result[i] = NumberOperations.SubtractNumbers(this[i], b[i]);
            }
            
        }

        /// <inheritdoc/>
        public void MultiplyByScalar(ElementType s, IVector<ElementType> result)
        {
            if (result == null)
            {
                throw new ArgumentNullException($"{nameof(MultiplyByScalar)}: The resulting vector to store scalar-vector product is null.");
            }
            if (result.Dim != Dim)
            {
                throw new ArgumentException($"{nameof(MultiplyByScalar)}: Dimension of vector to store the result is wrong: {result.Dim} vs. {Dim}");
            }
            for (int i = 0; i < Dim; i++)
            {
                result[i] = NumberOperations.MultiplyNumbers(this[i], s); 
            }            
        }

        /// <inheritdoc/>
        public virtual ElementType MultiplyLeft(IVector<ElementType> b)
        {
            if (b == null)
            {
                throw new ArgumentNullException($"{nameof(MultiplyLeft)}: The vector to be left-multiplied is null..");
            }
            if (b.Dim != Dim)
            {
                throw new ArgumentException($"{nameof(MultiplyLeft)}: Dimensions of factors in dot product do not match: {b.Dim} vs. {Dim}");
            }
            ElementType result = default(ElementType); // For number types of interest, default coincedes with zero for these types.
            for (int i = 0; i < Dim; i++)
            {
                result = NumberOperations.AddNumbers(result,
                    NumberOperations.MultiplyNumbers(b[i], NumberOperations.ConjugateNumber(this[i]))
                    );
            }
            return result;
        }

        /// <inheritdoc/>
        public virtual ElementType MultiplyRight(IVector<ElementType> b)
        {
            if (b == null)
            {
                throw new ArgumentNullException($"{nameof(MultiplyRight)}: The vector to be left-multiplied is null..");
            }
            if (b.Dim != Dim)
            {
                throw new ArgumentException($"{nameof(MultiplyRight)}: Dimensions of factors in dot product do not match: {b.Dim} vs. {Dim}");
            }
            ElementType result = default(ElementType);
            for (int i = 0; i < Dim; i++)
            {
                result = NumberOperations.AddNumbers(result,
                    NumberOperations.MultiplyNumbers(this[i], NumberOperations.ConjugateNumber(b[i]))
                    );
            }
            return result;
        }

        /// <inheritdoc/>
        public virtual void CopyVector(IVector<ElementType> result)
        {
            if (this == null)
            { 
                throw new ArgumentNullException($"{ nameof(CopyVector)}: The vector to be copied is null..");
            }
            if (this.Dim != result.Dim)
            {
                throw new ArgumentException($"{nameof(CopyVector)}: Dimensions of vectors do not match: {this.Dim} vs. {result.Dim}");
            }

            for (int i = 0; i < Dim; i++)
            {
                result[i] = this[i];
            }
        }

        /// <inheritdoc/>
        public virtual void NegativeVector(IVector<ElementType> result)
        {
            if (this == null)
            { 
                throw new ArgumentNullException($"{ nameof(NegativeVector)}: The vector to become negative one is null..");
            }
            if (this.Dim != result.Dim)
            {
                throw new ArgumentException($"{nameof(NegativeVector)}: Dimensions of vectors do not match: {this.Dim} vs. {result.Dim}");
            }

            for (int i = 0; i < Dim; i++)
            {
                result[i] = NumberOperations.NegativeNumber(this[i]); 
            }
        }

        /// <inheritdoc/>
        public virtual void ConjugateVector(IVector<ElementType> result)
        {
            if (this == null)
            { 
                throw new ArgumentNullException($"{nameof(ConjugateVector)}: The vector which conjugate value is returned is null..");
            }
            if (this.Dim != result.Dim)
            { 
                throw new ArgumentNullException($"{nameof(ConjugateVector)}> Dimensions of vectors do not match> {this.Dim} vs. {result.Dim}");
            }

            for (int i = 0; i < Dim; i++)
            {
                result[i] = NumberOperations.ConjugateNumber(this[i]);
            }
        }

        /// <inheritdoc/>
        public virtual ElementType Norm()
        {
            int dim = Dim;
            ElementType ret = default(ElementType);
            for (int i = 0; i < Dim; ++i)
            {
                ret = NumberOperations.AddNumbers(ret,
                    NumberOperations.AbsoluteSquare(this[i])
                    );
            }
            ret = NumberOperations.Sqrt(ret);
            return ret;
        }

        #endregion Operations_Instance_Plain


        /// <summary>Returns a sring representation of the current vector.
        /// <para>The form is similar to "[1.1, 2.2, 3.3, 3.4]".</para></summary>
        public override string ToString()
        {

            // ToDo: Improve this method to satisfy the description and to produce nicely formatted output!

            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            for (int i = 0; i < Dim; ++i)
            {
                sb.Append(this[i].ToString());  // We exploit the fact thatt ToString() is defined for any type
                if (i < this.Dim - 1)
                {
                    sb.Append(", ");
                }
            }
            sb.Append("}");
            return sb.ToString();
        }

        #region InstanceFactoryMethods


        ///// <summary>Creates and returns a new vector of the specified type (<typeparamref name="ElementType"/>)
        ///// and dimension (<paramref name="dim"/>), in the same way as <see cref="Create{VectorType}(int)"/>.</summary>
        ///// <typeparam name="VectorType">Type of the vector to be created.</typeparam>
        ///// <param name="dim">Dimension of the vector to be created.</param>
        ///// <returns>The newly created vector of the specified type and dimension..</returns>
        //public VectorType CreateVector<VectorType>(int dim)
        //    where VectorType : class, IVector
        //{
        //    return Factories.CreateVector<VectorType>(dim);
        //}

        /// <summary>Creeates and returns a new vector of dimension <paramref name="dim"/>, whose actual type is the 
        /// same as the type of the current vector object (which means it inherits from <see cref="VectorBase{ElementType}"/>).
        /// <para>The method throws exception if a factory for the precise vector type of the current object is not registered on 
        /// <see cref="Factories"/>.</para>
        /// <para>The declared type of the returned vector is VectorBase{ElementType} (where <typeparamref name="ElementType"/>
        /// will already be specified, because the method will be called on an object of a concrete inherited class), and the 
        /// object can be cast to other assignable type if necessary.</para></summary>
        /// <param name="dim">Dimension of the created vector object.</param>
        /// <remarks>For precise description of method behavior, see also <see cref="VectorFactoryRegistry{BaseVectorType}.CreateVector(Type, int)"/>.</remarks>
        public VectorBase<ElementType> CreateThisVectorType(int dim)
        {
            return Factories.CreateVector<VectorBase<ElementType>>(this.GetType(), dim);
        }

        
        /// <summary>Creeates and returns a new vector of dimension <paramref name="dim"/>, whose actual type is the 
        /// same as the type of the current vector object (which means it inherits from <see cref="VectorBase{ElementType}"/>).
        /// <para>The method throws exception if a factory for the precise vector type of the current object is not registered on 
        /// <see cref="Factories"/>.</para>
        /// <para>The declared type of the returned vector (<typeparamref name="VectorType"/>) must be assignable from the 
        /// actual type of the current object, otherwise the method throws exception.</para></summary>
        /// <param name="dim">Dimension of the created vector object.</param>
        /// <remarks>For precise description of method behavior, see also <see cref="VectorFactoryRegistry{BaseVectorType}.CreateVector(Type, int)"/>.</remarks>
        public VectorType CreateThisVectorType<VectorType>(int dim)
            where VectorType: VectorBase<ElementType>
        {
            return Factories.CreateVector<VectorType>(this.GetType(), dim);
        }

        #endregion InstanceFactoryMethods


        #region StaticFactoryMethod

        // This region enables the static CreateVector methods by registering an abstract vector factoriy for each actual type of vector.

        /// <summary>
        /// Registry of vector factories for different types of vectors. 
        /// <para>The <see cref="VectorBase{ElementType}.Create{VectorType}(int)"/> method uses this registry in order to get the factory 
        /// to which it delegates creation of the specified type of vector.</para>
        /// <para>Vector factories for vector types from this library are registered by the static constructor <see cref="VectorBase{ElementType}.VectorBase()"/>.
        /// Factories for creation of other vector types can be registered in their static constructors or in any other static constructor fit for 
        /// this purpose.</para>
        /// </summary>
        public static IVectorFactoryRegistry<IVector> Factories { get; }

        /// <summary>Static constructor. Registers vector factories for many known vector types, including for
        /// the generic interface types <see cref="IVector{ElementType}"/> and generic abstract class types 
        /// <see cref="VectorBase{ElementType}"/> for type parameters <see cref="double"/>, <see cref="Complex"/> and <see cref="float"/>.</summary>
        /// <remarks>C# language specification guarantees that staic constructor is called before the first instance of the class is
        /// created and before any static members are accessed.
        /// <para>For vector classes whose factories are not registered here, these classes should register their own factories
        /// in their static constructors. In ths way, the <see cref="VectorBase{ElementType}.CreateVector{VectorType}(int)"/> method 
        /// can create vectors even for vector types that might be defined in other libraries that reference the current library.</para>
        /// <para>For example of how to register vector factory objects for additional vector types in other static constructors, see
        /// the static constructor of the <see cref="Vector"/> class.</para></remarks>
        static VectorBase()
        {
            // Initiaize the static registry of vector factories, which will be used to instantiate any type
            // of vectors:
            Factories = new VectorFactoryRegistry();
            // Register factories for basic vector interfaces for different vector element types:
            //$$ Factories.RegisterFactory<IVector<double>>(dim => new Vector(dim));
            Factories.RegisterFactory<IVector<Complex>>(dim => new ComplexVector(dim));
            //$$ Factories.RegisterFactory<IVector<float>>(dim => new FloatVector(dim));

            // Register factories for generic base class for different vector element types:
            //$$ Factories.RegisterFactory<VectorBase<double>>(dim => new Vector(dim));
            Factories.RegisterFactory<VectorBase<Complex>>(dim => new ComplexVector(dim));
            //$$ Factories.RegisterFactory<VectorBase<float>>(dim => new FloatVector(dim));

            // Then, also register factories for known concrete vector types:
            //$$ Factories.RegisterFactory(dim => new Vector(dim));
            Factories.RegisterFactory(dim => new ComplexVector(dim));  // this RegisterFactory overload converts a delegate (lambda expression in brackets) to a vector factory before registration
            // differeft (longer) variant of the line above:  // $$ to remember!
            // Factories.RegisterFactory(new VectorFactoryFromDelegate<ComplexVector>(dim => new ComplexVector(dim)));  // explicit creation of a factory from a delegate
            // Even longer form where delegate is assigned with a new syntax:
            // Factories.RegisterFactory(new VectorFactoryFromDelegate<ComplexVector>(new Func<int, ComplexVector>(dim => new ComplexVector(dim))));  // new syntax for delegate assignment
            //$$ Factories.RegisterFactory(dim => new FloatVector(dim));
        }


        /// <summary>Creates and returns a new vector of type <typeparamref name="VectorType"/>.</summary>
        /// <typeparam name="VectorType">Type of the vector to be created.</typeparam>
        /// <param name="dim">Dimention of the vector to be created.</param>
        /// <returns>The created vector.</returns>
        /// <exception cref="InvalidCastException">When the factory for creation of the specified type of vectors
        /// is not registered on <see cref="Factories"/> (thrown by <see cref="VectorFactoryRegistry{BaseVectorType}.CreateVector{VectorType}(int)"/>).</exception>
        /// <remarks>
        /// <para>This method delegates the task to the static <see cref="VectorBase{ElementType}.Factories"/> object of type 
        /// <see cref="VectorFactoryRegistry"/>, its instance method <see cref="VectorFactoryRegistry{BaseVectorType}.CreateVector{VectorType}(int)"/>.</para>
        /// <para>For method call to succeed, the factory for the specified type (<typeparamref name="VectorType"/>) must be registered.
        /// The current vector base type (<see cref="VectorBase{ElementType}"/>) performs reegistration of factories for some known types 
        /// in its static constructor, <see cref="VectorBase{ElementType}.VectorBase()"/></para>
        /// <para>For vector classes whose factories are not registered here, these factories should register their own factories
        /// in their static constructors. In ths way, the current method can create new vectors of vector types that might be defined 
        /// in other libraries that reference the current library,and are not even accessible in this library.</para>
        /// </remarks>
        public static VectorType Create<VectorType>(int dim)
            where VectorType : class, IVector
        {
            return Factories.CreateVector<VectorType>(dim);// work is delegated to VectorFactoryRegistry.CreateVector()
        }

        #endregion StaticFactoryMethod

    }

    /// <summary>Abstract base class for vectors of different element types (with element types being number types like 
    /// double, float, int, Complex, etc.)</summary>
    /// <typeparam name="ElementType">Type of elements of the vectors that inherit from this base class.</typeparam>
    /// <typeparam name="VectorRetType">Actual type of a vector class that inherits from this base class; this is the
    /// vector type that is returned by the <see cref="CreateVector(int)"/> method. This type parameter is mainly defined
    /// for this purpose, such that it can be used in operations that need to return a new vector. For example, in 
    /// many cases this will be the current type on which instant members operate (this is the case when the derived
    /// vector states its own type as this type parameter <typeparamref name="VectorRetType"/>).
    /// <para>When deriving a concrete class from <see cref="VectorBase{ElementType, VectorType}"/>, this type parameter 
    /// must be set to class that is deriving.</para></typeparam>
    public abstract class VectorBase<ElementType, VectorRetType> : VectorBase<ElementType>,
        // IVector<ElementType>,   // commented because it is implemented by the interface below
        IVector<ElementType, VectorRetType>
        where ElementType : struct
        where VectorRetType : VectorBase<ElementType, VectorRetType>, IVector<ElementType>
    {

        #region Constructors

        protected VectorBase() : base() 
        {  }

        public VectorBase(int dim) : base(dim) 
        {  }

        public VectorBase(ElementType[] elements, bool copyElements = false) : base(elements, copyElements) 
        {  }

        public VectorBase(params ElementType[] elements): base(elements)
        {  }

        public VectorBase(IVector<ElementType> otherVector) : base(otherVector)
        {  }

        #endregion Constructors


        #region TypeConversions

        /// <summary>Implicit conversion of one-dimensional array of <typeparamref name="ElementType"/> to the 
        /// current generic base vector type.
        /// <para>In concrete types that inherit from the current generic base type (<see cref="VectorBase{ElementType}"/>),
        /// the type parameters <typeparamref name="ElementType"/> and <typeparamref name="VectorRetType"/> will 
        /// be actualize to concrete element and vector type.</para></summary>
        /// <param name="array">The array to be converted.</param>
        public static implicit operator VectorBase<ElementType, VectorRetType>(ElementType[] array)
        {
            if (array == null) { return null; }
            int numElements = array.Length;
            VectorRetType returned = Create<VectorRetType>(numElements);
            for (int i = 0; i < numElements; ++i)
            {
                returned[i] = array[i];
            }
            return returned;
        }

        /// <summary>Conversion of objects of the current base type to one-dimensional array with elements
        /// of type <typeparamref name="ElementType"/>.
        /// <para>In concrete types that inherit from the current generic base type (<see cref="VectorBase{ElementType, VectorRetType}"/>),
        /// the type parameters <typeparamref name="ElementType"/> and <typeparamref name="VectorRetType"/> will be 
        /// actualized to concrete element and vector types.</para></summary>
        /// <param name="vector">The vector that is converted.</param>
        public static implicit operator ElementType[](VectorBase<ElementType, VectorRetType> vector)
        {
            if (vector == null)
            { return null; }
            ElementType[] returnedArray = new ElementType[vector.Dim];
            for (int i = 0; i < returnedArray.Length; i++)
            {
                returnedArray[i] = vector[i];
            }
            return returnedArray;
        }


        /// <summary>Implicit conversion of this base type to the <typeparamref name="VectorRetType"/>.
        /// <para>In concrete types that inherit from the current generic base type (<see cref="VectorBase{ElementType, VectorRetType}"/>),
        /// the type parameters <typeparamref name="ElementType"/> and <typeparamref name="VectorRetType"/> will 
        /// be actualize to concrete element and vector type.</para></summary>
        /// <param name="otherVector">The vector that is implicitly converted to <typeparamref name="VectorRetType"/>.</param>
        public static implicit operator VectorRetType(VectorBase<ElementType, VectorRetType> otherVector)
        {
            return otherVector;
            //if (otherVector == null) { return null; }
            //int numElements = otherVector.Dim;
            //VectorRetType returned = CreateVector<VectorRetType>(numElements);
            //for (int i = 0; i < numElements; ++i)
            //{
            //    returned[i] = otherVector[i];
            //}
            //return returned;
        }


        /// <summary>Implicit conversion of this base type to the <typeparamref name="VectorRetType"/>.
        /// <para>In concrete types that inherit from the current generic base type (<see cref="VectorBase{ElementType, VectorRetType}"/>),
        /// the type parameters <typeparamref name="ElementType"/> and <typeparamref name="VectorRetType"/> will 
        /// be actualize to concrete element and vector type.</para></summary>
        /// <param name="otherVector">The vector that is implicitly converted to <typeparamref name="VectorRetType"/>.</param>
        public static implicit operator VectorBase<ElementType, VectorRetType>(VectorRetType otherVector)
        {
            return otherVector;
            //if (otherVector == null) { return null; }
            //int numElements = otherVector.Dim;
            //VectorRetType returned = CreateVector<VectorRetType>(numElements);
            //for (int i = 0; i < numElements; ++i)
            //{
            //    returned[i] = otherVector[i];
            //}
            //return returned;
        }

        #endregion TypeConversions


        #region InstanceFactoryMethods

        /// <summary>Creates a vector of the specified dimension.</summary>
        /// <param name="dim">Dimension of the created vector. If less than 0 then dimension 
        /// of the current vector is taken.</param>
        /// <returns>The newly created vector of the dimension specified by <paramref name="dim"/>.</returns>
        public abstract VectorRetType CreateVector(int dim = -1);

        #endregion InstanceFactoryMethods



        #region OperatorOverloaded

        // Remarks:
        // These operator overloads enable use of operators that returna a vector of the type VectorRetType (type parameter
        // of this class), which is defined in derived classes, usually to be equal the type of the derived class itself.
        // For example, class Vector inherits from this class where the generic type parameter VectorRetType is set to Vector.
        //
        // Binary operators overloads usually have three variants defined. One variant is for cases where the first operand
        // is of the current type but the second operand may be any vector type that has the same ElementType (generic type parameter).
        // Then there is a variant where these two types are swapped. There is also the third variant where types of both operands
        // are set to the current class. This case is covered by both first two variants, but because of this it is ambiguous
        // which variant of the overload the compiler should select, and code does not compile when both arguments are of the
        // current type. If there is a special method for both parameters being of the current type, the ambiguity is resolved
        // because the compiler gives priority to this overload, whose parameters are more derived types than with the other
        // two variants.

        /// <summary>Overloaded operator + where the first operand is of the current type and the second operator is 
        /// any vector with the same interface (<see cref="IVector<typeparamref name="ElementType"/>).</summary>
        /// <param name="a">The first operand.</param>
        /// <param name="b">The second operand.</param>
        /// <returns>Sum of both vectors as <typeparamref name="VectorRetType"/>, which is derived from the current generic base class.</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="a"/> is null.</exception>
        public static VectorRetType operator +(VectorBase<ElementType, VectorRetType> a, IVector<ElementType> b)
        {
            if (a == null) throw new ArgumentNullException("Vector summation: the first operand is null.");
            VectorRetType result = a.CreateVector(a.Dim);
            a.AddVector(b, result);
            return result;
        }



        public static VectorRetType operator +(IVector<ElementType> a, VectorBase<ElementType, VectorRetType> b)
        {
            if (b == null) throw new ArgumentNullException("Vector summation: the second operand is null.");
            return b + a;
            //if (b == null) throw new ArgumentNullException("Vector summation: the second operand is null.");
            //VectorRetType result = b.CreateVector(a.Dim);
            //b.AddVector(a, result);
            //return result;
        }

        /// <summary>Overload for operator + where both operands are of the current type. </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static VectorRetType operator +(VectorBase<ElementType, VectorRetType> a, VectorBase<ElementType, VectorRetType> b)
        {
            if (a == null) throw new ArgumentNullException("Vector summation: the first operand is null.");
            VectorRetType result = a.CreateVector(a.Dim);
            a.AddVector(b, result);
            return result;
        }

        /// <summary>Overloaded operator -.</summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static VectorRetType operator -(VectorBase<ElementType, VectorRetType> a, IVector<ElementType> b)
        {
            if (a == null) throw new ArgumentNullException("Vector subtraction: first operand is null.");
            VectorRetType result = a.CreateVector(a.Dim);
            a.SubtractVector(b, result);
            return result;
        }

        public static VectorRetType operator -(IVector<ElementType> a, VectorBase<ElementType, VectorRetType> b)
        {
            if (b == null) throw new ArgumentNullException("Vector subtraction: second operand is null.");
            VectorRetType result = b.CreateVector(b.Dim);
            a.SubtractVector(b, result);
            return result;
        }

        public static VectorRetType operator *(ElementType s, VectorBase<ElementType, VectorRetType> a)
        {
            if (a == null) throw new ArgumentNullException("Vector-scalar multiplication: vector is null.");
            VectorRetType result = a.CreateVector(a.Dim);
            a.MultiplyByScalar(s, result);
            return result;
        }

        public static VectorRetType operator *(VectorBase<ElementType, VectorRetType> a, ElementType s)
        {
            return s * a; 
        }

        public static ElementType operator *(VectorBase<ElementType, VectorRetType> a, IVector<ElementType> b)
        {
            if (a == null) throw new ArgumentNullException("Left vector multiplication (dot product): first operand is null.");
            return a.MultiplyRight(b);
        }

        public static ElementType operator *(IVector<ElementType> a, VectorBase<ElementType, VectorRetType> b)
        {
            if (b == null) throw new ArgumentNullException("Left vector multiplication (dot product): first operand is null.");
            return b.MultiplyLeft(a);
        }

        public static VectorRetType operator-(VectorBase<ElementType, VectorRetType> a)
        {
            if (a == null) throw new ArgumentNullException("Negative vector (unary -): the operand is null.");
            VectorRetType result = a.CreateVector(a.Dim);
            a.NegativeVector(result);
            return result;
        }


        public static VectorRetType operator +(VectorBase<ElementType, VectorRetType> a)
        {
            if (a == null) throw new ArgumentNullException("Positive vector (unary +): the operand is null.");
            VectorRetType result = a.CreateVector(a.Dim);
            a.NegativeVector(result);
            return result;
        }


        public static VectorRetType Conjugate(VectorBase<ElementType, VectorRetType> a)
        {
            if (a == null) throw new ArgumentNullException("Conjugate vector: the operand is null.");
            VectorRetType result = a.CreateVector(a.Dim);
            a.ConjugateVector(result);
            return result;
        }


        #endregion OperatorOverloaded


        #region HashCodeAndEqualsOverride
        // This contains overrides of the Equals() and GetHashCode() methods.

        /// <summary>Overrides <see cref="object.Equals(object?)"/> method. Returns true if <paramref name="obj"/> is also
        /// an <see cref="IVector{ElementType}"/> and it has the same dimension and elements, otherwise it returns false.</summary>
        /// <param name="obj">Object that is compered with the current vector for equality.</param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            IVector<ElementType> compared = obj as IVector<ElementType>;
            if (compared == null)  // this happens if obj is not IVector<ElementType>, i.e., it does not implement this interface
            {
                return false;
            }
            if (compared.Dim != this.Dim)
            {
                return false;
            }
            for (int i = 0; i < this.Dim; ++i)
            {
                if (!compared[i].Equals(this[i]))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>Override of <see cref="object.GetHashCode()"/>, to be suitable for vectors. See also:
        /// <para>https://stackoverflow.com/questions/1646807/quick-and-simple-hash-code-combinations</para>
        /// <para>https://stackoverflow.com/questions/638761/gethashcode-override-of-object-containing-generic-array</para>
        /// <para>https://stackoverflow.com/questions/299304/why-does-javas-hashcode-in-string-use-31-as-a-multiplier</para></summary>
        /// <returns>An integer hash code of the current complex number, suitable for use in hash tables.</returns>
        public override int GetHashCode()
        {
            int hash = 17;
            hash = hash * 31 + Dim.GetHashCode();
            for (int i = 0; i < this.Dim; ++i)
            {
                hash = hash * 31 + this[i].GetHashCode();
            }
            return hash;
        }

        #endregion HashCodeAndEqualsOverride


        #region StringConversions

        /// <summary>Parses a vector object from its string representation.
        /// <para>String representaition must be in the form "[v1, v2, v3]", e.g.:</para>
        /// <para>[0.43, 1.345, 5.6e-3, 22.4]</para></summary>
        /// <param name="s">String to be parsed.</param>
        /// <param name="result">The variable where the result will be stored. null is stored if not successful.</param>
        /// <returns>True if parsing was successful and a meaningful vector value is writen to <paramref name="result"/>,
        /// false if not (in which case null is written to <paramref name="result"/>).</returns>
        public static bool TryParse(string s, out VectorRetType result)
        {
            // ToDo: implement this!
            throw new NotImplementedException();
        }

        /// <summary>Parses a vector from its string representation. Exception is thrown if vector value cnnot be
        /// parsed from the string.</summary>
        /// <param name="s">String representation of the vector from which the vector value is parsed.</param>
        /// <returns>The vector whose value has been parsed from th string <paramref name="s"/>.</returns>
        /// <exception cref="FormatException">When the string <paramref name="s"/> does not contain a known representation
        /// of vector that could be parsed. Use <see cref="TryParse(string, out VectorRetType)"/> to avoid throwing exception
        /// and to provide success information in the returned value instead.</exception>
        public static VectorRetType Parse(string s)
        {
            VectorRetType result;
            bool success = TryParse(s, out result);
            if (success)
            {
                return result;
            }
            throw new FormatException($"Cannot parse a vector from string \"{s}\".");
        }

        #endregion StringConversions


    }

}

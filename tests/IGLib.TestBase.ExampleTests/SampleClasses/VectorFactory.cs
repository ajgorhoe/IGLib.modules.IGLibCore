using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LearnCs.Lib
{

    public interface IVectorFactory
    {

        /// <summary>Creates and returns a vector of dimension <paramref name="dimension"/> while type of 
        /// the vector is not specified (it is stated as the most basic vector interface, <see cref="IVector"/>).</summary>
        /// <param name="dimension">Dimension of the created vector.</param>
        /// <remarks><para>In most cases, implementations will simply call <see cref="CreateVector(int)"/>.</para>
        /// <para>This method is needed in some situations where the obtained type cannot be specified in advance, such 
        /// as in certain usages of vector factory registries (like <see cref="VectorFactoryRegistry{BaseVectorType}"/> 
        /// or <see cref="VectorFactoryRegistry"/>). In such cases, the factory is referenced via base factory type,
        /// it can still produce vector objects, but these must be cast to the correct type.</para></remarks>
        IVector CreateVectorObject(int dimension);

    }

    /// <summary>Generic vector factory interface.</summary>
    /// <typeparam name="VectorType">Type of the vectors created by the factory.</typeparam>
    public interface IVectorFactory<VectorType>: IVectorFactory
        where VectorType : class, IVector
    {

        /// <summary>Creates and returns a vector of the type <typeparamref name="VectorType"/> of
        /// dimension <paramref name="dimension"/>.</summary>
        /// <param name="dimension">Dimention of the created vector.</param>
        VectorType CreateVector(int dimension);

    }

    /// <summary>A base class for a vector factory without having the precise type of the created vectors
    /// specified.</summary>
    /// <remarks>ToDo: decide later whether this intermediate base type is needed. 
    /// In contrary, the <see cref="IVectorFactory"/> will be kept for sure.</remarks>
    public abstract class VectorFactoryBase : IVectorFactory
    {
        public abstract IVector CreateVectorObject(int dimension);
    }

    /// <summary>Abstract base class for vector factories.</summary>
    /// <typeparam name="VectorType">Type of the vectors that vector factories create.</typeparam>
    public abstract class VectorFactoryBase<VectorType> : VectorFactoryBase, IVectorFactory<VectorType>
        where VectorType : class, IVector
    {

        public abstract VectorType CreateVector(int dimension);

        public override IVector CreateVectorObject(int dimension)
        {
            return CreateVector(dimension);
        }

        public static implicit operator VectorFactoryBase<VectorType>(Func<int, VectorType> creationDelegate)
        {
            return new VectorFactoryFromDelegate<VectorType>(creationDelegate);
        }

    }

    /// <summary>This is just an example of a concrete vector factory, which creates vector objects of type
    /// <see cref="ComplexVector"/>. We use it as example, while for practical purposes we will create factory
    /// objects via creation delegates by the <see cref="VectorFactoryFromDelegate{VectorType}"/> factory class.</summary>
    public class ComplexVectorFactory : VectorFactoryBase<ComplexVector>
    {
        public override ComplexVector CreateVector(int dimension)
        {
            return new ComplexVector(dimension); // ToDo: pticka (mnemonic for Anka)
        }
    }

    /// <summary>A generic vector factory that creates vectors of type <typeparamref name="VectorType"/>, based on a
    /// delegate that is injected via constructor. Actual creation of vector objects is delegated to this delegate.</summary>
    /// <typeparam name="VectorType"></typeparam>
    public class VectorFactoryFromDelegate<VectorType>: VectorFactoryBase<VectorType>, IVectorFactory<VectorType>
        where VectorType : class, IVector
    {

        /// <summary>Constructor that initializes the factory with the delegate that creates a vector
        /// of the type <see cref="VectorType"/>, which is then used for actual creation of vectors.</summary>
        /// <param name="creationDelegate">Delegate that will be used for creation of vectors (it takes a parameter 
        /// of type int, which is vector dimension, and returns a vector of type <see cref="VectorType"/> of this
        /// dimension, allocated with default values).</param>
        public VectorFactoryFromDelegate(Func<int, VectorType> creationDelegate) // $$ to look again about the mechanism
        {
            CreationDelegate = creationDelegate;
        }

        /// <summary>Delegate that is called to create vectors of type <see cref="VectorType"/> with the specified dimension.</summary>
        protected Func<int, VectorType> CreationDelegate { get; set; }

        /// <summary>Creates and returns a vector of type <see cref="VectorType"/> of the specified dimension (<paramref name="dim"/>)</summary>
        /// <param name="dim">Dimension of the vector to be created.</param>
        /// <returns>A newly created vector of type <typeparamref name="VectorType"/>, whose dimension is <paramref name="dim"/>
        /// and whose elemennts are initialized to default values.</returns>
        public override VectorType CreateVector(int dim)
        {
            return CreationDelegate(dim);
        }

        /// <summary>Defines implicit conversion from a delegate of type <see cref="Func{int, VectorType}"/>
        /// to the vector factory of type <see cref="VectorFactoryFromDelegate{VectorType}."/> Conversion is performed in such
        /// a way that the delegate is wrapped into the factory oblect of the mentioned type.</summary>
        /// <param name="vectorCreationDelegate">The delegate that is wrapped into the factory class and will 
        /// be called each time the <see cref="CreateVector(int)"/> is called on the instance of the class.</param>
        public static implicit operator VectorFactoryFromDelegate<VectorType>(Func<int, VectorType> vectorCreationDelegate)
        {
            return new VectorFactoryFromDelegate<VectorType>(vectorCreationDelegate);
        }

    }

}

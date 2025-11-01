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

    /// <summary>Interface for the registry of vector factories. For method descriptions, see <see cref="VectorFactoryRegistry{BaseVectorType}"/>.</summary>
    /// <typeparam name="BaseVectorType">The base type of vector type that can be registered in the registry.
    /// Only factories for this type and its derived types can be registered.</typeparam>
    public interface IVectorFactoryRegistry<BaseVectorType> where BaseVectorType : class, IVector
    {
        IVector CreateVector(Type vectorType, int dimension);
        VectorType CreateVector<VectorType>(int dimension) where VectorType : class, IVector, BaseVectorType;
        VectorType CreateVector<VectorType>(Type vectorType, int dimension) where VectorType : class, IVector, BaseVectorType;
        IVectorFactory GetFactory(Type type);
        IVectorFactory<VectorType> GetFactory<VectorType>() where VectorType : class, IVector, BaseVectorType;
        string[] GetRegisteredTypeNames();
        Type[] GetRegisteredTypes();
        void RegisterFactory<VectorType>(Func<int, VectorType> creationDelegate) where VectorType : class, IVector, BaseVectorType;
        void RegisterFactory<VectorType>(IVectorFactory<VectorType> factory) where VectorType : class, IVector, BaseVectorType;
        bool UnRegisterFactory<VectorType>() where VectorType : class, IVector, BaseVectorType;
    }

    /// <summary>Registry of vector factories where factory for any type of vector can be registered.
    /// <para>This class inherits from <see cref="VectorFactoryRegistry{IVector}"/> because every vector type implements
    /// at least the <see cref="IVector"/> interface (independently also of the type of vector elements).</para></summary>
    public class VectorFactoryRegistry: VectorFactoryRegistry<IVector>
    {

        public VectorFactoryRegistry() : base()
        {  }

    }


    /// <summary>Registry of vector factories that create vectors of types that are derived from <typeparamref name="BaseVectorType"/>.</summary>
    /// <typeparam name="BaseVectorType">Base vector type or interface of the returned vectors for the vector 
    /// factories that can be registered by the <see cref="RegisterFactory{VectorType}(IVectorFactory{VectorType})"/>. 
    /// Factories for vector types that are not derived from this type
    /// cannot be registered by the registry. For example, if this type parameter is <see cref="IVector{double}"/>,
    /// then the registry can only registers factories that create vectors whose elements are of type double.</typeparam>
    public class VectorFactoryRegistry<BaseVectorType> : IVectorFactoryRegistry<BaseVectorType>
        where BaseVectorType : class, IVector
    {

        /// <summary>Default (parameterless) constructor. Does not do anything because all initializations are done by
        /// property initializers.</summary>
        public VectorFactoryRegistry()
        { }

        /// <summary>Internal dictionary to store and access vector factories by their return type.</summary>
        protected Dictionary<Type, IVectorFactory> FactoryRegistry { get; } = new Dictionary<Type, IVectorFactory>();

        /// <summary>Object used to lock operations to make them thread safe (prevents simultaneous access from parallel threads).
        /// Such a mechanism is essential for safety when objects of the class might be used in multithreading environments.</summary>
        protected object RegistryLock { get; } = new object();

        // ToDo:
        // Define the methods to check whether a factory is registered for a specific type (also add tests):
        //   * bool HasFactory<VectorType>()
        //   * bool HasFactory(Type vectorType)

        public Type[] GetRegisteredTypes()
        {
            lock (RegistryLock)
            {
                return FactoryRegistry.Keys.ToArray();
            }
        }

        public string[] GetRegisteredTypeNames()
        {
            lock (RegistryLock)
            {
                return FactoryRegistry.Keys.Select(key => key.Name).ToArray();
            }
        }

        /// <summary>Creates and returns a vector of the specified type (<typeparamref name="VectorType"/>). In order for 
        /// the call to be successful, this exact vector type needs to be registered previously on the current registry.</summary>
        /// <typeparam name="VectorType">Precise type of the vector to be created.</typeparam>
        /// <param name="dimension">Dimension of the vector to be created.</param>
        /// <exception cref="InvalidOperationException">When the factory for creating the specified type (<typeparamref name="VectorType"/>)
        /// of vectors is not registered.</exception>
        public VectorType CreateVector<VectorType>(int dimension)
            where VectorType : class, BaseVectorType, IVector
        {
            IVectorFactory<VectorType> factory = GetFactory<VectorType>(); 
            if (factory == null)
            {
                throw new InvalidOperationException($"{GetType().Name}.{nameof(CreateVector)}: A factory for vectors of type {typeof(VectorType)} is not registered with the factory registry.");
            }
            return factory.CreateVector(dimension);
        }

        /// <summary>Creates and returns a vector with the precise specified type <paramref name="vectorType"/>, of the
        /// specified dimension (<paramref name="dimension"/>).</summary>
        /// <param name="vectorType">Defines the precise type of the vector to be created.</param>
        /// <param name="dimension">Dimension of the created vector.</param>
        /// <returns>The created vector as the basic vector interface <see cref="Ivector"/>. The returned vector can be cast to type
        /// that corresponds to <paramref name="vectorType"/>, and cast is guaranteed to succeed if the vector is not null.</returns>
        /// <exception cref="InvalidOperationException">When the factory for the specified object could not be found.</exception>
        public IVector CreateVector(Type vectorType, int dimension)
        {
            IVectorFactory factory = GetFactory(vectorType);
            if (factory == null)
            {
                throw new InvalidOperationException($"{GetType().Name}.{nameof(CreateVector)}: A factory for vectors of type {vectorType} is not registered with the factory registry.");
            }
            return factory.CreateVectorObject(dimension);
        }

        /// <summary>Creates and returns a vector of the precise specified type (defined by <paramref name="vectorType"/>,
        /// which must be assignable to the generic type parameter <typeparamref name="VectorType"/>) and of the
        /// specified dimension (<paramref name="dimension"/>). Throws exception if the vector could not be provided.</summary>
        /// <param name="vectorType">Defines the precise type of the vector to be created.</param>
        /// <param name="dimension">Dimension of the created vector.</param>
        /// <returns>The created vector cast to type <typeparamref name="VectorType"/>.</returns>
        /// <exception cref="InvalidOperationException">When the factory for the specified object could not be found, or when
        /// the specified type <paramref name="vectorType"/> is not assignable to <typeparamref name="VectorType"/>.</exception>
        public VectorType CreateVector<VectorType>(Type vectorType, int dimension)
            where VectorType : class, BaseVectorType, IVector
        {
            IVectorFactory factory = GetFactory(vectorType);
            if (factory == null)
            {
                throw new InvalidOperationException($"{GetType().Name}.{nameof(CreateVector)}: A factory for vector type {vectorType} is not registered with the factory registry.");
            }
            VectorType vector = factory.CreateVectorObject(dimension) as VectorType;
            if (vector == null)
            {
                throw new InvalidOperationException($"{GetType().Name}.{nameof(CreateVector)}: Factory could not create the vector of type specified by the generic type parameter " +
                    $"of the method, although the factory for {vectorType} is registered with the factory registry. Possible type mismatch of generic type parameter ({typeof(VectorType)}) w.r. the specified type.");
            }
            return vector;
        }

        /// <summary>Registers the specified vector factory that creates vectors of type <typeparamref name="VectorType"/>.
        /// <para>After registration, the vector factory can be retrieved by the <see cref="GetFactory{VectorType}"/> method.</para>
        /// <para>If another factory is registered later with the same type parameter, this overrides the previous registration.</para></summary>
        /// <typeparam name="VectorType"></typeparam>
        /// <param name="factory"></param>
        public void RegisterFactory<VectorType>(IVectorFactory<VectorType> factory)
            where VectorType : class, BaseVectorType, IVector
        {
            lock (RegistryLock)
            {
                // ToDo: change such that null is returned when key does not exist, instead of throwing an exception
                // in indexer of dictionary.
                FactoryRegistry[typeof(VectorType)] = factory;
            }
        }

        /// <summary>Registers a vector factory for vectors of type <typeparamref name="VectorType"/>  created
        /// from the specified vector creation delegate.</summary>
        /// <typeparam name="VectorType">Vector type for which the factory is created.</typeparam>
        /// <param name="creationDelegate">Vector creation delegate from which the vector factory to be registered
        /// with the vector type is created.</param>
        public void RegisterFactory<VectorType>(Func<int, VectorType> creationDelegate)
            where VectorType : class, BaseVectorType, IVector
        {
            RegisterFactory<VectorType>(new VectorFactoryFromDelegate<VectorType>(creationDelegate));
        }

        /// <summary>Unregisters the vector factory for the type <typeparamref name="VectorType"/>, if a factory
        /// is registered for that type. Otherwise, it has no effect.</summary>
        /// <returns>True if the registered factory is was found and successfully removed, false otherwise.</returns>
        /// <typeparam name="VectorType">Vector type for which the registered factory is removed.</typeparam>
        public bool UnRegisterFactory<VectorType>()
            where VectorType : class, BaseVectorType, IVector
        {
            lock (RegistryLock)
            {
                return FactoryRegistry.Remove(typeof(VectorType));
            }
        }


        /// <summary>Retrieves and returns a vector factory that creates vectors of type <see cref="BaseVectorType"/>,
        /// or returns null if such a factory is not registered.</summary>
        /// <typeparam name="VectorType">Specifies type of vectors that are created by the vector factory to be returned.</typeparam>
        /// <returns>A vector factory of type <see cref="IVectorFactory{VectorType}"/> that creates vectors of type 
        /// <typeparamref name="VectorType"/>, if such a factory is registered with the current registry. Otherwise, null is returned.</returns>
        public IVectorFactory<VectorType> GetFactory<VectorType>()
            where VectorType : class, BaseVectorType, IVector
        {
            Type type = typeof(VectorType);
            IVectorFactory factory = null;
            lock (RegistryLock)
            {
                if (FactoryRegistry.ContainsKey(type))
                {
                    factory = FactoryRegistry[type];
                }
            }
            IVectorFactory<VectorType> returnedFactory = factory as IVectorFactory<VectorType>;
            return returnedFactory;
        }


        /// <summary>Retrieves and returns a vector factory that creates vectors of type <paramref name="type"/>,
        /// or returns null if such a factory is not registered.</summary>
        /// <param name="type">Specifies the type of the vector for which the factory is returned. Although
        /// the return type is the general <see cref="IVectorFactory"/> interface, the factory returned (if
        /// not null) is guaranteed to be for that type, and can be cast to <see cref="IVectorFactory{VectorType}"/>
        /// by the as operator.</param>
        /// <remarks>
        /// <para>if the specified type cannot be cast to <typeparamref name="BaseVectorType"/>, then the method will
        /// return null because it is not possible to register a factor for such a type with the current registry.</remarks>
        public IVectorFactory GetFactory(Type type)
        {
            IVectorFactory factory = null;
            lock (RegistryLock)
            {
                if (FactoryRegistry.ContainsKey(type))
                {
                    factory = FactoryRegistry[type];
                }
            }
            return factory;
        }


    }


}

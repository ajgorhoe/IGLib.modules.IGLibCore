
#nullable disable

using IG.Lib;
using IGLib.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IGLib.Commands
{

    /// <summary>Base class for generic command classes that implement the <see cref="IGenericCommand"/> interface.
    /// This class can serve as base class for other command classes, but is also a concrete command class
    /// on its own where execution methods can be defined via delegates (usually lambdas) passed to constructor
    /// (<see cref="GenericCommandBase.GenericCommandBase(Func{object[], object}, Func{object[], Task{object}})"/>).
    /// <para>Definition via delegates passed to constructor:</para>
    /// <para></para>
    /// <para>Automatic sync to async and vice versa conversion:</para></summary>
    public class GenericCommandBase : IdentifiableLockableBase, IGenericCommand,
        IIdentifiable, IStringIdentifiable
    {

        /// <summary>Command constructor. Delegates that implement the <see cref="Execute(object[])"/>
        /// or <see cref="ExecuteAsync(object[])"/> methods can be passed as parameters. If only one
        /// delegate is passed (either for synchronous oe asynchronous execution), the other method
        /// is implemented by adapting its counterpart (sync to async or vice versa), which is not an
        /// ideal scenario but may work sufficiently well in many cases when there is no other option
        /// (such as using a library where only synchronous or asynchronous version is available).
        /// <para>By default, both arguments are null. In this case, at least one of the execution
        /// functions must be overridden in a derived class.</para></summary>
        /// <param name="executeDelegate">Optional delegate that will implement the synchronous 
        /// execution method (<see cref="Execute(object[])"/>). Default is null, which means that 
        /// either the method must be overridden, or it will adapt the asynchronous method to do the
        /// job (usually not ideal but should work sufficiently well for many use cases).</param>
        /// <param name="executeAsyncDelegate">Optional delegate that will implement the synchronous 
        /// execution method (<see cref="ExecuteAsync(object[])"/>). Default is null, which means that 
        /// either the method must be overridden, or it will adapt the synchronous method to do the
        /// job (usually not ideal but should work sufficiently well for many use cases).</param>
        public GenericCommandBase(Func<object[], object> executeDelegate = null,
            Func<object[], Task<object>> executeAsyncDelegate = null, string description = null,
            string descriptionUrl = null)
        {
            ExecuteDelegate = executeDelegate;
            ExecuteAsyncDelegate = executeAsyncDelegate;
            if (description != null)
            {
                Description = description;
            }
            if (descriptionUrl != null)
            {
                DescriptionUrl = descriptionUrl;
            }
        }

        /// <summary>Constructor, defines the synchronous execution method <see cref="Execute(object[])"/>
        /// via the delegate passed as parameter.
        /// <para>This constructor does not define the async execution method <see cref="ExecuteAsync(object[])"/>.
        /// It can either be overridden if this is a base constructor in a derived class, or the current
        /// definition is used that adapts the synchronous execution method (not ideal, but works sufficiently well
        /// for some use cases).</para></summary>
        /// <param name="executeDelegate">The delegate that is used to implement <see cref="Execute(object[])"/>.
        /// If null then exception is thrown.</param>
        /// <exception cref="ArgumentNullException">Thrown when the delegate parameter passed is null.</exception>
        public GenericCommandBase(Func<object[], object> executeDelegate, string description = null,
            string descriptionUrl = null) :
            this(executeDelegate: executeDelegate, executeAsyncDelegate: null, 
                description: description, descriptionUrl: descriptionUrl)
        {
            if (executeDelegate == null) throw new ArgumentNullException(nameof(executeDelegate),
                $"{this.GetType().Name} The delegate passed to implement the command's {nameof(Execute)} method is null.");
        }

        /// <summary>Constructor, defines the asynchronous execution method <see cref="ExecuteAsync(object[]))"/>
        /// via the delegate passed as parameter.
        /// <para>This constructor does not define the synchronous execution method <see cref="Execute(object[])"/>.
        /// The method can either be overridden if this is a base constructor in a derived class, or the current
        /// definition is used that adapts the asynchronous execution method (not ideal, but works sufficiently well
        /// for some use cases).</para></summary>
        /// <param name="executeAsyncDelegate">The delegate that is used to implement <see cref="ExecuteAsync(object[])"/>.
        /// If null then exception is thrown.</param>
        /// <exception cref="ArgumentNullException">Thrown when the delegate parameter passed is null.</exception>
        public GenericCommandBase(Func<object[], Task<object>> executeAsyncDelegate, string description = null,
            string descriptionUrl = null) :
            this(executeDelegate: null, executeAsyncDelegate: executeAsyncDelegate, 
                description: description, descriptionUrl: descriptionUrl)
        {
            if (executeAsyncDelegate == null) throw new ArgumentNullException(nameof(executeAsyncDelegate),
                $"{this.GetType().Name} The delegate passed to implement the command's {nameof(ExecuteAsync)} method is null.");
        }

        private string _description = null;

        /// <inheritdoc />
        public virtual string Description
        {
            get
            {
                lock(Lock)
                {
                    if (_description == null)
                    {
                        _description = $"A command of type {GetType().Name}, ID = {Id}.";
                    }
                    return _description;
                }
            }
            protected init
            {
                if (value != _description)
                {
                    if (value == "")
                    { _description = null; }
                    else
                    {
                        _description = value;
                    }
                }
            }
        }

        /// <summary>Provides m URL to external description of what the current command does and what 
        /// is its behavior. This may be used instead of <see cref="Description"/> in some scenarios.</summary>
        /// <remarks>For now, this property will not be defined in the <see cref="IGenericCommand"/> interface. 
        /// System that make use of this property therefore need to cast the interface (with as operator)
        /// to the <see cref="GenericCommandBase"/> class.</remarks>
        public virtual string DescriptionUrl { get; protected init; }

        /// <summary>When <see cref="Execute(object[])"/> is not overridden in a derived class,
        /// this delegate is used to  execute the command synchronously, when defined. When not
        /// defined and the method is not overridden, synchronous execution is performed by
        /// <see cref="ExecuteByCallingAsync(object[])"/>, which adapts the asynchronous execution
        /// method.</summary>
        protected virtual Func<object[], object> ExecuteDelegate { get; init; } = null;

        /// <summary>When <see cref="ExecuteAsync(object[])"/> is not overridden in a derived class,
        /// this delegate is used to  execute the command asynchronously, when defined. When not
        /// defined and the method is not overridden, asynchronous execution is performed by
        /// <see cref="ExecuteAsyncByCallingSync(object[])(object[])"/>, which adapts the synchronous 
        /// execution method.</summary>
        protected virtual Func<object[], Task<object>> ExecuteAsyncDelegate { get; set; } = null;

        private int _callCountSyncToAsync = 0;

        private int _callCountAsyncToSync = 0;

        /// <summary>Performs the job of <see cref="Execute(object[])"/> by calling its asynchronous
        /// counterpart, <see cref="ExecuteAsync(object[])"/>. This method can be used by the first
        /// method when its own execution algorithm is not defined, but its asynchronous counterpart
        /// is defined.</summary>
        /// <remarks></remarks>
        /// <param name="parameters">Command parameters.</param>
        /// <returns>The result of executing the command.</returns>
        protected virtual object ExecuteByCallingAsync(params object[] parameters)
        {
            // ToDo: do this correctly (implement a mechanism to prevent deadlocks)
            return ExecuteAsync(parameters).Result;
        }

        /// <summary>Performs the job of <see cref="Execute(object[])"/> by calling its asynchronous
        /// counterpart, <see cref="ExecuteAsync(object[])"/>. This method can be used by the first
        /// method when its own execution algorithm is not defined, but its asynchronous counterpart
        /// is defined.</summary>
        /// <param name="parameters">Command parameters.</param>
        /// <returns>The result of executing the command.</returns>
        protected virtual async Task<object> ExecuteAsyncByCallingSync(params object[] parameters)
        {
            // ToDo: add possibility of decision whether to offload synchronous method to thread pool,
            // or a custom thread, or don't offload it.
            return await Task.Run(() => Execute(parameters));
        }

        /// <summary>Executes the current command synchronously.
        /// <para>The command itself is responsible for interpreting and validating the parameters
        /// and whether it returns anything and of what type.</para></summary>
        /// <param name="parameters">Command parameters.</param>
        /// <returns>Result of the command, or null if the command does not produce a result.</returns>
        /// <remarks>This basic implementation first checks whether the synchronous delegate <see cref="ExecuteDelegate"/>
        /// is not null and if this is the case, it calls it and returns its result.
        /// If the delegate is not defined then the method calls <see cref="ExecuteByCallingAsync(object[])"/>,
        /// which adapts the asynchronous <see cref="ExecuteAsync(object[])"/> into synchronous method.</remarks>
        public virtual object Execute(params object[] parameters)
        {
            if (ExecuteDelegate != null)
            {
                return ExecuteDelegate(parameters);
            }
            // This method is neither custom implemented (by overriding the base class' method)
            // nor defined via delegate; try to call the ExecuteAsync method and prevent infinite recursion:
            try
            {
                // Notify the async method that this method (synchronous) is calling it to do the job:
                int countThis = Interlocked.Increment(ref _callCountSyncToAsync);
                // Check if the other method has also been calling this method to do the job. If this is
                // the case, it will inevitably result in infinite recursion, and this method can not
                // be defined via its counterpart (and vice versa):
                int countOther = Interlocked.CompareExchange(ref _callCountAsyncToSync, 0, 0);  // just read, ensure it is atomic
                if (countOther > 0)
                {
                    // Number of times the other method tried to call this one is also larger than 0, which means
                    // that both methods don't have their own definition and rely on the other method to do the job,
                    // which means infinite recursion is detected.
                    // Increment the count of these calls once more to ensure it will not get decremented back to 0,
                    // so it always signals the other method that both methods are locked in infinite cycle of calling
                    // each other:
                    Interlocked.Increment(ref _callCountSyncToAsync);
                    // Instead of calling the other method, throw exception to notify of both methods not being
                    // implemented and to avoid infinite recursion:
                    throw new NotImplementedException($"Command object of type {nameof(GenericCommandBase)}: neither {nameof(Execute)}(...) nor {nameof(ExecuteAsync)} is implemented.");
                }
                // No sign of infinite rcursion yet; Call the other method via the adapter method:
                return ExecuteByCallingAsync(parameters);
            }
            finally
            {
                // Under normal conditions, this will return call count to 0 when all calls return:
                Interlocked.Decrement(ref _callCountAsyncToSync);
            }
            throw new NotImplementedException($"{nameof(GenericCommandBase)}.{nameof(Execute)}(...) is not implemented.");
        }

        /// <summary>Executes the current command asynchronously.
        /// <para>The command itself is responsible for interpreting and validating the parameters
        /// and whether it returns anything and of what type.</para></summary>
        /// <param name="parameters">Command parameters.</param>
        /// <returns>Result of the command, or null if the command does not produce a result.</returns>
        /// <remarks>This basic implementation first checks whether the asynchronous delegate <see cref="ExecuteAsyncDelegate"/>
        /// is not null and if this is the case, it awaits its execution & returns its result.
        /// If the delegate is not defined then the method calls <see cref="ExecuteByCallingAsync(object[])"/>,
        /// which adapts the asynchronous <see cref="ExecuteAsync(object[])"/> into an synchronous method.</remarks>
        public virtual async Task<object> ExecuteAsync(params object[] parameters)
        {
            if (ExecuteAsyncDelegate != null)
            {
                return await ExecuteAsyncDelegate(parameters);
            }
            // This method is neither customly implemented (by overriding the base class' method) nor
            // defined via delegate; try to call the synchronous Execute method and prevent infinite recursion:
            try
            {
                // Notify the synchronous method that this method (asynchronous) is calling it to do the job:
                int countThis = Interlocked.Increment(ref _callCountAsyncToSync);
                // Check if the other method has also been calling this method to do the job. If this is
                // the case, it will inevitably result in infinite recursion, and this method can not
                // be defined via its counterpart (and vice versa):
                int countOther = Interlocked.CompareExchange(ref _callCountSyncToAsync, 0, 0);  // just read, ensure it is atomic
                if (countOther > 0)
                {
                    // If infinite recursion is detected, increment the count of these calls once more to ensure it
                    // will not get decremented back to 0, so it always signals the other method that calling this
                    // method will result in infinite recursion, so it does not waste resources on calling this method
                    // again.
                    Interlocked.Increment(ref _callCountAsyncToSync);
                    // Throw exception to inform that the method is not implemented:
                    throw new NotImplementedException($"Command object of type {nameof(GenericCommandBase)}: neither {nameof(Execute)}(...) nor {nameof(ExecuteAsync)} is implemented.");
                }
                // No sign of infinite rcursion yet; Call the other method via the adapter method:
                return await ExecuteAsyncByCallingSync(parameters);
            }
            finally
            {
                // Under normal conditions, this will return call count to 0 when all calls return:
                Interlocked.Decrement(ref _callCountAsyncToSync);
            }
            throw new NotImplementedException($"{nameof(GenericCommandBase)}.{nameof(Execute)}(...) is not implemented.");
        }


    }

}

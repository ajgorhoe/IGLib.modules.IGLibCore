
# CommandRunner
 
## To Do
 
### Details
  
* Maybe Remove StringId in ExecutionInfo with Command, and maybe remove stringidentifiable from IRunnableCommand.
* ExecutionInfo hierarchy: create a minimal one with only CommandRunner and Result (no command data, no detailed execution stage information, etc.).
* In CommandInvoker.UpdateCommands(ICommandUpdater<CommandInvokerType> updater): Solution would lekely be to have argument of type ICommandUpdater<CommandType>, which would act on ICommandInvoker<CommandType>!
  * We don't need anything else but CommandType, because updater only uses operations like this (with generic parameter for commands), and can rely on ICommandIncoker<CommandType>; operations are like AddCommand(string, CommandType), ReplaceCommand(string, CommandType), RemoveCommand(string), etc.

### Tests to Be Added

* Chaining of commands and runner actions (like AddCommand, removeCommand, ...)
  * e.g. runner.AddCommand(cmdname1, new CmdName1()).Execute(commandName, parameters).Runner.Execute(commandName1, parameters1).Runner.AddCommand(...)...;
* Add tests for all combinations of executioninfo/extended and safe/unsafe

### Extend Commands to Support Async

Current command interface:

~~~csharp
public interface IRunnableCommand: IIdentifiable, IStringIdentifiable
{
    object Execute(params object[] parameters);
}
~~~

We want to extend this to support async commands as well. The new intefface could look like this:

~~~csharp
public interface IRunnableCommand: IIdentifiable, IStringIdentifiable
{
    object Execute(params object[] parameters);
    Task<object> ExecuteAsync(params object[] parameters);
}
~~~

#### Concrete Command Base Class that Automatizes Missing Methods

Concrete base class will **automatize the implementation of missing methods**: if only synchronous method is implemented, the asynchronous method will be wrapped in a Task and vice versa, if only asynchronous method is implemented, the synchronous method will await for the asynchronous method to complete and return the result (in a way that prevents a deadlock).

base class will enable specification of one or another or both execute- methods by **providing the corresponding lambda expressions in the construcor**. If the appropriate lambda is provided, the corresponding method will be call the lambda. If not, it will call the other method and wrap/await the result accordingly. In the second case, the base class will have to **prevent infinite recursion** by counting the number of nested calls of each method:

* before calling the other method, it will increase its call counter (each of the two methods will have its own counter)
* if the other counter is greater than 0, it **will throw** an InvalidOperationException or NotImplementedException to prevent infinite recursion, with a meaningful message (e.g. "neither sync nor async method is implemented")
* after calling the other method, it will decrease its call counter

**Incrementing and decrementing** call counters must be done **in a thread-safe way**, e.g. by using *Interlocked.Increment/Decrement*.

##### Correctly Calling Async Method from Synchronous Code without Deadlock

When calling the asynchronous method from the synchronous method, we need to **avoid deadlocks**. This can be achieved by using `ConfigureAwait(false)` when awaiting the task, and then using `.GetAwaiter().GetResult()` to synchronously wait for the result.
 
 See these resources:

 * [How to call asynchronous method from synchronous method in C#](https://stackoverflow.com/questions/9343594/how-to-call-asynchronous-method-from-synchronous-method-in-c)
   * The answer by [Erik Philips](https://stackoverflow.com/a/25097498) seems to point to a good solution. The quoted code is from Microsoft.AspNet.Identity and is under MIT license:
    * [AsyncHelper.cs](https://github.com/aspnet/AspNetIdentity/blob/main/src/Microsoft.AspNet.Identity.Core/AsyncHelper.cs) from AspNetIdentity (Microsoft)
   * [The first answer from Staphan Cleary](https://stackoverflow.com/a/9343733) also contains some useful information; it also contains link to his helper library and his blog post: [Don't Block on Async Code](https://blog.stephencleary.com/2012/07/dont-block-on-async-code.html)
* It is **recommended to go through these about sync/async**:
  * [Asynchronous Programming Suidance](https://github.com/davidfowl/AspNetCoreDiagnosticScenarios/blob/master/AsyncGuidance.md#avoid-using-taskrun-for-long-running-work-that-blocks-the-thread) from AspNetCoreDiagnosticScenarios by David Fowler (Microsoft)


























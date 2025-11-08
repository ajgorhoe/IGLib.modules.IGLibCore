
#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IGLib.Commands;


namespace IGLib.Commands.Tests
{


	/// <summary>Base class for <see cref="IGenericCommand"/> implementations for testing (dummy commands 
	/// containing additional functionality for testing).</summary>
	public class TestCommandBase : GenericCommandBase
	{

		/// <summary>Counts total number of executions of the current command object.</summary>
		public virtual int NumExecutions { get; protected set; } = 0;

		/// <summary>Used to suppress console output where this is not needed.</summary>
		public bool NoConsoleOutput { get; set; } = false;

		/// <summary>To set number of next consecutive executions that will throw exceptions.</summary>
		public virtual int ExceptionsToThrowCount { get; set; } = 0;

		/// <summary>Acts as count of all command objects created. Do not change this definition because it is essential
		/// for test setup!</summary>
		public static int CommandObjectsCount = 0;

		/// <summary>Command action, just performs some stuff that can be used in tests.</summary>
		/// <param name="parameters"></param>
		/// <returns></returns>
		public override object Execute(params object[] parameters)
		{
			++NumExecutions;
			if (!NoConsoleOutput)
			{
				Console.WriteLine($"Command executed, ID = {StringId}, class = {GetType().Name}, Total number of executios on this object: {NumExecutions}");
			}
			if (ExceptionsToThrowCount > 0)
			{
				--ExceptionsToThrowCount;
				if (!NoConsoleOutput)
				{
					Console.WriteLine($"  Exception will be thrown by the command (Command ID = {StringId}).");
				}
				throw new InvalidOperationException("Exception was thrown as part of intended behavior.");
			}
			return null;
		}
	}

	public class TestCmd1 : TestCommandBase, IGenericCommand
	{ }

	public class TestCmd2 : TestCommandBase, IGenericCommand
	{ }

	public class Command3 : TestCommandBase, IGenericCommand
	{ }

	public class Command4 : TestCommandBase, IGenericCommand
	{ }

	public class Command5 : TestCommandBase, IGenericCommand
	{ }

	/// <summary>Command that returns product of its arguments.</summary>
	public class TestCmdProd : TestCommandBase, IGenericCommand
	{
		public override object Execute(object[] parameters)
		{
			base.Execute();
			double ret = 1;
			if (parameters != null)
			{
				foreach (object parameter in parameters)
				{
					double numericalParameter = (double)parameter;
					ret *= numericalParameter;
				}
			}
			return ret;
		}
	}

	public class CommandUpdaterBaseLevel<TExecutionInfo> : ICommandUpdater<IGenericCommand, TExecutionInfo>
		where TExecutionInfo: IExecutionInfo<IGenericCommand, TExecutionInfo>
    {
		public virtual void UpdateCommands(ICommandRunner<IGenericCommand, TExecutionInfo> commandInvoker)
		{
			// Do not change this because it is essential for test setup!
			commandInvoker.AddOrReplaceCommand(TestConst.Cmd1, new TestCmd1());
			commandInvoker.AddOrReplaceCommand(TestConst.Cmd2, new TestCmd2());
		}
	}

	public class CommandUpdaterHigherLevel<TExecutionInfo> : CommandUpdaterBaseLevel<TExecutionInfo>, 
		ICommandUpdater<IGenericCommand, TExecutionInfo>
		where TExecutionInfo: IExecutionInfo<IGenericCommand, TExecutionInfo>
    {
		public override void UpdateCommands(ICommandRunner<IGenericCommand, TExecutionInfo> commandInvoker)
		{
			// This is a standard trick when commands are defined at multiple levels (across several projects
			// at different levels of dependency hierarchy):
			base.UpdateCommands(commandInvoker);
			// Do not change this because it is essential for test setup!
			commandInvoker.AddOrReplaceCommand(TestConst.Cmd3, new Command3());
			commandInvoker.AddOrReplaceCommand(TestConst.Cmd4, new Command4());
		}
	}

	public static class TestConst
	{
		public const string Cmd1 = "Command1";
		public const string Cmd1a = "Command1a";
		public const string Cmd2 = "Command2";
		public const string Cmd2a = "Command2a";
		public const string Cmd3 = "Command3";
		public const string Cmd4 = "Command4";
		public const string Cmd5 = "Command5";
		public const string CmdSum = "Sum";
	}


}


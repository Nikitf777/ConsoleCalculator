using System.Text;

namespace ConsoleCalculator;

public interface IOperation
{
	public bool TryCalculateResult(double firstNumber, double secondNumber, out double result);
}

public class Calculator
{
	private readonly Dictionary<string, IOperation> operations = [];
	private string operationNamesStringCache = string.Empty;

	public Calculator() { }

	public Calculator(ReadOnlySpan<(string name, IOperation operation)> initialOperations)
	{
		foreach (var operation in initialOperations) {
			this.AddOperation(operation.name, operation.operation);
		}
	}

	public void AddOperation(string name, IOperation operation)
	{
		if (!this.operations.TryAdd(name, operation)) {
			Console.Error.WriteLine($"Error: An operation with name {operation} is already added");
			return;
		}
		this.UpdateOperationStringCache();
	}

	public void RemoveOperation(string name)
	{
		if (!this.operations.Remove(name)) {
			Console.Error.WriteLine($"Error: Operation {name} is not found.");
			return;
		}
		this.UpdateOperationStringCache();
	}

	private void UpdateOperationStringCache()
	{
		var builder = new StringBuilder().Append("Enter the desired operation:\n");
		foreach (var operation in this.operations) {
			_ = builder.Append("| ").Append(operation.Key).Append(" |\n");
		}
		this.operationNamesStringCache = builder.ToString();
	}

	public void Run()
	{
		Console.WriteLine("Welcome to ConsoleCalculator. Press ^C (Ctrl + C) to exit.");

		while (true) {
			double firstNumber;
			do {
				Console.Write("\nEnter the first number: ");
			} while (!TryGetNumber(out firstNumber));

			Console.WriteLine("Enter the desired operation:");
			IOperation? enteredOperation;
			do {
				Console.Write(this.operationNamesStringCache);
			} while ((enteredOperation = this.TryGetOperation()) is null);

			double secondNumber;
			do {
				Console.Write("Enter the second number: ");
			}
			while (!TryGetNumber(out secondNumber));

			if (enteredOperation.TryCalculateResult(firstNumber, secondNumber, out var result)) {
				PrintResult(result);
			}


			var gotValidKey = false;
			while (!gotValidKey) {
				gotValidKey = true;

				Console.WriteLine("\nExit\t| Esc | or | Q |\nRepeat\t| R |");

				var key = Console.ReadKey().Key;
				Console.WriteLine();
				if (key is ConsoleKey.Escape) {
					Console.Write("0"); // Need to write a character to prevent the first letter of "Exiting..." to be hidden.
				}
				if (key is ConsoleKey.Escape or ConsoleKey.Q) {
					Console.WriteLine("Exiting...");
					return;
				} else if (key is ConsoleKey.R) {
					Console.WriteLine("Repeating...");
				} else {
					gotValidKey = false;
				}
			}
		}
	}

	private static bool TryGetNumber(out double number)
	{
		var line = Console.ReadLine();

		if (line is null) {
			PrintNoMoreLinesError();
			number = double.NaN;
			return false;
		}
		if (line == string.Empty) {
			PrintEmptyLineError();
			number = double.NaN;
			return false;
		}

		var result = double.TryParse(line, out number);
		if (!result) {
			PrintParseError(line);
		}

		return result;
	}

	private IOperation? TryGetOperation()
	{
		var operationName = Console.ReadLine();
		if (operationName is null) {
			PrintNoMoreLinesError();
			return null;
		}

		if (operationName == string.Empty) {
			PrintEmptyLineError();
			return null;
		}

		if (!this.operations.TryGetValue(operationName, out var enteredOperation)) {
			Console.Error.WriteLine($"Error: Operation {operationName} doesn't exist.");
			return null;
		}

		return enteredOperation;
	}

	private static void PrintNoMoreLinesError()
	{
		Console.Error.WriteLine("Error: No more lines are available.");
	}

	private static void PrintEmptyLineError()
	{
		Console.Error.WriteLine($"Error: The line is empty.");
	}

	private static void PrintParseError(string line)
	{
		Console.Error.WriteLine($"Error: Cannot parse the line {line} to a System.Double");
	}

	private static void PrintResult(double result)
	{
		Console.WriteLine($"Result: {result}");
	}
}

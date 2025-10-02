using ConsoleCalculator;

var calculator = new Calculator([
	("+", new OperationAdd()),
	("-", new OperationSubtract()),
	("*", new OperationMultiply()),
	("/", new OperationDivide())
]);

calculator.Run();

internal sealed class OperationAdd : IOperation
{
	public bool TryCalculateResult(double firstNumber, double secondNumber, out double result)
	{
		result = firstNumber + secondNumber;
		return true;
	}
}

internal sealed class OperationSubtract : IOperation
{
	public bool TryCalculateResult(double firstNumber, double secondNumber, out double result)
	{
		result = firstNumber - secondNumber;
		return true;
	}
}

internal sealed class OperationMultiply : IOperation
{
	public bool TryCalculateResult(double firstNumber, double secondNumber, out double result)
	{
		result = firstNumber * secondNumber;
		return true;
	}
}

internal sealed class OperationDivide : IOperation
{
	public bool TryCalculateResult(double firstNumber, double secondNumber, out double result)
	{
		result = firstNumber / secondNumber;
		if (secondNumber == 0.0) {
			Console.Error.WriteLine("Error: Division by zero.");
			return false;
		}
		return true;
	}
}

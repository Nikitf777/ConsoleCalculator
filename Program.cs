try {

	Console.WriteLine("Welcome to ConsoleCalculator. Press ^C (Ctrl+C) to exit.");

	while (true) {
		Console.Write("Enter the first number:");
		var line = Console.ReadLine();

		if (double.TryParse(line, out var firstNumber)) {

			Console.Write("Press the desired operation key | + | - | * | / |: ");
			var key = Console.ReadKey();

			Console.Write("\nEnter the second number:");
			line = Console.ReadLine();
			if (double.TryParse(line, out var secondNumber)) {
				switch (key.Key) {
					case ConsoleKey.Add: {
						PrintResult(firstNumber + secondNumber);
						continue;
					}
					case ConsoleKey.Subtract: {
						PrintResult(firstNumber - secondNumber);
						continue;
					}
					case ConsoleKey.Multiply: {
						PrintResult(firstNumber * secondNumber);
						continue;
					}
					case ConsoleKey.Divide: {
						if (secondNumber == 0.0) {
							Console.Error.WriteLine("Error: Division by zero. Skipping...");
							continue;
						}
						PrintResult(firstNumber / secondNumber);
						continue;
					}
					default: { continue; }
				}
			} else {
				ParseError();
				continue;
			}
		} else {
			ParseError();
			continue;
		}
	}
} catch (Exception e) {
	Console.Error.Write(e);
	return 1;
}

static void ParseError()
{
	Console.Error.WriteLine("Error: Cannot parse the line to a System.Double. Skipping...");
}

static void PrintResult(double result)
{
	Console.WriteLine($"Result: {result}");
}

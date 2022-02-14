ITest[] tests = { new Circle.Test() };
foreach(ITest test in tests) {
    Console.WriteLine(test.Name);
    test.Run();
}
ITest[] tests = { new NTree.Test(), new NodeMap.Test(), new Circle.Test() };
foreach(ITest test in tests) {
    Console.WriteLine(test.Name);
    test.Run();
}
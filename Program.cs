ITest[] tests = { new NodeMap.Test(), new NTree.Test() };
foreach(ITest test in tests) {
    Console.WriteLine(test.Name);
    test.Run();
}
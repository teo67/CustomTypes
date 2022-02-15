interface ITest {
    /// <summary>
    /// The name of a test case.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// A method to run a test for a given datatype.
    /// </summary>
    void Run();
}
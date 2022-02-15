namespace Circle {
    class Test : ITest {
        public string Name {
            get {
                return "Circle [add, add savepoint, add * 3, shift clockwise by 2, print root, shift to savepoint, print root, print metadata]";
            }
        }
        public void Run() {
            Circle<int> myCircle = new Circle<int>(true);
            myCircle.Add(1);
            myCircle.Save("original");
            myCircle.Add(2);
            myCircle.Add(3);
            myCircle.Add(4);
            myCircle.Shift(2);
            Console.WriteLine(myCircle.Root);
            myCircle.Shift("original");
            Console.WriteLine(myCircle.Root);
            Console.WriteLine(myCircle.Metadata);
        }
    }
}
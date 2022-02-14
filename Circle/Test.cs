namespace Circle {
    class Test : ITest {
        public string Name {
            get {
                return "Circle";
            }
        }
        public void Run() {
            Circle<int> myCircle = new Circle<int>();
            myCircle.Add(1);
            myCircle.Add(2);
            myCircle.Add(3);
            myCircle.Add(4);
            Console.WriteLine(myCircle.Root);
            myCircle.Shift(2);
            Console.WriteLine(myCircle.Root);
            myCircle.Shift(-1);
            Console.WriteLine(myCircle.Root);
        }
    }
}
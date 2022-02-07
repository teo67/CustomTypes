namespace NodeMap {
    class Test : ITest {
        public string Name {
            get {
                return "NodeMap [push * 2, pop, push, add * 2, remove, insert, delete, print, deepprint]";
            }
        }
        public void Run() {
            NodeMap<int> myMap = new NodeMap<int>();
            myMap.Push(0);
            myMap.Push(1);
            Console.WriteLine(myMap.Pop().Val);
            myMap.Push(2);
            myMap.Add(0, 2);
            myMap.Add(1, 3);
            myMap.Remove(1);
            myMap.Insert(0, 0, 4);
            Console.WriteLine(myMap.Delete(0, 0));
            Console.WriteLine(myMap);
            Console.WriteLine(myMap.DeepPrint());
        }
    }
}
namespace NTree {
    class Test : ITest {
        public string Name {
            get {
                return "NTree [BinaryTree, add * 4, print, invert, print]";
            }
        }
        public void Run() {
            BinaryTree myTree = new BinaryTree(); // binary tree is an implementation of ntree (see file)
            myTree.Add(0);
            myTree.Add(1);
            myTree.Add(-1);
            myTree.Add(-2);
            Console.WriteLine(myTree);
            myTree.Invert();
            Console.WriteLine(myTree);
        }
    }   
}
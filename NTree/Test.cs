namespace NTree {
    class Test : ITest {
        public string Name {
            get {
                return "NTree [add vals, invert, print, print size]";
            }
        }
        public void Run() {
            NTree<int> myTree = new NTree<int>(5, (int viewing, int adding) => {
                if(adding - viewing < -1) {
                    return 0;
                }
                if(adding - viewing == -1) {
                    return 1;
                }
                if(adding == viewing) {
                    return 2;
                }
                if(adding - viewing == 1) {
                    return 3;
                }
                return 4;
            });

            myTree.Add(4);
            myTree.Add(2);
            myTree.Add(3);
            myTree.Add(4);
            myTree.Add(5);
            myTree.Add(6);
            myTree.Add(1);
            myTree.Invert();

            Console.WriteLine(myTree);
            Console.WriteLine(myTree.Size);
        }
    }   
}
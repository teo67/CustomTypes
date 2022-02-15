namespace NTree {
    class NTree<T> {
        /// <summary>
        /// Stores the upper element (bottom of the tree) of an NTree.
        /// </summary>
        public Node<T>? Head { get; private set; }

        private int n;
        private Func<T, T, int> comparator = (T viewing, T adding) => 0;

        /// <summary>
        /// Instantiates a new NTree with a set number of branches per Node and an optional head element.
        /// NTrees are similar to binary trees in that each Node can store two elements below it using some form of a comparator.
        /// But instead of two, an NTree has n branches connected to each Node. By this logic, the given comparator must be able to return multiple integers.
        /// </summary>
        /// <param name="n">The number of branches to be stored per element of the tree.</param>
        /// <param name="comparator">A function that returns an integer from 0 to n - 1 given the value of the branch being viewed and the target value: for example, a typical binary tree comparator would return 0 for adding less than viewing and 1 for adding more than viewing.</param>
        /// <param name="head">An optional head Node to be added as the NTree is instantiated.</param>
        public NTree(int n, Func<T, T, int> comparator, Node<T>? head = null) {
            if(n < 0) {
                throw new Exception("N parameter must be 0 or greater.");
            }
            this.n = n;
            this.comparator = comparator;
            this.Head = head;
        }

        /// <summary>
        /// Gets the total number of nodes stored in an NTree.
        /// </summary>
        public int Size {
            get {
                return IterateThrough<int>(
                    this.Head, 
                    0, 
                    (Node<T>? node) => (node == null) ? 0 : 1, 
                    (int current, int adding) => current + adding
                );
            }
        }

        private Node<T> NewNode(T val) {
            return new Node<T>(val, n);
        }

        private Node<T>? SearchThrough(T val, Func<Node<T>, int, bool> checker, Action<Node<T>, int>? doer = null) {
            Node<T>? viewing = this.Head;
            while(viewing != null) {
                int next = comparator(viewing.Val, val);
                if(next >= n) {
                    throw new Exception($"Comparator function returned a value not in between 0 and {n} (n being exclusive).");
                }
                if(checker(viewing, next)) {
                    if(doer != null) {
                        doer(viewing, next);
                    }
                    return viewing;
                }
                viewing = viewing.cxns[next];
            }
            return null;
        }

        /// <summary>
        /// Pushes a new element to an NTree, using the logic of the comparator provided earlier.
        /// </summary>
        /// <param name="val">The value to be stored in the added Node.</param>
        /// <returns>The Node element being added.</returns>
        public Node<T> Add(T val) {
            if(this.Head == null) {
                this.Head = NewNode(val);
                return this.Head;
            }
            Node<T> newNode = NewNode(val);
            Node<T>? returning = SearchThrough(val, (viewing, next) => viewing.cxns[next] == null, (viewing, next) => {
                viewing.cxns[next] = newNode;
            });
            if(returning == null) {
                throw new Exception($"Your NTree is recursive or infinite.");
            }
            return newNode;
        }

        /// <summary>
        /// Gets an element in an NTree, using the logic of its comparator.
        /// </summary>
        /// <param name="val">The value to be searched for in the NTree.</param>
        public Node<T>? Get(T val) {
            if(val == null) {
                return null; // go away intellisense
            }
            return SearchThrough(val, (viewing, next) => val.Equals(viewing.Val));
        }

        private U IterateThrough<U>(Node<T>? branch, U def, Func<Node<T>?, U> withEach, Func<U, U, U> combine, Func<U, U>? changeDef = null) {
            U returning = combine(def, withEach(branch));
            if(branch != null) {
                for(int i = 0; i < branch.cxns.Length; i++) {
                    returning = combine(returning, IterateThrough<U>(
                        branch.cxns[i], 
                        (changeDef == null) ? def : changeDef(def), 
                        withEach, 
                        combine, 
                        changeDef
                    ));
                }
            }
            return returning;
        }

        private void Invert(Node<T>? branch) {
            if(branch == null) {
                return;
            }
            for(int i = 0; i < Math.Floor(n / 2.0); i++) {
                Node<T>? saved = branch.cxns[i];
                branch.cxns[i] = branch.cxns[n - i - 1];
                branch.cxns[n - i - 1] = saved;
                Invert(branch.cxns[i]);
                Invert(branch.cxns[n - i - 1]);
            }
        }

        /// <summary>
        /// Inverts an NTree by flipping its connections left to right. Don't ask me why this is included, everyone has to invert a tree at some point in their life.
        /// </summary>
        public void Invert() {
            Invert(this.Head);
        }

        /// <summary>
        /// Prints an NTree by recursively reading each of its nodes. Not recommended for low time complexity algorithms.
        /// </summary>
        public override string ToString() {
            return IterateThrough<string>(
                this.Head, 
                "\n", 
                (Node<T>? node) => {
                    if(node == null || node.Val == null) {
                        return "null";
                    }
                    string? str = node.Val.ToString();
                    if(str == null) {
                        return "null";
                    }
                    return str;
                },
                (string current, string adding) => current + adding, 
                (string def) => def + "--> "
            );
        }
    }
}
namespace NTree {
    class NTree<T> {
        public Node<T>? Head { get; private set; }
        private int n;
        private Func<T, T, int> comparator = (T viewing, T adding) => 0;
        public NTree(int n, Func<T, T, int> comparator, Node<T>? head = null) {
            if(n < 0) {
                throw new Exception("N parameter must be 0 or greater.");
            }
            this.n = n;
            this.comparator = comparator;
            this.Head = head;
        }

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

        public void Invert() {
            Invert(this.Head);
        }

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
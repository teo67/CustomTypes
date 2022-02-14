namespace Circle {
    class Circle<T> {
        public Node<T>? Root { get; private set; }
        public Metadata<T>? Metadata { get; private set; }
        public Circle(bool withData = false, Node<T>? root = null) {
            this.Root = root;
            if(withData) {
                this.Metadata = new Metadata<T>();
            }
        }

        public int Size {
            get {
                return IterateThrough<int>(0, (Node<T> viewing) => 1, (int a, int b) => a + b);
            }
        }

        public Node<T> Add(T val) { // adds clockwise to root
            if(Root == null) {
                Root = new Node<T>(val);
                return Root;
            }
            Node<T> saved = Root.clockwise;
            Root.clockwise = new Node<T>(val, Root, saved);
            saved.counterclockwise = Root.clockwise;
            return Root.clockwise;
        }
        
        public Node<T> Pop() { // pops root, shifts clockwise
            if(Root == null) {
                throw new Exception("Nothing to pop!");
            }
            Node<T> saved = Root;
            if(Root.clockwise == Root) {
                this.Root = null;
                return saved;
            }
            Root.counterclockwise.clockwise = Root.clockwise;
            Root.clockwise.counterclockwise = Root.counterclockwise;
            this.Root = Root.clockwise;
            return saved;
        }

        private U IterateThrough<U>(U starting, Func<Node<T>, U> withEach, Func<U, U, U> combine) {
            U returning = starting;
            if(this.Root == null) {
                return returning;
            }
            Node<T> viewing = this.Root;
            do {
                returning = combine(returning, withEach(viewing));
                viewing = viewing.clockwise;
            } while(viewing != Root);
            return returning;
        }

        public override string ToString() {
            return IterateThrough<string>("^ > >   ", (Node<T> viewing) => $"{viewing.Val}\n^       ", (string a, string b) => a + b).Trim() + " < <   END";
        }

        public string DeepPrint() {
            return IterateThrough<string>("", (Node<T> viewing) => $"{viewing.counterclockwise.Val} -> {viewing.Val} -> {viewing.clockwise.Val}\n", (string a, string b) => a + b);
        }

        public Node<T>? Find(T val) { // searches at the same time in both directions from the root until the lines intersect or reach the root again
            if(val == null || Root == null) {
                return null;
            }
            Node<T> counterViewing = Root;
            Node<T> clockViewing = Root;
            do {
                foreach(Node<T> each in new Node<T>[] { counterViewing, clockViewing } ) {
                    if(val.Equals(each.Val)) {
                        return each;
                    }
                }
                if(clockViewing.clockwise == counterViewing || counterViewing.counterclockwise == clockViewing || (clockViewing == counterViewing && clockViewing != Root)) {
                    return null;
                }
                counterViewing = counterViewing.counterclockwise;
                clockViewing = clockViewing.clockwise;
            } while(counterViewing != Root && clockViewing != Root);
            return null;
        }

        public Node<T> Shift(int nodes) {
            this.Root = Get(nodes);
            return Root;
        }

        public Node<T> ShiftToSavepoint(string name) {
            if(this.Metadata == null) {
                throw new Exception("No metadata for this circle!");
            }
            Node<T>? returned = Metadata.Get(name);
            if(returned == null) {
                throw new Exception("Savepoint not found!");
            }
            this.Root = returned;
            return Root;
        }

        public Node<T> Get(int nodes) {
            if(Root == null) {
                throw new Exception("Cannot get from an empty circle!");
            }
            int count = 0;
            Node<T> viewing = Root;
            while(count < Math.Abs(nodes)) {
                count++;
                viewing = (nodes > 0) ? viewing.clockwise : viewing.counterclockwise;
            }
            return viewing;
        }

        public void SaveRoot(string name) {
            if(Metadata == null) {
                throw new Exception("Enable savepoints first using ResetSavepoints()!");
            }
            if(Root == null) {
                throw new Exception("No root to save!");
            }
            this.Metadata.Set(name, Root);
        }

        public void ResetSavepoints() { // can be used to enable data saving as well
            this.Metadata = new Metadata<T>();
        }
    }
}
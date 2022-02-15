namespace Circle {
    class Circle<T> {
        /// <summary>
        /// Stores the head element of a Circle. This can be shifted throughout a Circle's lifespan.
        /// </summary>
        public Node<T>? Root { get; private set; }

        /// <summary>
        /// Stores the (optional) metadata for a Circle. A Metadata object contains a keyed list that can be used to quickly shift to a savepoint on a Circle.
        /// </summary>
        public Metadata<T>? Metadata { get; private set; }

        /// <summary>
        /// Instantiates a new Circle object with optional metadata and an optional starter element. 
        /// Each Node in a Circle stored two connections: one in the counterclockwise direction, and one in the clockwise direction.
        /// This makes it possible to search through a Circle from a given point by iterating through elements in both directions at once, useful when paired with the Metadata system.
        /// </summary>
        /// <param name="withData">Set to true if you wish to keep a companion Metadata object for this Circle (defaults to false).</param>
        /// <param name="root">An optional Node element to be added as the root of the Circle.</param>
        public Circle(bool withData = false, Node<T>? root = null) {
            this.Root = root;
            if(withData) {
                this.Metadata = new Metadata<T>();
            }
        }

        /// <summary>
        /// Gets the total number of Nodes in a Circle - in other words, the circumference.
        /// </summary>
        public int Size {
            get {
                return IterateThrough<int>(0, (Node<T> viewing) => 1, (int a, int b) => a + b);
            }
        }

        /// <summary>
        /// Adds a new Node element so that it's clockwise relative to the current root.
        /// </summary>
        /// <param name="val">The value of the element to be added.</param>
        /// <returns>The element being added to the Circle.</returns>
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
        
        /// <summary>
        /// Pops the current root of a Circle and sets the new root to its clockwise connection, as long as a root exists.
        /// </summary>
        /// <returns>The popped element of the Circle.</returns>
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

        /// <summary>
        /// Prints a somewhat readable version of a Circle, indicating its direction and its root position.
        /// </summary>
        public override string ToString() {
            return IterateThrough<string>("^ > >   ", (Node<T> viewing) => $"{viewing.Val}\n^       ", (string a, string b) => a + b).Trim() + " < <   END";
        }

        /// <summary>
        /// Prints every Node in a Circle, as well as every connection between elements (to be used for debugging).
        /// </summary>
        public string DeepPrint() {
            return IterateThrough<string>("", (Node<T> viewing) => $"{viewing.counterclockwise.Val} -> {viewing.Val} -> {viewing.clockwise.Val}\n", (string a, string b) => a + b);
        }

        /// <summary>
        /// Gets an element in a Circle based on its value, starting at the root. This algorithm searches concurrently in both directions.
        /// </summary>
        /// <param name="val">The value of the Node being searched for.</param>
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

        /// <summary>
        /// Shifts the root over a certain number of nodes, moving counterclockwise if that number is negative.
        /// </summary>
        /// <param name="nodes">The number of nodes to shift by.</param>
        /// <returns>The new root of the Circle after shifting.</returns>
        public Node<T> Shift(int nodes) {
            this.Root = Get(nodes);
            return Root;
        }

        /// <summary>
        /// Shifts the root to a specific savepoint stored in the Circle's Metadata object, given that there is one.
        /// </summary>
        /// <param name="name">The name of the key stored in the Circle's data.</param>
        /// <returns>The new root of the Circle after shifting.</returns>
        public Node<T> Shift(string name) {
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

        /// <summary>
        /// Gets a Node on a Circle based on distance from the root. Supply a negative integer to move counterclockwise instead of clockwise.
        /// </summary>
        /// <param name="nodes">The number of nodes past the root for this method to find.</param>
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

        /// <summary>
        /// Saves the current root of a Circle to its Metadata object, given that both of those things exist.
        /// </summary>
        /// <param name="name">The name of the new Entry object in the Circle's data.</param>
        public void Save(string name) {
            if(Metadata == null) {
                throw new Exception("Enable savepoints first using ResetSavepoints()!");
            }
            if(Root == null) {
                throw new Exception("No root to save!");
            }
            this.Metadata.Set(name, Root);
        }

        /// <summary>
        /// Clears the metadata for a Circle. Note that this method can also be used to enable metadata if it had been previously nullified.
        /// </summary>
        public void ResetSavepoints() { // can be used to enable data saving as well
            this.Metadata = new Metadata<T>();
        }
    }
}
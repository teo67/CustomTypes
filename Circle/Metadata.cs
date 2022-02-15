namespace Circle {
    class Entry<T> {
        /// <summary>
        /// Stores the name of the entry, which can be searched for by its Metadata container.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Stores the value of the entry, which can be used by its outer Circle dataset.
        /// </summary>
        public Node<T> Value { get; }

        /// <summary>
        /// Instantiates a new Entry as part of a Metadata object, given a name and a value.
        /// </summary>
        /// <param name="name">The name to be assigned to the new entry.</param>
        /// <param name="value">The value to be keyed to the entry's name.</param>
        public Entry(string name, Node<T> value) {
            this.Name = name;
            this.Value = value;
        }

        /// <summary>
        /// Prints out an entry by name and value.
        /// </summary>
        public override string ToString() {
            return $"{Name}: {Value}";
        }
    }
    class Metadata<T> {
        private List<Entry<T>> Entries { get; }

        /// <summary>
        /// Instantiates a new Metadata object as part of a Circle.
        /// Metadata stores a mutable list of Entry objects that contain simple keys and values. This class is intended for use with the Circle datatype.
        /// </summary>
        public Metadata() {
            this.Entries = new List<Entry<T>>();
        }

        /// <summary>
        /// Adds a new entry to a Metadata's list, given a name and a value.
        /// </summary>
        /// <param name="name">The name to be assigned to the new entry.</param>
        /// <param name="value">The value being stored in the entry.</param>
        public void Set(string name, Node<T> value) {
            this.Entries.Add(new Entry<T>(name, value));
        }

        /// <summary>
        /// Gets an entry from a Metadata object, given its name.
        /// </summary>
        /// <param name="name">The name of the entry being searched for.</param>
        public Node<T>? Get(string name) {
            foreach(Entry<T> entry in Entries) {
                if(entry.Name == name) {
                    return entry.Value;
                }
            }
            return null;
        }

        /// <summary>
        /// Prints out a list of entries being stored in a Metadata object.
        /// </summary>
        public override string ToString() {
            string returning = "";
            foreach(Entry<T> entry in Entries) {
                returning += $"{entry}\n";
            }
            return returning;
        }
    }
}
namespace Circle {
    class Entry<T> {
        public string Name { get; }
        public Node<T> Value { get; }
        public Entry(string name, Node<T> value) {
            this.Name = name;
            this.Value = value;
        }
        public override string ToString() {
            return $"{Name}: {Value}";
        }
    }
    class Metadata<T> {
        private List<Entry<T>> Entries { get; }
        public Metadata() {
            this.Entries = new List<Entry<T>>();
        }
        public void Set(string name, Node<T> value) {
            this.Entries.Add(new Entry<T>(name, value));
        }
        public Node<T>? Get(string name) {
            foreach(Entry<T> entry in Entries) {
                if(entry.Name == name) {
                    return entry.Value;
                }
            }
            return null;
        }
        public override string ToString() {
            string returning = "";
            foreach(Entry<T> entry in Entries) {
                returning += $"{entry}\n";
            }
            return returning;
        }
    }
}
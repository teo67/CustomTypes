namespace NodeMap {
    class Node<T> {
        public T Val { get; private set; }
        public Node<T>? right;
        public Node<T>? down;
        public Node(T val, Node<T>? right = null, Node<T>? down = null) {
            this.Val = val;
            this.right = right;
            this.down = down;
        }
        public override string ToString() {
            return $"Node / {Val}";
        }
    }
}

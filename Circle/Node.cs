namespace Circle {
    class Node<T> {
        public T Val { get; }
        public Node<T> counterclockwise;
        public Node<T> clockwise;
        public Node(T val, Node<T>? counterclockwise = null, Node<T>? clockwise = null) {
            this.Val = val;
            this.counterclockwise = (counterclockwise == null) ? this : counterclockwise;
            this.clockwise = (clockwise == null) ? this : clockwise;
        }
        public override string ToString() {
            return $"Node / {Val}";
        }
    }
}
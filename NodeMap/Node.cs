namespace NodeMap {
    class Node<T> where T : struct {
        public T val;
        public Node<T>? right;
        public Node<T>? down;
        public Node(T val, Node<T>? right = null, Node<T>? down = null) {
            this.val = val;
            this.right = right;
            this.down = down;
        }
    }
}

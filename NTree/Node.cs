namespace NTree {
    class Node<T> {
        public T Val { get; private set; }
        public Node<T>[] cxns;
        public Node(T val, int cxnlength) {
            this.Val = val;
            this.cxns = new Node<T>[cxnlength];
        }
        public override string ToString() {
            return $"Node / {Val}";
        }
    }
}
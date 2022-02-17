namespace NTree { // sample binary tree class using ntree
    class BinaryTree : NTree<int> {
        public BinaryTree(Node<int>? head = null): base(2, (int viewing, int adding) => {
            return (adding < viewing) ? 0 : 1;
        }, head) {}
    }
}
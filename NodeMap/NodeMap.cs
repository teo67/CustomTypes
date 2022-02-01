namespace NodeMap {
    class NodeMap<T> where T : struct {
        public Node<T>? Head { get; private set; }
        public NodeMap(Node<T>? head = null) {
            this.Head = head;
        }

        public int Height {
            get {
                int currentRow = 0;
                Node<T>? viewing = this.Head;
                while(viewing != null) {
                    currentRow++;
                    viewing = viewing.down;
                }
                return currentRow;
            }
        }

        public int Size {
            get {
                int returning = 0;
                Node<T>? leftmost = this.Head;
                while(leftmost != null) {
                    Node<T>? viewing = leftmost;
                    while(viewing != null) {
                        returning++;
                        viewing = viewing.right;
                    }
                    leftmost = leftmost.down;
                }
                return returning;
            }
        }

        public Node<T> Push(T val) {
            Node<T>? bottom = this.GetBottom();
            if(bottom == null) {
                this.Head = new Node<T>(val);
                return this.Head;
            }
            bottom.down = new Node<T>(val);
            return bottom.down;
        }

        public Node<T> Pop() {
            Node<T>? above = this.Head;
            if(above == null) {
                throw new Exception("Cannot pop from an empty NodeMap!");
            }
            if(above.down == null) {
                Node<T> savedHead = above;
                this.Head = null;
                return savedHead;
            }
            while(above.down != null && above.down.down != null) {
                above = above.down;
            }
            if(above.down == null) {
                throw new Exception("Pop function failed unexpectedly.");
            }
            Node<T> saved = above.down;
            while(above != null && above.down != null) {
                above.down = null;
                above = above.right;
            }
            return saved;
        }

        public Node<T> Add(int row, T val) {
            Node<T> left;
            Node<T> adding = new Node<T>(val);
            if(row != 0) {
                left = this.GetFirst(row - 1);
                if(left.down == null) {
                    throw new Exception("Tried to add to a row out of bounds!");
                }
                while(left.right != null && left.down != null && left.down.right != null) {
                    left = left.right;
                }
                if(left.down == null) {
                    throw new Exception("Add function failed unexpectedly."); // this should never happen, go away intellisense
                }
                if(left.right != null && left.down.right == null) {
                    left.right.down = adding; // set down property of above
                    left = left.down;
                } else {
                    left = left.down;
                    while(left.right != null) {
                        left = left.right;
                    }
                }
            } else {
                left = this.GetLast(0);
            }

            left.right = adding; // set right property of left

            if(left.down != null && left.down.right != null) {
                left.right.down = left.down.right; // set down property of new element
            }

            return left.right;
        }

        public Node<T> Remove(int row) {
            Node<T>? left;
            if(row != 0) {
                left = this.GetFirst(row - 1);
                if(left == null || left.down == null) {
                    throw new Exception("Tried to access a row out of bounds.");
                }
                if(left.down.right == null) {
                    throw new Exception("Unable to remove such that a row becomes empty!");
                }
                while(left.right != null && left.down != null && left.down.right != null && left.down.right.right != null) {
                    left = left.right;
                }
                if(left.down == null || left.down.right == null) {
                    throw new Exception("Remove function failed unexpectedly.");
                }
                if(left.right != null && left.down.right.right == null) {
                    left.right.down = null; // set down property of above
                    left = left.down;
                } else {
                    left = left.down;
                    while(left.right != null && left.right.right != null) {
                        left = left.right;
                    }
                }
            } else {
                left = this.Head; 
                if(left == null) {
                    throw new Exception("Tried to remove from an empty NodeMap.");
                }
                if(left.right == null) {
                    throw new Exception("Unable to remove such that the NodeMap becomes empty");
                }
                while(left.right != null && left.right.right != null) {
                    left = left.right;
                }
            }
            if(left.right == null) {
                throw new Exception("Remove function failed unexpectedly.");
            }
            Node<T> saved = left.right;
            left.right = null; // set right property of left
            return saved;
        }

        public Node<T> GetFirst(int row) {
            Node<T>? viewing = this.Head;
            int currentRow = 0;
            while(viewing != null && currentRow < row) {
                currentRow++;
                viewing = viewing.down;
            }
            if(viewing == null) {
                throw new Exception("Tried to access a row out of bounds!");
            }
            return viewing;
        }

        public Node<T> GetLast(int row) {
            Node<T> first = this.GetFirst(row);
            while(first.right != null) {
                first = first.right;
            }
            return first;
        }

        public Node<T>? GetBottom() {
            if(this.Head == null) {
                return null;
            }
            Node<T> viewing = this.Head;
            while(viewing.down != null) {
                viewing = viewing.down;
            }
            return viewing;
        }

        public Node<T> Get(int row, int col) {
            Node<T>? first = this.GetFirst(row);
            int currentCol = 0;
            while(first != null && currentCol < col) {
                currentCol++;
                first = first.right;
            }
            if(first == null) {
                throw new Exception($"Tried to access column out of bounds on row {row}!");
            }
            return first;
        }

        public override string ToString() {
            string returning = "";
            Node<T>? leftmost = this.Head;
            while(leftmost != null) {
                Node<T>? viewing = leftmost;
                while(viewing != null) {
                    returning += $"{viewing.val} ";
                    viewing = viewing.right;
                }
                returning += "\n";
                leftmost = leftmost.down;
            }
            return returning;
        }
    }
}
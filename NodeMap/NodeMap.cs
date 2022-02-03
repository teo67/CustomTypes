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
                return this.ThroughEach<int>(0, (Node<T> viewing) => 1, (int current, int adding) => current + adding);
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

        private void ReplaceBackwards(Node<T> next, Node<T>? setTo) {
            if(next.right != null) {
                ReplaceBackwards(next.right, next.down);
            }
            next.down = setTo;
        }

        public Node<T> Insert(int row, int col, T val) {
            Node<T> adding = new Node<T>(val);
            if(row == 0 && col == 0) {
                adding.right = this.Head;
                if(this.Head != null) {
                    adding.down = this.Head.down;
                }
                this.Head = adding;
            } else if(row == 0) {
                Node<T> left = this.Get(0, col - 1);
                adding.right = left.right;
                if(left.down != null) {
                    adding.down = left.down.right;
                }
                left.right = adding;
            } else {
                Node<T>? above;
                if(col != 0) {
                    int colCount = 0;
                    Node<T> aboveLeft = this.GetFirst(row - 1);
                    while(aboveLeft.right != null && colCount < col - 1) {
                        colCount++;
                        aboveLeft = aboveLeft.right;
                    }
                    if(aboveLeft.down == null) {
                        throw new Exception($"Your column value exceeded the length of row {row}.");
                    }
                    if(colCount == col - 1) {
                        above = aboveLeft.right;
                        aboveLeft = aboveLeft.down;
                    } else {
                        above = null;
                        aboveLeft = aboveLeft.down;
                        while(aboveLeft.right != null && colCount < col - 1) {
                            colCount++;
                            aboveLeft = aboveLeft.right;
                        }
                        if(colCount != col - 1) {
                            throw new Exception($"Your column value exceeded the length of row {row}.");
                        }
                    }
                    adding.right = aboveLeft.right; // at this point, aboveLeft is just left of adding
                    if(aboveLeft.down != null) {
                        adding.down = aboveLeft.down.right;
                    }
                    aboveLeft.right = adding;
                } else {
                    above = this.GetFirst(row - 1);
                    adding.right = above.down;
                    if(above.down != null) {
                        adding.down = above.down.down;
                    }
                }
                if(above != null) {
                    this.ReplaceBackwards(above, adding);
                }
            }
            // at this point, we've shifted over the row above as well as hooked up the left node to the new one and the new node to the node right of the left node
            // next, we need to shift the target row's down properties
            Node<T>? viewing = adding.right;
            while(viewing != null && viewing.down != null) {
                viewing.down = viewing.down.right;
                viewing = viewing.right;
            }
            return adding;
        }

        public Node<T> Delete(int row, int col) {
            Node<T> deleting;
            if(row == 0 && col == 0) {
                if(this.Head == null || this.Head.right == null) {
                    throw new Exception("Cannot delete such that a row would become empty!");
                }
                deleting = this.Head;
                this.Head = this.Head.right;
            } else if(row == 0) {
                Node<T> left = this.Get(0, col - 1);
                if(left.right == null) {
                    throw new Exception($"Item ({row}, {col}) does not exist.");
                }
                deleting = left.right;
                left.right = left.right.right;
            } else {
                Node<T>? above;
                if(col != 0) {
                    int colCount = 0;
                    Node<T> aboveLeft = this.GetFirst(row - 1);
                    while(aboveLeft.right != null && colCount < col - 1) {
                        colCount++;
                        aboveLeft = aboveLeft.right;
                    }
                    if(aboveLeft.down == null) {
                        throw new Exception($"Your column value exceeded the length of row {row}.");
                    }
                    if(colCount == col - 1) {
                        above = aboveLeft.right;
                        aboveLeft = aboveLeft.down;
                    } else {
                        above = null;
                        aboveLeft = aboveLeft.down;
                        while(aboveLeft.right != null && colCount < col - 1) {
                            colCount++;
                            aboveLeft = aboveLeft.right;
                        }
                        if(colCount != col - 1) {
                            throw new Exception($"Your column value exceeded the length of row {row}.");
                        }
                    }
                    if(aboveLeft.right == null) {
                        throw new Exception($"Item ({row}, {col}) does not exist.");
                    }
                    deleting = aboveLeft.right;
                    aboveLeft.right = aboveLeft.right.right;
                } else {
                    above = this.GetFirst(row - 1);
                    if(above.down == null) {
                        throw new Exception($"Item ({row}, {col}) does not exist.");
                    }
                    if(above.down.right == null) {
                        throw new Exception("Cannot delete such that a row would become empty!");
                    }
                    deleting = above.down;
                }
                while(above != null && above.down != null) {
                    above.down = above.down.right;
                    above = above.right;
                }
            }
            // at this point, we've shifted over the row above as well as hooked up the left node to the new one and the new node to the node right of the left node
            // next, we need to shift the target row's down properties
            if(deleting.right != null) {
                this.ReplaceBackwards(deleting.right, deleting.down);
            }
            return deleting;
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

        private U ThroughEach<U>(U starting, Func<Node<T>, U> withEach, Func<U, U, U> combine, Func<U>? withEachRow = null) {
            U returning = starting;
            Node<T>? leftmost = this.Head;
            while(leftmost != null) {
                Node<T>? viewing = leftmost;
                while(viewing != null) {
                    returning = combine(returning, withEach(viewing));
                    viewing = viewing.right;
                }
                if(withEachRow != null) {
                    returning = combine(returning, withEachRow());
                }
                leftmost = leftmost.down;
            }
            return returning;
        }

        public override string ToString() {
            return this.ThroughEach<string>("", (Node<T> viewing) => $"{viewing.val} ", (string current, string adding) => current + adding, () => "\n");
        }

        public string DeepPrint() {
            return this.ThroughEach<string>("", 
                (Node<T> viewing) => $"{viewing.val}>{((viewing.right == null) ? "null" : viewing.right.val)}v{((viewing.down == null) ? "null" : viewing.down.val)} ", 
            (string current, string adding) => current + adding, () => "\n");
        }
    }
}
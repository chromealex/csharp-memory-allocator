namespace ME.MA.Collections {

    using MemPtr = System.Int64;

    [System.Diagnostics.DebuggerTypeProxyAttribute(typeof(StackProxy<>))]
    public struct Stack<T> where T : unmanaged {

        private const int DEFAULT_CAPACITY = 4;

        private MemArrayAllocator<T> array;
        private int size;
        private int version;
        public bool isCreated => this.array.isCreated;

        public readonly int Count => this.size;

        public Stack(ref MemoryAllocator allocator, int capacity) {
            this = default;
            this.array = new MemArrayAllocator<T>(ref allocator, capacity);
        }

        public void Dispose(ref MemoryAllocator allocator) {
            
            this.array.Dispose(ref allocator);
            this = default;
            
        }

        public void Clear(in MemoryAllocator allocator) {
            this.size = 0;
            this.version++;
        }

        public bool Contains<U>(in MemoryAllocator allocator, U item) where U : System.IEquatable<T> {

            var count = this.size;
            while (count-- > 0) {
                if (item.Equals(this.array[in allocator, count])) {
                    return true;
                }
            }

            return false;

        }

        public readonly EnumeratorNoState GetEnumerator(in MemoryAllocator allocator) {
            return new EnumeratorNoState(this, in allocator);
        }

        public readonly T Peek(in MemoryAllocator allocator) {
            if (this.size == 0) {
                ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EmptyStack);
            }

            return this.array[in allocator, this.size - 1];
        }

        public T Pop(in MemoryAllocator allocator) {
            if (this.size == 0) {
                ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EmptyStack);
            }

            this.version++;
            var item = this.array[in allocator, --this.size];
            this.array[in allocator, this.size] = default;
            return item;
        }

        public void Push(ref MemoryAllocator allocator, T item) {
            if (this.size == this.array.Length) {
                this.array.Resize(ref allocator, this.array.Length == 0 ? Stack<T>.DEFAULT_CAPACITY : 2 * this.array.Length);
            }

            this.array[in allocator, this.size++] = item;
            this.version++;
        }

        public struct EnumeratorNoState : System.Collections.Generic.IEnumerator<T> {

            private readonly Stack<T> stack;
            private readonly MemoryAllocator allocator;
            private int index;
            private readonly int version;
            private T currentElement;

            internal EnumeratorNoState(Stack<T> stack, in MemoryAllocator allocator) {
                this.stack = stack;
                this.allocator = allocator;
                this.version = this.stack.version;
                this.index = -2;
                this.currentElement = default(T);
            }

            public void Dispose() {
                this.index = -1;
            }

            public bool MoveNext() {
                bool retval;
                if (this.version != this.stack.version) {
                    ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumFailedVersion);
                }

                if (this.index == -2) { // First call to enumerator.
                    this.index = this.stack.size - 1;
                    retval = this.index >= 0;
                    if (retval) {
                        this.currentElement = this.stack.array[in this.allocator, this.index];
                    }

                    return retval;
                }

                if (this.index == -1) { // End of enumeration.
                    return false;
                }

                retval = --this.index >= 0;
                if (retval) {
                    this.currentElement = this.stack.array[in this.allocator, this.index];
                } else {
                    this.currentElement = default(T);
                }

                return retval;
            }

            public T Current {
                get {
                    if (this.index == -2) {
                        ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumNotStarted);
                    }

                    if (this.index == -1) {
                        ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumEnded);
                    }

                    return this.currentElement;
                }
            }

            object System.Collections.IEnumerator.Current {
                get {
                    if (this.index == -2) {
                        ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumNotStarted);
                    }

                    if (this.index == -1) {
                        ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumEnded);
                    }

                    return this.currentElement;
                }
            }

            void System.Collections.IEnumerator.Reset() {
                if (this.version != this.stack.version) {
                    ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumFailedVersion);
                }

                this.index = -2;
                this.currentElement = default;
            }

        }

    }

}
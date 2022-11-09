namespace ME.MA.Collections {

    using Unity.Collections.LowLevel.Unsafe;

    public struct SparseSet<T> where T : unmanaged {

        private MemArraySlicedAllocator<T> dense;
        private MemArrayAllocator<int> sparse;
        private Stack<int> freeIndexes;

        public bool isCreated => this.sparse.isCreated;
        public int Length => this.sparse.Length;
        
        public SparseSet(ref MemoryAllocator allocator, int length) {

            this.dense = default;
            this.sparse = default;
            this.freeIndexes = default;
            this.Validate(ref allocator, length);

        }

        public T ReadDense(in MemoryAllocator allocator, int sparseIndex) {

            return this.dense[in allocator, sparseIndex];

        }
        
        public ref T GetDense(in MemoryAllocator allocator, int sparseIndex) {

            return ref this.dense[in allocator, sparseIndex];

        }

        public MemArrayAllocator<int> GetSparse() {

            return this.sparse;

        }

        public SparseSet<T> Merge(ref MemoryAllocator allocator) {

            this.dense = this.dense.Merge(ref allocator);
            return this;

        }
        
        public void Validate(ref MemoryAllocator allocator, int capacity) {

            if (this.freeIndexes.isCreated == false) this.freeIndexes = new Stack<int>(ref allocator, 10);
            this.sparse.Resize(ref allocator, capacity);

        }

        public SparseSet<T> Dispose(ref MemoryAllocator allocator) {
            
            this.freeIndexes.Dispose(ref allocator);
            this.sparse.Dispose(ref allocator);
            this.dense.Dispose(ref allocator);
            return this;

        }

        public void Set(ref MemoryAllocator allocator, int fromEntityId, int toEntityId, in T data) {

            for (int i = fromEntityId; i <= toEntityId; ++i) {
                this.Set(ref allocator, i, in data);
            }
            
        }

        public int Set(ref MemoryAllocator allocator, int entityId, in T data) {

            ref var idx = ref this.sparse[in allocator, entityId];
            if (idx == 0) {
                if (this.freeIndexes.Count > 0) {
                    idx = this.freeIndexes.Pop(in allocator);
                } else {
                    idx = this.dense.Length + 1;
                }
            }

            this.dense.Resize(ref allocator, idx + 1, out _);
            this.dense[in allocator, idx] = data;
            return idx;

        }

        public ref T Get(ref MemoryAllocator allocator, int entityId) {
            
            var idx = this.sparse[in allocator, entityId];
            if (idx == 0) idx = this.Set(ref allocator, entityId, default);
            return ref this.dense[in allocator, idx];

        }

        public T Read(ref MemoryAllocator allocator, int entityId) {

            var idx = this.sparse[in allocator, entityId];
            if (idx == 0) return default;
            return this.dense[in allocator, idx];

        }

        public void Remove(ref MemoryAllocator allocator, int entityId) {
            
            ref var idx = ref this.sparse[in allocator, entityId];
            this.dense[in allocator, idx] = default;
            this.freeIndexes.Push(ref allocator, idx);
            idx = 0;
            
        }

        public void Remove(ref MemoryAllocator allocator, int entityId, int length) {

            for (int i = entityId; i < length; ++i) {
                this.Remove(ref allocator, i);
            }
            
        }

        public T Has(ref MemoryAllocator allocator, int entityId) {

            return this.Get(ref allocator, entityId);

        }

        public unsafe int SetPtr(MemoryAllocator* allocator, int entityId, in T data) {
            
            ref var alloc = ref UnsafeUtility.AsRef<MemoryAllocator>(allocator);
            return this.Set(ref alloc, entityId, in data);

        }

        public unsafe T ReadPtr(MemoryAllocator* allocator, int entityId) {

            ref var alloc = ref UnsafeUtility.AsRef<MemoryAllocator>(allocator);
            var idx = this.sparse[in alloc, entityId];
            if (idx == 0) return default;
            return this.Get(ref alloc, entityId);

        }

        public unsafe bool HasDataPtr(MemoryAllocator* allocator, int entityId) {

            ref var alloc = ref UnsafeUtility.AsRef<MemoryAllocator>(allocator);
            var idx = this.sparse[in alloc, entityId];
            if (idx == 0) return false;
            return true;

        }

        public unsafe T HasPtr(MemoryAllocator* allocator, int entityId) {

            return this.ReadPtr(allocator, entityId);

        }

        public unsafe ref T GetPtr(MemoryAllocator* allocator, int entityId) {

            ref var alloc = ref UnsafeUtility.AsRef<MemoryAllocator>(allocator);
            return ref this.Get(ref alloc, entityId);

        }

        public unsafe void RemovePtr(MemoryAllocator* allocator, int entityId, int length) {

            ref var alloc = ref UnsafeUtility.AsRef<MemoryAllocator>(allocator);
            this.Remove(ref alloc, entityId, length);

        }

    }

}

namespace ME.MA.Tests.Collections {

    using NUnit.Framework;
    using UnityEngine;
    using ME.MA.Collections;

    public class Queue_Tests {

        [Test]
        public void Initialize() {

            var allocator = Core_Tests.GetAllocator(10);

            var list = new Queue<Vector3>(ref allocator, 10);
            Assert.IsTrue(list.isCreated);
            list.Dispose(ref allocator);

            allocator.Dispose();

        }

        [Test]
        public void ForEach() {

            var allocator = Core_Tests.GetAllocator(10);

            var list = new Queue<Vector3>(ref allocator, 10);
            for (int i = 0; i < 100; ++i) {

                list.Enqueue(ref allocator, new Vector3(i, i, i));
                
            }

            var cnt = 0;
            var e = list.GetEnumerator(in allocator);
            while (e.MoveNext() == true) {
                Assert.IsTrue(e.Current.x >= 0 && e.Current.x < 100);
                ++cnt;
            }
            e.Dispose();
            
            Assert.IsTrue(list.Count == 100);
            Assert.IsTrue(cnt == 100);

            allocator.Dispose();

        }

        [Test]
        public void Enqueue() {

            var allocator = Core_Tests.GetAllocator(10);

            var list = new Queue<Vector3>(ref allocator, 10);
            for (int i = 0; i < 100; ++i) {

                list.Enqueue(ref allocator, new Vector3(i, i, i));
                
            }
            
            Assert.IsTrue(list.Count == 100);

            allocator.Dispose();

        }

        [Test]
        public void Contains() {

            var allocator = Core_Tests.GetAllocator(10);

            var list = new Queue<Vector3>(ref allocator, 10);
            for (int i = 0; i < 100; ++i) {

                list.Enqueue(ref allocator, new Vector3(i, i, i));
                
            }
            
            for (int i = 0; i < 100; ++i) {

                Assert.IsTrue(list.Contains(in allocator, new Vector3(i, i, i)));
                
            }

            allocator.Dispose();

        }

        [Test]
        public void Dequeue() {

            var allocator = Core_Tests.GetAllocator(10);

            var list = new Queue<Vector3Int>(ref allocator, 10);
            for (int i = 0; i < 100; ++i) {

                list.Enqueue(ref allocator, new Vector3Int(i, i, i));
                
            }
            
            Assert.IsTrue(list.Count == 100);
            Assert.IsTrue(list.Dequeue(in allocator) == new Vector3Int(0, 0, 0));
            Assert.IsTrue(list.Dequeue(in allocator) == new Vector3Int(1, 1, 1));
            Assert.IsTrue(list.Count == 98);

            allocator.Dispose();

        }

        [Test]
        public void Peek() {

            var allocator = Core_Tests.GetAllocator(10);

            var list = new Queue<Vector3Int>(ref allocator, 10);
            for (int i = 0; i < 100; ++i) {

                list.Enqueue(ref allocator, new Vector3Int(i, i, i));
                
            }
            
            Assert.IsTrue(list.Count == 100);
            Assert.IsTrue(list.Peek(in allocator) == new Vector3Int(0, 0, 0));
            Assert.IsTrue(list.Dequeue(in allocator) == new Vector3Int(0, 0, 0));
            Assert.IsTrue(list.Count == 99);

            allocator.Dispose();

        }

        [Test]
        public void EnqueueDequeue() {

            var allocator = Core_Tests.GetAllocator(10);

            var list = new Queue<Vector3Int>(ref allocator, 10);
            for (int i = 0; i < 100; ++i) {

                list.Enqueue(ref allocator, new Vector3Int(i, i, i));
                
            }

            Assert.IsTrue(list.Count == 100);
            
            for (int i = 0; i < 100; ++i) {

                Assert.IsTrue(list.Dequeue(in allocator) == new Vector3Int(i, i, i));
                
            }

            Assert.IsTrue(list.Count == 0);

            allocator.Dispose();

        }

        [Test]
        public void Clear() {

            var allocator = Core_Tests.GetAllocator(10);

            var list = new Queue<Vector3>(ref allocator, 10);
            for (int i = 0; i < 100; ++i) {

                list.Enqueue(ref allocator, new Vector3(i, i, i));
                
            }
            
            Assert.IsTrue(list.Count == 100);
            list.Clear(in allocator);
            Assert.IsTrue(list.Count == 0);

            allocator.Dispose();

        }

    }

}
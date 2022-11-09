namespace ME.MA.Tests.Collections {

    using NUnit.Framework;
    using ME.MA.Collections;

    public class NativeHashSet_Tests {

        public struct Test : IEquatableAllocator<Test> {

            public float x;
            public float y;
            public float z;

            public Test(float x, float y, float z) {
                this.x = x;
                this.y = y;
                this.z = z;
            }

            public bool Equals(in MemoryAllocator allocator, Test obj) {
                return this.x == obj.x &&
                    this.y == obj.y &&
                    this.z == obj.z;
            }

            public int GetHash(in MemoryAllocator allocator) {
                return this.x.GetHashCode() ^ this.y.GetHashCode() ^ this.z.GetHashCode();
            }

        }

        [Test]
        public void Initialize() {

            var allocator = Core_Tests.GetAllocator(10);

            var list = new NativeHashSet<Test>(ref allocator, 10);
            Assert.IsTrue(list.isCreated);
            list.Dispose(ref allocator);
            
            allocator.Dispose();

        }

        [Test]
        public void ForEach() {

            var allocator = Core_Tests.GetAllocator(10);

            var list = new NativeHashSet<Test>(ref allocator, 10);
            for (int i = 0; i < 100; ++i) {

                list.Add(ref allocator, new Test(i, i, i));
                
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
        public void Add() {

            var allocator = Core_Tests.GetAllocator(10);

            var list = new NativeHashSet<Test>(ref allocator, 10);
            for (int i = 0; i < 100; ++i) {

                list.Add(ref allocator, new Test(i, i, i));
                
            }
            
            Assert.IsTrue(list.Count == 100);

            allocator.Dispose();

        }

        [Test]
        public void Contains() {

            var allocator = Core_Tests.GetAllocator(10);

            var cnt = 2;
            var list = new NativeHashSet<Test>(ref allocator, 10);
            for (int i = 0; i < cnt; ++i) {

                list.Add(ref allocator, new Test(i, i, i));
                
            }
            
            for (int i = 0; i < cnt; ++i) {

                Assert.IsTrue(list.Contains(in allocator, new Test(i, i, i)));
                
            }
            
            allocator.Dispose();

        }

        [Test]
        public void Remove() {

            var allocator = Core_Tests.GetAllocator(10);

            var list = new NativeHashSet<Test>(ref allocator, 10);
            for (int i = 0; i < 100; ++i) {

                list.Add(ref allocator, new Test(i, i, i));
                
            }
            
            Assert.IsTrue(list.Count == 100);
            Assert.IsTrue(list.Remove(ref allocator, new Test(50, 50, 50)));
            Assert.IsFalse(list.Remove(ref allocator, new Test(50, 50, 50)));
            Assert.IsTrue(list.Count == 99);

            allocator.Dispose();

        }

        [Test]
        public void Clear() {

            var allocator = Core_Tests.GetAllocator(10);

            var list = new NativeHashSet<Test>(ref allocator, 10);
            for (int i = 0; i < 100; ++i) {

                list.Add(ref allocator, new Test(i, i, i));
                
            }
            
            Assert.IsTrue(list.Count == 100);
            list.Clear(in allocator);
            Assert.IsTrue(list.Count == 0);

            allocator.Dispose();

        }

    }

}
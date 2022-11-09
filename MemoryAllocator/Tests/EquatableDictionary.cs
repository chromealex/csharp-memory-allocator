namespace ME.MA.Tests.Collections {

    using NUnit.Framework;
    using UnityEngine;
    using ME.MA.Collections;

    public class EquatableDictionary_Tests {

        [Test]
        public void Initialize() {

            var allocator = Core_Tests.GetAllocator(10);

            var list = new EquatableDictionary<int, Vector3>(ref allocator, 10);
            Assert.IsTrue(list.isCreated);
            list.Dispose(ref allocator);
            
            allocator.Dispose();

        }

        [Test]
        public void ForEach() {

            var allocator = Core_Tests.GetAllocator(10);

            var list = new EquatableDictionary<int, Vector3Int>(ref allocator, 10);
            for (int i = 0; i < 100; ++i) {

                list.Add(ref allocator, i, new Vector3Int(i, i, i));
                
            }

            var cnt = 0;
            var e = list.GetEnumerator(in allocator);
            while (e.MoveNext() == true) {
                Assert.IsTrue(e.Current.Key == e.Current.Value.x && e.Current.Value.x >= 0 && e.Current.Value.x < 100);
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

            var list = new EquatableDictionary<int, Vector3>(ref allocator, 10);
            for (int i = 0; i < 100; ++i) {

                list.Add(ref allocator, i, new Vector3(i, i, i));
                
            }
            
            Assert.IsTrue(list.Count == 100);

            allocator.Dispose();

        }

        [Test]
        public void Contains() {

            var allocator = Core_Tests.GetAllocator(10);

            var cnt = 2;
            var list = new EquatableDictionary<int, Vector3>(ref allocator, 10);
            for (int i = 0; i < cnt; ++i) {

                list.Add(ref allocator, i, new Vector3(i, i, i));
                
            }
            
            for (int i = 0; i < cnt; ++i) {

                Assert.IsTrue(list.ContainsKey(in allocator, i));
                Assert.IsTrue(list.ContainsValue(in allocator, new Vector3(i, i, i)));
                
            }
            
            allocator.Dispose();

        }

        [Test]
        public void Remove() {

            var allocator = Core_Tests.GetAllocator(10);

            var list = new EquatableDictionary<int, Vector3>(ref allocator, 10);
            for (int i = 0; i < 100; ++i) {

                list.Add(ref allocator, i, new Vector3(i, i, i));
                
            }
            
            Assert.IsTrue(list.Count == 100);
            Assert.IsTrue(list.Remove(ref allocator, 50));
            Assert.IsFalse(list.Remove(ref allocator, 50));
            Assert.IsTrue(list.Count == 99);

            allocator.Dispose();

        }

        [Test]
        public void Clear() {

            var allocator = Core_Tests.GetAllocator(10);

            var list = new EquatableDictionary<int, Vector3>(ref allocator, 10);
            for (int i = 0; i < 100; ++i) {

                list.Add(ref allocator, i, new Vector3(i, i, i));
                
            }
            
            Assert.IsTrue(list.Count == 100);
            list.Clear(in allocator);
            Assert.IsTrue(list.Count == 0);

            allocator.Dispose();

        }

    }

}
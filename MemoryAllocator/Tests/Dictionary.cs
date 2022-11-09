using System.Linq;

namespace ME.MA.Tests.Collections {

    using NUnit.Framework;
    using UnityEngine;
    using ME.MA.Collections;

    public class Dictionary_Tests {

        [Test]
        public void Initialize() {

            var allocator = Core_Tests.GetAllocator(10);

            var list = new Dictionary<int, Vector3>(ref allocator, 10);
            Assert.IsTrue(list.isCreated);
            list.Dispose(ref allocator);
            
            allocator.Dispose();

        }

        [Test]
        public void ForEach() {

            var allocator = Core_Tests.GetAllocator(10);

            var list = new Dictionary<int, Vector3Int>(ref allocator, 10);
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
            StaticAllocatorProxy.defaultAllocator = allocator;

            var list = new Dictionary<int, int>(ref allocator, 10);
            for (int i = 0; i < 100; ++i) {

                list.Add(ref allocator, i, i);
                
            }
            
            Assert.IsTrue(list.Count == 100);

            allocator.Dispose();

        }

        [Test]
        public void Contains() {

            var allocator = Core_Tests.GetAllocator(10);

            var cnt = 1000;
            var list = new Dictionary<int, int>(ref allocator, 10);
            for (int i = 0; i < cnt; ++i) {

                list.Add(ref allocator, i, i);
                
            }
            
            for (int i = 0; i < cnt; ++i) {

                Assert.IsTrue(list.ContainsKey(in allocator, i));
                Assert.IsTrue(list.ContainsValue(in allocator, i));
                
            }
            
            allocator.Dispose();

        }

        [Test]
        public void LikeADictionary() {

            var allocator = Core_Tests.GetAllocator(10);
            StaticAllocatorProxy.defaultAllocator = allocator;

            var cnt = 1000;
            var source = new System.Collections.Generic.Dictionary<int, int>(10);
            var list = new Dictionary<int, int>(ref allocator, 10);
            var testData = new int[cnt];
            for (int i = 0; i < cnt; ++i) {

                testData[i] = i;
                list.Add(ref allocator, i, i);
                source.Add(i, i);
                
            }
            
            testData = testData.OrderBy(x => Random.value).ToArray();

            for (int i = 0; i < testData.Length; ++i) {
                
                var key = testData[i];
                Assert.IsTrue(list.ContainsKey(in allocator, key));
                Assert.IsTrue(source.ContainsKey(key));
                
            }

            {
                var newDic = new Dictionary<int, int>(ref allocator, list.Count);
                newDic.CopyFrom(ref allocator, list);

                for (int i = 0; i < testData.Length; ++i) {

                    var key = testData[i];
                    Assert.IsTrue(newDic.ContainsKey(in allocator, key));

                }
            }

            for (int i = 0; i < testData.Length / 2; ++i) {
                
                var key = testData[i];
                Assert.IsTrue(list.Remove(ref allocator, key));
                Assert.IsTrue(source.Remove(key));

            }
            
            Assert.IsTrue(list.Count == cnt / 2);
            Assert.IsTrue(source.Count == cnt / 2);

            for (int i = testData.Length / 2; i < testData.Length; ++i) {
                
                var key = testData[i];
                Assert.IsTrue(list.Remove(ref allocator, key));
                Assert.IsTrue(source.Remove(key));

            }

            Assert.AreEqual(0, source.Count);
            Assert.AreEqual(0, list.Count);

            list.Clear(in allocator);
            source.Clear();

            testData = new int[] {
                38, 33, 138, 13, 17, 52, 130, 59, 2, 140, 64, 66, 60, 67, 35, 164, 6, 80, 81, 83, 4, 5, 155, 137, 3, 22, 65, 23, 24, 21, 18, 125, 82, 37, 118, 133, 75, 178
            };

            testData = testData.OrderBy(x => Random.value).ToArray();

            foreach (var data in testData) {
                
                list.Add(ref allocator, data, data);
                source.Add(data, data);
                
            }
            
            for (int i = 0; i < testData.Length; ++i) {

                var key = testData[i];
                Assert.IsTrue(list.ContainsKey(in allocator, key));
                Assert.IsTrue(source.ContainsKey(key));

            }

            {
                var newDic = new Dictionary<int, int>(ref allocator, list.Count);
                newDic.CopyFrom(ref allocator, list);

                for (int i = 0; i < testData.Length; ++i) {

                    var key = testData[i];
                    Assert.IsTrue(newDic.ContainsKey(in allocator, key));

                }
            }

            {
                var newDic = new Dictionary<int, int>(ref allocator, 1);
                var testData1 = new int[] {
                    38, 33, 138, 13, 17, 52, 130, 59, 2, 140, 64, 66, 60, 67, 35, 164, 6, 80, 81, 83, 4, 5, 155, 137, 3, 22, 65, 23, 24, 21, 18, 125, 82, 37, 118, 133, 75,
                };
                foreach (var item in testData1) {
                    newDic.Add(ref allocator, item, item);
                }
                
                var newDic2 = new Dictionary<int, int>(ref allocator, newDic.Count + 1);
                newDic2.CopyFrom(ref allocator, newDic);
                newDic2.Add(ref allocator, 178, 178);
                newDic.Add(ref allocator, 178, 178);
                Assert.IsTrue(newDic.ContainsKey(in allocator, 118));
                Assert.IsTrue(newDic2.ContainsKey(in allocator, 118));
            }

            allocator.Dispose();

        }

        [Test]
        public void Remove() {

            var allocator = Core_Tests.GetAllocator(10);

            var list = new Dictionary<int, Vector3>(ref allocator, 10);
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

            var list = new Dictionary<int, Vector3>(ref allocator, 10);
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
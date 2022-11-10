namespace ME.MA {
    
    public class OutOfBoundsException : System.Exception {

        public OutOfBoundsException() : base("ME.MA Exception") { }
        public OutOfBoundsException(string message) : base(message) { }

    }
    
    public class CollectionNotCreated : System.Exception {

        public CollectionNotCreated() : base("ME.MA Exception") { }
        public CollectionNotCreated(string message) : base(message) { }

    }
    
    public static class E {

        public static void RANGE(int index, int lowBound, int highBound) {
            
            if (index < lowBound || index >= highBound) {
                
                throw new OutOfBoundsException($"index {index} must be in range {lowBound}..{highBound}");
                
            }
            
        }

        public static void IS_CREATED<T>(T collection) where T : ME.MA.IIsCreated {

            if (collection.isCreated == false) {

                IS_CREATED_BURST_DISCARD(collection);
                throw new CollectionNotCreated("Collection not created");
                
            }

        }

        [Unity.Burst.BurstDiscardAttribute]
        private static void IS_CREATED_BURST_DISCARD<T>(T collection) where T : ME.MA.IIsCreated {

            if (collection.isCreated == false) {
                
                throw new CollectionNotCreated($"{collection.GetType()} not created");
                
            }

        }

    }
    
}

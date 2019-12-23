namespace Data {
    
    public struct OpenLoadingMenuOutput {

        public Slot[] slots;

        public struct Slot {

            public int slotId;
            public bool containsSaveData;
        }
    }
}

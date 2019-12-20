public interface IMetaGameStateStore {

    MetaGameSave GetSave(int slot);
    void Save(int slot, MetaGameSave save);
    bool SaveExists(int slot);
}
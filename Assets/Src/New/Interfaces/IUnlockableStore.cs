using Workers;

public interface IUnlockableStore {

    Unlockable[] GetUnlockables();
    Unlockable GetUnlockable(UnlockableType type);
}
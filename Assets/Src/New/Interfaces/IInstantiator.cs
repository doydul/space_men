public interface IInstantiator {

    T MakeObject<T>() where T : class;
}
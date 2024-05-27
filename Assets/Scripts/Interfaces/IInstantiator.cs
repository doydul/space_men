using System;

public interface IInstantiator {

    T MakeObject<T>(params object[] args) where T : class;
    object MakeObject(Type T, params object[] args);
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public class DependencyInjector : IInstantiator {
	
	public Dictionary<Type, Object> deps;
	
	public DependencyInjector() {
		deps = new Dictionary<Type, Object> { {typeof(IInstantiator), this} };
	}
	
	public void RegisterDependency(Type depType, Object dep) {
		deps.Add(depType, dep);
	}
	
	public T MakeObject<T>(params object[] args) where T : class {
		var constructor = typeof(T).GetConstructors()[0];
		var constructorParameters = constructor.GetParameters();
		if (args.Length != constructorParameters.Count(param => !param.IsOptional)) throw new Exception("wrong number of constructor arguments");
		var instance = Activator.CreateInstance(typeof(T), args) as T;

		var privateInstanceFields = typeof(T).GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
		foreach (var field in privateInstanceFields) {
			if (field.GetCustomAttributes(typeof(Dependency), true).Length > 0) {
				foreach (var typeDepPair in deps) {
					if (field.FieldType == typeDepPair.Key) {
						field.SetValue(instance, typeDepPair.Value);
						break;
					}
				}
			}
		}
		return instance;
	}

	public object MakeObject(Type T, params object[] args) {
		var constructor = T.GetConstructors()[0];
		var constructorParameters = constructor.GetParameters();
		if (args.Length != constructorParameters.Count(param => !param.IsOptional)) throw new Exception("wrong number of constructor arguments");
		var instance = Activator.CreateInstance(T, args);

		var privateInstanceFields = T.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
		foreach (var field in privateInstanceFields) {
			if (field.GetCustomAttributes(typeof(Dependency), true).Length > 0) {
				foreach (var typeDepPair in deps) {
					if (field.FieldType == typeDepPair.Key) {
						field.SetValue(instance, typeDepPair.Value);
						break;
					}
				}
			}
		}
		return instance;
	}
}

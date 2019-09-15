using System;
using System.Collections.Generic;

public class DependencyInjector : IInstantiator {
	
	public List<Object> deps;
	
	public DependencyInjector() {
		deps = new List<Object> { this };
	}
	
	public void RegisterDependency(Object dep) {
		deps.Add(dep);
	}
	
	public T MakeObject<T>() where T : class {
		var constructor = typeof(T).GetConstructors()[0];
		var parameters = constructor.GetParameters();
		var args = new List<Object>();
		foreach (var param in parameters) {
			foreach (var dep in deps) {
				if (param.ParameterType.IsAssignableFrom(dep.GetType())) {
					args.Add(dep);
					break;
				}
			}
		}
		return Activator.CreateInstance(typeof(T), args.ToArray()) as T;
	}
}

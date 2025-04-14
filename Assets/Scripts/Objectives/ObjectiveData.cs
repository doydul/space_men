using System;

[System.Serializable]
public class ObjectiveData {
    
    public bool required;
    public string objectiveType;
    
    public Objective Dump() {
        Type type = Type.GetType( objectiveType );
        Objective result = Activator.CreateInstance( type ) as Objective;
        result.required = required;
        return result;
    }
}
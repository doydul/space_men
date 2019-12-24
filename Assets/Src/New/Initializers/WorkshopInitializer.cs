using System;
using System.Collections.Generic;

using Interactors;
using Workers;

public class WorkshopInitializer : InitializerBase {

    protected override void GenerateControllerMapping() {
        controllerMapping.Add(typeof(MetaGameController), new Dictionary<Type, Type> {
            
        });
    }

    protected override void GenerateDependencies() {
        
    }

    protected override void Initialize() {
        
    }
}

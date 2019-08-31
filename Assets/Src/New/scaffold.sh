#!/bin/bash

modelName=$1
DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" >/dev/null && pwd )"
cd $DIR

cat > ./Data/Interactors/${modelName}Input.cs <<EOF
namespace Data {
    
    public struct ${modelName}Input {
        
    }
}
EOF

cat > ./Data/Interactors/${modelName}Output.cs <<EOF
namespace Data {
    
    public struct ${modelName}Output {
        
    }
}
EOF

cat > ./Interactors/${modelName}Interactor.cs <<EOF
using Data;
using Workers;

namespace Interactors {
    
    public class ${modelName}Interactor : Interactor<${modelName}Output> {

        public void Interact(${modelName}Input input) {
            var output = new ${modelName}Output();
            
            // TODO
            
            presenter.Present(output);
        }
    }
}
EOF

cat > ./Interactors/${modelName}Presenter.cs <<EOF
using Data;

public class ${modelName}Presenter : Presenter, IPresenter<${modelName}Output> {
  
    public static ${modelName}Presenter instance { get; private set; }
    
    void Awake() {
        instance = this;
    }
    
    public void Present(${modelName}Output input) {
        // TODO
    }
}

EOF

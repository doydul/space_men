#!/bin/bash

modelName=$1
DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" >/dev/null && pwd )"

cd $DIR

cat > ./Controllers/${modelName}Controller.cs <<EOF
using Interactors;
using Data;

// controllerMapping.Add(typeof(${modelName}Controller), new Dictionary<Type, Type> {
//    { typeof(DoSomeActionInteractor), typeof(DoSomeActionPresenter) }
// });
public class ${modelName}Controller : Controller {

    // public DoSomeActionInteractor doSomeActionInteractor { get; set; }

    public void DoSomeAction() {
        if (!disabled) {
            // doSomeActionInteractor.Interact(new DoSomeActionInput());
        }
    }
}
EOF
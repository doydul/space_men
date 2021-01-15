#!/bin/bash

modelName=$1
DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" >/dev/null && pwd )"

cd $DIR

cat > ./Initializers/${modelName}Initializer.cs <<EOF
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

using Interactors;
using Workers;

public class ${modelName}Initializer : InitializerBase {

    protected override void GenerateControllerMapping() {
        // controllerMapping.Add(typeof(SomeController), new Dictionary<Type, Type> {
        //    { typeof(DoSomeActionInteractor), typeof(DoSomeActionPresenter) }
        // });
    }

    protected override void GenerateDependencies() {
        // dependencies.Add(new SomeDep());
    }

    protected override void Initialize() {
        // someController.Init();
    }
}
EOF
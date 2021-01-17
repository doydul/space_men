using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Data;
using Workers;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Reflection;

public class ExecuteSpecialAbilityPresenter : Presenter, IPresenter<ExecuteSpecialAbilityOutput> {
  
    public static ExecuteSpecialAbilityPresenter instance { get; private set; }

    public Map map;
    public MapController mapInput;
    public AllControllers controllers;
    public StandardAnimations animations;
    public Scripting scripting;
    
    void Awake() {
        instance = this;
    }
    
    public void Present(ExecuteSpecialAbilityOutput input) {
        StartCoroutine(DoPresent(input));
    }

    IEnumerator DoPresent(ExecuteSpecialAbilityOutput input) {
        var output = input.output;
        controllers.DisableAll();

        var regex = new Regex(@"\.(\w+)\+");
        var className = regex.Match(output.GetType().FullName).Groups[1].Captures[0].Value;
        var type = AppDomain.CurrentDomain.GetAssemblies()
                       .SelectMany(t => t.GetTypes())
                       .First(c => c.Name.Contains(className + "Presenter"));
        var instance = type.GetProperty("instance", BindingFlags.Public | BindingFlags.Static)
            .GetValue(null);//
        yield return instance.GetType().GetMethod("Present").Invoke(instance, new object[] { output });
        Cleanup(input.soldierId);
    }

    void Cleanup(long soldierIndex) {
        controllers.EnableAll();
        mapInput.DisplayActions(soldierIndex);
    }
}


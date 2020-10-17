using System.Linq;
using Data;
using Workers;

namespace Interactors {
    
    public class ExecuteSpecialAbilityInteractor : Interactor<ExecuteSpecialAbilityOutput> {

        [Dependency] IInstantiator factory;

        public void Interact(ExecuteSpecialAbilityInput input) {
            var output = new ExecuteSpecialAbilityOutput();
            
            var specialActions = factory.MakeObject<SpecialActions>(input.soldierId, input.targetSquare);
            var ability = specialActions.GetAbilities().First(ab => ab.type == input.type);

            output.abilityOutput = ability.Execute();
            output.type = input.type;
            
            presenter.Present(output);
        }
    }
}

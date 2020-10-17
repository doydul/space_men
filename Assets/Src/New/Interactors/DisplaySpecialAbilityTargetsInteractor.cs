using System.Linq;
using Data;
using Workers;

namespace Interactors {
    
    public class DisplaySpecialAbilityTargetsInteractor : Interactor<DisplaySpecialAbilityTargetsOutput> {

        [Dependency] GameState gameState;
        [Dependency] IInstantiator factory;

        public void Interact(DisplaySpecialAbilityTargetsInput input) {
            var output = new DisplaySpecialAbilityTargetsOutput();
            
            var specialActions = factory.MakeObject<SpecialActions>(input.soldierId, default(Position));
            var ability = specialActions.GetAbilities().First(ab => ab.type == input.type);

            output.possibleActions = ability.possibleTargetSquares.Select(pos => new ActorAction {
                    index = input.soldierId,
                    type = ActorActionType.Special,
                    target = pos,
                    specialAction = input.type
                }).ToArray();
            output.executeImmediately = ability.executeImmediately;
            output.type = input.type;
            
            presenter.Present(output);
        }
    }
}

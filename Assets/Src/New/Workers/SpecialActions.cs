using System.Collections.Generic;
using System.Linq;
using Data;

namespace Workers {
    public class SpecialActions {

        [Dependency] GameState gameState;
        [Dependency] IInstantiator factory;

        long soldierId;
        Position target;

        public SpecialActions(long soldierId, Position target) {
            this.soldierId = soldierId;
            this.target = target;
        }

        public ActorAction[] GetSpecialActions() {
            return GetAbilities().Where(ability => ability.usable)
                .Select(ability => new ActorAction {
                    index = soldierId,
                    type = ActorActionType.Special,
                    specialAction = ability.type
                }).ToArray();
        }

        public SpecialAbility[] GetAbilities() {
            return new SpecialAbility[] {
                factory.MakeObject<FireAtGround>(new FireAtGround.Input { soldierId = soldierId, targetSquare = target }),
                factory.MakeObject<Grenade>(new Grenade.Input { soldierId = soldierId, targetSquare = target }),
                factory.MakeObject<CollectAmmo>(new CollectAmmo.Input { soldierId = soldierId }),
                factory.MakeObject<StunShot>(new StunShot.Input { soldierId = soldierId, targetSquare = target })
            };
        }
    }
}
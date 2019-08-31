using Data;
using Workers;
using System.Collections.Generic;
using System.Linq;

namespace Interactors {
    
    public class ProgressGamePhaseInteractor : Interactor<ProgressGamePhaseOutput> {
        
        int stage;

        private const int SHOOTING_PHASE_ITERATIONS = 3;

        public void Interact(ProgressGamePhaseInput input) {
            var currentPhase = Storage.instance.GetCurrentPhase();
            if (currentPhase == Data.GamePhase.Movement) {
                StartShootingPhase();
            } else {
                ProgessShootingPhase();
            }
        }

        void StartShootingPhase() {
            stage = 0;
            Storage.instance.SetCurrentPhase(Data.GamePhase.Shooting);
            AlienTurnHack.SpawnAliens();
        }

        void ProgessShootingPhase() {
            if (stage >= SHOOTING_PHASE_ITERATIONS) {
                StartMovementPhase();
                return;
            }
            stage++;
            AlienTurnHack.MoveAliens();
            // if player has no actions to take, proceed again
        }

        void StartMovementPhase() {
            Storage.instance.SetCurrentPhase(Data.GamePhase.Movement);
            StartMovementPhaseHack.StartMovementPhase();
        }
    }
}

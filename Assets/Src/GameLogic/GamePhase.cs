using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class GamePhase : MonoBehaviour {

    private const int SHOOTING_PHASE_ITERATIONS = 4;

    public static IPhaseFactory phaseFactory;

    public Phase phase { get; private set; }

    public bool movement { get { return phase is MovementPhase; } }
    public bool shooting { get { return phase is ShootingPhase; } }

    void Start() {
        phase = phaseFactory.MakeMovementPhase();
    }

    public void ProceedPhase() {
        phase.Proceed().Then(() => {
            if (phase.finished) TogglePhase();
        });
    }

    void TogglePhase() {
        phase.End();
        if (phase is MovementPhase) {
            phase = phaseFactory.MakeShootingPhase();
        } else {
            phase = phaseFactory.MakeMovementPhase();
        }
        phase.Start();
    }
}

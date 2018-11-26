using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class GamePhase : MonoBehaviour {

    private const int SHOOTING_PHASE_ITERATIONS = 4;

    public Map map;
    public AlienMovementPhaseDirector alienMovementPhaseDirector;
    public AlienDeployer alienDeployer;

    public Phase phase { get; private set; }

    public UnityEvent MovementPhaseStart;
    public UnityEvent MovementPhaseEnd;
    public UnityEvent ShootingPhaseStart;
    public UnityEvent ShootingPhaseEnd;
    public UnityEvent ShootingPhaseIterate;
    public UnityEvent ShootingPhaseIterateEnd;

    public bool movement { get { return phase is MovementPhase; } }
    public bool shooting { get { return phase is ShootingPhase; } }

    void Awake() {
        phase = new MovementPhase(map);
        if (MovementPhaseEnd == null) MovementPhaseStart = new UnityEvent();
        if (ShootingPhaseEnd == null) ShootingPhaseStart = new UnityEvent();
        if (MovementPhaseEnd == null) MovementPhaseEnd = new UnityEvent();
        if (ShootingPhaseEnd == null) ShootingPhaseEnd = new UnityEvent();
        if (ShootingPhaseIterate == null) ShootingPhaseIterate = new UnityEvent();
        if (ShootingPhaseIterateEnd == null) ShootingPhaseIterateEnd = new UnityEvent();
    }

    public void ProceedPhase() {
        phase.Proceed();
        if (phase.finished) TogglePhase();

        // if (movement) {
        //     MovementPhaseEnd.Invoke();
        //     phase = new ShootingPhase(map, alienMovementPhaseDirector);
        //     phase.Start();
        //     ShootingPhaseStart.Invoke();
        //     GameEvents.Trigger("ShootingPhaseStart");
        //     shootingPhaseIteration = 0;
        // } else {
        //     if (shootingPhaseIteration >= SHOOTING_PHASE_ITERATIONS) {
        //         ShootingPhaseEnd.Invoke();
        //         phase = new MovementPhase(map);
        //         phase.Start();
        //         MovementPhaseStart.Invoke();
        //         GameEvents.Trigger("MovementPhaseStart");
        //     } else {
        //         shootingPhaseIteration++;
        //         ShootingPhaseIterate.Invoke();
        //         StartCoroutine(AwaitShootingIterationEnd());
        //     }
        // }
    }

    void TogglePhase() {
        if (phase is MovementPhase) {
            phase = new ShootingPhase(map, alienMovementPhaseDirector, alienDeployer);
            ShootingPhaseStart.Invoke();
        } else {
            phase = new MovementPhase(map);
            MovementPhaseStart.Invoke();
        }
        phase.Start();
    }
}

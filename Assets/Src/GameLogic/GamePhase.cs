using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class GamePhase : MonoBehaviour {

    private const int SHOOTING_PHASE_ITERATIONS = 4;

    public enum Phase {
        Movement,
        Shooting
    }

    public Phase phase { get; private set; }

    public UnityEvent MovementPhaseStart;
    public UnityEvent MovementPhaseEnd;
    public UnityEvent ShootingPhaseStart;
    public UnityEvent ShootingPhaseEnd;
    public UnityEvent ShootingPhaseIterate;
    public UnityEvent ShootingPhaseIterateEnd;

    private int shootingPhaseIteration;

    public bool movement { get { return phase == Phase.Movement; } }
    public bool shooting { get { return phase == Phase.Shooting; } }
    public bool shootingIterationInProgress { get; set; }

    void Awake() {
        phase = Phase.Movement;
        if (MovementPhaseEnd == null) MovementPhaseStart = new UnityEvent();
        if (ShootingPhaseEnd == null) ShootingPhaseStart = new UnityEvent();
        if (MovementPhaseEnd == null) MovementPhaseEnd = new UnityEvent();
        if (ShootingPhaseEnd == null) ShootingPhaseEnd = new UnityEvent();
        if (ShootingPhaseIterate == null) ShootingPhaseIterate = new UnityEvent();
        if (ShootingPhaseIterateEnd == null) ShootingPhaseIterateEnd = new UnityEvent();
    }

    public void ProceedPhase() {
        if (phase == Phase.Movement) {
            MovementPhaseEnd.Invoke();
            phase = Phase.Shooting;
            ShootingPhaseStart.Invoke();
            GameEvents.Trigger("ShootingPhaseStart");
            shootingPhaseIteration = 0;
        } else {
            if (shootingPhaseIteration >= SHOOTING_PHASE_ITERATIONS) {
                ShootingPhaseEnd.Invoke();
                phase = Phase.Movement;
                MovementPhaseStart.Invoke();
                GameEvents.Trigger("MovementPhaseStart");
            } else {
                shootingPhaseIteration++;
                ShootingPhaseIterate.Invoke();
                StartCoroutine(AwaitShootingIterationEnd());
            }
        }
    }

    private IEnumerator AwaitShootingIterationEnd() {
        while (shootingIterationInProgress) {
            yield return null;
        }
        ShootingPhaseIterateEnd.Invoke();
    }
}

using UnityEngine;
using UnityEngine.Events;

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
    
    private int shootingPhaseIteration;
    
    public bool movement { get { return phase == Phase.Movement; } }
    public bool shooting { get { return phase == Phase.Shooting; } }
    
    void Awake() {
        phase = Phase.Movement;
        if (MovementPhaseEnd == null) MovementPhaseStart = new UnityEvent();
        if (ShootingPhaseEnd == null) ShootingPhaseStart = new UnityEvent();
        if (MovementPhaseEnd == null) MovementPhaseEnd = new UnityEvent();
        if (ShootingPhaseEnd == null) ShootingPhaseEnd = new UnityEvent();
        if (ShootingPhaseIterate == null) ShootingPhaseIterate = new UnityEvent();
    }
    
    public void ProceedPhase() {
        if (phase == Phase.Movement) {
            MovementPhaseEnd.Invoke();
            ShootingPhaseStart.Invoke();
            phase = Phase.Shooting;
            shootingPhaseIteration = 0;
        } else {
            if (shootingPhaseIteration >= SHOOTING_PHASE_ITERATIONS) {
                ShootingPhaseEnd.Invoke();
                MovementPhaseStart.Invoke();
                phase = Phase.Movement;
            } else {
                shootingPhaseIteration++;
                ShootingPhaseIterate.Invoke();
            }
        }
    }
}
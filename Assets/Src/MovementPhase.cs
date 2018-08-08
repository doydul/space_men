using System;

public class MovementPhase {
    
    public static event Action PhaseEnd;
    
    public static void TriggerPhaseEnd() {
        if (PhaseEnd != null) PhaseEnd();
    }
}
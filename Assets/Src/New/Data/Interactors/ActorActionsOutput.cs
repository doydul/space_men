namespace Data {
    
    public struct ActorActionsOutput {
        
        public ActorAction[] actions;
    }

    public struct ActorAction {

        public long index;
        public ActorActionType type;
        public Position target;
        public long actorTargetIndex;
        public bool sprint;
    }

    public enum ActorActionType {
        Move,
        Shoot,
        Turn
    }
}

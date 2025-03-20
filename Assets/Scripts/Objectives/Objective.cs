using UnityEngine;

public abstract class Objective {
    public bool required;
    public ObjectiveComponent ui;
    public abstract void Init(Objectives objectives);
    public abstract bool complete { get; }
    public abstract string description { get; }
    public abstract Vector2 targetLocation { get; }
    public virtual RoomTemplate[] specialRooms => new RoomTemplate[0];
    public virtual int extraTurns => 0;
    public int roomId { get; set; }
}
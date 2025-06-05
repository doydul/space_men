public class IgnoreAllPathingMask : IMask {
    public bool Contains(Tile tile) {
        return !tile.open;
    }
}

public class JustDoorsPathingMask : IMask {
    public bool Contains(Tile tile) {
        return !tile.open || tile.HasActor<Door>();
    }
}

public class SoldierImpassableTerrain : IMask {
    public bool Contains(Tile tile) {
        return !tile.open || tile.GetActor<Alien>() != null || tile.GetBackgroundActor<Door>() != null;
    }
}

public class SoldierLosMask : IMask {
    public bool Contains(Tile tile) {
        return !tile.open || tile.GetBackgroundActor<Door>() != null;
    }
}

public class AlienImpassableTerrain : IMask {
    public bool Contains(Tile tile) {
        return !tile.open || tile.GetActor<Soldier>() != null || tile.GetBackgroundActor<Door>() != null;
    }
}

public class AlienLosMask : IMask {
    public bool Contains(Tile tile) {
        return !tile.open || tile.GetBackgroundActor<Door>() != null;
    }
}

public class ExplosionImpassableTerrain : IMask {
    public bool Contains(Tile tile) {
        return !tile.open || tile.GetBackgroundActor<Door>() != null;
    }
}

public class AlienSpawnMask : IMask {
    public bool Contains(Tile tile) {
        return !tile.open || (tile.room != null && (tile.room.threatPriority == Map.ThreatPriority.None || tile.room.threatPriority == Map.ThreatPriority.Exempt));
    }
}
using System.Collections.Generic;

public class Explosion {

    public Tile centreTile { get; private set; }
    public List<ExplodedTile> explodedTiles { get; private set; }
    public List<CombatInstance> combatInstances { get; private set; }

    public Explosion(Tile centreTile) {
        this.centreTile = centreTile;
        explodedTiles = new List<ExplodedTile>();
        combatInstances = new List<CombatInstance>();
    }

    public void AddCombatInstance(CombatInstance combatInstance) {
        combatInstances.Add(combatInstance);
    }

    public void AddExplodedTile(Tile tile, float blastCoverage) {
        explodedTiles.Add(new ExplodedTile(tile, blastCoverage));
    }

    public class ExplodedTile {

        public Tile tile { get; private set; }
        public float blastCoverage { get; private set; }

        public ExplodedTile(Tile tile, float blastCoverage) {
            this.tile = tile;
            this.blastCoverage = blastCoverage;
        }
    }
}

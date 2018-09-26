using UnityEngine;

public class GameLogicComponent : MonoBehaviour {

    public static GameLogicComponent instance;
    public static IUserInterface userInterface { get { return instance.userInterfaceInstance; } }
    public static IWorld world { get { return instance.worldInstance; } }

    public IUserInterface userInterfaceInstance { get; private set; }
    public IWorld worldInstance { get; private set; }

    void Awake() {
        instance = this;
    }

    public void SetUserInterface(IUserInterface userInterfaceInstance) {
        this.userInterfaceInstance = userInterfaceInstance;
    }

    public void SetWorld(IWorld world) {
        worldInstance = world;
    }
}

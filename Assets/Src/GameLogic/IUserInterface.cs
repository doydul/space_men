public interface IUserInterface {

    void Select(Soldier soldier);
    void Deselect(Soldier soldier);
    void ShowAmmoIndicators(Soldier soldier);

    void ShowVictoryPopup();
}

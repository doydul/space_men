public class BtnData {
    public string label;
    public System.Action callback;
    
    public BtnData(string label, System.Action callback) {
        this.label = label;
        this.callback = callback;
    }
}
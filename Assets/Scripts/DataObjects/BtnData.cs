public class BtnData {
    public string label;
    public System.Action callback;
    
    public BtnData(string label, System.Action callback = null) {
        this.label = label;
        this.callback = callback;
    }
}
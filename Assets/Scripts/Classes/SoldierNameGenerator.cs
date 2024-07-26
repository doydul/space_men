using UnityEngine;

public class SoldierNameGenerator : MonoBehaviour {

    public TextAsset firstnamesFile;
    public TextAsset surnamesFile;

    static string[] firstnames;
    static string[] surnames;
    
    public string Generate() {
        if (firstnames == null) {
            firstnames = firstnamesFile.text.Split('\n');
            surnames = surnamesFile.text.Split('\n');
        }
        return firstnames[Random.Range(0, firstnames.Length)].Trim() + " " + surnames[Random.Range(0, surnames.Length)].Trim();
    }
}
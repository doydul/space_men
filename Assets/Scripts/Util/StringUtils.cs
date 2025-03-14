using System.Linq;
using System.Text.RegularExpressions;

public static class StringUtils {
    
    public static string RenderMoney(int value) {
        return $"{value}<voffset=0.1em>¢";
    }
    
    public static string UserFacingName(string fileName) {
        return string.Join(" ", Regex.Split(fileName, @"(?<!^)(?=[A-Z])"));
    }
}
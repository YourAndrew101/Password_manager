using System.Linq;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ServicesLibrary
{
    public class PasswordsService
    {
        public enum PasswordScore
        {
            Null = 0,
            VeryWeak = 1,
            Weak = 2,
            Medium = 3,
            Strong = 4,
            VeryStrong = 5
        }

        public static PasswordScore GetPasswordStrength(string password)
        {
            int passwordComplexity = 1;

            if (password.Length < 4) return PasswordScore.Null;
            if (password.Length < 8) return PasswordScore.VeryWeak;

            if (password.Length >= 8) passwordComplexity++;
            if (password.Length >= 16) passwordComplexity++;
            if (password.Any(char.IsDigit)) passwordComplexity++;
            if (password.Count(c => !char.IsLetterOrDigit(c)) > 0) passwordComplexity++;
            if (password.Length - password.Distinct().Count() >= password.Length / 3) passwordComplexity--;

            return (PasswordScore)passwordComplexity;
        }

        public static void SetPasswordComplexityRectangles(SolidColorBrush colorBrush, Color emptyRectanglesColor, PasswordScore passwordScore, Rectangle[] rectangles)
        {
            ClearPasswordComplexityRectangles(rectangles, emptyRectanglesColor);

            for (int i = 0; i < (int)passwordScore; i++)
                rectangles[i].Fill = colorBrush;
        }
        private static void ClearPasswordComplexityRectangles(Rectangle[] rectangles, Color color)
        {
            foreach (Rectangle item in rectangles)
                item.Fill = new SolidColorBrush(color);
        }
    }
}

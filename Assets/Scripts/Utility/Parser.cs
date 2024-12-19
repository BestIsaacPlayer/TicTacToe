using System.Text;
using Vector2 = UnityEngine.Vector2;

namespace Utility
{
    public static class Parser
    {
        public static string FieldToName(string fieldName)
        {
            if (string.IsNullOrEmpty(fieldName))
            {
                return fieldName;
            }

            fieldName = char.ToUpper(fieldName[0]) + fieldName[1..];

            var inspectorName = new StringBuilder();
            foreach (var character in fieldName)
            {
                if (char.IsUpper(character) && inspectorName.Length > 0)
                {
                    inspectorName.Append(' ');
                }

                inspectorName.Append(character);
            }
            
            return inspectorName.ToString();
        }
        
        public static Board.Result ParseWinner(Board.Cell.Content content)
        {
            return content == Board.Cell.Content.X ? Board.Result.XWon : Board.Result.OWon;
        }

        public static Direction Vector2ToDirection(Vector2 vector)
        {
            return vector.x switch
            {
                > 0 => Direction.Right,
                < 0 => Direction.Left,
                _ => vector.y > 0 ? Direction.Up : Direction.Down
            };
        }

        public static Board.Cell.Content GetOppositeSide(Board.Cell.Content content)
        {
            return content switch
            {
                Board.Cell.Content.X => Board.Cell.Content.O,
                Board.Cell.Content.O => Board.Cell.Content.X,
                _ => Board.Cell.Content.Empty
            };
        }
    }
}
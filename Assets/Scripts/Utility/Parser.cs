using System.Numerics;
using System.Text;
using Vector2 = UnityEngine.Vector2;

namespace Utility
{
    public static class Parser
    {
        /// <summary>
        /// This function takes C# field's name following the Unity's private SerializedField naming convention (camelCase) and formats it, so that the result is the field's name in the Unity's inspector (Every word is upper case and separated by a whitespace).
        /// </summary>
        /// <param name="fieldName">C# SerializedField field name (camelCase).</param>
        /// <returns>Formatted string, where all words are upper case and separated with a whitespace.</returns>
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
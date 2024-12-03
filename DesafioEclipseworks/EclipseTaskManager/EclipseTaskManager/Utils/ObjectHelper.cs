using EclipseTaskManager.Models;

namespace EclipseTaskManager.Utils
{
    public class ObjectHelper
    {
        /// <summary>
        /// This method basically compare two T objects and creates a dictionary where the key is the
        /// name of each changed property, and the value is a tuple composed by the old and new values
        /// converted to string format. If the new or the old values are null, it is converted to an
        /// empty string.
        /// </summary>
        /// <param name="original"></param>
        /// <param name="updated"></param>
        /// <returns></returns>
        public static Dictionary<string, (string OldValue, string NewValue)> DetectChanges<T>(T original, T updated)
        {
            var changes = new Dictionary<string, (string, string)>();

            var properties = typeof(T).GetProperties();
            foreach (var prop in properties)
            {
                var originalValue = prop.GetValue(original)?.ToString() ?? string.Empty;
                var updatedValue = prop.GetValue(updated)?.ToString() ?? string.Empty;

                if (!originalValue.Equals(updatedValue))
                {
                    changes.Add(prop.Name, (originalValue, updatedValue));
                }
            }

            return changes;
        }

        public static T CastToEnum<T>(int val, T defaultValue)
        {
            if (Enum.IsDefined(typeof(T), val))
            {
                return (T)Enum.ToObject(typeof(T), val);
            }
            else
            {
                return defaultValue;
            }
        }



    }
}

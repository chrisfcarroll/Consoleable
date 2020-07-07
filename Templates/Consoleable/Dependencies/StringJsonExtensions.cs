using System.Text.Json;

namespace Consoleable.Dependencies
{
    public static class StringJsonExtensions
    {
        /// <summary>Serialize <paramref name="value"/> as a Json string.</summary>
        /// <param name="value">the object to serialize</param>
        /// <param name="defaultIfSerializationError">the value to return in case there is an error during
        /// serialization</param>
        /// <typeparam name="T"></typeparam>
        /// <returns>
        /// <paramref name="value"/> serialized as a json string,
        /// or else <paramref name="defaultIfSerializationError"/> if there is an error during Serialization.
        /// </returns>
        public static string AsJsonElse<T>(this T value, string defaultIfSerializationError)
        {
            try{ return JsonSerializer.Serialize(value); }
            catch { return defaultIfSerializationError; }
        }

        /// <summary>Serialize <paramref name="value"/> as a Json string.</summary>
        /// <param name="value">the object to serialize</param>
        /// <returns>
        /// <paramref name="value"/> serialized as a json string,
        /// or else null if there is an error during Serialization.
        /// </returns>
        public static string AsJsonElseNull<T>(this T value) => AsJsonElse(value,null);
        
        /// <summary>Serialize <paramref name="value"/> as a Json string.</summary>
        /// <param name="value">the object to serialize</param>
        /// <returns>
        /// <paramref name="value"/> serialized as a json string,
        /// or else an empty string if there is an error during Serialization.
        /// </returns>
        public static string AsJsonElseEmpty<T>(this T value) => AsJsonElse(value, "");
    }
}
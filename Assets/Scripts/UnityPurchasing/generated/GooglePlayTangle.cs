// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("WnO+URff8CJDc626nip3adhrmVoBA5hC+IHS/YbS6QdXvo9XfhdBFYc1mlN1HbbJlomHpAPXvYikC6V6RObddyD9pdilbLfE5eZ6mwa817vGRUtEdMZFTkbGRUVExegxQtEgCnTGRWZ0SUJNbsIMwrNJRUVFQURH8iK52HJMOOm8FLjT7GkRVs+r5GH0qv4KsDm1WaOILWO9/GrhRCTx/X7ALwunS/AmWOPJPbLQMVqOQcq8nLjA/qLXsxLuwoXhNzjW38ZDVxKQn8iljnbQ7q4BW0p+MrSnkplaDawzASgP9XiJIsn2rW+kzgCzukD6Jt2iogQ7xWD4GEsUdfPZDgI4ZwMCklX8M0axigvjSqhEJTVe4xVt2AJhpo2m8Ipzf0ZHRURF");
        private static int[] order = new int[] { 11,3,2,12,12,11,9,11,13,12,13,11,12,13,14 };
        private static int key = 68;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}

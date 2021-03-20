namespace AndroidApp.Extensions
{
    public static class JavaExtensions
    {
        /// <summary>
        /// This only works on CLR objects that were wrapped by Android.Runtime.JavaObject in Android structures like Adapters.
        /// That's a MonoAndroid internal class with an Instance property containing the original CLR object, we're using Reflection to access it.
        /// Currently this should only be used for classes where the binding doesn't have a generic/<T/> version available in C#.
        /// </summary>
        /// <typeparam name="T">CLR object type.</typeparam>
        /// <param name="o">Android.Runtime.JavaObject wrapped object.</param>
        /// <returns></returns>
        public static T Cast<T>(this Java.Lang.Object o) => (T)o.GetType().GetProperty("Instance").GetValue(o);
    }
}

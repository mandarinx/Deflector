
namespace LunchGame01 {
    public static class IntUtils {

        public static bool WithinRange(int value, int low, int high) {
            return (uint)(value - low) <= (uint)(high - low);
        }
    }
}

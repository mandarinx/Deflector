
namespace Deflector {
    public interface IPoolable {
        void OnEnable();
        void OnDisable();
        void OnDestroy();
    }
}

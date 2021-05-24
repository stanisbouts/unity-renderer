using UnityEngine;

public class ShaderVariantTrackerComponent : MonoBehaviour
{
    public ShaderVariantCollection collection;
    public ShaderVariantTracker tracker;

#if (UNITY_EDITOR || UNITY_STANDALONE)
    private void Awake() { tracker = new ShaderVariantTracker(collection); }

    private void OnDestroy() { tracker?.Dispose(); }
#endif
}
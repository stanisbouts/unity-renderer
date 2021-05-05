using DCL;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuilderProjectsPanelLocalTest : MonoBehaviour
{
    private BuilderProjectsPanelController controller;

    void Awake()
    {
        WebRequestController.Create();

        controller = new BuilderProjectsPanelController();
    }
    void Start()
    {
        if (EventSystem.current == null)
        {
            var eventSystem = new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
        }
        controller.Initialize();
        controller.SetVisibility(true);
    }
}
using DCL.SettingsCommon.SettingsControllers.BaseControllers;
using UnityEngine;

namespace DCL.SettingsCommon.SettingsControllers.SpecificControllers
{
    [CreateAssetMenu(menuName = "Settings/Controllers/Controls/FOV", fileName = "FOVControlController")]
    public class FOVControlController : SliderSettingsControlController
    {
        public override object GetStoredValue() { return currentQualitySetting.cameraFOV; }

        public override void UpdateSetting(object newValue)
        {
            currentQualitySetting.cameraFOV = (float)newValue;
            
            SceneReferences.i.firstPersonCamera.m_Lens.FieldOfView = currentQualitySetting.cameraFOV;
        }
    }
}
namespace ScpProximityChat.SecretAPI.Settings;

public abstract class VolumeSettingBase : CustomSliderSetting
{

    protected VolumeSettingBase(int? id, string label, float defaultValue, string? hint = null)
        : base(id, label, 0, 100, defaultValue, true, "0'%'", hint: hint)
    {
    }

    public float Volume => SelectedValueInt * 0.01f;

}

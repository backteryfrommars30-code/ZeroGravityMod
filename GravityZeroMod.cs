using MelonLoader;
using UnityEngine;

[assembly: MelonInfo(typeof(GravityZeroMod.Core), "GravityZeroMod", "2.0.0", "YourName")]
[assembly: MelonGame("Omega Mega Gigal Intel", "Granny: Legacy")]

namespace GravityZeroMod
{
    public class Core : MelonMod
    {
        private MelonPreferences_Category category;
        private MelonPreferences_Entry<bool> entryEnabled;
        private MelonPreferences_Entry<float> entryX;
        private MelonPreferences_Entry<float> entryY;
        private MelonPreferences_Entry<float> entryZ;

        public override void OnInitializeMelon()
        {
            category = MelonPreferences.CreateCategory("GravityZeroMod", "Gravity Zero Mod");

            entryEnabled = category.CreateEntry(
                "Enabled", true,
                "Включить обнуление гравитации",
                "Если true — гравитация будет установлена в заданные ниже значения X/Y/Z"
            );

            entryX = category.CreateEntry("GravityX", 0f, "Gravity X");
            entryY = category.CreateEntry("GravityY", 0f, "Gravity Y");
            entryZ = category.CreateEntry("GravityZ", 0f, "Gravity Z");

            ApplyGravityFromPrefs();

            entryEnabled.OnEntryValueChanged.Subscribe((oldVal, newVal) => ApplyGravityFromPrefs());
            entryX.OnEntryValueChanged.Subscribe((oldVal, newVal) => ApplyGravityFromPrefs());
            entryY.OnEntryValueChanged.Subscribe((oldVal, newVal) => ApplyGravityFromPrefs());
            entryZ.OnEntryValueChanged.Subscribe((oldVal, newVal) => ApplyGravityFromPrefs());

            LoggerInstance.Msg("GravityZeroMod загружен. Настройки в UserData/MelonPreferences.cfg, секция [GravityZeroMod].");
        }

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            ApplyGravityFromPrefs();
        }

        public override void OnUpdate()
        {
            if (entryEnabled.Value)
            {
                Vector3 target = new Vector3(entryX.Value, entryY.Value, entryZ.Value);
                if (Physics.gravity != target)
                    Physics.gravity = target;
            }
        }

        private void ApplyGravityFromPrefs()
        {
            if (entryEnabled.Value)
            {
                Physics.gravity = new Vector3(entryX.Value, entryY.Value, entryZ.Value);
                LoggerInstance.Msg($"Гравитация установлена в ({entryX.Value}, {entryY.Value}, {entryZ.Value})");
            }
            else
            {
                Physics.gravity = new Vector3(0f, -9.81f, 0f);
                LoggerInstance.Msg("Гравитация возвращена к стандартной");
            }
        }
    }
}

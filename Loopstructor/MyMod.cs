using ActFramework_ByHZR.BasicUtil;
using ActFramework_ByHZR.MainLoop;
using HarmonyLib;
using MetroTD.RewardSystem;
using MetroTD.VehicleSystem;
using UnityEngine;

namespace Loopstructor
{
    public class MyMod : MonoBehaviour
    {
        public bool isFlag = false;

        public class MyCustom
        {
            public static bool isMoreExp = false;
            public static bool isMoreVehicle = false;
        }

        public Rect MyWindow { get; set; }

        private void Awake()
        {
            isFlag = true;
        }

        private void Start()
        {
            new Harmony("MyPlugin").PatchAll();
        }

        private void Update()
        {
        }

        public void OnGUI()
        {
            if (!isFlag)
            {
                return;
            }

            MyWindow = GUILayout.Window(0, MyWindow, MyGUI, "MOD", new GUILayoutOption[0]);
        }


        public void MyGUI(int windowID)
        {
            GUI.skin.label.alignment = TextAnchor.MiddleCenter;
            GUI.skin.button.alignment = TextAnchor.MiddleCenter;
            GUILayoutOption[] buttonOption = new GUILayoutOption[2] { GUILayout.Width(200f), GUILayout.Height(100f) };
            GUILayout.BeginVertical(new GUILayoutOption[0]);

            if (GUILayout.Button("更多经验\nMoreExp", MyGUIStyle(MyCustom.isMoreExp), buttonOption))
            {
                MyCustom.isMoreExp = !MyCustom.isMoreExp;
            }
            if (GUILayout.Button("三倍列车\nMoreVehicle", MyGUIStyle(MyCustom.isMoreVehicle), buttonOption))
            {
                MyCustom.isMoreVehicle = !MyCustom.isMoreVehicle;
            }

            GUILayout.EndVertical();
            GUI.DragWindow(new Rect(0f, 0f, (float)Screen.width, (float)Screen.height));
        }

        public GUIStyle MyGUIStyle(bool flag)
        {
            GUIStyle style = new GUIStyle(GUI.skin.button);
            style.fontSize = 30;
            if (flag)
            {
                style.normal.textColor = Color.red;
            }

            return style;
        }

        //多倍经验
        [HarmonyPatch(typeof(CurrencyExpManager), nameof(CurrencyExpManager.AddExpInParameter))]
        public class MoreExp
        {
            [HarmonyPrefix]
            static void Prefix(CurrencyExpManager __instance, ref float expAdder)
            {
                if (MyCustom.isMoreExp)
                {
                    expAdder *= 10;
                }
            }
        }

        //多倍列车
        [HarmonyPatch(typeof(RazorReward), nameof(RazorReward.TryGet))]
        public class MoreTrain
        {
            [HarmonyPrefix]
            static void Prefix(RazorReward __instance)
            {
                if (MyCustom.isMoreVehicle)
                {
                    BasicOdinGameManager<Main, IMainLoader>.instance.GetModule<VehicleManager>().GetNewMainRazor(__instance.vehicleType);
                    BasicOdinGameManager<Main, IMainLoader>.instance.GetModule<VehicleManager>().GetNewMainRazor(__instance.vehicleType);
                }
            }
        }
    }
}
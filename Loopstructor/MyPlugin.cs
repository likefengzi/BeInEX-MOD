using System;
using System.Text;
using BepInEx;

namespace Loopstructor
{
    [BepInPlugin("MyPlugin", "MyPlugin", "1.0.0.0")]
    public class MyPlugin:BaseUnityPlugin
    {
        private void Start()
        {
            //控制台可以使用中文
            Console.OutputEncoding = Encoding.UTF8;
            gameObject.AddComponent<MyMod>();
        }
    }
}
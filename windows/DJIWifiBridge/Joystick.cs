using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.DirectInput;
using System.Threading;

namespace DJIWifiBridge
{
    class MyJoystick
    {
        static DirectInput directInput = new DirectInput();

        public static List<DeviceInstance> GetJoysticks() {
            List<DeviceInstance> instances = new List<DeviceInstance>();
            var gamepads = directInput.GetDevices(DeviceType.Gamepad, DeviceEnumerationFlags.AllDevices);
            instances.AddRange(gamepads);
            var joysticks = directInput.GetDevices(DeviceType.Joystick, DeviceEnumerationFlags.AllDevices);
            instances.AddRange(joysticks);
            return instances;
        }
        Joystick joystick;
        Thread pollThread;
        bool running = false;
        public delegate void JoystickEventHandler(object sender, JoystickEventArgs e);
        public event JoystickEventHandler JoystickEvent;
        public MyJoystick(Guid guid) {
            joystick = new Joystick(directInput, guid);
            joystick.Properties.BufferSize = 128;
            joystick.Acquire();
            pollThread = new Thread(PollThread);
            running = true;
            pollThread.Start();

        }

        IList<EffectInfo> GetEffects() {
            return joystick.GetEffects();
        }

        public void Stop() {
            running = false;
            pollThread.Abort();
            joystick.Dispose();
        }

        void PollThread() {
            while (running)
            {
                joystick.Poll();
                var datas = joystick.GetBufferedData();
                JoystickEvent(this, new JoystickEventArgs(datas));
            }
        }

    }

    class JoystickEventArgs : EventArgs {
        public JoystickUpdate[] JoystickUpdates;
        public JoystickEventArgs(JoystickUpdate[] updates) {
            this.JoystickUpdates = updates;
        }
    }
}

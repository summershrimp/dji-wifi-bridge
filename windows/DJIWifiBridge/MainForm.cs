using SharpDX.DirectInput;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DJIWifiBridge
{
    public partial class MainForm : Form
    {
        WifiBridge wifiBridge;
        MyJoystick joystick, throttle;
        System.Timers.Timer timer;
        IList<DeviceInstance> joystickList;

        public MainForm()
        {
            InitializeComponent();
            wifiBridge = WifiBridge.getInstance();
            joystickList = MyJoystick.GetJoysticks();
            List<String> joystickStirngs = new List<String>();
            FormClosing += MainForm_FormClosing;
            timer = new System.Timers.Timer(60);
            timer.Elapsed += Timer_Elapsed;
            timer.AutoReset = true;
            timer.Start();
            foreach (var j in joystickList)
            {
                joystickStirngs.Add(j.InstanceName);
                JoystickCombo.Items.Add(j.InstanceName);
                ThrottleCombo.Items.Add(j.InstanceName);
            }
        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            wifiBridge.sendRCData(roll, pitch, yaw, thr);
            wifiBridge.sendGimbalSpeed(gimbalPitch, gimbalRoll, gimbalYaw);
            //Console.WriteLine(String.Format("Gimbal: {0} {1} {2}", gimbalRoll, gimbalPitch, gimbalYaw));
            //Console.WriteLine(String.Format("RC: {0} {1} {2} {3}", roll, pitch, yaw, thr));
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            timer.Stop();
            if(joystick != null)
                joystick.Stop();
            if(throttle != null )
                throttle.Stop();
            wifiBridge.Disconnect();
        }

        private void JoystickCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (joystick != null)
                joystick.Stop();
            joystick = new MyJoystick(joystickList[JoystickCombo.SelectedIndex].InstanceGuid);
            joystick.JoystickEvent += Joystick_JoystickEvent;
        }

        private void takeoff_Click(object sender, EventArgs e)
        {
            wifiBridge.takeOff();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            wifiBridge.landing();
        }

        private void ThrottleCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (throttle != null)
                throttle.Stop();
            throttle = new MyJoystick(joystickList[ThrottleCombo.SelectedIndex].InstanceGuid);
            throttle.JoystickEvent += Throttle_JoystickEvent;
        }

        private void wifiConnect_Click(object sender, EventArgs e)
        {
            if (IP.Enabled)
            {
                try
                {
                    string[] wifi = this.IP.Text.Split(':');
                    wifiBridge.Connect(wifi[0], int.Parse(wifi[1]));
                    wifiConnect.Enabled = true;
                    wifiConnect.Text = "断开";
                    IP.Enabled = false;
                    timer.Start();
                }
                catch (Exception)
                {
                    MessageBox.Show("错误", "连接失败");
                }
            }
            else
            {
                wifiBridge.Disconnect();
                timer.Stop();
                wifiConnect.Enabled = true;
                wifiConnect.Text = "连接";
                IP.Enabled = true;
            }
        }


        float gimbalYaw = 0, gimbalPitch = 0, gimbalRoll = 0;
        float yaw = 0, roll = 0, pitch = 0, thr = 0;

        private void Joystick_JoystickEvent(object sender, JoystickEventArgs e)
        {
            foreach (var j in e.JoystickUpdates)
            {
                if (j.Offset.Equals(JoystickOffset.PointOfViewControllers0))
                {
                    dealGimbalYawPitch(j.Value);
                }
                else if (j.Offset.Equals(JoystickOffset.X))
                {
                    roll = (j.Value - 32768) / 10000.0f;
                }
                else if (j.Offset.Equals(JoystickOffset.Y))
                {
                    pitch = (j.Value - 32768) / 10000.0f;
                }
                else if (j.Offset.Equals(JoystickOffset.Buttons0))
                {
                    if (j.Value == 128)
                    {
                        wifiBridge.takePhoto();
                    }
                }
                else if (j.Offset.Equals(JoystickOffset.Buttons11))
                {
                    if (j.Value == 128)
                    {
                        yaw = 5;
                    }
                    else
                    {
                        yaw = 0;
                    }
                }
                else if (j.Offset.Equals(JoystickOffset.Buttons13))
                {
                    if (j.Value == 128)
                    {
                        yaw = -5;
                    }
                    else
                    {
                        yaw = 0;
                    }
                }
                else if (j.Offset.Equals(JoystickOffset.Buttons7))
                {
                    if (j.Value == 128)
                    {
                        gimbalRoll = GimbalSpeed;
                    }
                    else
                    {
                        gimbalRoll = 0;
                    }
                }
                else if (j.Offset.Equals(JoystickOffset.Buttons9))
                {
                    if (j.Value == 128)
                    {
                        gimbalRoll = -GimbalSpeed;
                    }
                    else
                    {
                        gimbalRoll = 0;
                    }
                }
                else if (j.Offset.Equals(JoystickOffset.Buttons18))
                {
                    if (j.Value == 128)
                    {
                        wifiBridge.resetGimbal();
                    }
                }
                //Console.WriteLine(j.Offset.ToString() + ":" + j.Value);
            }
        }
        const int GimbalSpeed = 20;

        private void Throttle_JoystickEvent(object sender, JoystickEventArgs e)
        {
            foreach (var j in e.JoystickUpdates)
            {

                if (j.Offset.Equals(JoystickOffset.Buttons31))
                {
                    if (j.Value == 128)
                    {
                        wifiBridge.takeOff();
                    }
                }
                else if (j.Offset.Equals(JoystickOffset.Buttons20))
                {
                    if (j.Value == 128)
                    {
                        wifiBridge.landing();
                    }
                }
                else if (j.Offset.Equals(JoystickOffset.Sliders0))
                {
                    thr = (32768 - j.Value) / 10000.0f;
                }
                else if (j.Offset.Equals(JoystickOffset.Buttons26))
                {
                    if (j.Value == 128)
                    {
                        wifiBridge.enableRC();
                    }
                    else
                    {
                        wifiBridge.disableRC();
                    }
                }
                //Console.WriteLine(j.Offset.ToString() + ":" + j.Value);
            }
        }

        private void dealGimbalYawPitch(int value)
        {
            switch (value)
            {
                case -1:
                    gimbalPitch = gimbalYaw = 0;
                    break;
                case 0:
                    gimbalPitch = GimbalSpeed;
                    gimbalYaw = 0;
                    break;
                case 4500:
                    gimbalPitch = gimbalYaw = GimbalSpeed;
                    break;
                case 9000:
                    gimbalPitch = 0;
                    gimbalYaw = GimbalSpeed;
                    break;
                case 13500:
                    gimbalPitch = -GimbalSpeed;
                    gimbalYaw = GimbalSpeed;
                    break;
                case 18000:
                    gimbalPitch = -GimbalSpeed;
                    gimbalYaw = 0;
                    break;
                case 22500:
                    gimbalYaw = -GimbalSpeed;
                    gimbalPitch = -GimbalSpeed;
                    break;
                case 27000:
                    gimbalYaw = -GimbalSpeed;
                    gimbalPitch = 0;
                    break;
                case 31500:
                    gimbalYaw = -GimbalSpeed;
                    gimbalPitch = GimbalSpeed;
                    break;
            }
        }
    }
}

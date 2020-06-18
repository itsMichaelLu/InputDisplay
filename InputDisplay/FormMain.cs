using SharpDX.DirectInput;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InputDisplay
{
    public partial class FormMain : Form
    {

        private FormDisplay _formChild;
        DirectInput directInput = new DirectInput();
        Thread pollThread;
        public Joystick joystick;
        Dictionary<Guid, String> devices = new Dictionary<Guid, String>();

        public FormMain()
        {
            InitializeComponent();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            _formChild = new FormDisplay(this);
            _formChild.Hide();
        }

        private void joystickPoll()
        {
            while (true)
            {
                joystick.Poll();
            }
        }

        public JoystickState getJoyState()
        {
            if (joystick != null)
            {
                return joystick.GetCurrentState();
            }
            return null;
        }

        private void buttonReloadlist_Click(object sender, EventArgs e)
        {
            devices.Clear();

            foreach (var deviceInstance in directInput.GetDevices(DeviceType.Gamepad, DeviceEnumerationFlags.AllDevices))
            {
                devices.Add(deviceInstance.InstanceGuid, deviceInstance.InstanceName);
            }

            foreach (var deviceInstance in directInput.GetDevices(DeviceType.Joystick, DeviceEnumerationFlags.AllDevices))
            {
                devices.Add(deviceInstance.InstanceGuid, deviceInstance.InstanceName);
            }

            comboBoxInputList.Items.Clear();
            comboBoxInputList.Items.AddRange(devices.Values.ToArray<string>());

            if (comboBoxInputList.Items.Count > 0)
            {
                comboBoxInputList.SelectedIndex = 0;
            }
            //comboBoxInputList.Items.AddRange(devices.Keys.Select(x => x.ToString()).ToArray<string>());
            }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            if (comboBoxInputList.SelectedIndex < 0)
            {
                labelConnectedDevice.Text = "None";
                return;
            }


            // If its already running, kill it and reattach
            if (pollThread != null && pollThread.IsAlive)
            {
                pollThread.Abort();
                joystick.Unacquire();
                joystick.Dispose();
            }

            joystick = new Joystick(directInput, devices.FirstOrDefault(x => x.Value == comboBoxInputList.SelectedItem.ToString()).Key);

            var allEffects = joystick.GetEffects();
            foreach (var effectInfo in allEffects)
                Console.WriteLine("Effect available {0}", effectInfo.Name);

            joystick.Properties.BufferSize = 128;
            joystick.Acquire();

            labelConnectedDevice.Text = comboBoxInputList.SelectedItem.ToString();

            pollThread = new Thread(joystickPoll);
            pollThread.IsBackground = true;
            pollThread.Start();
        }

        private void checkBoxShowDisplay_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxShowDisplay.Checked)
            {
                _formChild.Show();
            }
            else
            {
                _formChild.Hide();
            }
        }
    }
}

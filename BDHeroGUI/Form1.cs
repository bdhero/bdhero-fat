using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BDHero;
using BDHero.Plugin;

namespace BDHeroGUI
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// IMPORTANT: This must be the absolute FIRST line of code that runs to initialize logging!
        /// </summary>
        private IController _controller = new Controller("bdhero-gui.log.config");

        /// <summary>
        /// Depends on <see cref="Controller"/> being initialized first.
        /// </summary>
//        private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public Form1()
        {
            InitializeComponent();
            Load += OnLoad;
        }

        private void OnLoad(object sender, EventArgs eventArgs)
        {
            _controller.ScanStarted += ControllerOnScanStarted;
            _controller.ScanSucceeded += ControllerOnScanSucceeded;
            _controller.ScanFailed += ControllerOnScanFailed;
            _controller.ScanCompleted += ControllerOnScanCompleted;

            _controller.ConvertStarted += ControllerOnConvertStarted;
            _controller.ConvertSucceeded += ControllerOnConvertSucceeded;
            _controller.ConvertFailed += ControllerOnConvertFailed;
            _controller.ConvertCompleted += ControllerOnConvertCompleted;

            _controller.PluginProgressUpdated += ControllerOnPluginProgressUpdated;
            
            _controller.LoadPlugins();
            _controller.SetEventScheduler();
        }

        private void EnableControls(bool enabled)
        {
            textBoxInput.Enabled = enabled;
            textBoxOutput.Enabled = enabled;
            buttonMux.Enabled = enabled;
        }

        private void AppendStatus(string statusLine = null)
        {
            textBoxStatus.Text = (statusLine ?? string.Empty);
        }

        private void ControllerOnScanStarted(object sender, EventArgs eventArgs)
        {
            textBoxStatus.Text = "Scan started...";
            EnableControls(false);
        }

        private void ControllerOnScanSucceeded(object sender, EventArgs eventArgs)
        {
            AppendStatus("Scan succeeded!");
        }

        private void ControllerOnScanFailed(object sender, EventArgs eventArgs)
        {
            AppendStatus("Scan failed!");
        }

        private void ControllerOnScanCompleted(object sender, EventArgs eventArgs)
        {
            AppendStatus("Scan completed!");
            EnableControls(true);
        }

        private void ControllerOnConvertStarted(object sender, EventArgs eventArgs)
        {
            AppendStatus("Convert started...");
            EnableControls(false);
        }

        private void ControllerOnConvertSucceeded(object sender, EventArgs eventArgs)
        {
            AppendStatus("Convert succeeded!");
        }

        private void ControllerOnConvertFailed(object sender, EventArgs eventArgs)
        {
            AppendStatus("Convert failed!");
        }

        private void ControllerOnConvertCompleted(object sender, EventArgs eventArgs)
        {
            AppendStatus("Convert completed!");
            EnableControls(true);
        }

        private void ControllerOnPluginProgressUpdated(IPlugin plugin, ProgressProvider progressProvider)
        {
            var line = string.Format("{0} is {1} - {2} complete - {3} - {4} elapsed, {5} remaining",
                                     plugin.Name, progressProvider.State, (progressProvider.PercentComplete / 100.0).ToString("P"),
                                     progressProvider.Status,
                                     progressProvider.RunTime, progressProvider.TimeRemaining);
            AppendStatus(line);
            progressBar.Value = (int) progressProvider.PercentComplete * 1000;
        }

        private void buttonMux_Click(object sender, EventArgs e)
        {
            var scanTask = _controller.CreateScanTask(textBoxInput.Text, textBoxOutput.Text);
            var convertTask = _controller.CreateConvertTask();

            scanTask.ContinueWith(task => convertTask.Start());
            scanTask.Start();
        }
    }
}

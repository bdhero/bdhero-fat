using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BDHero;
using BDHero.Plugin;
using BDHero.Startup;

namespace BDHeroGUI
{
    public partial class FormAsyncControllerTest : Form
    {
        private readonly log4net.ILog _logger;
        private readonly IDirectoryLocator _directoryLocator;
        private readonly PluginLoader _pluginLoader;
        private readonly IController _controller;

        public FormAsyncControllerTest(IDirectoryLocator directoryLocator, PluginLoader pluginLoader, IController controller)
        {
            InitializeComponent();

            _logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            _directoryLocator = directoryLocator;
            _pluginLoader = pluginLoader;
            _controller = controller;

            Load += OnLoad;
        }

        private void OnLoad(object sender, EventArgs eventArgs)
        {
            LogDirectoryPaths();
            LoadPlugins();
            LogPlugins();
            InitController();
        }

        private void LogDirectoryPaths()
        {
            _logger.InfoFormat("IsPortable = {0}", _directoryLocator.IsPortable);
            _logger.InfoFormat("InstallDir = {0}", _directoryLocator.InstallDir);
            _logger.InfoFormat("ConfigDir = {0}", _directoryLocator.ConfigDir);
            _logger.InfoFormat("PluginDir = {0}", _directoryLocator.PluginDir);
            _logger.InfoFormat("LogDir = {0}", _directoryLocator.LogDir);
        }

        private void LoadPlugins()
        {
            _pluginLoader.LoadPlugins();
        }

        private void LogPlugins()
        {
            _pluginLoader.LogPlugins();
        }

        private void InitController()
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
            progressBar.Value = (int) (progressProvider.PercentComplete * 1000d);
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

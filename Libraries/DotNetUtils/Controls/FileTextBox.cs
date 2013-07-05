using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DotNetUtils.Extensions;

namespace DotNetUtils.Controls
{
    public partial class FileTextBox : UserControl
    {
        /// <summary>
        /// Gets or sets the value of the TextBox.  Alias of <see cref="SelectedPath"/>.
        /// </summary>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override string Text
        {
            get { return SelectedPath; }
            set { SelectedPath = value; }
        }

        /// <summary>
        /// Gets or sets the value of the TextBox.  Alias of <see cref="Text"/>.
        /// </summary>
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public string SelectedPath
        {
            get { return textBoxPath.Text; }
            set { textBoxPath.Text = value; }
        }

        /// <summary>
        /// Gets or sets the label of the "Browse" button.
        /// </summary>
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DefaultValue("Browse...")]
        public string ButtonText
        {
            get { return buttonBrowse.Text; }
            set { buttonBrowse.Text = value; }
        }

        /// <summary>
        /// Gets or sets the type of dialog that will be displayed when the user clicks the "Browse" button.
        /// </summary>
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DefaultValue(DialogType.OpenFile)]
        public DialogType DialogType { get; set; }

        /// <summary>
        /// Gets or sets the text that will be displayed in the dialog's title.
        /// </summary>
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DefaultValue("Select a file:")]
        public string DialogTitle { get; set; }

        /// <summary>
        /// Gets or sets whether the "New folder" button will be visible in the dialog.
        /// Only applies when <see cref="DialogType"/> is set to <see cref="Controls.DialogType.OpenDirectory"/>.
        /// </summary>
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DefaultValue(true)]
        public bool ShowNewFolderButton { get; set; }

        /// <summary>
        /// Gets or sets whether the user can select multiple files or directories.
        /// Only applies when <see cref="DialogType"/> is set to <see cref="Controls.DialogType.OpenFile"/> or <see cref="Controls.DialogType.OpenFileOrDirectory"/>.
        /// </summary>
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DefaultValue(false)]
        public bool MultiSelect { get; set; }

        /// <summary>
        /// Gets or sets whether the user should be prompted to overwrite an existing file.
        /// Only applies when <see cref="DialogType"/> is set to <see cref="Controls.DialogType.SaveFile"/>.
        /// </summary>
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DefaultValue(true)]
        public bool OverwritePrompt { get; set; }

        /// <summary>
        /// Gets or sets the BorderStyle of the TextBox.
        /// </summary>
        public new BorderStyle BorderStyle
        {
            get { return textBoxPath.BorderStyle; }
            set { textBoxPath.BorderStyle = value; }
        }

        /// <summary>
        /// Triggered whenever the value of the TextBox changes.
        /// </summary>
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public new event EventHandler TextChanged;

        /// <summary>
        /// Triggered when the <see cref="SelectedPath"/> changes after the user selects a new path
        /// from the dialog box and presses "Select" (or "Save").
        /// </summary>
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public event EventHandler SelectedPathChanged;

        /// <summary>
        /// Constructs a new FileTextBox component.
        /// </summary>
        public FileTextBox()
        {
            InitializeComponent();
            textBoxPath.TextChanged += (sender, args) => OnTextChanged(args);
            buttonBrowse.Click += ShowDialog;
        }

        /// <summary>
        /// Highlights the TextBox by changing its <see cref="BorderStyle"/>.
        /// </summary>
        public void Highlight()
        {
            textBoxPath.Highlight();
        }

        /// <summary>
        /// Removes highlighting from the TextBox by reverting its <see cref="BorderStyle"/> back to its original value.
        /// </summary>
        public void UnHighlight()
        {
            textBoxPath.UnHighlight();
        }

        protected override void OnTextChanged(EventArgs e)
        {
            if (TextChanged != null)
                TextChanged(this, e);
        }

        private void OnSelectedPathChanged(EventArgs e)
        {
            if (SelectedPathChanged != null)
                SelectedPathChanged(this, e);
        }

        private void ShowDialog(object sender, EventArgs e)
        {
            var dialog = CreateDialog();

            if (!string.IsNullOrEmpty(SelectedPath))
            {
                dialog.SelectedPath = SelectedPath;
            }

            if (dialog.ShowDialog() != DialogResult.OK) return;

            var prevPath = SelectedPath;
            SelectedPath = dialog.SelectedPath;

            if (prevPath != SelectedPath)
                OnSelectedPathChanged(EventArgs.Empty);
        }

        private IDialog CreateDialog()
        {
            switch (DialogType)
            {
                case DialogType.OpenFile:
                    return new OpenFileDialog2
                        {
                            Title = DialogTitle
                        };
                case DialogType.SaveFile:
                    return new SaveFileDialog2
                        {
                            OverwritePrompt = OverwritePrompt,
                            Title = DialogTitle
                        };
                default:
                    return new FolderBrowserDialog2
                        {
                            Title = DialogTitle,
                            ShowNewFolderButton = ShowNewFolderButton
                        };
            }
        }
    }

    interface IDialog
    {
        string Title { get; set; }
        string SelectedPath { get; set; }
        DialogResult ShowDialog();
        DialogResult ShowDialog(IWin32Window owner);
    }

    class OpenFileDialog2 : IDialog
    {
        private readonly OpenFileDialog _dialog = new OpenFileDialog();

        public OpenFileDialog2()
        {
            _dialog.AutoUpgradeEnabled = true;
            _dialog.CheckPathExists = true;
            _dialog.DereferenceLinks = true;
            _dialog.Multiselect = false;
        }

        public string Title
        {
            get { return _dialog.Title; }
            set { _dialog.Title = value; }
        }

        public string SelectedPath
        {
            get { return _dialog.FileName; }
            set { _dialog.FileName = value; }
        }

        public DialogResult ShowDialog()
        {
            return _dialog.ShowDialog();
        }

        public DialogResult ShowDialog(IWin32Window owner)
        {
            return _dialog.ShowDialog(owner);
        }
    }

    class SaveFileDialog2 : IDialog
    {
        private readonly SaveFileDialog _dialog = new SaveFileDialog();

        public SaveFileDialog2()
        {
            _dialog.AutoUpgradeEnabled = true;
            _dialog.CheckPathExists = true;
            _dialog.DereferenceLinks = true;
            _dialog.Title = Title;
        }

        public bool OverwritePrompt
        {
            get { return _dialog.OverwritePrompt; }
            set { _dialog.OverwritePrompt = value; }
        }

        public string Title
        {
            get { return _dialog.Title; }
            set { _dialog.Title = value; }
        }

        public string SelectedPath
        {
            get { return _dialog.FileName; }
            set
            {
                _dialog.InitialDirectory = Path.GetDirectoryName(value);
                _dialog.FileName = Path.GetFileName(value);
            }
        }

        public DialogResult ShowDialog()
        {
            return _dialog.ShowDialog();
        }

        public DialogResult ShowDialog(IWin32Window owner)
        {
            return _dialog.ShowDialog(owner);
        }
    }

    class FolderBrowserDialog2 : IDialog
    {
        private readonly FolderBrowserDialog _dialog = new FolderBrowserDialog();

        public bool ShowNewFolderButton
        {
            get { return _dialog.ShowNewFolderButton; }
            set { _dialog.ShowNewFolderButton = value; }
        }

        public string Title
        {
            get { return _dialog.Description; }
            set { _dialog.Description = value; }
        }

        public string SelectedPath
        {
            get { return _dialog.SelectedPath; }
            set { _dialog.SelectedPath = value; }
        }

        public DialogResult ShowDialog()
        {
            return _dialog.ShowDialog();
        }

        public DialogResult ShowDialog(IWin32Window owner)
        {
            return _dialog.ShowDialog(owner);
        }
    }

    public enum DialogType
    {
        OpenFile,
        SaveFile,
        OpenDirectory
    }
}

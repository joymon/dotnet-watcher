using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JoymonsCode.DotNetWatcher.Core;

namespace JoymonsCode.DotNetWatcher.ViewModel
{
    public class SelectorViewModel : ViewModelBase
    {
        #region "Properties"
        
        private string _filePath;

        public string FilePath
        {
            get { return _filePath; }
            set
            {
                _filePath = value;
                base.OnPropertyChanged<string>(() => FilePath);
            }
        }
        private DelegateCommand _StartCommand;

        public DelegateCommand StartCommand
        {
            get { return _StartCommand; }
            set
            {
                _StartCommand = value;
                base.OnPropertyChanged<DelegateCommand>(() => StartCommand);
            }
        }
        #endregion

#region Events
        public event EventHandler Selected;
#endregion

        #region "Constructor"
        public SelectorViewModel()
        {
            Initialize();
        }
        #endregion

        #region "Methods"
        private void Initialize()
        {
            InitializeCommands(); 
            
        }

        private void InitializeCommands()
        {
            this.StartCommand = new DelegateCommand(
                () =>
                {
                    if (!string.IsNullOrWhiteSpace(this.FilePath))
                    {
                        OnFileSelected(EventArgs.Empty);
                    }
                });
        }
        protected virtual void OnFileSelected(EventArgs args)
        {
            if (Selected != null)
            {
                Selected(this, args);
            }
        }
        #endregion
    }
}

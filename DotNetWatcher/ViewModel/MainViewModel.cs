using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JoymonsCode.DotNetWatcher.Core;

namespace JoymonsCode.DotNetWatcher.ViewModel
{
    public class MainViewModel:ViewModelBase
    {
        #region Properties
        private SelectorViewModel m;

        public SelectorViewModel SelectorViewModel
        {
            get { return m; }
            set
            {
                m = value;
                OnPropertyChanged(() => this.SelectorViewModel);
            }
        }
        private ResultViewModel myVar;

        public ResultViewModel ResultViewModel
        {
            get { return myVar; }
            set
            {
                myVar = value;
                OnPropertyChanged<ResultViewModel>(() => this.ResultViewModel);
            }
        }
        
        #endregion
        #region Constructor
        public MainViewModel()
        {
            Initialize();
        }
#endregion
        #region Methods
        
        private void Initialize()
        {
            this.SelectorViewModel = new SelectorViewModel();
            this.SelectorViewModel.Selected += new EventHandler(SelectorViewModel_Selected);
            this.ResultViewModel = new ResultViewModel();
        }

        void SelectorViewModel_Selected(object sender, EventArgs e)
        {
            Processor.Instance.Attach(new string[] { SelectorViewModel.FilePath }, ResultViewModel.ReceiveNotification);
        }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolBoxNET.MVVM.Base;

namespace Oblivion.Utility.Navigator
{
    public class StackNavigator : ViewModelBase
    {
        private StackNavigator _CurrentView;
        public StackNavigator CurrentView
        {
            get { return _CurrentView;  }
            set { _CurrentView = value; RaisePropertyChanged(); }
        }

        public void Navigate(StackNavigator view)
        {
            CurrentView = view;
        }
    }
}

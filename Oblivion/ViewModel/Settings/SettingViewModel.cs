using MahApps.Metro.Controls;
using Oblivion.Model.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ToolBoxNET.MVVM.Base;
using ToolBoxNET.MVVM.Command;

namespace Oblivion.ViewModel.Settings
{
    public class SettingViewModel : ViewModelBase
    {
        private string _PathDofus;

        public string PathDofus
        {
            get { return _PathDofus; }
            set { _PathDofus = value; }
        }

        private string _AccountName;

        public string AccountName
        {
            get { return _AccountName; }
            set { _AccountName = value; }
        }

        private string _Password;

        public string Password
        {
            get { return _Password; }
            set { _Password = value; }
        }

        public bool CanHide { get; set; }

        private ICommand _SaveButton;
        public ICommand SaveButton
        {
            get { return _SaveButton ?? (_SaveButton = new RelayCommandParam(window => SaveConfig(window))); }
        }

        public SettingViewModel()
        {

        }

        /// <summary>
        /// Save config in file .json
        /// </summary>
        /// <param name="window"></param>
        private void SaveConfig(object window)
        {
            LoginConfig config = new LoginConfig(AccountName, Password, PathDofus);
            config.UpdateData();

            if (window is Window)
            {
                (window as Window).Close();
            }
        }

        public SettingViewModel(LoginConfig config)
        {
            PathDofus = config.PathDofus;
            AccountName = config.Username;
            Password = config.Password;
            CanHide = false;
        }
    }
}

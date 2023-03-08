using SotFSaveManager.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SotFSaveManager.MVVM.Model
{
    public class ObservableBoolean : ObservableObject
    {
        private bool _prop;

        public bool Property
        {
            get { return _prop; }
            set { _prop = value; OnPropertyChanged(); }
        }

        public ObservableBoolean(bool init = false)
        {
            Property = init;
        }

        public override string ToString()
        {
            return Property.ToString();
        }

    }
}

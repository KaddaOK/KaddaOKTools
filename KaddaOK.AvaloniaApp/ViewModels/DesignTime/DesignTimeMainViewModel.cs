using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaddaOK.AvaloniaApp.ViewModels.DesignTime
{
    public class DesignTimeMainViewModel : MainViewModel
    {
        public DesignTimeMainViewModel() : base(DesignTimeKaraokeProcess.Get())
        {
        }
    }
}

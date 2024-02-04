using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KaddaOK.AvaloniaApp.Models;
using KaddaOK.Library;

namespace KaddaOK.AvaloniaApp.ViewModels.DesignTime
{
    public class DesignTimeExportViewModel : ExportViewModel
    {
        public DesignTimeExportViewModel() 
            : base(DesignTimeKaraokeProcess.Get(), null!, null!, null!, null!, null!)
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KaddaOK.AvaloniaApp
{
    public class Debouncer
    {
        private CancellationTokenSource? _cancelTokenSource = null;

        public async Task Debounce(Func<Task> method, int milliseconds = 300)
        {
            _cancelTokenSource?.Cancel();
            _cancelTokenSource?.Dispose();

            _cancelTokenSource = new CancellationTokenSource();

            await Task.Delay(milliseconds, _cancelTokenSource.Token);

            await method();
        }
    }
}

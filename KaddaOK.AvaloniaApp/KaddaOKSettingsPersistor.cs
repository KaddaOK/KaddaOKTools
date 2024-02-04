using Newtonsoft.Json;
using System.IO;
using KaddaOK.AvaloniaApp.Models;

namespace KaddaOK.AvaloniaApp
{
    public interface IKaddaOKSettingsPersistor
    {
        KaddaOKSettings LoadState();
        void SaveState(KaddaOKSettings? state);
    }
    public class KaddaOKSettingsPersistor : IKaddaOKSettingsPersistor
    {
        private readonly string filename = "KaddaOKSettings.json";

        public KaddaOKSettings LoadState()
        {
            if (File.Exists(filename))
            {
                var lines = File.ReadAllText(filename);
                return JsonConvert.DeserializeObject<KaddaOKSettings>(lines) ?? new KaddaOKSettings();
            }

            return new KaddaOKSettings();
        }

        public void SaveState(KaddaOKSettings? state)
        {
            if (state != null)
            {
                var lines = JsonConvert.SerializeObject(state);
                File.WriteAllText(filename, lines);
            }
        }
    }
}

using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;

namespace WpfProcessTree
{
    public class PlaySound
    {
        public const string REG_SMS = @"AppEvents\Schemes\Apps\.Default\Notification.SMS\.Current";
        public const string REG_NFC_CONN = @"AppEvents\Schemes\Apps\.Default\ProximityConnection\.Current";
        public const string REG_NFC_DONE = @"AppEvents\Schemes\Apps\.Default\Notification.Proximity\.Current";
        public const string REG_NOTIFY = @"AppEvents\Schemes\Apps\.Default\Notification.Default\.Current";

        public static void PlayNotificationSound(string regKey = REG_SMS)
        {
            bool found = false;
            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(regKey))
                {
                    if (key != null)
                    {
                        Object o = key.GetValue(null); // pass null to get (Default)
                        if (o != null)
                        {
                            SoundPlayer theSound = new SoundPlayer((String)o);
                            theSound.Play();
                            found = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SysLog.printMsg(ex);
            }
            if (!found)
            {
                SystemSounds.Beep.Play(); // consolation prize
            }
        }
    }
}

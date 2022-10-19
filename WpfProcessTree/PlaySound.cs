using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Runtime.InteropServices;
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

        public class Notification
        {
            public static void play(string regKey = REG_SMS)
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

        public class Media
        {

            public static SoundPlayer playAsync(string path)
            {
                SoundPlayer player = new SoundPlayer(path);
                player.Load();
                player.Play();
                return player;
            }

            public static SoundPlayer playSync(string path)
            {
                SoundPlayer player = new SoundPlayer(path);
                player.Load();
                player.PlaySync();
                return player;
            }

            public static void play(string path)
            {
                Task.Run(() =>
                {
                    var refPlayer = playSync(path);
                    refPlayer.Stop();
                    refPlayer.Dispose();
                    System.Windows.MessageBox.Show(path);
                });
            }

        }

        public class Mpeg
        {
            public static WmmPlayer play(string path, bool bLoop = false)
            {
                WmmPlayer wp = open(path);
                wp.play(bLoop);
                return wp;
            }

            public static WmmPlayer open(string path, string alias = null)
            {
                WmmPlayer wp = new WmmPlayer();
                if (null != alias)
                {
                    wp.alias = alias;
                }
                wp.open(path);
                return wp;
            }
        }


        public class WmmPlayer
        {
            [DllImport("winmm.dll")]
            static extern long mciSendString(string strCommand, StringBuilder strReturn, int iReturnLength, IntPtr hwndCallback);

            string _commandLast = "";
            bool isOpen;
            string sAlias = "MediaFile";

            public string lastCommand { get { return _commandLast; } }
            public string alias { get { return sAlias; } set { sAlias = value; } }


            public void open(string sFileName)
            {
                var _command = String.Format("open \"{1}\" type mpegvideo alias {0}", sAlias, sFileName);
                _commandLast = _command;
                mciSendString(_command, null, 0, IntPtr.Zero);
                isOpen = true;
            }

            public void close()
            {
                var _command = String.Format("close {0}", sAlias);
                _commandLast = _command;
                mciSendString(_command, null, 0, IntPtr.Zero);
                isOpen = false;
            }

            public void play()
            {
                play(false);
            }

            public void play(bool loop)
            {
                var _command = String.Format("play {0}", sAlias);
                if (isOpen)
                {
                    if (loop)
                    {
                        _command += " REPEAT";
                    }
                    _commandLast = _command;
                    mciSendString(_command, null, 0, IntPtr.Zero);
                }
            }

            string _Status()
            {
                StringBuilder sBuffer = new StringBuilder(128);
                string cmdStatus = String.Format("status {0} mode", sAlias);
                mciSendString(cmdStatus, sBuffer, sBuffer.Capacity, IntPtr.Zero);
                return sBuffer.ToString();
            }

            public bool isPlaying()
            {
                string st = _Status();
                if (st.IndexOf("playing", StringComparison.InvariantCultureIgnoreCase) >= 0)
                {
                    return true;
                }
                return false;
            }

        }

    }
}

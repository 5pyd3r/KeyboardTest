using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace KeyboardTest
{
    public class CodeToHex : IValueConverter
    {
        #region IValueConverter Members
        public Object Convert(object value, Type targetType, 
            object parameter, System.Globalization.CultureInfo culture)
        {
            int code = (int)value;
            if (code != 0)
            {
                return String.Format("0x{0:X2}", code);
            }
            else return "    ";
            
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
    internal class ViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<KeyViewModel> Keys { get; } = new ObservableCollection<KeyViewModel>();
        public int _scan = 0;
        public int ScanCode
        {
            get
            {
                return _scan;
            }
            private set
            {
                if (value == _scan) return;
                _scan = value;
                NotifyPropertyChanged();
            }
        }
        private int _vitual = 0;
        public int VitualCode
        {
            get
            {
                return _vitual;
            }
            private set
            {
                if (value == _vitual) return;
                _vitual = value;
                NotifyPropertyChanged();
            }
        }
        private string _name = "";
        public string KeyName
        {
            get{ return _name; }
            private set
            {
                if(_name == value) return;
                _name = value;
                NotifyPropertyChanged();
            }
        }
        private bool _trapped = false;
        public  bool Trapped
        {
            get { return _trapped; }
            set
            {
                if (_trapped == value) return;
                _trapped = value;
                Listener.trapped = _trapped;
                NotifyPropertyChanged();
            }
        }

        private ICommand mResetter;
        public ICommand ResetCommand
        {
            get
            {
                if (mResetter == null)
                    mResetter = new ResetCommand();
                return mResetter;
            }
            set
            {
                mResetter = value;
            }
        }

        KeyboardListener Listener = new KeyboardListener();

        List<KeyInfo> keymap;
        List<Keycode> keycodes;

        public ViewModel()
        {
            keymap = Utils.getKeymap();
            keycodes = Utils.getKeycodes();

            foreach(var k in keymap)
            {
                Keys.Add(new KeyViewModel()
                {
                    Left = k.left * 10,
                    Top = k.top * 10,
                    Width = k.width * 10,
                    Height = k.height * 10,
                    Pressed = false,
                    Name = Utils.getKeycodeByVkCode(k.vk_code).name,
                });
            }

            Listener.KeyDown += new RawKeyEventHandler(KeyDown);
            Listener.KeyUp += new RawKeyEventHandler(KeyUp);
        }

        public void reset()
        {
            foreach(var k in Keys)
            {
                k.Pressed = false;
            }
        }

        private void KeyUp(object sender, RawKeyEventArgs args)
        {
            int vk_code = args.VKCode;
            int sc_code = Utils.VkToSc(vk_code);

            for (int i = 0; i < keymap.Count; i++)
            {
                var k = keymap[i];
                if (k.vk_code == vk_code)
                {
                    Keys[i].Pressing = false;
                    Keys[i].Pressed = true;
                    break;
                }
            }

        }

        private void KeyDown(object sender, RawKeyEventArgs args)
        {
            int vk_code = args.VKCode;

            VitualCode = vk_code;
            ScanCode = 0;
            KeyName = "";

            for(int i = 0; i < keymap.Count; i++)
            { 
                var k = keymap[i];
                if(k.vk_code == vk_code)
                {
                    Keys[i].Pressing = true;
                    break;
                }
            }

            for(int i = 0; i < keycodes.Count; i++)
            {
                var keycode = keycodes[i];
                if (keycode.vk_code == vk_code)
                {
                    ScanCode = keycode.sc_code;
                    KeyName = keycode.name;
                }
            }
        }

        ~ViewModel()
        {
            Listener.Dispose();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

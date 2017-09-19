using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyboardTest
{
    public class KeyViewModel : INotifyPropertyChanged
    {
        private int _top;
        private int _left;
        private int _width;
        private int _height;

        public int Top
        {
            get { return _top; }
            set
            {
                if (value == _top) return;
                _top = value;
                OnPropertyChanged();
            }
        }

        public int Left
        {
            get { return _left; }
            set
            {
                if (value == _left) return;
                _left = value;
                OnPropertyChanged();
            }
        }

        public int Width
        {
            get { return _width; }
            set
            {
                if (value == _width) return;
                _width = value;
                OnPropertyChanged();
            }
        }

        public int Height
        {
            get { return _height; }
            set
            {
                if (value == _height) return;
                _height = value;
                OnPropertyChanged();
            }
        }

        private bool _pressed;
        public bool Pressed
        {
            get { return _pressed; }
            set
            {
                if (value == _pressed) return;
                _pressed = value;
                OnPropertyChanged();
            }
        }

        private bool _pressing;
        public bool Pressing
        {
            get { return _pressing; }
            set
            {
                if (value == _pressing) return;
                _pressing = value;
                OnPropertyChanged();
            }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                if (value == _name) return;
                _name = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

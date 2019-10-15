using System;
using System.ComponentModel;
using System.Windows.Input;

namespace HammingCodeGenerator.ViewModels
{
    class MainWindowViewModel : INotifyPropertyChanged
    {
        private readonly Models.HammingCode _hamming;

        public MainWindowViewModel()
        {
            _hamming = new Models.HammingCode();
            InfoMsg = "Info";
            CorrectionData = "2진수 형식(01001)으로 입력하세요";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string _originalData;
        public string OriginalData
        {
            get => _originalData;
            set
            {
                _originalData = value;
                OnPropertyChanged("OriginalData");
            }
        }

        private string _hammingCode;
        public string HammingCode
        {
            get => _hammingCode;
            set
            {
                _hammingCode = value;
                OnPropertyChanged("HammingCode");
            }
        }

        private string _receiveData;
        public string ReceiveData
        {
            get => _receiveData;
            set
            {
                _receiveData = value;
                OnPropertyChanged("ReceiveData");
            }
        }

        private string _infoMsg;
        public string InfoMsg
        {
            get => _infoMsg;
            set
            {
                _infoMsg = value;
                OnPropertyChanged("InfoMsg");
            }
        }

        private string _correctionData;
        public string CorrectionData
        {
            get => _correctionData;
            set
            {
                _correctionData = value;
                OnPropertyChanged("CorrectionData");
            }
        }

        private ICommand _generateCommand;
        public ICommand GenerateCommand
        {
            get => (_generateCommand) ?? (_generateCommand = new DelegateCommand(GenerateHammingCode));
        }

        private void GenerateHammingCode()
        {
            if (_originalData == null || _originalData.Length == 0)
            {
                CorrectionData = "데이터를 입력하세요";
                return;
            }
            HammingCode = _hamming.GenerateHammingCode(ref _originalData);
        }

        private ICommand _checkCommand;
        public ICommand CheckCommand
        {
            get => (_checkCommand) ?? (_checkCommand = new DelegateCommand(CheckHammingCode));
        }

        private void CheckHammingCode()
        {
            if (_receiveData == null || _receiveData.Length == 0)
            {
                CorrectionData = "데이터를 입력하세요";
                return;
            }
            int correctPos = _hamming.CheckHammingCode(ref _receiveData);
            if (correctPos == 0)
            {
                InfoMsg = "Info";
                CorrectionData = "에러 없습니다";
            }
            else
            {
                InfoMsg = correctPos + "번 비트 에러 있음";
                CorrectionData = _hamming.CorrectHammingCode(ref _receiveData, correctPos);
            }
        }
    }
}

using Imagin.Common;
using Imagin.Common.Configuration;
using Imagin.Common.Data;
using System;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Alarm
{
    [Serializable]
    public class Options : Data
    {
        bool soundChanged = false;

        string audioFilePath = string.Empty;
        [Hidden]
        public string AudioFilePath
        {
            get => Get.Current<MainViewModel>().Sounds.FirstOrDefault(i => i == audioFilePath);
            set => this.Change(ref audioFilePath, value);
        }

        string audioFolderPath = string.Empty;
        [Hidden]
        public string AudioFolderPath
        {
            get => audioFolderPath;
            set
            {
                this.Change(ref audioFolderPath, value);
                OnFolderPathChanged(value);
            }
        }
        
        double audioVolume = 80;
        [Hidden]
        public double AudioVolume
        {
            get => audioVolume / 100.0;
            set => this.Change(ref audioVolume, value * 100);
        }

        TimeSpan endTime;
        [Hidden]
        public TimeSpan EndTime
        {
            get => endTime;
            set => this.Change(ref endTime, value);
        }

        TimeSpan startTime;
        [Hidden]
        public TimeSpan StartTime
        {
            get => startTime;
            set => this.Change(ref startTime, value);
        }

        double increment = 5;
        [Hidden]
        public double Increment
        {
            get => increment;
            set => this.Change(ref increment, value);
        }

        double mathDifficulty = 0;
        [Hidden]
        public double MathDifficulty
        {
            get => mathDifficulty;
            set
            {
                this.Change(ref mathDifficulty, value);
                Get.Current<MainViewModel>().Changed(() => Get.Current<MainViewModel>().MathDifficulty);
            }
        }

        public override void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            base.OnPropertyChanged(propertyName);
            switch (propertyName)
            {
                case nameof(StartTime):
                    Console.WriteLine(StartTime);
                    break;
                case nameof(EndTime):
                    Console.WriteLine(EndTime);
                    break;
                case nameof(AudioFilePath):

                    if (!soundChanged)
                    {
                        soundChanged = true;
                        return;
                    }

                    Get.Current<MainViewModel>().Media.Play();
                    break;
            }
        }

        public void OnFolderPathChanged(string folderPath)
        {
            Get.Current<MainViewModel>().Media.Stop();
            Get.Current<MainViewModel>().Sounds.Refresh(folderPath);
            OnPropertyChanged(nameof(AudioFilePath));
        }
    }
}
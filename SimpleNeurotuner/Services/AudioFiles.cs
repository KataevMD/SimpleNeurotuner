using System;
namespace SimpleNeurotuner.Services
{
    /*
     Класс предназначенный для работы с аудиофайлами WAV - формата
     */
    public partial class AudioFiles
    {
        public partial List<string> GetAudioFiles();
        public partial string BuildPathToAudioFile(string nameAudioFile);
        public partial bool DeleteAudioFile(string pathSaveAudioFile);
    }
}


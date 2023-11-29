using System;
using Microsoft.Maui.Storage;

namespace SimpleNeurotuner.Services;

public partial class AudioFiles
{
    public List<string> listAudioFiles;
    string folderRecordPath = "Contents/Record";

    public partial List<string> GetAudioFiles()
    {
        if (!Directory.Exists(folderRecordPath))
        {
            Directory.CreateDirectory(folderRecordPath);
        }
        listAudioFiles = Directory.GetFiles(folderRecordPath, "*.wav").Select(f => Path.GetFileName(f)).ToList();
        
        return listAudioFiles;
    }

    public partial string BuildPathToAudioFile(string nameAudioFile)
    {
        return Path.Combine(folderRecordPath, nameAudioFile); 
    }

    public partial bool DeleteAudioFile(string pathSaveAudioFile)
    {
        if (File.Exists(pathSaveAudioFile))
        {
            File.Delete(pathSaveAudioFile);
            return true;
        }
        return false;
    }
}


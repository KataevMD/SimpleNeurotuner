using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using Microsoft.Maui.Controls;

namespace SimpleNeurotuner;

public partial class MainPage : ContentPage
{
   
    private Services.AudioPlayer player;
    private Services.AudioFiles audioFiles;
    private string audioFileName;
    private string pathSelectAudioFile;
    public Picker picker;

    public MainPage()
    {
        InitializeComponent();
        picker = (Picker)FindByName("audioFilesPicker");
        audioFiles = new Services.AudioFiles();
        picker.ItemsSource = audioFiles.GetAudioFiles();
        picker.SelectedItem = null;
    }



    private void AudioFilesPickerSelectedIndexChanged(object sender, EventArgs e)
    {

        if (picker.SelectedItem != null || picker.Title != "Выберите запись" || picker.Title != "Select an audio recording")
        {
            pathSelectAudioFile = audioFiles.BuildPathToAudioFile(picker.SelectedItem.ToString());
            audioFileName = picker.SelectedItem.ToString();
            
        }

    }

    private void OnPlayAudioClicked(object sender, EventArgs e)
    {
        player = new Services.AudioPlayer();
        player.StartPlayer(pathSelectAudioFile);
    }

    private void OnStopAudioClicked(object sender, EventArgs e)
    {
        player.StopPlayer();
    }
}



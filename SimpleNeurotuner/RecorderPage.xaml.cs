using SimpleNeurotuner.Services;
using SimpleNeurotuner.Resources;
namespace SimpleNeurotuner;

public partial class RecorderPage : ContentPage
{
    private Services.AudioRecorder record;
    private Services.AudioPlayer player;
    private Services.AudioFiles audioFiles;
    private string fileName;


    public RecorderPage()
	{
		InitializeComponent();
        audioFiles = new Services.AudioFiles();
    }

    private void OnStartRecordClicked(object sender, EventArgs e)
    {
        fileName = AudioFileNameEntry.Text;
        record = new Services.AudioRecorder();
        if (fileName.Length > 0)
        {
            record.StartRecord(fileName);
            fileName = fileName + ".wav";
        }
        
        
    }

    private void OnStopRecordClicked(object sender, EventArgs e)
    {
        record.StopRecord();
        Reset();
        //if (await DisplayAlert(SimpleNeurotuner.Resources.Strings.Resource.TitlePopUpRecorderPage.ToString(),
        //    SimpleNeurotuner.Resources.Strings.Resource.TextPopUpRecorderPage.ToString(),
        //    SimpleNeurotuner.Resources.Strings.Resource.YesBtn.ToString(), SimpleNeurotuner.Resources.Strings.Resource.NoBtn.ToString()))
        //{
        //    string pathSaveAudioFile = audioFiles.BuildPathToAudioFile(fileName);
        //    player = new Services.AudioPlayer();
        //    player.StartPlayer(pathSaveAudioFile);

        //    if (!await DisplayAlert(SimpleNeurotuner.Resources.Strings.Resource.TitlePopUpSaveAudioRecorderPage.ToString(),
        //    SimpleNeurotuner.Resources.Strings.Resource.TextPopUpSaveAudioRecorderPage.ToString(),
        //    SimpleNeurotuner.Resources.Strings.Resource.YesBtn.ToString(), SimpleNeurotuner.Resources.Strings.Resource.NoBtn.ToString()))
        //    {
        //        bool answer = audioFiles.DeleteAudioFile(pathSaveAudioFile);
        //    }
        //    Reset();
        //}
        //else
        //{
        //    Reset();
        //}
        
    }

    void Reset()
    {
        (App.Current as App).MainPage.Dispatcher.Dispatch(() =>
        {
            (App.Current as App).MainPage = new AppShell();
        });
    }
}

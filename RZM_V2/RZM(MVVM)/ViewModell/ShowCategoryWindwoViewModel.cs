using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using RZM_MVVM_.MVVM;
using GalaSoft.MvvmLight;
using RZM_MVVM_.Modell;
using System.Collections.ObjectModel;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Windows;

public class ShowCategoryWindowViewModel : ViewModelBase
{
    private string _headerCategory;
    public string FullPath = System.IO.Path.GetFullPath(ConstValues.RezeptJsonPath);

    public ICommand TestCommand => new RelayCommand(TestFunktion);

    private ObservableCollection<string> _showRezepts;
    public ObservableCollection<string> ShowRezepts
    {
        get { return _showRezepts; }
        set { Set(ref _showRezepts, value); }
    }

    public void TestFunktion()
    {
        ShowRezepts.Add("Test");
    }

    public ShowCategoryWindowViewModel()
    {
        // Initialisiere die ObservableCollection
        ShowRezepts = new ObservableCollection<string>();

        // Registriere den Empfänger für die Nachricht
        HeaderCategory = "Kategorie";
        Messenger.Default.Register<UpdateHeaderMessage>(this, HandleUpdateHeaderMessage);

        // Hier könnte etwas hinzugefügt werden, z.B. um die Liste zu füllen
       
        UpdateList();
    }

    public string HeaderCategory
    {
        get { return _headerCategory; }
        set
        {
            _headerCategory = value;
            // Hier kannst du UpdateList() aufrufen, um die Liste zu aktualisieren
            UpdateList();
        }
    }

    public void UpdateList()
    {
        if (HeaderCategory == null)
        {
            HeaderCategory = "";
        }
        List<string> resultList = JsonUtils.ExtractStringListFromJsonCtaegory(FullPath, (string)HeaderCategory);
        foreach (var item in resultList)
        {
            ShowRezepts.Add(item);
        }
    }

    private void HandleUpdateHeaderMessage(UpdateHeaderMessage message)
    {
        HeaderCategory = message.NewHeader;
    }
    public override void Cleanup()
    {
        Messenger.Default.Unregister(this);
        base.Cleanup();
    }
}



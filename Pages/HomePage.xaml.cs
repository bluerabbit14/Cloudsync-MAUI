
using CloudSync.Models;
namespace CloudSync.Pages;
public partial class HomePage : ContentPage
{
    private readonly string _userId;
    private readonly string _sessionId;
    public HomePage(string userId)
    {
        InitializeComponent();
        CollectionView.ItemsSource = GetFeatures();
        _userId = userId;
        UserId.Text = $"UserId: {_userId}";
       // _sessionId = sessionId;
    }
    private List<Feature> GetFeatures()
    {
        return new List<Feature>
        {
            new Feature {Image="weather.png",Title="Weather" },
            new Feature {Image="file2.png",Title="Convert" },
            new Feature {Image="file4.png",Title="Notes" }
        };
     }
    private async void Button_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new LoginPage());
    }
    private async void Button_Clicked_2(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new SignaturePage());
    }
    private async void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is Feature selectedFeature)
        {
            await DisplayAlert("Selected", selectedFeature.Title, "OK");
            //var NoteDetailPage = new NoteDetailPage();
            //var NoteViewModelPage=ne

            // Clear selection so it can be selected again
            ((CollectionView)sender).SelectedItem = null;
        }
    }
}


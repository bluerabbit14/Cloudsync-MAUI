using CloudSync.Models;
using CloudSync.Models.ViewModels;

namespace CloudSync.Pages;

public partial class NotesPage : ContentPage
{
    private bool isNavigating = false;

    public NotesPage()
    {
        InitializeComponent();
        BindingContext = new NoteViewModel();
    }
    private void ClearForm()
    {
        TitleEntry.Text = string.Empty;
        ContentEntry.Text = string.Empty;
        DateEntry.Date = DateTime.Today;
    }
    private void ClearButton_Clicked(object sender, EventArgs e)
    {
        ClearForm();
    }
    private async void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is Note selectedNote)
        {
            await DisplayAlert("Selected", selectedNote.Title, "OK");
            //var NoteDetailPage = new NoteDetailPage();
            //var NoteViewModelPage=ne

            // Clear selection so it can be selected again
            ((CollectionView)sender).SelectedItem = null;
        }
    }
    private async void DeleteGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
            if (e.Parameter is Note note)
            {
                bool answer = await DisplayAlert("Delete", $"{note.Title}?", "Yes", "No");
            if (answer)
            {
                var viewModel = BindingContext as NoteViewModel;
                viewModel?.Delete(note); // Assuming you have a Delete method in your ViewModel
            }
            else
                return;
            }
        }
}
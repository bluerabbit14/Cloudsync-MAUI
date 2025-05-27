using CloudSync.Models.ViewModels;
using CloudSync.Models;

namespace CloudSync.Pages;

public partial class NoteDetailPage : ContentPage
{
	public NoteDetailPage()
	{
		InitializeComponent();
	}

    private void EditButton_Clicked(object sender, EventArgs e)
    {
            //Enable editing controls
            TitleEntry2.IsReadOnly = false;
            DateEntry2.IsEnabled = true;
            ContentEntry2.IsReadOnly = false;

            //Enable Save and Clear buttons,disable Edit button
            ClearButton.IsEnabled = true;
            EditButton.IsEnabled = false;

        // Assuming Save button is the middle one
        ((Button)((Grid)EditButton.Parent).Children[1]).IsEnabled = true;
    }

    private void SaveButton_Clicked(object sender, EventArgs e)
    {

        var note = (BindingContext as NoteDetailViewModel)?.Note;
        if (note != null)
        {
            note.Title = TitleEntry2.Text;
            note.CreatedAt = DateEntry2.Date;
            note.Content = ContentEntry2.Text;

            // You can add your save logic here (e.g., update database or API call)
        }

        // Disable editing controls
        TitleEntry2.IsReadOnly = true;
        DateEntry2.IsEnabled = false;
        ContentEntry2.IsReadOnly = true;

        // Reset buttons
        EditButton.IsEnabled = true;
        ClearButton.IsEnabled = false;
        ((Button)((Grid)EditButton.Parent).Children[1]).IsEnabled = false;

        DisplayAlert("Success", "Note saved successfully.", "OK");
    }

    private void ClearButton_Clicked(object sender, EventArgs e)
    {
        TitleEntry2.Text = string.Empty;
        DateEntry2.Date = DateTime.Today;
        ContentEntry2.Text = string.Empty;

        // Reset fields to original values from the ViewModel
        var note = (BindingContext as NoteDetailViewModel)?.Note;
        if (note != null)
        {
            TitleEntry2.Text = note.Title;
            DateEntry2.Date = note.CreatedAt;
            ContentEntry2.Text = note.Content;
        }
    }
}
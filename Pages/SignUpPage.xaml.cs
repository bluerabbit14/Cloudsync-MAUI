using CloudSync.Models;
using Newtonsoft.Json;
using System.Text;
using System.Text.Json;

namespace CloudSync.Pages;

public partial class SignUpPage : ContentPage
{
    private static readonly HttpClient client = new HttpClient();
    public SignUpPage()
	{
		InitializeComponent();
    }
    //makessure user has entered all credential
    private bool AreCredentialsEntered(string? name, string? email, string? password, string? re_password)
    {
        return !string.IsNullOrWhiteSpace(name) &&
               !string.IsNullOrWhiteSpace(email) &&
               !string.IsNullOrWhiteSpace(password) &&
               !string.IsNullOrWhiteSpace(re_password);
    }
    private async void Button_Clicked(object sender, EventArgs e)
    {
        var NameTXT = NameEntry.Text?.Trim();
        var EmailTXT = EmailEntry.Text?.Trim();
        var PasswordTXT = PasswordEntry.Text?.Trim();
        var Re_passwordTXT = ReWrite_PasswordEntry.Text?.Trim();

        //to check all credentials are entered
        if (!AreCredentialsEntered(NameTXT, EmailTXT, PasswordTXT, Re_passwordTXT)) // step 2: check if user has entered any credentials
        {
            await DisplayAlert("Signup", "You have not entered any credential", "OK");
            return;
        }
            //to check password and re_password are same
            if (PasswordTXT != Re_passwordTXT) // step 3: check for valid credential and logged in
            {
                await DisplayAlert("Signup", $"Password and Re-write Password is not same!\nEnter identical Passwords", "OK");
                PasswordEntry.Text = "";
                ReWrite_PasswordEntry.Text = "";
                return;
            }
        if (Connectivity.NetworkAccess != NetworkAccess.Internet)
        {
            await DisplayAlert("No Internet", "Please check your internet connection.", "OK");
            return;
        }
            var SignupData = new{userName=NameTXT,  email = EmailTXT, password = PasswordTXT };
            var json = JsonConvert.SerializeObject(SignupData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
        try
        {
            var response = await client.PostAsync($"http://aspnetstaging-004-site48.jtempurl.com/api/User/register",content);
            
            if (response.IsSuccessStatusCode)
            {
                var resultJson = await response.Content.ReadAsStringAsync();              
                if (resultJson != null)
                {
                    await DisplayAlert("Signup", "Successfully signed up. Please log in.", "OK");
                    await Navigation.PushAsync(new LoginPage());
                }
                else
                {
                    await DisplayAlert("Signup", "Invalid server response. Try again later.", "OK");
                }
            }
        }
        catch (HttpRequestException ex)
        {
            await DisplayAlert("Network Error", $"Unable to reach server. Check your internet.\n{ex.Message}", "OK");
        }
        catch (TaskCanceledException)
        {
            await DisplayAlert("Timeout", "Server took too long to respond. Try again later.", "OK");
        }
        catch (System.Text.Json.JsonException)
        {
            await DisplayAlert("Error", "Could not parse server response.", "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Unexpected Error", $"Something went wrong: {ex.Message}", "OK");
        }
    }
    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        Navigation.PushAsync(new LoginPage());
    }
}
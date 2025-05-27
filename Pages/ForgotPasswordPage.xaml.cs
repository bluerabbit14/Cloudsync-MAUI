using Newtonsoft.Json;
using System.Text;

namespace CloudSync.Pages;

public partial class ForgotPasswordPage : ContentPage
{
    private static readonly HttpClient client = new HttpClient();
    public ForgotPasswordPage()
    {
        InitializeComponent();
    }

    private bool AreCredentialsEntered(string? email, string? password, string? re_password)
    {
        return !string.IsNullOrWhiteSpace(email) && !string.IsNullOrWhiteSpace(password) && !string.IsNullOrWhiteSpace(re_password);
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        var Email = EmailEntryForgot.Text?.Trim();
        var Password = PasswordEntryForgot.Text?.Trim();
        var Re_Password = ReWrite_PasswordEntryForgot.Text?.Trim(); 

        // to check all credentials are entered
        if (!AreCredentialsEntered(Email, Password, Re_Password)) // step 2: check if user has entered any credentials
        {
            await DisplayAlert("Forgot Password", "You have not entered any credential", "OK");
            return;
        }
        else
        {
            // to check password and re_password are same
            if (Password != Re_Password) // step 3: check for valid credential and logged in
            {
                await DisplayAlert("Forgot Password", $"Password and Re-write Password is not same!\nEnter identical Passwords", "OK");
                PasswordEntryForgot.Text = "";
                ReWrite_PasswordEntryForgot.Text = "";
                return;
            }
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await DisplayAlert("No Internet", "Please check your internet connection.", "OK");
                return;
            }
            int id = 3; //API fetch data as per user id
            var ForgotPasswordData= new { password = Password };
            var json = JsonConvert.SerializeObject(ForgotPasswordData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                var response = await client.PutAsync("http://aspnetstaging-004-site48.jtempurl.com/api/User/forgot-password/3",content);
                if (response.IsSuccessStatusCode)
                {
                    var resultJson = await response.Content.ReadAsStringAsync();
                    if (resultJson != null)
                    {

                        await DisplayAlert("Forgot Password", $"Password changed successfully\nNow you can log in with the same credentials", "Ok");
                        await Navigation.PushAsync(new LoginPage());
                    }
                    else
                    {
                        await DisplayAlert("Forgot Password", $"{response}", "OK");
                    }
                }
                else
                {
                    await DisplayAlert("Forgot Password", $"{response}", "OK");
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
    }
    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        Navigation.PushAsync(new LoginPage());
    }
}

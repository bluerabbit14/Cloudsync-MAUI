//using Android.Webkit;
using CloudSync.Models;
using Newtonsoft.Json;
using System.Text;
using System.Text.Json;

namespace CloudSync.Pages;

public partial class LoginPage : ContentPage
{
    private static readonly HttpClient client = new HttpClient();
    private static readonly HttpClient client2 = new HttpClient();
    private int count = 3;                       //to store number of try count
    private bool permission = true;              //to store status for allowd to login 
    public LoginPage()
    {
        InitializeComponent();
        securitycount.Text = $"You only have {count} try left";
    }
    // Update the problematic line to properly parse the JSON response into a strongly-typed object.
    // Assuming the response JSON contains a property "userId", we need to deserialize it into a class.

    private async void Button_Clicked(object sender, EventArgs e)
    {
        var EmailTXT = EmailEntry.Text?.Trim();
        var PasswordTXT = PasswordEntry.Text;
        if (permission) // step 1: check we have permission is true means we have count>0
        {
            if (!AreCredentialsEntered(EmailTXT, PasswordTXT)) // step 2: check if user has entered any credentials
            {
                await DisplayAlert("Login", "You have not entered any credential", "OK");
                return;
            }
            else
            {
                var LoginData = new { email = EmailTXT, password = PasswordTXT };
                var json = JsonConvert.SerializeObject(LoginData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                try
                {   //User Login Handle!!
                    var response = await client.PostAsync("http://aspnetstaging-004-site48.jtempurl.com/api/User/login", content);
                    if (response.IsSuccessStatusCode)
                    {
                        var resultJson = await response.Content.ReadAsStringAsync();
                        if (!string.IsNullOrEmpty(resultJson))
                        {
                            // Deserialize the response into a strongly-typed object
                            var loginResponse = JsonConvert.DeserializeObject<LoginResponse>(resultJson);
                            if (loginResponse != null && loginResponse.UserId != null)
                            {
                                await DisplayAlert("Login", $"Successfully Logged in! UserId: {loginResponse.UserId}", "OK");
                                //Session Api Handle!!
                                var id = loginResponse.UserId;
                                var SessionData = new { userId = id, LoginTime = DateTime.UtcNow, LogOutTime = DateTime.UtcNow, ipAddress="Null", DeviceInfo="Null" };
                                var json2 = JsonConvert.SerializeObject(SessionData);
                                var content2 = new StringContent(json2, Encoding.UTF8, "application/json");

                                var response2 = await client2.PostAsync("http://aspnetstaging-004-site48.jtempurl.com/api/Session/Session-Registor", content2);    
                                

                                await Navigation.PushAsync(new HomePage(loginResponse.UserId));
                            }
                            else
                            {
                                await DisplayAlert("Login", "You have not Signed up yet, go signup", "OK");
                                await Navigation.PushAsync(new SignUpPage());
                            }
                        }
                    }
                    else
                    {
                        if (count > 0) // step 4: user entered invalid credentials, if give him 3 tries only
                        {
                            --count;
                            await DisplayAlert("ERROR", $"You now only have {count} try left", "OK");
                            securitycount.Text = $"You only have {count} try left";
                        }
                        if (count == 0) // step 5: if run out of tries, make permission false
                        {
                            permission = false;
                            await DisplayAlert("ERROR", "You are not my master, I'm calling cops", "OK");
                            securitycount.Text = $"I love Weeknd,cause you are out of time!\ncome back after 10 sec";
                            if (!permission)
                            {
                                // Updated to use BindableObject.Dispatcher.StartTimer timer set for 10seconds
                                Dispatcher.StartTimer(TimeSpan.FromSeconds(10), () =>
                                {
                                    permission = true;
                                    count = 3;
                                    securitycount.Text = $"You only have {count} try left";
                                    EmailEntry.Text = "";
                                    PasswordEntry.Text = "";
                                    return false; // Return false to stop the timer
                                });
                            }
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
        }
    }

    // Define a class to represent the login response
    public class LoginResponse
    {
        [JsonProperty("userId")]
        public string? UserId { get; set; }
    }

    //makesure user has entered both email and password
    private bool AreCredentialsEntered(string? email, string? password)
    {
        return !string.IsNullOrWhiteSpace(email) && !string.IsNullOrWhiteSpace(password);
    }
    //handle forgot password tap feature
    private async void forgotPassword_Clicked(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new ForgotPasswordPage());
    }
    //handle sing up tap feature
    private async void SignUp_Clicked(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new SignUpPage());
    }
}
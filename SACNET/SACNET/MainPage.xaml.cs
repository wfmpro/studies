using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Net.Http;

namespace SACNET
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {        
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void ToggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {            
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (lightrjinstcode.Text.Length == 10)
            {
                if (twitteronoff.IsOn == true)
                {
                    Notificar.IsEnabled = false;
                    FlipEnergia.SelectedValue = FlipEnergiaStatus;
                    StatusEnergia.Text = "Notificando interrupção de serviço da light... " + "(" + lightrjinstcode.Text + ")";
                    StatusEnergiaProgress.IsActive = true;

                    //função que conecta na nuvem para pegar os dados necessários
                    //vamos começar pelo simulador...

                    if (ToggleSimulador.IsOn == true)
                    {                        
                        HttpClient client = new HttpClient();

                        //http via post pq é um dos requerimentos do api.
                        try
                        {
                            var contents = new StringContent("");
                            var response = await client.PostAsync("http://localhost/simuladorApi.php",contents);
                            response.EnsureSuccessStatusCode();
                            string responseBody = await response.Content.ReadAsStringAsync();
                            StatusEnergia.Text = responseBody;

                            if (response.IsSuccessStatusCode)
                            {
                                //Windows API não dá suporte a JSON então tem q fazer o proprio parser.
                                StatusEnergia.Text = StatusEnergia.Text.Substring((StatusEnergia.Text.LastIndexOf("text") + 8));
                                StatusEnergia.Text = StatusEnergia.Text.Remove((StatusEnergia.Text.Length - 2));
                                StatusEnergiaProgress.IsActive = false;
                                
                                //tb não dá tempo de fazer uma UI até a feira.
                            }
                        }
                        catch
                        {
                            //ainda tem q fazer o catch mas ainda tenho q fazer o tcc!
                        }

                        client.Dispose();                       
                    }
                }
            }
            else
            {
                SacnetPivot.SelectedItem = PivotItemContas;
                FlipviewContas.SelectedItem = FlipViewItemContas;
                Prosseguir.Visibility = Visibility.Visible;
                Prosseguir.IsEnabled = true;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            SacnetPivot.SelectedItem = PivotItemEnergia;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            SacnetPivot.SelectedItem = PivotItemTelecom;
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {            
            //botão Prosseguir
            if(lightrjinstcode.Text.Length == 10)
            {
                if (twitteronoff.IsOn == false)
                {
                    //conta twitter não tem user/pass
                    if((usuariotwitter.Text.Length < 1) || (senhatwitter.Password.Length <= 5))
                    {
                        FlipviewContas.SelectedItem = FlipViewItemContasLogins;                        
                    }
                    else
                    {
                        twitteronoff.IsOn = true;
                        Prosseguir.IsEnabled = false;
                        Prosseguir.Visibility = Visibility.Collapsed;
                        SacnetPivot.SelectedItem = PivotItemEnergia;
                        FlipviewContas.SelectedItem = FlipViewItemContasLogins;
                    }
                    
                }
                else
                {
                    Prosseguir.IsEnabled = false;
                    Prosseguir.Visibility = Visibility.Collapsed;
                    SacnetPivot.SelectedItem = PivotItemEnergia;
                    FlipviewContas.SelectedItem = FlipViewItemContasLogins;
                }                                                            
            }                    
        }
    }
}

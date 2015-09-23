using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http;

namespace SACNET
{
    class Simulador
    {
        public string Resposta { get; set; }     
        public async Task Conectar()
        {
            HttpClient c = new HttpClient();
            Uri location = new Uri("http://localhost/simuladorApi.php");
            HttpResponseMessage msg = await c.GetAsync(location);
            if (msg.IsSuccessStatusCode)
            {
                Resposta = msg.ToString();
            }
        }
        
        public void Conectarr()
        {

        }
    }
}

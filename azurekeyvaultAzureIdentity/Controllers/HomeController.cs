using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using azurekeyvaultAzureIdentity.Models;
using Azure.Security.KeyVault.Secrets;
using Azure.Core;
using Azure.Identity;

namespace azurekeyvaultAzureIdentity.Controllers
{
    //ref:https://docs.microsoft.com/en-us/azure/key-vault/general/tutorial-net-create-vault-azure-web-app
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.keyvaultsecretvaule = await GETKVAsync(); ;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<string>  GETKVAsync() {
            SecretClientOptions options = new SecretClientOptions()
            {
                Retry =
        {
            Delay= TimeSpan.FromSeconds(2),
            MaxDelay = TimeSpan.FromSeconds(16),
            MaxRetries = 5,
            Mode = RetryMode.Exponential
         }
            };
            var client = new SecretClient(new Uri("https://keyvaultkpmddemokvapp.vault.azure.net/"), new DefaultAzureCredential(), options);

            KeyVaultSecret secret =await  client.GetSecretAsync("csnadmin");

            string secretValue = secret.Value;
            return secretValue;
        }
    }
}

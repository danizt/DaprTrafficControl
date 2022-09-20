using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FineCollectionService.Models;

namespace FineCollectionService.Proxies
{
  public class VehicleRegistrationService
  {
    private HttpClient _httpClient;

    public VehicleRegistrationService(HttpClient httpClient)
    {
      _httpClient = httpClient;
    }

    // public async Task<VehicleInfo> GetVehicleInfo(string licenseNumber)
    // {
    //   return await _httpClient.GetFromJsonAsync<VehicleInfo>($"http://localhost:6002/vehicleinfo/{licenseNumber}");
    // }

    // public async Task<VehicleInfo> GetVehicleInfo(string licenseNumber)
    // {
    //   string daprHttpPort = Environment.GetEnvironmentVariable("DAPR_HTTP_PORT") ?? "3602";
    //   return await _httpClient.GetFromJsonAsync<VehicleInfo>(
    //       $"http://localhost:{daprHttpPort}/v1.0/invoke/vehicleregistrationservice/method/vehicleinfo/{licenseNumber}");
    // }

    public async Task<VehicleInfo> GetVehicleInfo(string licenseNumber)
    {
      return await _httpClient.GetFromJsonAsync<VehicleInfo>(
          $"/vehicleinfo/{licenseNumber}");
    }
  }
}
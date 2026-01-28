using Microsoft.JSInterop;
using System.Text.Json;

namespace Gym.WebApplication.JSAdapters
{
    public class LocalStorageAdapter(IJSRuntime _jsRuntime)
    {
        public async Task SetItemAsync<TValue>(String key, TValue value) 
            => await _jsRuntime.InvokeVoidAsync("localStorage.setItem", key, JsonSerializer.Serialize(value));

        public async Task<TValue?> GetItemAsync<TValue>(String key)
        {
            String? value = await _jsRuntime.InvokeAsync<String>("localStorage.getItem", key);
            return value is not null ? JsonSerializer.Deserialize<TValue>(value) : default;
        }

        public async Task RemoveItemAsync(String key)
            => await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", key);

        public async Task ClearAsync()
            => await _jsRuntime.InvokeVoidAsync("localStorage.clear");
    }
}

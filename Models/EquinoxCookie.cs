using Microsoft.AspNetCore.Http;
using System;
using System.Text.Json;

namespace Equinox.Models
{
    public static class EquinoxCookie
    {
        public static void SetCookie(HttpResponse response, string key, string value, int days = 7)
        {
            CookieOptions option = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(days)
            };
            response.Cookies.Append(key, value, option);
        }

        public static string? GetCookie(HttpRequest request, string key)
        {
            request.Cookies.TryGetValue(key, out string? value);
            return value;
        }

        public static void RemoveCookie(HttpResponse response, string key)
        {
            response.Cookies.Delete(key);
        }
        public static void SetObjectAsJson(this HttpResponse response, string key, object value, int days = 7)
        {
            var json = JsonSerializer.Serialize(value);
            SetCookie(response, key, json, days);
        }

        public static T? GetObjectFromJson<T>(this HttpRequest request, string key)
        {
            var value = GetCookie(request, key);
            return value == null ? default : JsonSerializer.Deserialize<T>(value);
        }
    }
}
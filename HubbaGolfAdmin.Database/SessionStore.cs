using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using HubbaGolfAdmin.Database.Dtos;

namespace HubbaGolfAdmin.Database
{
    public class SessionStore
    {
        IHttpContextAccessor _httpContextAccessor { get; } = null!;

        public SessionStore(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public ISession currentSession
        {
            get
            {
                return _httpContextAccessor.HttpContext.Session;
            }
        }

        /// <summary>
        /// set and get info user by session
        /// </summary>
        public UserDto? UserLogged
        {
            get
            {
                currentSession.TryGetValue("IsLogged", out var byteArray);
                if (byteArray is null || byteArray.Any() == false)
                {
                    return null;
                }

                var obj = SessionExtensions.GetObjectFromByte<UserDto>(byteArray);

                return obj;
            }
            set
            {
                if (value is not null)
                {
                    var byteArray = SessionExtensions.GetByteFromObject<UserDto?>(value);

                    currentSession.Set("IsLogged", byteArray);
                }
            }
        }

        /// <summary>
        /// check session is logged
        /// </summary>
        public bool IsLogged
        {
            get
            {
                currentSession.TryGetValue("IsLogged", out var byteArray);
                if (byteArray is not null && byteArray.Any())
                {
                    return true;
                }

                return false;
            }
        }
        public void StoreCurrentUrl()
        {
            var currentUrl = $"{_httpContextAccessor.HttpContext.Request.PathBase}{_httpContextAccessor.HttpContext.Request.Path}{_httpContextAccessor.HttpContext.Request.QueryString}";
            // Convert the string to bytes
            byte[] urlBytes = System.Text.Encoding.UTF8.GetBytes(currentUrl);
            currentSession.Set("RefererUrl", urlBytes);
        }
        public void SetSessionValue<T>(string key, T value)
        {
            if (value == null)
            {
                currentSession.Remove(key);
                return;
            }

            byte[] byteArray;

            // Handle string and int types explicitly
            if (typeof(T) == typeof(string))
            {
                byteArray = Encoding.UTF8.GetBytes(value.ToString());
            }
            else if (typeof(T) == typeof(int))
            {
                byteArray = BitConverter.GetBytes((int)(object)value);
            }
            else
            {
                // For other types, use JSON serialization
                byteArray = SessionExtensions.GetByteFromObject(value);
            }

            currentSession.Set(key, byteArray);
        }

        public T? GetSessionValue<T>(string key)
        {
            currentSession.TryGetValue(key, out var byteArray);

            if (byteArray == null || byteArray.Length == 0)
                return default;

            // Handle string and int types explicitly
            if (typeof(T) == typeof(string))
            {
                return (T)(object)Encoding.UTF8.GetString(byteArray);
            }
            else if (typeof(T) == typeof(int))
            {
                return (T)(object)BitConverter.ToInt32(byteArray, 0);
            }

            // For other types, use JSON deserialization
            return SessionExtensions.GetObjectFromByte<T>(byteArray);
        }
        public string GetRefererUrl()
        {
            // Try to retrieve the byte array from session
            if (currentSession.TryGetValue("RefererUrl", out byte[] urlBytes))
            {
                // Convert the byte array to a string (assuming it was stored as UTF-8 bytes)
                string refererUrl = System.Text.Encoding.UTF8.GetString(urlBytes);

                return refererUrl;
            }

            return string.Empty;
        }
    }

    public static class SessionExtensions
    {
        static JsonSerializerOptions _JsonSerializerOptions => new()
        {
            PropertyNamingPolicy = null,
            WriteIndented = true,
            AllowTrailingCommas = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        };

        public static T? GetObjectFromByte<T>(byte[] byteArray)
        {
            if (byteArray is not null && byteArray.Any())
            {
                return JsonSerializer.Deserialize<T>(byteArray, _JsonSerializerOptions);
            }

            return default;
        }

        public static byte[] GetByteFromObject<T>(T sessionObject)
        {
            if (sessionObject is null)
            {
                return Array.Empty<byte>();
            }

            return Encoding.UTF8.GetBytes(JsonSerializer.Serialize(sessionObject, _JsonSerializerOptions));
        }
    }
}
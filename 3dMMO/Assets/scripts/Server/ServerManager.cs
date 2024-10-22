using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;

namespace MyServerManager
{
    public interface IServerManager
    {
        Task<HttpResponseMessage> PostAsync(string url, HttpContent content);
    }

    public class ServerManager : IServerManager
    {
        private static ServerManager _instance;
        private static readonly object _lock = new object(); // 스레드 안전성 보장

        // HttpClient는 재사용할 수 있도록 정적으로 설정
        private readonly HttpClient client;

        private ServerManager()
        {
            client = new HttpClient();
        }

        // Singleton 인스턴스 접근자
        public static ServerManager Instance
        {
            get
            {
                // 스레드 안전성 보장을 위해 lock을 사용
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new ServerManager();
                    }
                    return _instance;
                }
            }
        }

        // IServerManager 인터페이스의 메서드 구현
        public async Task<HttpResponseMessage> PostAsync(string url, HttpContent content)
        {
            return await client.PostAsync(url, content);
        }
    }
}


using UnityEngine;
using TMPro; // TextMeshPro 네임스페이스 추가
using UnityEngine.Networking;
using System.Collections;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic; //dictionary
using Newtonsoft.Json;
using ApiUtilities;
using MyServerManager;
using SettingAccountManager;

namespace LoginManager
{
    public class Login
    {
        public static async Task<bool> GoLoginAccount(string id, string password, TextMeshProUGUI duplicateErrorText)
        {
            bool isSuccess = await LoginAccount(id, password, duplicateErrorText);
            return isSuccess;
        }
        private static async Task<bool> LoginAccount(string id, string password, TextMeshProUGUI duplicateErrorText)
        {
            var values = new Dictionary<string, string>
        {
            { "id", id },
            { "password", password }
        };

            var content = new FormUrlEncodedContent(values);

            try
            {
                HttpResponseMessage response = await ServerManager.Instance.PostAsync(ApiUrls.LoginUrl, content);

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized) // 401 Unauthorized 처리
                {
                    duplicateErrorText.gameObject.SetActive(true);
                    duplicateErrorText.text = "failed login";
                    return false;
                }
                else
                {
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();
                    Debug.Log("Response: 로그인 성공" + responseBody);
                    //await SettingAccount(responseBody);
                    await SettingAccount.DoSettingAccount(responseBody);

                    return true;
                }
            }
            catch (HttpRequestException e)
            {
                Debug.LogError($"Request error: {e.Message}");
                return false;
            }
        }
    }

}

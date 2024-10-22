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

namespace RegisterManager
{
    public class Register
    {
        public static async Task<bool> CreateAccount(string id, string password, string username, TextMeshProUGUI duplicateErrorText)
        {
            bool isSuccess = await RegisterNewAccount(id, password, username, duplicateErrorText);
            return isSuccess;
        }
        private static async Task<bool> RegisterNewAccount(string id, string password, string username, TextMeshProUGUI duplicateErrorText)
        {
            var values = new Dictionary<string, string>
        {
            { "id", id },
            { "password", password },
            { "username", username }
        };

            var content = new FormUrlEncodedContent(values);

            try
            {
                HttpResponseMessage response = await ServerManager.Instance.PostAsync(ApiUrls.RegisterUrl, content);

                if (response.StatusCode == System.Net.HttpStatusCode.Conflict) // 409 Conflict 처리
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var errorResponse = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseBody);
                    foreach (var kvp in errorResponse)
                    {
                        Debug.Log($"Key: {kvp.Key}, Value: {kvp.Value}");
                    }
                    Debug.Log(errorResponse);

                    // 키가 존재하는지 먼저 확인한 후 접근
                    if (errorResponse.ContainsKey("error_type"))
                    {
                        if (errorResponse["error_type"] == "duplicate_id")
                        {
                            Debug.LogError("Error: ID already exists.");
                            duplicateErrorText.gameObject.SetActive(true);
                            duplicateErrorText.text = "ID already exists";
                        }
                        else if (errorResponse["error_type"] == "duplicate_name")
                        {
                            Debug.LogError("Error: Username already exists.");
                            duplicateErrorText.gameObject.SetActive(true);
                            duplicateErrorText.text = "Username already exists";
                        }
                    }
                    else
                    {
                        Debug.LogError("Unknown error: No error_type found.");
                    }

                    return false;
                }
                else
                {
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();
                    //await SettingAccount(responseBody);
                    await SettingAccount.DoSettingAccount(responseBody);
                    //InitializePlayer(jsonResponse["playerinfo"]);
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

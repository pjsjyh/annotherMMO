using UnityEngine;
using TMPro; // TextMeshPro 네임스페이스 추가
using UnityEngine.Networking;
using System.Collections;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic; //dictionary
using Newtonsoft.Json;
using ApiUtilities;
using CharacterInfo;
namespace AuthManager
{
    public class Auth
    {
        public LoginCheck loginCheckManager;
        private static readonly HttpClient client = new HttpClient();
        // 계정 생성 버튼 클릭 시 호출
        public async Task<bool> CreateAccount(string id, string password, string username, TextMeshProUGUI duplicateErrorText)
        {
            bool isSuccess = await RegisterNewAccount(id, password, username, duplicateErrorText);
            return isSuccess;
        }
        public async Task<bool> GoLoginAccount(string id, string password, TextMeshProUGUI duplicateErrorText)
        {
            bool isSuccess = await LoginAccount(id, password, duplicateErrorText);
            return isSuccess;
        }
        // 서버에 POST 요청을 보내는 비동기 메서드
        private async Task<bool> RegisterNewAccount(string id, string password, string username, TextMeshProUGUI duplicateErrorText)
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
                HttpResponseMessage response = await client.PostAsync(ApiUrls.RegisterUrl, content);
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
                    await SettingAccount(responseBody);
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
        private async Task<bool> LoginAccount(string id, string password, TextMeshProUGUI duplicateErrorText)
        {
            var values = new Dictionary<string, string>
        {
            { "id", id },
            { "password", password }
        };

            var content = new FormUrlEncodedContent(values);

            try
            {
                HttpResponseMessage response = await client.PostAsync(ApiUrls.LoginUrl, content);
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
                    await SettingAccount(responseBody);

                    return true;
                }
            }
            catch (HttpRequestException e)
            {
                Debug.LogError($"Request error: {e.Message}");
                return false;
            }
        }
        private async Task SettingAccount(string responseBody)
        {
            var jsonResponse = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseBody);

            var playerInfo = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonResponse["playerinfo"].ToString());

            Debug.Log("Response: " + playerInfo);
            var characterJson = playerInfo["character"].ToString();

            // 다시 JSON으로 파싱
            ChaInfoOther characterData = JsonConvert.DeserializeObject<ChaInfoOther>(characterJson);

            var playerHP = int.Parse(playerInfo["HP"].ToString());
            var playerMp = int.Parse(playerInfo["MP"].ToString());
            var playerMoney = int.Parse(playerInfo["Money"].ToString());
            var playerLevel = int.Parse(playerInfo["Level"].ToString());
            var playerName = playerInfo["Username"].ToString();
            CharacterManager.Instance.InitializePlayer(characterData, playerName, playerHP, playerMp, playerMoney, playerLevel);




            return;
        }
    }
}

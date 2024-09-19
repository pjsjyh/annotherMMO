using UnityEngine;
using TMPro; // TextMeshPro 네임스페이스 추가
using UnityEngine.Networking;
using System.Collections;
using ApiUtilities;
public class serverTest : MonoBehaviour
{
    public TMP_InputField idInputField; // TextMeshPro의 TMP_InputField 사용
    public TMP_InputField passwordInputField; // TextMeshPro의 TMP_InputField 사용
    //private string url = "http://localhost:8080/set"; // Go 서버 URL

    // 계정 생성 버튼 클릭 시 호출
    public void CreateAccount()
    {
        string id = idInputField.text;
        string password = passwordInputField.text;
        Debug.Log($"ID: {id}, Password: {password}");

        // 코루틴을 통해 서버에 POST 요청을 보냄
        StartCoroutine(RegisterNewAccount(id, password));
    }

    // 서버에 POST 요청을 보내는 코루틴
    IEnumerator RegisterNewAccount(string id, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("id", id);
        form.AddField("password", password);
        UnityWebRequest www = UnityWebRequest.Post(ApiUrls.LoginUrl, form);
        Debug.Log(ApiUrls.LoginUrl);
        Debug.Log(www);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError) // Unity 2020.1 이전 버전에서는 isNetworkError, isHttpError 사용
        {
            Debug.Log(www.error);
        }
        else
        {
            // 서버 응답 출력 (예: 성공 메시지)
            Debug.Log("Response: " + www.downloadHandler.text);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // TextMeshPro 네임스페이스 추가
using AuthManager;
public class LoginCheck : MonoBehaviour
{
    // Start is called before the first frame update
    bool isLogin = true;
    public TMP_InputField idInputField; // TextMeshPro의 TMP_InputField 사용
    public TMP_InputField usernameInputField; // TextMeshPro의 TMP_InputField 사용
    public TMP_InputField passwordInputField; // TextMeshPro의 TMP_InputField 사용
    public TextMeshProUGUI createAccountText;
    public TextMeshProUGUI loginBtnText;
    public TextMeshProUGUI duplicateErrorText;
    private Auth authManager;

    private void Start()
    {
        authManager = new Auth(); // Auth 클래스의 인스턴스를 생성
    }
    public void signAccount()
    {
        if (isLogin)
        {
            duplicateErrorText.gameObject.SetActive(false);
            string id = idInputField.text;
            string password = passwordInputField.text;
            authManager.GoLoginAccount(id, password);

        }
        else
        {
            if (fieldFull())
            {
                duplicateErrorText.gameObject.SetActive(false);
                string id = idInputField.text;
                string password = passwordInputField.text;
                string username = usernameInputField.text;
                authManager.CreateAccount(id, password, username, duplicateErrorText);
            }
            else
            {
                duplicateErrorText.gameObject.SetActive(true);
                duplicateErrorText.text = "Please fill in the blanks";
                Debug.Log("ID: " + idInputField.text + ", Username: " + usernameInputField.text + ", Password: " + passwordInputField.text);
            }
        }

    }
    public bool fieldFull()
    {
        if (idInputField.text != null && usernameInputField.text != null && passwordInputField.text != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void changeMode()
    {
        if (isLogin)
        {
            createAccountText.gameObject.SetActive(true);
            usernameInputField.gameObject.SetActive(true);
            loginBtnText.text = "create Account";

            isLogin = false;
        }
        else
        {
            createAccountText.gameObject.SetActive(false);
            usernameInputField.gameObject.SetActive(false);
            loginBtnText.text = "login";
            isLogin = true;
        }
    }
}
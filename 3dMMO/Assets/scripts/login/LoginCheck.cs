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
    private Auth authManager;

    public void signAccount()
    {
        if (isLogin)
        {

        }
        else
        {
            if (fieldFull() && authManager != null)
            {
                string id = idInputField.text;
                string password = passwordInputField.text;
                string username = usernameInputField.text;
                authManager.CreateAccount(id, password, username);
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
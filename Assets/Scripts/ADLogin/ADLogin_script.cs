using System;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class ADLogin_script : MonoBehaviour
{
    private string clientId = "f75681b3-d0b8-4e50-8d66-43f1b7399f04";
    private string clientSecret = "i3y8Q~NzKUo2uZO~Xbz~uZuliXYhKODRymohycBJ"; // Optional
    private string redirectUri = "VCAuthApp://callback/";
    private string tenantId = "79f292be-a8c7-4251-99b0-2af7280bcfe1";
    private string authorizationCode; // Set this after callback
    private string accessToken;
    private string callbackUrl = "VCAuthApp://callback/?code=";



    public void AuthenticateWithAzureAD()
    {
        string authUrl = $"https://login.microsoftonline.com/{tenantId}/oauth2/v2.0/authorize?" +
                         $"client_id={clientId}" +
                         "&response_type=code" +
                         $"&redirect_uri={redirectUri}" +
                         "&response_mode=query" +
                         "&scope=openid%20profile%20offline_access" +
                         $"&state={Guid.NewGuid()}";

        Application.OpenURL(authUrl);
    }
    public void HandleCallback(string callbackUrl)
    {
        Uri uri = new Uri(callbackUrl);
        authorizationCode = uri.Query.Substring(6); // Assuming "?code=" is at the beginning
        StartCoroutine(ExchangeCodeForAccessToken());
    }

    private IEnumerator ExchangeCodeForAccessToken()
    {
        Dictionary<string, string> formData = new Dictionary<string, string>
        {
            { "grant_type", "authorization_code" },
            { "client_id", clientId },
            { "scope", "openid profile offline_access" },
            { "code", authorizationCode },
            { "redirect_uri", redirectUri }
        };

        if (!string.IsNullOrEmpty(clientSecret))
        {
            formData.Add("client_secret", clientSecret);
        }

        using (UnityWebRequest tokenRequest = UnityWebRequest.Post($"https://login.microsoftonline.com/{tenantId}/oauth2/v2.0/token", formData))
        {
            yield return tokenRequest.SendWebRequest();

            if (tokenRequest.result == UnityWebRequest.Result.ConnectionError || tokenRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Token exchange error: " + tokenRequest.error);
            }
            else
            {
                AccessTokenResponse tokenResponse = JsonUtility.FromJson<AccessTokenResponse>(tokenRequest.downloadHandler.text);
                accessToken = tokenResponse.access_token;
                Debug.Log("AccessToken  : " + accessToken);

                // Now you have the access token in memory, you can use it to make authenticated requests
            }
        }       
    }

    private IEnumerator GetUserProfile()
{
    string graphApiUrl = "https://graph.microsoft.com/v1.0/me";
    
    using (UnityWebRequest userRequest = UnityWebRequest.Get(graphApiUrl))
    {
        userRequest.SetRequestHeader("Authorization", $"Bearer {accessToken}");
        yield return userRequest.SendWebRequest();

        if (userRequest.result == UnityWebRequest.Result.ConnectionError || userRequest.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error getting user profile: " + userRequest.error);
        }
        else
        {
            Debug.Log("User Profile: " + userRequest.downloadHandler.text);
        }
    }
}



/*
    public void HandleCallback(string callbackUrl)
    {
        Uri uri = new Uri(callbackUrl);
        authorizationCode = uri.Query.Substring(6); // Assuming "?code=" is at the beginning
        StartCoroutine(ExchangeCodeForAccessToken());
    }

    private IEnumerator ExchangeCodeForAccessToken()
    {
        Dictionary<string, string> formData = new Dictionary<string, string>
        {
            { "grant_type", "authorization_code" },
            { "client_id", clientId },
            { "scope", "openid profile offline_access" },
            { "code", authorizationCode },
            { "redirect_uri", redirectUri }
        };

        if (!string.IsNullOrEmpty(clientSecret))
        {
            formData.Add("client_secret", clientSecret);
        }

        using (UnityWebRequest tokenRequest = UnityWebRequest.Post($"https://login.microsoftonline.com/{tenantId}/oauth2/v2.0/token", formData))
        {
            yield return tokenRequest.SendWebRequest();

            if (tokenRequest.result == UnityWebRequest.Result.ConnectionError || tokenRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Token exchange error: " + tokenRequest.error);
            }
            else
            {
                AccessTokenResponse tokenResponse = JsonUtility.FromJson<AccessTokenResponse>(tokenRequest.downloadHandler.text);
                accessToken = tokenResponse.access_token;

                // Now you have the access token, you can make authenticated requests
                StartCoroutine(GetUserProfile());
            }
        }
    }

    private IEnumerator GetUserProfile()
    {
        using (UnityWebRequest userRequest = UnityWebRequest.Get("https://graph.microsoft.com/v1.0/me"))
        {
            userRequest.SetRequestHeader("Authorization", $"Bearer {accessToken}");
            yield return userRequest.SendWebRequest();

            if (userRequest.result == UnityWebRequest.Result.ConnectionError || userRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error getting user profile: " + userRequest.error);
            }
            else
            {
                Debug.Log("User Profile: " + userRequest.downloadHandler.text);
            }
        }
    }
*/

}

[Serializable]
public class AccessTokenResponse
{
    public string access_token;
    public string token_type;
    public string refresh_token;
    public int expires_in;
    // Other fields that might be present in the response
}


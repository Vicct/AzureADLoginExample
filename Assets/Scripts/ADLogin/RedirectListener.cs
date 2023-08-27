using UnityEngine;
using System;
using System.Diagnostics;
using System.Threading;

public class RedirectListener : MonoBehaviour
{
    private bool listening = false;

    private void Start()
    {
        //string customUriScheme = "VCAuthApp";
        string customUriScheme = "localhost";
        //string redirectUri = customUriScheme + "://callback/";
        string redirectUri = customUriScheme + ":/callback/";

        listening = true;

        Thread listenerThread = new Thread(() =>
        {
            using (Process process = new Process())
            {
                process.StartInfo.FileName = redirectUri;
                process.StartInfo.UseShellExecute = true;

                process.Start();
                process.WaitForExit();
            }
        });

        listenerThread.Start();
    }

    private void OnDestroy()
    {
        listening = false;
    }
}

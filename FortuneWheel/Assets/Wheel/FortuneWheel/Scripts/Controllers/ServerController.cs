//#define DEBUG
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Ucss;

public class ServerController : MonoBehaviour {

    public string FB_token;
    public bool isOnline
    {
        get
        {
            return this._isOnline;
        }
    }
    private bool _isOnline;

    public bool isOnlineCheckDone
    {
        get
        {
            return this._isOnlineCheckDone;
        }
    }
    private bool _isOnlineCheckDone = false;

    
    private float lastInternetCheckTime;
    private float internetCheckDelta = 30;

    private static ServerController _instance;

    public static ServerController Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject();
                _instance = go.AddComponent<ServerController>();
                go.name = "ServerController";
                go.transform.parent = Common.GetControllerHolder();
                _instance.Init();
            }
            return _instance;
        }
    }

    private void Init()
    {
        UCSSconfig.maxTimeOutTries = 2;
        UCSSconfig.maxResendTries = 0; // apply it for static requests only!!!
    }


    public void InitFortuneWheel()
    {
        // do request to Server
        string url = Config.serverEndpoint + "/fortune/init/";

        UDebug.Log("[GetFortuneWinner] url = " + url);
        HTTPRequest request = new HTTPRequest();
        request.url = url;
        request.timeOut = 3;
        request.stringCallback = new EventHandlerHTTPString(this.OnFortuneInitResponse);
        request.onError = new EventHandlerServiceError(this.OnFortuneInitError);

        request.onTimeOutRetry = new EventHandlerServiceTimeOutRetry(this.OnTimeOutRetry);

        UCSS.HTTP.GetString(request);
    } // InitFortuneWheel

    private void OnFortuneInitResponse(string data, string transactionId)
    {
        UDebug.Log("[ServerController] [OnFortuneInitResponse] data = " + data);

        int code = this.GetResponseCode(UCSS.HTTP.GetTransaction(transactionId).www);
        UDebug.Log("[ServerController] [OnFortuneInitResponse] code = " + code);

        UCSS.HTTP.RemoveTransaction(transactionId);

        FortuneWheelController.Instance.ParseInitData(data, code);

        MainController.Instance.ShowFortuneWheel();
    } // OnFortuneInitResponse

    private void OnFortuneInitError(string error, string transactionId)
    {
        UDebug.LogError("[ServerController] [OnFortuneInitError] error = " + error);
        UCSS.HTTP.RemoveTransaction(transactionId);

        this.ErrorMessage(error);
    } // OnFortuneInitError

    public void GetFortuneWinner()
    {
        // do request to Server
        string url = Config.serverEndpoint + "/fortune/winner/";

        UDebug.Log("[GetFortuneWinner] url = " + url);
        HTTPRequest request = new HTTPRequest();
        request.url = url;
        request.timeOut = 3;
        request.stringCallback = new EventHandlerHTTPString(this.OnFortuneWinnerResponse);
        request.onError = new EventHandlerServiceError(this.OnFortuneWinnerError);

        request.onTimeOutRetry = new EventHandlerServiceTimeOutRetry(this.OnTimeOutRetry);

        UCSS.HTTP.GetString(request);
    } // GetFortuneWinner

    private void OnFortuneWinnerResponse(string data, string transactionId)
    {
        UDebug.Log("[ServerController] [OnFortuneWinnerResponse] data = " + data);

        int code = this.GetResponseCode(UCSS.HTTP.GetTransaction(transactionId).www);
        UDebug.Log("[ServerController] [OnFortuneWinnerResponse] code = " + code);

        UCSS.HTTP.RemoveTransaction(transactionId);

        FortuneWheelController.Instance.WinnerServerResponseParse(data, code);
    } // OnFortuneWinnerResponse

    private void OnFortuneWinnerError(string error, string transactionId)
    {
        UDebug.LogError("[ServerController] [OnFortuneWinnerError] error = " + error);
        UCSS.HTTP.RemoveTransaction(transactionId);

        this.ErrorMessage(error);
    } // OnFortuneWinnerError


    

    private void OnTimeOutRetry(string transactionId)
    {
        UDebug.LogWarning("[ServerController] [OnTimeOutRetry]");
        if (DialogsController.Instance.panels.loadingPanel.isActiveAndEnabled)
        {
            DialogsController.Instance.panels.loadingPanel.ConnectionProblemMode(true);
        }
    } // OnTimeOutRetry

    private int GetResponseCode(WWW www)
    {
        int code = -1;
        if (www.responseHeaders.ContainsKey("STATUS"))
        {
            if (!int.TryParse(www.responseHeaders["STATUS"], out code))
            {
                string[] statusParts = www.responseHeaders["STATUS"].Split(' ');
                if (statusParts.Length > 1)
                {
                    if (!int.TryParse(statusParts[1], out code))
                    {
                        Debug.LogWarning("Cannot get response code from [" + www.responseHeaders["STATUS"] + "]");
                    }
                }
                else
                {
                    Debug.LogWarning("Cannot get response code from [" + www.responseHeaders["STATUS"] + "]");
                }
            }
        }
        else
        {
            Debug.LogError("Response headers has no STATUS.");
        }
        return code;
    } // GetResponseCode

    private void ErrorMessage(string data)
    {
        UDebug.LogError("[ServerController] [ErrorMessage] data = " + data);
        DialogsController.Instance.panels.loadingPanel.HideSmoothly();

        ErrorMessageDialog errorMessageDialog = (ErrorMessageDialog)DialogsController.Instance.GetDialog(DialogsNames.ErrorMessageDialog);
        errorMessageDialog.title.text = "ERROR";
        errorMessageDialog.message.text = data + "\n Please, try to relaunch the game.";
        errorMessageDialog.showButton = false;
        DialogsController.Instance.Show(errorMessageDialog.gameObject, true);
    }

    private void SomeErrorMessage(string data)
    {
        UDebug.LogError("[ServerController] [SomeErrorMessage] data = " + data);
        DialogsController.Instance.panels.loadingPanel.HideSmoothly();

        ErrorMessageDialog errorMessageDialog = (ErrorMessageDialog)DialogsController.Instance.GetDialog(DialogsNames.ErrorMessageDialog);
        errorMessageDialog.title.text = "SOME ERROR";
        errorMessageDialog.message.text = "I cannot understand what the server says. \n Please, try to relaunch the game.";
        errorMessageDialog.showButton = false;
        DialogsController.Instance.Show(errorMessageDialog.gameObject, true);
    }

    private void RequestIsUnauthorizedMessage(string data)
    {
        UDebug.LogError("[ServerController] [RequestIsUnauthorized] data = " + data);
        DialogsController.Instance.panels.loadingPanel.HideSmoothly();

        ErrorMessageDialog errorMessageDialog = (ErrorMessageDialog)DialogsController.Instance.GetDialog(DialogsNames.ErrorMessageDialog);
        errorMessageDialog.title.text = "ERROR";
        errorMessageDialog.message.text = "Server says request is unauthorized. \n Please, try to relaunch the game.";
        errorMessageDialog.showButton = false;
        DialogsController.Instance.Show(errorMessageDialog.gameObject, true);
    }

    private void InternalErrorMessage(string data)
    {
        UDebug.LogError("[ServerController] [InternalErrorMessage] data = " + data);
        if (DialogsController.Instance.panels != null)
        {
            DialogsController.Instance.panels.loadingPanel.HideSmoothly();
        }

        ErrorMessageDialog errorMessageDialog = (ErrorMessageDialog)DialogsController.Instance.GetDialog(DialogsNames.ErrorMessageDialog);
        errorMessageDialog.title.text = "ERROR";
        errorMessageDialog.message.text = "Sorry, internal server error occurred. \n Please, try to relaunch the game.";
        errorMessageDialog.showButton = false;
        DialogsController.Instance.Show(errorMessageDialog.gameObject, true);
    }


    // *** INTERNET CHECK ***
    private void CheckInternetStatus()
    {
        this.lastInternetCheckTime = Time.realtimeSinceStartup;
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            this.SetOnlineStatus(false);
            return;
        }
        // ping our server
        // do request to Server
        string url = Config.serverEndpoint + "/ping/";

        UDebug.Log("[CheckInternetStatus] url = " + url);
        HTTPRequest request = new HTTPRequest();
        request.url = url;
        request.timeOut = 5;
        request.formData = null;
        request.stringCallback = new EventHandlerHTTPString(this.OnCheckInternetResponse);
        request.onError = new EventHandlerServiceError(this.OnCheckInternetError);

        UCSS.HTTP.GetString(request);
    } // CheckInternetStatus

    private void OnCheckInternetResponse(string data, string transactionId)
    {
        UDebug.Log("[ServerController] [OnCheckInternetResponse] data = " + data);
        UCSS.HTTP.RemoveTransaction(transactionId);

        if (data == "pong" || data.IndexOf("success") != 0)
        {
            this.SetOnlineStatus(true);
        } else
        {
            this.SetOnlineStatus(false);
        }
    } // OnCheckInternetResponse

    private void OnCheckInternetError(string error, string transactionId)
    {
        UDebug.LogError("[ServerController] [OnCheckInternetError] error = " + error);
        UCSS.HTTP.RemoveTransaction(transactionId);

        this.SetOnlineStatus(false);
    } // OnCheckInternetError

    public void SetOnlineStatus(bool status)
    {
        UDebug.Log("[SetOnlineStatus] status = " + status);
        this._isOnlineCheckDone = true;
        this._isOnline = status;
        if (status == true)
        {
            this.lastInternetCheckTime = Time.realtimeSinceStartup + this.internetCheckDelta * 10; // rare check if internet is good
        }
    } // SetOnlineStatus

    // *** MONO ***
    void Update()
    {
        if (this.lastInternetCheckTime + this.internetCheckDelta < Time.realtimeSinceStartup)
        {
            this.CheckInternetStatus();
        }
    }

} // ServerController
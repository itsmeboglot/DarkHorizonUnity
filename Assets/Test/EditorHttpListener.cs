using Utils.Metamask;
using Object = UnityEngine.Object;
#if UNITY_EDITOR
using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Cysharp.Threading.Tasks;

#endif

public class EditorHttpListener
{
#if UNITY_EDITOR
    private HttpListener _listener;
    private UniTask _listenerThread;
    private MetamaskService _metamaskService;
    private readonly Dictionary<string, Action<string>> _listeners = new Dictionary<string, Action<string>>();

    ~EditorHttpListener()
    {
        Dispose();
    }

    public void StartListen(int port)
    {
        _listener = new HttpListener();
        _listener.Prefixes.Add($"http://localhost:{port}/");
        _listener.AuthenticationSchemes = AuthenticationSchemes.Anonymous;
        _listener.Start();

        _listenerThread = UniTask.Run(StartListener);
        Debug.Log($"Local server started on port: {port}");

        _listeners.Clear();

        _metamaskService = Object.FindObjectOfType<MetamaskService>();
    }

    public void RegisterTestListener(string dataType, Action<string> callback)
    {
        if (_listeners.ContainsKey(dataType) == false)
            _listeners.Add(dataType, callback);
        else
            Debug.LogError($"{dataType} already has listener!");
    }

    private void Dispose()
    {
        _listenerThread.SuppressCancellationThrow();

        _listener?.Stop();
        _listener?.Close();
    }

    private void StartListener()
    {
        while (true)
        {
            var result = _listener.BeginGetContext(ListenerCallback, _listener);
            result.AsyncWaitHandle.WaitOne();
        }
    }

    private void ListenerCallback(IAsyncResult result)
    {
        var context = _listener.EndGetContext(result);
        var path = context.Request.Url.LocalPath.Replace("/", "");

        if (context.Request.HasEntityBody)
        {
            var data = "{}";
            using (var reader = new StreamReader(context.Request.InputStream, context.Request.ContentEncoding))
            {
                data = reader.ReadToEnd();
                ProcessRequestData(path, data).Forget();
            }
        }
        else if (context.Request.QueryString.AllKeys.Length > 0)
        {
            ProcessRequestData(path, context.Request.QueryString.GetValues("data")?[0]).Forget();
        }

        context.Response.StatusCode = 200;
        context.Response.Close();
    }

    private async UniTaskVoid ProcessRequestData(string path, string data)
    {
        await UniTask.SwitchToMainThread();
        if (_listeners.ContainsKey(path))
            _listeners[path]?.Invoke(data);
        else
            Debug.LogError($"Not implemented {path} {data}");
    }
#endif
}
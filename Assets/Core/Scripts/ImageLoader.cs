using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace Core
{
    public class ImageLoader
    {
        public IEnumerator DownloadImage(string url, Action<Texture> callback, IProgress<float> progress = null)
        {
            using (var www = UnityWebRequestTexture.GetTexture(url))
            {
                var asyncOperation = www.SendWebRequest();

                while (!asyncOperation.isDone)
                {
                    progress?.Report(asyncOperation.progress);
                    yield return null;
                }

                if (www.result != UnityWebRequest.Result.Success)
                {
                    yield break;
                }

                callback.Invoke(DownloadHandlerTexture.GetContent(www));
            }
        }
    }
}

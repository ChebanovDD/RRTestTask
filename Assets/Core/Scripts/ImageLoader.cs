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
            using var www = UnityWebRequestTexture.GetTexture(url);

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                yield break;
            }

            callback.Invoke(DownloadHandlerTexture.GetContent(www));
        }
	}
}

using UnityEngine;
using UnityEngine.UI;

namespace Core.Extensions
{
    public static class RawImageExtensions
    {
        public static void FillParent(this RawImage image)
        {
            if (image.texture == null)
            {
                return;
            }

            var parent = image.transform.parent == null
                ? image.GetComponentInParent<RectTransform>()
                : image.transform.parent.GetComponent<RectTransform>();

            if (parent == null)
            {
                return;
            }

            image.SetNativeSize();

            var ratio = image.texture.width / (float) image.texture.height;
            var bounds = new Rect(0, 0, parent.rect.width, parent.rect.height);

            // Size by height first.
            var h = bounds.height;
            var w = h * ratio;

            if (w < bounds.width)
            {
                w = bounds.width;
                h = w / ratio;
            }
            else
            {
                h = bounds.height;
                w = h * ratio;
            }

            var imageTransform = image.GetComponent<RectTransform>();
            imageTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, h);
            imageTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, w);
        }
    }
}

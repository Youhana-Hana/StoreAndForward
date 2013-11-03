namespace StoreAndForward
{
    using System;
    using System.Net;
    using System.Threading.Tasks;

    public static class Extensions
    {
        public static Task<PostResult> UploadStringTask(this WebClient webClient, Uri uri, string data)
        {
            var tcs = new TaskCompletionSource<PostResult>();

            webClient.UploadStringCompleted += (s, e) =>
            {
                if (e.Error != null)
                {
                    tcs.SetResult(e.Error.ToPostResult());
                }
                else
                {
                    tcs.SetResult(PostResult.OK);
                }
            };

            webClient.UploadStringAsync(uri, data);

            return tcs.Task;
        }

        public static void SetHeaders(this WebClient webClient, WebHeaderCollection headers)
        {
            if (headers == null)
            {
                return;
            }

            foreach (var key in headers.AllKeys)
            {
                webClient.Headers[key] = headers[key];
            }
        }

        public static PostResult ToPostResult(this Exception exception)
        {
            if (exception is WebException)
            {
                var webException = (WebException)exception;
                var response = (HttpWebResponse)webException.Response;
                if (response != null)
                {
                    return response.StatusCode.ToPostResult();
                }
            }

            return PostResult.PermanentError;
        }

        public static PostResult ToPostResult(this HttpStatusCode httpStatusCode)
        {
            var statusCode = (int)httpStatusCode;

            if (statusCode >= 300 && statusCode < 500)
            {
                return PostResult.PermanentError;
            }

            if (statusCode >= 500 && statusCode < 600)
            {
                return PostResult.TemporaryError;
            }

            return PostResult.OK;
        }
    }
}

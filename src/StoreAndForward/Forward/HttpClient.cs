namespace StoreAndForward
{
    using System;
    using System.Net;
    using System.Threading.Tasks;

    internal class HttpClient : IHttpClient
     {
       public PostResult Post(IMessage message)
         {
             if (message == null)
             {
                throw new ArgumentNullException("message");
             }

            var task = this.UploadString(message);
            task.Wait();

            return task.Result;
         }

       private Task<PostResult> UploadString(IMessage message)
       {
           var webClient = new WebClient();
           
           webClient.SetHeaders(message.Headers);
           webClient.Headers["content-type"] = message.ContentType;

           return webClient.UploadStringTask(message.EndPoint, message.Body);
        }
     }
}

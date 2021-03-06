using System;
using System.Net;
using StoreAndForward;
namespace MoqaLate.Autogenerated
{
public partial class MessageMoqaLate : IMessage
{
// ------------ Property Id
private int _Id;
public virtual int Id
{
get { return _Id; }
}
public virtual void __SetId(int val)
{
   _Id = val;
}
// ------------ Property ContentType
private string _ContentType;
public virtual string ContentType
{
get { return _ContentType; }
}
public virtual void __SetContentType(string val)
{
   _ContentType = val;
}
// ------------ Property Body
private string _Body;
public virtual string Body
{
get { return _Body; }
}
public virtual void __SetBody(string val)
{
   _Body = val;
}
// ------------ Property EndPoint
private Uri _EndPoint;
public virtual Uri EndPoint
{
get { return _EndPoint; }
}
public virtual void __SetEndPoint(Uri val)
{
   _EndPoint = val;
}
// ------------ Property Headers
private WebHeaderCollection _Headers;
public virtual WebHeaderCollection Headers
{
get { return _Headers; }
}
public virtual void __SetHeaders(WebHeaderCollection val)
{
   _Headers = val;
}
}
}

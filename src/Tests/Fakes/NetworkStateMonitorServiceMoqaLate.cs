using System;
using StoreAndForward;
namespace MoqaLate.Autogenerated
{
public partial class NetworkStateMonitorServiceMoqaLate : INetworkStateMonitorService
{


// -------------- Start ------------


        private int _startNumberOfTimesCalled;        

        

        public virtual bool StartWasCalled()
{
   return _startNumberOfTimesCalled > 0;
}


public virtual bool StartWasCalled(int times)
{
   return _startNumberOfTimesCalled == times;
}


public virtual int StartTimesCalled()
{
   return _startNumberOfTimesCalled;
}




        public void Start()
        {
            _startNumberOfTimesCalled++;            

            
        }


// -------------- Stop ------------


        private int _stopNumberOfTimesCalled;        

        

        public virtual bool StopWasCalled()
{
   return _stopNumberOfTimesCalled > 0;
}


public virtual bool StopWasCalled(int times)
{
   return _stopNumberOfTimesCalled == times;
}


public virtual int StopTimesCalled()
{
   return _stopNumberOfTimesCalled;
}




        public void Stop()
        {
            _stopNumberOfTimesCalled++;            

            
        }
public virtual event EventHandler<NetworkStateEventArgs> NetworkStatusChanged;
}
}

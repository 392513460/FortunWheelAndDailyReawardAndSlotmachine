namespace Ucss
{
    public class UCSSRequest
    {
        public string url;
        public string transactionId;

        public EventHandlerServiceError onError;
        public EventHandlerServiceTimeOut onTimeOut;

        public EventHandlerServiceRetry onRetry;
        public EventHandlerServiceTimeOutRetry onTimeOutRetry;

        public int timeOut;
        public int maxResends;
        public int maxTimeOutTries;

        private int resends;
        private int timeOutTries;

        public bool IsTimeOutTryAllowed(bool increaseCounter = true)
        {
            if (this.maxTimeOutTries > 0 && this.timeOutTries < this.maxTimeOutTries)
            {
                if (increaseCounter)
                {
                    this.timeOutTries++;
                }
                return true;
            }
            return false;
        }

        public bool IsResendsAllowed(bool increaseCounter = true)
        {
            if (this.maxResends > 0 && this.resends < this.maxResends)
            {
                if (increaseCounter)
                {
                    this.resends++;
                }
                return true;
            }
            return false;
        }
    } // UCSSRequest
}
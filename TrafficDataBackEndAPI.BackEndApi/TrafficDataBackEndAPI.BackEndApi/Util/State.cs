namespace TrafficDataBackEndAPI.BackEndApi.Util
{
    // This class provides the integer state values to be inserted in the database
    public class State
    {
        public const int MeasurementPointsNotRetrieved = 0;
        public const int MeasurementPointsRetrieved = 1;
        public const int ExtractingTrafficData = 2;
        public const int Sleeping = 3;
        public const int Aborted = 4;
        public const int Resumed = 5;
        public const int Aborting = 6;

        public static string ConvertToString(int state)
        {
            string strState = "";
            switch (state)
            {
                case 0:
                    strState  = "Measurement Points Not Retrieved";
                    break;
                case 1:
                    strState = "Measurement Points Retrieved";
                    break;
                case 2:
                    strState = "Extracting Traffic Data";
                    break;
                case 3:
                    strState = "Sleeping";
                    break;
                case 4: 
                    strState = "Aborted";
                    break;
                case 5:
                    strState = "Resumed";
                    break;
                case 6:
                    strState = "Aborting";
                    break;
            }
            return strState;
        }
    }
}
namespace TrafficDataBackEndAPI.BackEndApi
{
    public class InitializationOptions
    {
        public bool SeedDatabase {get; set;}

        public InitializationOptions()
        {
            SeedDatabase = false;
        }
    }
}
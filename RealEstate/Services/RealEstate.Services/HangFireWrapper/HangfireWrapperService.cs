namespace RealEstate.Services.HangFireWrapper
{
    using Hangfire;

    using RealEstate.Services.Interfaces;

    public class HangfireWrapperService : IHangfireWrapperService
    {
        public IBackgroundJobClient BackgroundJobClient 
            => new BackgroundJobClient(JobStorage.Current);

        public IRecurringJobManagerV2 RecurringJobManager 
            => new RecurringJobManager();
    }
}

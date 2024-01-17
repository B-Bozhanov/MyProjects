namespace RealEstate.Services.Data.Tests.Mocks
{
    using System;

    using Hangfire;
    using Hangfire.Common;
    using Hangfire.States;
    using Hangfire.Storage;

    using Moq;

    using RealEstate.Services.Interfaces;

    internal static class HangFireWrapperServiceMock
    {
        internal static IHangfireWrapperService Instance
        {
            get
            {
                var hangfireWrapperService = new Mock<IHangfireWrapperService>();
                var jobStorageMock = new Mock<JobStorage>();
                var storageConnectionMock = new Mock<IStorageConnection>();
                var transactionConnectionMock = new Mock<IWriteOnlyTransaction>();

                JobStorage.Current = jobStorageMock.Object;

                jobStorageMock
                    .Setup(y => y.GetConnection())
                    .Returns(storageConnectionMock.Object);

                storageConnectionMock
                    .Setup(y => y.AcquireDistributedLock(It.IsAny<string>(), It.IsAny<TimeSpan>()))
                    .Returns(transactionConnectionMock.Object);

                storageConnectionMock
                    .Setup(y => y.CreateWriteTransaction())
                    .Returns(transactionConnectionMock.Object);

                transactionConnectionMock
                    .Setup(y => y.RemoveHash(It.IsAny<string>()));

                transactionConnectionMock
                    .Setup(y => y.RemoveFromSet(It.IsAny<string>(), It.IsAny<string>()));

                transactionConnectionMock
                .Setup(y => y.Commit());

                hangfireWrapperService.Setup(x => x.BackgroundJobClient.Create(It.IsAny<Job>(), It.IsAny<EnqueuedState>()));
                hangfireWrapperService.Setup(x => x.RecurringJobManager).Returns(new RecurringJobManager(JobStorage.Current));

                return hangfireWrapperService.Object;
            }
        }
    }
}

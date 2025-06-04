using gWorks.Hiring.Data;
using Moq;

namespace gWorks.Hiring.TestConsoleApplication.Tests;

internal static class MockHelpers
{
    internal static (Mock<IRepository<T>> Mock, ICollection<T> DataSet) CreateMockRepo<T>()
        where T : class
    {
        var dataSet = new List<T>();

        var mock = new Mock<IRepository<T>>();
        mock.Setup(x => x.AddRange(It.IsAny<IEnumerable<T>>())).Callback<IEnumerable<T>>(dataSet.AddRange).Verifiable();
        mock.Setup(x => x.Add(It.IsAny<T[]>())).Callback<T[]>(dataSet.AddRange).Verifiable();
        mock.Setup(x => x.AddRangeAsync(It.IsAny<IEnumerable<T>>())).Returns<IEnumerable<T>>(items => { dataSet.AddRange(items); return Task.CompletedTask; }).Verifiable();
        mock.Setup(x => x.AddAsync(It.IsAny<T[]>())).Returns<T[]>(items => { dataSet.AddRange(items); return Task.CompletedTask; }).Verifiable();

        mock.Setup(x => x.RemoveRange(It.IsAny<IEnumerable<T>>())).Callback<IEnumerable<T>>(x =>
        {
            foreach (var r in x)
            {
                dataSet.Remove(r);
            }
        }).Verifiable();
        mock.Setup(x => x.Remove(It.IsAny<T[]>())).Callback<T[]>(x =>
        {
            foreach (var r in x)
            {
                dataSet.Remove(r);
            }
        }).Verifiable();

        mock.SetupGet(x => x.Query).Returns(() => dataSet.AsQueryable()).Verifiable();
        return (mock, dataSet);
    }
}

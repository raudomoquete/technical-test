using Xunit.Abstractions;
using Xunit.Sdk;

namespace DGII.Tests;

[AttributeUsage(AttributeTargets.Class)]
public class TestPriorityAttribute : Attribute
{
    public TestPriorityAttribute(int priority)
    {
        Priority = priority;
    }

    public int Priority { get; }
}

public class TestCollectionOrderer : ITestCollectionOrderer
{
    public IEnumerable<ITestCollection> OrderTestCollections(IEnumerable<ITestCollection> testCollections)
    {
        return testCollections.OrderBy(GetTestPriority);
    }

    private static int GetTestPriority(ITestCollection testCollection)
    {
        var attribute = testCollection.CollectionDefinition?.GetCustomAttributes(typeof(TestPriorityAttribute))
            .FirstOrDefault();

        return attribute?.GetNamedArgument<int>("Priority") ?? 0;
    }
}

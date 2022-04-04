using System;
using Xunit;

namespace CoreModule.Tests
{
    public class GuidExtensionsTests
    {
        [Fact]
        public void EmptyGuid_Increase_SeuquentialNewGuid()
        {
            var currentGuid = Guid.Empty;
            var newGuid = currentGuid.SequentialGuid();
            Assert.NotEqual(currentGuid, newGuid);
        }
        [Fact]
        public void NewGuid_Increase_SeuquentialNewGuid()
        {
            var currentGuid = Guid.NewGuid();
            var newGuid = currentGuid.SequentialGuid();
            Assert.NotEqual(currentGuid, newGuid);
        }
    }
}
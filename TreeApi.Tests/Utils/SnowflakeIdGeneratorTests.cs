using NUnit.Framework;
using TreeApi.Utils;

namespace TreeApi.Tests.Utils
{
    [TestFixture]
    public class SnowflakeIdGeneratorTests
    {
        [Test]
        public void SnowflakeIdGenerator_NextId_ShouldReturnValidId()
        {
            var id = SnowflakeIdGenerator.NextId();
            
            Assert.That(id, Is.GreaterThan(0));
            Assert.That(id, Is.LessThan(long.MaxValue));
        }

        [Test]
        public void SnowflakeIdGenerator_NextId_ShouldReturnUniqueIds()
        {
            var id1 = SnowflakeIdGenerator.NextId();
            var id2 = SnowflakeIdGenerator.NextId();
            
            Assert.That(id1, Is.Not.EqualTo(id2));
            Assert.That(id1, Is.GreaterThan(0));
            Assert.That(id2, Is.GreaterThan(0));
        }

        [Test]
        public void SnowflakeIdGenerator_NextId_ShouldReturnIncreasingIds()
        {
            var id1 = SnowflakeIdGenerator.NextId();
            var id2 = SnowflakeIdGenerator.NextId();
            var id3 = SnowflakeIdGenerator.NextId();
            
            Assert.That(id1, Is.LessThan(id2));
            Assert.That(id2, Is.LessThan(id3));
        }

        [Test]
        public void SnowflakeIdGenerator_NextId_ShouldReturnValidSnowflakeFormat()
        {
            var id = SnowflakeIdGenerator.NextId();
            
            // Snowflake ID should be a 64-bit integer
            Assert.That(id, Is.GreaterThan(0));
            Assert.That(id, Is.LessThan(long.MaxValue));
        }

        [Test]
        public void SnowflakeIdGenerator_NextId_ShouldHandleConcurrentAccess()
        {
            var tasks = new List<Task<long>>();
            
            for (int i = 0; i < 100; i++)
            {
                tasks.Add(Task.Run(() => SnowflakeIdGenerator.NextId()));
            }
            
            Task.WaitAll(tasks.ToArray());
            
            var ids = tasks.Select(t => t.Result).ToList();
            var uniqueIds = ids.Distinct().ToList();
            
            Assert.That(uniqueIds.Count, Is.EqualTo(ids.Count));
        }
    }
}

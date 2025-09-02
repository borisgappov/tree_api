namespace TreeApi.Utils
{
    public static class SnowflakeIdGenerator
    {
        private static readonly object _lock = new object();
        private static long _lastTimestamp = -1L;
        private static long _sequence = 0L;
        private const int MACHINE_ID_BITS = 10;
        private const int SEQUENCE_BITS = 12;       
        private const long MAX_MACHINE_ID = (1L << MACHINE_ID_BITS) - 1;
        private const long MAX_SEQUENCE = (1L << SEQUENCE_BITS) - 1;
        // Epoch: January 1, 2020 UTC
        private const long EPOCH = 1577836800000L;        
        // Machine ID (can be configured)
        private static readonly long _machineId = GetMachineId();

        /// <summary>
        /// Gets the machine identifier for Snowflake ID generation.
        /// In production, this should be configured uniquely per machine.
        /// </summary>
        /// <returns>Machine identifier (from 0 to MAX_MACHINE_ID)</returns>
        private static long GetMachineId()
        {
            // Simple machine ID generation based on environment
            // In production, this should be configured per machine
            var machineName = Environment.MachineName;
            var hash = machineName.GetHashCode();
            return Math.Abs(hash) % (MAX_MACHINE_ID + 1);
        }

        /// <summary>
        /// Generates the next unique Snowflake identifier.
        /// </summary>
        /// <returns>Unique 64-bit identifier</returns>
        public static long NextId()
        {
            lock (_lock)
            {
                var timestamp = GetCurrentTimestamp();
                
                if (timestamp < _lastTimestamp)
                {
                    throw new InvalidOperationException("Clock moved backwards!");
                }
                
                if (timestamp == _lastTimestamp)
                {
                    _sequence = (_sequence + 1) & MAX_SEQUENCE;
                    if (_sequence == 0)
                    {
                        // Wait for next millisecond
                        timestamp = WaitForNextMillisecond(_lastTimestamp);
                    }
                }
                else
                {
                    _sequence = 0;
                }
                
                _lastTimestamp = timestamp;
                
                return ((timestamp - EPOCH) << (MACHINE_ID_BITS + SEQUENCE_BITS)) |
                       (_machineId << SEQUENCE_BITS) |
                       _sequence;
            }
        }

        /// <summary>
        /// Gets the current time in milliseconds since the Unix epoch.
        /// </summary>
        /// <returns>Current time in milliseconds</returns>
        private static long GetCurrentTimestamp()
        {
            return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        }

        /// <summary>
        /// Waits for the next millisecond if the sequence overflows.
        /// </summary>
        /// <param name="lastTimestamp">The last used timestamp</param>
        /// <returns>The timestamp of the next millisecond</returns>
        private static long WaitForNextMillisecond(long lastTimestamp)
        {
            var timestamp = GetCurrentTimestamp();
            while (timestamp <= lastTimestamp)
            {
                timestamp = GetCurrentTimestamp();
            }
            return timestamp;
        }
    }
}

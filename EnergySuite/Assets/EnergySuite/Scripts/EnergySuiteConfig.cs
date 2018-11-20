using System.Collections.Generic;

namespace EnergySuite
{
	public static class EnergySuiteConfig
	{
		public static Dictionary<TimeValue, TimeBasedValue> StoredInfo = new Dictionary<TimeValue, TimeBasedValue>()
		{
			//Examples
			{ TimeValue.Key, new TimeBasedValue(TimeValue.Key, 0, 10, 20) },
			{ TimeValue.Life, new TimeBasedValue(TimeValue.Life, 5, 0, 5) }
		};

		public const string Password = "examplePass";

		public const string PasswordSalt = "exampleSalt";

		public const string AmountPrefixKey = "Amount_";

		public const string LastTimeAddedPrefixKey = "LastTimeAdded_";
	}
}

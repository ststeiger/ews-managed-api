
namespace System
{


    public sealed partial class TimeZoneInfo
    {
        private static void PopulateAllSystemTimeZones(CachedData cachedData)
        {
            // https://docs.microsoft.com/en-us/previous-versions/sql/sql-server-2005/ms171251(v=sql.90)
            // https://docs.microsoft.com/en-us/dotnet/standard/datetime/enumerate-time-zones
            throw new NotImplementedException();
        }


        private static TimeZoneInfoResult TryGetTimeZoneFromLocalMachine(string id, out TimeZoneInfo match, out Exception e)
        {
            match = null;
            e = null;

            throw new NotImplementedException();
            return new TimeZoneInfoResult();
        }

        public AdjustmentRule[] GetAdjustmentRules()
        {
            throw new NotImplementedException();
            return null;
        }

        private static TimeZoneInfo GetLocalTimeZone(CachedData cachedData)
        {
            throw new NotImplementedException();
            return null;
        }

        public static TimeZoneInfo FindSystemTimeZoneById(string id)
        {
            throw new NotImplementedException();
            return null;
        }


    }



    public static class DateTimeExtensions
    {
        public static bool IsAmbiguousDaylightSavingTime(this System.DateTime dte)
        {
            throw new NotImplementedException();
            return false;
        }

        public static void GetDatePart(this System.DateTime dte, out int timeOfDayYear, out int timeOfDayMonth, out int timeOfDayDay)
        {
            throw new NotImplementedException();
            timeOfDayYear = 0;
            timeOfDayMonth = 0;
            timeOfDayDay = 0;
        }


    }

    namespace Threading
    {
        public static class Monitor2
        {
            public static bool IsEntered(params object[] p)
            {
                return true;
            }
        }
    }



    namespace Runtime.Serialization
    {
        public static class SerializationInfoExtensions
        {
            public static object GetValueNoThrow(this SerializationInfo info, string name, System.Type type)
            {
                object o = null;
                try
                {
                    o = info.GetValue(name, type);
                }
                catch 
                { }

                return o;
            }
        }
    }



    public static class SR
    {

        public static string Argument_InvalidId = "Argument Invalid: Id";
        public static string ArgumentOutOfRange_UtcOffset = "ArgumentOutOfRange: UtcOffset";
        public static string Argument_TimeSpanHasSeconds = "Argument: TimeSpanHasSeconds";
        public static string Argument_AdjustmentRulesNoNulls = "Argument: AdjustmentRulesNoNulls";
        public static string ArgumentOutOfRange_UtcOffsetAndDaylightDelta = "ArgumentOutOfRange: UtcOffsetAndDaylightDelta";
        public static string Argument_AdjustmentRulesOutOfOrder = "Argument: AdjustmentRulesOutOfOrder";
        public static string Serialization_InvalidData = "Serialization: InvalidData";
        public static string Serialization_CorruptField = "Serialization: CorruptField";
        public static string Serialization_InvalidEscapeSequence = "Serialization: InvalidEscapeSequence";

        public static string Argument_DateTimeKindMustBeUnspecifiedOrUtc = "Argument: DateTimeKindMustBeUnspecifiedOrUtc";
        public static string Argument_TransitionTimesAreIdentical = "Argument: TransitionTimesAreIdentical";
        public static string Argument_DateTimeHasTimeOfDay = "Argument: DateTimeHasTimeOfDay";
        public static string Argument_OutOfOrderDateTimes = "Argument: OutOfOrderDateTimes";
        public static string Argument_InvalidSerializedString = "Argument: InvalidSerializedString";
        public static string Argument_DateTimeIsInvalid = "Argument: DateTimeIsInvalid";
        public static string Argument_ConvertMismatch = "Argument: ConvertMismatch";
        public static string Argument_DateTimeIsNotAmbiguous = "Argument: DateTimeIsNotAmbiguous";
        public static string Argument_DateTimeOffsetIsNotAmbiguous = "Argument: DateTimeOffsetIsNotAmbiguous";
        public static string Argument_DateTimeKindMustBeUnspecified = "Argument: DateTimeKindMustBeUnspecified";
        public static string ArgumentOutOfRange_MonthParam = "ArgumentOutOfRange: MonthParam";
        public static string ArgumentOutOfRange_DayParam = "ArgumentOutOfRange: DayParam";
        public static string ArgumentOutOfRange_Week = "ArgumentOutOfRange: Week";
        public static string ArgumentOutOfRange_DayOfWeek = "ArgumentOutOfRange: DayOfWeek";
        public static string Argument_DateTimeHasTicks = "Argument: DateTimeHasTicks";
        

        public static string Format(string format, params object[] parameters)
        {
            return string.Format(format, parameters);
        }


    }


}

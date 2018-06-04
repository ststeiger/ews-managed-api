
using System.Reflection;


namespace System
{


    internal class NativeMethods
    {

        /// <summary>
        /// The SystemTime structure represents a date and time using individual members 
        /// for the month, day, year, weekday, hour, minute, second, and millisecond.
        /// </summary>
        [Runtime.InteropServices.StructLayoutAttribute(Runtime.InteropServices.LayoutKind.Sequential)]
        public struct SystemTime
        {
            public short year;
            public short month;
            public short dayOfWeek;
            public short day;
            public short hour;
            public short minute;
            public short second;
            public short milliseconds;
        }

        /// <summary>
        /// The TimeZoneInformation structure specifies information specific to the time zone.
        /// </summary>
        [Runtime.InteropServices.StructLayout(Runtime.InteropServices.LayoutKind.Sequential, CharSet = Runtime.InteropServices.CharSet.Unicode)]
        public struct TimeZoneInformation
        {
            /// <summary>
            /// Current bias for local time translation on this computer, in minutes. The bias is the difference, in minutes, between Coordinated Universal Time (UTC) and local time. All translations between UTC and local time are based on the following formula: 
            /// <para>UTC = local time + bias</para>
            /// <para>This member is required.</para>
            /// </summary>
            public int bias;
            /// <summary>
            /// Pointer to a null-terminated string associated with standard time. For example, "EST" could indicate Eastern Standard Time. The string will be returned unchanged by the GetTimeZoneInformation function. This string can be empty.
            /// </summary>
            [Runtime.InteropServices.MarshalAs(Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 32)]
            public string standardName;
            /// <summary>
            /// A SystemTime structure that contains a date and local time when the transition from daylight saving time to standard time occurs on this operating system. If the time zone does not support daylight saving time or if the caller needs to disable daylight saving time, the wMonth member in the SystemTime structure must be zero. If this date is specified, the DaylightDate value in the TimeZoneInformation structure must also be specified. Otherwise, the system assumes the time zone data is invalid and no changes will be applied.
            /// <para>To select the correct day in the month, set the wYear member to zero, the wHour and wMinute members to the transition time, the wDayOfWeek member to the appropriate weekday, and the wDay member to indicate the occurence of the day of the week within the month (first through fifth).</para>
            /// <para>Using this notation, specify the 2:00a.m. on the first Sunday in April as follows: wHour = 2, wMonth = 4, wDayOfWeek = 0, wDay = 1. Specify 2:00a.m. on the last Thursday in October as follows: wHour = 2, wMonth = 10, wDayOfWeek = 4, wDay = 5.</para>
            /// </summary>
            public SystemTime standardDate;
            /// <summary>
            /// Bias value to be used during local time translations that occur during standard time. This member is ignored if a value for the StandardDate member is not supplied. 
            /// <para>This value is added to the value of the Bias member to form the bias used during standard time. In most time zones, the value of this member is zero.</para>
            /// </summary>
            public int standardBias;
            /// <summary>
            /// Pointer to a null-terminated string associated with daylight saving time. For example, "PDT" could indicate Pacific Daylight Time. The string will be returned unchanged by the GetTimeZoneInformation function. This string can be empty.
            /// </summary>
            [Runtime.InteropServices.MarshalAs(Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 32)]
            public string daylightName;
            /// <summary>
            /// A SystemTime structure that contains a date and local time when the transition from standard time to daylight saving time occurs on this operating system. If the time zone does not support daylight saving time or if the caller needs to disable daylight saving time, the wMonth member in the SystemTime structure must be zero. If this date is specified, the StandardDate value in the TimeZoneInformation structure must also be specified. Otherwise, the system assumes the time zone data is invalid and no changes will be applied.
            /// <para>To select the correct day in the month, set the wYear member to zero, the wHour and wMinute members to the transition time, the wDayOfWeek member to the appropriate weekday, and the wDay member to indicate the occurence of the day of the week within the month (first through fifth).</para>
            /// </summary>
            public SystemTime daylightDate;
            /// <summary>
            /// Bias value to be used during local time translations that occur during daylight saving time. This member is ignored if a value for the DaylightDate member is not supplied. 
            /// <para>This value is added to the value of the Bias member to form the bias used during daylight saving time. In most time zones, the value of this member is –60.</para>
            /// </summary>
            public int daylightBias;
        }



        [Runtime.InteropServices.DllImport("kernel32.dll")]
        public static extern uint GetTimeZoneInformation(out TimeZoneInformation lpTimeZoneInformation);

    }


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
            if (System.Environment.OSVersion.Platform == PlatformID.Unix)
            {
                // /etc/localtime
                // match with /usr/share/zoneinfo/ 
            }
            else
            {
                NativeMethods.TimeZoneInformation tzi;
                NativeMethods.GetTimeZoneInformation(out tzi);
            }
            
            throw new NotImplementedException();
            return null;
        }

        public static TimeZoneInfo FindSystemTimeZoneById(string id)
        {
            // https://stackoverflow.com/questions/3621540/convert-reg-binary-data-to-reg-tzi-format
            // HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Time Zones
            // /usr/share/zoneinfo/ 
            throw new NotImplementedException();
            return null;
        }
        
        
    }
    
    
    
    
    public static class DateTimeExtensions
    {
        private static System.Reflection.FieldInfo s_dateData;
        private static readonly int[] DaysToMonth365;
        private static readonly int[] DaysToMonth366;
        
     
        
        static DateTimeExtensions()
        {
            s_dateData = typeof(System.DateTime).GetField("dateData", BindingFlags.Instance | BindingFlags.NonPublic);
            
            DaysToMonth365 = new int[13]
            {
                0,
                31,
                59,
                90,
                120,
                151,
                181,
                212,
                243,
                273,
                304,
                334,
                365
            };
        
            DaysToMonth366 = new int[13]
            {
                0,
                31,
                60,
                91,
                121,
                152,
                182,
                213,
                244,
                274,
                305,
                335,
                366
            };
            
        }
        
        
        public static bool IsAmbiguousDaylightSavingTime(this System.DateTime dte)
        {
            const UInt64 flagsMask             = 0xC000000000000000;
            const UInt64 kindLocalAmbiguousDst = 0xC000000000000000;
            ulong dateData = (ulong) s_dateData.GetValue(dte);
            ulong  internalKind = (dateData & flagsMask);
            
            return (internalKind == kindLocalAmbiguousDst);
        }
        
        
        // Exactly the same as GetDatePart(int part), except computing all of
        // year/month/day rather than just one of them.  Used when all three
        // are needed rather than redoing the computations for each.
        internal static void GetDatePart(this System.DateTime dte, out int year, out int month, out int day)
        {
            ulong dateData = (ulong) s_dateData.GetValue(dte);
            long internalTicks =  (long) dateData & 4611686018427387903L;
            
            const long TicksPerDay = 864000000000; 
            const int DaysPer400Years = 146097;
            const int DaysPer100Years = 36524;
            const int DaysPer4Years = 1461;
            const int DaysPerYear = 365;
            
            Int64 ticks = internalTicks;
            // n = number of days since 1/1/0001
            int n = (int)(ticks / TicksPerDay);
            // y400 = number of whole 400-year periods since 1/1/0001
            int y400 = n / DaysPer400Years;
            // n = day number within 400-year period
            n -= y400 * DaysPer400Years;
            // y100 = number of whole 100-year periods within 400-year period
            int y100 = n / DaysPer100Years;
            // Last 100-year period has an extra day, so decrement result if 4
            if (y100 == 4) y100 = 3;
            // n = day number within 100-year period
            n -= y100 * DaysPer100Years;
            // y4 = number of whole 4-year periods within 100-year period
            int y4 = n / DaysPer4Years;
            // n = day number within 4-year period
            n -= y4 * DaysPer4Years;
            // y1 = number of whole years within 4-year period
            int y1 = n / DaysPerYear;
            // Last year has an extra day, so decrement result if 4
            if (y1 == 4) y1 = 3;
            // compute year
            year = y400 * 400 + y100 * 100 + y4 * 4 + y1 + 1;
            // n = day number within year
            n -= y1 * DaysPerYear;
            // dayOfYear = n + 1;
            // Leap year calculation looks different from IsLeapYear since y1, y4,
            // and y100 are relative to year 1, not year 0
            bool leapYear = y1 == 3 && (y4 != 24 || y100 == 3);
            int[] days = leapYear ? DaysToMonth366 : DaysToMonth365;
            // All months have less than 32 days, so n >> 5 is a good conservative
            // estimate for the month
            int m = (n >> 5) + 1;
            // m = 1-based month number
            while (n >= days[m]) m++;
            // compute month and day
            month = m;
            day = n - days[m - 1] + 1;
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

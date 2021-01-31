using System;
using System.Collections.Generic;

namespace UbiClub.Feedback.Tests.Helpers
{
    internal class ExpectedTestData
    {
        internal static List<Guid> GameSessionIds => new List<Guid>(new Guid[]
        {
            Guid.Parse("C587CA3D-41A0-4A73-BE15-1B1BB677E982"), 
            Guid.Parse("EC1CC1F5-8BE4-43CA-82BF-44D27C9F3430"), 
            Guid.Parse("B1784629-7273-4AE8-AA1B-B3A6CBE47646")
        });
    }
}
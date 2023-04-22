using System.Collections;
using UnityEngine;

namespace ShipMaker.Data
{
    public enum CrewPosition
    {
        None = -1, // 0xFFFFFFFF
        Engineer = 0,
        Pilot = 1,
        Navigator = 2,
        Supervisor = 3,
        Gunner = 4,
        Instructor = 5,
        Tactician = 6,
        Steward = 7,
        Adviser = 8,
        RelationsOfficer = 9,
        other1 = 10, // 0x0000000A
        other2 = 11, // 0x0000000B
        other3 = 12, // 0x0000000C
        other4 = 13, // 0x0000000D
        other5 = 14, // 0x0000000E
        Co_Pilot = 15, // 0x0000000F
        FirstOfficer = 16, // 0x00000010
        Primary = 17, // 0x00000011
        Staff = 18, // 0x00000012
        Captain = 19, // 0x00000013
    }
}
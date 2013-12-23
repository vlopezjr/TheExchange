using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheExchange.Domain.Entities
{
    public enum AppointmentStatus
    {
        PreConfirmed = 1,
        Confirmed = 2,
        PickedUp = 3,
        DroppedOff = 4,
        Verified = 5,
        Cancelled = 6,
        NoShow = 7
    }

    public enum VenueServices
    {
        GuestList = 1,
        BottleService = 2,
        VIP = 3,
        Cabana = 4,
        Daybed = 5
    }

    public enum VenueTypes
    {
        UltraLounges = 1,
        NightClub = 2,
        GentlemenClubs21 = 3,
        Afterhours = 4,
        PoolParty = 5,
        LimoService = 6,
        Hotels = 7,
        GentlemenClubs18 = 8,
        Dayclubs = 9
    }
}

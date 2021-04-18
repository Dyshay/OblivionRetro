using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace AivyData.Enums
{
    public enum AccountState
    {
        DISCONNECTED = 0,
        CONNECTING = 1,
        IDLE = 2,
        WALKING = 3,
        GATHERING = 4,
        FIGHTING = 5,
        DIALOGING = 6
    }
}

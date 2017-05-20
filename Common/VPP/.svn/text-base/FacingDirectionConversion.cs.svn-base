using System;
using System.Collections.Generic;
using System.Text;

using VideoPaintballCommon.MapObjects;

namespace VideoPaintballCommon.VPP
{
    public static class FacingDirectionConversion
    {
        public static string ToString(FacingDirectionType facingDirection)
        {
            string direction = string.Empty;
         
            switch (facingDirection)
            {
                case FacingDirectionType.North:
                    direction = "North";
                    break;

                case FacingDirectionType.South:
                    direction = "South";
                    break;

                case FacingDirectionType.East:
                    direction = "East";
                    break;

                case FacingDirectionType.West:
                    direction = "West";
                    break;

            }

            return direction;
        }

        public static FacingDirectionType FromString(string facingDirection)
        {
            FacingDirectionType direction = FacingDirectionType.North;

            if (facingDirection == "West")
            {
                direction = FacingDirectionType.West;
            }
            else if (facingDirection == "East")
            {
                direction = FacingDirectionType.East;
            }
            else if (facingDirection == "South")
            {
                direction = FacingDirectionType.South;
            }

            return direction;
        }
    }
}

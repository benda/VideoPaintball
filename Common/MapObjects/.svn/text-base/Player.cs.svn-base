using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

using VideoPaintballCommon.Util;

namespace VideoPaintballCommon.MapObjects
{
    public enum ShieldLocationType
    {
        Front = 1,
        Back,
        Left,
        Right,
    }

    public enum FacingDirectionType
    {
        North = 1,
        East,
        South,
        West,
    }


    public class Player : MapObject, IRenderable, IMovable
    {
        private Color _teamColor;
        private UInt16 _health;
        private UInt16 _ammo;
        protected UInt16 _maxHealth = 100;
        protected UInt16 _maxAmmo = 100;
        private ShieldLocationType _shieldLocation;
        private FacingDirectionType _facingDirection;
        private string _id;
        private PointF _velocity;
        private bool _moveWasUndoed = false;
        private PointF _oldLocation;
        private ushort _teamNumber;

        public Player(ushort teamNumber, string id)
        {
            this.TeamNumber = teamNumber;
            this.TeamColor = DetermineTeamColor();
            this.ShieldLocation = ShieldLocationType.Front;
            this.Size = new SizeF(DimensionsUtil.GetAvatarWidth(), DimensionsUtil.GetAvatarHeight());
            this.ID = id;
            this.Health = _maxHealth;
            this.Ammo = _maxAmmo;
            this.FacingDirection = FacingDirectionType.North;
        }

        private Color DetermineTeamColor()
        {
            Color color = Color.Green; ;

            if (TeamNumber == 1)
            {
                color = Color.White;
            }
            else if(TeamNumber == 2)
            {
                color = Color.Red;
            }
            else if (TeamNumber == 3)
            {
                color = Color.Green;
            }
            else if (TeamNumber == 4)
            {
                color = Color.HotPink;
            }
            else if (TeamNumber == 5)
            {
                color = Color.Yellow;
            }
            else if (TeamNumber == 6)
            {
                color = Color.Gray;
            }
            return color;
        }

        /// <summary>
        /// issue [A.1.5] of the design document
        /// </summary>
        /// <param name="graphics"></param>
        public void Render(Graphics graphics)
        {
            Pen pen = new Pen(TeamColor);

            int healthMeterX = (int)Location.X + (int)(DimensionsUtil.GetAvatarWidth() - 20);
            int healthMeterY = (int)Location.Y + 20;
            float healthMeterBarHeight = ((float)Health / (float)_maxHealth) * DimensionsUtil.GetHealthMeterHeight();

            int ammoMeterX = (int)Location.X + 10;
            int ammoMeterY = (int)Location.Y + 20;
            float ammoMeterBarHeight = ((float)Ammo / (float)_maxAmmo) * DimensionsUtil.GetAmmoMeterHeight();
            Color healthMeterColor = MeterUtil.GetColorFromValue(Health);
            SolidBrush healthMeterBrush = new SolidBrush(healthMeterColor);

            Color ammoMeterColor = MeterUtil.GetColorFromValue(Ammo);
            SolidBrush ammoMeterBrush = new SolidBrush(ammoMeterColor);

            GraphicsPath graphicsPath = new GraphicsPath();
            RectangleF avatarMainBody = new RectangleF(Location, Size);
            RectangleF avatarGun = new RectangleF(Location.X + ((Size.Width / 2) - (DimensionsUtil.GetGunWidth() / 2)), Location.Y - DimensionsUtil.GetGunHeight(), DimensionsUtil.GetGunWidth(), DimensionsUtil.GetGunHeight());
            RectangleF healthMeterOutline = new RectangleF(healthMeterX, healthMeterY, DimensionsUtil.GetHealthMeterWidth(), DimensionsUtil.GetHealthMeterHeight());
            RectangleF ammoMeterOutline = new RectangleF(ammoMeterX, ammoMeterY, DimensionsUtil.GetAmmoMeterWidth(), DimensionsUtil.GetAmmoMeterHeight());

            graphicsPath.AddRectangle(avatarMainBody);
            graphicsPath.AddRectangle(avatarGun);        
            graphicsPath.AddRectangle(healthMeterOutline);
            graphicsPath.AddRectangle(ammoMeterOutline);
            
            RectangleF healthMeterBar = new RectangleF(healthMeterX, healthMeterY + (DimensionsUtil.GetHealthMeterHeight() - healthMeterBarHeight), DimensionsUtil.GetHealthMeterWidth(), healthMeterBarHeight );
            RectangleF ammoMeterBar = new RectangleF(ammoMeterX, ammoMeterY + DimensionsUtil.GetAmmoMeterHeight() - ammoMeterBarHeight, DimensionsUtil.GetAmmoMeterWidth(), ammoMeterBarHeight);

            GraphicsPath healthMeterBarPath = new GraphicsPath();
            healthMeterBarPath.AddRectangle(healthMeterBar);

            GraphicsPath ammoMeterBarPath = new GraphicsPath();
            ammoMeterBarPath.AddRectangle(ammoMeterBar);

            Matrix rotationMatrix = new Matrix(1, 0, 0, 1, 0, 0); 
            PointF rotationPoint = new PointF(avatarMainBody.Location.X + (DimensionsUtil.GetAvatarWidth() /2), avatarMainBody.Location.Y + (DimensionsUtil.GetAvatarHeight() /2)); 

            switch(FacingDirection)
            {
                case FacingDirectionType.North:
                    break;

                case FacingDirectionType.East:
                    rotationMatrix.RotateAt(90F, rotationPoint);
                    break;

                case FacingDirectionType.South:
                    rotationMatrix.RotateAt(180F, rotationPoint);
                    break;

                case FacingDirectionType.West:
                    rotationMatrix.RotateAt(270F, rotationPoint);
                    break;
            }

            graphicsPath.Transform(rotationMatrix);
            ammoMeterBarPath.Transform(rotationMatrix);
            healthMeterBarPath.Transform(rotationMatrix);

            graphics.FillPath(healthMeterBrush, healthMeterBarPath);
            graphics.FillPath(ammoMeterBrush, ammoMeterBarPath);

            graphics.DrawPath(pen, graphicsPath);
     
            //render the shield, ignoring any rotations (rotations are ignored because it is easier for collision detection with the shield)
            switch (ShieldLocation)
            {
                case ShieldLocationType.Front:
                   graphics.DrawLine(pen, Location.X, Location.Y - DimensionsUtil.GetAvatarShieldOffset(), Location.X + Size.Width, Location.Y - DimensionsUtil.GetAvatarShieldOffset());
                    break;

                case ShieldLocationType.Back:
                    graphics.DrawLine(pen, Location.X, Location.Y + DimensionsUtil.GetAvatarShieldOffset() + Size.Height, Location.X + Size.Width, Location.Y + DimensionsUtil.GetAvatarShieldOffset() + Size.Height);
                    break;

                case ShieldLocationType.Left:
                    graphics.DrawLine(pen, Location.X - DimensionsUtil.GetAvatarShieldOffset(), Location.Y, Location.X - DimensionsUtil.GetAvatarShieldOffset(), Location.Y + Size.Height);
                    break;

                case ShieldLocationType.Right:
                    graphics.DrawLine(pen, Location.X + Size.Width + DimensionsUtil.GetAvatarShieldOffset(), Location.Y, Location.X + Size.Width + DimensionsUtil.GetAvatarShieldOffset(), Location.Y + Size.Height);
                    break;
            }
        }    

        public void MoveShieldToFront()
        {
            ShieldLocation = ShieldLocationType.Front;
        }

        public void MoveShieldToBack()
        {
            ShieldLocation = ShieldLocationType.Back;
        }

        public void MoveShieldToLeft()
        {
            ShieldLocation = ShieldLocationType.Left;
        }

        public void MoveShieldToRight()
        {
            ShieldLocation = ShieldLocationType.Right;
        }

        public void RotateLeft()
        {
            switch (FacingDirection)
            {
                case FacingDirectionType.North:
                    FacingDirection = FacingDirectionType.West;
                    break;

                case FacingDirectionType.East:
                    FacingDirection = FacingDirectionType.North;
                    break;

                case FacingDirectionType.South:
                    FacingDirection = FacingDirectionType.East;
                    break;

                case FacingDirectionType.West:
                   FacingDirection = FacingDirectionType.South; 
                  break;
            }
        }

        public void RotateRight()
        {
         switch (FacingDirection)
            {
             case FacingDirectionType.North:
                    FacingDirection = FacingDirectionType.East;
                    break;

                case FacingDirectionType.East:
                    FacingDirection = FacingDirectionType.South;
                    break;

                case FacingDirectionType.South:
                    FacingDirection = FacingDirectionType.West;
                    break;

                case FacingDirectionType.West:
                   FacingDirection = FacingDirectionType.North; 
                  break;
            }
        }

        public override string ToString()
        {
            return "<p," + ID + "," + Location.X.ToString() + "," + Location.Y.ToString() + "," + FacingDirection.ToString() + "," + ShieldLocation.ToString() + "," + Health.ToString() + "," + Ammo.ToString() + "," + TeamNumber.ToString() + ">";
        }

        public void RecordPaintballHit()
        {
            if (Health > 5)
            {
                this.Health -= 5;
            }
            else
            {
                Health = 0;
            }
        }

        public void RecordPaintballFired()
        {
            if (Ammo > 1)
            {
                this.Ammo -= 1;
            }
            else
            {
                Ammo = 0;
            }
        }

        public void Move()
        {
            this._oldLocation = this.Location;
            this.Location = new PointF(this.Location.X + this.Velocity.X, this.Location.Y + this.Velocity.Y);
        }

        public void UndoMove()
        {
            if (!_moveWasUndoed)
            {
                this.Location = this._oldLocation;
            }
        }

        public PointF Velocity
        {
            get { return _velocity; }
            set { _velocity = value; }
        }

        public ShieldLocationType ShieldLocation
        {
            get { return _shieldLocation; }
            set { _shieldLocation = value; }
        }

        public FacingDirectionType FacingDirection
        {
            get { return _facingDirection; }
            set { _facingDirection = value; }
        }


        public Color TeamColor
        {
            get { return _teamColor; }
            set { _teamColor = value; }
        }

        public UInt16 Health
        {
            get { return _health; }
            set { _health = value; }
        }

        public UInt16 Ammo
        {
            get { return _ammo; }
            set { _ammo = value; }
        }

        public string ID
        {
            get { return _id; }
            set { _id = value; }
        }

        public ushort TeamNumber
        {
            get { return _teamNumber; }
            set { _teamNumber = value; }
        }
    }

}

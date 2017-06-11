using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Net.Sockets;

using VideoPaintballClient.Net;
using VideoPaintballCommon.Util;
using VideoPaintballCommon.MapObjects;
using VideoPaintballCommon.VPP;
using VideoPaintballCommon;
using VideoPaintballClient.Util;
using System.Threading;
using VideoPaintballCommon.Net;
using System.Diagnostics;

namespace VideoPaintballClient
{
    public class GameScreen : System.Windows.Forms.Form
    {
        private System.ComponentModel.IContainer components = null;
       private Game _game;

        public GameScreen(Game game)
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true);             
            this.Size = new Size((int)DimensionsUtil.GetMapWidth(), (int)DimensionsUtil.GetMapHeight());
            _game = game;
            _game.InvalidateNeeded += _game_InvalidateNeeded;

            Application.Idle += Application_Idle;
        }

        private void Application_Idle(object sender, EventArgs e)
        {
            NativeMethods.Message message;

            while (!NativeMethods.PeekMessage(out message, IntPtr.Zero, 0, 0, 0))
            {
                _game.Tick(sender, e);
            }
        }

        private void _game_InvalidateNeeded(object sender, EventArgs e)
        {
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            this.Text = string.Format("FPS: {0} TPS: {1}", FrameUtil.CalculateRatePerSecond(), _game.TurnsPerSecond);

            _game.Render(e.Graphics);
        }

        //issue [B.1.5] of the design document
        private void GameEngine_KeyDown(object sender, KeyEventArgs e)
        {
            string message = string.Empty;            
            switch (e.KeyCode)
            {
                case Keys.W:
                    _game.PlayerAction = MessageConstants.PlayerActionShieldFront;
                    break;

                case Keys.A:
                    _game.PlayerAction = MessageConstants.PlayerActionShieldLeft;
                    break;

                case Keys.S:
                    _game.PlayerAction = MessageConstants.PlayerActionShieldBack;
                    break;

                case Keys.D:
                    _game.PlayerAction = MessageConstants.PlayerActionShieldRight;
                    break;

                case Keys.Up:
                    _game.PlayerAction = MessageConstants.PlayerActionUp;
                    break;

                case Keys.Down:
                    _game.PlayerAction = MessageConstants.PlayerActionDown;
                    break;
                    
                case Keys.Left:
                    if (e.Shift) //issue [B.1.6] of the design document
                    {
                        _game.PlayerAction = MessageConstants.PlayerActionRotateLeft;
                    }
                    else
                    {
                        _game.PlayerAction = MessageConstants.PlayerActionLeft;
                    }
                    break;

                case Keys.Right:
                    if (e.Shift) //issue [B.1.6] of the design document
                    {
                        _game.PlayerAction = MessageConstants.PlayerActionRotateRight;
                    }
                    else
                    {
                        _game.PlayerAction = MessageConstants.PlayerActionRight;
                    }
                    break;

                case Keys.Space:
                    _game.PlayerAction = MessageConstants.PlayerActionShoot;
                    break;
            }          
        }

        #region Windows Form Designer generated code
        
        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            // 
            // GameEngine
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(292, 266);
            this.Name = "GameEngine";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "GameEngine";
            this.KeyDown += new KeyEventHandler(GameEngine_KeyDown);
        }

     
        #endregion
    }
}

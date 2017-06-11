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
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel statusLabel;
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
            statusLabel.Text = string.Format("FPS: {0} TPS: {1}", FrameUtil.CalculateRatePerSecond(), _game.TurnsPerSecond);

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
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 244);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(292, 22);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // statusLabel
            // 
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(246, 17);
            this.statusLabel.Spring = true;
            this.statusLabel.Text = "toolStripStatusLabel1";
            // 
            // GameScreen
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(292, 266);
            this.Controls.Add(this.statusStrip1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "GameScreen";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Diablo IV";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.GameEngine_KeyDown);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

     
        #endregion
    }
}

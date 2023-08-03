using Cosmos.System.Graphics.Fonts;
using Cosmos.System.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Sys = Cosmos.System;
using System.Drawing;

namespace FalconOS
{
    public struct App
    {
        public int x = 0;
        public int y = 0;
        public int w;
        public int h;
        public bool hC;
        public bool hTP;
        public string t;
        public bool modified = false;

        public App(int x, int y, int w, int h, bool hC, bool hTP, string t)
        {
            if (!modified)
            {
                this.x = x;
                this.y = y;
            }
            this.w = w;
            this.h = h;
            this.hTP = hTP;
            this.t = t;
            this.hC = hC;
        }

        public void changeXY(int cx, int cy) { this.x = cx; this.y = cy; this.modified = true; }
        public string getT() { return this.t; }
    }

    public class yindowmgr
    {
        public List<App> apps;
        public string focussedApp;
        public bool moving = false;

        public void Init()
        {
            apps = new List<App>();
        }

        public void drawWindow(string title, bool hasTopBar, bool hasControls, int x, int y, int width, int height)
        {
            foreach (var app in apps) {
                if (app.getT() == title)
                {
                    return;
                }
            }
            apps.Add(new App(x, y, width, height, hasControls, hasTopBar, title));
            return;
        }

        public void updateWindow()
        {
            var tempColor = data.deepAccentColor;
            var index = 0;
            foreach (var win in apps)
            {
                if (data.pressed && Sys.MouseManager.X >= win.x && Sys.MouseManager.X <= win.x + win.w && Sys.MouseManager.Y >= win.y && Sys.MouseManager.Y <= win.y + 15)
                {
                    tempColor = Color.DarkGoldenrod;
                    win.changeXY((int)Sys.MouseManager.X, (int)Sys.MouseManager.Y);
                    moving = true;
                }
                data.canvas.DrawFilledRectangle(Color.White, win.x, win.y, win.w, win.h);
                if (win.hTP)
                {
                    data.canvas.DrawFilledRectangle(tempColor, win.x, win.y, win.w, 15);
                    data.canvas.DrawString(win.t, PCScreenFont.Default, Color.White, win.x + 2, win.y + 1);
                }
                tempColor = data.deepAccentColor;
                index++;
            }
            //focussedApp = apps[0];
        }

    }
}

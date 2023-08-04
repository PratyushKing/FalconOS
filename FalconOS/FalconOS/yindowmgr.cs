using Cosmos.System.Graphics.Fonts;
using System;
using System.Collections.Generic;
using System.Drawing;
using Sys = Cosmos.System;

namespace FalconOS
{
    public class App
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

        public void updateWin(bool focussed)
        {
            var tempColor = data.deepAccentColor;
            if (!focussed)
            {
                tempColor = Color.DarkGray;
            }
            if (data.pressed && Sys.MouseManager.X >= x && Sys.MouseManager.X <= x + w && Sys.MouseManager.Y >= y && Sys.MouseManager.Y <= y + 15)
            {
                tempColor = Color.DarkGoldenrod;
                changeXY((int)Sys.MouseManager.X, (int)Sys.MouseManager.Y);
            }
            data.canvas.DrawFilledRectangle(Color.White, x, y, w, h);
            data.canvas.DrawRectangle(tempColor, x - 1, y - 1, w + 1, h + 1);
            data.canvas.DrawRectangle(tempColor, x - 2, y - 2, w + 2, h + 2);
            if (hTP)
            {
                data.canvas.DrawFilledRectangle(tempColor, x, y, w, 15);
                data.canvas.DrawString(t, PCScreenFont.Default, Color.White, x + 2, y + 1);
                if (hC)
                {
                    data.canvas.DrawFilledRectangle(Color.IndianRed, (x + w) - 20, y, 20, 15);
                    data.canvas.DrawChar('X', PCScreenFont.Default, Color.White, x + w - 15, y + 1);
                }
            }
            tempColor = data.deepAccentColor;
        }
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
            foreach (var app in apps)
            {
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
            
            for (var i = 0; i < apps.Count; i++)
            {
                if (focussedApp == apps[i].getT())
                {
                    apps[i].updateWin(true);
                } else
                {
                    apps[i].updateWin(false);
                }
            }
        }

    }
}

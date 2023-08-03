using Cosmos.System.Graphics.Fonts;
using System.Collections.Generic;
using System.Drawing;
using Sys = Cosmos.System;

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
            var tempColor = data.deepAccentColor;
            for (var i = 0; i < apps.Count; i++)
            {
                if (data.pressed && Sys.MouseManager.X >= apps[i].x && Sys.MouseManager.X <= apps[i].x + apps[i].w && Sys.MouseManager.Y >= apps[i].y && Sys.MouseManager.Y <= apps[i].y + 15)
                {
                    tempColor = Color.DarkGoldenrod;
                    apps[i].changeXY((int)Sys.MouseManager.X, (int)Sys.MouseManager.Y);
                    moving = true;
                }
                data.canvas.DrawFilledRectangle(Color.White, apps[i].x, apps[i].y, apps[i].w, apps[i].h);
                if (apps[i].hTP)
                {
                    data.canvas.DrawFilledRectangle(tempColor, apps[i].x, apps[i].y, apps[i].w, 15);
                    data.canvas.DrawString(apps[i].t, PCScreenFont.Default, Color.White, apps[i].x + 2, apps[i].y + 1);
                }
                tempColor = data.deepAccentColor;
                i++;
            }
            //focussedApp = apps[0];
        }

    }
}

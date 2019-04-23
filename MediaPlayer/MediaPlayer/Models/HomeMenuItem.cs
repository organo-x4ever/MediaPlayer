using System;
using System.Collections.Generic;
using System.Text;

namespace MediaPlayer.Models
{
    public enum MenuItemType
    {
        Browse=1,
        About=2,
        AudioPlayer=3
    }
    public class HomeMenuItem
    {
        public MenuItemType Id { get; set; }

        public string Title { get; set; }
    }
}

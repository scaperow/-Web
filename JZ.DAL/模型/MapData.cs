using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace JZ.BLL.模型
{
    public class MapData
    {
        private string mapCenterX;

        public string MapCenterX
        {
            get { return mapCenterX; }
            set { mapCenterX = value; }
        }
        private string mapCenterY;

        public string MapCenterY
        {
            get { return mapCenterY; }
            set { mapCenterY = value; }
        }
        private DataTable lineCoordinate;

        public DataTable LineCoordinate
        {
            get { return lineCoordinate; }
            set { lineCoordinate = value; }
        }

        private string tagX;

        public string TagX
        {
            get { return tagX; }
            set { tagX = value; }
        }

        private string tagY;

        public string TagY
        {
            get { return tagY; }
            set { tagY = value; }
        }

        private string zoomLevel;

        public string ZoomLevel
        {
            get { return zoomLevel; }
            set { zoomLevel = value; }
        }

        private string titleImg;

        public string TitleImg
        {
            get { return titleImg; }
            set { titleImg = value; }
        }
    }
}
